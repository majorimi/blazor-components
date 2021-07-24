using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Components.Notifications
{
	/// <summary>
	/// These properties are available only on instances of the `Notification` object.
	/// </summary>
	public class HtmlNotificationData
	{
		/// <summary>
		/// The title of the notification as specified in the first parameter of the constructor.
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// The actions array of the notification as specified in the constructor's options parameter.
		/// </summary>
		public NotificationAction[]? Actions { get; set; }

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
		public string Dir { get; set; } = "";

		/// <summary>
		/// The language code of the notification as specified in the constructor's options parameter.
		/// </summary>
		public string Lang { get; set; } = "";

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
		public DateTime Timestamp { get; set; }

		/// <summary>
		/// Specifies a vibration pattern for devices with vibration hardware to emit.
		/// An array of values describes alternating periods in which the device is vibrating and not vibrating.
		/// </summary>
		public int[]? Vibrate { get; set; }

		/// <summary>
		/// Default constructor.
		/// </summary>
		public HtmlNotificationData(string title)
		{
			if (string.IsNullOrWhiteSpace(title))
			{
				throw new ArgumentException($"Argument: {nameof(title)} is required.");
			}
			Title = title;
		}
	}

	/// <summary>
	/// Injectable service to handle Browser/HML5 notifications.
	/// https://developer.mozilla.org/en-US/docs/Web/API/notification
	/// </summary>
	public interface IHtmlNotificationService : IAsyncDisposable
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="callback"></param>
		/// <returns></returns>
		ValueTask RequestPermissionAsync(Func<HtmlNotificationPermissionTypes, Task> callback);

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		ValueTask<HtmlNotificationPermissionTypes> CheckPermissionAsync();

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		ValueTask<int> CheckMaxActionsAsync();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="notificationData"></param>
		/// <returns></returns>
		ValueTask<Guid> ShowsAsync(HtmlNotificationData notificationData);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="notificationId"></param>
		/// <returns></returns>
		ValueTask CloseAsync(Guid notificationId);

	}

	/// <summary>
	/// Implementation of <see cref="IHtmlNotificationService"/>
	/// </summary>
	public class HtmlNotificationService : IHtmlNotificationService
	{
		private List<DotNetObjectReference<HtmlNotificationEventInfo>> _dotNetObjectReferences;
		private readonly Lazy<Task<IJSObjectReference>> _moduleTask;
		private readonly IJSRuntime _jsRuntime;

		public HtmlNotificationService(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime;

			string js = "";
#if DEBUG
			js = "./_content/Majorsoft.Blazor.Components.Notifications/notification.js";
#else
			js = "./_content/Majorsoft.Blazor.Components.Notifications/notification.min.js";
#endif

			_moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>("import", js).AsTask());
			_dotNetObjectReferences = new List<DotNetObjectReference<HtmlNotificationEventInfo>>();
		}

		public async ValueTask RequestPermissionAsync(Func<HtmlNotificationPermissionTypes, Task> callback)
		{
			var module = await _moduleTask.Value;

			var info = new HtmlNotificationPermissionRequestEventInfo(callback);
			var dotnetRef = DotNetObjectReference.Create<HtmlNotificationPermissionRequestEventInfo>(info);
			info.DotNetObjectReference = dotnetRef;

			await module.InvokeVoidAsync("requestPermission", dotnetRef);
		}

		public async ValueTask<HtmlNotificationPermissionTypes> CheckPermissionAsync()
		{
			var module = await _moduleTask.Value;
			var permission = await module.InvokeAsync<string>("checkPermission");

			return Enum.Parse<HtmlNotificationPermissionTypes>(permission, true);
		}

		public async ValueTask<int> CheckMaxActionsAsync()
		{
			var module = await _moduleTask.Value;
			return await module.InvokeAsync<int>("checkMaxActions");
		}

		public async ValueTask<Guid> ShowsAsync(HtmlNotificationData notificationData)
		{
			var module = await _moduleTask.Value;

			var id = Guid.NewGuid();
			var info = new HtmlNotificationEventInfo(id, null);
			var dotnetRef = DotNetObjectReference.Create<HtmlNotificationEventInfo>(info);
			_dotNetObjectReferences.Add(dotnetRef);

			await module.InvokeVoidAsync("show");
			return id;
		}

		public async ValueTask CloseAsync(Guid notificationId)
		{
			var module = await _moduleTask.Value;

			await module.InvokeVoidAsync("close", notificationId);
		}

		public async ValueTask DisposeAsync()
		{
			if (_moduleTask.IsValueCreated)
			{
				var module = await _moduleTask.Value;
				await module.InvokeVoidAsync("dispose", (object)_dotNetObjectReferences.Select(s => s.Value.Id).ToArray());
				await module.DisposeAsync();
			}

			foreach (var item in _dotNetObjectReferences)
			{
				item.Dispose();
			}
		}
	}

	/// <summary>
	/// Html5 Notification Permission Request result event <see cref="DotNetObjectReference"/> info to handle JS callback
	/// </summary>
	internal sealed class HtmlNotificationEventInfo
	{
		private readonly Func<string, Task> _actionsCallback;
		public Guid Id { get; }

		public HtmlNotificationEventInfo(Guid id, Func<string, Task> actionsCallback)
		{
			Id = id;
			_actionsCallback = actionsCallback;
		}

		[JSInvokable("ActionsCallback")]
		public async Task ActionsCallback(string action)
		{
			if (_actionsCallback is not null)
			{
				await _actionsCallback(action);
			}
		}
	}
}