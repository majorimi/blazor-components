using System;

namespace Majorsoft.Blazor.Components.Common.JsInterop.Scroll
{
	/// <summary>
	/// Scroll event args
	/// </summary>
	public sealed class ScrollEventArgs : EventArgs
	{
		/// <summary>
		/// Scroll X position
		/// </summary>
		public double X { get; set; }
		/// <summary>
		/// Scroll Y position
		/// </summary>
		public double Y { get; set; }
	}
}