namespace Blazor.Components.CssEvents.Animation
{
	public sealed class AnimationEventArgs : CssBaseEventArgs
	{
		public string OriginalAnimationNameFilter { get; set; }

		public string AnimationName { get; set; }
	}
}