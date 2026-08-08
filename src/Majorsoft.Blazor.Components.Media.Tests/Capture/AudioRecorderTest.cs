using System.Linq;
using System.Threading.Tasks;

using Bunit;

using Majorsoft.Blazor.Components.CommonTestsBase;
using Majorsoft.Blazor.Components.Media.Capture;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Majorsoft.Blazor.Components.Media.Tests.Capture
{
	[TestClass]
	public class AudioRecorderTest : ComponentsTestBase<MediaCaptureBase>
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
		public void AudioRecorder_should_render_no_html()
		{
			var rendered = _testContext.Render<AudioRecorder>();

			Assert.AreEqual("", rendered.Markup.Trim());
		}

		[TestMethod]
		public async Task AudioRecorder_should_open_microphone_and_start_level_meter()
		{
			_module.Setup<CaptureStartResult>("startCapture", _ => true)
				.SetResult(new CaptureStartResult { Id = 2 });

			var opened = false;
			var rendered = _testContext.Render<AudioRecorder>(parameters => parameters
				.Add(p => p.OnMicrophoneOpened, () => { opened = true; }));

			var result = await rendered.InvokeAsync(() => rendered.Instance.OpenMicrophoneAsync());

			Assert.IsTrue(result);
			Assert.IsTrue(opened);
			Assert.IsTrue(rendered.Instance.IsCaptureActive);
			_module.VerifyInvoke("startLevelMeter"); //EnableLevelMeter default is true
		}

		[TestMethod]
		public async Task AudioRecorder_should_not_start_level_meter_when_disabled()
		{
			_module.Setup<CaptureStartResult>("startCapture", _ => true)
				.SetResult(new CaptureStartResult { Id = 2 });

			var rendered = _testContext.Render<AudioRecorder>(parameters => parameters
				.Add(p => p.EnableLevelMeter, false));

			await rendered.InvokeAsync(() => rendered.Instance.OpenMicrophoneAsync());

			Assert.AreEqual(0, _module.Invocations.Count(i => i.Identifier == "startLevelMeter"));
		}

		[TestMethod]
		public async Task AudioRecorder_should_close_microphone_with_event()
		{
			_module.Setup<CaptureStartResult>("startCapture", _ => true)
				.SetResult(new CaptureStartResult { Id = 2 });

			var closed = false;
			var rendered = _testContext.Render<AudioRecorder>(parameters => parameters
				.Add(p => p.OnMicrophoneClosed, () => { closed = true; }));

			await rendered.InvokeAsync(() => rendered.Instance.OpenMicrophoneAsync());
			await rendered.InvokeAsync(() => rendered.Instance.StopCaptureAsync());

			Assert.IsTrue(closed);
			Assert.IsFalse(rendered.Instance.IsCaptureActive);
		}

		[TestMethod]
		public async Task AudioRecorder_should_report_audio_level_from_js_callback()
		{
			var level = 0d;
			var rendered = _testContext.Render<AudioRecorder>(parameters => parameters
				.Add(p => p.OnAudioLevelChanged, value => { level = value; }));

			await rendered.InvokeAsync(() => rendered.Instance.AudioLevelChangedAsync(0.42));

			Assert.AreEqual(0.42, level);
		}
	}
}
