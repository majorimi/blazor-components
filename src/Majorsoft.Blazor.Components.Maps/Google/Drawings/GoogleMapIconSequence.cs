namespace Majorsoft.Blazor.Components.Maps.Google
{
	/// <summary>
	/// Describes how icons are to be rendered on a line.
	/// </summary>
	public class GoogleMapIconSequence
	{
		/// <summary>
		/// If true, each icon in the sequence has the same fixed rotation regardless of the angle of the edge on which it lies. Defaults to false, 
		/// in which case each icon in the sequence is rotated to align with its edge.
		/// </summary>
		public bool FixedRotation { get; set; }

		/// <summary>
		/// The icon to render on the line.
		/// </summary>
		public GoogleMapIconSequenceSymbol Icon { get; set; }

		/// <summary>
		/// The distance from the start of the line at which an icon is to be rendered. This distance may be expressed as a percentage of line's length 
		/// (e.g. '50%') or in pixels (e.g. '50px'). Defaults to '100%'.
		/// </summary>
		public string Offset { get; set; }

		/// <summary>
		/// The distance between consecutive icons on the line. This distance may be expressed as a percentage of the line's length (e.g. '50%') or in pixels 
		/// (e.g. '50px'). To disable repeating of the icon, specify '0'. Defaults to '0'.
		/// </summary>
		public string Repeat { get; set; }
	}
}