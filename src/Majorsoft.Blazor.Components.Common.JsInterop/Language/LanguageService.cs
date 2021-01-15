using System.Globalization;
using System.Threading.Tasks;

using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Components.Common.JsInterop.Language
{
	/// <summary>
	/// Implementation of <see cref="ILanguageService"/>
	/// </summary>
	public sealed class LanguageService : ILanguageService
	{
		private readonly IJSRuntime _jsRuntime;
		private IJSObjectReference _langJs;

		public LanguageService(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime;
		}

		public async Task<CultureInfo> GetBrowserLanguageAsync()
		{
			await CheckJsObjectAsync();
			var lang = await _langJs.InvokeAsync<string>("detectLanguage");

			return new CultureInfo(lang, false);
		}

		private async Task CheckJsObjectAsync()
		{
			if (_langJs is null)
			{
#if DEBUG
				_langJs = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Majorsoft.Blazor.Components.Common.JsInterop/lang.js");
#else
				_langJs = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Majorsoft.Blazor.Components.Common.JsInterop/lang.min.js");
#endif
			}
		}

		public async ValueTask DisposeAsync()
		{
			if (_langJs is not null)
			{
				await _langJs.DisposeAsync();
			}
		}
	}
}