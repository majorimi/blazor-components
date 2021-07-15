using System;
using System.Collections.Generic;
using System.Linq;

namespace Majorsoft.Blazor.Components.GdprConsent
{
	/// <summary>
	/// GdprConsentData to store collections of <see cref="GdprConsentDetail"/> and validity date.
	/// </summary>
	public class GdprConsentData
	{
		/// <summary>
		/// Collections of <see cref="GdprConsentDetail"/>
		/// </summary>
		public IEnumerable<GdprConsentDetail> GdprConsentDetails { get; set; }

		/// <summary>
		/// Consent answered date.
		/// </summary>
		public DateTime AnsweredAt { get; set; }

		/// <summary>
		/// Consent answer validity date.
		/// </summary>
		public DateTime AnswerValidUntil { get; set; }

		/// <summary>
		/// Gets weather the Consent answer is valid or not.
		/// </summary>
		public bool IsValid => AnswerValidUntil >= DateTime.Now;

		/// <summary>
		/// Gets weather all Consent were accepted.
		/// </summary>
		public bool AllAccepted => (GdprConsentDetails?.Any() ?? false) && (GdprConsentDetails?.All(x => x.IsAccepted) ?? false);

		/// <summary>
		/// Default constructor
		/// </summary>
		public GdprConsentData()
		{
			GdprConsentDetails = new List<GdprConsentDetail>();
		}
	}
}