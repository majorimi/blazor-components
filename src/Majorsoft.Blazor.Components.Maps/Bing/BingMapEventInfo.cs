using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Majorsoft.Blazor.Components.Maps.Bing
{
	internal sealed class BingMapEventInfo
	{
		private readonly string _mapContainerId;
		private readonly Func<string, Task>? _mapInitializedCallback;
		private readonly Func<GeolocationCoordinate, Task>? _mapClickedCallback;
		private readonly Func<GeolocationCoordinate, Task>? _mapDoubleClickedCallback;
		private readonly Func<GeolocationCoordinate, Task>? _mapContextMenuCallback;
		private readonly Func<GeolocationCoordinate, Task>? _mapMouseUpCallback;
		private readonly Func<GeolocationCoordinate, Task>? _mapMouseDownCallback;
		private readonly Func<GeolocationCoordinate, Task>? _mapMouseMoveCallback;
		private readonly Func<Task>? _mapMouseOverCallback;
		private readonly Func<Task>? _mapMouseOutCallback;
		private readonly Func<GeolocationCoordinate, Task>? _mapCenterChangedCallback;
		private readonly Func<byte, Task>? _mapZoomChangedCallback;
		private readonly Func<Task>? _mapBoundsChangedCallback;
		private readonly Func<GeolocationCoordinate, Task>? _mapDragCallback;
		private readonly Func<GeolocationCoordinate, Task>? _mapDragEndCallback;
		private readonly Func<GeolocationCoordinate, Task>? _mapDragStartCallback;
		private readonly Func<Task>? _mapTilesLoadedCallback;
		private readonly Func<Task>? _mapIdleCallback;
		private readonly Func<Rect, Task>? _mapResizedCallback;

		public BingMapEventInfo(string mapContainerId,
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
			_mapContainerId = mapContainerId;
			_mapInitializedCallback = mapInitializedCallback;
			_mapClickedCallback = mapClickedCallback;
			_mapDoubleClickedCallback = mapDoubleClickedCallback;
			_mapContextMenuCallback = mapContextMenuCallback;
			_mapMouseUpCallback = mapMouseUpCallback;
			_mapMouseDownCallback = mapMouseDownCallback;
			_mapMouseMoveCallback = mapMouseMoveCallback;
			_mapMouseOverCallback = mapMouseOverCallback;
			_mapMouseOutCallback = mapMouseOutCallback;
			_mapCenterChangedCallback = mapCenterChangedCallback;
			_mapZoomChangedCallback = mapZoomChangedCallback;
			_mapBoundsChangedCallback = mapBoundsChangedCallback;
			_mapDragCallback = mapDragCallback;
			_mapDragEndCallback = mapDragEndCallback;
			_mapDragStartCallback = mapDragStartCallback;
			_mapTilesLoadedCallback = mapTilesLoadedCallback;
			_mapIdleCallback = mapIdleCallback;
			_mapResizedCallback = mapResizedCallback;
		}

		[JSInvokable("MapInitialized")]
		public async Task MapInitialized(string mapContainerId)
		{
			if (_mapInitializedCallback is not null)
				await _mapInitializedCallback.Invoke(mapContainerId);
		}

		[JSInvokable("MarkerClicked")]
		public async Task MarkerClicked(string markerId)
		{
			// placeholder - actual marker click handled in component via Marker collection callbacks
		}

		[JSInvokable("MarkerDrag")]
		public async Task MarkerDrag(string markerId, GeolocationCoordinate coord)
		{
			// placeholder
		}

		[JSInvokable("MarkerDragEnd")]
		public async Task MarkerDragEnd(string markerId, GeolocationCoordinate coord)
		{
			// placeholder
		}

		[JSInvokable("MarkerDragStart")]
		public async Task MarkerDragStart(string markerId, GeolocationCoordinate coord)
		{
			// placeholder
		}

		[JSInvokable("MapClicked")]
		public async Task MapClicked(GeolocationCoordinate coord)
		{
			if (_mapClickedCallback is not null)
				await _mapClickedCallback.Invoke(coord);
		}

		[JSInvokable("MapDoubleClicked")]
		public async Task MapDoubleClicked(GeolocationCoordinate coord)
		{
			if (_mapDoubleClickedCallback is not null)
				await _mapDoubleClickedCallback.Invoke(coord);
		}

		[JSInvokable("MapContextMenu")]
		public async Task MapContextMenu(GeolocationCoordinate coord)
		{
			if (_mapContextMenuCallback is not null)
				await _mapContextMenuCallback.Invoke(coord);
		}

		[JSInvokable("MapMouseUp")]
		public async Task MapMouseUp(GeolocationCoordinate coord)
		{
			if (_mapMouseUpCallback is not null)
				await _mapMouseUpCallback.Invoke(coord);
		}

		[JSInvokable("MapMouseDown")]
		public async Task MapMouseDown(GeolocationCoordinate coord)
		{
			if (_mapMouseDownCallback is not null)
				await _mapMouseDownCallback.Invoke(coord);
		}

		[JSInvokable("MapMouseMove")]
		public async Task MapMouseMove(GeolocationCoordinate coord)
		{
			if (_mapMouseMoveCallback is not null)
				await _mapMouseMoveCallback.Invoke(coord);
		}

		[JSInvokable("MapMouseOver")]
		public async Task MapMouseOver()
		{
			if (_mapMouseOverCallback is not null)
				await _mapMouseOverCallback.Invoke();
		}

		[JSInvokable("MapMouseOut")]
		public async Task MapMouseOut()
		{
			if (_mapMouseOutCallback is not null)
				await _mapMouseOutCallback.Invoke();
		}

		[JSInvokable("MapCenterChanged")]
		public async Task MapCenterChanged(GeolocationCoordinate coord)
		{
			if (_mapCenterChangedCallback is not null)
				await _mapCenterChangedCallback.Invoke(coord);
		}

		[JSInvokable("MapZoomChanged")]
		public async Task MapZoomChanged(byte zoom)
		{
			if (_mapZoomChangedCallback is not null)
				await _mapZoomChangedCallback.Invoke(zoom);
		}

		[JSInvokable("MapBoundsChanged")]
		public async Task MapBoundsChanged()
		{
			if (_mapBoundsChangedCallback is not null)
				await _mapBoundsChangedCallback.Invoke();
		}

		[JSInvokable("MapDrag")]
		public async Task MapDrag(GeolocationCoordinate coord)
		{
			if (_mapDragCallback is not null)
				await _mapDragCallback.Invoke(coord);
		}

		[JSInvokable("MapDragEnd")]
		public async Task MapDragEnd(GeolocationCoordinate coord)
		{
			if (_mapDragEndCallback is not null)
				await _mapDragEndCallback.Invoke(coord);
		}

		[JSInvokable("MapDragStart")]
		public async Task MapDragStart(GeolocationCoordinate coord)
		{
			if (_mapDragStartCallback is not null)
				await _mapDragStartCallback.Invoke(coord);
		}

		[JSInvokable("MapTilesLoaded")]
		public async Task MapTilesLoaded()
		{
			if (_mapTilesLoadedCallback is not null)
				await _mapTilesLoadedCallback.Invoke();
		}

		[JSInvokable("MapIdle")]
		public async Task MapIdle()
		{
			if (_mapIdleCallback is not null)
				await _mapIdleCallback.Invoke();
		}
	}
}
