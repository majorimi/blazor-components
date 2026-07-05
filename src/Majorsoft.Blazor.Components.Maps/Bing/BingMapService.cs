using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Majorsoft.Blazor.Components.Maps.Bing
{
	public sealed class BingMapService : IBingMapService
	{
		private readonly IJSRuntime _jsRuntime;
		private IJSObjectReference? _mapsJs;
		private DotNetObjectReference<BingMapEventInfo>? _dotNetObjectReference;

		public string MapContainerId { get; private set; }

		public BingMapService(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime;
		}

		public async Task InitMapAsync(string apiKey, string mapContainerId,
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
			Func<Rect, Task>? mapResizedCallback = null)
		{
			if (MapContainerId == mapContainerId) return;

			MapContainerId = mapContainerId;
			if (_mapsJs is null)
			{
				_mapsJs = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Majorsoft.Blazor.Components.Maps/bingMaps.js");
			}

			var info = new BingMapEventInfo(mapContainerId,
				mapInitializedCallback,
				mapClickedCallback,
				mapDoubleClickedCallback,
				mapContextMenuCallback,
				mapMouseUpCallback,
				mapMouseDownCallback,
				mapMouseMoveCallback,
				mapMouseOverCallback,
				mapMouseOutCallback,
				mapCenterChangedCallback,
				mapZoomChangedCallback,
				mapBoundsChangedCallback,
				mapDragCallback,
				mapDragEndCallback,
				mapDragStartCallback,
				mapTilesLoadedCallback,
				mapIdleCallback,
				mapResizedCallback);

			_dotNetObjectReference = DotNetObjectReference.Create(info);

			await _mapsJs.InvokeVoidAsync("init", apiKey, mapContainerId, _dotNetObjectReference);
		}

		public async Task SetCenterAsync(double latitude, double longitude)
		{
			if (_mapsJs is null) return;
			await _mapsJs.InvokeVoidAsync("setCenterCoords", MapContainerId, latitude, longitude);
		}

		public async Task SetZoomAsync(byte zoom)
		{
			if (_mapsJs is null) return;
			await _mapsJs.InvokeVoidAsync("setZoom", MapContainerId, zoom);
		}

		public async Task ResizeMapAsync()
		{
			if (_mapsJs is null) return;
			await _mapsJs.InvokeVoidAsync("resizeMap", MapContainerId);
		}

		public async Task SetOptionsAsync(System.Dynamic.ExpandoObject options)
		{
			if (_mapsJs is null) return;
			await _mapsJs.InvokeVoidAsync("setOptions", MapContainerId, options);
		}

		public async Task CreateMarkersAsync(System.Collections.Generic.IEnumerable<Majorsoft.Blazor.Components.Maps.Bing.Markers.BingMapMarker>? newMarkers, System.Collections.Generic.IEnumerable<Majorsoft.Blazor.Components.Maps.Bing.Markers.BingMapMarker>? markers)
		{
			if (_mapsJs is null) return;

			if (newMarkers is null && markers is null)
			{
				await _mapsJs.InvokeVoidAsync("removeMarkers", MapContainerId, (object)Array.Empty<object>());
				return;
			}

			if (newMarkers is not null)
			{
				await _mapsJs.InvokeVoidAsync("createMarkers", MapContainerId, (object)newMarkers);
			}

			if (markers is not null)
			{
				// determine removed markers
				await _mapsJs.InvokeVoidAsync("removeMarkers", MapContainerId, (object)markers);
			}
		}

		public async ValueTask DisposeAsync()
		{
			if (_mapsJs is not null)
			{
				try
				{
					await _mapsJs.InvokeVoidAsync("dispose", MapContainerId);
					await _mapsJs.DisposeAsync();
				}
				catch (JSDisconnectedException) { }
				catch (ObjectDisposedException) { }
			}

			_dotNetObjectReference?.Dispose();
		}
	}
}
