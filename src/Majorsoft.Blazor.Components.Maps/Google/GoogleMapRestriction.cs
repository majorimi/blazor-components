namespace Majorsoft.Blazor.Components.Maps.Google
{
    /// <summary>
    /// Restrictions coordinates for <see cref="GoogleMap"/>
    /// NOTE: Google Maps restriction is basically a MAX Zoom level. So it does not allow users to zoom out (zoom level value forced). 
    /// In order to notify Blazor about the maximum Zoom level two-way binding MUST be used: `@bind-Zoom="_jsMapZoomLevel" @bind-Zoom:event="OnMapZoomLevelChanged"`
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