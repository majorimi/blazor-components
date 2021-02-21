namespace Majorsoft.Blazor.Components.Maps.Google
{
	/// <summary>
	/// This setting controls how the API handles gestures on the map.
	/// </summary>
	public enum GoogleMapGestureHandlingTypes
	{
		/// <summary>
		/// (default) Gesture handling is either cooperative or greedy, depending on whether the page is scrollable or in an iframe.
		/// </summary>
		Auto = 0,
		/// <summary>
		/// Scroll events and one-finger touch gestures scroll the page, and do not zoom or pan the map. Two-finger touch gestures pan and zoom the map. 
		/// Scroll events with a ctrl key or ⌘ key pressed zoom the map.
		/// </summary>
		Cooperative = 1,
		/// <summary>
		/// All touch gestures and scroll events pan or zoom the map.
		/// </summary>
		Greedy = 2,
		/// <summary>
		/// The map cannot be panned or zoomed by user gestures.
		/// </summary>
		None = 3,
	}
}