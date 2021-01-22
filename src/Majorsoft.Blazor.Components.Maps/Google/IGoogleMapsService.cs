using Microsoft.JSInterop;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majorsoft.Blazor.Components.Maps.Google
{
	/// <summary>
	/// GoogleMapsEventInfo event <see cref="DotNetObjectReference"/> info to handle Google Maps event callbacks
	/// </summary>
	internal sealed class GoogleMapsEventInfo
	{
		private readonly string _mapContainerId;
		private readonly Func<string, Task> _mapInitializedCallback;

		public GoogleMapsEventInfo(string mapContainerId, 
			Func<string, Task> mapInitializedCallback = null)
		{
			_mapContainerId = mapContainerId;
			_mapInitializedCallback = mapInitializedCallback;
		}

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
	}

	/// <summary>
	/// 
	/// </summary>
	public interface IGoogleMapsService : IAsyncDisposable
	{
		string MapContainerId { get; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="apiKey"></param>
		/// <param name="mapContainerId"></param>
		/// <param name="mapInitializedCallback"></param>
		/// <returns></returns>
		Task InitMap(string apiKey, string mapContainerId, Func<string, Task> mapInitializedCallback = null);

		Task SetCenter();
	}

	public sealed class GoogleMapsService : IGoogleMapsService
	{
		private readonly IJSRuntime _jsRuntime;
		private IJSObjectReference _mapsJs;

		public string MapContainerId { get; private set; }

		public GoogleMapsService(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime;
		}

		public async Task InitMap(string apiKey, string mapContainerId, Func<string, Task> mapInitializedCallback)
		{
			if(MapContainerId == mapContainerId)
			{
				return; //Prevent double init...
			}

			MapContainerId = mapContainerId;
			await CheckJsObjectAsync();

			var info = new GoogleMapsEventInfo(mapContainerId, mapInitializedCallback);
			var dotnetRef = DotNetObjectReference.Create<GoogleMapsEventInfo>(info);

			await _mapsJs.InvokeVoidAsync("init", apiKey, mapContainerId, dotnetRef);
		}

		public async Task SetCenter()
		{
			await CheckJsObjectAsync();
			await _mapsJs.InvokeVoidAsync("setCenter", MapContainerId);
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
		}
	}
}