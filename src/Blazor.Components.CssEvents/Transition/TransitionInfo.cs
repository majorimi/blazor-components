using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor.Components.CssEvents.Transition
{
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
}