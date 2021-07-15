using System;
using System.Dynamic;
using System.Threading.Tasks;

namespace Majorsoft.Blazor.Extensions.Analytics.Google
{
	/// <summary>
	/// Injectable service to handle Google analytics gtag.js.
	/// </summary>
	public interface IGoogleAnalyticsService : IAsyncDisposable
	{
		/// <summary>
		/// Google analytics uniquely Id which was used in <see cref="InitializeAsync(string)"/> method.
		/// </summary>
		string TrackingId { get; }

		/// <summary>
		/// Initialize Google analytics by registering gtag.js to the HTML document. Should be called once.
		/// Do not call this method if you used <see cref="GoogleAnalyticsInitializer"/>.
		/// </summary>
		/// <param name="trackingId">Is an identifier that uniquely identifies the target for hits, such as a Google Analytics property</param>
		/// <returns>Async ValueTask</returns>
		ValueTask InitializeAsync(string trackingId);

		/// <summary>
		/// Allows you to add additional configuration information to targets. This is typically product-specific configuration for a product
		/// such as Google Ads or Google Analytics.
		/// </summary>
		/// <param name="trackingId">Is an identifier that uniquely identifies the target for hits, such as a Google Analytics property</param>
		/// <param name="configInfo">Is one or more optional parameter-value pairs</param>
		/// <returns>Async ValueTask</returns>
		ValueTask ConfigAsync(string trackingId = "", ExpandoObject? configInfo = null);

		/// <summary>
		/// Allows you to get various values from gtag.js including values set with the set command.
		/// </summary>
		/// <param name="fieldName">The name of the field to get.</param>
		/// <param name="callback">A function that will be invoked with the requested field, or undefined if it is unset.</param>
		/// <param name="trackingId">Is an identifier that uniquely identifies the target for hits, such as a Google Analytics property</param>
		/// <returns>Async ValueTask</returns>
		ValueTask GetAsync(string fieldName, Func<object, Task> callback, string trackingId = "");

		/// <summary>
		/// Allows you to set values that persist across all the subsequent gtag() calls on the page.
		/// </summary>
		/// <param name="parameters">Is a key name and the value that is to persist across gtag() calls.</param>
		/// <returns>Async ValueTask</returns>
		ValueTask SetAsync(ExpandoObject parameters);

		/// <summary>
		/// Use the event command to send event data.
		/// </summary>
		/// <param name="eventType">A recommended event</param>
		/// <param name="eventParams">Is one or more parameter-value pairs.</param>
		/// <returns>Async ValueTask</returns>
		ValueTask EventAsync(GoogleAnalyticsEventTypes eventType, ExpandoObject eventParams);

		/// <summary>
		/// Use the event command to send custom event data.
		/// </summary>
		/// <param name="customEventName">Custom event name.</param>
		/// <param name="eventData">Custom event data</param>
		/// <returns>Async ValueTask</returns>
		ValueTask CustomEventAsync(string customEventName, GoogleAnalyticsCustomEventArgs eventData);
	}
}