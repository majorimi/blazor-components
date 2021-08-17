using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Components.CssEvents.Transition
{
	/// <summary>
	/// Transition event <see cref="DotNetObjectReference"/> info to handle JS callback
	/// </summary>
	internal sealed class TransitionEventInfo
	{
		private readonly Func<TransitionEventArgs, Task> _transitionEventCallback;

		public ElementReference Element { get; init; }
		public string TransitionPropertyName { get; init; }

		public TransitionEventInfo(ElementReference element, Func<TransitionEventArgs, Task> transitionEventCallback, string transitionPropertyName)
		{
			Element = element;
			TransitionPropertyName = transitionPropertyName;

			_transitionEventCallback = transitionEventCallback;
		}

		[JSInvokable("TransitionEvent")]
		public async Task TransitionEvent(TransitionEventArgs args)
		{
			if (_transitionEventCallback is not null)
			{
				args.Element = Element;
				args.OriginalPropertyNameFilter = TransitionPropertyName;

				await _transitionEventCallback(args);
			}
		}
	}
}