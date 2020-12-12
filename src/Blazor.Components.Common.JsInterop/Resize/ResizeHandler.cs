using Blazor.Components.Common.JsInterop.GlobalMouseEvents;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.Components.Common.JsInterop.Resize
{
	/// <summary>
	/// Implementation of <see cref="IResizeHandler"/>
	/// </summary>
	public class ResizeHandler : IResizeHandler
	{
		private readonly IJSRuntime _jsRuntime;
		private List<ElementReference> _registeredElements;
		private List<string> _registeredEvents;
		private IJSObjectReference _resizeJs;

		public ResizeHandler(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime;
			_registeredElements = new List<ElementReference>();
			_registeredEvents = new List<string>();
		}

		public async Task<string> RegisterPageResizeAsync(Func<ResizeEventArgs, Task> resizeCallback)
		{
			await CheckJsObjectAsync();

			var id = Guid.NewGuid().ToString();
			var info = new ResizeEventInfo(resizeCallback, id);
			var dotnetRef = DotNetObjectReference.Create<ResizeEventInfo>(info);

			await _resizeJs.InvokeVoidAsync("addGlobalResizeEvent", dotnetRef, id);
			_registeredEvents.Add(id);

			return id;
		}

		public async Task RemovePageResizeAsync(string eventId)
		{
			await CheckJsObjectAsync();

			await _resizeJs.InvokeVoidAsync("removeGlobalResizeEvent", eventId);
			RemoveEvent(eventId);
		}

		public async Task RegisterResizeAsync(ElementReference elementRef, Func<ResizeEventArgs, Task> resizeCallback = null)
		{
			await CheckJsObjectAsync();

			var info = new ResizeEventInfo(resizeCallback, null);
			var dotnetRef = DotNetObjectReference.Create<ResizeEventInfo>(info);

			await _resizeJs.InvokeVoidAsync("addResizeEventHandler", elementRef, dotnetRef);
			_registeredElements.Add(elementRef);
		}

		public async Task RemoveResizeAsync(ElementReference elementRef)
		{
			await CheckJsObjectAsync();

			await _resizeJs.InvokeVoidAsync("removeResizeEventHandler", elementRef);
			RemoveElement(elementRef);
		}

		private void RemoveEvent(string eventId)
		{
			var items = _registeredEvents.Where(x => x.Equals(eventId));
			_registeredEvents = _registeredEvents.Except(items).ToList();
		}
		private void RemoveElement(ElementReference elementRef)
		{
			var items = _registeredElements.Where(x => x.Equals(elementRef));

			_registeredElements = _registeredElements.Except(items).ToList();
		}

		private async Task CheckJsObjectAsync()
		{
			if (_resizeJs is null)
			{
#if DEBUG
				_resizeJs = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Majorsoft.Blazor.Components.Common.JsInterop/resize.js");
#else
				_resizeJs = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Majorsoft.Blazor.Components.Common.JsInterop/resize.min.js");
#endif
			}
		}

		public async ValueTask DisposeAsync()
		{
			if (_resizeJs is not null)
			{
				await _resizeJs.InvokeVoidAsync("dispose", _registeredElements.ToArray());
				await _resizeJs.InvokeVoidAsync("disposeGlobal", _registeredEvents.ToArray());

				await _resizeJs.DisposeAsync();
			}
		}
	}
}