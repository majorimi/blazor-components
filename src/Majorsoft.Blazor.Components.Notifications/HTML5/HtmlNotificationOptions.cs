using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Majorsoft.Blazor.Components.Notifications
{
	/// <summary>
	/// These properties are available only on instances of the `Notification` object.
	/// </summary>
	public class HtmlNotificationOptions : HtmlNotificationData
	{
		/// <summary>
		/// Callback method for Notification Open event.
		/// </summary>
		[JsonIgnore]
		public Func<Guid, Task>? OnOpenCallback { get; set; } = null;

		/// <summary>
		/// Callback method for Notification Close event.
		/// </summary>
		[JsonIgnore]
		public Func<Guid, Task>? OnCloseCallback { get; set; } = null;

		/// <summary>
		/// Callback method for Notification on Error event.
		/// </summary>
		[JsonIgnore]
		public Func<Guid, Task>? OnErrorCallback { get; set; } = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="title">Notification title</param>
		public HtmlNotificationOptions(string title)
			: base(title)
		{}
	}
}