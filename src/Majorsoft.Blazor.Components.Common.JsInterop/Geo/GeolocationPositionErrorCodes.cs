namespace Majorsoft.Blazor.Components.Common.JsInterop.Geo
{
	/// <summary>
	/// Represents the reason of an error occurring when using the geolocating device.
	/// </summary>
	public enum GeolocationPositionErrorCodes
	{
		/// <summary>
		/// The acquisition of the geolocation information failed because the page didn't have the permission to do it.
		/// </summary>
		PERMISSION_DENIED = 1,
		/// <summary>
		/// The acquisition of the geolocation failed because at least one internal source of position returned an internal error.
		/// </summary>
		POSITION_UNAVAILABLE = 2,
		/// <summary>
		/// The time allowed to acquire the geolocation, defined by PositionOptions.timeout information was reached before the information was obtained.
		/// </summary>
		TIMEOUT = 3
	}
}