using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Majorsoft.Blazor.Components.Common.JsInterop.Language
{
	/// <summary>
	/// Injectable service to handle Browser language Interops.
	/// </summary>
	public interface ILanguageService : IAsyncDisposable
	{
		/// <summary>
		/// Returns the browser preferred language.
		/// </summary>
		/// <returns>Async Task</returns>
		Task<CultureInfo> GetBrowserLanguageAsync();
	}
}