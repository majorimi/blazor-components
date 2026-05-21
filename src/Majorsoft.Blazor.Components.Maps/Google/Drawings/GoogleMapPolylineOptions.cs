using System;
using System.Threading.Tasks;

namespace Majorsoft.Blazor.Components.Maps.Google
{
	/// <summary>
	/// Google Map drawing objects common Properties
	/// </summary>
	public abstract class GoogleMapDrawingBase
	{
		/// <summary>
		/// Id of the drawing shape.
		/// </summary>
		public string Id { get; init; }

		/// <summary>
		/// Indicates whether this shape handles mouse events. Defaults to true.
		/// </summary>
		public bool Clickable { get; set; }

		/// <summary>
		/// If set to true, the user can drag this shape over the map. Defaults to false.
		/// </summary>
		public bool Draggable { get; set; }

		/// <summary>
		/// If set to true, the user can edit this shape by dragging the control points. Defaults to false.
		/// </summary>
		public bool Editable { get; set; }

		/// <summary>
		/// The stroke color. All CSS3 colors are supported except for extended named colors.
		/// </summary>
		public string StrokeColor { get; set; } = "black";

		/// <summary>
		/// The stroke opacity between 0.0 and 1.0.
		/// </summary>
		public double StrokeOpacity { get; set; } = 1.0;

		/// <summary>
		/// The stroke width in pixels.
		/// </summary>
		public double StrokeWeight { get; set; } = 2;

		/// <summary>
		/// Whether this shape is visible on the map. Defaults to true.
		/// </summary>
		public bool Visible { get; set; } = true;

		/// <summary>
		/// The zIndex compared to other shapes.
		/// </summary>
		public int ZIndex { get; set; }

		/// <summary>
		/// Callback function called when shape was clicked.
		/// </summary>
		public Func<string, Task>? OnClickCallback { get; set; }

		/// <summary>
		/// Callback function called when shape is being dragged.
		/// </summary>
		public Func<string, GeolocationCoordinate, Task>? OnDragCallback { get; set; }

		/// <summary>
		/// Callback function called when shape drag ended.
		/// </summary>
		public Func<string, GeolocationCoordinate, Task>? OnDragEndCallback { get; set; }

		/// <summary>
		/// Callback function called when shape drag started.
		/// </summary>
		public Func<string, GeolocationCoordinate, Task>? OnDragStartCallback { get; set; }

		/// <summary>
		/// Default constructor.
		/// </summary>
		protected GoogleMapDrawingBase()
		{
			Id = Guid.NewGuid().ToString();
		}
	}

	/// <summary>
	/// Google Map drawing Shapes objects common Properties
	/// </summary>
	public abstract class GoogleMapDrawingShapesBase : GoogleMapDrawingBase
	{
	}

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
		/// The fill color. All CSS3 colors are supported except for extended named colors.
		/// </summary>
		public string FillColor { get; set; }

		/// <summary>
		/// The fill opacity between 0.0 and 1.0.
		/// </summary>
		public double FillOpacity { get; set; } = 0.0;

		/// <summary>
		/// The radius in meters on the Earth's surface.
		/// </summary>
		public double Radius { get; set; }

		/// <summary>
		/// The stroke position. Defaults to CENTER.
		/// </summary>
		public string StrokePosition { get; set; } = "CENTER";

