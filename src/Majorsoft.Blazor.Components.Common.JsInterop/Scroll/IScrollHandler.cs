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
		/// <param name="smooth">Scroll should jump or smoothly scroll Note: might not all browsers support it</param>
		/// <returns>Async Task</returns>
		Task ScrollToElementAsync(ElementReference elementReference, bool smooth = false);
		/// <summary>
		/// Finds element by Id and scrolls the given element into the page view area.
		/// </summary>
		/// <param name="id">DOM element id</param>
		/// <param name="smooth">Scroll should jump or smoothly scroll Note: might not all browsers support it</param>
		/// <returns>Async Task</returns>
		Task ScrollToElementByIdAsync(string id, bool smooth = false);
		/// <summary>
		/// Finds element by name and scrolls the given element into the page view area.
		/// </summary>
		/// <param name="name">DOM element name</param>
		/// <param name="smooth">Scroll should jump or smoothly scroll Note: might not all browsers support it</param>
		/// <returns>Async Task</returns>
		Task ScrollToElementByNameAsync(string name, bool smooth = false);

		/// <summary>
		/// Scrolls to end of the page (X bottom).
		/// </summary>
		/// <param name="smooth">Scroll should jump or smoothly scroll</param>
		/// <returns>Async Task</returns>
		Task ScrollToPageEndAsync(bool smooth = false);
		/// <summary>
		/// Scrolls to top of the page (X top).
		/// </summary>
		/// <param name="smooth">Scroll should jump or smoothly scroll</param>
		/// <returns>Async Task</returns>
		Task ScrollToPageTopAsync(bool smooth = false);
		/// <summary>
		/// Scrolls to X position on the page.
		/// </summary>
		/// <param name="x">Scroll top x value</param>
		/// <param name="smooth">Scroll should jump or smoothly scroll</param>
		/// <returns>Async Task</returns>
		Task ScrollToPageXAsync(double x, bool smooth = false);
		/// <summary>
		/// Scrolls to Y position on the page.
		/// </summary>
		/// <param name="y">Scroll top x value</param>
		/// <param name="smooth">Scroll should jump or smoothly scroll</param>
		/// <returns>Async Task</returns>
		Task ScrollToPageYAsync(double y, bool smooth = false);
		/// <summary>
		/// Returns page X,Y scroll current position as <see cref="ScrollResult"/>.
		/// </summary>
		/// <returns>Async Task</returns>
		Task<ScrollResult> GetPageScrollPosAsync();
		/// <summary>
		/// Returns page X,Y scroll size (max values) as <see cref="ScrollResult"/>.
		/// </summary>
		/// <returns>Async Task</returns>
		Task<ScrollResult> GetPageScrollSizeAsync();

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