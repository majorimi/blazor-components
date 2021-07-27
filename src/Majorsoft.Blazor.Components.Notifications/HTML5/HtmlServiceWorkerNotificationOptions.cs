using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Majorsoft.Blazor.Components.Notifications
{
	/// <summary>
	/// These properties are available for Service worker `Notification`.
	/// </summary>
	public class HtmlServiceWorkerNotificationOptions : HtmlNotificationData
	{
		/// <summary>
		/// Callback method for Notification Click event.
		/// </summary>
		[JsonIgnore]
		public Func<Guid, string, Task>? OnActionClickCallback { get; set; } = null;

		/// <summary>
		/// The actions array of the Notification as specified in the constructor's options parameter.
		/// </summary>
		public NotificationAction[] Actions { get; set; } = new NotificationAction[0];

		/// <summary>
		/// Service worker must be registered to show Notifications with Actions. Service worker need a JS file to be registered.
		/// </summary>
		public string ServiceWorkerUrl { get; set; } = "_content/Majorsoft.Blazor.Components.Notifications/sw.js";

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="title">Notification title</param>
		public HtmlServiceWorkerNotificationOptions(string title)
			:base(title)
		{}
	}
}