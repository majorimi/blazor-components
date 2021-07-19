using System;
using System.Threading.Tasks;

using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Components.Common.JsInterop.BrowserDate
{
	/// <summary>
	/// Implementation of <see cref="IBrowserDateService"/>
	/// </summary>
	public class BrowserDateService : IBrowserDateService
	{
		private readonly IJSRuntime _jSRuntime;

		public BrowserDateService(IJSRuntime jSRuntime)
		{
			_jSRuntime = jSRuntime;
		}

		public async Task<DateTime> GetBrowserDateTimeAsync()
		{
			return await _jSRuntime.InvokeAsync<DateTime>("eval", "(new Date()).toJSON();");
		}
	}
}