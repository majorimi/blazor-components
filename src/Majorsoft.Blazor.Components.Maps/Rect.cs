namespace Majorsoft.Blazor.Components.Maps
{
	/// <summary>
	/// Custom type to represent Rectangle size.
	/// </summary>
	public class Rect
	{
		/// <summary>
		/// Rect height
		/// </summary>
		public double Height { get; set; }
		/// <summary>
		/// Rect width
		/// </summary>
		public double Width { get; set; }

		/// <summary>
		/// Rect string representation.
		/// </summary>
		/// <returns></returns>
		public override string ToString() => $"Width: {Width}px, Height: {Height}px";
	}
}