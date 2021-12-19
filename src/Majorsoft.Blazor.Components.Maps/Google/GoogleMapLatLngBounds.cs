namespace Majorsoft.Blazor.Components.Maps.Google
{
    /// <summary>
    /// A LatLngBounds instance represents a rectangle in geographical coordinates, including one that crosses the 180 degrees longitudinal meridian.
    /// </summary>
    public class GoogleMapLatLngBounds
    {
        /// <summary>
        /// Computes the center of this LatLngBounds
        /// Equivalent with `getCenter()` method call but it would require JsInterop.
        /// </summary>
        public GoogleMapLatLng Center { get; set; }

        /// <summary>
        /// Returns the north-east corner of this bounds.
        /// Equivalent with `getNorthEast()` method call but it would require JsInterop.
        /// </summary>
        public GoogleMapLatLng NorthEast { get; set; }

        /// <summary>
        /// Returns the south-west corner of this bounds.
        /// Equivalent with `getSouthWest()` method call but it would require JsInterop.
        /// </summary>
        public GoogleMapLatLng SouthWest { get; set; }

        /// <summary>
        /// Converts the given map bounds to a lat/lng span.
        /// Equivalent with `toSpan()` method call but it would require JsInterop.
        /// </summary>
        public GoogleMapLatLng Span { get; set; }

        /// <summary>
        /// Returns if the bounds are empty.
        /// Equivalent with `isEmpty()` method call but it would require JsInterop.
        /// </summary>
        public bool IsEmpty { get; set; }
    }
}