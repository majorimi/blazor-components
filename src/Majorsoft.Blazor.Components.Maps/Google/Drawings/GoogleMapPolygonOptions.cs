namespace Majorsoft.Blazor.Components.Maps.Google
{
	/// <summary>
	/// PolygonOptions object used to define the properties that can be set on a Polygon.
	/// https://developers.google.com/maps/documentation/javascript/reference/polygon#Polygon
	/// </summary>
	public class GoogleMapPolygonOptions : GoogleMapDrawingShapesBase
	{
		/// <summary>
		/// When true, edges of the polygon are interpreted as geodesic and will follow the curvature of the Earth. When false, edges of the polygon are rendered as 
		/// straight lines in screen space. Note that the shape of a geodesic polygon may appear to change when dragged, as the dimensions are maintained relative to
		/// the surface of the earth. Defaults to false.
		/// </summary>
		public bool Geodesic { get; set; }

		/// <summary>
		/// The ordered sequence of coordinates that designates a closed loop.
		/// </summary>
		public GoogleMapLatLng[] Paths { get; set; }

		public GoogleMapPolygonOptions() : base()
		{
		}
	}
}