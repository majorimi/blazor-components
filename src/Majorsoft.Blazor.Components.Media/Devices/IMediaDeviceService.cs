using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Majorsoft.Blazor.Components.Media
{
	/// <summary>
	/// Injectable service to discover media capture capabilities and devices of the browser.
	/// Register it with <c>services.AddMediaComponents()</c>.
	/// </summary>
	public interface IMediaDeviceService : IAsyncDisposable
	{
		/// <summary>Returns true when the browser supports media capture (<c>getUserMedia</c> API).</summary>
		ValueTask<bool> IsCaptureSupportedAsync();

		/// <summary>Returns true when the browser supports recording (<c>MediaRecorder</c> API).</summary>
		ValueTask<bool> IsMediaRecorderSupportedAsync();

		/// <summary>
		/// Lists the available cameras, microphones and speakers. Device labels are only populated
		/// after the user granted media permission (see <see cref="RequestPermissionsAsync"/>).
		/// </summary>
		ValueTask<IEnumerable<MediaDeviceInfo>> GetMediaDevicesAsync();

		/// <summary>
		/// Asks the user for microphone and/or camera permission by opening (then immediately closing) a stream.
		/// Returns null on success, otherwise the error message (e.g. permission denied).
		/// </summary>
		ValueTask<string?> RequestPermissionsAsync(bool audio = true, bool video = true);

		/// <summary>Lists the recording MIME types (container/codec) supported by the browser.</summary>
		/// <param name="video">True for video recording types, false for audio only types.</param>
		ValueTask<IEnumerable<string>> GetSupportedMimeTypesAsync(bool video);
	}
}
