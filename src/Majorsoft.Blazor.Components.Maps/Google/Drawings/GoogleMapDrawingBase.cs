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
}