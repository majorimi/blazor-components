using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor.Components.Common.JsInterop.Scroll
{
	public static class ElementReferenceScrollExtensions
	{
		public static async Task ScrollToElementAsync(this ElementReference elementReference)
		{
			await using (var module = await elementReference.GetJsObject())
			{
				if (module is not null)
				{
					await module.InvokeVoidAsync("scrollToElement", elementReference);
				}
			}
		}

		public static async Task ScrollToEndAsync(this ElementReference elementReference)
		{
			await using (var module = await elementReference.GetJsObject())
			{
				if (module is not null)
				{
					await module.InvokeVoidAsync("scrollToEnd", elementReference);
				}
			}
		}

		public static async Task ScrollToTopAsync(this ElementReference elementReference)
		{
			await using (var module = await elementReference.GetJsObject())
			{
				if (module is not null)
				{
					await module.InvokeVoidAsync("scrollToTop", elementReference);
				}
			}
		}

		public static async Task ScrollToAsync(this ElementReference elementReference, double xPos)
		{
			await using (var module = await elementReference.GetJsObject())
			{
				if (module is not null)
				{
					await module.InvokeVoidAsync("scrollTo", elementReference, xPos);
				}
			}
		}

		public static async Task<double> GetScrollPositionAsync(this ElementReference elementReference)
		{
			await using (var module = await elementReference.GetJsObject())
			{
				if (module is not null)
				{
					return await module.InvokeAsync<double>("getScrollPosition", elementReference);
				}
			}

			return 0;
		}

		private static async Task<IJSObjectReference?> GetJsObject(this ElementReference elementReference)
		{
			var jsRuntime = elementReference.GetJSRuntime();

			if (jsRuntime is null)
			{
				throw new InvalidOperationException("No JavaScript runtime found.");
			}

#if DEBUG
			var jsName = "Majorsoft.Blazor.Components.Common.JsInterop/scroll.js";
#else
			var jsName = "Majorsoft.Blazor.Components.Common.JsInterop/scroll.min.js";
#endif

			return await jsRuntime.InvokeAsync<IJSObjectReference>("import", $"./_content/{jsName}");
		}
	}
}