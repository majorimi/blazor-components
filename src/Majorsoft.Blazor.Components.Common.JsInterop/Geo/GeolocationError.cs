namespace Majorsoft.Blazor.Components.Common.JsInterop.Geo
{
	/// <summary>
	/// The GeolocationPositionError interface represents the reason of an error occurring when using the geolocating device.
	/// https://developer.mozilla.org/en-US/docs/Web/API/GeolocationPositionError
	/// </summary>
	public sealed class GeolocationError
	{
		/// <summary>
		/// Represents the reason of an error occurring.
		/// </summary>
		public GeolocationPositionErrorCodes ErrorCode { get; set; }
		/// <summary>
		/// Returns a human-readable DOMString describing the details of the error.
		/// </summary>
		public string ErrorMessage { get; set; }
	}
}