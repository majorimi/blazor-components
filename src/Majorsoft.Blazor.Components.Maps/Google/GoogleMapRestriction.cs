namespace Majorsoft.Blazor.Components.Maps.Google
{
    /// <summary>
    /// Restrictions coordinates for <see cref="GoogleMap"/>
    /// </summary>
    public class GoogleMapRestriction
    {
        /// <summary>
        /// Latitude and Longitude SW/NE corners of the bound.
        /// </summary>
        public GoogleMapLatLngBounds LatLngBounds { get; set; }

        /// <summary>
        /// Is restriction strict.
        /// </summary>
        public bool StrictBounds { get; set; }
    }
}