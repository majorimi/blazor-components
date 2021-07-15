namespace Majorsoft.Blazor.Components.GdprConsent
{
	/// <summary>
	/// Injectable singleton service to handle GDPR Consent changes  with events.
	/// </summary>
	public interface IGdprConsentNotificationService
	{
		/// <summary>
		/// Event handler to subscribe for GDPR Consent changes.
		/// </summary>
		event ConsentNotificationEventHandler GdprConsentStateChanged;

		/// <summary>
		/// Method called by <see cref="IGdprConsentService"/> to trigger Change event when GDPR Consent value was changed.
		/// </summary>
		void OnChange();
	}
}