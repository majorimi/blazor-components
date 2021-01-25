using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Components.CssEvents.Animation
{
	/// <summary>
	/// Implementation of <see cref="IAnimationEventsService"/>
	/// </summary>
	public sealed class AnimationEventsService  : IAnimationEventsService
	{
		private readonly IJSRuntime _jsRuntime;
		private List<DotNetObjectReference<AnimationEventInfo>> _registeredStartEvents;
		private List<DotNetObjectReference<AnimationEventInfo>> _registeredIterationEvents;
		private List<DotNetObjectReference<AnimationEventInfo>> _registeredEndEvents;
		private IJSObjectReference _animationJs;

		public AnimationEventsService(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime;

			_registeredStartEvents = new List<DotNetObjectReference<AnimationEventInfo>>();
			_registeredIterationEvents = new List<DotNetObjectReference<AnimationEventInfo>>();
			_registeredEndEvents = new List<DotNetObjectReference<AnimationEventInfo>>();
		}

		public async Task RegisterAllAnimationEventsAsync(ElementReference elementRef, Func<AnimationEventArgs, Task> onEventCallback, string animationName = "")
		{
			await RegisterAnimationStartedAsync(elementRef, onEventCallback, animationName);
			await RegisterAnimationIterationAsync(elementRef, onEventCallback, animationName);
			await RegisterAnimationEndedAsync(elementRef, onEventCallback, animationName);
		}
		public async Task RegisterAllAnimationEventsAsync(ElementReference elementRef, Func<AnimationEventArgs, Task> onStartedCallback, Func<AnimationEventArgs, Task> onIterationCallback, Func<AnimationEventArgs, Task> onEndedCallback, string animationName = "")
		{
			await RegisterAnimationStartedAsync(elementRef, onStartedCallback, animationName);
			await RegisterAnimationIterationAsync(elementRef, onIterationCallback, animationName);
			await RegisterAnimationEndedAsync(elementRef, onEndedCallback, animationName);
		}
		public async Task RemoveAllAnimationEventsAsync(ElementReference elementRef, string animationName = "")
		{
			await RemoveAnimationStartedAsync(elementRef, animationName);
			await RemoveAnimationIterationAsync(elementRef, animationName);
			await RemoveAnimationEndedAsync(elementRef, animationName);
		}

		public async Task RegisterAnimationStartedAsync(ElementReference elementRef, Func<AnimationEventArgs, Task> onStartedCallback, string animationName = "")
		{
			await RegisterEventAsync("addAnimationStart", _registeredStartEvents, elementRef, onStartedCallback, animationName);
		}
		public async Task RemoveAnimationStartedAsync(ElementReference elementRef, string animationName = "")
		{
			await RemoveEventAsync("removeAnimationStart", _registeredStartEvents, elementRef, animationName);
		}

		public async Task RegisterAnimationIterationAsync(ElementReference elementRef, Func<AnimationEventArgs, Task> onIterationCallback, string animationName = "")
		{
			await RegisterEventAsync("addAnimationIteration", _registeredIterationEvents, elementRef, onIterationCallback, animationName);
		}
		public async Task RemoveAnimationIterationAsync(ElementReference elementRef, string animationName = "")
		{
			await RemoveEventAsync("removeAnimationIteration", _registeredIterationEvents, elementRef, animationName);
		}

		public async Task RegisterAnimationEndedAsync(ElementReference elementRef, Func<AnimationEventArgs, Task> onEndedCallback, string animationName = "")
		{
			await RegisterEventAsync("addAnimationEnd", _registeredEndEvents, elementRef, onEndedCallback, animationName);
		}
		public async Task RemoveAnimationEndedAsync(ElementReference elementRef, string animationName = "")
		{
			await RemoveEventAsync("removeAnimationEnd", _registeredEndEvents, elementRef, animationName);
		}

		public async Task RegisterAnimationsWhenAllEndedAsync(Func<AnimationEventArgs[], Task> onEndedCallback, params KeyValuePair<ElementReference, string>[] elementRefsWithProperties)
		{
			await CheckJsObjectAsync();

			var collection = new AnimationCollectionInfo(onEndedCallback);
			foreach (var item in elementRefsWithProperties)
			{
				var info = new AnimationEventInfo(item.Key, collection.WhenAllFinished, item.Value);
				collection.Add(info);
				var dotnetRef = DotNetObjectReference.Create<AnimationEventInfo>(info);
				_registeredEndEvents.Add(dotnetRef);

				await _animationJs.InvokeVoidAsync("addAnimationEnd", item.Key, dotnetRef, item.Value);
			}
		}
		public async Task RemoveAnimationsWhenAllEndedAsync(params KeyValuePair<ElementReference, string>[] elementRefsWithProperties)
		{
			await CheckJsObjectAsync();

			foreach (var item in elementRefsWithProperties)
			{
				await _animationJs.InvokeVoidAsync("removeAnimationEnd", item.Key, item.Value);
				RemoveElementFromList(_registeredEndEvents, item.Key, item.Value);
			}
		}

		private async Task RegisterEventAsync(string jsMethodName, 
			List<DotNetObjectReference<AnimationEventInfo>> registeredEvents, 
			ElementReference elementRef,
			Func<AnimationEventArgs, Task> onEventCallback,
			string animationName)
		{
			await CheckJsObjectAsync();

			var info = new AnimationEventInfo(elementRef, onEventCallback, animationName);
			var dotnetRef = DotNetObjectReference.Create<AnimationEventInfo>(info);
			registeredEvents.Add(dotnetRef);

			await _animationJs.InvokeVoidAsync(jsMethodName, elementRef, dotnetRef, animationName);
		}
		private async Task RemoveEventAsync(string methodName, 
			List<DotNetObjectReference<AnimationEventInfo>> registeredEvents, 
			ElementReference elementRef, 
			string animationName)
		{
			await CheckJsObjectAsync();

			await _animationJs.InvokeVoidAsync(methodName, elementRef, animationName);
			RemoveElementFromList(registeredEvents, elementRef, animationName);
		}

		private void RemoveElementFromList(List<DotNetObjectReference<AnimationEventInfo>> registeredEvents,
			ElementReference elementRef, 
			string animationName)
		{
			var dotNetRefs = registeredEvents
				.Where(x => x.Value.Element.Equals(elementRef) && x.Value.AnimationName == animationName);
			registeredEvents = registeredEvents.Except(dotNetRefs).ToList();

			foreach (var item in dotNetRefs)
			{
				item.Dispose();
			}
		}

		private async Task CheckJsObjectAsync()
		{
			if (_animationJs is null)
			{
#if DEBUG
				_animationJs = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Majorsoft.Blazor.Components.CssEvents/animationEvents.js");
#else
				_animationJs = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Majorsoft.Blazor.Components.CssEvents/animationEvents.min.js");
#endif
			}
		}

		public async ValueTask DisposeAsync()
		{
			if (_animationJs is not null)
			{
				var start = _registeredStartEvents
					.Select(s => new KeyValuePair<ElementReference, string>(s.Value.Element, s.Value.AnimationName));
				var iteration = _registeredIterationEvents
					.Select(s => new KeyValuePair<ElementReference, string>(s.Value.Element, s.Value.AnimationName));
				var end = _registeredEndEvents
					.Select(s => new KeyValuePair<ElementReference, string>(s.Value.Element, s.Value.AnimationName));

				await _animationJs.InvokeVoidAsync("dispose", start.ToArray(), iteration.ToArray(), end.ToArray());

				await _animationJs.DisposeAsync();
			}

			foreach (var item in _registeredStartEvents)
			{
				item.Dispose();
			}
			foreach (var item in _registeredIterationEvents)
			{
				item.Dispose();
			}
			foreach (var item in _registeredEndEvents)
			{
				item.Dispose();
			}
		}
	}
}