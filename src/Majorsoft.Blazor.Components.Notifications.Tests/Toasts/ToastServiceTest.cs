using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Majorsoft.Blazor.Components.Notifications.Tests.Toasts
{
	[TestClass]
	public class ToastServiceTest
	{
		private ToastService _toastService;

		[TestInitialize]
		public void InitGenericBase()
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
		public void ToastService_GlobalSettings_is_not_null()
		{
			Assert.IsNotNull(_toastService.GlobalSettings);
		}

		[TestMethod]
		public void ToastService_Toasts_is_not_null()
		{
			Assert.IsNotNull(_toastService.Toasts);
			Assert.AreEqual(0, _toastService.Toasts.Count());
		}

		[TestMethod]
		public void ToastService_correct_Toasts_count()
		{
			_toastService.ShowToast(new ToastSettings());
			_toastService.ShowToast(new ToastSettings());
			_toastService.ShowToast(new ToastSettings());

			Assert.IsNotNull(_toastService.Toasts);
			Assert.AreEqual(3, _toastService.AllToasts.Count());
			Assert.AreEqual(3, _toastService.Toasts.Count());
		}

		[TestMethod]
		public void ToastService_Toasts_count_should_Clear()
		{
			_toastService.ShowToast(new ToastSettings() { IsVisible = false });
			_toastService.ShowToast(new ToastSettings() { IsVisible = false });
			_toastService.ShowToast(new ToastSettings() { IsVisible = false });

			Assert.IsNotNull(_toastService.Toasts);
			Assert.AreEqual(0, _toastService.AllToasts.Count());
			Assert.AreEqual(0, _toastService.Toasts.Count());
		}

		[TestMethod]
		public void ToastService_should_ShowToast_msg_with_global_settings()
		{
			ToastContainerGlobalSettings.DefaultToastsShowIcon = false;
			ToastContainerGlobalSettings.DefaultToastsNotificationStyle = NotificationStyles.Outlined;
			ToastContainerGlobalSettings.DefaultToastsShowCloseCountdownProgress = false;
			ToastContainerGlobalSettings.DefaultToastsShowCloseButton = false;
			ToastContainerGlobalSettings.DefaultToastsAutoCloseInSec = 55;
			ToastContainerGlobalSettings.DefaultToastsShadowEffect = 22;

			var id1 = _toastService.ShowToast("msg1", NotificationTypes.Danger, NotificationStyles.Strong);
			var id2 = _toastService.ShowToast("msg2", NotificationTypes.Info);

			Assert.IsNotNull(_toastService.Toasts);
			Assert.AreEqual(2, _toastService.AllToasts.Count());
			Assert.AreEqual(2, _toastService.Toasts.Count());
			Assert.AreNotEqual(Guid.Empty, id1);
			Assert.AreNotEqual(Guid.Empty, id2);
			
			Assert.AreEqual(true, _toastService.Toasts.ElementAt(0).IsVisible);
			Assert.AreEqual(false, _toastService.Toasts.ElementAt(0).IsRemove);
			Assert.IsNotNull(_toastService.Toasts.ElementAt(0).Content);
			Assert.AreEqual(NotificationStyles.Strong, _toastService.Toasts.ElementAt(0).NotificationStyle);
			Assert.AreEqual(NotificationTypes.Danger, _toastService.Toasts.ElementAt(0).Type);
			Assert.AreEqual(ToastContainerGlobalSettings.DefaultToastsShowIcon, _toastService.Toasts.ElementAt(0).ShowIcon);
			Assert.AreEqual(NotificationStyles.Strong, _toastService.Toasts.ElementAt(0).NotificationStyle);
			Assert.AreEqual(ToastContainerGlobalSettings.DefaultToastsShowCloseCountdownProgress, _toastService.Toasts.ElementAt(0).ShowCloseCountdownProgress);
			Assert.AreEqual(ToastContainerGlobalSettings.DefaultToastsShowCloseButton, _toastService.Toasts.ElementAt(0).ShowCloseButton);
			Assert.AreEqual(ToastContainerGlobalSettings.DefaultToastsAutoCloseInSec, _toastService.Toasts.ElementAt(0).AutoCloseInSec);
			Assert.AreEqual(ToastContainerGlobalSettings.DefaultToastsShadowEffect, _toastService.Toasts.ElementAt(0).ShadowEffect);

			Assert.AreEqual(true, _toastService.Toasts.ElementAt(1).IsVisible);
			Assert.AreEqual(false, _toastService.Toasts.ElementAt(1).IsRemove);
			Assert.IsNotNull(_toastService.Toasts.ElementAt(1).Content);
			Assert.AreEqual(NotificationStyles.Outlined, _toastService.Toasts.ElementAt(1).NotificationStyle);
			Assert.AreEqual(NotificationTypes.Info, _toastService.Toasts.ElementAt(1).Type);
			Assert.AreEqual(ToastContainerGlobalSettings.DefaultToastsShowIcon, _toastService.Toasts.ElementAt(1).ShowIcon);
			Assert.AreEqual(ToastContainerGlobalSettings.DefaultToastsNotificationStyle, _toastService.Toasts.ElementAt(1).NotificationStyle);
			Assert.AreEqual(ToastContainerGlobalSettings.DefaultToastsShowCloseCountdownProgress, _toastService.Toasts.ElementAt(1).ShowCloseCountdownProgress);
			Assert.AreEqual(ToastContainerGlobalSettings.DefaultToastsShowCloseButton, _toastService.Toasts.ElementAt(1).ShowCloseButton);
			Assert.AreEqual(ToastContainerGlobalSettings.DefaultToastsAutoCloseInSec, _toastService.Toasts.ElementAt(1).AutoCloseInSec);
			Assert.AreEqual(ToastContainerGlobalSettings.DefaultToastsShadowEffect, _toastService.Toasts.ElementAt(1).ShadowEffect);
		}

		[TestMethod]
		public void ToastService_should_ShowToast_Markup_with_global_settings()
		{
			ToastContainerGlobalSettings.DefaultToastsShowIcon = false;
			ToastContainerGlobalSettings.DefaultToastsNotificationStyle = NotificationStyles.Outlined;
			ToastContainerGlobalSettings.DefaultToastsShowCloseCountdownProgress = false;
			ToastContainerGlobalSettings.DefaultToastsShowCloseButton = false;
			ToastContainerGlobalSettings.DefaultToastsAutoCloseInSec = 55;
			ToastContainerGlobalSettings.DefaultToastsShadowEffect = 22;

			var id1 = _toastService.ShowToast(builder => builder.AddMarkupContent(0, "msg1"), NotificationTypes.Danger, NotificationStyles.Strong);
			var id2 = _toastService.ShowToast(builder => builder.AddMarkupContent(0, "msg2"), NotificationTypes.Info);

			Assert.IsNotNull(_toastService.Toasts);
			Assert.AreEqual(2, _toastService.AllToasts.Count());
			Assert.AreEqual(2, _toastService.Toasts.Count());
			Assert.AreNotEqual(Guid.Empty, id1);
			Assert.AreNotEqual(Guid.Empty, id2);

			Assert.AreEqual(true, _toastService.Toasts.ElementAt(0).IsVisible);
			Assert.AreEqual(false, _toastService.Toasts.ElementAt(0).IsRemove);
			Assert.IsNotNull(_toastService.Toasts.ElementAt(0).Content);
			Assert.AreEqual(NotificationStyles.Strong, _toastService.Toasts.ElementAt(0).NotificationStyle);
			Assert.AreEqual(NotificationTypes.Danger, _toastService.Toasts.ElementAt(0).Type);
			Assert.AreEqual(ToastContainerGlobalSettings.DefaultToastsShowIcon, _toastService.Toasts.ElementAt(0).ShowIcon);
			Assert.AreEqual(NotificationStyles.Strong, _toastService.Toasts.ElementAt(0).NotificationStyle);
			Assert.AreEqual(ToastContainerGlobalSettings.DefaultToastsShowCloseCountdownProgress, _toastService.Toasts.ElementAt(0).ShowCloseCountdownProgress);
			Assert.AreEqual(ToastContainerGlobalSettings.DefaultToastsShowCloseButton, _toastService.Toasts.ElementAt(0).ShowCloseButton);
			Assert.AreEqual(ToastContainerGlobalSettings.DefaultToastsAutoCloseInSec, _toastService.Toasts.ElementAt(0).AutoCloseInSec);
			Assert.AreEqual(ToastContainerGlobalSettings.DefaultToastsShadowEffect, _toastService.Toasts.ElementAt(0).ShadowEffect);

			Assert.AreEqual(true, _toastService.Toasts.ElementAt(1).IsVisible);
			Assert.AreEqual(false, _toastService.Toasts.ElementAt(1).IsRemove);
			Assert.IsNotNull(_toastService.Toasts.ElementAt(1).Content);
			Assert.AreEqual(NotificationStyles.Outlined, _toastService.Toasts.ElementAt(1).NotificationStyle);
			Assert.AreEqual(NotificationTypes.Info, _toastService.Toasts.ElementAt(1).Type);
			Assert.AreEqual(ToastContainerGlobalSettings.DefaultToastsShowIcon, _toastService.Toasts.ElementAt(1).ShowIcon);
			Assert.AreEqual(ToastContainerGlobalSettings.DefaultToastsNotificationStyle, _toastService.Toasts.ElementAt(1).NotificationStyle);
			Assert.AreEqual(ToastContainerGlobalSettings.DefaultToastsShowCloseCountdownProgress, _toastService.Toasts.ElementAt(1).ShowCloseCountdownProgress);
			Assert.AreEqual(ToastContainerGlobalSettings.DefaultToastsShowCloseButton, _toastService.Toasts.ElementAt(1).ShowCloseButton);
			Assert.AreEqual(ToastContainerGlobalSettings.DefaultToastsAutoCloseInSec, _toastService.Toasts.ElementAt(1).AutoCloseInSec);
			Assert.AreEqual(ToastContainerGlobalSettings.DefaultToastsShadowEffect, _toastService.Toasts.ElementAt(1).ShadowEffect);
		}

		[TestMethod]
		public void ToastService_should_ShowToast_ToastSettings_with_overwritten_global_settings()
		{
			ToastContainerGlobalSettings.DefaultToastsShowIcon = false;
			ToastContainerGlobalSettings.DefaultToastsNotificationStyle = NotificationStyles.Outlined;
			ToastContainerGlobalSettings.DefaultToastsShowCloseCountdownProgress = false;
			ToastContainerGlobalSettings.DefaultToastsShowCloseButton = false;
			ToastContainerGlobalSettings.DefaultToastsAutoCloseInSec = 55;
			ToastContainerGlobalSettings.DefaultToastsShadowEffect = 22;

			var id1 = _toastService.ShowToast(new ToastSettings()
			{
				Content = builder => builder.AddMarkupContent(0, "msg1"),
				Type = NotificationTypes.Primary,
				NotificationStyle = NotificationStyles.Strong,
				ShowIcon = true,
				ShowCloseButton = true,
				ShowCloseCountdownProgress = true,
				AutoCloseInSec = 11,
				ShadowEffect = 15,
			});
			var id2 = _toastService.ShowToast(new ToastSettings()
			{
				Content = builder => builder.AddMarkupContent(0, "msg2"),
				Type = NotificationTypes.Info,
			});

			Assert.IsNotNull(_toastService.Toasts);
			Assert.AreEqual(2, _toastService.AllToasts.Count());
			Assert.AreEqual(2, _toastService.Toasts.Count());
			Assert.AreNotEqual(Guid.Empty, id1);
			Assert.AreNotEqual(Guid.Empty, id2);


			Assert.AreEqual(true, _toastService.Toasts.ElementAt(0).IsVisible);
			Assert.AreEqual(false, _toastService.Toasts.ElementAt(0).IsRemove);
			Assert.IsNotNull(_toastService.Toasts.ElementAt(0).Content);
			Assert.AreEqual(NotificationStyles.Strong, _toastService.Toasts.ElementAt(0).NotificationStyle);
			Assert.AreEqual(NotificationTypes.Primary, _toastService.Toasts.ElementAt(0).Type);
			Assert.AreEqual(true, _toastService.Toasts.ElementAt(0).ShowIcon);
			Assert.AreEqual(NotificationStyles.Strong, _toastService.Toasts.ElementAt(0).NotificationStyle);
			Assert.AreEqual(true, _toastService.Toasts.ElementAt(0).ShowCloseCountdownProgress);
			Assert.AreEqual(true, _toastService.Toasts.ElementAt(0).ShowCloseButton);
			Assert.AreEqual((uint)11, _toastService.Toasts.ElementAt(0).AutoCloseInSec);
			Assert.AreEqual((uint)15, _toastService.Toasts.ElementAt(0).ShadowEffect);

			Assert.AreEqual(true, _toastService.Toasts.ElementAt(1).IsVisible);
			Assert.AreEqual(false, _toastService.Toasts.ElementAt(1).IsRemove);
			Assert.IsNotNull(_toastService.Toasts.ElementAt(1).Content);
			Assert.AreEqual(NotificationStyles.Outlined, _toastService.Toasts.ElementAt(1).NotificationStyle);
			Assert.AreEqual(NotificationTypes.Info, _toastService.Toasts.ElementAt(1).Type);
			Assert.AreEqual(ToastContainerGlobalSettings.DefaultToastsShowIcon, _toastService.Toasts.ElementAt(1).ShowIcon);
			Assert.AreEqual(ToastContainerGlobalSettings.DefaultToastsNotificationStyle, _toastService.Toasts.ElementAt(1).NotificationStyle);
			Assert.AreEqual(ToastContainerGlobalSettings.DefaultToastsShowCloseCountdownProgress, _toastService.Toasts.ElementAt(1).ShowCloseCountdownProgress);
			Assert.AreEqual(ToastContainerGlobalSettings.DefaultToastsShowCloseButton, _toastService.Toasts.ElementAt(1).ShowCloseButton);
			Assert.AreEqual(ToastContainerGlobalSettings.DefaultToastsAutoCloseInSec, _toastService.Toasts.ElementAt(1).AutoCloseInSec);
			Assert.AreEqual(ToastContainerGlobalSettings.DefaultToastsShadowEffect, _toastService.Toasts.ElementAt(1).ShadowEffect);
		}

		[TestMethod]
		public void ToastService_RemoveToast_should_handle_not_existing_id()
		{
			_toastService.ShowToast(new ToastSettings());
			_toastService.ShowToast(new ToastSettings());
			var id = _toastService.ShowToast(new ToastSettings());

			_toastService.RemoveToast(Guid.NewGuid());

			Assert.IsNotNull(_toastService.Toasts);
			Assert.AreEqual(3, _toastService.AllToasts.Count());
			Assert.AreEqual(3, _toastService.Toasts.Count());

			Assert.AreEqual(false, _toastService.Toasts.First().IsRemove);
			Assert.AreEqual(false, _toastService.Toasts.Last().IsRemove);
		}

		[TestMethod]
		public void ToastService_RemoveToast_should_set_IsRemove()
		{
			_toastService.ShowToast(new ToastSettings());
			_toastService.ShowToast(new ToastSettings());
			var id = _toastService.ShowToast(new ToastSettings());

			_toastService.RemoveToast(id);

			Assert.IsNotNull(_toastService.Toasts);
			Assert.AreEqual(3, _toastService.AllToasts.Count());
			Assert.AreEqual(3, _toastService.Toasts.Count());

			Assert.AreEqual(false, _toastService.Toasts.First().IsRemove);
			Assert.AreEqual(true, _toastService.Toasts.Last().IsRemove);
		}

		[TestMethod]
		public void ToastService_ClearAll_should_set_IsRemove()
		{
			_toastService.ShowToast(new ToastSettings());
			_toastService.ShowToast(new ToastSettings());
			var id = _toastService.ShowToast(new ToastSettings());

			_toastService.ClearAll();

			Assert.IsNotNull(_toastService.Toasts);
			Assert.AreEqual(3, _toastService.AllToasts.Count());
			Assert.AreEqual(3, _toastService.Toasts.Count());

			foreach (var item in _toastService.Toasts)
			{
				Assert.AreEqual(true, item.IsRemove);
			}
		}
	}
}