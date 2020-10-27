using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor.Components.Common.JsInterop.Focus
{
	/// <summary>
	/// Implementation of <see cref="IFocusHandler"/>
	/// </summary>
	public sealed class FocusHandler : IFocusHandler
	{
		private readonly IJSRuntime _jsRuntime;
		private IJSObjectReference _focusJs;

		public FocusHandler(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime;
		}
		public async Task<IJSObjectReference> GetFocusedElementAsync()
		{
			await CheckJsObjectAsync();
			return await _focusJs.InvokeAsync<IJSObjectReference>("getFocusedElement");
		}

		public async Task FocusElementAsync(IJSObjectReference objectReference)
		{
			await CheckJsObjectAsync();
			await _focusJs.InvokeVoidAsync("focusElement", objectReference);
		}
		public async Task FocusElementAsync(ElementReference elementReference)
		{
			await CheckJsObjectAsync();
			await _focusJs.InvokeVoidAsync("focusElement", elementReference);
		}

		public async Task StoreFocusedElementAsync()
		{
			await CheckJsObjectAsync();
			await _focusJs.InvokeVoidAsync("storeFocusedElement");
		}

		public async Task RestoreStoredElementFocusAsync(bool clearElementRef)
		{
			await CheckJsObjectAsync();
			await _focusJs.InvokeVoidAsync("restoreStoredElementFocus", clearElementRef);
		}

		private async Task CheckJsObjectAsync()
		{
			if (_focusJs is null)
			{
#if DEBUG
				_focusJs = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Majorsoft.Blazor.Components.Common.JsInterop/focus.js");
#else
				_focusJs = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Majorsoft.Blazor.Components.Common.JsInterop/focus.min.js");
#endif
			}
		}

		public async ValueTask DisposeAsync()
		{
			if (_focusJs is not null)
			{
				await _focusJs.DisposeAsync();
			}
		}
	}
}
