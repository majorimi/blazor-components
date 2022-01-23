using System;
using System.Collections.Generic;

namespace Majorsoft.Blazor.Components.Maps.Google
{
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
		public GoogleMapMarkerShapeTypes ShapeType { get; set; }

		public string Type => ShapeType.ToString().ToLower();
	}
}