using System;
using System.Threading.Tasks;

namespace Majorsoft.Blazor.Components.Common.JsInterop.BrowserDate
{
	/// <summary>
	/// Injectable service to query Browser Date with JS Interops.
	/// </summary>
	public interface IBrowserDateService
	{
		/// <summary>
		/// Returns Date and time from browser client machine.
		/// </summary>
		/// <returns>Async Task</returns>
		Task<DateTime> GetBrowserDateTimeAsync();
	}
}