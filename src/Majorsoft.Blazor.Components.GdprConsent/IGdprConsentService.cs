using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Majorsoft.Blazor.Extensions.BrowserStorage;

namespace Majorsoft.Blazor.Components.GdprConsent
{
	/// <summary>
	/// 
	/// </summary>
	public interface IGdprConsentService
	{
		ValueTask<GdprConsentData> GetGdprConsentDataAsync();
	}

	public class GdprConsentService : IGdprConsentService
	{
		private readonly ILocalStorageService _storageService;

		public GdprConsentService(ILocalStorageService storageService)
		{
			_storageService = storageService;
		}

		public ValueTask<GdprConsentData> GetGdprConsentDataAsync() => throw new NotImplementedException();
	}

	public class GdprConsentData
	{
		public bool IsAccepted { get; set; }

		public DateTime AnsweredAt { get; set; }

		public DateTime AnswerValidUntil { get; set; }
	}
}