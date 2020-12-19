using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Components;

namespace Majorsoft.Blazor.Components.CssEvents.Animation
{
	/// <summary>
	/// Injectable service to handle CSS Animation events
	/// </summary>
	public interface IAnimationEventsService : IAsyncDisposable
	{
		//Single events

		/// <summary>
		/// Adds event listener for 'animationstart' HTML event for the given element with Animation name filter.
		/// </summary>
		/// <param name="elementRef">Blazor reference to an HTML element</param>
		/// <param name="onStartedCallback">Func to call when Animation event has started fired</param>
		/// <param name="animationName">Animation name for filter event</param>
		/// <returns>Async Task</returns>
		Task RegisterAnimationStartedAsync(ElementReference elementRef, Func<AnimationEventArgs, Task> onStartedCallback, string animationName = "");

		/// <summary>
		/// Removes event listener for 'animationstart' HTML event for the given element with Animation name filter.
		/// </summary>
		/// <param name="elementRef">Blazor reference to an HTML element</param>
		/// <param name="animationName">Animation name for filter event</param>
		/// <returns>Async Task</returns>
		Task RemoveAnimationStartedAsync(ElementReference elementRef, string animationName = "");

		/// <summary>
		/// Adds event listener for 'animationiteration' HTML event for the given element with Animation name filter.
		/// </summary>
		/// <param name="elementRef">Blazor reference to an HTML element</param>
		/// <param name="onIterationCallback">Func to call when Animation event has started new iteration fired</param>
		/// <param name="animationName">Animation name for filter event</param>
		/// <returns>Async Task</returns>
		Task RegisterAnimationIterationAsync(ElementReference elementRef, Func<AnimationEventArgs, Task> onIterationCallback, string animationName = "");

		/// <summary>
		/// Removes event listener for 'animationiteration' HTML event for the given element with Animation name filter.
		/// </summary>
		/// <param name="elementRef">Blazor reference to an HTML element</param>
		/// <param name="animationName">Animation name for filter event</param>
		/// <returns>Async Task</returns>
		Task RemoveAnimationIterationAsync(ElementReference elementRef, string animationName = "");

		/// <summary>
		/// Adds event listener for 'animationend' HTML event for the given element with Animation name filter.
		/// </summary>
		/// <param name="elementRef">Blazor reference to an HTML element</param>
		/// <param name="onEndedCallback">Func to call when Animation event has finished fired</param>
		/// <param name="animationName">Animation name for filter event</param>
		/// <returns>Async Task</returns>
		Task RegisterAnimationEndedAsync(ElementReference elementRef, Func<AnimationEventArgs, Task> onEndedCallback, string animationName = "");

		/// <summary>
		/// Removes event listener for 'animationend' HTML event for the given element with Animation name filter.
		/// </summary>
		/// <param name="elementRef">Blazor reference to an HTML element</param>
		/// <param name="animationName">Animation name for filter event</param>
		/// <returns>Async Task</returns>
		Task RemoveAnimationEndedAsync(ElementReference elementRef, string animationName = "");


		//Composits

		/// <summary>
		/// Adds event listeners with single callback for all supported HTML events for the given element with Animation name filter.
		/// </summary>
		/// <param name="elementRef">Blazor reference to an HTML element</param>
		/// <param name="onEventCallback">Func to call when any Animation event fired</param>
		/// <param name="animationName">Animation name for filter event</param>
		/// <returns>Async Task</returns>
		Task RegisterAllAnimationEventsAsync(ElementReference elementRef, Func<AnimationEventArgs, Task> onEventCallback, string animationName = "");

		/// <summary>
		/// Adds event listeners with different callbacks for all supported HTML events for the given element with Animation name filter.
		/// </summary>
		/// <param name="elementRef">Blazor reference to an HTML element</param>
		/// <param name="onStartedCallback">Func to call when Animation event has started fired</param>
		/// <param name="onIterationCallback">Func to call when Animation event has started new iteration fired</param>
		/// <param name="onEndedCallback">Func to call when Animation event has finished fired</param>
		/// <param name="animationName">Animation name for filter event</param>
		/// <returns>Async Task</returns>
		Task RegisterAllAnimationEventsAsync(ElementReference elementRef,
			Func<AnimationEventArgs, Task> onStartedCallback,
			Func<AnimationEventArgs, Task> onIterationCallback,
			Func<AnimationEventArgs, Task> onEndedCallback,
			string animationName = "");

		/// <summary>
		/// Removes event listener for all supported HTML event for the given element with Animation name filter.
		/// </summary>
		/// <param name="elementRef">Blazor reference to an HTML element</param>
		/// <param name="animationName">Animation name for filter event</param>
		/// <returns>Async Task</returns>
		Task RemoveAllAnimationEventsAsync(ElementReference elementRef, string animationName = "");

		/// <summary>
		/// Adds event listeners for 'animationend' HTML event for the given elements with Animation names filters.
		/// </summary>
		/// <param name="onEndedCallback">Func to call when ALL Animation events has finished</param>
		/// <param name="elementRefsWithProperties">Params KeyValuePair with Blazor reference to an HTML element and Animation name for filter event</param>
		/// <returns>Async Task</returns>
		Task RegisterAnimationsWhenAllEndedAsync(Func<AnimationEventArgs[], Task> onEndedCallback, params KeyValuePair<ElementReference, string>[] elementRefsWithProperties);

		/// <summary>
		/// Removes event listeners for 'animationend' HTML event for the given elements with Animation names filters.
		/// </summary>
		/// <param name="elementRefsWithProperties">Params KeyValuePair with Blazor reference to an HTML element and Animation name for filter event</param>
		/// <returns>Async Task</returns>
		Task RemoveAnimationsWhenAllEndedAsync(params KeyValuePair<ElementReference, string>[] elementRefsWithProperties);
	}
}