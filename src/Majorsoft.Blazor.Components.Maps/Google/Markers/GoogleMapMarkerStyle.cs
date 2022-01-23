using System.Collections.Generic;
using System.Linq;

namespace Majorsoft.Blazor.Components.Maps.Google
{
	/// <summary>
	/// The set of marker style descriptors is a series of value assignments separated by the pipe (|) character. 
	/// </summary>
	public sealed class GoogleMapMarkerStyle
	{
		/// <summary>
		/// Specifies the size of marker from the set {tiny, mid, small}. 
		/// If no size parameter is set, the marker will appear in its default (normal) size.
		/// </summary>
		public GoogleMapMarkerSizes Size { get; set; } = GoogleMapMarkerSizes.Normal;

		/// <summary>
		/// Specifies a 24-bit color (example: color=0xFFFFCC) or a predefined color from the set 
		/// {black, brown, green, purple, yellow, blue, gray, orange, red, white}.
		/// </summary>
		public string Color { get; set; }

		/// <summary>
		/// Specifies a single uppercase alphanumeric character from the set {A-Z, 0-9}.
		/// </summary>
		public char Label { get; set; }

		/// <summary>
		/// Scale value is multiplied with the marker image size to produce the actual output size of the marker in pixels. 
		/// Default scale value is 1; accepted values are 1, 2, and 4.
		/// </summary>
		public byte Scale { get; set; } = 1;

		public override string ToString()
		{
			var parts = new List<string>()
			{
				{ Scale != 1 ? $"scale:{Scale}" : null },
				{ Size != GoogleMapMarkerSizes.Normal ? $"size:{Size.ToString().ToLower()}" : null },
				{ !string.IsNullOrWhiteSpace(Color) ? $"color:{Color?.ToLower()}" : null },
				{ Label != default(char) ? $"label:{Label.ToString().ToUpper()}" : null }
			};

			return string.Join("|", parts.Where(x => !string.IsNullOrWhiteSpace(x)));
		}
	}
}