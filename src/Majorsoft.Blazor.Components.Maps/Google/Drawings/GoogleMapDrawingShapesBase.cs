namespace Majorsoft.Blazor.Components.Maps.Google
{
	/// <summary>
	/// Google Map drawing Shapes objects common Properties (for shapes with fill: Circle, Rectangle, Polygon)
	/// </summary>
	public abstract class GoogleMapDrawingShapesBase : GoogleMapDrawingBase
	{
		/// <summary>
		/// The fill color. All CSS3 colors are supported except for extended named colors.
		/// </summary>
		public string FillColor { get; set; }

		/// <summary>
		/// The fill opacity between 0.0 and 1.0.
		/// </summary>
		public double FillOpacity { get; set; } = 0.0;

		/// <summary>
		/// The stroke position. Defaults to CENTER.
		/// </summary>
		public string StrokePosition { get; set; } = "CENTER";
	}
}