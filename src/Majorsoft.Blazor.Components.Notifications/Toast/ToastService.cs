using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

using Microsoft.AspNetCore.Components;

namespace Majorsoft.Blazor.Components.Notifications
{
	/// <summary>
	/// 
	/// </summary>
	public class ToastService : IToastService, IToastEvents
	{
		private readonly ObservableCollection<ToastSettings> _toasts;
		public IEnumerable<ToastSettings> Toasts => _toasts;

		public ToastContainerGlobalSettings GlobalSettings { get; set; } = new ToastContainerGlobalSettings();

		public event NotifyCollectionChangedEventHandler? CollectionChanged;
		public event ToastEvent OnToastOpen;
		public event ToastEvent OnToastClosed;
		public event ToastEvent OnToastCloseButtonClicked;

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

		public void ClearAll()
		{
			_toasts.Clear();
		}

		private void Toasts_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) => CollectionChanged?.Invoke(sender, e);

		public void TriggerToastOpen(Guid id) => OnToastOpen?.Invoke(id);
		public void TriggerToastClosed(Guid id) => OnToastClosed?.Invoke(id);
		public void TriggerToastCloseButtonClicked(Guid id) => OnToastCloseButtonClicked?.Invoke(id);

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

		/// <summary>
		/// 
		/// </summary>
		event NotifyCollectionChangedEventHandler? CollectionChanged;

		/// <summary>
		/// 
		/// </summary>
		event ToastEvent OnToastOpen;
		/// <summary>
		/// 
		/// </summary>
		event ToastEvent OnToastClosed;
		/// <summary>
		/// 
		/// </summary>
		event ToastEvent OnToastCloseButtonClicked;

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
		void ClearAll();
	}
}