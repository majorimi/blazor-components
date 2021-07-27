using System;
using System.Linq;
using System.Threading.Tasks;

using Bunit;

using Majorsoft.Blazor.Components.CommonTestsBase;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Majorsoft.Blazor.Components.GdprConsent.Tests
{
	[TestClass]
	public class GdprBannerTest : ComponentsTestBase<GdprBanner>
	{
		private Mock<IGdprConsentService> _dprConsentServiceMock;
		private Mock<IGdprConsentNotificationService> _gdprConsentNotificationServiceMock;

		[TestInitialize]
		public void Init()
		{
			_gdprConsentNotificationServiceMock = new Mock<IGdprConsentNotificationService>();

			_dprConsentServiceMock = new Mock<IGdprConsentService>();
			_dprConsentServiceMock.SetupGet(g => g.ConsentNotificationService).Returns(_gdprConsentNotificationServiceMock.Object);
			_testContext.Services.Add(new ServiceDescriptor(typeof(IGdprConsentService), _dprConsentServiceMock.Object));
		}

		[TestMethod]
		public void GdprBanner_should_not_render_anything_if_consent_valid()
		{
			_dprConsentServiceMock.Setup(s => s.GetGdprConsentDataAsync())
				.ReturnsAsync(new GdprConsentData()
				{
					AnsweredAt = DateTime.Now,
					AnswerValidUntil = DateTime.Now.AddDays(1),
				});

			var rendered = _testContext.RenderComponent<GdprBanner>();
			rendered.MarkupMatches("");

			_dprConsentServiceMock.Verify(v => v.GetGdprConsentDataAsync(), Times.Once);
		}

		[TestMethod]
		public void GdprBanner_should_rendered_correctly_html_attributes()
		{
			var rendered = _testContext.RenderComponent<GdprBanner>(
				("id", "id1"), //HTML attributes
				("title", "text"), //HTML attributes
				(nameof(GdprBanner.BannerOpacity), 0.5)
				);

			var div = rendered.Find("div");
			Assert.IsNotNull(div);

			rendered.MarkupMatches("<div class=\"banner\" style=\"background-color: rgba(128,128,128, 0.50)\" id=\"id1\" title=\"text\"  ></div>");
		}

		[TestMethod]
		public void GdprBanner_should_rendered_correctly_BannerOpacity()
		{
			var rendered = _testContext.RenderComponent<GdprBanner>(parameters => parameters
				.Add(p => p.BannerOpacity, 0.99));

			var div = rendered.Find("div");
			Assert.IsNotNull(div);

			rendered.MarkupMatches("<div class=\"banner\" style=\"background-color: rgba(128,128,128, 0.99)\"></div>");
		}

		[TestMethod]
		public void GdprBanner_should_rendered_correctly_BannerBackgroundColor()
		{
			var rendered = _testContext.RenderComponent<GdprBanner>(parameters => parameters
				.Add(p => p.BannerBackgroundColor, "red"));

			var div = rendered.Find("div");
			Assert.IsNotNull(div);

			rendered.MarkupMatches("<div class=\"banner\" style=\"background-color: rgba(255,0,0, 0.90)\"></div>");
		}

		[TestMethod]
		public void GdprBanner_should_rendered_correctly_Content()
		{
			var rendered = _testContext.RenderComponent<GdprBanner>(parameters => parameters
				.Add(p => p.Content, "<div><p>Banner text</p></div>"));

			var div = rendered.Find("div");
			Assert.IsNotNull(div);

			rendered.MarkupMatches("<div class=\"banner\" style=\"background-color: rgba(128,128,128, 0.90)\"><div><p>Banner text</p></div></div>");
		}

		[TestMethod]
		public async Task GdprBanner_should_accept_all_ConsentDetails()
		{
			var rendered = _testContext.RenderComponent<GdprBanner>(parameters => parameters
				.Add(p => p.ConsentDetails, new GdprConsentDetail[]
				{
					new GdprConsentDetail() { ConsentName = "All", IsAccepted = false },
					new GdprConsentDetail() { ConsentName = "Tracking" },
					new GdprConsentDetail() { ConsentName = "Session" },
				}));

			await rendered.Instance.AcceptAll();

			_dprConsentServiceMock.Verify(v => v.SetGdprConsentDataAsync(It.Is<GdprConsentData>(v => v.AllAccepted)), Times.Once);
		}

		[TestMethod]
		public async Task GdprBanner_should_reject_all_ConsentDetails()
		{
			var rendered = _testContext.RenderComponent<GdprBanner>(parameters => parameters
				.Add(p => p.ConsentDetails, new GdprConsentDetail[]
				{
					new GdprConsentDetail() { ConsentName = "All", IsAccepted = true },
					new GdprConsentDetail() { ConsentName = "Tracking" },
					new GdprConsentDetail() { ConsentName = "Session" },
				}));

			await rendered.Instance.RejectAll();

			_dprConsentServiceMock.Verify(v => v.SetGdprConsentDataAsync(It.Is<GdprConsentData>(v => v.GdprConsentDetails.All(x => !x.IsAccepted))), Times.Once);
		}
	}
}