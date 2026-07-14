using System.Threading.Tasks;

using Bunit;

using Majorsoft.Blazor.Components.CommonTestsBase;
using Majorsoft.Blazor.Components.Media.Capture;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Majorsoft.Blazor.Components.Media.Tests.Capture
{
	[TestClass]
	public class CameraCaptureTest : ComponentsTestBase<MediaCaptureBase>
	{
		private BunitJSModuleInterop _module = default!;

		[TestInitialize]
		public void Init()
		{
			_testContext.JSInterop.Mode = JSRuntimeMode.Loose;
			_module = _testContext.JSInterop.SetupModule("./_content/Majorsoft.Blazor.Components.Media/media.js");
			_module.Mode = JSRuntimeMode.Loose;
		}

		[TestMethod]
		public void CameraCapture_should_render_muted_autoplay_video_with_pixel_dimensions()
		{
			var rendered = _testContext.Render<CameraCapture>(parameters => parameters
				.Add(p => p.Width, 800)
				.Add(p => p.Height, 600)
				.Add(p => p.Class, "my-class")
				.AddUnmatched("title", "text"));

			var video = rendered.Find("video");

			Assert.AreEqual("800", video.GetAttribute("width"));
			Assert.AreEqual("600", video.GetAttribute("height"));
			Assert.IsTrue(video.HasAttribute("autoplay"));
			Assert.IsTrue(video.HasAttribute("muted"));
			Assert.IsTrue(video.HasAttribute("playsinline"));
			Assert.AreEqual("my-class", video.GetAttribute("class"));
			Assert.AreEqual("text", video.GetAttribute("title"));
		}

		[TestMethod]
		public void CameraCapture_should_render_percent_dimensions_as_css_style()
		{
			var rendered = _testContext.Render<CameraCapture>(parameters => parameters
				.Add(p => p.Width, 50)
				.Add(p => p.Height, 100)
				.Add(p => p.IsCameraDimensionInPixels, false)
				.Add(p => p.Style, "border: 1px solid red;"));

			var video = rendered.Find("video");

			Assert.IsFalse(video.HasAttribute("width"));
			Assert.IsFalse(video.HasAttribute("height"));

			var style = video.GetAttribute("style");
			StringAssert.Contains(style, "width: 50%");
			StringAssert.Contains(style, "height: 100%");
			StringAssert.Contains(style, "border: 1px solid red;");
		}

		[TestMethod]
		public async Task CameraCapture_should_start_and_stop_camera_with_events()
		{
			_module.Setup<CaptureStartResult>("startCapture", _ => true)
				.SetResult(new CaptureStartResult { Id = 1 });

			var started = false;
			var stopped = false;

			var rendered = _testContext.Render<CameraCapture>(parameters => parameters
				.Add(p => p.OnCameraStarted, () => { started = true; })
				.Add(p => p.OnCameraStopped, () => { stopped = true; }));

			var camera = rendered.Instance;
			Assert.IsFalse(camera.IsCaptureActive);

			var result = await rendered.InvokeAsync(() => camera.StartCameraAsync());

			Assert.IsTrue(result);
			Assert.IsTrue(started);
			Assert.IsTrue(camera.IsCaptureActive);

			await rendered.InvokeAsync(() => camera.StopCaptureAsync());

			Assert.IsTrue(stopped);
			Assert.IsFalse(camera.IsCaptureActive);
			_module.VerifyInvoke("stopCapture");
		}

		[TestMethod]
		public async Task CameraCapture_should_report_error_when_start_fails()
		{
			_module.Setup<CaptureStartResult>("startCapture", _ => true)
				.SetResult(new CaptureStartResult { Id = 0, Error = "NotAllowedError: Permission denied" });

			var error = "";
			var rendered = _testContext.Render<CameraCapture>(parameters => parameters
				.Add(p => p.OnCaptureError, message => { error = message; }));

			var result = await rendered.InvokeAsync(() => rendered.Instance.StartCameraAsync());

			Assert.IsFalse(result);
			Assert.IsFalse(rendered.Instance.IsCaptureActive);
			StringAssert.Contains(error, "NotAllowedError");
		}

		[TestMethod]
		public async Task CameraCapture_should_take_photo_and_fire_event()
		{
			_module.Setup<CaptureStartResult>("startCapture", _ => true)
				.SetResult(new CaptureStartResult { Id = 1 });
			_module.Setup<MediaPhotoInfo?>("takePhoto", _ => true)
				.SetResult(new MediaPhotoInfo { Url = "blob:photo", Width = 640, Height = 480, MimeType = "image/png", SizeBytes = 1024 });

			MediaPhotoInfo? photoFromEvent = null;
			var rendered = _testContext.Render<CameraCapture>(parameters => parameters
				.Add(p => p.OnPhotoTaken, photo => { photoFromEvent = photo; }));

			await rendered.InvokeAsync(() => rendered.Instance.StartCameraAsync());
			var photo = await rendered.InvokeAsync(() => rendered.Instance.TakePhotoAsync("image/png"));

			Assert.IsNotNull(photo);
			Assert.AreEqual("blob:photo", photo.Url);
			Assert.AreEqual(640, photo.Width);
			Assert.IsNotNull(photoFromEvent);
			Assert.AreEqual("blob:photo", photoFromEvent.Url);
		}

		[TestMethod]
		public async Task CameraCapture_should_track_recording_state_from_js_callbacks()
		{
			_module.Setup<CaptureStartResult>("startCapture", _ => true)
				.SetResult(new CaptureStartResult { Id = 1 });

			RecordingStates? lastState = null;
			MediaRecordingInfo? finished = null;

			var rendered = _testContext.Render<CameraCapture>(parameters => parameters
				.Add(p => p.OnRecordingStateChanged, state => { lastState = state; })
				.Add(p => p.OnRecordingFinished, recording => { finished = recording; }));

			var camera = rendered.Instance;
			await rendered.InvokeAsync(() => camera.StartCameraAsync());

			Assert.AreEqual(RecordingStates.Inactive, camera.RecordingState);

			//JS invokable callbacks simulate the MediaRecorder events.
			await rendered.InvokeAsync(() => camera.RecordingStateChangedAsync("recording"));
			Assert.AreEqual(RecordingStates.Recording, camera.RecordingState);
			Assert.AreEqual(RecordingStates.Recording, lastState);

			await rendered.InvokeAsync(() => camera.RecordingStateChangedAsync("paused"));
			Assert.AreEqual(RecordingStates.Paused, camera.RecordingState);

			await rendered.InvokeAsync(() => camera.RecordingFinishedAsync(
				new MediaRecordingInfo { Url = "blob:rec", MimeType = "video/webm", SizeBytes = 2048, DurationMs = 1500 }));

			Assert.AreEqual(RecordingStates.Inactive, camera.RecordingState);
			Assert.IsNotNull(finished);
			Assert.AreEqual("blob:rec", finished.Url);
			Assert.IsNotNull(camera.LastRecording);
			Assert.AreEqual("video/webm", camera.LastRecording.MimeType);
		}

		[TestMethod]
		public async Task CameraCapture_should_report_js_capture_errors()
		{
			var error = "";
			var rendered = _testContext.Render<CameraCapture>(parameters => parameters
				.Add(p => p.OnCaptureError, message => { error = message; }));

			await rendered.InvokeAsync(() => rendered.Instance.CaptureErrorAsync("Media stream track ended: video"));

			StringAssert.Contains(error, "track ended");
		}
	}
}
