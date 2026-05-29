using System;
using System.Threading.Tasks;

namespace Majorsoft.Blazor.Components.Maps.Bing
{
	/// <summary>
	/// Injectable service to handle Bing JavaScript Maps functionalities. Available on the instance of <see cref="BingMap"/> object ref as well.
	/// </summary>
	public interface IBingMapService : IAsyncDisposable
	{
		/// <summary>
		/// HTML Div Id which was set when map was initialized with <see cref="InitMapAsync"/> method.
		/// </summary>
		string MapContainerId { get; }

		/// <summary>
		/// Initializes Bing Maps JavaScript API for given container and registers event callbacks.
		/// This method must be called once per map instance.
		/// </summary>
		/// <param name="apiKey">Bing Maps API key.</param>
		/// <param name="mapContainerId">HTML Div Id which will contain Bing Map.</param>
		/// <param name="mapInitializedCallback">Callback invoked when map is initialized.</param>
		/// <param name="mapClickedCallback">Callback invoked when map is clicked.</param>
		/// <param name="mapDoubleClickedCallback">Callback invoked when map is double clicked.</param>
		/// <param name="mapContextMenuCallback">Callback invoked when context menu is requested on the map.</param>
		/// <param name="mapMouseUpCallback">Callback invoked on mouse up.</param>
		/// <param name="mapMouseDownCallback">Callback invoked on mouse down.</param>
		/// <param name="mapMouseMoveCallback">Callback invoked when mouse moves over the map.</param>
		/// <param name="mapMouseOverCallback">Callback invoked when mouse enters map area.</param>
		/// <param name="mapMouseOutCallback">Callback invoked when mouse leaves map area.</param>
		/// <param name="mapCenterChangedCallback">Callback invoked when map center changes.</param>
		/// <param name="mapZoomChangedCallback">Callback invoked when map zoom level changes.</param>
		/// <param name="mapBoundsChangedCallback">Callback invoked when map bounds change.</param>
		/// <param name="mapDragCallback">Callback invoked while map is dragged.</param>
		/// <param name="mapDragEndCallback">Callback invoked when map drag ends.</param>
		/// <param name="mapDragStartCallback">Callback invoked when map drag starts.</param>
		/// <param name="mapTilesLoadedCallback">Callback invoked when map tiles have been loaded.</param>
		/// <param name="mapIdleCallback">Callback invoked when map becomes idle.</param>
		/// <param name="mapResizedCallback">Callback invoked when map container is resized (useful to update layout).
		/// </param>
		/// <returns>Async task</returns>
		Task InitMapAsync(string apiKey,
			string mapContainerId,
			Func<string, Task>? mapInitializedCallback = null,
			Func<GeolocationCoordinate, Task>? mapClickedCallback = null,
			Func<GeolocationCoordinate, Task>? mapDoubleClickedCallback = null,
			Func<GeolocationCoordinate, Task>? mapContextMenuCallback = null,
			Func<GeolocationCoordinate, Task>? mapMouseUpCallback = null,
			Func<GeolocationCoordinate, Task>? mapMouseDownCallback = null,
			Func<GeolocationCoordinate, Task>? mapMouseMoveCallback = null,
			Func<Task>? mapMouseOverCallback = null,
			Func<Task>? mapMouseOutCallback = null,
			Func<GeolocationCoordinate, Task>? mapCenterChangedCallback = null,
			Func<byte, Task>? mapZoomChangedCallback = null,
			Func<Task>? mapBoundsChangedCallback = null,
			Func<GeolocationCoordinate, Task>? mapDragCallback = null,
			Func<GeolocationCoordinate, Task>? mapDragEndCallback = null,
			Func<GeolocationCoordinate, Task>? mapDragStartCallback = null,
			Func<Task>? mapTilesLoadedCallback = null,
			Func<Task>? mapIdleCallback = null,
			Func<Rect, Task>? mapResizedCallback = null);

		/// <summary>
		/// Sets the center point as coordinates of the map.
		/// </summary>
		/// <param name="latitude">Latitude component</param>
		/// <param name="longitude">Longitude component</param>
		/// <returns>Async task</returns>
		Task SetCenterAsync(double latitude, double longitude);

		/// <summary>
		/// Sets map zoom level.
		/// </summary>
		/// <param name="zoom">Required Zoom level should be 0-22</param>
		/// <returns>Async task</returns>
		Task SetZoomAsync(byte zoom);

		/// <summary>
		/// Notify Maps JS about container DIV resized.
		/// </summary>
		/// <returns>Async task</returns>
		Task ResizeMapAsync();

		/// <summary>
		/// Sets given options to Map.
		/// </summary>
		/// <param name="options">Map options as dynamic object</param>
		/// <returns>Async task</returns>
		Task SetOptionsAsync(System.Dynamic.ExpandoObject options);

		/// <summary>
		/// Creates and removes markers on the map. New markers provided in <paramref name="newMarkers"/> will be created,
		/// while <paramref name="markers"/> contains the current full marker list (used to remove absent markers on JS side).
		/// </summary>
		/// <param name="newMarkers">Enumerable new markers to add</param>
		/// <param name="markers">Enumerable markers currently present (used to detect removals)</param>
		/// <returns>Async task</returns>
		Task CreateMarkersAsync(System.Collections.Generic.IEnumerable<Majorsoft.Blazor.Components.Maps.Bing.Markers.BingMapMarker>? newMarkers, System.Collections.Generic.IEnumerable<Majorsoft.Blazor.Components.Maps.Bing.Markers.BingMapMarker>? markers);
	}
}
