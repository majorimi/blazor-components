using Bunit;

using Majorsoft.Blazor.Components.CommonTestsBase;
using Majorsoft.Blazor.Components.Media.Capture;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Majorsoft.Blazor.Components.Media.Tests.Capture
{
	[TestClass]
	public class MediaFileCaptureTest : ComponentsTestBase<MediaFileCapture>
	{
		[TestMethod]
		public void MediaFileCapture_should_render_file_input_with_photo_accept_by_default()
		{
			var rendered = _testContext.Render<MediaFileCapture>();

			var input = rendered.Find("input");

			Assert.AreEqual("file", input.GetAttribute("type"));
			Assert.AreEqual("image/*", input.GetAttribute("accept"));
			Assert.IsFalse(input.HasAttribute("capture"));
			Assert.IsFalse(input.HasAttribute("multiple"));
			Assert.IsFalse(input.HasAttribute("disabled"));
		}

		[TestMethod]
		public void MediaFileCapture_should_render_accept_for_media_types()
		{
			var video = _testContext.Render<MediaFileCapture>(parameters => parameters
				.Add(p => p.MediaType, CaptureMediaTypes.Video));
			var audio = _testContext.Render<MediaFileCapture>(parameters => parameters
				.Add(p => p.MediaType, CaptureMediaTypes.Audio));
			var any = _testContext.Render<MediaFileCapture>(parameters => parameters
				.Add(p => p.MediaType, CaptureMediaTypes.Any));

			Assert.AreEqual("video/*", video.Find("input").GetAttribute("accept"));
			Assert.AreEqual("audio/*", audio.Find("input").GetAttribute("accept"));
			Assert.IsFalse(any.Find("input").HasAttribute("accept"));
		}

		[TestMethod]
		public void MediaFileCapture_should_render_capture_attribute_for_facing_modes()
		{
			var user = _testContext.Render<MediaFileCapture>(parameters => parameters
				.Add(p => p.CaptureMode, CameraFacingModes.User));
			var environment = _testContext.Render<MediaFileCapture>(parameters => parameters
				.Add(p => p.CaptureMode, CameraFacingModes.Environment));

			Assert.AreEqual("user", user.Find("input").GetAttribute("capture"));
			Assert.AreEqual("environment", environment.Find("input").GetAttribute("capture"));
		}

		[TestMethod]
		public void MediaFileCapture_should_render_multiple_disabled_and_custom_attributes()
		{
			var rendered = _testContext.Render<MediaFileCapture>(parameters => parameters
				.Add(p => p.Multiple, true)
				.Add(p => p.Disabled, true)
				.Add(p => p.Class, "my-class")
				.Add(p => p.Style, "width: 100px;")
				.AddUnmatched("title", "text"));

			var input = rendered.Find("input");

			Assert.IsTrue(input.HasAttribute("multiple"));
			Assert.IsTrue(input.HasAttribute("disabled"));
			Assert.AreEqual("my-class", input.GetAttribute("class"));
			Assert.AreEqual("width: 100px;", input.GetAttribute("style"));
			Assert.AreEqual("text", input.GetAttribute("title"));
		}
	}
}
