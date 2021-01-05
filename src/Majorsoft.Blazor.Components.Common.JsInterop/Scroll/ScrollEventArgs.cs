using System;

namespace Majorsoft.Blazor.Components.Common.JsInterop.Scroll
{
	public sealed class ScrollEventArgs : EventArgs
	{
		public double X { get; set; }
		public double Y { get; set; }
	}
}