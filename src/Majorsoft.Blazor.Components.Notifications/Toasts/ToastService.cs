using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

using Microsoft.AspNetCore.Components;

namespace Majorsoft.Blazor.Components.Notifications
{
	/// <summary>
	/// Implementation of <see cref="IToastService"/>.
	/// </summary>
	internal class ToastService : IToastService, IToastInternals
	{
		private readonly ObservableCollection<ToastSettings> _toasts;
		public IEnumerable<ToastSettings> Toasts => _toasts.Where(x => x.IsVisible);
		public IEnumerable<ToastSettings> AllToasts
		{
			get
			{
				if (_toasts.Any() && _toasts.All(x => !x.IsVisible))
				{
					_toasts.Clear();
				}

				return _toasts;
			}
		}

		public ToastContainerGlobalSettings GlobalSettings { get; set; } = new ToastContainerGlobalSettings();


		public event NotifyCollectionChangedEventHandler? CollectionChanged;
		public event ToastEvent? OnToastShow;
		public event ToastEvent? OnToastClosed;
		public event ToastEvent? OnToastCloseButtonClicked;

		/// <summary>
		/// Default constructor
		/// </summary>
		public ToastService()
		{
			_toasts = new ObservableCollection<ToastSettings>();
			_toasts.CollectionChanged += Toasts_CollectionChanged;
		}

		public Guid ShowToast(string message, NotificationTypes notificationType = NotificationTypes.Info, NotificationStyles? notificationStyle = null)
		{
			return ShowToast(builder => builder.AddMarkupContent(0, message), notificationType, notificationStyle);
		}
		public Guid ShowToast(RenderFragment content, NotificationTypes notificationType = NotificationTypes.Info, NotificationStyles? notificationStyle = null)
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

			_toasts.Add(toastSettings);

			return toastSettings.Id;
		}

		public void RemoveToast(Guid id)
		{
			var toast = _toasts.SingleOrDefault(x => x.Id == id);

			if(toast is not null)
			{
				toast.IsRemove = true;
				CollectionChanged?.Invoke(_toasts, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, toast, toast));
			}
		}

		public void ClearAll()
		{
			foreach (var item in _toasts)
			{
				item.IsRemove = true;
			}
			CollectionChanged?.Invoke(_toasts, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		private void Toasts_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) => CollectionChanged?.Invoke(sender, e);

		public void TriggerToastShow(Guid id) => OnToastShow?.Invoke(id);
		public void TriggerToastClosed(Guid id) => OnToastClosed?.Invoke(id);
		public void TriggerToastCloseButtonClicked(Guid id) => OnToastCloseButtonClicked?.Invoke(id);

		public void Dispose()
		{
			_toasts.CollectionChanged -= Toasts_CollectionChanged;
		}
	}
}