using System;
using System.Text.RegularExpressions;

using Majorsoft.Blazor.Components.Common.JsInterop.Scroll;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Logging;

namespace Majorsoft.Blazor.Components.PermaLink
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

		public event EventHandler<PermalinkDetectedEventArgs> PermalinkDetected;

		public bool SmoothScroll { get; set; }

		public PermaLinkWatcherService(IScrollHandler scrollHandler, NavigationManager navigationManager, ILogger<IPermaLinkWatcherService> logger, bool smoothScroll = false)
		{
			_scrollHandler = scrollHandler;
			_navigationManager = navigationManager;
			_logger = logger;

			SmoothScroll = smoothScroll;
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
			var perma = DetectPermalink(e.Location);

			if(!string.IsNullOrWhiteSpace(perma))
			{
				_logger.LogDebug($"{nameof(PermaLinkWatcherService)} - {nameof(HandleLocationChanged)}: PermaLink found: {perma}");

				if(PermalinkDetected is not null)
				{
					PermalinkDetected.Invoke(this, new PermalinkDetectedEventArgs(e, perma));
				}	

				_scrollHandler.ScrollToElementByNameAsync(perma, SmoothScroll);
			}
		}

		public void ChangePermalink(string? newPermalink, bool doNotNavigate)
		{
			var perma = DetectPermalink(_navigationManager.Uri);
			if (!string.IsNullOrWhiteSpace(perma))
			{
				if (newPermalink?.ToLower() == perma?.ToLower()) //same
				{
					return;
				}

				var newUri = _navigationManager.Uri.Replace($"#{perma}", "");
				if (string.IsNullOrWhiteSpace(newPermalink)) //remove
				{
					_navigationManager.NavigateTo(newUri);
					return;
				}

				SetBrowserUrl($"{newUri}#{newPermalink}", doNotNavigate);
			}
			else
			{
				SetBrowserUrl($"{_navigationManager.Uri}#{newPermalink}", doNotNavigate);
			}
		}

		private void SetBrowserUrl(string uri, bool doNotNavigate)
		{
			if(doNotNavigate)
			{
				//TODO: history.pushState(null, '', url);
				return;
			}

			_navigationManager.NavigateTo(uri, false);
		}

		private string DetectPermalink(string uri)
		{
			var match = _poundRegex.Match(uri);
			if (match.Success && match.Groups.Count == 2)
			{
				var perma = match.Groups[1].Value;
				return perma;
			}

			return null;
		}

		public void Dispose()
		{
			_navigationManager.LocationChanged -= HandleLocationChanged;
		}
	}
}