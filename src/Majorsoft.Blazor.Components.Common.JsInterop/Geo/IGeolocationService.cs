using Microsoft.JSInterop;

using System;
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
		/// <param name="locationResultCallback">Callback function which will receive <see cref="GeolocationResult"/></param>
		/// <param name="highAccuracy">Indicates the application would like to receive the best possible results</param>
		/// <param name="timeout">Representing the maximum length of time (in milliseconds) the device is allowed to take in order to return a position. Default is 5000</param>
		/// <param name="cacheTime">Indicating the maximum age in milliseconds of a possible cached position that is acceptable to return. Default is 0 (no cache)</param>
		/// <returns>Async Task</returns>
		Task GetCurrentPosition(Func<GeolocationResult, Task> locationResultCallback, bool highAccuracy = false, TimeSpan? timeout = null, TimeSpan? cacheTime = null);

		/// <summary>
		/// Register a handler function that will be called automatically each time the position of the device changes.
		/// </summary>
		/// <param name="locationEventsCallback">Callback function which will receive <see cref="GeolocationResult"/></param>
		/// <param name="highAccuracy">Indicates the application would like to receive the best possible results</param>
		/// <param name="timeout">Representing the maximum length of time (in milliseconds) the device is allowed to take in order to return a position. Default is 5000</param>
		/// <param name="cacheTime">Indicating the maximum age in milliseconds of a possible cached position that is acceptable to return. Default is 0 (no cache)</param>
		/// <returns>Async Task with int32 handlerId which can be used to remove watcher in <see cref="RemoveGeolocationWatcher"/></returns>
		Task<int> AddGeolocationWatcher(Func<GeolocationResult, Task> locationEventsCallback, bool highAccuracy = false, TimeSpan? timeout = null, TimeSpan? cacheTime = null);

		/// <summary>
		/// Unregister location/error monitoring handlers previously installed using <see cref="AddGeolocationWatcher"/>
		/// </summary>
		/// <param name="handlerId"></param>
		/// <returns>Async Task</returns>
		Task RemoveGeolocationWatcher(int handlerId);
	}
}