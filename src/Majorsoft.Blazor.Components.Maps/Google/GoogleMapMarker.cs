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
		public IList<GeolocationData> Locations { get; }

		/// <summary>
		/// Checks if any marker style was defined in <see cref="Style"/> or <see cref="CustomIcon"/> properties.
		/// </summary>
		public bool HasStyleDefined => CustomIcon is not null || Style is not null;

		/// <summary>
		/// Default constructor
		/// </summary>
		public GoogleMapMarker()
		{
			Locations = new List<GeolocationData>();
		}

		public override string ToString()
		{
			var loc = Locations.Where(x => !string.IsNullOrWhiteSpace(x?.ToString()))
				.Select(s => s.ToString());

			if (!loc.Any())
			{
				return string.Empty;
			}

			var style = HasStyleDefined 
				? CustomIcon is not null ? CustomIcon?.ToString() : Style?.ToString()
				: null;

			if(!string.IsNullOrWhiteSpace(style))
			{
				style += "%7C";
			}

			return $"markers={style}{string.Join("%7C", loc)}";
		}
	}
}