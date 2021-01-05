using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

namespace Majorsoft.Blazor.Components.Common.JsInterop.Scroll
{
	/// <summary>
	/// Injectable service to handle JS document/window 'scroll' events for the whole document.
	/// </summary>
	public interface IScrollHandler : IAsyncDisposable
	{
		/// <summary>
		/// Scrolls the given element into the page view area.
		/// </summary>
		/// <param name="elementReference">Blazor reference to an HTML element</param>
		/// <returns>Async Task</returns>
		Task ScrollToElementAsync(ElementReference elementReference);
		/// <summary>
		/// Finds element by Id and scrolls the given element into the page view area.
		/// </summary>
		/// <param name="name">DOM element id</param>
		/// <returns>Async Task</returns>
		Task ScrollToElementByIdAsync(string id);
		/// <summary>
		/// Finds element by name and scrolls the given element into the page view area.
		/// </summary>
		/// <param name="name">DOM element name</param>
		/// <returns>Async Task</returns>
		Task ScrollToElementByNameAsync(string name);

		/// <summary>
		/// Scrolls to end of the page (X bottom).
		/// </summary>
		/// <returns>Async Task</returns>
		Task ScrollToPageEndAsync();
		/// <summary>
		/// Scrolls to top of the page (X top).
		/// </summary>
		/// <returns>Async Task</returns>
		Task ScrollToPageTopAsync();
		/// <summary>
		/// Scrolls to X position on the page.
		/// </summary>
		/// <param name="x">Scroll top x value</param>
		/// <returns>Async Task</returns>
		Task ScrollToPageXAsync(double x);
		/// <summary>
		/// Scrolls to Y position on the page.
		/// </summary>
		/// <param name="x">Scroll top x value</param>
		/// <returns>Async Task</returns>
		Task ScrollToPageYAsync(double y);
		/// <summary>
		/// Returns page X,Y scroll position as <see cref="ScrollEventArgs"/>.
		/// </summary>
		/// <returns>Async Task</returns>
		Task<ScrollEventArgs> GetPageScrollPosAsync();

		/// <summary>
		/// Adds event listener for 'scroll' HTML event for the whole document/window.
		/// </summary>
		/// <param name="scrollCallback">Func to call when scroll happened</param>
		/// <returns>Async Task with event id to unsubscribe from event</returns>
		Task<string> RegisterPageScrollAsync(Func<ScrollEventArgs, Task> scrollCallback);
		/// <summary>
		/// Removes event listener for 'scroll' HTML event for the whole document/window by the given event Id.
		/// </summary>
		/// <param name="eventId">Event id from <see cref="RegisterPageScrollAsync"/></param>
		/// <returns>Async Task</returns>
		Task RemovePageScrollAsync(string eventId);
	}
}