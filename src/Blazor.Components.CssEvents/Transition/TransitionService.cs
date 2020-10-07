using System;
using System.Linq;
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
		private List<KeyValuePair<ElementReference, string>> _registeredElements;
		private JSObjectReference _transitionJs;

		public TransitionEventsService(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime;
			_registeredElements = new List<KeyValuePair<ElementReference, string>>();
		}

		public async Task RegisterTransitionEndedAsync(ElementReference elementRef, Func<TransitionEndEventArgs, Task> onEndedCallback, string transitionPropertyName = "")
		{
			await CheckJsObjectAsync();

			var info = new TransitionInfo(elementRef, onEndedCallback, transitionPropertyName);
			var dotnetRef = DotNetObjectReference.Create<TransitionInfo>(info);

			await _transitionJs.InvokeVoidAsync("addTransitionEnd", elementRef, dotnetRef, transitionPropertyName);
			_registeredElements.Add(new KeyValuePair<ElementReference, string>(elementRef, transitionPropertyName));
		}

		public async Task RemoveTransitionEndedAsync(ElementReference elementRef, string transitionPropertyName = "")
		{
			await CheckJsObjectAsync();

			await _transitionJs.InvokeVoidAsync("removeTransitionEnd", elementRef, transitionPropertyName);
			RemoveElement(elementRef, transitionPropertyName);
		}

		public async Task RegisterTransitionsWhenAllEndedAsync(Func<TransitionEndEventArgs[], Task> onEndedCallback, params KeyValuePair<ElementReference, string>[] elementRefsWithProperties)
		{
			await CheckJsObjectAsync();

			var collection = new TransitionCollectionInfo(onEndedCallback);
			foreach (var item in elementRefsWithProperties)
			{
				var info = new TransitionInfo(item.Key, collection.WhenAllFinished, item.Value);
				collection.Add(info);
				var dotnetRef = DotNetObjectReference.Create<TransitionInfo>(info);

				await _transitionJs.InvokeVoidAsync("addTransitionEnd", item.Key, dotnetRef, item.Value);
				_registeredElements.Add(new KeyValuePair<ElementReference, string>(item.Key, item.Value));
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
			var items = _registeredElements.Where(x => x.Key.Equals(elementRef) && x.Value == transitionPropertyName);

			_registeredElements = _registeredElements.Except(items).ToList();
		}

		private async Task CheckJsObjectAsync()
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
				await _transitionJs.InvokeVoidAsync("dispose", _registeredElements.ToArray());

				await _transitionJs.DisposeAsync();
			}
		}
	}
}