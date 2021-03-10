using System.Collections.Generic;
using System.Threading.Tasks;

namespace Majorsoft.Blazor.Extensions.BrowserStorage
{
	/// <summary>
	/// The CookieStore interface of the Cookie Store API provides methods for getting and setting cookies.
	/// This feature is available only in secure contexts (HTTPS), in some or all supporting browsers.
	/// https://developer.mozilla.org/en-US/docs/Web/API/CookieStore
	/// </summary>
	public interface ICookieStoreService
	{
		/// <summary>
		/// The get() method gets a single cookie with the given name.
		/// </summary>
		/// <param name="name">Cookie name</param>
		/// <returns>Async Task</returns>
		Task<Cookie> GetAsync(string name);

		/// <summary>
		/// The getAll() method gets all matching cookies.
		/// </summary>
		/// <returns>Async Task</returns>
		Task<IEnumerable<Cookie>> GetAllAsync();

		/// <summary>
		/// The set() method sets a cookie with the given name and value.
		/// </summary>
		/// <param name="cookie">Cookie data to set</param>
		/// <returns>Async Task</returns>
		Task SetAsync(Cookie cookie);

		/// <summary>
		/// The delete() method deletes a cookie with the given name.
		/// </summary>
		/// <param name="name">Cookie name</param>
		/// <returns>Async Task</returns>
		Task DeleteAsync(string name);
	}
}