using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

using Microsoft.AspNetCore.Components;

namespace Majorsoft.Blazor.Components.Notifications
{
	/// <summary>
	/// 
	/// </summary>
	public class ToastService : IToastService
	{
		private readonly List<ToastSettings> _toasts;
		public IEnumerable<ToastSettings> Toasts => _toasts;

		public ToastContainerGlobalSettings GlobalSettings { get; set; } = new ToastContainerGlobalSettings();

		//public event NotifyCollectionChangedEventHandler? CollectionChanged;

		public ToastService()
		{
			_toasts = new List<ToastSettings>();
			//_toasts.CollectionChanged += Toasts_CollectionChanged;
		}

		public Guid ShowToast(string message, NotificationTypes notificationType = NotificationTypes.Info, NotificationStyles notificationStyle = NotificationStyles.Normal)
		{
			return ShowToast((MarkupString)message, notificationType, notificationStyle);
		}
		public Guid ShowToast(MarkupString content, NotificationTypes notificationType = NotificationTypes.Info, NotificationStyles notificationStyle = NotificationStyles.Normal)
		{
			return ShowToast(new ToastSettings()
			{
				Content = content,
				Type = notificationType,
				NotificationStyle = notificationStyle,

				ShowCloseButton = ToastContainerGlobalSettings.DefaultToastsShowCloseButton,
				ShowIcon = ToastContainerGlobalSettings.DefaultToastsShowIcon,
				AutoCloseInSec = ToastContainerGlobalSettings.DefaultToastsAutoCloseInSec,
				ShadowEffect = ToastContainerGlobalSettings.DefaultToastsShadowEffect,
				ShowCloseCountdownProgress = ToastContainerGlobalSettings.DefaultToastsShowCloseCountdownProgress
			});
		}

		public Guid ShowToast(ToastSettings toastSettings)
		{
			var id = Guid.NewGuid();
			toastSettings.Id = id;
			toastSettings.NotificationTime = DateTime.Now;

			_toasts.Add(toastSettings);

			return id;
		}

		public void RemoveToast(Guid id)
		{
			var remove = _toasts.SingleOrDefault(x => x.Id == id);
			if (remove is not null)
			{
				_toasts.Remove(remove);
			}
		}

		public void ClearAll()
		{
			_toasts.Clear();
		}

		//private void Toasts_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) => CollectionChanged?.Invoke(sender, e);

		//public void Dispose()
		//{
		//	//_toasts.CollectionChanged -= Toasts_CollectionChanged;
		//}
	}

	/// <summary>
	/// 
	/// </summary>
	public interface IToastService// : IDisposable
	{
		/// <summary>
		/// 
		/// </summary>
		IEnumerable<ToastSettings> Toasts { get; }

		/// <summary>
		/// 
		/// </summary>
		public ToastContainerGlobalSettings GlobalSettings { get; set; }

		//event NotifyCollectionChangedEventHandler? CollectionChanged;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="notificationType"></param>
		/// <param name="notificationStyle"></param>
		/// <returns></returns>
		Guid ShowToast(string message, NotificationTypes notificationType = NotificationTypes.Info, NotificationStyles notificationStyle = NotificationStyles.Normal);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="content"></param>
		/// <param name="notificationType"></param>
		/// <param name="notificationStyle"></param>
		/// <returns></returns>
		Guid ShowToast(MarkupString content, NotificationTypes notificationType = NotificationTypes.Info, NotificationStyles notificationStyle = NotificationStyles.Normal);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="toastSettings"></param>
		/// <returns></returns>
		Guid ShowToast(ToastSettings toastSettings);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		void RemoveToast(Guid id);

		/// <summary>
		/// 
		/// </summary>
		void ClearAll();
	}
}