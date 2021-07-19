namespace Majorsoft.Blazor.Components.GdprConsent
{
	/// <summary>
	/// Default implementation for <see cref="IGdprConsentNotificationService"/>
	/// </summary>
	public class GdprConsentNotificationService : IGdprConsentNotificationService
	{
		public event ConsentNotificationEventHandler GdprConsentStateChanged;

		public void OnChange()
		{
			GdprConsentStateChanged?.Invoke();
		}
	}
}