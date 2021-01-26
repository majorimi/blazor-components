namespace Majorsoft.Blazor.Components.Maps
{
	/// <summary>
	/// Represents a Geo Coordinate or Address to show on Maps.
	/// </summary>
	public sealed class GeolocationData : GeolocationCoordinate
	{
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
				? base.ToString()
				: Address;
		}
	}
}