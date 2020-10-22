using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor.Components.Common.JsInterop.ElementInfo
{
	public static class ElementReferenceExtensions
	{
		[Inject]
		public static IJSRuntime JsRuntime { get; set; }

		public static async Task<DomRect> GetClientRectAsync(this ElementReference elementReference)
		{
			if (JsRuntime is not null)
			{

#if DEBUG
				var jsName = "Majorsoft.Blazor.Components.Common.JsInterop/elementInfo.js";
#else
				var jsName = "Majorsoft.Blazor.Components.Common.JsInterop/elementInfo.min.js";
#endif
				await using (var module = await JsRuntime.InvokeAsync<IJSObjectReference>("import", $"./_content/{jsName}"))
				{
					return await module.InvokeAsync<DomRect>("getBoundingClientRect", elementReference);
				}
			}

			return new DomRect();
		}
	}
}