using System;

namespace Majorsoft.Blazor.Components.Maps.Google
{
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