using System.Linq;
using System.Threading.Tasks;

using Bunit;

using Majorsoft.Blazor.Components.CommonTestsBase;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Majorsoft.Blazor.Components.Media.Tests
{
	[TestClass]
	public class MediaDeviceServiceTest : ComponentsTestBase
	{
		private BunitJSModuleInterop _module = default!;
		private MediaDeviceService _service = default!;

		[TestInitialize]
		public void Init()
		{
			_testContext.JSInterop.Mode = JSRuntimeMode.Loose;
			_module = _testContext.JSInterop.SetupModule("./_content/Majorsoft.Blazor.Components.Media/media.js");
			_module.Mode = JSRuntimeMode.Loose;

			_service = new MediaDeviceService(_testContext.JSInterop.JSRuntime);
		}

		[TestMethod]
		public async Task MediaDeviceService_should_check_capture_support()
		{
			_module.Setup<bool>("isCaptureSupported").SetResult(true);

			Assert.IsTrue(await _service.IsCaptureSupportedAsync());
		}

		[TestMethod]
		public async Task MediaDeviceService_should_check_MediaRecorder_support()
		{
			_module.Setup<bool>("isMediaRecorderSupported").SetResult(false);

			Assert.IsFalse(await _service.IsMediaRecorderSupportedAsync());
		}

		[TestMethod]
		public async Task MediaDeviceService_should_list_devices()
		{
			_module.Setup<MediaDeviceInfo[]>("getDevices").SetResult(new[]
			{
				new MediaDeviceInfo { DeviceId = "cam1", Kind = "videoinput", Label = "Front camera" },
				new MediaDeviceInfo { DeviceId = "mic1", Kind = "audioinput", Label = "Microphone" }
			});

			var devices = (await _service.GetMediaDevicesAsync()).ToList();

			Assert.AreEqual(2, devices.Count);
			Assert.IsTrue(devices[0].IsVideoInput);
			Assert.IsTrue(devices[1].IsAudioInput);
			Assert.AreEqual("Front camera", devices[0].Label);
		}

		[TestMethod]
		public async Task MediaDeviceService_should_request_permissions()
		{
			_module.Setup<string?>("requestPermissions", _ => true).SetResult(null);

			var error = await _service.RequestPermissionsAsync(audio: true, video: true);

			Assert.IsNull(error);
			_module.VerifyInvoke("requestPermissions");
		}

		[TestMethod]
		public async Task MediaDeviceService_should_return_permission_error()
		{
			_module.Setup<string?>("requestPermissions", _ => true)
				.SetResult("NotAllowedError: Permission denied");

			var error = await _service.RequestPermissionsAsync();

			StringAssert.Contains(error, "NotAllowedError");
		}

		[TestMethod]
		public async Task MediaDeviceService_should_list_supported_mime_types()
		{
			_module.Setup<string[]>("getSupportedMimeTypes", _ => true)
				.SetResult(new[] { "video/webm;codecs=vp9,opus", "video/webm" });

			var types = (await _service.GetSupportedMimeTypesAsync(video: true)).ToList();

			Assert.AreEqual(2, types.Count);
			StringAssert.Contains(types[0], "vp9");
		}
	}
}
