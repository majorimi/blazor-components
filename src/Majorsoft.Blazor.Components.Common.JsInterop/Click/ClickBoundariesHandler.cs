using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Components.Common.JsInterop.Click
{
	/// <summary>
	/// Implementation of <see cref="IClickBoundariesHandler"/>
	/// </summary>
	public sealed class ClickBoundariesHandler : IClickBoundariesHandler
	{
		private readonly IJSRuntime _jsRuntime;
		private IJSObjectReference _clickJs;
		private List<DotNetObjectReference<ClickBoundariesEventInfo>> _dotNetObjectReferences;

		public ClickBoundariesHandler(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime;
			_dotNetObjectReferences = new List<DotNetObjectReference<ClickBoundariesEventInfo>>();
		}

		public async Task RegisterClickBoundariesAsync(ElementReference elementRef, 
			Func<MouseEventArgs, Task> outsideClickCallback = null, 
			Func<MouseEventArgs, Task> insideClickCallback = null)
		{
			await CheckJsObjectAsync();

			var info = new ClickBoundariesEventInfo(elementRef, outsideClickCallback, insideClickCallback);
			var dotnetRef = DotNetObjectReference.Create<ClickBoundariesEventInfo>(info);
			_dotNetObjectReferences.Add(dotnetRef);

			await _clickJs.InvokeVoidAsync("addClickBoundariesHandler", elementRef, dotnetRef);
		}

		public async Task RemoveClickBoundariesAsync(ElementReference elementRef)
		{
			await CheckJsObjectAsync();

			await _clickJs.InvokeVoidAsync("removeClickBoundariesHandler", elementRef);
			RemoveElement(elementRef);
		}

		private void RemoveElement(ElementReference elementRef)
		{
			var dotNetRefs = _dotNetObjectReferences.Where(x => x.Value.ElementRef.Equals(elementRef));
			_dotNetObjectReferences = _dotNetObjectReferences.Except(dotNetRefs).ToList();

			foreach (var item in dotNetRefs)
			{
				item.Dispose();
			}
		}

		private async Task CheckJsObjectAsync()
		{
			if (_clickJs is null)
			{
#if DEBUG
				_clickJs = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Majorsoft.Blazor.Components.Common.JsInterop/click.js");
#else
				_clickJs = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Majorsoft.Blazor.Components.Common.JsInterop/click.min.js");
#endif
			}
		}

		public async ValueTask DisposeAsync()
		{
			if (_clickJs is not null)
			{
				await _clickJs.InvokeVoidAsync("dispose", (object)_dotNetObjectReferences.Select(s => s.Value.ElementRef).ToArray());

				await _clickJs.DisposeAsync();
			}

			foreach (var item in _dotNetObjectReferences)
			{
				item.Dispose();
			}
		}
	}
}