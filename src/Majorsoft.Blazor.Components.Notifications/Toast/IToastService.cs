using System;
using System.Collections.Generic;
using System.Collections.Specialized;

using Microsoft.AspNetCore.Components;

namespace Majorsoft.Blazor.Components.Notifications
{
	/// <summary>
	/// Injectable service to handle Toast notifications and settings.
	/// Note: Injected as Singleton so all components use the same instance do NOT dispose manually.
	/// </summary>
	public interface IToastService : IDisposable
	{
		/// <summary>
		/// Collection of Toasts notifications.
		/// </summary>
		IEnumerable<ToastSettings> Toasts { get; }

		/// <summary>
		/// Global Toast Container and common Toasts settings with default values.
		/// </summary>
		ToastContainerGlobalSettings GlobalSettings { get; set; }

		/// <summary>
		/// Event triggered when Toasts collection changed.
		/// </summary>
		event NotifyCollectionChangedEventHandler? CollectionChanged;

		/// <summary>
		/// Event triggered when one of the Toast is opening.
		/// </summary>
		event ToastEvent OnToastOpen;
		/// <summary>
		/// Event triggered when the Toast is closing.
		/// </summary>
		event ToastEvent OnToastClosed;
		/// <summary>
		/// Event triggered when close `x` button was clicked on one of the Toast.
		/// </summary>
		event ToastEvent OnToastCloseButtonClicked;

		/// <summary>
		/// Shows a new Toast notification with given parameters. Other settings applied from default values.
		/// </summary>
		/// <param name="message">Toast string message</param>
		/// <param name="notificationType">Notification type or severity level</param>
		/// <param name="notificationStyle">Notification style to show different variant of the same <see cref="notificationType"/> Toast</param>
		/// <returns>Guid Id for the new Toast</returns>
		Guid ShowToast(string message, NotificationTypes notificationType = NotificationTypes.Info, NotificationStyles? notificationStyle = null);
		/// <summary>
		/// Shows a new Toast notification with given parameters. Other settings applied from default values.
		/// </summary>
		/// <param name="content">Toast HTML content</param>
		/// <param name="notificationType">Notification type or severity level</param>
		/// <param name="notificationStyle">Notification style to show different variant of the same <see cref="notificationType"/> Toast</param>
		/// <returns>Guid Id for the new Toast</returns>
		Guid ShowToast(MarkupString content, NotificationTypes notificationType = NotificationTypes.Info, NotificationStyles? notificationStyle = null);

		/// <summary>
		/// Shows a new Toast notification with given <see cref="ToastSettings"/>. It can be override all default values.
		/// </summary>
		/// <param name="toastSettings"></param>
		/// <returns>Guid Id for the new Toast</returns>
		Guid ShowToast(ToastSettings toastSettings);

		/// <summary>
		/// Removes a Toast notification from collection and UI.
		/// </summary>
		/// <param name="id">Toast Id to remove</param>
		void RemoveToast(Guid id);

		/// <summary>
		/// Clears <see cref="Toasts"/> collections and Removes all Toast notifications from UI.
		/// </summary>
		void ClearAll();
	}
}