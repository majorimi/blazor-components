namespace Blazor.Components.CssEvents.Transition
{
	public class TransitionEventArgs : CssBaseEventArgs
	{
		public string OriginalPropertyNameFilter { get; set; }

		public string PropertyName { get; set; }
	}
}