using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor.Components.CssEvents.Transition
{
	/// <summary>
	/// Implementation of <see cref="ITransitionEventsService"/>
	/// </summary>
	public class TransitionEventsService : ITransitionEventsService
	{
		private readonly IJSRuntime _jsRuntime;
		private JSObjectReference _transitionJs;

		public TransitionEventsService(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime;
		}

		public async Task RegisterTransitionEnded(ElementReference elementRef, Func<TransitionEndEventArgs, Task> onEndedCallback, string transitionPropertyName = "")
		{
			await CheckJsObject();

			var info = new TransitionInfo(elementRef, onEndedCallback, transitionPropertyName);
			var dotnetRef = DotNetObjectReference.Create<TransitionInfo>(info);

			await _transitionJs.InvokeVoidAsync("addTransitionEnd", elementRef, dotnetRef, transitionPropertyName);
		}

		public async Task RemoveTransitionEnded(ElementReference elementRef, string transitionPropertyName = "")
		{
			await CheckJsObject();

			await _transitionJs.InvokeVoidAsync("removeTransitionEnd", elementRef, transitionPropertyName);
		}

		public async Task RegisterTransitionsWhenAllEnded(Func<TransitionEndEventArgs[], Task> onEndedCallback, params KeyValuePair<ElementReference, string>[] elementRefsWithProperties)
		{
			await CheckJsObject();

			var collection = new TransitionCollectionInfo(onEndedCallback);
			foreach (var item in elementRefsWithProperties)
			{
				var info = new TransitionInfo(item.Key, collection.WhenAllFinished, item.Value);
				collection.Add(info);
				var dotnetRef = DotNetObjectReference.Create<TransitionInfo>(info);

				await _transitionJs.InvokeVoidAsync("addTransitionEnd", item.Key, dotnetRef, item.Value);
			}
		}

		public async Task RemoveTransitionsWhenAllEnded(params KeyValuePair<ElementReference, string>[] elementRefsWithProperties)
		{
			await CheckJsObject();

			foreach (var item in elementRefsWithProperties)
			{
				await _transitionJs.InvokeVoidAsync("removeTransitionEnd", item.Key, item.Value);
			}
		}

		private async Task CheckJsObject()
		{
			if (_transitionJs is null)
			{
#if DEBUG
				_transitionJs = await _jsRuntime.InvokeAsync<JSObjectReference>("import", "./_content/Majorsoft.Blazor.Components.CssEvents/transitionEvents.js");
#else
				_transitionJs = await _jsRuntime.InvokeAsync<JSObjectReference>("import", "./_content/Majorsoft.Blazor.Components.CssEvents/transitionEvents.min.js");
#endif
			}
		}

		public async ValueTask DisposeAsync()
		{
			if (_transitionJs is not null)
			{
				await _transitionJs.InvokeVoidAsync("dispose");

				await _transitionJs.DisposeAsync();
			}
		}
	}
}