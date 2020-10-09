using System;

using Microsoft.AspNetCore.Components;

namespace Blazor.Components.CssEvents
{
	public abstract class CssBaseEventArgs : EventArgs
	{
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