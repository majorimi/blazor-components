using System;

namespace Majorsoft.Blazor.Components.Common.JsInterop.Geo
{
	/// <summary>
	/// Wrapper object for return value of Geolocation service.
	/// </summary>
	public sealed class GeolocationResult
	{
		/// <summary>
		/// Represents the position, if any.
		/// </summary>
		public GeolocationCoordinates? Coordinates { get; set; }
		/// <summary>
		/// Represents the reason of an error occurring, if any.
		/// </summary>
		public GeolocationError? Error { get; set; }

		/// <summary>
		/// Returns true if there is no error object, otherwise false.
		/// </summary>
		public bool IsSuccess => Error is null && Coordinates is not null;

		/// <summary>
		/// Event time
		/// </summary>
		public DateTime TimeStamp { get; set; }
	}
}