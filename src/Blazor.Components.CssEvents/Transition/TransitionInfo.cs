using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor.Components.CssEvents.Transition
{
	public class TransitionInfo
	{
		private readonly Func<TransitionEndEventArgs, Task> _transitionEndedCallback;

		public ElementReference Element { get; init; }
		public string TransitionPropertyName { get; init; }

		public TransitionInfo(ElementReference element, Func<TransitionEndEventArgs, Task> transitionEndedCallback, string transitionPropertyName)
		{
			Element = element;
			TransitionPropertyName = transitionPropertyName;

			_transitionEndedCallback = transitionEndedCallback;
		}

		[JSInvokable("TransitionEnded")]
		public async Task TransitionEnded(TransitionEndEventArgs args)
		{
			args.Element = Element;
			args.OriginalPropertyNameFilter = TransitionPropertyName;

			await _transitionEndedCallback(args);
		}
	}
}