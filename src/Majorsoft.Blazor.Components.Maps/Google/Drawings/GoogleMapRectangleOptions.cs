namespace Majorsoft.Blazor.Components.Maps.Google
{
	/// <summary>
	/// RectangleOptions object used to define the properties that can be set on a Rectangle.
	/// https://developers.google.com/maps/documentation/javascript/reference/polygon#Rectangle
	/// </summary>
	public class GoogleMapRectangleOptions : GoogleMapDrawingShapesBase
	{
		/// <summary>
		/// The bounds of the rectangle.
		/// </summary>
		public GoogleMapLatLngBounds Bounds { get; set; }

		public GoogleMapRectangleOptions() : base()
		{
		}
	}
}