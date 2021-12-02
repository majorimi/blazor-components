using System;

using Microsoft.AspNetCore.Components.Routing;

namespace Majorsoft.Blazor.Components.PermaLink
{
	/// <summary>
	/// Event arguments for <see cref="PermaLinkWatcherService"/> PermalinkDetected event.
	/// </summary>
	public class PermalinkDetectedEventArgs : EventArgs
	{
		/// <summary>
		/// System.EventArgs for Microsoft.AspNetCore.Components.NavigationManager.LocationChanged.
		/// </summary>
		public LocationChangedEventArgs LocationChangedEventArgs { get; }

		/// <summary>
		/// Detected permalink value.
		/// </summary>
		public string Permalink { get; } = "";

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="locationChangedEventArgs">NavigationManager event</param>
		/// <param name="permalink">Detected permalink value</param>
		public PermalinkDetectedEventArgs(LocationChangedEventArgs locationChangedEventArgs, string permalink)
		{
			LocationChangedEventArgs = locationChangedEventArgs;
			Permalink = permalink;
		}
	}
}