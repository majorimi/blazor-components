using System;
using System.Text.RegularExpressions;

using Majorsoft.Blazor.Components.Common.JsInterop.Navigation;
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
		private readonly INavigationHistoryService _navigationHistoryService;

		public event EventHandler<PermalinkDetectedEventArgs> PermalinkDetected;

		public bool SmoothScroll { get; set; }

		public PermaLinkWatcherService(IScrollHandler scrollHandler, 
			NavigationManager navigationManager, 
			ILogger<IPermaLinkWatcherService> logger,
			INavigationHistoryService navigationHistoryService,
			bool smoothScroll = false)
		{
			_scrollHandler = scrollHandler;
			_navigationManager = navigationManager;
			_logger = logger;
			_navigationHistoryService = navigationHistoryService;
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
			_logger.LogDebug($"{nameof(PermaLinkWatcherService)} - {nameof(HandleLocationChanged)}: navigation happened new URL: '{e.Location}'");
			var perma = DetectPermalink(e.Location);

			if(!string.IsNullOrWhiteSpace(perma))
			{
				if(PermalinkDetected is not null)
				{
					PermalinkDetected.Invoke(this, new PermalinkDetectedEventArgs(e, perma));
				}

				_scrollHandler.ScrollToElementByNameAsync(perma, SmoothScroll);
			}
		}

		public void ChangePermalink(string? newPermalink, bool doNotNavigate)
		{
			_logger.LogDebug($"{nameof(PermaLinkWatcherService)} - {nameof(ChangePermalink)}: current URL: '{_navigationManager.Uri}', new URL Permalink: '{newPermalink}'");

			var perma = DetectPermalink(_navigationManager.Uri);
			if (!string.IsNullOrWhiteSpace(perma))
			{
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
			_logger.LogDebug($"{nameof(PermaLinkWatcherService)} - {nameof(SetBrowserUrl)}: new URL: '{uri}', doNotNavigate: '{doNotNavigate}'");

			if (doNotNavigate)
			{
				_navigationHistoryService?.PushStateAsync(null, "", uri);
				return;
			}

			_navigationManager.NavigateTo(uri, false);
		}
		public string? CheckPermalink(bool triggerEvent)
		{
			var perma = DetectPermalink(_navigationManager.Uri);
			if (!string.IsNullOrWhiteSpace(perma))
			{
				if (PermalinkDetected is not null)
				{
					PermalinkDetected.Invoke(this, new PermalinkDetectedEventArgs(
						   new LocationChangedEventArgs(_navigationManager.Uri, false), perma));
				}
			}

			return perma;
		}

		private string? DetectPermalink(string uri)
		{
			var match = _poundRegex.Match(uri);
			if (match.Success && match.Groups.Count == 2)
			{
				var perma = match.Groups[1].Value;
				_logger.LogDebug($"{nameof(PermaLinkWatcherService)} - {nameof(DetectPermalink)}: PermaLink found: '{perma}'");

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