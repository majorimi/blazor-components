using Microsoft.JSInterop;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Majorsoft.Blazor.Components.Common.JsInterop.Geo
{
	/// <summary>
	/// Geolocation event <see cref="DotNetObjectReference"/> info to handle JS callback
	/// </summary>
	internal sealed class GeolocationEventInfo
	{
		private readonly Func<GeolocationResult, Task> _locationResultCallback;

		public GeolocationEventInfo(Func<GeolocationResult, Task> locationResultCallback)
		{
			_locationResultCallback = locationResultCallback;
		}

		[JSInvokable("GeolocationEvent")]
		public async Task GeolocationEvent(GeolocationResult args)
		{
			await _locationResultCallback(args);
		}
	}

	/// <summary>
	/// Injectable service to handle Browser Geolocation Interops.
	/// </summary>
	public interface IGeolocationService : IAsyncDisposable
	{
		/// <summary>
		/// Get the current position of the device.
		/// </summary>
		/// <param name="locationResultCallback"></param>
		/// <param name="highAccuracy"></param>
		/// <param name="timeout"></param>
		/// <param name="cacheTime"></param>
		/// <returns>Async Task</returns>
		Task GetCurrentPosition(Func<GeolocationResult, Task> locationResultCallback, bool highAccuracy = false, TimeSpan? timeout = null, TimeSpan? cacheTime = null);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="locationEventsCallback"></param>
		/// <param name="highAccuracy"></param>
		/// <param name="timeout"></param>
		/// <param name="cacheTime"></param>
		/// <returns>Async Task</returns>
		Task<int> AddGeolocationWatcher(Func<GeolocationResult, Task> locationEventsCallback, bool highAccuracy = false, TimeSpan? timeout = null, TimeSpan? cacheTime = null);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="handlerId"></param>
		/// <returns>Async Task</returns>
		Task RemoveGeolocationWatcher(int handlerId);
	}

	/// <summary>
	/// Implementation of <see cref="IGeolocationService"/>
	/// </summary>
	public sealed class GeolocationService : IGeolocationService
	{
		private const int DefaultTimeOut = 5000;
		private const int DefaultCacheTime = 0;

		private readonly IJSRuntime _jsRuntime;
		private IJSObjectReference _geoJs;
		private List<int> _registeredEvents;

		public GeolocationService(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime;
			_registeredEvents = new List<int>();
		}

		public async Task GetCurrentPosition(Func<GeolocationResult, Task> locationResultCallback, bool highAccuracy, TimeSpan? timeout, TimeSpan? cacheTime)
		{
			await CheckJsObjectAsync();

			var info = new GeolocationEventInfo(locationResultCallback);
			var dotnetRef = DotNetObjectReference.Create<GeolocationEventInfo>(info);

			await _geoJs.InvokeVoidAsync("getCurrentPosition", dotnetRef, 
				highAccuracy, 
				timeout?.TotalMilliseconds ?? DefaultTimeOut, 
				cacheTime?.TotalMilliseconds ?? DefaultCacheTime);
		}

		public async Task<int> AddGeolocationWatcher(Func<GeolocationResult, Task> locationEventsCallback, bool highAccuracy = false, TimeSpan? timeout = null, TimeSpan? cacheTime = null)
		{
			var info = new GeolocationEventInfo(locationEventsCallback);
			var dotnetRef = DotNetObjectReference.Create<GeolocationEventInfo>(info);

			var id = await _geoJs.InvokeAsync<int>("addGeolocationWatcher", dotnetRef,
				highAccuracy,
				timeout?.TotalMilliseconds ?? DefaultTimeOut,
				cacheTime?.TotalMilliseconds ?? DefaultCacheTime);

			_registeredEvents.Add(id);
			return id;
		}

		public async Task RemoveGeolocationWatcher(int handlerId)
		{
			_registeredEvents.Remove(handlerId);
			await _geoJs.InvokeVoidAsync("removeGeolocationWatcher", handlerId);
		}

		private async Task CheckJsObjectAsync()
		{
			if (_geoJs is null)
			{
#if DEBUG
				_geoJs = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Majorsoft.Blazor.Components.Common.JsInterop/geo.js");
#else
				_geoJs = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Majorsoft.Blazor.Components.Common.JsInterop/geo.min.js");
#endif
			}
		}

		public async ValueTask DisposeAsync()
		{
			if (_geoJs is not null)
			{
				await _geoJs.InvokeVoidAsync("dispose", (object)_registeredEvents.ToArray());

				await _geoJs.DisposeAsync();
			}
		}
	}
}