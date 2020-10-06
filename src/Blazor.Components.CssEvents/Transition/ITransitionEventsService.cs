using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

namespace Blazor.Components.CssEvents.Transition
{
	/// <summary>
	/// Injectable service to handle CSS Transition events
	/// </summary>
	public interface ITransitionEventsService : IAsyncDisposable
	{
		/// <summary>
		/// Adds event listener for 'transitionend' HTML event for the given element with property filter.
		/// </summary>
		/// <param name="elementRef">Blazor reference to an HTML element</param>
		/// <param name="onEndedCallback">Func to call when Transition event has finished</param>
		/// <param name="transitionPropertyName">Transition property name for filter event</param>
		/// <returns>Async Task</returns>
		Task RegisterTransitionEndedAsync(ElementReference elementRef, Func<TransitionEndEventArgs, Task> onEndedCallback, string transitionPropertyName = "");

		/// <summary>
		/// Adds event listeners for 'transitionend' HTML event for the given elements with property filters.
		/// </summary>
		/// <param name="onEndedCallback">Func to call when all Transition events has finished</param>
		/// <param name="elementRefsWithProperties">Params KeyValuePair with Blazor reference to an HTML element and property name for filter event</param>
		/// <returns>Async Task</returns>
		Task RegisterTransitionsWhenAllEndedAsync(Func<TransitionEndEventArgs[], Task> onEndedCallback, params KeyValuePair<ElementReference, string>[] elementRefsWithProperties);

		/// <summary>
		/// Removes event listener for 'transitionend' HTML event for the given element with property filter.
		/// </summary>
		/// <param name="elementRef">Blazor reference to an HTML element</param>
		/// <param name="transitionPropertyName">Transition property name for filter event</param>
		/// <returns>Async Task</returns>
		Task RemoveTransitionEndedAsync(ElementReference elementRef, string transitionPropertyName = "");

		/// <summary>
		/// Removes event listeners for 'transitionend' HTML event for the given elements with property filters.
		/// </summary>
		/// <param name="elementRefsWithProperties">Params KeyValuePair with Blazor reference to an HTML element and property name for filter event</param>
		/// <returns>Async Task</returns>
		Task RemoveTransitionsWhenAllEndedAsync(params KeyValuePair<ElementReference, string>[] elementRefsWithProperties);
	}
}