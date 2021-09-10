using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Components.Common.JsInterop.Scroll
{
	/// <summary>
	/// Extensions for <see cref="ElementReference"/> HTML elements.
	/// </summary>
	public static class ElementReferenceScrollExtensions
	{
		/// <summary>
		/// Scrolls HTML page to given element.
		/// </summary>
		/// <param name="elementReference">Blazor reference to an HTML element</param>
		/// <param name="smooth">Scroll should jump or smoothly scroll Note: might not all browsers support it</param>
		/// <returns>Async Task</returns>
		public static async Task ScrollToElementAsync(this ElementReference elementReference, bool smooth = false)
		{
			await using (var module = await elementReference.GetJsObject())
			{
				if (module is not null)
				{
					await module.InvokeVoidAsync("scrollToElement", elementReference, smooth);
				}
			}
		}

		/// <summary>
		/// Scrolls inside the given element to the bottom (end).
		/// </summary>
		/// <param name="elementReference">Blazor reference to an HTML element</param>
		/// <param name="smooth">Scroll should jump or smoothly scroll Note: might not all browsers support it</param>
		/// <returns>Async Task</returns>
		public static async Task ScrollToEndAsync(this ElementReference elementReference, bool smooth = false)
		{
			await using (var module = await elementReference.GetJsObject())
			{
				if (module is not null)
				{
					await module.InvokeVoidAsync("scrollToEnd", elementReference, smooth);
				}
			}
		}

		/// <summary>
		/// Scrolls inside the given element to the beginning (top).
		/// </summary>
		/// <param name="elementReference">Blazor reference to an HTML element</param>
		/// <param name="smooth">Scroll should jump or smoothly scroll Note: might not all browsers support it</param>
		/// <returns>Async Task</returns>
		public static async Task ScrollToTopAsync(this ElementReference elementReference, bool smooth = false)
		{
			await using (var module = await elementReference.GetJsObject())
			{
				if (module is not null)
				{
					await module.InvokeVoidAsync("scrollToTop", elementReference, smooth);
				}
			}
		}

		/// <summary>
		/// Scrolls inside the given element to the given X position.
		/// </summary>
		/// <param name="elementReference">Blazor reference to an HTML element</param>
		/// <param name="xPos">Scroll X position</param>
		/// <param name="smooth">Scroll should jump or smoothly scroll Note: might not all browsers support it</param>
		/// <returns>Async Task</returns>
		public static async Task ScrollToXAsync(this ElementReference elementReference, double xPos, bool smooth = false)
		{
			await using (var module = await elementReference.GetJsObject())
			{
				if (module is not null)
				{
					await module.InvokeVoidAsync("scrollToX", elementReference, xPos, smooth);
				}
			}
		}

		/// <summary>
		/// Scrolls inside the given element to the given Y position.
		/// </summary>
		/// <param name="elementReference">Blazor reference to an HTML element</param>
		/// <param name="yPos">Scroll Y position</param>
		/// <param name="smooth">Scroll should jump or smoothly scroll Note: might not all browsers support it</param>
		/// <returns>Async Task</returns>
		public static async Task ScrollToYAsync(this ElementReference elementReference, double yPos, bool smooth = false)
		{
			await using (var module = await elementReference.GetJsObject())
			{
				if (module is not null)
				{
					await module.InvokeVoidAsync("scrollToY", elementReference, yPos, smooth);
				}
			}
		}

		/// <summary>
		/// Scrolls inside the given element to the given X and Y positions.
		/// </summary>
		/// <param name="elementReference">Blazor reference to an HTML element</param>
		/// <param name="xPos">Scroll X position</param>
		/// <param name="yPos">Scroll Y position</param>
		/// <param name="smooth">Scroll should jump or smoothly scroll Note: might not all browsers support it</param>
		/// <returns>Async Task</returns>
		public static async Task ScrollToAsync(this ElementReference elementReference, double xPos, double yPos, bool smooth = false)
		{
			await using (var module = await elementReference.GetJsObject())
			{
				if (module is not null)
				{
					await module.InvokeVoidAsync("scrollTo", elementReference, xPos, yPos, smooth);
				}
			}
		}

		/// <summary>
		/// Returns given element scroll X (left) position.
		/// </summary>
		/// <param name="elementReference">Blazor reference to an HTML element</param>
		/// <returns>Async Task with X pos</returns>
		public static async Task<double> GetScrollXPositionAsync(this ElementReference elementReference)
		{
			await using (var module = await elementReference.GetJsObject())
			{
				if (module is not null)
				{
					return await module.InvokeAsync<double>("getScrollXPosition", elementReference);
				}
			}

			return 0;
		}
		/// <summary>
		/// Returns given element scroll Y (top) position.
		/// </summary>
		/// <param name="elementReference">Blazor reference to an HTML element</param>
		/// <returns>Async Task with Y pos</returns>
		public static async Task<double> GetScrollYPositionAsync(this ElementReference elementReference)
		{
			await using (var module = await elementReference.GetJsObject())
			{
				if (module is not null)
				{
					return await module.InvokeAsync<double>("getScrollYPosition", elementReference);
				}
			}

			return 0;
		}

		/// <summary>
		/// Returns given element is visible on HTML document or not.
		/// </summary>
		/// <param name="elementReference">Blazor reference to an HTML element</param>
		/// <returns>Async Task</returns>
		public static async Task<bool> IsElementHiddenAsync(this ElementReference elementReference)
		{
			await using (var module = await elementReference.GetJsObject())
			{
				if (module is not null)
				{
					return await module.InvokeAsync<bool>("isElementHidden", elementReference);
				}
			}

			return false;
		}
		/// <summary>
		/// Returns given element is below of the view port.
		/// </summary>
		/// <param name="elementReference">Blazor reference to an HTML element</param>
		/// <returns>Async Task</returns>
		public static async Task<bool> IsElementHiddenBelowAsync(this ElementReference elementReference)
		{
			await using (var module = await elementReference.GetJsObject())
			{
				if (module is not null)
				{
					return await module.InvokeAsync<bool>("isElementHiddenBelow", elementReference);
				}
			}

			return false;
		}
		/// <summary>
		/// Returns given element is above of the view port.
		/// </summary>
		/// <param name="elementReference">Blazor reference to an HTML element</param>
		/// <returns>Async Task</returns>
		public static async Task<bool> IsElementHiddenAboveAsync(this ElementReference elementReference)
		{
			await using (var module = await elementReference.GetJsObject())
			{
				if (module is not null)
				{
					return await module.InvokeAsync<bool>("isElementHiddenAbove", elementReference);
				}
			}

			return false;
		}

		/// <summary>
		/// Scrolls inside the given parent element to the given inner element.
		/// </summary>
		/// <param name="parent">Blazor reference to an HTML (outer/wrapper) element</param>
		/// <param name="innerElement">Blazor reference to an inner HTML element to scroll to</param>
		/// <returns>Async Task</returns>
		public static async Task ScrollToElementInParentAsync(this ElementReference parent, ElementReference innerElement)
		{
			await using (var module = await parent.GetJsObject())
			{
				if (module is not null)
				{
					await module.InvokeVoidAsync("scrollToElementInParent", parent, innerElement);
				}
			}
		}

		/// <summary>
		/// Scrolls inside the given parent element to the given inner element by Id.
		/// </summary>
		/// <param name="parent">Blazor reference to an HTML (outer/wrapper) element</param>
		/// <param name="id">Inner element Id to scroll to</param>
		/// <returns>Async Task</returns>
		public static async Task ScrollInParentByIdAsync(this ElementReference parent, string id)
		{
			await using (var module = await parent.GetJsObject())
			{
				if (module is not null)
				{
					await module.InvokeVoidAsync("scrollInParentById", parent, id);
				}
			}
		}

		/// <summary>
		/// Scrolls inside the given parent element to the given first found inner element by class name.
		/// </summary>
		/// <param name="parent">Blazor reference to an HTML (outer/wrapper) element</param>
		/// <param name="className">Inner element CSS class to scroll to</param>
		/// <returns>Async Task</returns>
		public static async Task ScrollInParentByClassAsync(this ElementReference parent, string className)
		{
			await using (var module = await parent.GetJsObject())
			{
				if (module is not null)
				{
					await module.InvokeVoidAsync("scrollInParentByClass", parent, className);
				}
			}
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