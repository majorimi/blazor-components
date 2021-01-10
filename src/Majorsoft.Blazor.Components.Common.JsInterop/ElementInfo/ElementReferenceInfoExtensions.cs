using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Components.Common.JsInterop.ElementInfo
{
	/// <summary>
	/// Static class for <see cref="ElementReference"/> extension methods.
	/// </summary>
	public static class ElementReferenceInfoExtensions
	{
		/// <summary>
		/// Returns the given HTML element ClintBoundRect data.
		/// </summary>
		/// <param name="elementReference">Blazor reference to an HTML element</param>
		/// <returns>Async Task with <see cref="DomRect"/> value</returns>
		public static async Task<DomRect> GetClientRectAsync(this ElementReference elementReference)
		{
			await using (var module = await elementReference.GetJsObject())
			{
				if (module is not null)
				{
					return await module.InvokeAsync<DomRect>("getBoundingClientRect", elementReference);
				}
			}

			return new DomRect();
		}

		private static async Task<IJSObjectReference?> GetJsObject(this ElementReference elementReference)
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

			return await jsRuntime.InvokeAsync<IJSObjectReference>("import", $"./_content/{jsName}");
		}
	}
}