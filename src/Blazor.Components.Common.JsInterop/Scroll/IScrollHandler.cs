using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor.Components.Common.JsInterop.Scroll
{
	/// <summary>
	/// Injectable service to handle JS document/window 'scroll' events for the whole document.
	/// </summary>
	public interface IScrollHandler : IAsyncDisposable
	{
		Task ScrollToElementAsync(ElementReference elementReference);

		Task ScrollToPageEndAsync();
		Task ScrollToPageTopAsync();
		Task ScrollToPageXAsync(double x);
		Task<double> GetPageScrollPosXAsync();

		Task RegisterPageScrollAsync(Func<ScrollEventArgs, Task> scrollCallback);
		Task RemovePageScrollAsync();
	}

	/// <summary>
	/// Implementation of <see cref="IScrollHandler"/>
	/// </summary>
	public sealed class ScrollHandler : IScrollHandler
	{
		private readonly IJSRuntime _jsRuntime;
		private IJSObjectReference _clickJs;

		public ScrollHandler(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime;
		}

		public async Task ScrollToPageEndAsync()
		{
			await CheckJsObjectAsync();
			await _clickJs.InvokeVoidAsync("scrollToPageEnd");
		}
		public async Task ScrollToPageTopAsync()
		{
			await CheckJsObjectAsync();
			await _clickJs.InvokeVoidAsync("scrollToPageTop");
		}
		public async Task ScrollToPageXAsync(double x)
		{
			await CheckJsObjectAsync();
			await _clickJs.InvokeVoidAsync("scrollToPageX", x);
		}
		public async Task<double> GetPageScrollPosXAsync()
		{
			await CheckJsObjectAsync();
			return await _clickJs.InvokeAsync<double>("getPageScrollXPosition");
		}

		public async Task ScrollToElementAsync(ElementReference elementReference)
		{
			await CheckJsObjectAsync();
			await _clickJs.InvokeVoidAsync("scrollToElement", elementReference);
		}

		public Task RegisterPageScrollAsync(Func<ScrollEventArgs, Task> scrollCallback) => throw new NotImplementedException();
		public Task RemovePageScrollAsync() => throw new NotImplementedException();

		private async Task CheckJsObjectAsync()
		{
			if (_clickJs is null)
			{
#if DEBUG
				_clickJs = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Majorsoft.Blazor.Components.Common.JsInterop/scroll.js");
#else
				_transitionJs = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Majorsoft.Blazor.Components.Common.JsInterop/scroll.min.js");
#endif
			}
		}

		public async ValueTask DisposeAsync()
		{
			if (_clickJs is not null)
			{
				await _clickJs.DisposeAsync();
			}
		}
	}
}