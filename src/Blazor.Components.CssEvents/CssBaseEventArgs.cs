using System;

using Microsoft.AspNetCore.Components;

namespace Blazor.Components.CssEvents
{
	/// <summary>
	/// Common properties of CSS Animation and Transition events.
	/// </summary>
	public abstract class CssBaseEventArgs : EventArgs
	{
		/// <summary>
		/// Original element Ref
		/// </summary>
		public ElementReference Element { get; set; }

		public bool Composed { get; set; }
		public double ElapsedTime { get; set; }
		public int EventPhase { get; set; }
		//public string[] Path { get; set; } //Should be ElementReference[] but cannot serialize from JS
		public bool ReturnValue { get; set; }
		//public string Target { get; set; } //Should be ElementReference but cannot serialize from JS
		public string Type { get; set; }
	}
}