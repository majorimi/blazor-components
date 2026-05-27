namespace Majorsoft.Blazor.Components.Maps.Google.Markers
{
	/// <summary>
	/// Data class for Google Maps marker cluster click event information.
	/// </summary>
	public class GoogleMapClusterData
	{
		/// <summary>
		/// The number of markers in the cluster.
		/// </summary>
		public int Count { get; set; }

		/// <summary>
		/// The center position of the cluster.
		/// </summary>
		public GeolocationCoordinate Position { get; set; }
	}
}
