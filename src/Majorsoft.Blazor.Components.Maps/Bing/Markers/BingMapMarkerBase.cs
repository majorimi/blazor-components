using System;

namespace Majorsoft.Blazor.Components.Maps.Bing
{
	/// <summary>
	/// Base class for Bing map marker options.
	/// Mirrors properties available on GoogleMapMarkerBase for API parity.
	/// </summary>
	public abstract class BingMapMarkerBase
	{
		/// <summary>
		/// Unique identifier of the marker.
		/// </summary>
		public string Id { get; init; }

		/// <summary>
		/// Marker position. Required to display the marker on the map.
		/// </summary>
		public GeolocationCoordinate Position { get; set; }

		/// <summary>
		/// Rollover text. Accessibility text for the marker.
		/// </summary>
		public string? Title { get; set; }

		/// <summary>
		/// If true, the marker receives mouse and touch events. Default is true.
		/// </summary>
		public bool Clickable { get; set; } = true;

		/// <summary>
		/// If true, the marker can be dragged.
		/// </summary>
		public bool Draggable { get; set; }

		/// <summary>
		/// Optional label text displayed on the marker.
		/// </summary>
		public string? Label { get; set; }

		/// <summary>
		/// Creates a new instance of <see cref="BingMapMarkerBase"/> with the required position.
		/// </summary>
		/// <param name="position">Marker position on the map.</param>
		public BingMapMarkerBase(GeolocationCoordinate position)
		{
			Id = Guid.NewGuid().ToString();
			Position = position ?? throw new ArgumentNullException(nameof(position));
		}
	}
}
