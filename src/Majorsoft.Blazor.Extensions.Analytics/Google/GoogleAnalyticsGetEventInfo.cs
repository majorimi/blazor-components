using System;
using System.Threading.Tasks;

using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Extensions.Analytics.Google
{
	/// <summary>
	/// Google analytics Get response event <see cref="DotNetObjectReference"/> info to handle JS callback
	/// </summary>
	internal sealed class GoogleAnalyticsGetEventInfo
	{
		private readonly Func<object, Task> _googleAnalyticsGetEventCallback;

		public GoogleAnalyticsGetEventInfo(Func<object, Task> googleAnalyticsGetEventCallback)
		{
			_googleAnalyticsGetEventCallback = googleAnalyticsGetEventCallback;
		}

		[JSInvokable("GoogleAnalyticsResult")]
		public async Task TransitionEvent(object args)
		{
			await _googleAnalyticsGetEventCallback(args);
		}
	}
}