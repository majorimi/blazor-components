using System;

namespace Majorsoft.Blazor.Components.Maps.Google
{
	/// <summary>
	/// These options specify the appearance of a marker label. 
	/// A marker label is a string (often a single character) which will appear inside the marker.
	/// </summary>
	public class GoogleMapMarkerLabel
	{
		/// <summary>
		/// The text to be displayed in the label.
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// The className property of the label's element (equivalent to the element's class attribute). 
		/// Multiple space-separated CSS classes can be added.
		/// </summary>
		public string? ClassName { get; set; }

		/// <summary>
		/// The color of the label text. Default color is black.
		/// </summary>
		public string? Color { get; set; }

		/// <summary>
		/// The font family of the label text (equivalent to the CSS font-family property).
		/// </summary>
		public string? FontFamily { get; set; }

		/// <summary>
		/// The font size of the label text (equivalent to the CSS font-size property). Default size is 14px.
		/// </summary>
		public string? FontSize { get; set; }

		/// <summary>
		/// The font weight of the label text (equivalent to the CSS font-weight property).
		/// </summary>
		public string? FontWeight { get; set; }

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="text">Label text</param>
		public GoogleMapMarkerLabel(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				throw new ArgumentException($"'{nameof(text)}' cannot be null or empty", nameof(text));
			}

			Text = text;
		}
	}
}