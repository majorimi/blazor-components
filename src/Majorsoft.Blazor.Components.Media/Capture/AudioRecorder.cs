using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

namespace Majorsoft.Blazor.Components.Media.Capture
{
	/// <summary>
	/// Blazor component that records audio from a microphone using the <c>getUserMedia</c> and
	/// <c>MediaRecorder</c> APIs. It renders no HTML, use it with <c>@ref</c> and control it from code:
	/// <see cref="OpenMicrophoneAsync"/> then <see cref="MediaCaptureBase.StartRecordingAsync"/>.
	/// A live input level meter is available via <see cref="MediaCaptureBase.OnAudioLevelChanged"/>.
	/// Microphone access requires HTTPS (or localhost) and is granted by the user per site.
	/// </summary>
	public class AudioRecorder : MediaCaptureBase
	{
		/// <summary>True to open the microphone automatically after the component was rendered. Default is false.</summary>
		[Parameter] public bool AutoStart { get; set; }

		/// <summary>
		/// True to start the input level meter automatically when the microphone was opened,
		/// reported via <see cref="MediaCaptureBase.OnAudioLevelChanged"/>. Default is true.
		/// </summary>
		[Parameter] public bool EnableLevelMeter { get; set; } = true;

		/// <summary>Reporting interval of the level meter in milliseconds. Default is 100.</summary>
		[Parameter] public int LevelMeterIntervalMs { get; set; } = 100;

		/// <summary>Callback fired when the microphone stream was opened.</summary>
		[Parameter] public EventCallback OnMicrophoneOpened { get; set; }

		/// <summary>Callback fired when the microphone stream was closed.</summary>
		[Parameter] public EventCallback OnMicrophoneClosed { get; set; }

		/// <summary>Opens the microphone after first render when <see cref="AutoStart"/> is set.</summary>
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender && AutoStart)
			{
				await OpenMicrophoneAsync();
			}
		}

		/// <summary>
		/// Opens the microphone stream (the browser will ask the user for permission).
		/// Returns true on success, errors are reported via <see cref="MediaCaptureBase.OnCaptureError"/>.
		/// </summary>
		public async Task<bool> OpenMicrophoneAsync()
		{
			var started = await StartCaptureAsync(new
			{
				video = false,
				audio = true,
				audioDeviceId = AudioDeviceId
			}, null);

			if (started)
			{
				if (EnableLevelMeter)
				{
					await StartLevelMeterAsync(LevelMeterIntervalMs);
				}
				if (OnMicrophoneOpened.HasDelegate)
				{
					await OnMicrophoneOpened.InvokeAsync();
				}
			}
			return started;
		}

		/// <summary>Closes the microphone stream and releases the device.</summary>
		public override async Task StopCaptureAsync()
		{
			var wasActive = IsCaptureActive;
			await base.StopCaptureAsync();

			if (wasActive && OnMicrophoneClosed.HasDelegate)
			{
				await OnMicrophoneClosed.InvokeAsync();
			}
		}
	}
}
