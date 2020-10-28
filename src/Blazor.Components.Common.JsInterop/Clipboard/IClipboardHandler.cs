using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

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
}