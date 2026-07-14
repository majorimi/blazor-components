using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Components.Media.Capture
{
	/// <summary>
	/// Base class of the <see cref="CameraCapture"/> and <see cref="AudioRecorder"/> components.
	/// Owns the getUserMedia stream, the <c>MediaRecorder</c> based recording, the audio level meter
	/// and the JS interop module lifecycle.
	/// </summary>
	public abstract class MediaCaptureBase : ComponentBase, IAsyncDisposable
	{
		private Task<IJSObjectReference>? _moduleTask;
		private DotNetObjectReference<MediaCaptureBase>? _dotNetRef;
		private int _captureId;

		[Inject] private IJSRuntime _jsRuntime { get; set; } = default!;
		[Inject] private ILogger<MediaCaptureBase> _logger { get; set; } = default!;

		/// <summary>True while the media stream (camera/microphone) is open.</summary>
		public bool IsCaptureActive => _captureId != 0;

		/// <summary>Current state of the recording.</summary>
		public RecordingStates RecordingState { get; private set; } = RecordingStates.Inactive;

		/// <summary>Details of the last finished recording, null when nothing was recorded yet.</summary>
		public MediaRecordingInfo? LastRecording { get; private set; }

		/// <summary>Microphone device Id to use, null for the browser default (see <see cref="IMediaDeviceService.GetMediaDevicesAsync"/>).</summary>
		[Parameter] public string? AudioDeviceId { get; set; }

		/// <summary>Callback fired when a recording finished with the recording details.</summary>
		[Parameter] public EventCallback<MediaRecordingInfo> OnRecordingFinished { get; set; }

		/// <summary>Callback fired when the recording state changed (started, paused, resumed).</summary>
		[Parameter] public EventCallback<RecordingStates> OnRecordingStateChanged { get; set; }

		/// <summary>Callback fired periodically with the microphone input level (0..1) while the level meter runs.</summary>
		[Parameter] public EventCallback<double> OnAudioLevelChanged { get; set; }

		/// <summary>Callback fired when opening the stream, recording or the device failed, with the error message.</summary>
		[Parameter] public EventCallback<string> OnCaptureError { get; set; }

		private protected Task<IJSObjectReference> GetModuleAsync()
		{
#if DEBUG
			var js = "./_content/Majorsoft.Blazor.Components.Media/media.js";
#else
			var js = "./_content/Majorsoft.Blazor.Components.Media/media.min.js";
#endif
			return _moduleTask ??= _jsRuntime.InvokeAsync<IJSObjectReference>("import", js).AsTask();
		}

		/// <summary>Opens the media stream with the given JS constraints options. Returns true on success.</summary>
		private protected async Task<bool> StartCaptureAsync(object options, ElementReference? videoElement)
		{
			if (IsCaptureActive)
			{
				return true;
			}

			var module = await GetModuleAsync();
			_dotNetRef ??= DotNetObjectReference.Create(this);

			var result = await module.InvokeAsync<CaptureStartResult>("startCapture", videoElement, _dotNetRef, options);
			if (result.Id == 0)
			{
				await ReportErrorAsync($"Media capture could not be started: {result.Error}");
				return false;
			}

			_captureId = result.Id;
			return true;
		}

		/// <summary>Stops the media stream and releases the camera/microphone device.</summary>
		public virtual async Task StopCaptureAsync()
		{
			if (!IsCaptureActive)
			{
				return;
			}

			var module = await GetModuleAsync();
			var id = _captureId;
			_captureId = 0;
			RecordingState = RecordingStates.Inactive;
			await module.InvokeVoidAsync("stopCapture", id);
		}

		/// <summary>
		/// Starts recording the open stream with the <c>MediaRecorder</c> API.
		/// Returns the actual MIME type of the recording or null when recording could not be started.
		/// </summary>
		public async Task<string?> StartRecordingAsync(MediaRecordingOptions? options = null)
		{
			EnsureCaptureActive();

			var module = await GetModuleAsync();
			var result = await module.InvokeAsync<RecordingStartResult>("startRecording", _captureId, options ?? new MediaRecordingOptions());
			if (result.Error is not null)
			{
				await ReportErrorAsync($"Recording could not be started: {result.Error}");
				return null;
			}

			return result.MimeType;
		}

		/// <summary>Pauses the running recording.</summary>
		public async Task PauseRecordingAsync()
		{
			EnsureCaptureActive();
			var module = await GetModuleAsync();
			await module.InvokeVoidAsync("pauseRecording", _captureId);
		}

		/// <summary>Resumes the paused recording.</summary>
		public async Task ResumeRecordingAsync()
		{
			EnsureCaptureActive();
			var module = await GetModuleAsync();
			await module.InvokeVoidAsync("resumeRecording", _captureId);
		}

		/// <summary>
		/// Stops the running recording. The finished recording is reported asynchronously
		/// via <see cref="OnRecordingFinished"/> and <see cref="LastRecording"/>.
		/// </summary>
		public async Task StopRecordingAsync()
		{
			EnsureCaptureActive();
			var module = await GetModuleAsync();
			await module.InvokeVoidAsync("stopRecording", _captureId);
		}

		/// <summary>
		/// Downloads the last finished recording (see <see cref="LastRecording"/>) from the browser as a .NET stream,
		/// e.g. to save or upload it. Dispose the returned stream after use.
		/// </summary>
		/// <param name="maxAllowedSize">Maximum recording size to allow in bytes, default is 512 MB.</param>
		public async Task<Stream?> GetRecordingStreamAsync(long maxAllowedSize = 512 * 1024 * 1024)
		{
			EnsureCaptureActive();
			var module = await GetModuleAsync();
			return await MediaJsInterop.GetJsDataStreamAsync(module, "getRecordingData", _captureId, maxAllowedSize);
		}

		/// <summary>
		/// Starts the microphone level meter firing <see cref="OnAudioLevelChanged"/>. The stream must be open and contain audio.
		/// </summary>
		/// <param name="intervalMs">Reporting interval in milliseconds, default is 100.</param>
		public async Task StartLevelMeterAsync(int intervalMs = 100)
		{
			EnsureCaptureActive();
			var module = await GetModuleAsync();
			await module.InvokeVoidAsync("startLevelMeter", _captureId, intervalMs);
		}

		/// <summary>Stops the microphone level meter.</summary>
		public async Task StopLevelMeterAsync()
		{
			EnsureCaptureActive();
			var module = await GetModuleAsync();
			await module.InvokeVoidAsync("stopLevelMeter", _captureId, null);
		}

		private protected int CaptureId => _captureId;

		private protected void EnsureCaptureActive()
		{
			if (!IsCaptureActive)
			{
				throw new InvalidOperationException("Media capture is not active, the stream must be started first.");
			}
		}

		private protected async Task ReportErrorAsync(string message)
		{
			_logger.LogWarning($"Component {GetType()}: {message}");
			if (OnCaptureError.HasDelegate)
			{
				await OnCaptureError.InvokeAsync(message);
			}
		}

		/// <summary>Recording finished callback invoked from JS. Do not call it directly.</summary>
		[JSInvokable("RecordingFinishedAsync")]
		public async Task RecordingFinishedAsync(MediaRecordingInfo recording)
		{
			LastRecording = recording;
			RecordingState = RecordingStates.Inactive;

			if (OnRecordingStateChanged.HasDelegate)
			{
				await OnRecordingStateChanged.InvokeAsync(RecordingStates.Inactive);
			}
			if (OnRecordingFinished.HasDelegate)
			{
				await OnRecordingFinished.InvokeAsync(recording);
			}
			StateHasChanged();
		}

		/// <summary>Recording state callback invoked from JS. Do not call it directly.</summary>
		[JSInvokable("RecordingStateChangedAsync")]
		public async Task RecordingStateChangedAsync(string state)
		{
			RecordingState = state switch
			{
				"recording" => RecordingStates.Recording,
				"paused" => RecordingStates.Paused,
				_ => RecordingStates.Inactive
			};

			if (OnRecordingStateChanged.HasDelegate)
			{
				await OnRecordingStateChanged.InvokeAsync(RecordingState);
			}
			StateHasChanged();
		}

		/// <summary>Audio level callback invoked from JS. Do not call it directly.</summary>
		[JSInvokable("AudioLevelChangedAsync")]
		public async Task AudioLevelChangedAsync(double level)
		{
			if (OnAudioLevelChanged.HasDelegate)
			{
				await OnAudioLevelChanged.InvokeAsync(level);
			}
		}

		/// <summary>Capture error callback invoked from JS. Do not call it directly.</summary>
		[JSInvokable("CaptureErrorAsync")]
		public async Task CaptureErrorAsync(string error)
		{
			await ReportErrorAsync(error);
		}

		/// <summary>Releases the media stream and the JS interop module.</summary>
		public async ValueTask DisposeAsync()
		{
			try
			{
				if (_moduleTask is not null)
				{
					var module = await _moduleTask;
					if (IsCaptureActive)
					{
						await module.InvokeVoidAsync("stopCapture", _captureId);
						_captureId = 0;
					}
					await module.DisposeAsync();
				}
			}
			catch (JSDisconnectedException)
			{
				//Circuit or page already gone, the browser cleaned up the stream.
			}

			_dotNetRef?.Dispose();
		}
	}
}
