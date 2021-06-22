namespace Majorsoft.Blazor.Components.Common.JsInterop.Scroll
{
	/// <summary>
	/// Scroll result
	/// </summary>
	public sealed class ScrollResult
	{
		/// <summary>
		/// Scroll X value
		/// </summary>
		public double X { get; set; }
		/// <summary>
		/// Scroll Y value
		/// </summary>
		public double Y { get; set; }

		/// <summary>
		/// Is scroll at page top
		/// </summary>
		public bool IsPageTop { get; set; }

		/// <summary>
		/// Is scroll at page bottom.
		/// </summary>
		public bool IsPageBottom { get; set; }
	}
}