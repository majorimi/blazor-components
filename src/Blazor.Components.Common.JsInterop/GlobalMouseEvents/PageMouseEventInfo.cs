using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Blazor.Components.Common.JsInterop.GlobalMouseEvents
{
	/// <summary>
	/// PageMouseEventInfo event <see cref="DotNetObjectReference"/> info to handle JS callback
	/// </summary>
	internal sealed class PageMouseEventInfo
	{
		private readonly Func<MouseEventArgs, Task> _mouseEventCallback;
		private readonly string _eventId;

		public PageMouseEventInfo(Func<MouseEventArgs, Task> mouseEventCallback, string eventId)
		{
			_mouseEventCallback = mouseEventCallback;
			_eventId = eventId;
		}

		[JSInvokable("GlobalMouseMove")]
		public async Task GlobalMouseMove(MouseEventArgs args)
		{
			if (_mouseEventCallback is not null)
			{
				await _mouseEventCallback.Invoke(args);
			}
		}
		[JSInvokable("GlobalMouseDown")]
		public async Task GlobalMouseDown(MouseEventArgs args)
		{
			if (_mouseEventCallback is not null)
			{
				await _mouseEventCallback.Invoke(args);
			}
		}
		[JSInvokable("GlobalMouseUp")]
		public async Task GlobalMouseUp(MouseEventArgs args)
		{
			if (_mouseEventCallback is not null)
			{
				await _mouseEventCallback.Invoke(args);
			}
		}

		private async Task CallEvent(Func<MouseEventArgs, Task> eventCallback, MouseEventArgs args)
		{
			if (eventCallback is not null)
			{
				await eventCallback.Invoke(args);
			}
		}
	}
}