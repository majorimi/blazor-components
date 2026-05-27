namespace Majorsoft.Blazor.Components.Maps.Google
{
	/// <summary>
	/// CircleOptions object used to define the properties that can be set on a Circle.
	/// https://developers.google.com/maps/documentation/javascript/reference/polygon#Circle
	/// </summary>
	public class GoogleMapCircleOptions : GoogleMapDrawingShapesBase
	{
		/// <summary>
		/// The center of the circle.
		/// </summary>
		public GeolocationCoordinate Center { get; set; }

		/// <summary>
		/// The radius in meters on the Earth's surface.
		/// </summary>
		public double Radius { get; set; }

		public GoogleMapCircleOptions() : base()
		{
		}
	}
}