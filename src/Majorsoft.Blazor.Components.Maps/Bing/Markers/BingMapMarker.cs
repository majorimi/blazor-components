using System;
using System.Threading.Tasks;

namespace Majorsoft.Blazor.Components.Maps.Bing.Markers
{
	/// <summary>
	/// Marker options for Bing map with per-marker event callbacks.
	/// Callbacks are not serialized and will be invoked on the .NET side when JS triggers events.
	/// </summary>
	public class BingMapMarker : Majorsoft.Blazor.Components.Maps.Bing.BingMapMarkerBase
	{
		/// <summary>
		/// Callback function called when Marker was clicked.
		/// </summary>
		public Func<string, Task>? OnClickCallback { get; set; }

		/// <summary>
		/// Callback function called when Marker is dragged.
		/// </summary>
		public Func<string, GeolocationCoordinate, Task>? OnDragCallback { get; set; }

		/// <summary>
		/// Callback function called when Marker drag ended.
		/// </summary>
		public Func<string, GeolocationCoordinate, Task>? OnDragEndCallback { get; set; }

		/// <summary>
		/// Callback function called when Marker drag started.
		/// </summary>
		public Func<string, GeolocationCoordinate, Task>? OnDragStartCallback { get; set; }

		/// <summary>
		/// Creates a new BingMapMarker with the required position.
		/// </summary>
		/// <param name="position">Marker position on the map.</param>
		public BingMapMarker(GeolocationCoordinate position) : base(position)
		{
		}
	}
}
