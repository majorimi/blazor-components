using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor.Components.Common.JsInterop.Clipboard
{
	public static class ClipboardExtensions
	{
		public static async Task<bool> CopyElementTextToClipboardAsync(this ElementReference elementReference)
		{
			await using (var module = await elementReference.GetJsObject())
			{
				if (module is not null)
				{
					return await module.InvokeAsync<bool>("copyElementTextToClipboard", elementReference);
				}
			}

			return false;
		}

		private static async Task<IJSObjectReference?> GetJsObject(this ElementReference elementReference)
		{
			var jsRuntime = elementReference.GetJSRuntime();

			if (jsRuntime is null)
			{
				throw new InvalidOperationException("No JavaScript runtime found.");
			}

#if DEBUG
			var jsName = "Majorsoft.Blazor.Components.Common.JsInterop/clipboard.js";
#else
			var jsName = "Majorsoft.Blazor.Components.Common.JsInterop/clipboard.min.js";
#endif

			return await jsRuntime.InvokeAsync<IJSObjectReference>("import", $"./_content/{jsName}");
		}
	}
}