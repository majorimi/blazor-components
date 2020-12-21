using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Components.Common.JsInterop.GlobalMouseEvents
{
	/// <summary>
	/// Implementation of <see cref="IGlobalMouseEventHandler"/>
	/// </summary>
	public sealed class GlobalMouseEventHandler : IGlobalMouseEventHandler
	{
		private readonly IJSRuntime _jsRuntime;
		private List<string> _registeredEvents;
		private IJSObjectReference _mouseJs;

		public GlobalMouseEventHandler(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime;
			_registeredEvents = new List<string>();
		}

		public async Task<string> RegisterPageMouseDownAsync(Func<MouseEventArgs, Task> mouseDownCallback)
		{
			await CheckJsObjectAsync();

			var id = Guid.NewGuid().ToString();
			var info = new PageMouseEventInfo(mouseDownCallback, id);
			var dotnetRef = DotNetObjectReference.Create<PageMouseEventInfo>(info);

			await _mouseJs.InvokeVoidAsync("addGlobalMouseDownHandler", dotnetRef, id);
			_registeredEvents.Add(id);

			return id;
		}
		public async Task RemovePageMouseDownAsync(string eventId)
		{
			await CheckJsObjectAsync();

			await _mouseJs.InvokeVoidAsync("removeGlobalMouseDownHandler", eventId);
			RemoveElement(eventId);
		}

		public async Task<string> RegisterPageMouseMoveAsync(Func<MouseEventArgs, Task> mouseMoveCallback)
		{
			await CheckJsObjectAsync();

			var id = Guid.NewGuid().ToString();
			var info = new PageMouseEventInfo(mouseMoveCallback, id);
			var dotnetRef = DotNetObjectReference.Create<PageMouseEventInfo>(info);

			await _mouseJs.InvokeVoidAsync("addGlobalMouseMoveHandler", dotnetRef, id);
			_registeredEvents.Add(id);

			return id;
		}
		public async Task RemovePageMouseMoveAsync(string eventId)
		{
			await CheckJsObjectAsync();

			await _mouseJs.InvokeVoidAsync("removeGlobalMouseMoveHandler", eventId);
			RemoveElement(eventId);
		}

		public async Task<string> RegisterPageMouseUpAsync(Func<MouseEventArgs, Task> mouseUpCallback)
		{
			await CheckJsObjectAsync();

			var id = Guid.NewGuid().ToString();
			var info = new PageMouseEventInfo(mouseUpCallback, id);
			var dotnetRef = DotNetObjectReference.Create<PageMouseEventInfo>(info);

			await _mouseJs.InvokeVoidAsync("addGlobalMouseUpHandler", dotnetRef, id);
			_registeredEvents.Add(id);

			return id;
		}
		public async Task RemovePageMouseUpAsync(string eventId)
		{
			await CheckJsObjectAsync();

			await _mouseJs.InvokeVoidAsync("removeGlobalMouseUpHandler", eventId);
			RemoveElement(eventId);
		}

		private void RemoveElement(string eventId)
		{
			var items = _registeredEvents.Where(x => x.Equals(eventId));
			_registeredEvents = _registeredEvents.Except(items).ToList();
		}

		private async Task CheckJsObjectAsync()
		{
			if (_mouseJs is null)
			{
#if DEBUG
				_mouseJs = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Majorsoft.Blazor.Components.Common.JsInterop/globalMouseEvents.js");
#else
				_mouseJs = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Majorsoft.Blazor.Components.Common.JsInterop/globalMouseEvents.min.js");
#endif
			}
		}

		public async ValueTask DisposeAsync()
		{
			if (_mouseJs is not null)
			{
				await _mouseJs.InvokeVoidAsync("dispose", (object)_registeredEvents.ToArray());
				await _mouseJs.DisposeAsync();
			}
		}
	}
}