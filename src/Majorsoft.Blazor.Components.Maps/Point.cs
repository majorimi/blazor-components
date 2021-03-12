namespace Majorsoft.Blazor.Components.Maps
{
	/// <summary>
	/// Custom type to represent Point.
	/// </summary>
	public class Point
	{
		/// <summary>
		/// Point x
		/// </summary>
		public double X { get; set; }
		/// <summary>
		/// Point y
		/// </summary>
		public double Y { get; set; }

		/// <summary>
		/// Point string representation.
		/// </summary>
		/// <returns></returns>
		public override string ToString() => $"({X}, {Y})";
	}
}