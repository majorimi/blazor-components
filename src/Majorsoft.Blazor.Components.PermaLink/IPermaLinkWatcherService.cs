using System;

namespace Majorsoft.Blazor.Components.PermaLink
{
	/// <summary>
	/// Injectable service to handle Permalink navigation for the whole application. It is registered as Singleton
	/// and should be injected only once for the whole application. Best way to use MainLayout.razor.
	/// </summary>
	public interface IPermaLinkWatcherService : IDisposable
	{
		/// <summary>
		/// Should scroll smoothly to the found permalink element or jump.
		/// Note: smooth scroll on element level might not supported by all Browsers.
		/// </summary>
		bool SmoothScroll { get; set; }

		/// <summary>
		/// Event handler for parmalinks detected on navigation.
		/// </summary>
		public event EventHandler<PermalinkDetectedEventArgs> PermalinkDetected;

		/// <summary>
		/// Modify the current URL with given new peralink value and trigger navigation or just update browser History.
		/// </summary>
		/// <param name="newPermalink">New peramlink value, NULL will remove permalink part from URL</param>
		/// <param name="doNotNavigate">False, will trigger a navigation. When true, just add new URL to the History</param>
		void ChangePermalink(string? newPermalink, bool doNotNavigate);

		/// <summary>
		/// Checks the current URL for permalink again and re-triggers `PermalinkDetected` event if requested.
		/// </summary>
		/// <param name="triggerEvent">PermalinkDetected should be re-triggered or not</param>
		/// <returns>Found permalink value or NULL</returns>
		string? CheckPermalink(bool triggerEvent = false);

		/// <summary>
		/// Starts a navigation watcher which will check for Permalinks in the URLs.
		/// </summary>
		void WatchPermaLinks();
	}
}