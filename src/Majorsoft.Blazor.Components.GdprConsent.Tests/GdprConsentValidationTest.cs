using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Majorsoft.Blazor.Components.GdprConsent.Tests
{
    [TestClass]
    public class GdprConsentDataValidationTest
    {
        [TestMethod]
        public void GdprConsentData_should_initialize_empty()
        {
            var data = new GdprConsentData();

            Assert.IsNotNull(data.GdprConsentDetails);
            Assert.AreEqual(0, data.GdprConsentDetails.Count());
        }

        [TestMethod]
        public void GdprConsentData_should_validate_expiry()
        {
            var expiredData = new GdprConsentData()
            {
                AnsweredAt = DateTime.Now.AddDays(-1),
                AnswerValidUntil = DateTime.Now.AddHours(-1)
            };

            Assert.AreEqual(false, expiredData.IsValid);
        }

        [TestMethod]
        public void GdprConsentData_should_validate_future_expiry()
        {
            var validData = new GdprConsentData()
            {
                AnsweredAt = DateTime.Now,
                AnswerValidUntil = DateTime.Now.AddDays(30)
            };

            Assert.AreEqual(true, validData.IsValid);
        }

        [TestMethod]
        public void GdprConsentData_AllAccepted_should_be_false_when_empty()
        {
            var data = new GdprConsentData()
            {
                GdprConsentDetails = new GdprConsentDetail[0]
            };

            Assert.AreEqual(false, data.AllAccepted);
        }

        [TestMethod]
        public void GdprConsentData_AllAccepted_should_be_true_when_all_accepted()
        {
            var data = new GdprConsentData()
            {
                GdprConsentDetails = new GdprConsentDetail[]
                {
                    new GdprConsentDetail() { IsAccepted = true },
                    new GdprConsentDetail() { IsAccepted = true },
                    new GdprConsentDetail() { IsAccepted = true }
                }
            };

            Assert.AreEqual(true, data.AllAccepted);
        }

        [TestMethod]
        public void GdprConsentData_AllAccepted_should_be_false_when_any_rejected()
        {
            var data = new GdprConsentData()
            {
                GdprConsentDetails = new GdprConsentDetail[]
                {
                    new GdprConsentDetail() { IsAccepted = true },
                    new GdprConsentDetail() { IsAccepted = false },
                    new GdprConsentDetail() { IsAccepted = true }
                }
            };

            Assert.AreEqual(false, data.AllAccepted);
        }

        [TestMethod]
        public void GdprConsentData_AllAccepted_should_be_false_with_default_detail()
        {
            var data = new GdprConsentData()
            {
                GdprConsentDetails = new GdprConsentDetail[]
                {
                    new GdprConsentDetail() // Default is not accepted
                }
            };

            Assert.AreEqual(false, data.AllAccepted);
        }

        [TestMethod]
        public void GdprConsentData_should_handle_many_details()
        {
            var details = Enumerable.Range(0, 100)
                .Select(i => new GdprConsentDetail() { IsAccepted = i % 2 == 0 })
                .ToArray();

            var data = new GdprConsentData()
            {
                GdprConsentDetails = details
            };

            Assert.AreEqual(false, data.AllAccepted); // Not all accepted
            Assert.AreEqual(100, data.GdprConsentDetails.Count());
        }

        [TestMethod]
        public void GdprConsentData_should_track_answered_time()
        {
            var now = DateTime.Now;
            var data = new GdprConsentData()
            {
                AnsweredAt = now
            };

            Assert.AreEqual(now.Year, data.AnsweredAt.Year);
            Assert.AreEqual(now.Month, data.AnsweredAt.Month);
            Assert.AreEqual(now.Day, data.AnsweredAt.Day);
        }

        [TestMethod]
        public void GdprConsentData_should_track_validity_expiry()
        {
            var future = DateTime.Now.AddDays(365);
            var data = new GdprConsentData()
            {
                AnswerValidUntil = future
            };

            Assert.AreEqual(future.Year, data.AnswerValidUntil.Year);
            Assert.AreEqual(future.Month, data.AnswerValidUntil.Month);
        }
    }

    [TestClass]
    public class GdprConsentDetailStatusTest
    {
        [TestMethod]
        public void GdprConsentDetail_should_default_to_not_accepted()
        {
            var detail = new GdprConsentDetail();
            Assert.AreEqual(false, detail.IsAccepted);
        }

        [TestMethod]
        public void GdprConsentDetail_should_allow_acceptance()
        {
            var detail = new GdprConsentDetail() { IsAccepted = true };
            Assert.AreEqual(true, detail.IsAccepted);
        }

        [TestMethod]
        public void GdprConsentDetail_should_allow_rejection()
        {
            var detail = new GdprConsentDetail() { IsAccepted = false };
            Assert.AreEqual(false, detail.IsAccepted);
        }

        [TestMethod]
        public void GdprConsentDetail_should_allow_toggling()
        {
            var detail = new GdprConsentDetail() { IsAccepted = false };
            Assert.AreEqual(false, detail.IsAccepted);

            detail.IsAccepted = true;
            Assert.AreEqual(true, detail.IsAccepted);

            detail.IsAccepted = false;
            Assert.AreEqual(false, detail.IsAccepted);
        }

        [TestMethod]
        public void GdprConsentDetail_multiple_instances_should_be_independent()
        {
            var detail1 = new GdprConsentDetail() { IsAccepted = true };
            var detail2 = new GdprConsentDetail() { IsAccepted = false };
            var detail3 = new GdprConsentDetail() { IsAccepted = true };

            Assert.AreEqual(true, detail1.IsAccepted);
            Assert.AreEqual(false, detail2.IsAccepted);
            Assert.AreEqual(true, detail3.IsAccepted);

            // Modifying one doesn't affect others
            detail1.IsAccepted = false;
            Assert.AreEqual(false, detail1.IsAccepted);
            Assert.AreEqual(false, detail2.IsAccepted);
            Assert.AreEqual(true, detail3.IsAccepted);
        }
    }
}
