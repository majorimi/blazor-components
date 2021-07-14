using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Majorsoft.Blazor.Components.GdprConsent.Tests
{
	[TestClass]
	public class GdprConsentDataTest
	{
		[TestMethod]
		public void GdprConsentData_GdprConsentDetails_initialized()
		{
			var data = new GdprConsentData();

			Assert.IsNotNull(data.GdprConsentDetails);
		}

		[TestMethod]
		public void GdprConsentData_IsValid_true()
		{
			var data = new GdprConsentData()
			{
				AnsweredAt = DateTime.Now,
				AnswerValidUntil = DateTime.Now.AddHours(1)
			};

			Assert.AreEqual(true, data.IsValid);
		}

		[TestMethod]
		public void GdprConsentData_IsValid_false()
		{
			var data = new GdprConsentData()
			{
				AnsweredAt = DateTime.Now,
				AnswerValidUntil = DateTime.Now.AddHours(-1)
			};

			Assert.AreEqual(false, data.IsValid);
		}

		[TestMethod]
		public void GdprConsentData_AllAccepted_true()
		{
			var data = new GdprConsentData()
			{
				GdprConsentDetails = new GdprConsentDetail[]
				{
					new GdprConsentDetail() { IsAccepted = true },
					new GdprConsentDetail() { IsAccepted = true },
					new GdprConsentDetail() { IsAccepted = true },
				}
			};

			Assert.AreEqual(true, data.AllAccepted);
		}

		[TestMethod]
		public void GdprConsentData_AllAccepted_false()
		{
			var data = new GdprConsentData()
			{
				GdprConsentDetails = new GdprConsentDetail[]
				{
					new GdprConsentDetail() { IsAccepted = true },
					new GdprConsentDetail() { IsAccepted = true },
					new GdprConsentDetail(),
				}
			};

			Assert.AreEqual(false, new GdprConsentData().AllAccepted);
			Assert.AreEqual(false, data.AllAccepted);
		}
	}
}