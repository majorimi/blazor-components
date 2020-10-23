using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Blazor.Components.Common.JsInterop.Click
{
	/// <summary>
	/// Implementation of <see cref="IClickBoundariesHandler"/>
	/// </summary>
	public sealed class ClickBoundariesHandler : IClickBoundariesHandler
	{
		private readonly IJSRuntime _jsRuntime;
		private List<ElementReference> _registeredElements;
		private IJSObjectReference _transitionJs;

		public ClickBoundariesHandler(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime;
			_registeredElements = new List<ElementReference>();
		}

		public async Task RegisterClickBoundariesAsync(ElementReference elementRef, 
			Func<MouseEventArgs, Task> outsideClickCallback = null, 
			Func<MouseEventArgs, Task> insideClickCallback = null)
		{
			await CheckJsObjectAsync();

			var info = new ClickBoundariesEventInfo(outsideClickCallback, insideClickCallback);
			var dotnetRef = DotNetObjectReference.Create<ClickBoundariesEventInfo>(info);

			await _transitionJs.InvokeVoidAsync("addClickBoundariesHandler", elementRef, dotnetRef);
			_registeredElements.Add(elementRef);
		}

		public async Task RemoveClickBoundariesAsync(ElementReference elementRef)
		{
			await CheckJsObjectAsync();

			await _transitionJs.InvokeVoidAsync("removeClickBoundariesHandler", elementRef);
			RemoveElement(elementRef);
		}

		private void RemoveElement(ElementReference elementRef)
		{
			var items = _registeredElements.Where(x => x.Equals(elementRef));

			_registeredElements = _registeredElements.Except(items).ToList();
		}

		private async Task CheckJsObjectAsync()
		{
			if (_transitionJs is null)
			{
#if DEBUG
				_transitionJs = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Majorsoft.Blazor.Components.Common.JsInterop/click.js");
#else
				_transitionJs = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Majorsoft.Blazor.Components.Common.JsInterop/click.min.js");
#endif
			}
		}

		public async ValueTask DisposeAsync()
		{
			if (_transitionJs is not null)
			{
				await _transitionJs.InvokeVoidAsync("dispose", _registeredElements.ToArray());

				await _transitionJs.DisposeAsync();
			}
		}
	}
}