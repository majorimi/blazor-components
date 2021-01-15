using Microsoft.JSInterop;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Majorsoft.Blazor.Components.Common.JsInterop.Geo
{
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
			await CheckJsObjectAsync();

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
			await CheckJsObjectAsync();

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