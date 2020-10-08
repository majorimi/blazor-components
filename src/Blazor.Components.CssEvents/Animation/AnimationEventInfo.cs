using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor.Components.CssEvents.Animation
{
	public class AnimationEventInfo
	{
		private readonly Func<AnimationEventArgs, Task> _animationCallback;

		public ElementReference Element { get; init; }
		public string AnimationName { get; init; }

		public AnimationEventInfo(ElementReference element, Func<AnimationEventArgs, Task> animationEventCallback, string animationName)
		{
			Element = element;
			AnimationName = animationName;

			_animationCallback = animationEventCallback;
		}

		[JSInvokable("AnimationEvent")]
		public async Task AnimationEvent(AnimationEventArgs args)
		{
			args.Element = Element;
			args.OriginalAnimationNameFilter = AnimationName;

			await _animationCallback(args);
		}
	}
}