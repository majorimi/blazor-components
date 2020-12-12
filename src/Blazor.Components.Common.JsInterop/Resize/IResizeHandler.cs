using Blazor.Components.Common.JsInterop.GlobalMouseEvents;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.Components.Common.JsInterop.Resize
{
	/// <summary>
	/// Injectable service to handle JS 'resize' events for HTML element or the whole document.
	/// </summary>
	public interface IResizeHandler : IAsyncDisposable
	{
		/// <summary>
		/// Adds event listener for 'resize' HTML event for the whole document/window.
		/// </summary>
		/// <param name="resizeCallback">Func to call when page resize happened</param>
		/// <returns>Async Task with event id to unsubscribe from event</returns>
		Task<string> RegisterPageMouseMoveAsync(Func<ResizeEventArgs, Task> resizeCallback);
		/// <summary>
		/// Removes event listener for 'resize' HTML event for the whole document/window by the given event Id.
		/// </summary>
		/// <param name="eventId">Event id from <see cref="RegisterPageMouseMoveAsync"/></param>
		/// <returns>Async Task</returns>
		Task RemovePageMouseMoveAsync(string eventId);

		/// <summary>
		/// Adds event listener for 'resize' HTML event for the given element with property filter.
		/// </summary>
		/// <param name="elementRef">Blazor reference to an HTML element</param>
		/// <param name="resizeCallback">Func to call when the given element was resized</param>
		/// <returns>Async Task</returns>
		Task RegisterResizeAsync(ElementReference elementRef, Func<ResizeEventArgs, Task> resizeCallback = null);

		/// <summary>
		/// Removes event listener for 'resize' HTML event for the given element.
		/// </summary>
		/// <param name="elementRef">Blazor reference to an HTML element</param>
		/// <returns>Async Task</returns>
		Task RemoveResizeAsync(ElementReference elementRef);
	}

	/// <summary>
	/// Implementation of <see cref="IResizeHandler"/>
	/// </summary>
	public class ResizeHandler : IResizeHandler
	{
		private readonly IJSRuntime _jsRuntime;
		private List<ElementReference> _registeredElements;
		private IJSObjectReference _clickJs;

		public ResizeHandler(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime;
			_registeredElements = new List<ElementReference>();
		}

		public async Task RegisterResizeAsync(ElementReference elementRef, Func<ResizeEventArgs, Task> resizeCallback = null)
		{
			await CheckJsObjectAsync();

			var info = new PageResizeEventInfo(resizeCallback, null);
			var dotnetRef = DotNetObjectReference.Create<PageResizeEventInfo>(info);

			await _clickJs.InvokeVoidAsync("", elementRef, dotnetRef);
			_registeredElements.Add(elementRef);
		}

		public async Task RemoveResizeAsync(ElementReference elementRef)
		{
			await CheckJsObjectAsync();

			await _clickJs.InvokeVoidAsync("", elementRef);
			RemoveElement(elementRef);
		}

		private void RemoveElement(ElementReference elementRef)
		{
			var items = _registeredElements.Where(x => x.Equals(elementRef));

			_registeredElements = _registeredElements.Except(items).ToList();
		}

		private async Task CheckJsObjectAsync()
		{
			if (_clickJs is null)
			{
#if DEBUG
				_clickJs = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Majorsoft.Blazor.Components.Common.JsInterop/resize.js");
#else
				_clickJs = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Majorsoft.Blazor.Components.Common.JsInterop/resize.min.js");
#endif
			}
		}

		public async ValueTask DisposeAsync()
		{
			if (_clickJs is not null)
			{
				await _clickJs.InvokeVoidAsync("dispose", _registeredElements.ToArray());

				await _clickJs.DisposeAsync();
			}
		}
	}
}