using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Components.Common.JsInterop.BrowserColorTheme
{
	/// <summary>
	/// Implementation of <see cref="IBrowserThemeService"/>
	/// </summary>
	public class BrowserThemeService : IBrowserThemeService
	{
		private List<DotNetObjectReference<BrowserThemeEventInfo>> _dotNetObjectReferences;
		private readonly Lazy<Task<IJSObjectReference>> _moduleTask;
		private readonly IJSRuntime _jsRuntime;

		public BrowserThemeService(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime;

			string js = "";
#if DEBUG
			js = "./_content/Majorsoft.Blazor.Components.Common.JsInterop/colorTheme.js";
#else
			js = "./_content/Majorsoft.Blazor.Components.Common.JsInterop/colorTheme.min.js";
#endif

			_moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>("import", js).AsTask());
			_dotNetObjectReferences = new List<DotNetObjectReference<BrowserThemeEventInfo>>();
		}

		public async Task<BrowserColorThemes> GetBrowserColorThemeAsync()
		{
			var module = await _moduleTask.Value;
			var ret = await module.InvokeAsync<int>("getColorTheme");

			return ret == 0 ? BrowserColorThemes.Dark : BrowserColorThemes.Light;
		}

		public async Task<string> RegisterColorThemeChangeAsync(Func<BrowserColorThemes, Task> colorThemeChangeCallback)
		{
			var module = await _moduleTask.Value;

			var id = Guid.NewGuid().ToString();
			var info = new BrowserThemeEventInfo(colorThemeChangeCallback, id);
			var dotnetRef = DotNetObjectReference.Create<BrowserThemeEventInfo>(info);
			_dotNetObjectReferences.Add(dotnetRef);

			await module.InvokeVoidAsync("addColorThemeEvent", dotnetRef, id);
			return id;
		}

		public async Task RemoveColorThemeChangeAsync(string eventId)
		{
			var module = await _moduleTask.Value;

			await module.InvokeVoidAsync("removeColorThemeEvent", eventId);
			RemoveElement(eventId);
		}

		private void RemoveElement(string eventId)
		{
			var dotNetRefs = _dotNetObjectReferences.Where(x => x.Value.EventId == eventId);
			_dotNetObjectReferences = _dotNetObjectReferences.Except(dotNetRefs).ToList();

			foreach (var item in dotNetRefs)
			{
				item.Dispose();
			}
		}

		public async ValueTask DisposeAsync()
		{
			if (_moduleTask.IsValueCreated)
			{
				var module = await _moduleTask.Value;
				await module.InvokeVoidAsync("dispose", (object)_dotNetObjectReferences.Select(s => s.Value.EventId).ToArray());
				await module.DisposeAsync();
			}

			foreach (var item in _dotNetObjectReferences)
			{
				item.Dispose();
			}
		}
	}
}