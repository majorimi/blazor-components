namespace Majorsoft.Blazor.Components.Maps.Google
{
	/// <summary>
	/// Rather than use Google's marker icons, you are free to use your own custom icons instead.
	/// </summary>
	public sealed class GoogleMapMarkerCustomIcon
	{
		/// <summary>
		/// Custom icon URL.
		/// </summary>
		public string IconUrl { get; set; }

		/// <summary>
		/// Icon is position in relation to the specified markers locations.
		/// </summary>
		public GoogleMapMarkerCustomIconAnchors Anchor { get; set; }

		public override string ToString()
		{
			return string.IsNullOrWhiteSpace(IconUrl)
				? string.Empty
				: $"anchor:{Anchor.ToString().ToLower()}|icon:{IconUrl}";
		}
	}
}