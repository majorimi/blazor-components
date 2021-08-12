
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Majorsoft.Blazor.Components.Notifications.Tests.Toasts
{
	[TestClass]
	public class ToastSettingsTest
	{
		[TestCleanup]
		public void Cleanup()
		{
			ToastContainerGlobalSettings.DefaultToastsShowIcon = true;
			ToastContainerGlobalSettings.DefaultToastsNotificationStyle = NotificationStyles.Normal;
			ToastContainerGlobalSettings.DefaultToastsShowCloseCountdownProgress = true;
			ToastContainerGlobalSettings.DefaultToastsShowCloseButton = true;
			ToastContainerGlobalSettings.DefaultToastsAutoCloseInSec = 10;
			ToastContainerGlobalSettings.DefaultToastsShadowEffect = 5;
		}

		[TestMethod]
		public void ToastSettings_should_have_default_values()
		{
			var settings = new ToastSettings();

			Assert.AreEqual(ToastContainerGlobalSettings.DefaultToastsShowIcon, settings.ShowIcon);
			Assert.AreEqual(ToastContainerGlobalSettings.DefaultToastsAutoCloseInSec, settings.AutoCloseInSec);
			Assert.AreEqual(ToastContainerGlobalSettings.DefaultToastsShowCloseCountdownProgress, settings.ShowCloseCountdownProgress);
			Assert.AreEqual(ToastContainerGlobalSettings.DefaultToastsShadowEffect, settings.ShadowEffect);
		}

		[TestMethod]
		public void ToastSettings_should_have_changed_values()
		{
			ToastContainerGlobalSettings.DefaultToastsShowIcon = false;
			ToastContainerGlobalSettings.DefaultToastsAutoCloseInSec = 77;
			ToastContainerGlobalSettings.DefaultToastsShowCloseCountdownProgress = false;
			ToastContainerGlobalSettings.DefaultToastsShadowEffect = 88;

			var settings = new ToastSettings();

			Assert.AreEqual(ToastContainerGlobalSettings.DefaultToastsShowIcon, settings.ShowIcon);
			Assert.AreEqual(ToastContainerGlobalSettings.DefaultToastsAutoCloseInSec, settings.AutoCloseInSec);
			Assert.AreEqual(ToastContainerGlobalSettings.DefaultToastsShowCloseCountdownProgress, settings.ShowCloseCountdownProgress);
			Assert.AreEqual(ToastContainerGlobalSettings.DefaultToastsShadowEffect, settings.ShadowEffect);
		}

		[TestMethod]
		public void ToastSettings_should_set_ShadowEffect_range()
		{
			var settings = new ToastSettings() { ShadowEffect = 0 };
			var settings2 = new ToastSettings() { ShadowEffect = 30 };

			Assert.AreEqual((uint)0, settings.ShadowEffect);
			Assert.AreEqual((uint)20, settings2.ShadowEffect);
		}
	}
}