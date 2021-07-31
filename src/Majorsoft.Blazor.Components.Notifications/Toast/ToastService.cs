using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		}

		public Guid ShowToast(MarkupString message, ToastPositions position, NotificationTypes notificationType = NotificationTypes.Info, NotificationStyles notificationStyle = NotificationStyles.Normal) => throw new NotImplementedException();
		public Guid ShowToast(ToastSettings toastSettings)
		{
			var id = Guid.NewGuid();
			toastSettings.Id = id;
			toastSettings.NotificationTime = DateTime.Now;

			_toasts.Add(toastSettings);

			return id;
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public interface IToastService
	{
		IEnumerable<ToastSettings> Toasts { get; }

		event NotifyCollectionChangedEventHandler? CollectionChanged;

		Guid ShowToast(MarkupString message, ToastPositions position, 
			NotificationTypes notificationType = NotificationTypes.Info, 
			NotificationStyles notificationStyle = NotificationStyles.Normal);

		Guid ShowToast(ToastSettings toastSettings);
	}

	/// <summary>
	/// 
	/// </summary>
	public class ToastSettings
	{
		internal Guid Id { get; set; }

		internal DateTime NotificationTime { get; set; }
	}
}