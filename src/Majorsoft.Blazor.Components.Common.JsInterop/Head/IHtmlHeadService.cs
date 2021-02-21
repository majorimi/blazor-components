using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Majorsoft.Blazor.Components.Common.JsInterop.Head
{
	/// <summary>
	/// The `link` tag defines the relationship between the current document and an external resource.
	/// </summary>
	public class HtmlHeadLinkTag
	{
		/// <summary>
		/// Specifies how the element handles cross-origin requests
		/// </summary>
		public string Crossorigin { get; set; }

		/// <summary>
		/// Specifies the location of the linked document
		/// </summary>
		public string Href { get; set; }

		/// <summary>
		/// Specifies the language of the text in the linked document
		/// </summary>
		public string HrefLang { get; set; }

		/// <summary>
		/// Specifies on what device the linked document will be displayed
		/// </summary>
		public string Media { get; set; }

		/// <summary>
		/// Specifies which referrer to use when fetching the resource
		/// </summary>
		public string ReferrerPolicy { get; set; }

		/// <summary>
		/// Required. Specifies the relationship between the current document and the linked document
		/// </summary>
		public string Rel { get; set; }

		/// <summary>
		/// Specifies the size of the linked resource. Only for rel="icon"
		/// </summary>
		public string Sizes { get; set; }

		/// <summary>
		/// Defines a preferred or an alternate stylesheet
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// Specifies the media type of the linked document
		/// </summary>
		public string Type { get; set; }
	}

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