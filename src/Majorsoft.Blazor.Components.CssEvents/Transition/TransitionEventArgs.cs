namespace Majorsoft.Blazor.Components.CssEvents.Transition
{
	public sealed class TransitionEventArgs : CssBaseEventArgs
	{
		public string OriginalPropertyNameFilter { get; set; }

		public string PropertyName { get; set; }
	}
}