namespace Majorsoft.Blazor.Components.Maps.Google
{
	/// <summary>
	/// PolylineOptions object used to define the properties that can be set on a Polyline.
	/// https://developers.google.com/maps/documentation/javascript/reference/polygon#Polyline
	/// </summary>
	public class GoogleMapPolylineOptions : GoogleMapDrawingBase
	{
		/// <summary>
		/// When true, edges of the polyline are interpreted as geodesic and will follow the curvature of the Earth. When false, edges of the polyline are rendered as 
		/// straight lines in screen space. Defaults to false.
		/// </summary>
		public bool Geodesic { get; set; }

		/// <summary>
		/// The icons to be rendered along the polyline.
		/// </summary>
		public GoogleMapIconSequence[] Icons { get; set; }

		/// <summary>
		/// The ordered sequence of coordinates of the Polyline.
		/// </summary>
		public GoogleMapLatLng[] Path { get; set; }

		public GoogleMapPolylineOptions() : base()
		{
		}
	}
}