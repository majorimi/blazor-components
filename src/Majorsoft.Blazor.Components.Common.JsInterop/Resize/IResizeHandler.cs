using Majorsoft.Blazor.Components.Common.JsInterop.GlobalMouseEvents;

using Microsoft.AspNetCore.Components;

using System;
using System.Threading.Tasks;

namespace Majorsoft.Blazor.Components.Common.JsInterop.Resize
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
		Task<string> RegisterPageResizeAsync(Func<ResizeEventArgs, Task> resizeCallback);
		/// <summary>
		/// Removes event listener for 'resize' HTML event for the whole document/window by the given event Id.
		/// </summary>
		/// <param name="eventId">Event id from <see cref="RegisterPageResizeAsync"/></param>
		/// <returns>Async Task</returns>
		Task RemovePageResizeAsync(string eventId);

		/// <summary>
		/// Returns Browser Window inner size (height and width in Pixel).
		/// </summary>
		/// <returns>Async Task with Window size</returns>
		Task<PageSize> GetPageSizeAsync();

		/// <summary>
		/// Returns Browser Window screen size (height and width in Pixel).
		/// </summary>
		/// <returns>Async Task with Window size</returns>
		Task<PageSize> GetScreenSizeAsync();

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