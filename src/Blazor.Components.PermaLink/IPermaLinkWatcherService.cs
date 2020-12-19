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
		/// Starts a navigation watcher which will check for Permalinks in the URLs.
		/// </summary>
		void WatchPermaLinks();
	}
}