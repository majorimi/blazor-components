using System.Threading.Tasks;

using Majorsoft.Blazor.Extensions.BrowserStorage;

namespace Majorsoft.Blazor.Components.GdprConsent
{
	/// <summary>
	/// Implementation of <see cref="IGdprConsentService"/>
	/// </summary>
	public class GdprConsentService : IGdprConsentService
	{
		private readonly ILocalStorageService _storageService;

		public string ConsentStoreKeyName => "GDPR_ConsentData";

		public IGdprConsentNotificationService ConsentNotificationService { get; }

		public GdprConsentService(ILocalStorageService storageService,
			IGdprConsentNotificationService gdprConsentNotificationService)
		{
			_storageService = storageService;
			ConsentNotificationService = gdprConsentNotificationService;
		}

		public async ValueTask<GdprConsentData> GetGdprConsentDataAsync() => await _storageService.GetItemAsync<GdprConsentData>(ConsentStoreKeyName);
		public async ValueTask SetGdprConsentDataAsync(GdprConsentData gdprConsentData)
		{
			await _storageService.SetItemAsync<GdprConsentData>(ConsentStoreKeyName, gdprConsentData);
			ConsentNotificationService.OnChange();
		}
		public async ValueTask ClearGdprConsentDataAsync()
		{
			await _storageService.RemoveItemAsync(ConsentStoreKeyName);
			ConsentNotificationService.OnChange();
		}
	}
}