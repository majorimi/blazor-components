using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Extensions.Analytics.Google
{
	/// <summary>
	/// Implementation of <see cref="IGoogleAnalyticsService"/>
	/// </summary>
	public class GoogleAnalyticsService : IGoogleAnalyticsService
	{
		private List<DotNetObjectReference<GoogleAnalyticsGetEventInfo>> _dotNetObjectReferences;
		private readonly Lazy<Task<IJSObjectReference>> moduleTask;
		private static string _trackingId = ""; //Service cannot registered as Singleton.

		public string TrackingId => _trackingId;

		public GoogleAnalyticsService(IJSRuntime jsRuntime)
		{
			string js = "";
#if DEBUG
			js = "./_content/Majorsoft.Blazor.Extensions.Analytics/googleAnalytics.js";
#else
			js = "./_content/Majorsoft.Blazor.Extensions.Analytics/googleAnalytics.min.js";
#endif

			moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>("import", js).AsTask());
			_dotNetObjectReferences = new List<DotNetObjectReference<GoogleAnalyticsGetEventInfo>>();
		}

		public async ValueTask InitializeAsync(string trackingId)
		{
			if(!string.IsNullOrWhiteSpace(TrackingId) || string.IsNullOrWhiteSpace(trackingId)) //Already initializer or wrong id
			{
				return;
			}

			_trackingId = trackingId;

			var module = await moduleTask.Value;
			await module.InvokeVoidAsync("init", trackingId);
		}

		public async ValueTask ConfigAsync(string trackingId = "", ExpandoObject? configInfo = null)
		{
			var module = await moduleTask.Value;
			await module.InvokeVoidAsync("config", string.IsNullOrWhiteSpace(trackingId) ? TrackingId : trackingId, configInfo?.ToList());
		}
		public async ValueTask GetAsync(string fieldName, Func<object, Task> callback, string trackingId = "")
		{
			var module = await moduleTask.Value;

			var info = new GoogleAnalyticsGetEventInfo(callback);
			var dotnetRef = DotNetObjectReference.Create<GoogleAnalyticsGetEventInfo>(info);

			_dotNetObjectReferences.Add(dotnetRef);

			await module.InvokeVoidAsync("get", string.IsNullOrWhiteSpace(trackingId) ? TrackingId : trackingId, fieldName, dotnetRef);
		}
		public async ValueTask SetAsync(ExpandoObject parameters)
		{
			var module = await moduleTask.Value;
			await module.InvokeVoidAsync("set", parameters);
		}
		public async ValueTask EventAsync(GoogleAnalyticsEventTypes eventType, ExpandoObject eventParams)
		{
			var module = await moduleTask.Value;
			await module.InvokeVoidAsync("event", eventType.ToString(), eventParams);
		}

		public async ValueTask CustomEventAsync(string customEventName, GoogleAnalyticsCustomEventArgs eventData)
		{
			var module = await moduleTask.Value;
			await module.InvokeVoidAsync("customEvent", customEventName, eventData);
		}

		public async ValueTask DisposeAsync()
		{
			if (moduleTask.IsValueCreated)
			{
				var module = await moduleTask.Value;
				await module.DisposeAsync();
			}

			foreach (var item in _dotNetObjectReferences)
			{
				item.Dispose();
			}
		}
	}
}