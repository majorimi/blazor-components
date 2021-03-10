using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Components.Common.JsInterop.GlobalMouseEvents
{
	/// <summary>
	/// PageResizeEventInfo event <see cref="DotNetObjectReference"/> info to handle JS callback
	/// </summary>
	internal sealed class ResizeEventInfo
	{
		private readonly Func<ResizeEventArgs, Task> _resizeEventCallback;

		internal string EventId { get; }
		internal ElementReference ElementReference { get; set; }

		public ResizeEventInfo(Func<ResizeEventArgs, Task> resizeEventCallback, string eventId)
		{
			_resizeEventCallback = resizeEventCallback;
			EventId = eventId;
		}

		[JSInvokable("ResizeEvent")]
		public async Task ResizeEvent(ResizeEventArgs args)
		{
			if (_resizeEventCallback is not null)
			{
				args.EventId = EventId;
				await _resizeEventCallback.Invoke(args);
			}
		}
	}
}