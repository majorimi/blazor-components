using System.Threading.Tasks;

using Bunit;

using Majorsoft.Blazor.Components.CommonTestsBase;
using Majorsoft.Blazor.Components.Media.Capture;
using Majorsoft.Blazor.Components.Media.Players;
using Majorsoft.Blazor.Components.Media.Visualization;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Majorsoft.Blazor.Components.Media.Tests.Visualization
{
	[TestClass]
	public class AudioVisualizerTest : ComponentsTestBase<AudioVisualizer>
	{
		private BunitJSModuleInterop _module = default!;

		[TestInitialize]
		public void Init()
		{
			var captureLogger = new Mock<ILogger<MediaCaptureBase>>();
			_testContext.Services.Add(new ServiceDescriptor(typeof(ILogger<MediaCaptureBase>), captureLogger.Object));
			var playerLogger = new Mock<ILogger<MediaPlayerBase>>();
			_testContext.Services.Add(new ServiceDescriptor(typeof(ILogger<MediaPlayerBase>), playerLogger.Object));

			_testContext.JSInterop.Mode = JSRuntimeMode.Loose;
			_module = _testContext.JSInterop.SetupModule("./_content/Majorsoft.Blazor.Components.Media/media.js");
			_module.Mode = JSRuntimeMode.Loose;
		}

		[TestMethod]
		public void AudioVisualizer_should_render_canvas_element_with_attributes()
		{
			var rendered = _testContext.Render<AudioVisualizer>(parameters => parameters
				.Add(p => p.Width, 640)
				.Add(p => p.Height, 120)
				.Add(p => p.Class, "my-class")
				.Add(p => p.Style, "border: 1px solid red;")
				.AddUnmatched("title", "text"));

			var canvas = rendered.Find("canvas");

			Assert.AreEqual("640", canvas.GetAttribute("width"));
			Assert.AreEqual("120", canvas.GetAttribute("height"));
			Assert.AreEqual("my-class", canvas.GetAttribute("class"));
			Assert.AreEqual("border: 1px solid red;", canvas.GetAttribute("style"));
			Assert.AreEqual("text", canvas.GetAttribute("title"));
		}

		[TestMethod]
		public void AudioVisualizer_should_render_percent_dimensions_as_css_size()
		{
			var rendered = _testContext.Render<AudioVisualizer>(parameters => parameters
				.Add(p => p.Width, 100)
				.Add(p => p.Height, 50)
				.Add(p => p.IsVisualizerDimensionInPixels, false)
				.Add(p => p.Style, "border: 1px solid red;"));

			var canvas = rendered.Find("canvas");

			//HTML width/height attributes are px by spec, in % mode the size is CSS only.
			Assert.IsFalse(canvas.HasAttribute("width"));
			Assert.IsFalse(canvas.HasAttribute("height"));
			Assert.AreEqual("width: 100%; height: 50%; border: 1px solid red;", canvas.GetAttribute("style"));
		}

		[TestMethod]
		public async Task AudioVisualizer_should_fail_to_start_without_source()
		{
			string? error = null;
			var rendered = _testContext.Render<AudioVisualizer>(parameters => parameters
				.Add(p => p.OnVisualizerError, message => { error = message; }));

			var started = await rendered.InvokeAsync(() => rendered.Instance.StartAsync());

			Assert.IsFalse(started);
			Assert.IsFalse(rendered.Instance.IsActive);
			StringAssert.Contains(error, "no CaptureSource or PlayerSource");
		}

		[TestMethod]
		public void AudioVisualizer_should_report_error_when_both_sources_set()
		{
			var recorder = _testContext.Render<AudioRecorder>();
			var player = _testContext.Render<AudioPlayer>();

			string? error = null;
			_testContext.Render<AudioVisualizer>(parameters => parameters
				.Add(p => p.CaptureSource, recorder.Instance)
				.Add(p => p.PlayerSource, player.Instance)
				.Add(p => p.OnVisualizerError, message => { error = message; }));

			StringAssert.Contains(error, "not both");
		}

		[TestMethod]
		public async Task AudioVisualizer_should_auto_start_when_capture_is_already_active()
		{
			_module.Setup<CaptureStartResult>("startCapture", _ => true)
				.SetResult(new CaptureStartResult { Id = 2 });
			_module.Setup<VisualizerStartResult>("startVisualizer", _ => true)
				.SetResult(new VisualizerStartResult { Id = 5 });

			var recorder = _testContext.Render<AudioRecorder>();
			await recorder.InvokeAsync(() => recorder.Instance.OpenMicrophoneAsync());

			var rendered = _testContext.Render<AudioVisualizer>(parameters => parameters
				.Add(p => p.CaptureSource, recorder.Instance));

			Assert.IsTrue(rendered.Instance.IsActive);
			_module.VerifyInvoke("startVisualizer");
		}

		[TestMethod]
		public async Task AudioVisualizer_should_auto_start_when_capture_opens_later()
		{
			_module.Setup<CaptureStartResult>("startCapture", _ => true)
				.SetResult(new CaptureStartResult { Id = 2 });
			_module.Setup<VisualizerStartResult>("startVisualizer", _ => true)
				.SetResult(new VisualizerStartResult { Id = 5 });

			var recorder = _testContext.Render<AudioRecorder>();
			var rendered = _testContext.Render<AudioVisualizer>(parameters => parameters
				.Add(p => p.CaptureSource, recorder.Instance));

			Assert.IsFalse(rendered.Instance.IsActive);

			await recorder.InvokeAsync(() => recorder.Instance.OpenMicrophoneAsync());

			Assert.IsTrue(rendered.Instance.IsActive);
		}

		[TestMethod]
		public async Task AudioVisualizer_should_attach_source_set_after_first_render()
		{
			//@ref captured sources are null during the parent's first render and only arrive
			//with a later parameter update, the visualizer must attach then.
			_module.Setup<CaptureStartResult>("startCapture", _ => true)
				.SetResult(new CaptureStartResult { Id = 2 });
			_module.Setup<VisualizerStartResult>("startVisualizer", _ => true)
				.SetResult(new VisualizerStartResult { Id = 5 });

			var recorder = _testContext.Render<AudioRecorder>();
			await recorder.InvokeAsync(() => recorder.Instance.OpenMicrophoneAsync());

			var rendered = _testContext.Render<AudioVisualizer>();
			Assert.IsFalse(rendered.Instance.IsActive);

			rendered.Render(parameters => parameters
				.Add(p => p.CaptureSource, recorder.Instance));

			Assert.IsTrue(rendered.Instance.IsActive);
		}

		[TestMethod]
		public async Task AudioVisualizer_should_deactivate_when_capture_stops()
		{
			_module.Setup<CaptureStartResult>("startCapture", _ => true)
				.SetResult(new CaptureStartResult { Id = 2 });
			_module.Setup<VisualizerStartResult>("startVisualizer", _ => true)
				.SetResult(new VisualizerStartResult { Id = 5 });

			var recorder = _testContext.Render<AudioRecorder>();
			await recorder.InvokeAsync(() => recorder.Instance.OpenMicrophoneAsync());

			var rendered = _testContext.Render<AudioVisualizer>(parameters => parameters
				.Add(p => p.CaptureSource, recorder.Instance));
			Assert.IsTrue(rendered.Instance.IsActive);

			await recorder.InvokeAsync(() => recorder.Instance.StopCaptureAsync());

			Assert.IsFalse(rendered.Instance.IsActive);
		}

		[TestMethod]
		public async Task AudioVisualizer_should_validate_fft_size()
		{
			_module.Setup<CaptureStartResult>("startCapture", _ => true)
				.SetResult(new CaptureStartResult { Id = 2 });

			var recorder = _testContext.Render<AudioRecorder>();
			await recorder.InvokeAsync(() => recorder.Instance.OpenMicrophoneAsync());

			string? error = null;
			var rendered = _testContext.Render<AudioVisualizer>(parameters => parameters
				.Add(p => p.CaptureSource, recorder.Instance)
				.Add(p => p.AutoStart, false)
				.Add(p => p.FftSize, 1000)
				.Add(p => p.OnVisualizerError, message => { error = message; }));

			var started = await rendered.InvokeAsync(() => rendered.Instance.StartAsync());

			Assert.IsFalse(started);
			StringAssert.Contains(error, "power of 2");
		}

		[TestMethod]
		public async Task AudioVisualizer_should_validate_decibel_range()
		{
			_module.Setup<CaptureStartResult>("startCapture", _ => true)
				.SetResult(new CaptureStartResult { Id = 2 });

			var recorder = _testContext.Render<AudioRecorder>();
			await recorder.InvokeAsync(() => recorder.Instance.OpenMicrophoneAsync());

			string? error = null;
			var rendered = _testContext.Render<AudioVisualizer>(parameters => parameters
				.Add(p => p.CaptureSource, recorder.Instance)
				.Add(p => p.AutoStart, false)
				.Add(p => p.MinDecibels, -10)
				.Add(p => p.MaxDecibels, -90)
				.Add(p => p.OnVisualizerError, message => { error = message; }));

			var started = await rendered.InvokeAsync(() => rendered.Instance.StartAsync());

			Assert.IsFalse(started);
			StringAssert.Contains(error, "MinDecibels");
		}

		[TestMethod]
		public async Task AudioVisualizer_should_stop_and_clear_state()
		{
			_module.Setup<CaptureStartResult>("startCapture", _ => true)
				.SetResult(new CaptureStartResult { Id = 2 });
			_module.Setup<VisualizerStartResult>("startVisualizer", _ => true)
				.SetResult(new VisualizerStartResult { Id = 5 });

			var recorder = _testContext.Render<AudioRecorder>();
			await recorder.InvokeAsync(() => recorder.Instance.OpenMicrophoneAsync());

			var rendered = _testContext.Render<AudioVisualizer>(parameters => parameters
				.Add(p => p.CaptureSource, recorder.Instance));
			Assert.IsTrue(rendered.Instance.IsActive);

			await rendered.InvokeAsync(() => rendered.Instance.StopAsync());

			Assert.IsFalse(rendered.Instance.IsActive);
			_module.VerifyInvoke("stopVisualizer");
		}

		[TestMethod]
		public async Task AudioVisualizer_should_report_spectrum_bands_from_js_callback()
		{
			double[]? bands = null;
			var rendered = _testContext.Render<AudioVisualizer>(parameters => parameters
				.Add(p => p.OnSpectrumChanged, value => { bands = value; }));

			await rendered.InvokeAsync(() => rendered.Instance.SpectrumChangedAsync(new[] { 0.1, 0.5, 1.0 }));

			Assert.IsNotNull(bands);
			Assert.AreEqual(3, bands.Length);
			Assert.AreEqual(0.5, bands[1]);
		}
	}
}
