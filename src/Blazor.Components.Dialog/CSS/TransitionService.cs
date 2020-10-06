﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor.Components.Dialog.CSS
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
				_transitionJs = await _jsRuntime.InvokeAsync<JSObjectReference>("import", "./_content/Majorsoft.Blazor.Components.Dialog/transitionEvents.js");
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

	public class TransitionCollectionInfo : Collection<TransitionInfo>
	{
		private readonly Func<TransitionEndEventArgs[], Task> _transitionEndedCallback;

		public TransitionCollectionInfo(Func<TransitionEndEventArgs[], Task> transitionEndedCallback)
		{
			_transitionEndedCallback = transitionEndedCallback;
		}

		private List<TransitionEndEventArgs> _finishedTransitions = new List<TransitionEndEventArgs>();
		public async Task WhenAllFinished(TransitionEndEventArgs args)
		{
			_finishedTransitions.Add(args);

			if (this.Count == _finishedTransitions.Count)
			{
				await _transitionEndedCallback(_finishedTransitions.ToArray());
				_finishedTransitions.Clear();
			}
		}
	}

	public class TransitionInfo
	{
		private readonly Func<TransitionEndEventArgs, Task> _transitionEndedCallback;
		private readonly ElementReference _element;
		private readonly string _transitionPropertyName;

		public TransitionInfo(ElementReference element, Func<TransitionEndEventArgs, Task> transitionEndedCallback, string transitionPropertyName)
		{
			_element = element;
			_transitionPropertyName = transitionPropertyName;
			_transitionEndedCallback = transitionEndedCallback;
		}

		[JSInvokable("TransitionEnded")]
		public async Task TransitionEnded(TransitionEndEventArgs args)
		{
			args.Element = _element;
			args.OriginalPropertyNameFilter = _transitionPropertyName;

			await _transitionEndedCallback(args);
		}
	}

	public class TransitionEndEventArgs : EventArgs
	{
		public ElementReference Element { get; set; }
		public string OriginalPropertyNameFilter { get; set; }

		public bool Composed { get; set; }
		public double ElapsedTime { get; set; }
		public int EventPhase { get; set; }
		//public string[] Path { get; set; } //Should be ElementReference[] but cannot serialize from JS
		public string PropertyName { get; set; }
		public bool ReturnValue { get; set; }
		//public string Target { get; set; } //Should be ElementReference but cannot serialize from JS
		public string Type { get; set; }
	}
}