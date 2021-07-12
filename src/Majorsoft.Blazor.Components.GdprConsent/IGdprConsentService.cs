using System.Threading.Tasks;

namespace Majorsoft.Blazor.Components.GdprConsent
{
	/// <summary>
	/// 
	/// </summary>
	public interface IGdprConsentNotificationService
	{
		/// <summary>
		/// 
		/// </summary>
		event ConsentNotificationEventHandler GdprConsentStateChanged;

		/// <summary>
		/// 
		/// </summary>
		void OnChange();
	}

	/// <summary>
	/// 
	/// </summary>
	public class GdprConsentNotificationService : IGdprConsentNotificationService
	{
		public event ConsentNotificationEventHandler GdprConsentStateChanged;

		public void OnChange()
		{
			GdprConsentStateChanged?.Invoke();
		}
	}

	/// <summary>
	/// Injectable service to handle GDPR Consent actions.
	/// </summary>
	public interface IGdprConsentService
	{
		/// <summary>
		/// Gets GDPR Consent Browser storage key name
		/// </summary>
		string ConsentStoreKeyName { get; }

		/// <summary>
		/// 
		/// </summary>
		IGdprConsentNotificationService ConsentNotificationService { get; }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		ValueTask<GdprConsentData> GetGdprConsentDataAsync();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="gdprConsentData"></param>
		/// <returns></returns>
		ValueTask SetGdprConsentDataAsync(GdprConsentData gdprConsentData);

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		ValueTask ClearGdprConsentDataAsync();
	}
}