using System.Dynamic;
using System.Threading.Tasks;

using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Components.Common.JsInterop.Navigation
{
	/// <summary>
	/// Implementation of <see cref="INavigationHistoryService"/>
	/// </summary>
	public class NavigationHistoryService : INavigationHistoryService
	{
		private readonly IJSRuntime _jSRuntime;

		public NavigationHistoryService(IJSRuntime jSRuntime)
		{
			_jSRuntime = jSRuntime;
		}

		public async ValueTask<int> GetLengthAsync() => await _jSRuntime.InvokeAsync<int>("eval", "history.length");
		public async ValueTask<string> GetScrollRestorationAsync() => await _jSRuntime.InvokeAsync<string>("eval", "history.scrollRestoration");

		public async ValueTask BackAsync()
			=> await _jSRuntime.InvokeVoidAsync("history.back");

		public async ValueTask ForwardAsync()
			=> await _jSRuntime.InvokeVoidAsync("history.forward");

		public async ValueTask GoAsync(int delta)
			=> await _jSRuntime.InvokeVoidAsync("history.go", delta);

		public async ValueTask PushStateAsync(ExpandoObject? state, string title, string url)
			=> await _jSRuntime.InvokeVoidAsync("history.pushState", state, title, url);

		public async ValueTask ReplaceStateAsync(ExpandoObject? state, string title, string url)
			=> await _jSRuntime.InvokeVoidAsync("history.replaceState", state, title, url);
	}
}