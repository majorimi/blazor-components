using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Majorsoft.Blazor.Components.Media.Tests
{
	[TestClass]
	public class MediaTypesTest
	{
		[TestMethod]
		public void MediaRecordingInfo_Duration_should_be_calculated_from_DurationMs()
		{
			var recording = new MediaRecordingInfo { DurationMs = 90500 };

			Assert.AreEqual(TimeSpan.FromMilliseconds(90500), recording.Duration);
		}

		[TestMethod]
		public void MediaDeviceInfo_kind_helpers_should_match_device_kind()
		{
			var camera = new MediaDeviceInfo { Kind = "videoinput" };
			var microphone = new MediaDeviceInfo { Kind = "audioinput" };
			var speaker = new MediaDeviceInfo { Kind = "audiooutput" };

			Assert.IsTrue(camera.IsVideoInput);
			Assert.IsFalse(camera.IsAudioInput);
			Assert.IsFalse(camera.IsAudioOutput);

			Assert.IsTrue(microphone.IsAudioInput);
			Assert.IsFalse(microphone.IsVideoInput);

			Assert.IsTrue(speaker.IsAudioOutput);
			Assert.IsFalse(speaker.IsAudioInput);
		}
	}
}
