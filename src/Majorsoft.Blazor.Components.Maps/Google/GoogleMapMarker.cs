using System.Collections.Generic;
using System.Linq;

namespace Majorsoft.Blazor.Components.Maps.Google
{
	/// <summary>
	/// The markers parameter defines a set of one or more markers (map pins) at a set of locations.
	/// </summary>
	public sealed class GoogleMapMarker
	{
		/// <summary>
		/// Set of marker style descriptors.
		/// </summary>
		public GoogleMapMarkerStyle? Style { get; set; }

		/// <summary>
		/// Override default markers and <see cref="Style"/> with your own custom icons instead.
		/// </summary>
		public GoogleMapMarkerCustomIcon? CustomIcon { get; set; }

		/// <summary>
		/// Each marker descriptor must contain a set of one or more locations defining where to place the marker on the map.
		/// </summary>
		public IList<GeolocationCoordinate> Locations { get; }

		/// <summary>
		/// 
		/// </summary>
		public bool HasCustomIcon => CustomIcon is not null;

		/// <summary>
		/// Default constructor
		/// </summary>
		public GoogleMapMarker()
		{
			Locations = new List<GeolocationCoordinate>();
		}

		public override string ToString()
		{
			var loc = Locations.Where(x => !string.IsNullOrWhiteSpace(x.ToString()))
				.Select(s => s.ToString());

			if (!loc.Any() || (Style is null && CustomIcon is null))
			{
				return string.Empty;
			}

			var style = HasCustomIcon ? CustomIcon?.ToString() : Style?.ToString();
			return $"markers={style}%7C{string.Join("%7C", loc)}";
		}
	}

	/// <summary>
	/// Rather than use Google's marker icons, you are free to use your own custom icons instead.
	/// </summary>
	public sealed class GoogleMapMarkerCustomIcon
	{
		/// <summary>
		/// Custom icon URL.
		/// </summary>
		public string IconUrl { get; set; }

		/// <summary>
		/// Icon is position in relation to the specified markers locations.
		/// </summary>
		public GoogleMapMarkerCustomIconAnchors Anchor { get; set; }

		public override string ToString()
		{
			return $"anchor:{Anchor.ToString().ToLower()}%7Cicon:{IconUrl}";
		}
	}

	/// <summary>
	/// The anchor point sets how the icon is placed in relation to the specified markers locations.
	/// </summary>
	public enum GoogleMapMarkerCustomIconAnchors
	{
		Top, 
		Bottom, 
		Left, 
		Right, 
		Center, 
		Topleft, 
		Topright, 
		Bottomleft,
		Bottomright
	}

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
				{ $"scale:{Scale}" },
				{ Size != GoogleMapMarkerSizes.Normal ? $"size:{Size.ToString().ToLower()}" : null },
				{ string.IsNullOrWhiteSpace(Color) ? $"color:{Color?.ToLower()}" : null },
				{ Label != default(char) ? $"label:{Label.ToString().ToUpper()}" : null }
			};

			return string.Join("%7C", parts);
		}
	}

	/// <summary>
	/// Specifies the size of marker
	/// </summary>
	public enum GoogleMapMarkerSizes
	{
		Normal = 0,
		Small = 1,
		Mid = 2,
		Tiny = 3,
	}
}