namespace Majorsoft.Blazor.Components.Maps.Google
{
	/// <summary>
	/// Identifiers used to specify the placement of controls on the map. 
	/// Controls are positioned relative to other controls in the same layout position. 
	/// Controls that are added first are positioned closer to the edge of the map.
	/// </summary>
	public enum GoogleMapControlPositions
	{
		/// <summary>
		/// Elements are positioned in the top left and flow towards the middle.
		/// </summary>
		TOP_LEFT = 1,
		/// <summary>
		/// Elements are positioned in the center of the top row.
		/// </summary>
		TOP_CENTER = 2,
		/// <summary>
		/// Elements are positioned in the top right and flow towards the middle.
		/// </summary>
		TOP_RIGHT = 3,
		/// <summary>
		/// Elements are positioned in the center of the left side.
		/// </summary>
		LEFT_CENTER = 4,
		/// <summary>
		///  Elements are positioned on the left, below top-left elements, and flow downwards.
		/// </summary>
		LEFT_TOP = 5,
		/// <summary>
		/// Elements are positioned on the left, above bottom-left elements, and flow upwards.
		/// </summary>
		LEFT_BOTTOM = 6,
		/// <summary>
		///  Elements are positioned on the right, below top-right elements, and flow downwards.
		/// </summary>
		RIGHT_TOP = 7,
		/// <summary>
		/// Elements are positioned in the center of the right side.
		/// </summary>
		RIGHT_CENTER = 8,
		/// <summary>
		/// Elements are positioned on the right, above bottom-right elements, and flow upwards.
		/// </summary>
		RIGHT_BOTTOM = 9,
		/// <summary>
		///  Elements are positioned in the bottom left and flow towards the middle. Elements are positioned to the right of the Google logo.
		/// </summary>
		BOTTOM_LEFT = 10,
		/// <summary>
		/// Elements are positioned in the center of the bottom row.
		/// </summary>
		BOTTOM_CENTER = 11,
		/// <summary>
		/// Elements are positioned in the bottom right and flow towards the middle. Elements are positioned to the left of the copyrights.
		/// </summary>
		BOTTOM_RIGHT = 12,
	}
}