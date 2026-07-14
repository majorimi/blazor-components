namespace Majorsoft.Blazor.Components.Media.Players
{
	/// <summary>
	/// Blazor component that renders an HTML &lt;audio&gt; element with full playback control from .NET code:
	/// events and play/pause/seek/volume/rate API. Sources can be URLs, Blob URLs of recordings
	/// (<see cref="MediaRecordingInfo.Url"/>) or .NET streams
	/// (<see cref="MediaPlayerBase.SetSourceAsync(System.IO.Stream, string)"/>).
	/// </summary>
	public partial class AudioPlayer : MediaPlayerBase
	{
	}
}
