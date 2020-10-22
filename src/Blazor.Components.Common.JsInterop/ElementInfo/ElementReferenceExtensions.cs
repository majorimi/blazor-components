using System;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor.Components.Common.JsInterop.ElementInfo
{
	public static class ElementReferenceExtensions
	{
		public static async Task<DomRect> GetClientRectAsync(this ElementReference elementReference)
		{
			var jsRuntime = elementReference.GetJSRuntime();

			if (jsRuntime is null)
			{
				throw new InvalidOperationException("No JavaScript runtime found.");
			}

#if DEBUG
			var jsName = "Majorsoft.Blazor.Components.Common.JsInterop/elementInfo.js";
#else
			var jsName = "Majorsoft.Blazor.Components.Common.JsInterop/elementInfo.min.js";
#endif
			await using (var module = await jsRuntime.InvokeAsync<IJSObjectReference>("import", $"./_content/{jsName}"))
			{
				return await module.InvokeAsync<DomRect>("getBoundingClientRect", elementReference);
			}
		}

		public static IJSRuntime GetJSRuntime(this ElementReference elementReference)
		{
			if (!(elementReference.Context is WebElementReferenceContext context))
			{
				throw new InvalidOperationException("ElementReference has not been configured correctly.");
			}

			var prop = context.GetType().GetProperty("JSRuntime", BindingFlags.Instance|BindingFlags.NonPublic);
			return prop?.GetValue(context) as IJSRuntime;
		}
	}
}