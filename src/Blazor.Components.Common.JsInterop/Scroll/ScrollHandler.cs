using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Components.Common.JsInterop.Scroll
{
	/// <summary>
	/// Implementation of <see cref="IScrollHandler"/>
	/// </summary>
	public sealed class ScrollHandler : IScrollHandler
	{
		private readonly IJSRuntime _jsRuntime;
		private List<string> _registeredEvents;
		private IJSObjectReference _scrollJs;

		public ScrollHandler(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime;
			_registeredEvents = new List<string>();
		}

		public async Task ScrollToPageEndAsync()
		{
			await CheckJsObjectAsync();
			await _scrollJs.InvokeVoidAsync("scrollToPageEnd");
		}
		public async Task ScrollToPageTopAsync()
		{
			await CheckJsObjectAsync();
			await _scrollJs.InvokeVoidAsync("scrollToPageTop");
		}
		public async Task ScrollToPageXAsync(double x)
		{
			await CheckJsObjectAsync();
			await _scrollJs.InvokeVoidAsync("scrollToPageX", x);
		}
		public async Task ScrollToPageYAsync(double y)
		{
			await CheckJsObjectAsync();
			await _scrollJs.InvokeVoidAsync("scrollToPageY", y);
		}
		public async Task<ScrollEventArgs> GetPageScrollPosAsync()
		{
			await CheckJsObjectAsync();
			return await _scrollJs.InvokeAsync<ScrollEventArgs>("getPageScrollPosition");
		}

		public async Task ScrollToElementAsync(ElementReference elementReference)
		{
			await CheckJsObjectAsync();
			await _scrollJs.InvokeVoidAsync("scrollToElement", elementReference);
		}

		public async Task ScrollToElementByIdAsync(string id)
		{
			await CheckJsObjectAsync();
			await _scrollJs.InvokeVoidAsync("scrollToElementById", id);
		}

		public async Task ScrollToElementByNameAsync(string name)
		{
			await CheckJsObjectAsync();
			await _scrollJs.InvokeVoidAsync("scrollToElementByName", name);
		}

		public async Task<string> RegisterPageScrollAsync(Func<ScrollEventArgs, Task> scrollCallback)
		{
			await CheckJsObjectAsync();

			var id = Guid.NewGuid().ToString();
			var info = new PageScrollEventInfo(scrollCallback, id);
			var dotnetRef = DotNetObjectReference.Create<PageScrollEventInfo>(info);

			await _scrollJs.InvokeVoidAsync("addScrollEvent", dotnetRef, id);
			_registeredEvents.Add(id);

			return id;
		}

		public async Task RemovePageScrollAsync(string eventId)
		{
			await CheckJsObjectAsync();

			await _scrollJs.InvokeVoidAsync("removeScrollEvent", eventId);
			RemoveElement(eventId);
		}

		private void RemoveElement(string eventId)
		{
			var items = _registeredEvents.Where(x => x.Equals(eventId));
			_registeredEvents = _registeredEvents.Except(items).ToList();
		}

		private async Task CheckJsObjectAsync()
		{
			if (_scrollJs is null)
			{
#if DEBUG
				_scrollJs = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Majorsoft.Blazor.Components.Common.JsInterop/scroll.js");
#else
				_scrollJs = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Majorsoft.Blazor.Components.Common.JsInterop/scroll.min.js");
#endif
			}
		}

		public async ValueTask DisposeAsync()
		{
			if (_scrollJs is not null)
			{
				await _scrollJs.InvokeVoidAsync("dispose", _registeredEvents.ToArray());

				await _scrollJs.DisposeAsync();
			}
		}
	}
}