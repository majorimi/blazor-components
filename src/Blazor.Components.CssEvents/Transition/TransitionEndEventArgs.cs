using System;

using Microsoft.AspNetCore.Components;

namespace Blazor.Components.CssEvents.Transition
{
	public class TransitionEndEventArgs : EventArgs
	{
		public ElementReference Element { get; set; }
		public string OriginalPropertyNameFilter { get; set; }

		public bool Composed { get; set; }
		public double ElapsedTime { get; set; }
		public int EventPhase { get; set; }
		//public string[] Path { get; set; } //Should be ElementReference[] but cannot serialize from JS
		public string PropertyName { get; set; }
		public bool ReturnValue { get; set; }
		//public string Target { get; set; } //Should be ElementReference but cannot serialize from JS
		public string Type { get; set; }
	}
}