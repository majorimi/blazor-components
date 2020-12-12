using System;
using System.Threading.Tasks;

using Microsoft.JSInterop;

namespace Blazor.Components.Common.JsInterop.GlobalMouseEvents
{
	/// <summary>
	/// PageResizeEventInfo event <see cref="DotNetObjectReference"/> info to handle JS callback
	/// </summary>
	internal sealed class PageResizeEventInfo
	{
		private readonly Func<ResizeEventArgs, Task> _resizeEventCallback;
		private readonly string _eventId;

		public PageResizeEventInfo(Func<ResizeEventArgs, Task> resizeEventCallback, string eventId)
		{
			_resizeEventCallback = resizeEventCallback;
			_eventId = eventId;
		}

		[JSInvokable("GlobalMouseMove")]
		public async Task GlobalMouseMove(ResizeEventArgs args)
		{
			if (_resizeEventCallback is not null)
			{
				args.EventId = _eventId;
				await _resizeEventCallback.Invoke(args);
			}
		}
	}
}