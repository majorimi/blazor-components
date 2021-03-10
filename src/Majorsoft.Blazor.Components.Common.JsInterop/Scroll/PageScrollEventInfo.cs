using System;
using System.Threading.Tasks;

using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Components.Common.JsInterop.Scroll
{
	/// <summary>
	/// PageScrollEventInfo event <see cref="DotNetObjectReference"/> info to handle JS callback
	/// </summary>
	internal sealed class PageScrollEventInfo
	{
		private readonly Func<ScrollEventArgs, Task> _scrollCallback;
		internal string EventId { get; }

		public PageScrollEventInfo(Func<ScrollEventArgs, Task> scrollCallback, string eventId)
		{
			_scrollCallback = scrollCallback;
			EventId = eventId;
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