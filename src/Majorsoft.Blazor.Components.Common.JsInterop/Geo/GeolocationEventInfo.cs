using Microsoft.JSInterop;

using System;
using System.Threading.Tasks;

namespace Majorsoft.Blazor.Components.Common.JsInterop.Geo
{
	/// <summary>
	/// Base class for Geolocation event <see cref="DotNetObjectReference"/> info to handle JS callback
	/// </summary>
	internal abstract class GeolocationEventInfo
	{
		private readonly Func<GeolocationResult, Task> _locationResultCallback;

		public GeolocationEventInfo(Func<GeolocationResult, Task> locationResultCallback)
		{
			_locationResultCallback = locationResultCallback;
		}

		[JSInvokable("GeolocationEvent")]
		public virtual async Task GeolocationEvent(GeolocationResult args)
		{
			await _locationResultCallback(args);
		}
	}
}