using System.Globalization;

namespace Majorsoft.Blazor.Components.Maps
{
	/// <summary>
	/// Represents a Geo Coordinate or Address to show on Maps.
	/// </summary>
	public class GeolocationData
	{
		/// <summary>
		/// Representing the latitude of the position in decimal degrees.
		/// </summary>
		public double? Latitude { get; set; }
		/// <summary>
		/// Represents the longitude of a geographical position, specified in decimal degrees.
		/// </summary>
		public double? Longitude { get; set; }

		/// <summary>
		/// Checks to <see cref="Latitude"/> and <see cref="Longitude"/> are defined.
		/// </summary>
		public bool HasCoordinates => Latitude.HasValue && Longitude.HasValue;

		/// <summary>
		/// Represents a secondary location as a string address. 
		/// It will be omitted if <see cref="Latitude"/> and <see cref="Longitude"/> coordinates are defined.
		/// </summary>
		public string Address { get; set; }

		/// <summary>
		/// Formats coordinates to Maps specific string or return address.
		/// </summary>
		/// <returns>Maps specific geo coordinates or address</returns>
		public override string ToString()
		{
			return HasCoordinates
				? $"{Latitude.Value.ToString(CultureInfo.InvariantCulture)},{Longitude.Value.ToString(CultureInfo.InvariantCulture)}"
				: Address;
		}
	}
}