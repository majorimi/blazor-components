using System.Text.RegularExpressions;

using Blazor.Components.Common.JsInterop.Scroll;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Logging;

namespace Blazor.Components.PermaLink
{
	/// <summary>
	/// Implementation of <see cref="IPermaLinkWatcherService"/>
	/// </summary>
	public class PermaLinkWatcherService : IPermaLinkWatcherService
	{
		private bool _subscribed = false;
		private readonly Regex _poundRegex = new Regex("#(.*)$", RegexOptions.Singleline|RegexOptions.Compiled);
		private readonly IScrollHandler _scrollHandler;
		private readonly NavigationManager _navigationManager;
		private readonly ILogger<IPermaLinkWatcherService> _logger;

		public PermaLinkWatcherService(IScrollHandler scrollHandler, NavigationManager navigationManager, ILogger<IPermaLinkWatcherService> logger)
		{
			_scrollHandler = scrollHandler;
			_navigationManager = navigationManager;
			_logger = logger;
		}

		public void WatchPermaLinks()
		{
			if (!_subscribed)
			{
				_navigationManager.LocationChanged += HandleLocationChanged;
				_subscribed = true;
				HandleLocationChanged(this, new LocationChangedEventArgs(_navigationManager.Uri, false));
			}
		}

		private void HandleLocationChanged(object sender, LocationChangedEventArgs e)
		{
			_logger.LogDebug($"{nameof(PermaLinkWatcherService)} - {nameof(HandleLocationChanged)}: navigation happened new URL: {e.Location}");
			var match = _poundRegex.Match(e.Location);

			if(match.Success && match.Groups.Count == 2)
			{
				var perma = match.Groups[1].Value;
				_logger.LogDebug($"{nameof(PermaLinkWatcherService)} - {nameof(HandleLocationChanged)}: PermaLink found: {perma}");
				_scrollHandler.ScrollToElementByNameAsync(perma);
			}
		}

		public void Dispose()
		{
			_navigationManager.LocationChanged -= HandleLocationChanged;
		}
	}
}