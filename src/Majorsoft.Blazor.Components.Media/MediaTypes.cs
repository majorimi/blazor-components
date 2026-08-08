using System;

namespace Majorsoft.Blazor.Components.Media
{
	/// <summary>
	/// Camera direction preference used when no explicit device Id was chosen
	/// (maps to the <c>facingMode</c> media constraint and the HTML <c>capture</c> attribute).
	/// </summary>
	public enum CameraFacingModes
	{
		/// <summary>No preference, browser picks the default camera.</summary>
		Default = 0,
		/// <summary>Front (selfie) camera.</summary>
		User,
		/// <summary>Rear (environment) camera.</summary>
		Environment
	}

	/// <summary>
	/// State of a <c>MediaRecorder</c> based recording.
	/// </summary>
	public enum RecordingStates
	{
		/// <summary>No recording is running.</summary>
		Inactive = 0,
		/// <summary>Recording is running.</summary>
		Recording,
		/// <summary>Recording was paused, it can be resumed.</summary>
		Paused
	}

	/// <summary>
	/// Rendering mode of the <see cref="Visualization.AudioVisualizer"/> component.
	/// </summary>
	public enum AudioVisualizerModes
	{
		/// <summary>FFT frequency spectrum drawn as vertical bars (spectrum analyzer).</summary>
		FrequencyBars = 0,
		/// <summary>Time domain waveform drawn as a line (oscilloscope).</summary>
		Waveform
	}

	/// <summary>
	/// Media type filter for the <see cref="Capture.MediaFileCapture"/> component (HTML <c>accept</c> attribute).
	/// </summary>
	public enum CaptureMediaTypes
	{
		/// <summary>Any file type is accepted.</summary>
		Any = 0,
		/// <summary>Images only (<c>image/*</c>).</summary>
		Photo,
		/// <summary>Videos only (<c>video/*</c>).</summary>
		Video,
		/// <summary>Audio only (<c>audio/*</c>).</summary>
		Audio
	}

	/// <summary>
	/// Describes a media input/output device returned by <see cref="IMediaDeviceService.GetMediaDevicesAsync"/>.
	/// Labels are only populated after the user granted media permissions
	/// (see <see cref="IMediaDeviceService.RequestPermissionsAsync"/>).
	/// </summary>
	public class MediaDeviceInfo
	{
		/// <summary>Unique Id of the device, can be used as device Id constraint for capture components.</summary>
		public string DeviceId { get; set; } = "";

		/// <summary>Group Id, devices sharing the same physical device have the same group Id.</summary>
		public string GroupId { get; set; } = "";

		/// <summary>Device kind: "videoinput", "audioinput" or "audiooutput".</summary>
		public string Kind { get; set; } = "";

		/// <summary>Human readable device name, empty until media permission was granted.</summary>
		public string Label { get; set; } = "";

		/// <summary>True when the device is a camera.</summary>
		public bool IsVideoInput => Kind == "videoinput";

		/// <summary>True when the device is a microphone.</summary>
		public bool IsAudioInput => Kind == "audioinput";

		/// <summary>True when the device is a speaker.</summary>
		public bool IsAudioOutput => Kind == "audiooutput";
	}

	/// <summary>
	/// A photo taken from a camera stream or a captured video frame. The image data lives in the browser
	/// as a Blob: <see cref="Url"/> is a Blob URL usable directly as <c>img src</c>, the bytes can be
	/// downloaded to .NET with <c>GetPhotoStreamAsync</c>/<c>GetFrameStreamAsync</c> of the owning component.
	/// </summary>
	public class MediaPhotoInfo
	{
		/// <summary>Blob URL of the image, usable as <c>img src</c> in the same browser session.</summary>
		public string Url { get; set; } = "";

		/// <summary>Width of the image in pixels.</summary>
		public int Width { get; set; }

		/// <summary>Height of the image in pixels.</summary>
		public int Height { get; set; }

