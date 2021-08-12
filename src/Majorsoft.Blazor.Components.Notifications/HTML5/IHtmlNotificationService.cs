using System;
using System.Threading.Tasks;

namespace Majorsoft.Blazor.Components.Notifications
{
	/// <summary>
	/// Injectable service to handle Browser/HML5 notifications.
	/// Docs: https://developer.mozilla.org/en-US/docs/Web/API/notification
	/// Settings on Windows: https://stackoverflow.com/questions/51907779/browser-notification-not-showing-up#58982697
	/// </summary>
	public interface IHtmlNotificationService : IAsyncDisposable
	{
		/// <summary>
		/// Prompts User Consent permission request popup to the User to decide whether to allow Notifications or not.
		/// </summary>
		/// <param name="callback">Callback function to call when User consent result provided</param>
		/// <returns>ValueTask</returns>
		ValueTask RequestPermissionAsync(Func<HtmlNotificationPermissionTypes, Task> callback);

		/// <summary>
		/// Checks the Notification User Consent status.
		/// </summary>
		/// <returns>User consent value</returns>
		ValueTask<HtmlNotificationPermissionTypes> CheckPermissionAsync();

		/// <summary>
		/// Returns maxActions attribute of the Notification interface returns the maximum number of actions supported by the device and the User Agent. 
		/// </summary>
		/// <returns>Maximum allowed Notification</returns>
		ValueTask<int> CheckMaxActionsAsync();

		/// <summary>
		/// Checks if Browser supports HTML Notifications API or not.
		/// </summary>
		/// <returns>Browser supported Notification or not</returns>
		ValueTask<bool> IsBrowserSupportedAsync();

		/// <summary>
		/// Shows a Notification with the given <see cref="HtmlNotificationOptions"/> options data.
		/// </summary>
		/// <param name="notificationOptions">Notification details</param>
		/// <returns>Reference id</returns>
		ValueTask<Guid> ShowsAsync(HtmlNotificationOptions notificationOptions);

		/// <summary>
		/// Registers the given ServiceWorker from the URL and prompts Notifications with Options provided.
		/// ServiceWorker can handle Custom events with custom data provided.
		/// </summary>
		/// <param name="notificationOptions"><see cref="HtmlServiceWorkerNotificationOptions"/> data type required for ServiceWorker Notifications</param>
		/// <returns></returns>
		ValueTask ShowsWithActionsAsync(HtmlServiceWorkerNotificationOptions notificationOptions);
	}
}