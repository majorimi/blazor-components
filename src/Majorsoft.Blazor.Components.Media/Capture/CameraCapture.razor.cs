using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Components.Media.Capture
{
	/// <summary>
	/// Blazor component that shows live camera preview in an HTML &lt;video&gt; element using the
	/// <c>getUserMedia</c> API. Photos can be taken from the live stream (<see cref="TakePhotoAsync"/>)
	/// and video (optionally with audio) can be recorded with the <c>MediaRecorder</c> API
	/// (see <see cref="MediaCaptureBase.StartRecordingAsync"/>).
	/// Camera access requires HTTPS (or localhost) and is granted by the user per site.
	/// </summary>
	public partial class CameraCapture : MediaCaptureBase
	{
		private ElementReference _videoRef;

		/// <summary>Exposes the Blazor <see cref="ElementReference"/> of the rendered &lt;video&gt; preview element.</summary>
		public ElementReference InnerElementReference => _videoRef;

		/// <summary>Width of the preview element in px or % (see <see cref="IsCameraDimensionInPixels"/>). Default is 640.</summary>
		[Parameter] public int Width { get; set; } = 640;

		/// <summary>Height of the preview element in px or % (see <see cref="IsCameraDimensionInPixels"/>). Default is 480.</summary>
		[Parameter] public int Height { get; set; } = 480;

		/// <summary>
		/// Gets or sets True indicating whether the preview dimensions are specified in pixels, False means %. Default is true.
		/// In pixel mode <see cref="Width"/>/<see cref="Height"/> are also used as the requested camera resolution,
		/// in % mode they are applied as CSS size only and the browser picks the default resolution.
		/// </summary>
		[Parameter] public bool IsCameraDimensionInPixels { get; set; } = true;

		/// <summary>Camera device Id to use, null for the browser default (see <see cref="IMediaDeviceService.GetMediaDevicesAsync"/>).</summary>
		[Parameter] public string? VideoDeviceId { get; set; }

		/// <summary>Camera direction preference (front/rear), used when no <see cref="VideoDeviceId"/> was set. Mostly for mobile devices.</summary>
		[Parameter] public CameraFacingModes FacingMode { get; set; } = CameraFacingModes.Default;

		/// <summary>True to open the microphone as well so video recordings have sound. Default is false. The preview stays muted.</summary>
		[Parameter] public bool RecordAudio { get; set; }

		/// <summary>True to start the camera automatically after the component was rendered. Default is false.</summary>
		[Parameter] public bool AutoStart { get; set; }

		/// <summary>Custom CSS class(es) applied to the &lt;video&gt; element.</summary>
		[Parameter] public string? Class { get; set; }

		/// <summary>Custom inline style applied to the &lt;video&gt; element.</summary>
		[Parameter] public string? Style { get; set; }

		/// <summary>Callback fired when the camera stream was opened and the preview is live.</summary>
		[Parameter] public EventCallback OnCameraStarted { get; set; }

		/// <summary>Callback fired when the camera stream was stopped.</summary>
		[Parameter] public EventCallback OnCameraStopped { get; set; }

		/// <summary>Callback fired when a photo was taken with <see cref="TakePhotoAsync"/>.</summary>
		[Parameter] public EventCallback<MediaPhotoInfo> OnPhotoTaken { get; set; }

		/// <summary>Arbitrary HTML attributes applied to the &lt;video&gt; element.</summary>
		[Parameter(CaptureUnmatchedValues = true)]
		public Dictionary<string, object>? AdditionalAttributes { get; set; }

		//HTML width/height attributes are px by spec, they are only rendered in pixel mode.
		private string? WidthAttribute => IsCameraDimensionInPixels ? Width.ToString() : null;
		private string? HeightAttribute => IsCameraDimensionInPixels ? Height.ToString() : null;

		//In % mode the dimensions are applied as CSS size in front of the user provided Style.
		private string? VideoStyle => IsCameraDimensionInPixels
			? Style
			: $"width: {Width}%; height: {Height}%; {Style}";

		/// <summary>Starts the camera after first render when <see cref="AutoStart"/> is set.</summary>
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender && AutoStart)
			{
				await StartCameraAsync();
			}
		}

		/// <summary>
		/// Opens the camera (and microphone when <see cref="RecordAudio"/> is set) and starts the live preview.
		/// The browser will ask the user for permission. Returns true on success, errors are reported via
		/// <see cref="MediaCaptureBase.OnCaptureError"/>.
		/// </summary>
		public async Task<bool> StartCameraAsync()
		{
			var started = await StartCaptureAsync(new
			{
				video = true,
				audio = RecordAudio,
				videoDeviceId = VideoDeviceId,
				audioDeviceId = AudioDeviceId,
				facingMode = FacingMode == CameraFacingModes.Default ? null : FacingMode.ToString().ToLower(),
				//In % mode Width/Height are CSS percentages, not a meaningful camera resolution constraint.
				width = IsCameraDimensionInPixels ? Width : 0,
				height = IsCameraDimensionInPixels ? Height : 0
			}, _videoRef);

			if (started && OnCameraStarted.HasDelegate)
			{
				await OnCameraStarted.InvokeAsync();
			}
			return started;
		}

		/// <summary>Stops the camera stream, the preview and releases the device.</summary>
		public override async Task StopCaptureAsync()
		{
			var wasActive = IsCaptureActive;
			await base.StopCaptureAsync();

			if (wasActive && OnCameraStopped.HasDelegate)
			{
				await OnCameraStopped.InvokeAsync();
			}
		}

		/// <summary>
		/// Takes a photo from the current live preview frame in full camera resolution.
		/// Returns null when the camera is not active yet. The photo stays in the browser as a Blob:
		/// show it with <see cref="MediaPhotoInfo.Url"/>, download the bytes with <see cref="GetPhotoStreamAsync"/>.
		/// </summary>
		/// <param name="mimeType">Image format: "image/png" (default), "image/jpeg" or "image/webp".</param>
		/// <param name="quality">Compression quality 0..1 for lossy formats (jpeg/webp), null for browser default.</param>
		public async Task<MediaPhotoInfo?> TakePhotoAsync(string mimeType = "image/png", double? quality = null)
		{
			EnsureCaptureActive();

			var module = await GetModuleAsync();
			var photo = await module.InvokeAsync<MediaPhotoInfo?>("takePhoto", CaptureId, new { mimeType, quality });

			if (photo is not null && OnPhotoTaken.HasDelegate)
			{
				await OnPhotoTaken.InvokeAsync(photo);
			}
			return photo;
		}

		/// <summary>
		/// Downloads the last taken photo (see <see cref="TakePhotoAsync"/>) from the browser as a .NET stream,
		/// e.g. to save or upload it. Dispose the returned stream after use.
		/// </summary>
		/// <param name="maxAllowedSize">Maximum photo size to allow in bytes, default is 50 MB.</param>
		public async Task<System.IO.Stream?> GetPhotoStreamAsync(long maxAllowedSize = 50 * 1024 * 1024)
		{
			EnsureCaptureActive();
			var module = await GetModuleAsync();
			return await MediaJsInterop.GetJsDataStreamAsync(module, "getPhotoData", CaptureId, maxAllowedSize);
		}
	}
}
