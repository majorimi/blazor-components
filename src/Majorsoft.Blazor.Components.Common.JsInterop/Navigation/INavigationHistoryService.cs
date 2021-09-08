using System.Dynamic;
using System.Threading.Tasks;

namespace Majorsoft.Blazor.Components.Common.JsInterop.Navigation
{
	/// <summary>
	/// Injectable service to handle Browser history JS Interops.
	/// https://developer.mozilla.org/en-US/docs/Web/API/History
	/// </summary>
	public interface INavigationHistoryService
	{
		/// <summary>
		/// Returns an Integer representing the number of elements in the session history, including the currently loaded page. For example, for a page loaded in a new tab this property returns 1.
		/// </summary>
		/// <returns>ValueTask</returns>
		ValueTask<int> GetLengthAsync();

		/// <summary>
		/// Allows web applications to explicitly set default scroll restoration behavior on history navigation. This property can be either `auto` or `manual`.
		/// </summary>
		/// <returns>ValueTask</returns>
		ValueTask<string> GetScrollRestorationAsync();

		/// <summary>
		/// This asynchronous method goes to the previous page in session history, the same action as when the user clicks the browser's Back button. Equivalent to history.go(-1).
		/// </summary>
		/// <returns>ValueTask</returns>
		ValueTask BackAsync();

		/// <summary>
		/// This asynchronous method goes to the next page in session history, the same action as when the user clicks the browser's Forward button; this is equivalent to history.go(1).
		/// </summary>
		/// <returns>ValueTask</returns>
		ValueTask ForwardAsync();

		/// <summary>
		/// Asynchronously loads a page from the session history, identified by its relative location to the current page, for example -1 for the previous page or 1 for the next page.
		/// </summary>
		/// <param name="delta">The position in the history to which you want to move, relative to the current page. A negative value moves backwards, a positive value moves forwards.</param>
		/// <returns>ValueTask</returns>
		ValueTask GoAsync(int delta);

		/// <summary>
		/// Updates the most recent entry on the history stack to have the specified data, title, and, if provided, URL.
		/// </summary>
		/// <param name="state">Arbitrary object of the page</param>
		/// <param name="title">Page tile</param>
		/// <param name="url">New URL to show and add to history</param>
		/// <returns>ValueTask</returns>
		ValueTask ReplaceStateAsync(ExpandoObject? state, string title, string url);

		/// <summary>
		/// Pushes the given data onto the session history stack with the specified title (and, if provided, URL).
		/// </summary>
		/// <param name="state">Arbitrary object of the page</param>
		/// <param name="title">Page tile</param>
		/// <param name="url">New URL to show and add to history</param>
		/// <returns>ValueTask</returns>
		ValueTask PushStateAsync(ExpandoObject? state, string title, string url);
	}
}