using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

namespace Majorsoft.Blazor.Components.Common.JsInterop.Clipboard
{
	/// <summary>
	/// Injectable service to handle JS 'copy' to clipboard Interops.
	/// </summary>
	public interface IClipboardHandler : IAsyncDisposable
	{
		/// <summary>
		/// Copies the given element text content to clipboard.
		/// </summary>
		/// <param name="elementReference">ElementReference to get text</param>
		/// <returns>Async Task</returns>
		Task<bool> CopyElementTextToClipboardAsync(ElementReference elementReference);

		/// <summary>
		/// Copies the given text content to clipboard.
		/// </summary>
		/// <param name="elementReference">Text to copy</param>
		/// <returns>Async Task</returns>
		Task<bool> CopyTextToClipboardAsync(string text);
	}
}