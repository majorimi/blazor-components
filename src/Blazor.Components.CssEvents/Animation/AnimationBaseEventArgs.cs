namespace Blazor.Components.CssEvents.Animation
{
	public abstract class AnimationBaseEventArgs : CssBaseEventArgs
	{
		public string OriginalAnimationNameFilter { get; set; }

		public string AnimationName { get; set; }
	}

	public class AnimationStartEventArgs : AnimationBaseEventArgs
	{
	}

	public class AnimationIterationEventArgs : AnimationBaseEventArgs
	{
	}

	public class AnimationEndEventArgs : AnimationBaseEventArgs
	{
	}
}