		/// <summary>MIME type of the image, e.g. "image/png".</summary>
		public string MimeType { get; set; } = "";

		/// <summary>Size of the image in bytes.</summary>
		public long SizeBytes { get; set; }
	}

	/// <summary>
	/// Describes a finished audio/video recording. The recorded data lives in the browser as a Blob:
	/// <see cref="Url"/> is a Blob URL playable by the player components, the bytes can be downloaded
	/// to .NET with <c>GetRecordingStreamAsync</c> of the capture components.
	/// </summary>
	public class MediaRecordingInfo
	{
		/// <summary>Blob URL of the recording, usable as media source in the same browser session.</summary>
		public string Url { get; set; } = "";

		/// <summary>Actual MIME type of the recording, e.g. "video/webm;codecs=vp9,opus".</summary>
		public string MimeType { get; set; } = "";

		/// <summary>Size of the recording in bytes.</summary>
		public long SizeBytes { get; set; }

		/// <summary>Length of the recording in milliseconds (wall clock time including paused periods).</summary>
		public double DurationMs { get; set; }

		/// <summary>Length of the recording (wall clock time including paused periods).</summary>
		public TimeSpan Duration => TimeSpan.FromMilliseconds(DurationMs);
	}

	/// <summary>
	/// Options for starting a <c>MediaRecorder</c> based recording.
	/// </summary>
	public class MediaRecordingOptions
	{
		/// <summary>
		/// Requested container/codec MIME type, e.g. "video/webm;codecs=vp9,opus". When null or not supported
		/// the first supported type is picked automatically (see <see cref="IMediaDeviceService.GetSupportedMimeTypesAsync"/>).
		/// </summary>
		public string? MimeType { get; set; }

		/// <summary>Target video bitrate in bits/sec, 0 means browser default.</summary>
		public int VideoBitsPerSecond { get; set; }

		/// <summary>Target audio bitrate in bits/sec, 0 means browser default.</summary>
		public int AudioBitsPerSecond { get; set; }

		/// <summary>When greater than 0 the recording is chunked into blocks of the given length in milliseconds.</summary>
		public int TimeSliceMs { get; set; }
	}

	/// <summary>
	/// Snapshot of an HTML media element state, reported with player events and returned by <c>GetStateAsync</c>.
	/// </summary>
	public class MediaPlayerState
	{
		/// <summary>Current playback position in seconds.</summary>
		public double CurrentTime { get; set; }

		/// <summary>Length of the media in seconds, 0 when not known (yet) or for live streams.</summary>
		public double Duration { get; set; }

		/// <summary>True when playback is paused.</summary>
		public bool Paused { get; set; }

		/// <summary>True when playback reached the end of the media.</summary>
		public bool Ended { get; set; }

		/// <summary>True when the element is muted.</summary>
		public bool Muted { get; set; }

		/// <summary>Volume between 0 and 1.</summary>
		public double Volume { get; set; }

		/// <summary>Playback speed, 1 is normal speed.</summary>
		public double PlaybackRate { get; set; }

		/// <summary>HTML media element readyState (0 = HAVE_NOTHING ... 4 = HAVE_ENOUGH_DATA).</summary>
		public int ReadyState { get; set; }

		/// <summary>True when the media is seekable.</summary>
		public bool IsSeekable { get; set; }
	}

	/// <summary>Result of starting a capture stream (internal interop DTO).</summary>
	internal class CaptureStartResult
	{
		public int Id { get; set; }
		public string? Error { get; set; }
	}

	/// <summary>Result of starting a recording (internal interop DTO).</summary>
	internal class RecordingStartResult
	{
		public string? Error { get; set; }
		public string? MimeType { get; set; }
	}

	/// <summary>Result of starting an audio visualizer (internal interop DTO).</summary>
	internal class VisualizerStartResult
	{
		public int Id { get; set; }
		public string? Error { get; set; }
	}
}
