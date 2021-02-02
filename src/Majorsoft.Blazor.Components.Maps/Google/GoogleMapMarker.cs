using System;
using System.Collections.Generic;

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
		public bool Optimized { get; set; }

		/// <summary>
		/// Marker position. Required in order to display the marker.
		/// </summary>
		public GeolocationData Position { get; set; }

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
		/// Default constructor.
		/// </summary>
		public GoogleMapMarkerBase()
		{
			Id = Guid.NewGuid().ToString();
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public class GoogleMapMarker : GoogleMapMarkerBase
	{
	}

	/// <summary>
	/// This object defines the clickable region of a marker image.
	/// </summary>
	public class GoogleMapMarkerShape
	{
		/// <summary>
		/// The format of this attribute depends on the value of the type and follows the w3 AREA coords specification found 
		/// at http://www.w3.org/TR/REC-html40/struct/objects.html#adef-coords.
		/// </summary>
		public IEnumerable<int> Coords { get; set; }

		/// <summary>
		/// Describes the shape's type and can be circle, poly or rect.
		/// </summary>
		public GoogleMapMarkerShapeTypes MyProperty { get; set; }
	}

	/// <summary>
	/// Describes the shape's type and can be circle, poly or rect.
	/// </summary>
	public enum GoogleMapMarkerShapeTypes
	{
		Circle,
		Poly,
		Rect
	}

	/// <summary>
	/// These options specify the appearance of a marker label. 
	/// A marker label is a string (often a single character) which will appear inside the marker.
	/// </summary>
	public class GoogleMapMarkerLabel
	{
		/// <summary>
		/// The text to be displayed in the label.
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// The className property of the label's element (equivalent to the element's class attribute). 
		/// Multiple space-separated CSS classes can be added.
		/// </summary>
		public string? ClassName { get; set; }

		/// <summary>
		/// The color of the label text. Default color is black.
		/// </summary>
		public string? Color { get; set; }

		/// <summary>
		/// The font family of the label text (equivalent to the CSS font-family property).
		/// </summary>
		public string? FontFamily { get; set; }

		/// <summary>
		/// The font size of the label text (equivalent to the CSS font-size property). Default size is 14px.
		/// </summary>
		public string? FontSize { get; set; }

		/// <summary>
		/// The font weight of the label text (equivalent to the CSS font-weight property).
		/// </summary>
		public string? FontWeight { get; set; }

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="text">Label text</param>
		public GoogleMapMarkerLabel(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				throw new ArgumentException($"'{nameof(text)}' cannot be null or empty", nameof(text));
			}

			Text = text;
		}
	}

	/// <summary>
	/// A structure representing a Marker icon image.
	/// </summary>
	public class GoogleMapMarkerLabelIcon
	{
		/// <summary>
		/// The URL of the image or sprite sheet.
		/// </summary>
		public string Url { get; set; }

		/// <summary>
		/// The position at which to anchor an image in correspondence to the location of the marker on the map.
		/// By default, the anchor is located along the center point of the bottom of the image.
		/// </summary>
		public Point? Anchor { get; set; }

		/// <summary>
		/// The origin of the label relative to the top-left corner of the icon image, if a label is supplied by the marker. 
		/// By default, the origin is located in the center point of the image.
		/// </summary>
		public Point? LabelOrigin { get; set; }

		/// <summary>
		/// The position of the image within a sprite, if any. 
		/// By default, the origin is located at the top left corner of the image (0, 0).
		/// </summary>
		public Point? Origin { get; set; }

		/// <summary>
		/// The size of the entire image after scaling, if any. Use this property to stretch/shrink an image or a sprite.
		/// </summary>
		public Rect? ScaledSize { get; set; }

		/// <summary>
		/// The display size of the sprite or image. When using sprites, you must specify the sprite size. 
		/// If the size is not provided, it will be set when the image loads.
		/// </summary>
		public Rect? Size { get; set; }

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="url">Icon Url</param>
		public GoogleMapMarkerLabelIcon(string url)
		{
			if (string.IsNullOrWhiteSpace(url))
			{
				throw new ArgumentException($"'{nameof(url)}' cannot be null or whitespace", nameof(url));
			}

			Url = url;
		}
	}

	/// <summary>
	/// Animations that can be played on a marker.
	/// </summary>
	public enum GoogleMapMarkerAnimationTypes
	{
		/// <summary>
		/// No animation.
		/// </summary>
		None = 0,

		/// <summary>
		/// Marker bounces until animation is stopped.
		/// </summary>
		BOUNCE = 1,

		/// <summary>
		/// Marker falls from the top of the map ending with a small bounce.
		/// </summary>
		DROP = 2,
	}
}