namespace Majorsoft.Blazor.Components.Maps.Google
{
	/// <summary>
	/// Animations that can be played on a marker.
	/// </summary>
	public enum GoogleMapMarkerAnimationTypes
	{
		/// <summary>
		/// No animation.
		/// </summary>
		None = 0,

		/// <summary>
		/// Marker bounces until animation is stopped.
		/// </summary>
		BOUNCE = 1,

		/// <summary>
		/// Marker falls from the top of the map ending with a small bounce.
		/// </summary>
		DROP = 2,
	}
}