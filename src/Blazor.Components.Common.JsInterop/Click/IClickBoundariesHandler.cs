using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Blazor.Components.Common.JsInterop.Click
{
	/// <summary>
	/// Injectable service to handle JS 'click' events for the whole document.
	/// </summary>
	public interface IClickBoundariesHandler : IAsyncDisposable
	{
		/// <summary>
		/// Adds event listener for 'click' HTML event for the given element with property filter.
		/// </summary>
		/// <param name="elementRef">Blazor reference to an HTML element</param>
		/// <param name="outsideClickCallback">Func to call when clicked outside of the given element</param>
		/// <param name="insideClickCallback">Func to call when clicked inside of the given element</param>
		/// <returns>Async Task</returns>
		Task RegisterClickBoundariesAsync(ElementReference elementRef, Func<MouseEventArgs, Task> outsideClickCallback = null, Func<MouseEventArgs, Task> insideClickCallback = null);

		/// <summary>
		/// Removes event listener for 'click' HTML event for the given element.
		/// </summary>
		/// <param name="elementRef">Blazor reference to an HTML element</param>
		/// <returns>Async Task</returns>
		Task RemoveClickBoundariesAsync(ElementReference elementRef);
	}
}