using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Components.CssEvents.Transition
{
	/// <summary>
	/// Implementation of <see cref="ITransitionEventsService"/>
	/// </summary>
	public sealed class TransitionEventsService : ITransitionEventsService
	{
		private readonly IJSRuntime _jsRuntime;
		private List<DotNetObjectReference<TransitionEventInfo>> _dotNetObjectReferences;
		private IJSObjectReference _transitionJs;

		public TransitionEventsService(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime;
			_dotNetObjectReferences = new List<DotNetObjectReference<TransitionEventInfo>>();
		}

		public async Task RegisterTransitionEndedAsync(ElementReference elementRef, Func<TransitionEventArgs, Task> onEndedCallback, string transitionPropertyName = "")
		{
			await CheckJsObjectAsync();

			var info = new TransitionEventInfo(elementRef, onEndedCallback, transitionPropertyName);
			var dotnetRef = DotNetObjectReference.Create<TransitionEventInfo>(info);

			_dotNetObjectReferences.Add(dotnetRef);
			await _transitionJs.InvokeVoidAsync("addTransitionEnd", elementRef, dotnetRef, transitionPropertyName);
		}

		public async Task RemoveTransitionEndedAsync(ElementReference elementRef, string transitionPropertyName = "")
		{
			await CheckJsObjectAsync();

			await _transitionJs.InvokeVoidAsync("removeTransitionEnd", elementRef, transitionPropertyName);
			RemoveElement(elementRef, transitionPropertyName);
		}

		public async Task RegisterTransitionsWhenAllEndedAsync(Func<TransitionEventArgs[], Task> onEndedCallback, params KeyValuePair<ElementReference, string>[] elementRefsWithProperties)
		{
			await CheckJsObjectAsync();

			var collection = new TransitionCollectionInfo(onEndedCallback);
			foreach (var item in elementRefsWithProperties)
			{
				var info = new TransitionEventInfo(item.Key, collection.WhenAllFinished, item.Value);
				collection.Add(info);
				var dotnetRef = DotNetObjectReference.Create<TransitionEventInfo>(info);

				_dotNetObjectReferences.Add(dotnetRef);
				await _transitionJs.InvokeVoidAsync("addTransitionEnd", item.Key, dotnetRef, item.Value);
			}
		}

		public async Task RemoveTransitionsWhenAllEndedAsync(params KeyValuePair<ElementReference, string>[] elementRefsWithProperties)
		{
			await CheckJsObjectAsync();

			foreach (var item in elementRefsWithProperties)
			{
				await _transitionJs.InvokeVoidAsync("removeTransitionEnd", item.Key, item.Value);
				RemoveElement(item.Key, item.Value);
			}
		}

		private void RemoveElement(ElementReference elementRef, string transitionPropertyName)
		{
			var dotNetRefs = _dotNetObjectReferences
				.Where(x => x.Value.Element.Equals(elementRef) && x.Value.TransitionPropertyName == transitionPropertyName);
			_dotNetObjectReferences = _dotNetObjectReferences.Except(dotNetRefs).ToList();

			foreach (var item in dotNetRefs)
			{
				item.Dispose();
			}
		}

		private async Task CheckJsObjectAsync()
		{
			if (_transitionJs is null)
			{
#if DEBUG
				_transitionJs = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Majorsoft.Blazor.Components.CssEvents/transitionEvents.js");
#else
				_transitionJs = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Majorsoft.Blazor.Components.CssEvents/transitionEvents.min.js");
#endif
			}
		}

		public async ValueTask DisposeAsync()
		{
			if (_transitionJs is not null)
			{
				var registeredElements = _dotNetObjectReferences
					.Select(s => new KeyValuePair<ElementReference, string>(s.Value.Element, s.Value.TransitionPropertyName));
				await _transitionJs.InvokeVoidAsync("dispose", registeredElements.ToArray());

				await _transitionJs.DisposeAsync();
			}

			foreach (var item in _dotNetObjectReferences)
			{
				item.Dispose();
			}
		}
	}
}