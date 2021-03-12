using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Components.Common.JsInterop.Head
{
	/// <summary>
	/// Implementation of <see cref="IHtmlHeadService"/>
	/// </summary>
	public class HtmlHeadService : IHtmlHeadService
	{
		private readonly IJSRuntime _jsRuntime;
		private IJSObjectReference _headJs;

		public HtmlHeadService(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime;
		}

		public async Task<string> GetHtmlTitleAsync()
		{
			await CheckJsObjectAsync();

			return await _headJs.InvokeAsync<string>("getHtmlTitle");
		}
		public async Task SetHtmlTitleAsync(string title)
		{
			await CheckJsObjectAsync();

			await _headJs.InvokeVoidAsync("setHtmlTitle", title);
		}

		public async Task<IEnumerable<HtmlHeadLinkTag>> GetHtmlHeadLinkTagsAsync(HtmlHeadLinkTagRelTypes? linkType)
		{
			await CheckJsObjectAsync();

			return await _headJs.InvokeAsync<IEnumerable<HtmlHeadLinkTag>>("getAllLinkHeadTags", linkType?.ToString()?.ToLower());
		}

		public async Task<IEnumerable<HtmlHeadLinkTag>> GetHtmlFavIconsAsync()
		{
			await CheckJsObjectAsync();

			return await _headJs.InvokeAsync<IEnumerable<HtmlHeadLinkTag>>("getAllLinkHeadTags", HtmlHeadLinkTagRelTypes.Icon.ToString().ToLower());
		}

		public async Task SetHtmlFavIconsAsync(IEnumerable<HtmlHeadLinkTag> favIcons)
		{
			await CheckJsObjectAsync();

			await _headJs.InvokeVoidAsync("setFavIconsHeadTags", (object)favIcons.ToArray());		
		}

		private async Task CheckJsObjectAsync()
		{
			if (_headJs is null)
			{
#if DEBUG
				_headJs = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Majorsoft.Blazor.Components.Common.JsInterop/head.js");
#else
				_headJs = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Majorsoft.Blazor.Components.Common.JsInterop/head.min.js");
#endif
			}
		}

		public async ValueTask DisposeAsync()
		{
			if (_headJs is not null)
			{
				await _headJs.DisposeAsync();
			}
		}
	}
}