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
		/// 
		/// </summary>
		/// <param name="elementRef"></param>
		/// <param name="onEndedCallback"></param>
		/// <param name="transitionPropertyName"></param>
		/// <returns></returns>
		Task RegisterTransitionEnded(ElementReference elementRef, Func<TransitionEndEventArgs, Task> onEndedCallback, string transitionPropertyName = "");

		/// <summary>
		/// 
		/// </summary>
		/// <param name="onEndedCallback"></param>
		/// <param name="elementRefsWithProperties"></param>
		/// <returns></returns>
		Task RegisterTransitionsWhenAllEnded(Func<TransitionEndEventArgs[], Task> onEndedCallback, params KeyValuePair<ElementReference, string>[] elementRefsWithProperties);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="elementRef"></param>
		/// <param name="transitionPropertyName"></param>
		/// <returns></returns>
		Task RemoveTransitionEnded(ElementReference elementRef, string transitionPropertyName = "");

		/// <summary>
		/// 
		/// </summary>
		/// <param name="elementRefsWithProperties"></param>
		/// <returns></returns>
		Task RemoveTransitionsWhenAllEnded(params KeyValuePair<ElementReference, string>[] elementRefsWithProperties);
	}
}