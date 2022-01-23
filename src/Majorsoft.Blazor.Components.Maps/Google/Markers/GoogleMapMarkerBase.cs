using System;

namespace Majorsoft.Blazor.Components.Maps.Google
{
	/// <summary>
	/// MarkerOptions object used to define the properties that can be set on a Marker.
	/// </summary>
	public abstract class GoogleMapMarkerBase
	{
		/// <summary>
		/// Id of the Custom control.
		/// </summary>
		public string Id { get; init; }

		/// <summary>
		/// The offset from the marker's position to the tip of an InfoWindow that has been opened with the marker as anchor.
		/// </summary>
		public Point? AnchorPoint { get; set; }

		/// <summary>
		/// Which animation to play when marker is added to a map.
		/// </summary>
		public GoogleMapMarkerAnimationTypes Animation { get; set; }

		/// <summary>
		/// If true, the marker receives mouse and touch events. Default value is true.
		/// </summary>
		public bool Clickable { get; set; } = true;

		/// <summary>
		/// If false, disables cross that appears beneath the marker when dragging. This option is true by default.
		/// </summary>
		public bool CrossOnDrag { get; set; }

		/// <summary>
		/// Mouse cursor to show on hover.
		/// </summary>
		public string? Cursor { get; set; }

		/// <summary>
		/// If true, the marker can be dragged. Default value is false.
		/// </summary>
		public bool Draggable { get; set; }

		/// <summary>
		/// Adds a label to the marker. The label can either be a string, or a MarkerLabel object.
		/// </summary>
		public GoogleMapMarkerLabel? Label { get; set; }

		/// <summary>
		/// Icon for the foreground. If a string is provided, it is treated as though it were an Icon with the string as url.
		/// </summary>
		public GoogleMapMarkerLabelIcon? Icon { get; set; }

		/// <summary>
		/// The marker's opacity between 0.0 and 1.0.
		/// </summary>
		public double Opacity { get; set; } = 1.0;

		/// <summary>
		/// Optimization renders many markers as a single static element. Optimized rendering is enabled by default. 
		/// Disable optimized rendering for animated GIFs or PNGs, or when each marker must be rendered as a separate DOM element (advanced usage only).
		/// </summary>
		public bool Optimized { get; set; } = true;

		/// <summary>
		/// Marker position. Required in order to display the marker.
		/// </summary>
		public GeolocationCoordinate Position { get; set; }

		/// <summary>
		/// Image map region definition used for drag/click.
		/// </summary>
		public GoogleMapMarkerShape? Shape { get; set; }

		/// <summary>
		/// Rollover text. If provided, an accessibility text (e.g. for use with screen readers) 
		/// will be added to the marker with the provided value.
		/// </summary>
		public string? Title { get; set; }

		/// <summary>
		/// If true, the marker is visible.
		/// </summary>
		public bool Visible { get; set; } = true;

		/// <summary>
		/// All markers are displayed on the map in order of their zIndex, with higher values displaying in front of markers with lower values.
		/// </summary>
		public int ZIndex { get; set; }

		/// <summary>
		/// Info Window to show for Marker on click.
		/// </summary>
		public GoogleMapInfoWindow? InfoWindow { get; set; }

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="position">Marker position on the Map</param>
		public GoogleMapMarkerBase(GeolocationCoordinate position)
		{
			Id = Guid.NewGuid().ToString();
			Position = position ?? throw new ArgumentNullException(nameof(position));
		}
	}
}