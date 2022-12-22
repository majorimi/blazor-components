using System;
using System.Text.Json.Serialization;

namespace Majorsoft.Blazor.Components.Notifications
{
	/// <summary>
	/// Base properties for all types of HTML notifications.
	/// </summary>
	public abstract class HtmlNotificationData
	{
		/// <summary>
		/// The title of the notification as specified in the first parameter of the constructor.
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// The URL of the image used to represent the notification when there is not enough space to display the notification itself.
		/// </summary>
		public string Badge { get; set; } = "";

		/// <summary>
		/// The body string of the notification as specified in the constructor's options parameter.
		/// </summary>
		public string Body { get; set; } = "";

		/// <summary>
		/// The data read-only property of the Notification interface returns a structured clone of the notification's data, as specified in the data option of the Notification() constructor.
		/// The notification's data can be any arbitrary data that you want associated with the notification.
		/// </summary>
		public object Data { get; set; } = new object();

		/// <summary>
		/// The text direction of the notification as specified in the constructor's options parameter.
		/// </summary>
		public string Dir { get; set; } = "auto";

		/// <summary>
		/// The language code of the notification as specified in the constructor's options parameter.
		/// </summary>
		public string Lang { get; set; } = "en";

		/// <summary>
		/// The ID of the notification (if any) as specified in the constructor's options parameter.
		/// </summary>
		public string Tag { get; set; } = "";

		/// <summary>
		/// The URL of the image used as an icon of the notification as specified in the constructor's options parameter.
		/// </summary>
		public string Icon { get; set; } = "";

		/// <summary>
		/// The URL of an image to be displayed as part of the notification, as specified in the constructor's options parameter.
		/// </summary>
		public string Image { get; set; } = "";

		/// <summary>
		/// Specifies whether the user should be notified after a new notification replaces an old one.
		/// </summary>
		public bool Renotify { get; set; }

		/// <summary>
		/// A Boolean indicating that a notification should remain active until the user clicks or dismisses it, rather than closing automatically.
		/// </summary>
		public bool RequireInteraction { get; set; }

		/// <summary>
		/// Specifies whether the notification should be silent — i.e., no sounds or vibrations should be issued, regardless of the device settings.
		/// </summary>
		public bool Silent { get; set; }

		/// <summary>
		/// Specifies the time at which a notification is created or applicable (past, present, or future).
		/// </summary>
		[JsonConverter(typeof(EpochTimestampDateTimeConverter))]
		public DateTime Timestamp { get; set; } = DateTime.UtcNow;

		/// <summary>
		/// Specifies a vibration pattern for devices with vibration hardware to emit.
		/// An array of values describes alternating periods in which the device is vibrating and not vibrating.
		/// </summary>
		public int[] Vibrate { get; set; } = new int[0];

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="title">Notification title</param>
		public HtmlNotificationData(string title)
		{
			if (string.IsNullOrWhiteSpace(title))
			{
				throw new ArgumentException($"Argument: {nameof(title)} is required.");
			}

			Title = title;
		}
	}
}