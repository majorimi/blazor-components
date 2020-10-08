namespace Blazor.Components.CssEvents.Animation
{
	public abstract class AnimationEventArgs : CssBaseEventArgs
	{
		public string OriginalAnimationNameFilter { get; set; }

		public string AnimationName { get; set; }
	}
}