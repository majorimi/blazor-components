namespace Majorsoft.Blazor.Components.Common.JsInterop.Resize
{
	/// <summary>
	/// Custom type to represent Browser Window size.
	/// </summary>
	public class PageSize
	{
		/// <summary>
		/// Browser Window height
		/// </summary>
		public double Height { get; set; }
		/// <summary>
		/// Browser Window width
		/// </summary>
		public double Width { get; set; }

		/// <summary>
		/// PageSize string representation.
		/// </summary>
		/// <returns></returns>
		public override string ToString() => $"Width: {Width}px, Height: {Height}px";
	}
}