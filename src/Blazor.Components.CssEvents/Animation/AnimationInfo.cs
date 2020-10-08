using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor.Components.CssEvents.Animation
{
	public class AnimationInfo
	{
		private readonly Func<AnimationEventArgs, Task> _animationEndedCallback;

		public ElementReference Element { get; init; }
		public string AnimationName { get; init; }

		public AnimationInfo(ElementReference element, Func<AnimationEventArgs, Task> animationEndedCallback, string animationName)
		{
			Element = element;
			AnimationName = animationName;

			_animationEndedCallback = animationEndedCallback;
		}

		[JSInvokable("AnimationEnded")]
		public async Task AnimationEnded(AnimationEventArgs args)
		{
			args.Element = Element;
			args.OriginalAnimationNameFilter = AnimationName;

			await _animationEndedCallback(args);
		}
	}
}