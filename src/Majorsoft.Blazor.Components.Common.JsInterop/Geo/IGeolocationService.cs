using Microsoft.JSInterop;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majorsoft.Blazor.Components.Common.JsInterop.Geo
{
	public interface IGeolocationService : IAsyncDisposable
	{
		Task<GeolocationResult> GetCurrentPosition(bool highAccuracy = false, int timeout = 5000, int cacheTime = 0);
	}

	public sealed class GeolocationService : IGeolocationService
	{
		private readonly IJSRuntime _jsRuntime;
		private IJSObjectReference _geoJs;

		public GeolocationService(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime;
		}

		public async Task<GeolocationResult> GetCurrentPosition(bool highAccuracy, int timeout, int cacheTime)
		{
			await CheckJsObjectAsync();
			var pos = await _geoJs.InvokeAsync<GeolocationResult>("getCurrentPosition", highAccuracy, timeout, cacheTime);

			return pos;
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
				await _geoJs.DisposeAsync();
			}
		}
	}
}