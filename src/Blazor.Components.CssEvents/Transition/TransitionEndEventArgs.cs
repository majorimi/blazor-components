namespace Blazor.Components.CssEvents.Transition
{
	public class TransitionEndEventArgs : CssBaseEventArgs
	{
		public string OriginalPropertyNameFilter { get; set; }

		public string PropertyName { get; set; }
	}
}