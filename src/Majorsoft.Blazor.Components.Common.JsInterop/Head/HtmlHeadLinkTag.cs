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
}