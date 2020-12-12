using Blazor.Components.Common.JsInterop.GlobalMouseEvents;

using Microsoft.AspNetCore.Components;

using System;
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
}