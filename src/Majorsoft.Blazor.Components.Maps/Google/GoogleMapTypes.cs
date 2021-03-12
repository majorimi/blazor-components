namespace Majorsoft.Blazor.Components.Maps.Google
{
	/// <summary>
	/// The Maps Static API creates maps in several formats
	/// </summary>
	public enum GoogleMapTypes
	{
		/// <summary>
		/// (default) specifies a standard roadmap image, as is normally shown on the Google Maps website.
		/// </summary>
		Roadmap = 0,
		/// <summary>
		/// Specifies satellite image.
		/// </summary>
		Satellite = 1,
		/// <summary>
		/// Specifies a physical relief map image, showing terrain and vegetation.
		/// </summary>
		Terrain = 2,
		/// <summary>
		/// Specifies a hybrid of the satellite and roadmap image, showing a transparent layer of major streets and place names on the satellite image.
		/// </summary>
		Hybrid = 3
	}
}