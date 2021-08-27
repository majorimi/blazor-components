namespace Majorsoft.Blazor.Components.Notifications
{
	/// <summary>
	/// The NotificationAction interface of the Notifications API is used to represent 
	/// action buttons the user can click to interact with notifications. These buttons' 
	/// appearance and specific functionality vary across platforms but generally they 
	/// provide a way to asynchronously show actions to the user in a notification.
	/// </summary>
	public class NotificationAction
	{
		/// <summary>
		/// The name of the action, which can be used to identify the clicked action similar to input names.
		/// </summary>
		public string Action { get; set; } = "";

		/// <summary>
		/// The string describing the action that is displayed to the user.
		/// </summary>
		public string Title { get; set; } = "";

		/// <summary>
		/// The URL of the image used to represent the notification when there is not enough space to display the notification itself.
		/// </summary>
		public string Icon { get; set; } = "";
	}
}