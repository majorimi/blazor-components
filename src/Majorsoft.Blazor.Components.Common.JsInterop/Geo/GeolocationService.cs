using Microsoft.JSInterop;

using System;
using System.Collections.Generic;
using System.Linq;
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
		private List<DotNetObjectReference<GeolocationEventWatcherInfo>> _dotNetObjectReferences;

		public GeolocationService(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime;
			_dotNetObjectReferences = new List<DotNetObjectReference<GeolocationEventWatcherInfo>>();
		}

		public async Task GetCurrentPosition(Func<GeolocationResult, Task> locationResultCallback, bool highAccuracy, TimeSpan? timeout, TimeSpan? cacheTime)
		{
			await CheckJsObjectAsync();

			var info = new GeolocationEventCurrentPositionInfo(locationResultCallback);
			var dotnetRef = DotNetObjectReference.Create<GeolocationEventCurrentPositionInfo>(info);
			info.DotNetObjectReference = dotnetRef; //Store ref to self dispose.

			await _geoJs.InvokeVoidAsync("getCurrentPosition", dotnetRef, 
				highAccuracy, 
				timeout?.TotalMilliseconds ?? DefaultTimeOut, 
				cacheTime?.TotalMilliseconds ?? DefaultCacheTime);
		}

		public async Task<int> AddGeolocationWatcher(Func<GeolocationResult, Task> locationEventsCallback, bool highAccuracy = false, TimeSpan? timeout = null, TimeSpan? cacheTime = null)
		{
			await CheckJsObjectAsync();

			var info = new GeolocationEventWatcherInfo(locationEventsCallback);
			var dotnetRef = DotNetObjectReference.Create<GeolocationEventWatcherInfo>(info);

			var id = await _geoJs.InvokeAsync<int>("addGeolocationWatcher", dotnetRef,
				highAccuracy,
				timeout?.TotalMilliseconds ?? DefaultTimeOut,
				cacheTime?.TotalMilliseconds ?? DefaultCacheTime);

			dotnetRef.Value.HandlerId = id;
			_dotNetObjectReferences.Add(dotnetRef);
			return id;
		}

		public async Task RemoveGeolocationWatcher(int handlerId)
		{
			await CheckJsObjectAsync();

			await _geoJs.InvokeVoidAsync("removeGeolocationWatcher", handlerId);

			var dotNetRefs = _dotNetObjectReferences.Where(x => x.Value.HandlerId == handlerId);
			_dotNetObjectReferences = _dotNetObjectReferences.Except(dotNetRefs).ToList();

			foreach (var item in dotNetRefs)
			{
				item.Dispose();
			}
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
				await _geoJs.InvokeVoidAsync("dispose", 
					(object)_dotNetObjectReferences.Select(s => s.Value.HandlerId).Distinct().ToArray());

				await _geoJs.DisposeAsync();
			}

			foreach (var item in _dotNetObjectReferences)
			{
				item.Dispose();
			}
		}
	}
}