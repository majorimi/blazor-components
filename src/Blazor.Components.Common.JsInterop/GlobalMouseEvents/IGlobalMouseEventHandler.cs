using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components.Web;

namespace Blazor.Components.Common.JsInterop.GlobalMouseEvents
{
	/// <summary>
	/// Injectable service to handle JS document/window 'mouse' events for the whole document.
	/// </summary>
	public interface IGlobalMouseEventHandler : IAsyncDisposable
	{
		/// <summary>
		/// Adds event listener for mouse 'move' HTML event for the whole document/window.
		/// </summary>
		/// <param name="mouseMoveCallback">Func to call when mouse move happened</param>
		/// <returns>Async Task with event id to unsubscribe from event</returns>
		Task<string> RegisterPageMouseMoveAsync(Func<MouseEventArgs, Task> mouseMoveCallback);
		/// <summary>
		/// Removes event listener for mouse 'move' HTML event for the whole document/window by the given event Id.
		/// </summary>
		/// <param name="eventId">Event id from <see cref="RegisterPageMouseMoveAsync"/></param>
		/// <returns>Async Task</returns>
		Task RemovePageMouseMoveAsync(string eventId);

		/// <summary>
		/// Adds event listener for mouse 'down' HTML event for the whole document/window.
		/// </summary>
		/// <param name="mouseDownCallback">Func to call when mouse down happened</param>
		/// <returns>Async Task with event id to unsubscribe from event</returns>
		Task<string> RegisterPageMouseDownAsync(Func<MouseEventArgs, Task> mouseDownCallback);
		/// <summary>
		/// Removes event listener for mouse 'down' HTML event for the whole document/window by the given event Id.
		/// </summary>
		/// <param name="eventId">Event id from <see cref="RegisterPageMouseDownAsync"/></param>
		/// <returns>Async Task</returns>
		Task RemovePageMouseDownAsync(string eventId);


		/// <summary>
		/// Adds event listener for mouse 'up' HTML event for the whole document/window.
		/// </summary>
		/// <param name="mouseUpCallback">Func to call when mouse move happened</param>
		/// <returns>Async Task with event id to unsubscribe from event</returns>
		Task<string> RegisterPageMouseUpAsync(Func<MouseEventArgs, Task> mouseUpCallback);
		/// <summary>
		/// Removes event listener for mouse 'up' HTML event for the whole document/window by the given event Id.
		/// </summary>
		/// <param name="eventId">Event id from <see cref="RegisterPageMouseUpAsync"/></param>
		/// <returns>Async Task</returns>
		Task RemovePageMouseUpAsync(string eventId);
	}
}