		public GoogleMapCircleOptions() : base()
		{
		}
	}

	/// <summary>
	/// RectangleOptions object used to define the properties that can be set on a Rectangle.
	/// https://developers.google.com/maps/documentation/javascript/reference/polygon#Rectangle
	/// </summary>
	public class GoogleMapRectangleOptions : GoogleMapDrawingShapesBase
	{
		/// <summary>
		/// The bounds of the rectangle.
		/// </summary>
		public GoogleMapLatLngBounds Bounds { get; set; }

		/// <summary>
		/// The fill color. All CSS3 colors are supported except for extended named colors.
		/// </summary>
		public string FillColor { get; set; }

		/// <summary>
		/// The fill opacity between 0.0 and 1.0.
		/// </summary>
		public double FillOpacity { get; set; } = 0.0;

		/// <summary>
		/// The stroke position. Defaults to CENTER.
		/// </summary>
		public string StrokePosition { get; set; } = "CENTER";

		public GoogleMapRectangleOptions() : base()
		{
		}
	}

	/// <summary>
	/// PolylineOptions object used to define the properties that can be set on a Polyline.
	/// https://developers.google.com/maps/documentation/javascript/reference/polygon#Polyline
	/// </summary>
	public class GoogleMapPolylineOptions : GoogleMapDrawingBase
	{
		/// <summary>
		/// When true, edges of the polyline are interpreted as geodesic and will follow the curvature of the Earth. When false, edges of the polyline are rendered as 
		/// straight lines in screen space. Defaults to false.
		/// </summary>
		public bool Geodesic { get; set; }

		/// <summary>
		/// The icons to be rendered along the polyline.
		/// </summary>
		public GoogleMapIconSequence[] Icons { get; set; }

		/// <summary>
		/// The ordered sequence of coordinates of the Polyline.
		/// </summary>
		public GoogleMapLatLng[] Path { get; set; }

		public GoogleMapPolylineOptions() : base()
		{
		}
	}

	/// <summary>
	/// PolygonOptions object used to define the properties that can be set on a Polygon.
	/// https://developers.google.com/maps/documentation/javascript/reference/polygon#Polygon
	/// </summary>
	public class GoogleMapPolygonOptions : GoogleMapDrawingShapesBase
	{
		/// <summary>
		/// The fill color. All CSS3 colors are supported except for extended named colors.
		/// </summary>
		public string FillColor { get; set; }

		/// <summary>
		/// The fill opacity between 0.0 and 1.0.
		/// </summary>
		public double FillOpacity { get; set; } = 0.0;

		/// <summary>
		/// When true, edges of the polygon are interpreted as geodesic and will follow the curvature of the Earth. When false, edges of the polygon are rendered as 
		/// straight lines in screen space. Note that the shape of a geodesic polygon may appear to change when dragged, as the dimensions are maintained relative to
		/// the surface of the earth. Defaults to false.
		/// </summary>
		public bool Geodesic { get; set; }

		/// <summary>
		/// The ordered sequence of coordinates that designates a closed loop.
		/// </summary>
		public GoogleMapLatLng[] Paths { get; set; }

		/// <summary>
		/// The stroke position. Defaults to CENTER.
		/// </summary>
		public string StrokePosition { get; set; } = "CENTER";

		public GoogleMapPolygonOptions() : base()
		{
		}
	}

	/// <summary>
	/// Describes how icons are to be rendered on a line.
	/// </summary>
	public class GoogleMapIconSequence
	{
		/// <summary>
		/// If true, each icon in the sequence has the same fixed rotation regardless of the angle of the edge on which it lies. Defaults to false, 
		/// in which case each icon in the sequence is rotated to align with its edge.
		/// </summary>
		public bool FixedRotation { get; set; }

		/// <summary>
		/// The icon to render on the line.
		/// </summary>
		public GoogleMapIconSequenceSymbol Icon { get; set; }

		/// <summary>
		/// The distance from the start of the line at which an icon is to be rendered. This distance may be expressed as a percentage of line's length 
		/// (e.g. '50%') or in pixels (e.g. '50px'). Defaults to '100%'.
		/// </summary>
		public string Offset { get; set; }

		/// <summary>
		/// The distance between consecutive icons on the line. This distance may be expressed as a percentage of the line's length (e.g. '50%') or in pixels 
		/// (e.g. '50px'). To disable repeating of the icon, specify '0'. Defaults to '0'.
		/// </summary>
		public string Repeat { get; set; }
	}

	/// <summary>
	/// Describes a symbol, which consists of a vector path with styling. A symbol can be used as the icon of a marker, or placed on a polyline.
	/// </summary>
	public class GoogleMapIconSequenceSymbol
	{
		/// <summary>
		/// The symbol's path, which is a built-in symbol path, or a custom path expressed using SVG path notation. Required.
		/// </summary>
		public string Path { get; set; }

		/// <summary>
		/// The position at which to anchor an image in correspondence to the location of the marker on the map.
		/// By default, the anchor is located along the center point of the bottom of the image.
		/// </summary>
		public Point? Anchor { get; set; }

		/// <summary>
		/// The symbol's fill color. All CSS3 colors are supported except for extended named colors. For symbol markers, this defaults to 'black'. 
		/// For symbols on polylines, this defaults to the stroke color of the corresponding polyline.
		/// </summary>
		public string FillColor { get; set; }

		/// <summary>
		/// The symbol's fill opacity. Defaults to 0.
		/// </summary>
		public double FillOpacity { get; set; }

		/// <summary>
		/// The origin of the label relative to the top-left corner of the icon image, if a label is supplied by the marker. 
		/// By default, the origin is located in the center point of the image.
		/// </summary>
		public Point? LabelOrigin { get; set; }

		/// <summary>
		/// The angle by which to rotate the symbol, expressed clockwise in degrees. Defaults to 0. A symbol in an IconSequence where fixedRotation is false 
		/// is rotated relative to the angle of the edge on which it lies.
		/// </summary>
		public double Rotation { get; set; }

		/// <summary>
		/// The amount by which the symbol is scaled in size. For symbol markers, this defaults to 1; after scaling, the symbol may be of any size. For symbols on a polyline, 
		/// this defaults to the stroke weight of the polyline; after scaling, the symbol must lie inside a square 22 pixels in size centered at the symbol's anchor.
		/// </summary>
		public double Scale { get; set; }

		/// <summary>
		/// The symbol's stroke color. All CSS3 colors are supported except for extended named colors. For symbol markers, this defaults to 'black'. 
		/// For symbols on a polyline, this defaults to the stroke color of the polyline.
		/// </summary>
		public string StrokeColor { get; set; }

		/// <summary>
		/// The symbol's stroke opacity. For symbol markers, this defaults to 1. For symbols on a polyline, this defaults to the stroke opacity of the polyline.
		/// </summary>
		public double StrokeOpacity { get; set; }

		/// <summary>
		/// The symbol's stroke weight. Defaults to the scale of the symbol.
		/// </summary>
		public double StrokeWeight { get; set; }

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="path">The symbol's path</param>
		public GoogleMapIconSequenceSymbol(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace", nameof(path));
			}

			Path = path;
		}
	}
}