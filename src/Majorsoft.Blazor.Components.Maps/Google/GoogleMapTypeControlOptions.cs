using System.Collections.Generic;
using System.Linq;

namespace Majorsoft.Blazor.Components.Maps.Google
{
	/// <summary>
	/// Options for the rendering of the map type control.
	/// </summary>
	public class GoogleMapTypeControlOptions
	{
		/// <summary>
		/// IDs of map types to show in the control.
		/// </summary>
		public IEnumerable<GoogleMapTypes> MapTypeIds { get; private set; }

		/// <summary>
		/// Position id. Used to specify the position of the control on the map. The default position is TOP_RIGHT.
		/// </summary>
		public GoogleMapControlPositions Position { get; set; } = GoogleMapControlPositions.TOP_LEFT;

		/// <summary>
		/// Style id. Used to select what style of map type control to display.
		/// </summary>
		public GoogleMapTypeControlStyles MapTypeControlStyle { get; set; } = GoogleMapTypeControlStyles.DEFAULT;

		/// <summary>
		/// Default constructor
		/// </summary>
		public GoogleMapTypeControlOptions()
		{
			MapTypeIds = new List<GoogleMapTypes>()
			{
				GoogleMapTypes.Hybrid,
				GoogleMapTypes.Roadmap,
				GoogleMapTypes.Satellite,
				GoogleMapTypes.Terrain,
			};
		}

		/// <summary>
		/// Clear <see cref="MapTypeIds"/> list.
		/// </summary>
		public void ClearTypes()
		{
			MapTypeIds = new List<GoogleMapTypes>();
		}

		/// <summary>
		/// Resets the list of Map types with the given params array.
		/// </summary>
		/// <param name="mapTypes">Required map types to use</param>
		public void ResetMapTypes(params GoogleMapTypes[] mapTypes)
		{
			MapTypeIds = mapTypes.Distinct().ToList();
		}
	}
}