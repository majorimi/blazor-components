using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.JSInterop;

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
		/// <returns></returns>
		ValueTask<bool> IsBrowserSupportedAsync();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="notificationOptions"></param>
		/// <returns></returns>
		ValueTask<Guid> ShowsAsync(HtmlNotificationOptions notificationOptions);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="notificationOptions"></param>
		/// <returns></returns>
		ValueTask<Guid> ShowsWithActionsAsync(HtmlServiceWorkerNotificationOptions notificationOptions);

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
		public async ValueTask<bool> IsBrowserSupportedAsync()
		{
			var module = await _moduleTask.Value;
			return await module.InvokeAsync<bool>("isBrowserSupported");
		}

		public async ValueTask<Guid> ShowsAsync(HtmlNotificationOptions notificationOptions)
		{
			var module = await _moduleTask.Value;

			var id = Guid.NewGuid();
			var info = new HtmlNotificationEventInfo(id, 
				notificationOptions.OnOpenCallback,
				notificationOptions.OnClickCallback,
				notificationOptions.OnCloseCallback,
				notificationOptions.OnErrorCallback);

			var dotnetRef = DotNetObjectReference.Create<HtmlNotificationEventInfo>(info);
			_dotNetObjectReferences.Add(dotnetRef);

			await module.InvokeVoidAsync("showSimple", id.ToString(), notificationOptions, dotnetRef);
			return id;
		}

		public async ValueTask<Guid> ShowsWithActionsAsync(HtmlServiceWorkerNotificationOptions notificationOptions)
		{
			var module = await _moduleTask.Value;

			var id = Guid.NewGuid();
			var info = new HtmlNotificationEventInfo(id, 
				onClickCallback: notificationOptions.OnClickCallback, 
				onActionClickCallback: notificationOptions.OnActionClickCallback);

			var dotnetRef = DotNetObjectReference.Create<HtmlNotificationEventInfo>(info);
			_dotNetObjectReferences.Add(dotnetRef);

			await module.InvokeVoidAsync("showWithActions", id.ToString(), notificationOptions, notificationOptions.ServiceWorkerUrl, dotnetRef);
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
		private readonly Func<Guid, Task>? _onOpenCallback;
		private readonly Func<Guid, Task>? _onClickCallback;
		private readonly Func<Guid, Task>? _onCloseCallback;
		private readonly Func<Guid, Task>? _onErrorCallback;
		private readonly Func<Guid, string, Task>? _onActionClickCallback;

		public Guid Id { get; }

		public HtmlNotificationEventInfo(Guid id,
			Func<Guid, Task>? onOpenCallback = null,
			Func<Guid, Task>? onClickCallback = null,
			Func<Guid, Task>? onCloseCallback = null,
			Func<Guid, Task>? onErrorCallback = null,
			Func<Guid, string, Task>? onActionClickCallback = null)
		{
			Id = id;
			_onOpenCallback = onOpenCallback;
			_onClickCallback = onClickCallback;
			_onCloseCallback = onCloseCallback;
			_onErrorCallback = onErrorCallback;
			_onActionClickCallback = onActionClickCallback;
		}

		[JSInvokable("ActionsCallback")]
		public async Task ActionsCallback(string action)
		{
			if (_onActionClickCallback is not null)
			{
				await _onActionClickCallback(Id, action);
			}
		}

		[JSInvokable("OnOpen")]
		public async Task OnOpen()
		{
			if (_onOpenCallback is not null)
			{
				await _onOpenCallback(Id);
			}
		}
		[JSInvokable("OnClick")]
		public async Task OnClick()
		{
			if (_onClickCallback is not null)
			{
				await _onClickCallback(Id);
			}
		}
		[JSInvokable("OnClose")]
		public async Task OnClose()
		{
			if (_onCloseCallback is not null)
			{
				await _onCloseCallback(Id);
			}
		}
		[JSInvokable("OnError")]
		public async Task OnError()
		{
			if (_onErrorCallback is not null)
			{
				await _onErrorCallback(Id);
			}
		}
	}
}