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

		public event NotifyCollectionChangedEventHandler? CollectionChanged;

		public ToastService()
		{
			_toasts = new ObservableCollection<ToastSettings>();
			_toasts.CollectionChanged += Toasts_CollectionChanged;
		}

		public Guid ShowToast(string message, NotificationTypes notificationType = NotificationTypes.Info, NotificationStyles notificationStyle = NotificationStyles.Normal)
		{
			return ShowToast(new ToastSettings()
			{
				Content = (MarkupString)message,
				Type = notificationType,
				NotificationStyle = notificationStyle,
			});
		}
		public Guid ShowToast(MarkupString content, NotificationTypes notificationType = NotificationTypes.Info, NotificationStyles notificationStyle = NotificationStyles.Normal)
		{
			return ShowToast(new ToastSettings()
			{
				Content = content,
				Type = notificationType,
				NotificationStyle = notificationStyle,
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
			if(remove is not null)
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

		event NotifyCollectionChangedEventHandler? CollectionChanged;

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

	/// <summary>
	/// Properties for <see cref="Toast"/> Notification.
	/// </summary>
	public class ToastSettings
	{
		/// <summary>
		/// Internal Toast Id
		/// </summary>
		internal Guid Id { get; set; }
		/// <summary>
		/// Internal Toast created time
		/// </summary>
		internal DateTime NotificationTime { get; set; }


		/// <summary>
		/// HTML Content of the collapse panel.
		/// </summary>
		[Parameter] public MarkupString Content { get; set; }

		/// <summary>
		/// Notification type or severity level.
		/// </summary>
		public NotificationTypes Type { get; set; }

		/// <summary>
		/// Notification style to show different variant of the same <see cref="Type"/> Toast.
		/// </summary>
		public NotificationStyles NotificationStyle { get; set; }

		/// <summary>
		/// When true Toast will show an icon corresponding to the <see cref="NotificationTypes"/>.
		/// </summary>
		public bool ShowIcon { get; set; } = true;
		/// <summary>
		/// Icon customization it accepts an SVG `Path` value to override the default icon.
		/// </summary>
		public string CustomIconSvgPath { get; set; } = "";
		/// <summary>
		/// When true Toast will show close "x" button.
		/// </summary>
		public bool ShowCloseButton { get; set; } = true;

		/// <summary>
		/// Toast will close after set time elapsed in Sec.
		/// </summary>
		public uint AutoCloseInSec { get; set; } = 10;

		/// <summary>
		/// When it's true a progress bar will show the remaining time until Alert closes.
		/// </summary>
		public bool ShowCloseCountdownProgress { get; set; } = true;

		private uint _shadowEffect = 5;
		/// <summary>
		/// Determines the shadow effect strongness which makes Alert elevated. Value should be between 0 and 20.
		/// </summary>
		public uint ShadowEffect
		{
			get => _shadowEffect;
			set
			{
				if (value > 20)
				{
					_shadowEffect = 20;
				}
				else
				{
					_shadowEffect = value;
				}
			}
		}
	}
}