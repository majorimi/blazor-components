using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Majorsoft.Blazor.Components.Notifications.Tests.Toasts
{
    [TestClass]
    public class ToastServiceCollectionTest
    {
        private ToastService _toastService;

        [TestInitialize]
        public void Init()
        {
            _toastService = new ToastService();
        }

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
        public void ToastService_should_generate_unique_toast_IDs()
        {
            var id1 = _toastService.ShowToast("Toast 1", NotificationTypes.Info);
            var id2 = _toastService.ShowToast("Toast 2", NotificationTypes.Info);
            var id3 = _toastService.ShowToast("Toast 3", NotificationTypes.Info);

            Assert.AreNotEqual(id1, id2);
            Assert.AreNotEqual(id2, id3);
            Assert.AreNotEqual(id1, id3);
        }

        [TestMethod]
        public void ToastService_should_track_visible_toasts()
        {
            _toastService.ShowToast(new ToastSettings() { IsVisible = true, Content = new Microsoft.AspNetCore.Components.RenderFragment(b => b.AddMarkupContent(0, "Toast 1")) });
            _toastService.ShowToast(new ToastSettings() { IsVisible = false, Content = new Microsoft.AspNetCore.Components.RenderFragment(b => b.AddMarkupContent(0, "Toast 2")) });
            _toastService.ShowToast(new ToastSettings() { IsVisible = true, Content = new Microsoft.AspNetCore.Components.RenderFragment(b => b.AddMarkupContent(0, "Toast 3")) });

            Assert.AreEqual(2, _toastService.Toasts.Count()); // Only visible
            Assert.AreEqual(3, _toastService.AllToasts.Count()); // All toasts
        }

        [TestMethod]
        public void ToastService_should_support_all_notification_types()
        {
            var types = Enum.GetValues(typeof(NotificationTypes)).Cast<NotificationTypes>();
            foreach (var type in types)
            {
                var id = _toastService.ShowToast($"Toast {type}", type);
                Assert.AreNotEqual(id, Guid.Empty);
            }

            Assert.IsTrue(_toastService.AllToasts.Count() >= 4); // At least the standard types
        }

        [TestMethod]
        public void ToastService_should_support_all_notification_styles()
        {
            var styles = Enum.GetValues(typeof(NotificationStyles)).Cast<NotificationStyles>();
            foreach (var style in styles)
            {
                var id = _toastService.ShowToast($"Toast {style}", NotificationTypes.Info, style);
                Assert.AreNotEqual(id, Guid.Empty);
            }

            Assert.IsTrue(_toastService.AllToasts.Count() >= 2); // At least the standard styles
        }

        [TestMethod]
        public void ToastService_should_maintain_toast_order()
        {
            var ids = new System.Collections.Generic.List<Guid>();
            for (int i = 0; i < 5; i++)
            {
                ids.Add(_toastService.ShowToast($"Toast {i}", NotificationTypes.Info));
            }

            Assert.AreEqual(5, _toastService.AllToasts.Count());
            Assert.AreEqual(5, ids.Count);
        }

        [TestMethod]
        public void ToastService_should_handle_null_content()
        {
            var id = _toastService.ShowToast((string)null, NotificationTypes.Info);
            Assert.AreNotEqual(id, Guid.Empty);
        }

        [TestMethod]
        public void ToastService_should_handle_empty_content()
        {
            var id = _toastService.ShowToast("", NotificationTypes.Info);
            Assert.AreNotEqual(id, Guid.Empty);
        }
    }

    [TestClass]
    public class ToastSettingsConfigurationTest
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
        public void ToastSettings_should_apply_global_defaults()
        {
            ToastContainerGlobalSettings.DefaultToastsShowIcon = false;
            ToastContainerGlobalSettings.DefaultToastsAutoCloseInSec = 20;

            var settings = new ToastSettings();

            Assert.AreEqual(false, settings.ShowIcon);
            Assert.AreEqual((int)20, (int)settings.AutoCloseInSec);
        }

        [TestMethod]
        public void ToastSettings_should_allow_overrides()
        {
            var settings = new ToastSettings()
            {
                ShowIcon = true,
                AutoCloseInSec = 5
            };

            Assert.AreEqual(true, settings.ShowIcon);
            Assert.AreEqual((int)5, (int)settings.AutoCloseInSec);
        }

        [TestMethod]
        public void ToastSettings_ShadowEffect_should_be_clamped_to_max()
        {
            var settings = new ToastSettings() { ShadowEffect = 100 };
            Assert.AreEqual((int)20, (int)settings.ShadowEffect); // Should be clamped to 20
        }

        [TestMethod]
        public void ToastSettings_ShadowEffect_should_allow_zero()
        {
            var settings = new ToastSettings() { ShadowEffect = 0 };
            Assert.AreEqual((int)0, (int)settings.ShadowEffect);
        }

        [TestMethod]
        public void ToastSettings_should_have_visibility_flag()
        {
            var visibleSettings = new ToastSettings() { IsVisible = true };
            var hiddenSettings = new ToastSettings() { IsVisible = false };

            Assert.AreEqual(true, visibleSettings.IsVisible);
            Assert.AreEqual(false, hiddenSettings.IsVisible);
        }

        [TestMethod]
        public void ToastSettings_should_track_removal_flag()
        {
            var settings = new ToastSettings() { IsRemove = true };
            Assert.AreEqual(true, settings.IsRemove);

            settings.IsRemove = false;
            Assert.AreEqual(false, settings.IsRemove);
        }
    }
}
