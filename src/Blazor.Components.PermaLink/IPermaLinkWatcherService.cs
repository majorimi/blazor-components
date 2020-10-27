using System;

namespace Blazor.Components.PermaLink
{
	/// <summary>
	/// Injectable service to handle Permalink navigation for the whole document.
	/// </summary>
	public interface IPermaLinkWatcherService : IDisposable
	{
		void WatchPermaLinks();
	}
}