using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Components.Media.Players
{
	/// <summary>
	/// Blazor component that renders an HTML &lt;video&gt; element with full playback control from .NET code:
	/// events, play/pause/seek/volume/rate API, fullscreen, Picture-in-Picture and frame capture.
	/// Sources can be URLs, Blob URLs of recordings (<see cref="MediaRecordingInfo.Url"/>) or .NET streams
	/// (<see cref="MediaPlayerBase.SetSourceAsync(System.IO.Stream, string)"/>).
	/// </summary>
	public partial class VideoPlayer : MediaPlayerBase
	{
		/// <summary>Width of the video element in px or % (see <see cref="IsPlayerDimensionInPixels"/>). Default is 640.</summary>
		[Parameter] public int Width { get; set; } = 640;

		/// <summary>Height of the video element in px or % (see <see cref="IsPlayerDimensionInPixels"/>). Default is 360.</summary>
		[Parameter] public int Height { get; set; } = 360;

		/// <summary>
		/// Gets or sets True indicating whether the player dimensions are specified in pixels, False means %. Default is true.
		/// In % mode <see cref="Width"/>/<see cref="Height"/> are applied as CSS size relative to the parent element.
		/// </summary>
		[Parameter] public bool IsPlayerDimensionInPixels { get; set; } = true;

		/// <summary>URL of the image shown while the video is not playing yet (HTML <c>poster</c> attribute).</summary>
		[Parameter] public string? Poster { get; set; }

		//HTML width/height attributes are px by spec, they are only rendered in pixel mode.
		private string? WidthAttribute => IsPlayerDimensionInPixels ? Width.ToString() : null;
		private string? HeightAttribute => IsPlayerDimensionInPixels ? Height.ToString() : null;

		//In % mode the dimensions are applied as CSS size in front of the user provided Style.
		private string? PlayerStyle => IsPlayerDimensionInPixels
			? Style
			: $"width: {Width}%; height: {Height}%; {Style}";

		/// <summary>Requests browser fullscreen mode for the video element.</summary>
		public async Task RequestFullscreenAsync()
		{
			var module = await EnsurePlayerAsync();
			await module.InvokeVoidAsync("playerRequestFullscreen", PlayerId);
		}

		/// <summary>
		/// Enters or exits Picture-in-Picture mode. Returns true when the video is in PiP mode after the call,
		/// false when it was exited or PiP is not supported by the browser.
		/// </summary>
		public async Task<bool> TogglePictureInPictureAsync()
		{
			var module = await EnsurePlayerAsync();
			return await module.InvokeAsync<bool>("playerTogglePictureInPicture", PlayerId);
		}

		/// <summary>
		/// Captures the currently displayed video frame as an image. Returns null when no frame is available yet.
		/// The image stays in the browser as a Blob: show it with <see cref="MediaPhotoInfo.Url"/>, download the
		/// bytes with <see cref="GetFrameStreamAsync"/>.
		/// Note: the video source must be CORS accessible, otherwise the browser blocks reading the frame pixels.
		/// </summary>
		/// <param name="mimeType">Image format: "image/png" (default), "image/jpeg" or "image/webp".</param>
		/// <param name="quality">Compression quality 0..1 for lossy formats (jpeg/webp), null for browser default.</param>
		public async Task<MediaPhotoInfo?> CaptureFrameAsync(string mimeType = "image/png", double? quality = null)
		{
			var module = await EnsurePlayerAsync();
			return await module.InvokeAsync<MediaPhotoInfo?>("playerCaptureFrame", PlayerId, new { mimeType, quality });
		}

		/// <summary>
		/// Downloads the last captured frame (see <see cref="CaptureFrameAsync"/>) from the browser as a .NET stream.
		/// Dispose the returned stream after use.
		/// </summary>
		/// <param name="maxAllowedSize">Maximum image size to allow in bytes, default is 50 MB.</param>
		public async Task<Stream?> GetFrameStreamAsync(long maxAllowedSize = 50 * 1024 * 1024)
		{
			var module = await EnsurePlayerAsync();
			return await MediaJsInterop.GetJsDataStreamAsync(module, "playerGetFrameData", PlayerId, maxAllowedSize);
		}
	}
}
