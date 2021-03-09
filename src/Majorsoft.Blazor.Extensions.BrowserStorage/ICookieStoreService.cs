using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Extensions.BrowserStorage
{
	/// <summary>
	/// 
	/// </summary>
	public interface ICookieStoreService
	{
		Task<Cookie> GetAsync(string name);
		Task<IEnumerable<Cookie>> GetAllAsync();

		Task SetAsync(Cookie cookie);

		Task DeleteAsync(string name);
	}

	public class CookieStoreService: ICookieStoreService
	{
		private readonly IJSRuntime _jSRuntime;

		public CookieStoreService(IJSRuntime jSRuntime)
		{
			_jSRuntime = jSRuntime;
		}

		public async Task<Cookie> GetAsync(string name)
		{
			var ret = await _jSRuntime.InvokeAsync<Cookie>("cookieStore.get", name);
			return ret;
		}

		public async Task<IEnumerable<Cookie>> GetAllAsync() => await _jSRuntime.InvokeAsync<IEnumerable<Cookie>>("cookieStore.getAll");

		public async Task SetAsync(Cookie cookie)
		{
			await _jSRuntime.InvokeVoidAsync("cookieStore.set", cookie);
		}

		public async Task DeleteAsync(string name) => await _jSRuntime.InvokeAsync<Cookie>("cookieStore.delete", name);
	}
}