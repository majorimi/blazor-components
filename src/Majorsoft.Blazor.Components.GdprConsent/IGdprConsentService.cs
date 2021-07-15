using System.Threading.Tasks;

namespace Majorsoft.Blazor.Components.GdprConsent
{
	/// <summary>
	/// Injectable service to handle GDPR Consent actions.
	/// </summary>
	public interface IGdprConsentService
	{
		/// <summary>
		/// Gets GDPR Consent Browser local storage key name
		/// </summary>
		string ConsentStoreKeyName { get; }

		/// <summary>
		/// Gets a <see cref="IGdprConsentNotificationService"/> to able to subscribe on Consent changed events.
		/// </summary>
		IGdprConsentNotificationService ConsentNotificationService { get; }

		/// <summary>
		/// Gets the GDPR Consent data from Browser local storage if any stored.
		/// </summary>
		/// <returns>ValueTask</returns>
		ValueTask<GdprConsentData> GetGdprConsentDataAsync();

		/// <summary>
		/// Sets or overrides the given GDPR Consent data in Browser local storage.
		/// </summary>
		/// <param name="gdprConsentData">GDPR consent details with accepted and rejected cookies list</param>
		/// <returns>ValueTask</returns>
		ValueTask SetGdprConsentDataAsync(GdprConsentData gdprConsentData);

		/// <summary>
		/// Removes the GDPR Consent data from Browser local storage if any stored.
		/// </summary>
		/// <returns>ValueTask</returns>
		ValueTask ClearGdprConsentDataAsync();
	}
}