namespace Majorsoft.Blazor.Extensions.Analytics.Google
{
	/// <summary>
	/// Data for custom event.
	/// For more details see: https://developers.google.com/analytics/devguides/collection/gtagjs/events
	/// </summary>
	public class GoogleAnalyticsCustomEventArgs
	{
		/// <summary>
		/// The value that will appear as the event action in Google Analytics Event reports.
		/// </summary>
		public string Action { get; set; }

		/// <summary>
		/// The category of the event.
		/// </summary>
		public string Category { get; set; }

		/// <summary>
		/// The label of the event.
		/// </summary>
		public string Label { get; set; }

		/// <summary>
		/// A non-negative integer that will appear as the event value.
		/// </summary>
		public int Value { get; set; }
	}
}