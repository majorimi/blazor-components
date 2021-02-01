using Microsoft.JSInterop;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Majorsoft.Blazor.Components.Maps.Google
{
	/// <summary>
	/// GoogleMapsEventInfo event <see cref="DotNetObjectReference"/> info to handle Google Maps event callbacks
	/// </summary>
	internal sealed class GoogleMapEventInfo
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
		private readonly Func<GoogleMapTypes, Task>? _mapTypeChangedCallback;
		private readonly Func<int, Task>? _mapHeadingChangedCallback;
		private readonly Func<byte, Task>? _mapTiltChangedCallback;
		private readonly Func<Task>? _mapBoundsChangedCallback;
		private readonly Func<Task>? _mapProjectionChangedCallback;
		private readonly Func<Task>? _mapDraggableChangedCallback;
		private readonly Func<Task>? _mapStreetviewChangedCallback;
		private readonly Func<GeolocationCoordinate, Task>? _mapDragCallback;
		private readonly Func<GeolocationCoordinate, Task>? _mapDragEndCallback;
		private readonly Func<GeolocationCoordinate, Task>? _mapDragStartCallback;
		private readonly Func<Rect, Task>? _mapResizedCallback;
		private readonly Func<Task>? _mapTilesLoadedCallback;
		private readonly Func<Task>? _mapIdleCallback;

		private readonly Dictionary<string, GoogleMapCustomControl> _customControls;

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="mapContainerId">HTML Div Id which will contain Google Map</param>
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
		public GoogleMapEventInfo(string mapContainerId, 
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
			Func<Task>? mapIdleCallback = null)
		{
			_mapContainerId = mapContainerId;
			_customControls = new Dictionary<string, GoogleMapCustomControl>();

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
			_mapTypeChangedCallback = mapTypeChangedCallback;
			_mapHeadingChangedCallback = mapHeadingChangedCallback;
			_mapTiltChangedCallback = mapTiltChangedCallback;
			_mapBoundsChangedCallback = mapBoundsChangedCallback;
			_mapProjectionChangedCallback = mapProjectionChangedCallback;
			_mapDraggableChangedCallback = mapDraggableChangedCallback;
			_mapStreetviewChangedCallback = mapStreetviewChangedCallback;
			_mapDragCallback = mapDragCallback;
			_mapDragEndCallback = mapDragEndCallback;
			_mapDragStartCallback = mapDragStartCallback;
			_mapResizedCallback = mapResizedCallback;
			_mapTilesLoadedCallback = mapTilesLoadedCallback;
			_mapIdleCallback = mapIdleCallback;
		}

		public void AddCustomControls(IEnumerable<GoogleMapCustomControl> mapCustomControls)
		{
			foreach (var item in mapCustomControls)
			{
				if (!_customControls.ContainsKey(item.Id))
				{
					_customControls.Add(item.Id, item);
				}
			}
		}

		//Map evetns
		[JSInvokable("MapInitialized")]
		public async Task MapInitialized(string mapContainerId)
		{
			if(_mapContainerId != mapContainerId)
			{
				throw new InvalidProgramException($"{nameof(MapInitialized)} method was called with invalid Map container Div Id: {mapContainerId}, expected Id isL {_mapContainerId}.");
			}

			if (_mapInitializedCallback is not null)
			{
				await _mapInitializedCallback.Invoke(mapContainerId);
			}
		}
		//Mouse
		[JSInvokable("MapClicked")]
		public async Task MapClicked(GeolocationCoordinate geolocationCoordinate)
		{
			if (_mapClickedCallback is not null)
			{
				await _mapClickedCallback.Invoke(geolocationCoordinate);
			}
		}
		[JSInvokable("MapMouseUp")]
		public async Task MapMouseUp(GeolocationCoordinate geolocationCoordinate)
		{
			if (_mapMouseUpCallback is not null)
			{
				await _mapMouseUpCallback.Invoke(geolocationCoordinate);
			}
		}
		[JSInvokable("MapMouseDown")]
		public async Task MapMouseDown(GeolocationCoordinate geolocationCoordinate)
		{
			if (_mapMouseDownCallback is not null)
			{
				await _mapMouseDownCallback.Invoke(geolocationCoordinate);
			}
		}
		[JSInvokable("MapDoubleClicked")]
		public async Task MapDoubleClicked(GeolocationCoordinate geolocationCoordinate)
		{
			if (_mapDoubleClickedCallback is not null)
			{
				await _mapDoubleClickedCallback.Invoke(geolocationCoordinate);
			}
		}
		[JSInvokable("MapContextMenu")]
		public async Task MapContextMenu(GeolocationCoordinate geolocationCoordinate)
		{
			if (_mapContextMenuCallback is not null)
			{
				await _mapContextMenuCallback.Invoke(geolocationCoordinate);
			}
		}
		[JSInvokable("MapMouseMove")]
		public async Task MapMouseMove(GeolocationCoordinate geolocationCoordinate)
		{
			if (_mapMouseMoveCallback is not null)
			{
				await _mapMouseMoveCallback.Invoke(geolocationCoordinate);
			}
		}
		[JSInvokable("MapMouseOver")]
		public async Task MapMouseOver()
		{
			if (_mapMouseOverCallback is not null)
			{
				await _mapMouseOverCallback.Invoke();
			}
		}
		[JSInvokable("MapMouseOut")]
		public async Task MapMouseOut()
		{
			if (_mapMouseOutCallback is not null)
			{
				await _mapMouseOutCallback.Invoke();
			}
		}
		//Changes
		[JSInvokable("MapCenterChanged")]
		public async Task MapCenterChanged(GeolocationCoordinate geolocationCoordinate)
		{
			if (_mapCenterChangedCallback is not null)
			{
				await _mapCenterChangedCallback.Invoke(geolocationCoordinate);
			}
		}
		[JSInvokable("MapZoomChanged")]
		public async Task MapZoomChanged(byte zoom)
		{
			if (_mapZoomChangedCallback is not null)
			{
				await _mapZoomChangedCallback.Invoke(zoom);
			}
		}
		[JSInvokable("MapTypeIdChanged")]
		public async Task MapTypeIdChanged(string mapTypeId)
		{
			if (_mapTypeChangedCallback is not null)
			{
				var mapType = (GoogleMapTypes)Enum.Parse(typeof(GoogleMapTypes), mapTypeId, true);
				await _mapTypeChangedCallback.Invoke(mapType);
			}
		}
		[JSInvokable("MapHeadingChanged")]
		public async Task MapHeadingChanged(int heading)
		{
			if (_mapHeadingChangedCallback is not null)
			{
				await _mapHeadingChangedCallback.Invoke(heading);
			}
		}
		[JSInvokable("MapTiltChanged")]
		public async Task MapTiltChanged(byte tilt)
		{
			if (_mapTiltChangedCallback is not null)
			{
				await _mapTiltChangedCallback.Invoke(tilt);
			}
		}
		[JSInvokable("MapBoundsChanged")]
		public async Task MapBoundsChanged()
		{
			if (_mapBoundsChangedCallback is not null)
			{
				await _mapBoundsChangedCallback.Invoke();
			}
		}
		[JSInvokable("MapProjectionChanged")]
		public async Task MapProjectionChanged()
		{
			if (_mapProjectionChangedCallback is not null)
			{
				await _mapProjectionChangedCallback.Invoke();
			}
		}
		[JSInvokable("MapDraggableChanged")]
		public async Task MapDraggableChanged()
		{
			if (_mapDraggableChangedCallback is not null)
			{
				await _mapDraggableChangedCallback.Invoke();
			}
		}
		[JSInvokable("MapStreetviewChanged")]
		public async Task MapStreetviewChanged()
		{
			if (_mapStreetviewChangedCallback is not null)
			{
				await _mapStreetviewChangedCallback.Invoke();
			}
		}
		//Drag
		[JSInvokable("MapDrag")]
		public async Task MapDrag(GeolocationCoordinate geolocationCoordinate)
		{
			if (_mapDragCallback is not null)
			{
				await _mapDragCallback.Invoke(geolocationCoordinate);
			}
		}
		[JSInvokable("MapDragEnd")]
		public async Task MapDragEnd(GeolocationCoordinate geolocationCoordinate)
		{
			if (_mapDragEndCallback is not null)
			{
				await _mapDragEndCallback.Invoke(geolocationCoordinate);
			}
		}
		[JSInvokable("MapDragStart")]
		public async Task MapDragStart(GeolocationCoordinate geolocationCoordinate)
		{
			if (_mapDragStartCallback is not null)
			{
				await _mapDragStartCallback.Invoke(geolocationCoordinate);
			}
		}
		[JSInvokable("MapResized")]
		public async Task MapResized(Rect size)
		{
			if (_mapResizedCallback is not null)
			{
				await _mapResizedCallback.Invoke(size);
			}
		}
		[JSInvokable("MapTilesLoaded")]
		public async Task MapTilesLoaded()
		{
			if (_mapTilesLoadedCallback is not null)
			{
				await _mapTilesLoadedCallback.Invoke();
			}
		}
		[JSInvokable("MapIdle")]
		public async Task MapIdle()
		{
			if (_mapIdleCallback is not null)
			{
				await _mapIdleCallback.Invoke();
			}
		}

		//Custom control events.
		[JSInvokable("CustomControlClicked")]
		public async Task CustomControlClicked(string id)
		{
			if(_customControls.ContainsKey(id))
			{
				var callback = _customControls[id].OnClickCallback;

				if (callback is not null)
				{
					await callback.Invoke();
				}
			}
		}
	}
}