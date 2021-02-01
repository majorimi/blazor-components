using System;

namespace Majorsoft.Blazor.Components.Maps.Google
{
	/// <summary>
	/// Custom control basic info to add to the Map.
	/// </summary>
	public abstract class GoogleMapCustomControlBase
	{
		/// <summary>
		/// Id of the Custom control.
		/// </summary>
		public string Id { get; init; }

		/// <summary>
		/// Control position on the Map.
		/// </summary>
		public GoogleMapControlPositions ControlPosition { get; set; }

		/// <summary>
		/// HTML content to render as Custom Control button.
		/// </summary>
		public string Content { get; init; }
		//public RenderFragment Content { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="htmlContent"></param>
		public GoogleMapCustomControlBase(string htmlContent)
		{
			if (string.IsNullOrWhiteSpace(htmlContent))
			{
				throw new ArgumentException($"'{nameof(htmlContent)}' cannot be null or whitespace", nameof(htmlContent));
			}

			Content = htmlContent;
			Id = Guid.NewGuid().ToString();
		}
	}
}