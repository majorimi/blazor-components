using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Extensions.Analytics.Google
{
	/// <summary>
	/// Injectable service to handle Google analytics gtag.js.
	/// </summary>
	public interface IGoogleAnalyticsService : IAsyncDisposable
	{
		/// <summary>
		/// Google analytics uniquely Id which was used in <see cref="Initialize(string)"/> method.
		/// </summary>
		string TrackingId { get; }

		/// <summary>
		/// Initialize Google analytics by registering gtag.js to the HTML document. Should be called once.
		/// </summary>
		/// <param name="trackingId">Is an identifier that uniquely identifies the target for hits, such as a Google Analytics property</param>
		/// <returns>Async ValueTask</returns>
		ValueTask Initialize(string trackingId);

		/// <summary>
		/// Allows you to add additional configuration information to targets. This is typically product-specific configuration for a product
		/// such as Google Ads or Google Analytics.
		/// </summary>
		/// <param name="trackingId">Is an identifier that uniquely identifies the target for hits, such as a Google Analytics property</param>
		/// <param name="configInfo">Is one or more optional parameter-value pairs</param>
		/// <returns>Async ValueTask</returns>
		ValueTask Config(string trackingId = "", Dictionary<string, object> configInfo = null);

		/// <summary>
		/// Allows you to get various values from gtag.js including values set with the set command.
		/// </summary>
		/// <param name="fieldName">The name of the field to get.</param>
		/// <param name="trackingId">Is an identifier that uniquely identifies the target for hits, such as a Google Analytics property</param>
		/// <returns>Async ValueTask</returns>
		ValueTask Get(string fieldName, string trackingId = "");

		/// <summary>
		/// Allows you to set values that persist across all the subsequent gtag() calls on the page.
		/// </summary>
		/// <param name="parameterValuePair">Is a key name and the value that is to persist across gtag() calls.</param>
		/// <returns>Async ValueTask</returns>
		ValueTask Set(Dictionary<string, object> parameterValuePair = null);

		/// <summary>
		/// Use the event command to send event data.
		/// </summary>
		/// <param name="eventName">A recommended event or a custom event name.</param>
		/// <param name="eventParams">Is one or more parameter-value pairs.</param>
		/// <returns>Async ValueTask</returns>
		ValueTask Event(string eventName, Dictionary<string, object> eventParams);
	}

	/// <summary>
	/// Implementation of <see cref="IGoogleAnalyticsService"/>
	/// </summary>
	public class GoogleAnalyticsService : IGoogleAnalyticsService
	{
		private readonly Lazy<Task<IJSObjectReference>> moduleTask;

		public string TrackingId { get; private set; }

		public GoogleAnalyticsService(IJSRuntime jsRuntime)
		{
			string js = "";
#if DEBUG
			js = "./_content/Majorsoft.Blazor.Extensions.Analytics/googleAnalytics.js";
#else
			js = "./_content/Majorsoft.Blazor.Extensions.Analytics/googleAnalytics.min.js";
#endif

			moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>("import", js).AsTask());
		}

		public async ValueTask Initialize(string trackingId)
		{
			if(!string.IsNullOrWhiteSpace(TrackingId) || string.IsNullOrWhiteSpace(trackingId)) //Already initializer or wrong id
			{
				return;
			}

			TrackingId = trackingId;

			var module = await moduleTask.Value;
			await module.InvokeVoidAsync("init", trackingId);
		}

		public async ValueTask Config(string trackingId = "", Dictionary<string, object> configInfo = null)
		{
			var module = await moduleTask.Value;
			await module.InvokeVoidAsync("config", string.IsNullOrWhiteSpace(trackingId) ? TrackingId : trackingId, configInfo?.ToList());
		}
		public async ValueTask Get(string fieldName, string trackingId = "")
		{
			var module = await moduleTask.Value;
			await module.InvokeVoidAsync("get", string.IsNullOrWhiteSpace(trackingId) ? TrackingId : trackingId, fieldName); //TODO: callback results
		}
		public async ValueTask Set(Dictionary<string, object> parameterValuePair = null)
		{
			var module = await moduleTask.Value;
			await module.InvokeVoidAsync("set", parameterValuePair?.ToList());
		}
		public async ValueTask Event(string eventName, Dictionary<string, object> eventParams)
		{
			var module = await moduleTask.Value;
			await module.InvokeVoidAsync("event", eventName, eventParams?.ToList());
		}

		public async ValueTask DisposeAsync()
		{
			if (moduleTask.IsValueCreated)
			{
				var module = await moduleTask.Value;
				await module.DisposeAsync();
			}
		}
	}
}