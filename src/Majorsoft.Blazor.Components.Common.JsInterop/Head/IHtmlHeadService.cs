using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Components.Common.JsInterop.Head
{
	/// <summary>
	/// Injectable service to handle HTML Head Interops.
	/// </summary>
	public interface IHtmlHeadService : IAsyncDisposable
	{
		/// <summary>
		/// Returns the current HTML page title.
		/// </summary>
		/// <returns>Async Task</returns>
		Task<string> GetHtmlTitleAsync();

		/// <summary>
		/// Sets the given string value as HTML page title.
		/// </summary>
		/// <param name="title"></param>
		/// <returns>Async Task</returns>
		Task SetHtmlTitleAsync(string title);
	}

	public class HtmlHeadService : IHtmlHeadService
	{
		private readonly IJSRuntime _jsRuntime;
		private IJSObjectReference _headJs;

		public HtmlHeadService(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime;
		}

		public async Task<string> GetHtmlTitleAsync()
		{
			await CheckJsObjectAsync();

			return await _jsRuntime.InvokeAsync<string>("getHtmlTitle");
		}
		public async Task SetHtmlTitleAsync(string title)
		{
			await CheckJsObjectAsync();

			await _jsRuntime.InvokeVoidAsync("setHtmlTitle", title);
		}

		private async Task CheckJsObjectAsync()
		{
			if (_headJs is null)
			{
#if DEBUG
				_headJs = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Majorsoft.Blazor.Components.Common.JsInterop/head.js");
#else
				_headJs = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Majorsoft.Blazor.Components.Common.JsInterop/head.min.js");
#endif
			}
		}

		public async ValueTask DisposeAsync()
		{
			if (_headJs is not null)
			{
				await _headJs.DisposeAsync();

			}
		}
	}
}