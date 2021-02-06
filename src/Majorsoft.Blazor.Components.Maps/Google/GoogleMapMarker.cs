using System;
using System.Threading.Tasks;

namespace Majorsoft.Blazor.Components.Maps.Google
{
	/// <summary>
	/// MarkerOptions object used to define the properties that can be set on a Marker with event callbacks.
	/// </summary>
	public class GoogleMapMarker : GoogleMapMarkerBase
	{
		/// <summary>
		/// Callback function called when Marker was clicked.
		/// </summary>
		public Func<string, Task>? OnClickCallback { get; set; }

		/// <summary>
		/// Callback function called when Marker dragging.
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
		/// Default constructor.
		/// </summary>
		/// <param name="position">Marker position on the Map</param>
		public GoogleMapMarker(GeolocationCoordinate position)
			: base(position)
		{
		}
	}
}