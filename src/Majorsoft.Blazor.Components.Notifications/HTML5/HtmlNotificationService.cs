using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Components.Notifications
{
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

		public async ValueTask ShowsWithActionsAsync(HtmlServiceWorkerNotificationOptions notificationOptions)
		{
			var module = await _moduleTask.Value;

			await module.InvokeVoidAsync("showWithServiceWorker", notificationOptions, notificationOptions.ServiceWorkerUrl/*, dotnetRef*/);
		}

		public async ValueTask DisposeAsync()
		{
			if (_moduleTask.IsValueCreated)
			{
				var module = await _moduleTask.Value;
				//await module.InvokeVoidAsync("dispose", (object)_dotNetObjectReferences.Select(s => s.Value.Id).ToArray());
				await module.DisposeAsync();
			}

			foreach (var item in _dotNetObjectReferences)
			{
				item.Dispose();
			}
		}
	}
}