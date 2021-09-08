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
		private IJSObjectReference _scrollJs;
		private List<DotNetObjectReference<PageScrollEventInfo>> _dotNetObjectReferences;

		public ScrollHandler(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime;
			_dotNetObjectReferences = new List<DotNetObjectReference<PageScrollEventInfo>>();
		}

		public async Task ScrollToPageEndAsync(bool smooth)
		{
			await CheckJsObjectAsync();
			await _scrollJs.InvokeVoidAsync("scrollToPageEnd", smooth);
		}
		public async Task ScrollToPageTopAsync(bool smooth)
		{
			await CheckJsObjectAsync();
			await _scrollJs.InvokeVoidAsync("scrollToPageTop", smooth);
		}
		public async Task ScrollToPageXAsync(double x, bool smooth)
		{
			await CheckJsObjectAsync();
			await _scrollJs.InvokeVoidAsync("scrollToPageX", x, smooth);
		}
		public async Task ScrollToPageYAsync(double y, bool smooth)
		{
			await CheckJsObjectAsync();
			await _scrollJs.InvokeVoidAsync("scrollToPageY", y, smooth);
		}
		public async Task<ScrollResult> GetPageScrollPosAsync()
		{
			await CheckJsObjectAsync();
			return await _scrollJs.InvokeAsync<ScrollResult>("getPageScrollPosition");
		}
		public async Task<ScrollResult> GetPageScrollSizeAsync()
		{
			await CheckJsObjectAsync();
			return await _scrollJs.InvokeAsync<ScrollResult>("getPageScrollSize");
		}

		public async Task ScrollToElementAsync(ElementReference elementReference, bool smooth)
		{
			await CheckJsObjectAsync();
			await _scrollJs.InvokeVoidAsync("scrollToElement", elementReference, smooth);
		}

		public async Task ScrollToElementByIdAsync(string id, bool smooth)
		{
			await CheckJsObjectAsync();
			await _scrollJs.InvokeVoidAsync("scrollToElementById", id, smooth);
		}

		public async Task ScrollToElementByNameAsync(string name, bool smooth)
		{
			await CheckJsObjectAsync();
			await _scrollJs.InvokeVoidAsync("scrollToElementByName", name, smooth);
		}

		public async Task<string> RegisterPageScrollAsync(Func<ScrollEventArgs, Task> scrollCallback)
		{
			await CheckJsObjectAsync();

			var id = Guid.NewGuid().ToString();
			var info = new PageScrollEventInfo(scrollCallback, id);
			var dotnetRef = DotNetObjectReference.Create<PageScrollEventInfo>(info);
			_dotNetObjectReferences.Add(dotnetRef);

			await _scrollJs.InvokeVoidAsync("addScrollEvent", dotnetRef, id);

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
			var dotNetRefs = _dotNetObjectReferences.Where(x => x.Value.EventId == eventId);
			_dotNetObjectReferences = _dotNetObjectReferences.Except(dotNetRefs).ToList();

			foreach (var item in dotNetRefs)
			{
				item.Dispose();
			}
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
				await _scrollJs.InvokeVoidAsync("dispose",
					(object)_dotNetObjectReferences.Select(s => s.Value.EventId).ToArray());

				await _scrollJs.DisposeAsync();
			}
		}
	}
}