using Majorsoft.Blazor.Components.Common.JsInterop.GlobalMouseEvents;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Majorsoft.Blazor.Components.Common.JsInterop.Resize
{
	/// <summary>
	/// Implementation of <see cref="IResizeHandler"/>
	/// </summary>
	public sealed class ResizeHandler : IResizeHandler
	{
		private readonly IJSRuntime _jsRuntime;
		private IJSObjectReference _resizeJs;
		private List<DotNetObjectReference<ResizeEventInfo>> _dotNetObjectReferences;

		public ResizeHandler(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime;
			_dotNetObjectReferences = new List<DotNetObjectReference<ResizeEventInfo>>();
		}

		public async Task<string> RegisterPageResizeAsync(Func<ResizeEventArgs, Task> resizeCallback)
		{
			await CheckJsObjectAsync();

			var id = Guid.NewGuid().ToString();
			var info = new ResizeEventInfo(resizeCallback, id);
			var dotnetRef = DotNetObjectReference.Create<ResizeEventInfo>(info);
			_dotNetObjectReferences.Add(dotnetRef);

			await _resizeJs.InvokeVoidAsync("addGlobalResizeEvent", dotnetRef, id);

			return id;
		}

		public async Task RemovePageResizeAsync(string eventId)
		{
			await CheckJsObjectAsync();

			await _resizeJs.InvokeVoidAsync("removeGlobalResizeEvent", eventId);

			var dotNetRefs = _dotNetObjectReferences.Where(x => x.Value.EventId == eventId);
			RemoveElement(dotNetRefs);
		}

		public async Task<PageSize> GetPageSizeAsync()
		{
			await CheckJsObjectAsync();

			var size = await _resizeJs.InvokeAsync<PageSize>("getPageSize");
			return size;
		}

		public async Task<PageSize> GetScreenSizeAsync()
		{
			await CheckJsObjectAsync();

			var size = await _resizeJs.InvokeAsync<PageSize>("getScreenSize");
			return size;
		}

		public async Task RegisterResizeAsync(ElementReference elementRef, Func<ResizeEventArgs, Task> resizeCallback = null)
		{
			await CheckJsObjectAsync();

			var info = new ResizeEventInfo(resizeCallback, null);
			var dotnetRef = DotNetObjectReference.Create<ResizeEventInfo>(info);
			info.ElementReference = elementRef;
			_dotNetObjectReferences.Add(dotnetRef);

			await _resizeJs.InvokeVoidAsync("addResizeEventHandler", elementRef, dotnetRef);
		}

		public async Task RemoveResizeAsync(ElementReference elementRef)
		{
			await CheckJsObjectAsync();

			await _resizeJs.InvokeVoidAsync("removeResizeEventHandler", elementRef);

			var dotNetRefs = _dotNetObjectReferences.Where(x => x.Value.ElementReference.Equals(elementRef));
			RemoveElement(dotNetRefs);
		}

		private void RemoveElement(IEnumerable<DotNetObjectReference<ResizeEventInfo>> dotNetRefs)
		{
			_dotNetObjectReferences = _dotNetObjectReferences.Except(dotNetRefs).ToList();

			foreach (var item in dotNetRefs)
			{
				item.Dispose();
			}
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
				await _resizeJs.InvokeVoidAsync("dispose", 
					(object)_dotNetObjectReferences.Select(s => s.Value.ElementReference).ToArray());
				await _resizeJs.InvokeVoidAsync("disposeGlobal", 
					(object)_dotNetObjectReferences.Select(s => s.Value.EventId).ToArray());

				await _resizeJs.DisposeAsync();
			}

			foreach (var item in _dotNetObjectReferences)
			{
				item.Dispose();
			}
		}
	}
}