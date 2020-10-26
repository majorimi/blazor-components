using System;
using System.Threading.Tasks;

using Microsoft.JSInterop;

namespace Blazor.Components.Common.JsInterop.Scroll
{
	/// <summary>
	/// PageScrollEventInfo event <see cref="DotNetObjectReference"/> info to handle JS callback
	/// </summary>
	internal sealed class PageScrollEventInfo
	{
		private readonly Func<ScrollEventArgs, Task> _scrollCallback;
		private readonly string _eventId;

		public PageScrollEventInfo(Func<ScrollEventArgs, Task> scrollCallback, string eventId)
		{
			_scrollCallback = scrollCallback;
			_eventId = eventId;
		}

		[JSInvokable("PageScroll")]
		public async Task PageScroll(ScrollEventArgs args)
		{
			if (_scrollCallback is not null)
			{
				await _scrollCallback.Invoke(args);
			}
		}
	}
}