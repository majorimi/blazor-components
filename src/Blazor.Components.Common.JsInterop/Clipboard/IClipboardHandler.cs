using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor.Components.Common.JsInterop.Clipboard
{
	/// <summary>
	/// Injectable service to handle JS 'copy' clipboard interops.
	/// </summary>
	public interface IClipboardHandler : IAsyncDisposable
	{
		/// <summary>
		/// Copy the given element text content to clipboard.
		/// </summary>
		/// <param name="elementReference">ElementReference to get text</param>
		/// <returns>Async Task</returns>
		Task<bool> CopyElementTextToClipboardAsync(ElementReference elementReference);

		/// <summary>
		/// Copy the given text content to clipboard.
		/// </summary>
		/// <param name="elementReference">Text to copy</param>
		/// <returns>Async Task</returns>
		Task<bool> CopyTextToClipboardAsync(string text);
	}

	/// <summary>
	/// Implementation of <see cref="IClipboardHandler"/>
	/// </summary>
	public class ClipboardHandler : IClipboardHandler
	{
		private readonly IJSRuntime _jsRuntime;
		private IJSObjectReference _clipboardJs;

		public ClipboardHandler(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime;
		}

		public async Task<bool> CopyElementTextToClipboardAsync(ElementReference elementReference)
		{
			await CheckJsObjectAsync();
			return await _clipboardJs.InvokeAsync<bool>("copyElementTextToClipboard", elementReference);
		}

		public async Task<bool> CopyTextToClipboardAsync(string text)
		{
			await CheckJsObjectAsync();
			return await _clipboardJs.InvokeAsync<bool>("copyTextToClipboard", text);
		}

		private async Task CheckJsObjectAsync()
		{
			if (_clipboardJs is null)
			{
#if DEBUG
				_clipboardJs = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Majorsoft.Blazor.Components.Common.JsInterop/clipboard.js");
#else
				_clipboardJs = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Majorsoft.Blazor.Components.Common.JsInterop/clipboard.min.js");
#endif
			}
		}

		public async ValueTask DisposeAsync()
		{
			if (_clipboardJs is not null)
			{
				await _clipboardJs.DisposeAsync();
			}
		}
	}
}