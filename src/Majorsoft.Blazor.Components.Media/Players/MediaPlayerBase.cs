using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Components.Media.Players
{
	/// <summary>
	/// Base class of the <see cref="VideoPlayer"/> and <see cref="AudioPlayer"/> components.
	/// Renders an HTML media element, subscribes to its events and exposes the playback API
	/// (play, pause, seek, volume, rate, source switching) to .NET code.
	/// </summary>
	public abstract class MediaPlayerBase : ComponentBase, IAsyncDisposable
	{
		private Task<IJSObjectReference>? _moduleTask;
		private DotNetObjectReference<MediaPlayerBase>? _dotNetRef;
		private int _playerId;

		[Inject] private IJSRuntime _jsRuntime { get; set; } = default!;
		[Inject] private ILogger<MediaPlayerBase> _logger { get; set; } = default!;

		private protected ElementReference _elementRef;

		/// <summary>Exposes the Blazor <see cref="ElementReference"/> of the rendered media element.</summary>
		public ElementReference InnerElementReference => _elementRef;

		/// <summary>Media source URL. To play .NET provided data use <see cref="SetSourceAsync(Stream, string)"/> instead.</summary>
		[Parameter] public string? Source { get; set; }

		/// <summary>True to show the browser's built-in player controls. Default is true.</summary>
		[Parameter] public bool ShowControls { get; set; } = true;

		/// <summary>True to start playback automatically (browsers usually only allow muted autoplay). Default is false.</summary>
		[Parameter] public bool Autoplay { get; set; }

		/// <summary>True to restart the media automatically when it ended. Default is false.</summary>
		[Parameter] public bool Loop { get; set; }

		/// <summary>True to mute the media. Default is false.</summary>
		[Parameter] public bool Muted { get; set; }

		/// <summary>HTML <c>preload</c> attribute: "none", "metadata" or "auto". Null (default) renders no attribute.</summary>
		[Parameter] public string? Preload { get; set; }

		/// <summary>Custom CSS class(es) applied to the media element.</summary>
		[Parameter] public string? Class { get; set; }

		/// <summary>Custom inline style applied to the media element.</summary>
		[Parameter] public string? Style { get; set; }

		/// <summary>Callback fired when playback started.</summary>
		[Parameter] public EventCallback<MediaPlayerState> OnPlay { get; set; }

		/// <summary>Callback fired when playback was paused.</summary>
		[Parameter] public EventCallback<MediaPlayerState> OnPause { get; set; }

		/// <summary>Callback fired when playback reached the end of the media.</summary>
		[Parameter] public EventCallback<MediaPlayerState> OnEnded { get; set; }

		/// <summary>Callback fired when a seek operation finished.</summary>
		[Parameter] public EventCallback<MediaPlayerState> OnSeeked { get; set; }

		/// <summary>Callback fired when the volume or muted state changed.</summary>
		[Parameter] public EventCallback<MediaPlayerState> OnVolumeChanged { get; set; }

		/// <summary>Callback fired when the playback rate changed.</summary>
		[Parameter] public EventCallback<MediaPlayerState> OnRateChanged { get; set; }

		/// <summary>Callback fired when the media metadata (e.g. duration) was loaded.</summary>
		[Parameter] public EventCallback<MediaPlayerState> OnMetadataLoaded { get; set; }

		/// <summary>Callback fired when the media can start playing.</summary>
		[Parameter] public EventCallback<MediaPlayerState> OnCanPlay { get; set; }

		/// <summary>Callback fired when playback stopped for buffering.</summary>
		[Parameter] public EventCallback<MediaPlayerState> OnWaiting { get; set; }

		/// <summary>
		/// Callback fired periodically with the playback position while playing (browser controlled rate, ~4x/sec).
		/// The element event is only subscribed when this callback is set to avoid unnecessary interop calls.
		/// </summary>
		[Parameter] public EventCallback<MediaPlayerState> OnTimeUpdate { get; set; }

		/// <summary>Callback fired when loading or playing the media failed, with the error message.</summary>
		[Parameter] public EventCallback<string> OnError { get; set; }

		/// <summary>Arbitrary HTML attributes applied to the media element.</summary>
		[Parameter(CaptureUnmatchedValues = true)]
		public Dictionary<string, object>? AdditionalAttributes { get; set; }

		/// <summary>Loads the JS interop module and initializes the JS player instance after first render.</summary>
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (!firstRender)
			{
				return;
			}

#if DEBUG
			var js = "./_content/Majorsoft.Blazor.Components.Media/media.js";
#else
			var js = "./_content/Majorsoft.Blazor.Components.Media/media.min.js";
#endif
			_moduleTask = _jsRuntime.InvokeAsync<IJSObjectReference>("import", js).AsTask();

			var module = await _moduleTask;
			_dotNetRef = DotNetObjectReference.Create(this);
			_playerId = await module.InvokeAsync<int>("initPlayer", _elementRef, _dotNetRef, OnTimeUpdate.HasDelegate);

			if (PlayerInitialized is not null)
			{
				await PlayerInitialized.Invoke();
			}
		}

		/// <summary>
		/// Starts playback. Returns true on success; false when the browser blocked it (e.g. autoplay policy
		/// requires a user gesture), in which case <see cref="OnError"/> is fired with the reason.
		/// </summary>
		public async Task<bool> PlayAsync()
		{
			var module = await EnsurePlayerAsync();
			var error = await module.InvokeAsync<string?>("playerPlay", _playerId);
			if (error is not null)
			{
				await ReportErrorAsync($"Media playback could not be started: {error}");
				return false;
			}
			return true;
		}

		/// <summary>Pauses playback.</summary>
		public async Task PauseAsync()
		{
			var module = await EnsurePlayerAsync();
			await module.InvokeVoidAsync("playerPause", _playerId);
		}

		/// <summary>Seeks to the given playback position in seconds.</summary>
		public async Task SeekAsync(double seconds)
		{
			var module = await EnsurePlayerAsync();
			await module.InvokeVoidAsync("playerSeek", _playerId, seconds);
		}

		/// <summary>Sets the volume (0..1).</summary>
		public async Task SetVolumeAsync(double volume)
		{
			var module = await EnsurePlayerAsync();
			await module.InvokeVoidAsync("playerSetVolume", _playerId, volume);
		}

		/// <summary>Mutes or unmutes the media.</summary>
		public async Task SetMutedAsync(bool muted)
		{
			var module = await EnsurePlayerAsync();
			await module.InvokeVoidAsync("playerSetMuted", _playerId, muted);
		}

		/// <summary>Sets the playback speed, 1 is normal speed.</summary>
		public async Task SetPlaybackRateAsync(double rate)
		{
			var module = await EnsurePlayerAsync();
			await module.InvokeVoidAsync("playerSetRate", _playerId, rate);
		}

		/// <summary>Returns the current state of the media element (position, duration, volume, ...).</summary>
		public async Task<MediaPlayerState?> GetStateAsync()
		{
			var module = await EnsurePlayerAsync();
			return await module.InvokeAsync<MediaPlayerState?>("playerGetState", _playerId);
		}

		/// <summary>Switches the media source to the given URL (regular URL or Blob URL, e.g. <see cref="MediaRecordingInfo.Url"/>).</summary>
		public async Task SetSourceAsync(string url)
		{
			var module = await EnsurePlayerAsync();
			await module.InvokeVoidAsync("playerSetSource", _playerId, url);
		}

		/// <summary>
		/// Streams the given .NET data to the browser and plays it from a Blob, e.g. media loaded from
		/// a database or downloaded recording. The stream is disposed after the transfer.
		/// </summary>
		/// <param name="stream">Media content to play.</param>
		/// <param name="mimeType">MIME type of the content, e.g. "video/webm".</param>
		public async Task SetSourceAsync(Stream stream, string mimeType)
		{
			var module = await EnsurePlayerAsync();
			using var streamRef = new DotNetStreamReference(stream);
			await module.InvokeVoidAsync("playerSetStreamSource", _playerId, streamRef, mimeType);
		}

		internal int PlayerId => _playerId;

		/// <summary>
		/// Raised when the JS player instance was initialized after first render, lets an attached
		/// <see cref="Visualization.AudioVisualizer"/> start regardless of the render order of the components.
		/// </summary>
		internal event Func<Task>? PlayerInitialized;

		private protected async Task<IJSObjectReference> EnsurePlayerAsync()
		{
			if (_moduleTask is null || _playerId == 0)
			{
				throw new InvalidOperationException("The media player was not initialized yet, the component must be rendered first.");
			}
			return await _moduleTask;
		}

		private protected async Task ReportErrorAsync(string message)
		{
			_logger.LogWarning($"Component {GetType()}: {message}");
			if (OnError.HasDelegate)
			{
				await OnError.InvokeAsync(message);
			}
		}

		/// <summary>Media element event callback invoked from JS. Do not call it directly.</summary>
		[JSInvokable("PlayerEventAsync")]
		public async Task PlayerEventAsync(string eventName, MediaPlayerState state)
		{
			var callback = eventName switch
			{
				"play" => OnPlay,
				"pause" => OnPause,
				"ended" => OnEnded,
				"seeked" => OnSeeked,
				"volumechange" => OnVolumeChanged,
				"ratechange" => OnRateChanged,
				"loadedmetadata" => OnMetadataLoaded,
				"canplay" => OnCanPlay,
				"waiting" => OnWaiting,
				"timeupdate" => OnTimeUpdate,
				_ => default
			};

			if (callback.HasDelegate)
			{
				await callback.InvokeAsync(state);
			}
		}

		/// <summary>Media element error callback invoked from JS. Do not call it directly.</summary>
		[JSInvokable("PlayerErrorAsync")]
		public async Task PlayerErrorAsync(string error)
		{
			await ReportErrorAsync(error);
		}

		/// <summary>Releases the JS player instance and the interop module.</summary>
		public async ValueTask DisposeAsync()
		{
			try
			{
				if (_moduleTask is not null)
				{
					var module = await _moduleTask;
					if (_playerId != 0)
					{
						await module.InvokeVoidAsync("disposePlayer", _playerId);
					}
					await module.DisposeAsync();
				}
			}
			catch (JSDisconnectedException)
			{
				//Circuit or page already gone, nothing to release.
			}

			_dotNetRef?.Dispose();
		}
	}
}
