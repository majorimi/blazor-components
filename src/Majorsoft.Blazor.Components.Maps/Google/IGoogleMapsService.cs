using Microsoft.JSInterop;

using System;
using System.Threading.Tasks;

namespace Majorsoft.Blazor.Components.Maps.Google
{
	/// <summary>
	/// Injectable service to handle Google JavaScript Maps functionalities.
	/// </summary>
	public interface IGoogleMapsService : IAsyncDisposable
	{
		/// <summary>
		/// HTML Div Id which was set when Maps initialized with <see cref="InitMap"/> method.
		/// </summary>
		string MapContainerId { get; }

		/// <summary>
		/// This function must be called only once to initialize Google JavaScript Maps with ApiKey and event callbacks.
		/// </summary>
		/// <param name="apiKey">Google API Key which has permission for Google JavaScript Maps</param>
		/// <param name="mapContainerId">HTML Div Id which will contain Google Map</param>
		/// <param name="mapInitializedCallback">Callback function for Map initialized event</param>
		/// <param name="mapClickedCallback">Callback function for Map clicked event</param>
		/// <param name="mapDoubleClickedCallback">Callback function for Map double clicked event</param>
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
		Task InitMap(string apiKey, string mapContainerId,
			Func<string, Task> mapInitializedCallback = null,
			Func<GeolocationCoordinate, Task> mapClickedCallback = null,
			Func<GeolocationCoordinate, Task> mapDoubleClickedCallback = null,
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
			Func<Task> mapIdleCallback = null);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="latitude"></param>
		/// <param name="longitude"></param>
		/// <returns></returns>
		Task SetCenter(double latitude, double longitude);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		Task SetCenter(string address);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="zoom"></param>
		/// <returns></returns>
		Task SetZoom(byte zoom);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="latitude"></param>
		/// <param name="longitude"></param>
		/// <returns></returns>
		Task PanTo(double latitude, double longitude);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="googleMapType"></param>
		/// <returns></returns>
		Task SetMapType(GoogleMapTypes googleMapType);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="heading"></param>
		/// <returns></returns>
		Task SetHeading(int heading);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="tilt"></param>
		/// <returns></returns>
		Task SetTilt(byte tilt);

		Task ResizeMap();

	}

	/// <summary>
	/// Default implementation of <see cref="IGoogleMapsService"/>
	/// </summary>
	public sealed class GoogleMapsService : IGoogleMapsService
	{
		private readonly IJSRuntime _jsRuntime;
		private IJSObjectReference _mapsJs;
		private DotNetObjectReference<GoogleMapsEventInfo> _dotNetObjectReference;

		public string MapContainerId { get; private set; }

		public GoogleMapsService(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime;
		}

		public async Task InitMap(string apiKey, string mapContainerId,
			Func<string, Task> mapInitializedCallback,
			Func<GeolocationCoordinate, Task> mapClickedCallback = null,
			Func<GeolocationCoordinate, Task> mapDoubleClickedCallback = null,
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
			Func<Task> mapIdleCallback = null)
		{
			if(MapContainerId == mapContainerId)
			{
				return; //Prevent double init...
			}

			MapContainerId = mapContainerId;
			await CheckJsObjectAsync();

			var info = new GoogleMapsEventInfo(mapContainerId, 
				mapInitializedCallback: mapInitializedCallback,
				mapClickedCallback: mapClickedCallback,
				mapDoubleClickedCallback: mapDoubleClickedCallback,
				mapMouseUpCallback: mapMouseUpCallback,
				mapMouseDownCallback: mapMouseDownCallback,
				mapMouseMoveCallback: mapMouseMoveCallback,
				mapMouseOverCallback: mapMouseOverCallback,
				mapMouseOutCallback: mapMouseOutCallback,
				mapCenterChangedCallback: mapCenterChangedCallback,
				mapZoomChangedCallback: mapZoomChangedCallback,
				mapTypeChangedCallback: mapTypeChangedCallback,
				mapHeadingChangedCallback: mapHeadingChangedCallback,
				mapTiltChangedCallback: mapTiltChangedCallback,
				mapBoundsChangedCallback: mapBoundsChangedCallback,
				mapProjectionChangedCallback: mapProjectionChangedCallback,
				mapDraggableChangedCallback: mapDraggableChangedCallback,
				mapStreetviewChangedCallback: mapStreetviewChangedCallback,
				mapDragCallback: mapDragCallback,
				mapDragEndCallback: mapDragEndCallback,
				mapDragStartCallback: mapDragStartCallback,
				mapResizedCallback: mapResizedCallback,
				mapTilesLoadedCallback: mapTilesLoadedCallback,
				mapIdleCallback: mapIdleCallback);

			_dotNetObjectReference = DotNetObjectReference.Create<GoogleMapsEventInfo>(info);

			await _mapsJs.InvokeVoidAsync("init", apiKey, mapContainerId, _dotNetObjectReference);
		}

		public async Task SetCenter(double latitude, double longitude)
		{
			await CheckJsObjectAsync();
			await _mapsJs.InvokeVoidAsync("setCenterCoords", MapContainerId, latitude, longitude);
		}

		public async Task SetCenter(string address)
		{
			await CheckJsObjectAsync();
			await _mapsJs.InvokeVoidAsync("setCenterAddress", MapContainerId, address);
		}

		public async Task SetZoom(byte zoom)
		{
			await CheckJsObjectAsync();
			await _mapsJs.InvokeVoidAsync("setZoom", MapContainerId, zoom);
		}

		public async Task PanTo(double latitude, double longitude)
		{
			await CheckJsObjectAsync();
			await _mapsJs.InvokeVoidAsync("panTo", MapContainerId, latitude, longitude);
		}

		public async Task SetMapType(GoogleMapTypes googleMapType)
		{
			await CheckJsObjectAsync();
			await _mapsJs.InvokeVoidAsync("setMapType", MapContainerId, googleMapType.ToString().ToLower());
		}

		public async Task SetHeading(int heading)
		{
			await CheckJsObjectAsync();
			await _mapsJs.InvokeVoidAsync("setHeading", MapContainerId, heading);
		}

		public async Task SetTilt(byte tilt)
		{
			await CheckJsObjectAsync();
			await _mapsJs.InvokeVoidAsync("setTilt", MapContainerId, tilt);
		}

		public async Task ResizeMap()
		{
			await CheckJsObjectAsync();
			await _mapsJs.InvokeVoidAsync("resizeMap", MapContainerId);
		}

		private async Task CheckJsObjectAsync()
		{
			if (_mapsJs is null)
			{
#if DEBUG
				_mapsJs = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Majorsoft.Blazor.Components.Maps/googleMaps.js");
#else
				_mapsJs = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Majorsoft.Blazor.Components.Maps/googleMaps.min.js");
#endif
			}
		}

		public async ValueTask DisposeAsync()
		{
			if (_mapsJs is not null)
			{
				await _mapsJs.InvokeVoidAsync("dispose", MapContainerId);

				await _mapsJs.DisposeAsync();
			}

			_dotNetObjectReference?.Dispose();
		}
	}
}