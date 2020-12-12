using System;

namespace Blazor.Components.Common.JsInterop.GlobalMouseEvents
{
	/// <summary>
	/// Custom defined event args for Resize events
	/// </summary>
	public class ResizeEventArgs : EventArgs
	{
		/// <summary>
		/// Element or Window height
		/// </summary>
		public int Height { get; set; }
		/// <summary>
		/// Element or Window width
		/// </summary>
		public int Width { get; set; }
		
		/// <summary>
		/// Registered event id
		/// </summary>
		public string EventId { get; set; }
	}
}