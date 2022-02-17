using System.Text.Json.Serialization;

namespace Majorsoft.Blazor.Components.Maps.Google
{
    /// <summary>
    /// A LatLng is a point in geographical coordinates: latitude and longitude.
    /// </summary>
    public class GoogleMapLatLng
    {
        /// <summary>
        /// Latitude ranges between -90 and 90 degrees, inclusive. Values above or below this range will be clamped to the range [-90, 90]. 
        /// This means that if the value specified is less than -90, it will be set to -90. And if the value is greater than 90, it will be set to 90.
        /// </summary>
        [JsonPropertyName("lat")]
        public double Latitude { get; set; }

        /// <summary>
        /// Longitude ranges between -180 and 180 degrees, inclusive. Values above or below this range will be wrapped so that they fall within the range.
        /// For example, a value of -190 will be converted to 170. A value of 190 will be converted to -170. This reflects the fact that longitudes wrap around the globe.
        /// </summary>
        [JsonPropertyName("lng")]
        public double Longitude { get; set; }
    }
}