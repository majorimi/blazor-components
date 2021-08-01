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
		private readonly ObservableCollection<ToastSettings> _toasts;
		public IEnumerable<ToastSettings> Toasts => _toasts;

		public ToastContainerGlobalSettings GlobalSettings { get; set; } = new ToastContainerGlobalSettings();

		public event NotifyCollectionChangedEventHandler? CollectionChanged;

		public ToastService()
		{
			_toasts = new ObservableCollection<ToastSettings>();
			_toasts.CollectionChanged += Toasts_CollectionChanged;
		}

		public Guid ShowToast(string message, NotificationTypes notificationType = NotificationTypes.Info, NotificationStyles? notificationStyle = null)
		{
			return ShowToast((MarkupString)message, notificationType, notificationStyle);
		}
		public Guid ShowToast(MarkupString content, NotificationTypes notificationType = NotificationTypes.Info, NotificationStyles? notificationStyle = null)
		{
			return ShowToast(new ToastSettings()
			{
				Content = content,
				Type = notificationType,
				NotificationStyle = notificationStyle ?? ToastContainerGlobalSettings.DefaultToastsNotificationStyle,

				ShowCloseButton = ToastContainerGlobalSettings.DefaultToastsShowCloseButton,
				ShowIcon = ToastContainerGlobalSettings.DefaultToastsShowIcon,
				AutoCloseInSec = ToastContainerGlobalSettings.DefaultToastsAutoCloseInSec,
				ShadowEffect = ToastContainerGlobalSettings.DefaultToastsShadowEffect,
				ShowCloseCountdownProgress = ToastContainerGlobalSettings.DefaultToastsShowCloseCountdownProgress
			});
		}

		public Guid ShowToast(ToastSettings toastSettings)
		{
			if (toastSettings is null)
			{
				throw new ArgumentNullException(nameof(toastSettings));
			}

			toastSettings.Id = Guid.NewGuid();
			toastSettings.NotificationTime = DateTime.Now;

			_toasts.Add(toastSettings);

			return toastSettings.Id;
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

		private void Toasts_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) => CollectionChanged?.Invoke(sender, e);

		public void Dispose()
		{
			_toasts.CollectionChanged -= Toasts_CollectionChanged;
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public interface IToastService : IDisposable
	{
		/// <summary>
		/// 
		/// </summary>
		IEnumerable<ToastSettings> Toasts { get; }

		/// <summary>
		/// 
		/// </summary>
		ToastContainerGlobalSettings GlobalSettings { get; set; }

		event NotifyCollectionChangedEventHandler? CollectionChanged;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="notificationType"></param>
		/// <param name="notificationStyle"></param>
		/// <returns></returns>
		Guid ShowToast(string message, NotificationTypes notificationType = NotificationTypes.Info, NotificationStyles? notificationStyle = null);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="content"></param>
		/// <param name="notificationType"></param>
		/// <param name="notificationStyle"></param>
		/// <returns></returns>
		Guid ShowToast(MarkupString content, NotificationTypes notificationType = NotificationTypes.Info, NotificationStyles? notificationStyle = null);

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