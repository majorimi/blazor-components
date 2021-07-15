namespace Majorsoft.Blazor.Components.GdprConsent
{
	/// <summary>
	/// GdprConsentDetail with Name and acceptance indicator.
	/// </summary>
	public class GdprConsentDetail
	{
		/// <summary>
		/// Name of the Consent type e.g: All, Session, Tracking.
		/// </summary>
		public string ConsentName { get; set; } = "";

		/// <summary>
		/// User accepted or rejected cookie consent.
		/// </summary>
		public bool IsAccepted { get; set; }
	}
}