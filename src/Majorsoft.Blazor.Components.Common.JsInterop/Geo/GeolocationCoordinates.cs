namespace Majorsoft.Blazor.Components.Common.JsInterop.Geo
{
	/// <summary>
	/// The GeolocationCoordinates interface represents the position and altitude of the device on Earth, as well as the accuracy with which these properties are calculated.
	/// https://developer.mozilla.org/en-US/docs/Web/API/GeolocationCoordinates
	/// </summary>
	public sealed class GeolocationCoordinates
	{
		/// <summary>
		/// Representing the latitude of the position in decimal degrees.
		/// </summary>
		public double Latitude { get; set; }
		/// <summary>
		/// Represents the longitude of a geographical position, specified in decimal degrees.
		/// </summary>
		public double Longitude { get; set; }
		/// <summary>
		/// representing the accuracy, with a 95% confidence level, of the 
		/// <see cref="Latitude"/> and <see cref="Longitude"/> properties expressed in meters.
		/// </summary>
		public double Accuracy { get; set; }
		/// <summary>
		/// Representing the altitude of the position in meters above the WGS84 ellipsoid (which defines the nominal sea level surface). 
		/// This value is null if the implementation cannot provide this data.
		/// </summary>
		public double? Altitude { get; set; }
		/// <summary>
		/// Representing the accuracy, with a 95% confidence level, of the <see cref="Altitude"/> expressed in meters. 
		/// This value is null if the implementation doesn't support measuring altitude.
		/// </summary>
		public double? AltitudeAccuracy { get; set; }
		/// <summary>
		/// Representing the velocity of the device in meters per second. This value can be null.
		/// </summary>
		public double? Speed { get; set; }
		/// <summary>
		/// representing the direction in which the device is traveling. This value, specified in degrees, indicates how far off from heading due north the device is. Zero degrees represents true north, and the direction is determined clockwise (which means that east is 90 degrees and west is 270 degrees). 
		/// If GeolocationCoordinates.speed is 0, heading is NaN. If the device is not able to provide heading information, this value is null.
		/// </summary>
		public double? Heading { get; set; }
	}
}