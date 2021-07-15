using System;
using System.Threading.Tasks;

using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Components.Common.JsInterop.BrowserColorTheme
{
	/// <summary>
	/// Browser color scheme queries response event <see cref="DotNetObjectReference"/> info to handle JS callback
	/// </summary>
	internal sealed class BrowserThemeEventInfo
	{
		private readonly Func<BrowserColorThemes, Task> _browserThemeChangedEventCallback;
		internal string EventId { get; }

		public BrowserThemeEventInfo(Func<BrowserColorThemes, Task> browserThemeChangedEventCallback, string eventId)
		{
			_browserThemeChangedEventCallback = browserThemeChangedEventCallback;
			EventId = eventId;
		}

		[JSInvokable("BrowserThemeChanged")]
		public async Task TransitionEvent(int colorTheme)
		{
			await _browserThemeChangedEventCallback((BrowserColorThemes)colorTheme);
		}
	}
}