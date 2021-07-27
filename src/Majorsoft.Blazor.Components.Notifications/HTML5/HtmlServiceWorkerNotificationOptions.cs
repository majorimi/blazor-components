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
		///// <summary>
		///// Callback method for Notification Click event.
		///// </summary>
		//[JsonIgnore]
		//public Func<Guid, string, Task>? OnActionClickCallback { get; set; } = null;

		/// <summary>
		/// The actions array of the Notification as specified in the constructor's options parameter.
		/// </summary>
		public NotificationAction[] Actions { get; set; } = new NotificationAction[0];

		/// <summary>
		/// Service worker must be registered to show Notifications with Actions. Service worker need a JS file to be registered.
		/// </summary>
		public string ServiceWorkerUrl { get; }

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="title">Notification title</param>
		/// <param name="serviceWorkerUrl">To handle Notification action a ServiceWorker must be registered with and URL to a JS file</param>
		public HtmlServiceWorkerNotificationOptions(string title, string serviceWorkerUrl)
			:base(title)
		{
			if (string.IsNullOrWhiteSpace(serviceWorkerUrl))
			{
				throw new ArgumentException($"'{nameof(serviceWorkerUrl)}' cannot be null or whitespace.", nameof(serviceWorkerUrl));
			}
			ServiceWorkerUrl = serviceWorkerUrl;
		}
	}
}