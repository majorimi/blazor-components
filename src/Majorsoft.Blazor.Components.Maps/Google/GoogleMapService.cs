
using Microsoft.JSInterop;

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Majorsoft.Blazor.Components.Maps.Google
{
	/// <summary>
	/// Default implementation of <see cref="IGoogleMapService"/>
	/// </summary>
	public sealed class GoogleMapService : IGoogleMapService
	{
		private readonly IJSRuntime _jsRuntime;
		private IJSObjectReference _mapsJs;
		private DotNetObjectReference<GoogleMapEventInfo> _dotNetObjectReference;

		public string MapContainerId { get; private set; }

		public GoogleMapService(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime;
		}

		public async Task InitMapAsync(string apiKey,
			string mapContainerId,
			string backgroundColor,
			int controlSize,
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
			Func<GoogleMapTypes, Task>? mapTypeChangedCallback = null,
			Func<int, Task>? mapHeadingChangedCallback = null,
			Func<byte, Task>? mapTiltChangedCallback = null,
			Func<Task>? mapBoundsChangedCallback = null,
			Func<Task>? mapProjectionChangedCallback = null,
			Func<Task>? mapDraggableChangedCallback = null,
			Func<Task>? mapStreetviewChangedCallback = null,
			Func<GeolocationCoordinate, Task>? mapDragCallback = null,
			Func<GeolocationCoordinate, Task>? mapDragEndCallback = null,
			Func<GeolocationCoordinate, Task>? mapDragStartCallback = null,
			Func<Rect, Task>? mapResizedCallback = null,
			Func<Task>? mapTilesLoadedCallback = null,
			Func<Task>? mapIdleCallback = null,
			GoogleMapRestriction restriction = null)
		{
			if(MapContainerId == mapContainerId)
			{
				return; //Prevent double init...
			}

			MapContainerId = mapContainerId;
			await CheckJsObjectAsync();

			var info = new GoogleMapEventInfo(mapContainerId, 
				mapInitializedCallback: mapInitializedCallback,
				mapClickedCallback: mapClickedCallback,
				mapDoubleClickedCallback: mapDoubleClickedCallback,
				mapContextMenuCallback: mapContextMenuCallback,
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

			_dotNetObjectReference = DotNetObjectReference.Create<GoogleMapEventInfo>(info);

			await _mapsJs.InvokeVoidAsync("init", apiKey, mapContainerId, _dotNetObjectReference, backgroundColor, controlSize, restriction);
		}

		public async Task SetCenterAsync(double latitude, double longitude)
		{
			await CheckJsObjectAsync();
			await _mapsJs.InvokeVoidAsync("setCenterCoords", MapContainerId, latitude, longitude);
		}

		public async Task SetCenterAsync(string address)
		{
			await CheckJsObjectAsync();
			await _mapsJs.InvokeVoidAsync("setCenterAddress", MapContainerId, address);
		}

		public async Task SetZoomAsync(byte zoom)
		{
			await CheckJsObjectAsync();
			await _mapsJs.InvokeVoidAsync("setZoom", MapContainerId, zoom);
		}

		public async Task PanToAsync(double latitude, double longitude)
		{
			await CheckJsObjectAsync();
			await _mapsJs.InvokeVoidAsync("panToCoords", MapContainerId, latitude, longitude);
		}

		public async Task PanToAsync(string address)
		{
			await CheckJsObjectAsync();
			await _mapsJs.InvokeVoidAsync("panToAddress", MapContainerId, address);
		}

		public async Task SetMapTypeAsync(GoogleMapTypes googleMapType)
		{
			await CheckJsObjectAsync();
			await _mapsJs.InvokeVoidAsync("setMapType", MapContainerId, googleMapType.ToString().ToLower());
		}

		public async Task SetHeadingAsync(int heading)
		{
			await CheckJsObjectAsync();
			await _mapsJs.InvokeVoidAsync("setHeading", MapContainerId, heading);
		}

		public async Task SetTiltAsync(byte tilt)
		{
			await CheckJsObjectAsync();
			await _mapsJs.InvokeVoidAsync("setTilt", MapContainerId, tilt);
		}

		public async Task ResizeMapAsync()
		{
			await CheckJsObjectAsync();
			await _mapsJs.InvokeVoidAsync("resizeMap", MapContainerId);
		}

		public async Task SetClickableIconsAsync(bool isClickable)
		{
			await CheckJsObjectAsync();
			await _mapsJs.InvokeVoidAsync("setClickableIcons", MapContainerId, isClickable);
		}

		public async Task SetOptionsAsync(ExpandoObject options)
		{
			await CheckJsObjectAsync();
			await _mapsJs.InvokeVoidAsync("setOptions", MapContainerId, options);
		}

		public async Task CreateCustomControlsAsync(IEnumerable<GoogleMapCustomControl> mapCustomControls)
		{
			await CheckJsObjectAsync();
			_dotNetObjectReference.Value.AddCustomControls(mapCustomControls);

			await _mapsJs.InvokeVoidAsync("createCustomControls", MapContainerId, 
				(object)mapCustomControls.Cast<GoogleMapCustomControlBase>().ToArray());
		}

		public async Task CreateMarkersAsync(IEnumerable<GoogleMapMarker>? newMarkers, IEnumerable<GoogleMapMarker>? markers)
		{
			await CheckJsObjectAsync();

			if (newMarkers is null && markers is null) //Clear
			{
				await _mapsJs.InvokeVoidAsync("removeMarkers", MapContainerId,
					(object)_dotNetObjectReference.Value.Markers
						.Select(s => s.Value)
						.Cast<GoogleMapMarkerBase>()
						.ToArray());

				_dotNetObjectReference.Value.RemoveMarkers(_dotNetObjectReference.Value.Markers.Select(s => s.Value));

				return;
			}

			//Add new Markers
			if (newMarkers is not null)
			{
				_dotNetObjectReference.Value.AddMarkers(newMarkers);
				if (newMarkers.Count() > 0)
				{
					await _mapsJs.InvokeVoidAsync("createMarkers", MapContainerId,
						(object)newMarkers.Cast<GoogleMapMarkerBase>().ToArray());
				}
			}

			if (markers is not null)
			{
				//Detect switched objects add new markers to the map
				newMarkers = markers.Select(x => new KeyValuePair<string, GoogleMapMarker>(x.Id, x))
					.Except(_dotNetObjectReference.Value.Markers)
					.Distinct().Select(s => s.Value).ToList();

				if (newMarkers.Count() > 0)
				{
					_dotNetObjectReference.Value.AddMarkers(newMarkers);

					await _mapsJs.InvokeVoidAsync("createMarkers", MapContainerId,
						(object)newMarkers.Cast<GoogleMapMarkerBase>().ToArray());
				}

				//Detect removed markers from the map
				var removedMarkers = _dotNetObjectReference.Value.Markers
					.Except(markers.Select(x => new KeyValuePair<string, GoogleMapMarker>(x.Id, x)))
					.Distinct().Select(s => s.Value).ToList();

				if (removedMarkers.Count() > 0)
				{
					_dotNetObjectReference.Value.RemoveMarkers(removedMarkers);

					await _mapsJs.InvokeVoidAsync("removeMarkers", MapContainerId,
						(object)removedMarkers.Cast<GoogleMapMarkerBase>().ToArray());
				}

				////Update markers NOT SUPPORTED
				//var updateMarkers = _dotNetObjectReference.Value.Markers
				//	.Intersect(markers.Select(x => new KeyValuePair<string, GoogleMapMarker>(x.Id, x)))
				//	.Select(s => s.Value).ToList();

				//if (updateMarkers.Count() > 0)
				//{
				//	await _mapsJs.InvokeVoidAsync("updateMarkers", MapContainerId,
				//		(object)updateMarkers.Cast<GoogleMapMarkerBase>().ToArray());
				//}
			}
		}

		public async ValueTask<GoogleMapLatLngBounds> GetBoundsAsync()
		{
			await CheckJsObjectAsync();
			return await _mapsJs.InvokeAsync<GoogleMapLatLngBounds>("getBounds", MapContainerId);
		}

		public async ValueTask<GoogleMapLatLng> GetCenterAsync()
		{
			await CheckJsObjectAsync();
			return await _mapsJs.InvokeAsync<GoogleMapLatLng>("getCenter", MapContainerId);
		}

		public async ValueTask<IJSObjectReference> GetDivAsync()
		{
			await CheckJsObjectAsync();
			return await _mapsJs.InvokeAsync<IJSObjectReference>("getDiv", MapContainerId);
		}

		public async Task AddPolyline(params GoogleMapPolylineOptions[] googleMapPolylineOptions)
		{
			await CheckJsObjectAsync();
			await _mapsJs.InvokeVoidAsync("polylineSetMap", MapContainerId, googleMapPolylineOptions);
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