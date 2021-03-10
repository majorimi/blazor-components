namespace Majorsoft.Blazor.Components.Maps.Google
{
	/// <summary>
	/// Info Window to display HTML content on Map.
	/// </summary>
	public class GoogleMapInfoWindow
	{
		/// <summary>
		/// HTML content of the Info Window.
		/// </summary>
		public string Content { get; set; }
		//public RenderFragment Content { get; set; }

		/// <summary>
		/// Maximum allowed with of the Info Window.
		/// </summary>
		public int? MaxWidth { get; set; }
	}
}