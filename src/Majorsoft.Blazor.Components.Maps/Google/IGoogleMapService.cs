using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Components.Maps.Google
{
	/// <summary>
	/// Injectable service to handle Google JavaScript Maps functionalities. Available on the instance of <see cref="GoogleMap"/> object ref as well.
	/// </summary>
	public interface IGoogleMapService : IAsyncDisposable
	{
		/// <summary>
		/// HTML Div Id which was set when Maps initialized with <see cref="InitMapAsync"/> method.
		/// </summary>
		string MapContainerId { get; }

		/// <summary>
		/// This function must be called only once to initialize Google JavaScript Maps with ApiKey and event callbacks.
		/// </summary>
		/// <param name="apiKey">Google API Key which has permission for Google JavaScript Maps</param>
		/// <param name="mapContainerId">HTML Div Id which will contain Google Map</param>
		/// <param name="backgroundColor">Color used for the background of the Map div. This color will be visible when tiles have not yet loaded as the user pans. This option can only be set when the map is initialized.</param>
		/// <param name="controlSize">Size in pixels of the controls appearing on the map. This value must be supplied directly when creating the Map.</param>
		/// <param name="mapInitializedCallback">Callback function for Map initialized event</param>
		/// <param name="mapClickedCallback">Callback function for Map clicked event</param>
		/// <param name="mapDoubleClickedCallback">Callback function for Map double clicked event</param>
		/// <param name="mapContextMenuCallback">Callback function for Map content menu event</param>
		/// <param name="mapMouseUpCallback">Callback function for Map mouse up event</param>
		/// <param name="mapMouseDownCallback">Callback function for Map mouse down event</param>
		/// <param name="mapMouseMoveCallback">Callback function for Map mouse move event</param>
		/// <param name="mapMouseOverCallback">Callback function for Map mouse enter event</param>
		/// <param name="mapMouseOutCallback">Callback function for Map mouse leaving event</param>
		/// <param name="mapCenterChangedCallback">Callback function for Map center point changed event</param>
		/// <param name="mapZoomChangedCallback">Callback function for Map zoom level changed event</param>
		/// <param name="mapTypeChangedCallback">Callback function for Map type changed event</param>
		/// <param name="mapHeadingChangedCallback">Callback function for Map heading changed event</param>
		/// <param name="mapTiltChangedCallback">Callback function for Map tilt position changed event</param>
		/// <param name="mapBoundsChangedCallback">Callback function for Map boundaries changed event</param>
		/// <param name="mapProjectionChangedCallback">Callback function for Map projection changed event</param>
		/// <param name="mapDraggableChangedCallback">Callback function for Map draggable changed event</param>
		/// <param name="mapStreetviewChangedCallback">Callback function for Map street-view changed event</param>
		/// <param name="mapDragCallback">Callback function for Map dragging event</param>
		/// <param name="mapDragEndCallback">Callback function for Map drag ended event</param>
		/// <param name="mapDragStartCallback">Callback function for Map drag started event</param>
		/// <param name="mapResizedCallback">Callback function for Map resized event</param>
		/// <param name="mapTilesLoadedCallback">Callback function for Map tiles loaded event</param>
		/// <param name="mapIdleCallback">Callback function for Map idle event</param>
		/// <returns>Async task</returns>
		Task InitMapAsync(string apiKey, 
			string mapContainerId,
			string backgroundColor = "lightgray",
			int controlSize = 40,
			Func<string, Task> mapInitializedCallback = null,
			Func<GeolocationCoordinate, Task> mapClickedCallback = null,
			Func<GeolocationCoordinate, Task> mapDoubleClickedCallback = null,
			Func<GeolocationCoordinate, Task> mapContextMenuCallback = null,
			Func<GeolocationCoordinate, Task> mapMouseUpCallback = null,
			Func<GeolocationCoordinate, Task> mapMouseDownCallback = null,
			Func<GeolocationCoordinate, Task> mapMouseMoveCallback = null,
			Func<Task> mapMouseOverCallback = null,
			Func<Task> mapMouseOutCallback = null,
			Func<GeolocationCoordinate, Task> mapCenterChangedCallback = null,
			Func<byte, Task> mapZoomChangedCallback = null,
			Func<GoogleMapTypes, Task> mapTypeChangedCallback = null,
			Func<int, Task> mapHeadingChangedCallback = null,
			Func<byte, Task> mapTiltChangedCallback = null,
			Func<Task> mapBoundsChangedCallback = null,
			Func<Task> mapProjectionChangedCallback = null,
			Func<Task> mapDraggableChangedCallback = null,
			Func<Task> mapStreetviewChangedCallback = null,
			Func<GeolocationCoordinate, Task> mapDragCallback = null,
			Func<GeolocationCoordinate, Task> mapDragEndCallback = null,
			Func<GeolocationCoordinate, Task> mapDragStartCallback = null,
			Func<Rect, Task> mapResizedCallback = null,
			Func<Task> mapTilesLoadedCallback = null,
			Func<Task> mapIdleCallback = null,
			GoogleMapRestriction restriction = null);

		/// <summary>
		/// Sets the center point as coordinates of the Map.
		/// </summary>
		/// <param name="latitude">Latitude component</param>
		/// <param name="longitude">Longitude component</param>
		/// <returns>Async task</returns>
		Task SetCenterAsync(double latitude, double longitude);

		/// <summary>
		/// Sets the center point as coordinates after Address was resolved to coords of the Map.
		/// </summary>
		/// <param name="address">Address as required center point</param>
		/// <returns>Async task</returns>
		Task SetCenterAsync(string address);

		/// <summary>
		/// Sets the center point as coordinates of the Map with smooth slide animation.
		/// </summary>
		/// <param name="latitude">Latitude component</param>
		/// <param name="longitude">Longitude component</param>
		/// <returns>Async task</returns>
		Task PanToAsync(double latitude, double longitude);

		/// <summary>
		/// Sets the center point as coordinates after Address was resolved to coords of the Map smooth slide animation.
		/// </summary>
		/// <param name="address">Address as required center point</param>
		/// <returns>Async task</returns>
		Task PanToAsync(string address);

		/// <summary>
		/// Sets map Zoom level.
		/// </summary>
		/// <param name="zoom">Required Zoom level should be 0-22</param>
		/// <returns>Async task</returns>
		Task SetZoomAsync(byte zoom);

		/// <summary>
		/// Sets the map type.
		/// </summary>
		/// <param name="googleMapType">Required <see cref="GoogleMapTypes"/></param>
		/// <returns>Async task</returns>
		Task SetMapTypeAsync(GoogleMapTypes googleMapType);

		/// <summary>
		/// Sets the compass heading for aerial imagery measured in degrees from cardinal direction North.
		/// </summary>
		/// <param name="heading">Required heading 0-360</param>
		/// <returns>Async task</returns>
		Task SetHeadingAsync(int heading);

		/// <summary>
		/// Controls the automatic switching behavior for the angle of incidence of the map. The only allowed values are 0 and 45. setTilt(0) causes the map to always use a 0° overhead view regardless of the zoom level and viewport. 
		/// setTilt(45) causes the tilt angle to automatically switch to 45 whenever 45° imagery is available for the current zoom level and viewport, and switch back to 0 whenever 45° imagery is not available (this is the default behavior). 
		/// </summary>
		/// <param name="tilt">Required tilt 0 or 45</param>
		/// <returns>Async task</returns>
		Task SetTiltAsync(byte tilt);

		/// <summary>
		/// Notify Maps JS about container DIV resized.
		/// </summary>
		/// <returns>Async task</returns>
		Task ResizeMapAsync();

		/// <summary>
		/// Controls whether the map icons are clickable or not. A map icon represents a point of interest, also known as a POI. 
		/// To disable the clickability of map icons, pass a value of false to this method.
		/// </summary>
		/// <param name="isClickable">Icons are clickable or not</param>
		/// <returns>Async task</returns>
		Task SetClickableIconsAsync(bool isClickable);

		/// <summary>
		/// Sets given options to Map.
		/// </summary>
		/// <param name="options">Google JavaScript Maps options</param>
		/// <returns>Async task</returns>
		Task SetOptionsAsync(ExpandoObject options);

		/// <summary>
		/// Creates Custom Controls on the Map on the given position with event callbacks.
		/// </summary>
		/// <param name="mapCustomControls">Enumerable CustomControl elements</param>
		/// <returns>Async task</returns>
		Task CreateCustomControlsAsync(IEnumerable<GoogleMapCustomControl> mapCustomControls);

		/// <summary>
		/// Creates and removes Markers on the Map with InfoWindows on the given position with event callbacks.
		/// </summary>
		/// <param name="newMarkers">Enumerable new markers to add</param>
		/// <param name="markers">Enumerable markers removed or replaced</param>
		/// <returns>Async task</returns>
		Task CreateMarkersAsync(IEnumerable<GoogleMapMarker>? newMarkers, IEnumerable<GoogleMapMarker>? markers);

		/// <summary>
		/// Returns the lat/lng bounds of the current viewport. If more than one copy of the world is visible, 
		/// the bounds range in longitude from -180 to 180 degrees inclusive. If the map is not yet initialized or 
		/// center and zoom have not been set then the result is undefined. For vector maps with non-zero tilt or heading, 
		/// the returned lat/lng bounds represents the smallest bounding box that includes the visible region of the map's viewport.
		/// </summary>
		/// <returns>Async task</returns>
		ValueTask<GoogleMapLatLngBounds> GetBoundsAsync();

		/// <summary>
		/// Returns the position displayed at the center of the map. Note that this LatLng object is not wrapped. See LatLng for more information. 
		/// If the center or bounds have not been set then the result is undefined.
		/// </summary>
		/// <returns>Async task</returns>
		ValueTask<GoogleMapLatLng> GetCenterAsync();

		/// <summary>
		/// Returns HTMLElement The mapDiv of the map.
		/// </summary>
		/// <returns>Async task</returns>
		ValueTask<IJSObjectReference> GetDivAsync();

		/// <summary>
		/// Creates and removes Polyline on the Map with given values and event callbacks.
		/// </summary>
		/// <param name="googleMapPolylineOptions"></param>
		/// <returns></returns>
		Task AddPolyline(params GoogleMapPolylineOptions[] googleMapPolylineOptions);
	}
}