using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Majorsoft.Blazor.Components.Common.JsInterop.Head
{
	/// <summary>
	/// Injectable service to handle HTML Head Interops.
	/// </summary>
	public interface IHtmlHeadService : IAsyncDisposable
	{
		/// <summary>
		/// Returns the current HTML page title.
		/// </summary>
		/// <returns>Async Task</returns>
		Task<string> GetHtmlTitleAsync();

		/// <summary>
		/// Sets the given string value as HTML page title.
		/// </summary>
		/// <param name="title"></param>
		/// <returns>Async Task</returns>
		Task SetHtmlTitleAsync(string title);

		/// <summary>
		/// Returns all existing Link tags from HTML page Head by given Rel type filter.
		/// </summary>
		/// <param name="linkType">Allowed Link `rel` types</param>
		/// <returns>Async Task</returns>
		Task<IEnumerable<HtmlHeadLinkTag>> GetHtmlHeadLinkTagsAsync(HtmlHeadLinkTagRelTypes? linkType = null);

		/// <summary>
		/// Returns all existing fav icon "rel=icon" tags from HTML page Head.
		/// </summary>
		/// <returns>Async Task</returns>
		Task<IEnumerable<HtmlHeadLinkTag>> GetHtmlFavIconsAsync();

		/// <summary>
		/// Removes all existing fav icon "rel=icon" tags from HTML page Head and creates new tags from the given objects.
		/// If you have multiple fav icon tags set in the Head first call <see cref="GetHtmlFavIconsAsync"/> method and change <see cref="HtmlHeadLinkTag.Href"/> values.
		/// </summary>
		/// <param name="favIcons">New fav icons to set</param>
		/// <returns>Async Task</returns>
		Task SetHtmlFavIconsAsync(IEnumerable<HtmlHeadLinkTag> favIcons);
	}
}