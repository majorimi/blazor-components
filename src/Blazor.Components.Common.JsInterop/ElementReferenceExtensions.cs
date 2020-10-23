using System;
using System.Reflection;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor.Components.Common.JsInterop
{
	public static class ElementReferenceExtensions
	{
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