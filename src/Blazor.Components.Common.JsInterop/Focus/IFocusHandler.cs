using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor.Components.Common.JsInterop.Focus
{
	/// <summary>
	/// Injectable service to handle JS 'focus' Interops.
	/// </summary>
	public interface IFocusHandler : IAsyncDisposable
	{
		/// <summary>
		/// Returns the actually focused HTML DOM element reference. It works with outside element of a Blazor component.
		/// Note: <see cref="IJSObjectReference"/> object is disposable.
		/// </summary>
		/// <returns>Async Task</returns>
		Task<IJSObjectReference> GetFocusedElementAsync();

		/// <summary>
		/// Sets focus on the given HTML DOM element reference
		/// Note: <see cref="IJSObjectReference"/> object is disposable.
		/// </summary>
		/// <param name="objectReference">IJSObjectReference to set focus on</param>
		/// <returns>Async Task</returns>
		Task FocusElementAsync(IJSObjectReference objectReference);

		/// <summary>
		/// Sets focus on the given HTML DOM element reference
		/// </summary>
		/// <param name="elementReference">ElementReference to set focus on</param>
		/// <returns>Async Task</returns>
		Task FocusElementAsync(ElementReference elementReference);

		/// <summary>
		/// Stores the actually focused HTML DOM element reference into a JS variable. This can be restored by calling <code>RestoreStoredElementFocusAsync</code> method.
		/// </summary>
		/// <returns>Async Task</returns>
		Task StoreFocusedElementAsync();

		/// <summary>
		/// Restores the HTML DOM element reference stored by calling <code>StoreFocusedElementAsync</code> method.
		/// </summary>
		/// <returns>Async Task</returns>
		Task RestoreStoredElementFocusAsync(bool clearElementRef = true);
	}
}
