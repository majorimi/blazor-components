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
	public class ToastService : IToastService, IDisposable
	{
		private readonly ObservableCollection<ToastSettings> _toasts;
		public IEnumerable<ToastSettings> Toasts => _toasts;

		public event NotifyCollectionChangedEventHandler? CollectionChanged;

		public ToastService()
		{
			_toasts = new ObservableCollection<ToastSettings>();
			_toasts.CollectionChanged += Toasts_CollectionChanged;
		}

		public Guid ShowToast(RenderFragment message, ToastPositions position, 
			NotificationTypes notificationType = NotificationTypes.Info, 
			NotificationStyles notificationStyle = NotificationStyles.Normal)
		{
			return ShowToast(new ToastSettings()
			{
				//
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
	public interface IToastService
	{
		IEnumerable<ToastSettings> Toasts { get; }

		event NotifyCollectionChangedEventHandler? CollectionChanged;

		Guid ShowToast(RenderFragment message, ToastPositions position, 
			NotificationTypes notificationType = NotificationTypes.Info, 
			NotificationStyles notificationStyle = NotificationStyles.Normal);

		Guid ShowToast(ToastSettings toastSettings);

		void RemoveToast(Guid id);

		void ClearAll();
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