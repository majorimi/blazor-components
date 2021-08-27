using System;
using System.Threading.Tasks;

using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Components.Notifications
{
	/// <summary>
	/// Html5 Notification Permission Request result event <see cref="DotNetObjectReference"/> info to handle JS callback
	/// </summary>
	internal sealed class HtmlNotificationPermissionRequestEventInfo
	{
		private readonly Func<HtmlNotificationPermissionTypes, Task> _callback;
		internal DotNetObjectReference<HtmlNotificationPermissionRequestEventInfo> DotNetObjectReference { get; set; }

		public HtmlNotificationPermissionRequestEventInfo(Func<HtmlNotificationPermissionTypes, Task> callback)
		{
			_callback = callback;
		}

		[JSInvokable("PermissionResult")]
		public async Task PermissionResult(string permission)
		{
			if (_callback is not null)
			{
				await _callback(Enum.Parse<HtmlNotificationPermissionTypes>(permission, true));
			}

			if(DotNetObjectReference is not null)
			{
				DotNetObjectReference.Dispose();
			}
		}
	}
}