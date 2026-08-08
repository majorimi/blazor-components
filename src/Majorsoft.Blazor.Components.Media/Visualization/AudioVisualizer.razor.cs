using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Majorsoft.Blazor.Components.Media.Capture;
using Majorsoft.Blazor.Components.Media.Players;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Components.Media.Visualization
{
	/// <summary>
	/// Blazor component that renders a live audio visualization (FFT frequency spectrum bars or
	/// time domain waveform) onto an HTML canvas with the Web Audio <c>AnalyserNode</c> API.
	/// Attach it to an <see cref="AudioRecorder"/>/<see cref="CameraCapture"/> via <see cref="CaptureSource"/>
	/// or to an <see cref="AudioPlayer"/>/<see cref="VideoPlayer"/> via <see cref="PlayerSource"/>.
	/// Drawing runs entirely in JS (requestAnimationFrame) without .NET interop; reduced FFT band data
	/// can additionally be reported to .NET via <see cref="OnSpectrumChanged"/> on a throttled timer.
	/// Note: cross-origin player sources produce no data unless served with CORS headers, Blob URLs
	/// of recordings (<see cref="MediaRecordingInfo.Url"/>) and .NET stream sources always work.
	/// </summary>
	public partial class AudioVisualizer : ComponentBase, IAsyncDisposable
	{
		private Task<IJSObjectReference>? _moduleTask;
		private DotNetObjectReference<AudioVisualizer>? _dotNetRef;
		private ElementReference _canvasRef;
		private int _visualizerId;
		private MediaCaptureBase? _attachedCapture;
		private MediaPlayerBase? _attachedPlayer;
		private bool _bothSourcesReported;

		[Inject] private IJSRuntime _jsRuntime { get; set; } = default!;
		[Inject] private ILogger<AudioVisualizer> _logger { get; set; } = default!;

		/// <summary>Exposes the Blazor <see cref="ElementReference"/> of the rendered canvas element.</summary>
		public ElementReference InnerElementReference => _canvasRef;

		/// <summary>True while the visualizer is running.</summary>
		public bool IsActive => _visualizerId != 0;

		/// <summary>Capture component (microphone/camera stream) to visualize. Set either this or <see cref="PlayerSource"/>.</summary>
		[Parameter] public MediaCaptureBase? CaptureSource { get; set; }

		/// <summary>Player component (audio/video element) to visualize. Set either this or <see cref="CaptureSource"/>.</summary>
		[Parameter] public MediaPlayerBase? PlayerSource { get; set; }

		/// <summary>Rendering mode: FFT frequency bars or waveform. Default is <see cref="AudioVisualizerModes.FrequencyBars"/>.</summary>
		[Parameter] public AudioVisualizerModes Mode { get; set; } = AudioVisualizerModes.FrequencyBars;

		/// <summary>Width of the canvas in px or % (see <see cref="IsVisualizerDimensionInPixels"/>). Default is 300.</summary>
		[Parameter] public int Width { get; set; } = 300;

		/// <summary>Height of the canvas in px or % (see <see cref="IsVisualizerDimensionInPixels"/>). Default is 100.</summary>
		[Parameter] public int Height { get; set; } = 100;

		/// <summary>
		/// Gets or sets True indicating whether the visualizer dimensions are specified in pixels, False means %. Default is true.
		/// In % mode <see cref="Width"/>/<see cref="Height"/> are applied as CSS size relative to the parent element,
		/// the canvas drawing resolution follows the CSS size automatically.
		/// </summary>
		[Parameter] public bool IsVisualizerDimensionInPixels { get; set; } = true;

		/// <summary>
		/// FFT window size, must be a power of 2 between 32 and 32768. The spectrum has <c>FftSize / 2</c>
		/// frequency bins covering 0 to sampleRate/2 Hz. Default is 2048. Applied when the visualizer starts.
		/// </summary>
		[Parameter] public int FftSize { get; set; } = 2048;

		/// <summary>Time smoothing of the FFT data between 0 (no smoothing) and 1. Default is 0.8.</summary>
		[Parameter] public double SmoothingTimeConstant { get; set; } = 0.8;

		/// <summary>Lower bound of the FFT magnitude scale in dB (rendered as zero). Default is -90.</summary>
		[Parameter] public int MinDecibels { get; set; } = -90;

		/// <summary>Upper bound of the FFT magnitude scale in dB (rendered as full height). Default is -10.</summary>
		[Parameter] public int MaxDecibels { get; set; } = -10;

		/// <summary>Number of bars the FFT bins are grouped into in <see cref="AudioVisualizerModes.FrequencyBars"/> mode. Default is 32.</summary>
		[Parameter] public int BarCount { get; set; } = 32;

		/// <summary>
		/// True (default) to group the FFT bins on a logarithmic frequency scale which distributes
		/// voice/music much better across the bars, false for a linear scale.
		/// </summary>
		[Parameter] public bool LogarithmicScale { get; set; } = true;

		/// <summary>Color of the bars/waveform line, any CSS color. Default is "#2196f3".</summary>
		[Parameter] public string BarColor { get; set; } = "#2196f3";

		/// <summary>When set, the bars are filled with a vertical gradient from <see cref="BarColor"/> (bottom) to this color (top).</summary>
		[Parameter] public string? BarGradientColor { get; set; }

		/// <summary>Background color of the canvas, any CSS color. Null (default) leaves the canvas transparent.</summary>
		[Parameter] public string? BackgroundColor { get; set; }

		/// <summary>True (default) to start the visualization automatically when the attached source becomes active.</summary>
		[Parameter] public bool AutoStart { get; set; } = true;

		/// <summary>
		/// Callback fired periodically with the FFT spectrum reduced to <see cref="SpectrumBandCount"/> bands (values 0..1)
		/// for custom .NET side processing/rendering. The timer only runs when this callback is set to avoid interop flooding;
		/// the canvas rendering itself never calls .NET.
		/// </summary>
		[Parameter] public EventCallback<double[]> OnSpectrumChanged { get; set; }

		/// <summary>Reporting interval of <see cref="OnSpectrumChanged"/> in milliseconds. Default is 100.</summary>
		[Parameter] public int SpectrumIntervalMs { get; set; } = 100;

		/// <summary>Number of bands reported via <see cref="OnSpectrumChanged"/>. Default is 16.</summary>
		[Parameter] public int SpectrumBandCount { get; set; } = 16;

		/// <summary>Callback fired when starting the visualizer failed, with the error message.</summary>
		[Parameter] public EventCallback<string> OnVisualizerError { get; set; }

		/// <summary>Custom CSS class(es) applied to the canvas element.</summary>
		[Parameter] public string? Class { get; set; }

		/// <summary>Custom inline style applied to the canvas element.</summary>
		[Parameter] public string? Style { get; set; }

		/// <summary>Arbitrary HTML attributes applied to the canvas element.</summary>
		[Parameter(CaptureUnmatchedValues = true)]
		public Dictionary<string, object>? AdditionalAttributes { get; set; }

		//HTML canvas width/height attributes are the drawing resolution in px, only rendered in pixel mode.
		//In % mode the JS side sizes the drawing resolution to the CSS layout size.
		private string? WidthAttribute => IsVisualizerDimensionInPixels ? Width.ToString() : null;
		private string? HeightAttribute => IsVisualizerDimensionInPixels ? Height.ToString() : null;

		//In % mode the dimensions are applied as CSS size in front of the user provided Style.
		private string? VisualizerStyle => IsVisualizerDimensionInPixels
			? Style
			: $"width: {Width}%; height: {Height}%; {Style}";

		/// <summary>
		/// Attaches to the source component lifecycle and starts when the source is already active.
		/// Evaluated after every render (not only the first one) because <c>@ref</c> captured sources
		/// are typically only assigned when the parent component renders the next time.
		/// </summary>
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (CaptureSource is not null && PlayerSource is not null)
			{
				if (!_bothSourcesReported)
				{
					_bothSourcesReported = true;
					await ReportErrorAsync("Set either CaptureSource or PlayerSource, not both.");
				}
				return;
			}
			_bothSourcesReported = false;

			if (CaptureSource == _attachedCapture && PlayerSource == _attachedPlayer)
			{
				return;
			}

			//Source changed: release the visualizer of the previous source first.
			if (_attachedCapture is not null)
			{
				_attachedCapture.CaptureStateChanged -= CaptureStateChangedAsync;
			}
			if (_attachedPlayer is not null)
			{
				_attachedPlayer.PlayerInitialized -= PlayerInitializedAsync;
			}
			await StopAsync();

			_attachedCapture = CaptureSource;
			_attachedPlayer = PlayerSource;

			if (_attachedCapture is not null)
			{
				_attachedCapture.CaptureStateChanged += CaptureStateChangedAsync;
				if (AutoStart && _attachedCapture.IsCaptureActive)
				{
					await StartAsync();
				}
			}
			else if (_attachedPlayer is not null)
			{
				_attachedPlayer.PlayerInitialized += PlayerInitializedAsync;
				if (AutoStart && _attachedPlayer.PlayerId != 0)
				{
					await StartAsync();
				}
			}
		}

		/// <summary>
		/// Starts the visualization. The attached source must be active (stream opened / player rendered),
		/// with <see cref="AutoStart"/> this is handled automatically. Returns true on success,
		/// errors are reported via <see cref="OnVisualizerError"/>.
		/// </summary>
		public async Task<bool> StartAsync()
		{
			if (IsActive)
			{
				return true;
			}

			string ownerKind;
			int ownerId;
			if (_attachedCapture is not null)
			{
				ownerKind = "capture";
				ownerId = _attachedCapture.CaptureId;
				if (ownerId == 0)
				{
					await ReportErrorAsync("Visualizer could not be started: the capture stream is not open.");
					return false;
				}
			}
			else if (_attachedPlayer is not null)
			{
				ownerKind = "player";
				ownerId = _attachedPlayer.PlayerId;
				if (ownerId == 0)
				{
					await ReportErrorAsync("Visualizer could not be started: the player is not initialized.");
					return false;
				}
			}
			else
			{
				await ReportErrorAsync("Visualizer could not be started: no CaptureSource or PlayerSource was set.");
				return false;
			}

			if (FftSize < 32 || FftSize > 32768 || (FftSize & (FftSize - 1)) != 0)
			{
				await ReportErrorAsync($"Visualizer could not be started: FftSize {FftSize} is not a power of 2 between 32 and 32768.");
				return false;
			}
			if (MinDecibels >= MaxDecibels)
			{
				await ReportErrorAsync($"Visualizer could not be started: MinDecibels ({MinDecibels}) must be less than MaxDecibels ({MaxDecibels}).");
				return false;
			}

			var module = await GetModuleAsync();
			_dotNetRef ??= DotNetObjectReference.Create(this);

			var result = await module.InvokeAsync<VisualizerStartResult>("startVisualizer", ownerKind, ownerId, _canvasRef, _dotNetRef, new
			{
				mode = Mode == AudioVisualizerModes.Waveform ? "waveform" : "bars",
				fftSize = FftSize,
				smoothingTimeConstant = SmoothingTimeConstant,
				minDecibels = MinDecibels,
				maxDecibels = MaxDecibels,
				barCount = Math.Max(1, BarCount),
				logScale = LogarithmicScale,
				barColor = BarColor,
				barGradientColor = BarGradientColor,
				backgroundColor = BackgroundColor,
				reportIntervalMs = OnSpectrumChanged.HasDelegate ? SpectrumIntervalMs : 0,
				reportBandCount = SpectrumBandCount
			});

			if (result.Id == 0)
			{
				await ReportErrorAsync($"Visualizer could not be started: {result.Error}");
				return false;
			}

			_visualizerId = result.Id;
			return true;
		}

		/// <summary>Stops the visualization and clears the canvas. The audio source keeps working (recording/playback).</summary>
		public async Task StopAsync()
		{
			if (!IsActive || _moduleTask is null)
			{
				return;
			}

			var module = await _moduleTask;
			var id = _visualizerId;
			_visualizerId = 0;
			await module.InvokeVoidAsync("stopVisualizer", id);
		}

		private async Task CaptureStateChangedAsync()
		{
			if (_attachedCapture!.IsCaptureActive)
			{
				if (AutoStart && !IsActive)
				{
					await StartAsync();
				}
			}
			else
			{
				//The JS side already stopped the visualizer together with the capture stream.
				_visualizerId = 0;
			}
		}

		private async Task PlayerInitializedAsync()
		{
			if (AutoStart && !IsActive)
			{
				await StartAsync();
			}
		}

		private Task<IJSObjectReference> GetModuleAsync()
		{
#if DEBUG
			var js = "./_content/Majorsoft.Blazor.Components.Media/media.js";
#else
			var js = "./_content/Majorsoft.Blazor.Components.Media/media.min.js";
#endif
			return _moduleTask ??= _jsRuntime.InvokeAsync<IJSObjectReference>("import", js).AsTask();
		}

		private async Task ReportErrorAsync(string message)
		{
			_logger.LogWarning($"Component {GetType()}: {message}");
			if (OnVisualizerError.HasDelegate)
			{
				await OnVisualizerError.InvokeAsync(message);
			}
		}

		/// <summary>Spectrum band data callback invoked from JS. Do not call it directly.</summary>
		[JSInvokable("SpectrumChangedAsync")]
		public async Task SpectrumChangedAsync(double[] bands)
		{
			if (OnSpectrumChanged.HasDelegate)
			{
				await OnSpectrumChanged.InvokeAsync(bands);
			}
		}

		/// <summary>Releases the JS visualizer instance and the interop module.</summary>
		public async ValueTask DisposeAsync()
		{
			if (_attachedCapture is not null)
			{
				_attachedCapture.CaptureStateChanged -= CaptureStateChangedAsync;
			}
			if (_attachedPlayer is not null)
			{
				_attachedPlayer.PlayerInitialized -= PlayerInitializedAsync;
			}

			try
			{
				if (_moduleTask is not null)
				{
					var module = await _moduleTask;
					if (_visualizerId != 0)
					{
						await module.InvokeVoidAsync("stopVisualizer", _visualizerId);
						_visualizerId = 0;
					}
					await module.DisposeAsync();
				}
			}
			catch (JSDisconnectedException)
			{
				//Circuit or page already gone, the browser cleaned up the visualizer.
			}

			_dotNetRef?.Dispose();
		}
	}
}
