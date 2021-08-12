using System;
using System.Threading.Tasks;

using Bunit;

using Majorsoft.Blazor.Components.Common.JsInterop.Focus;
using Majorsoft.Blazor.Components.CommonTestsBase;
using Majorsoft.Blazor.Components.CssEvents.Transition;
using Majorsoft.Blazor.Components.Modal;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Majorsoft.Blazor.Components.GdprConsent.Tests
{
	[TestClass]
	public class GdprModalTest : ComponentsTestBase<GdprModal>
	{
		private Mock<IGdprConsentService> _dprConsentServiceMock;
		private Mock<IGdprConsentNotificationService> _gdprConsentNotificationServiceMock;
		private Mock<ITransitionEventsService> _transitionMock;
		private Mock<IFocusHandler> _focusHandlerMock;

		[TestInitialize]
		public void Init()
		{
			_gdprConsentNotificationServiceMock = new Mock<IGdprConsentNotificationService>();
			var logger = new Mock<ILogger<ModalDialog>>();
			_dprConsentServiceMock = new Mock<IGdprConsentService>();
			_dprConsentServiceMock.SetupGet(g => g.ConsentNotificationService).Returns(_gdprConsentNotificationServiceMock.Object);

			_transitionMock = new Mock<ITransitionEventsService>();
			_focusHandlerMock = new Mock<IFocusHandler>();

			_testContext.Services.Add(new ServiceDescriptor(typeof(ILogger<ModalDialog>), logger.Object));
			_testContext.Services.Add(new ServiceDescriptor(typeof(IGdprConsentService), _dprConsentServiceMock.Object));
			_testContext.Services.Add(new ServiceDescriptor(typeof(ITransitionEventsService), _transitionMock.Object));
			_testContext.Services.Add(new ServiceDescriptor(typeof(IFocusHandler), _focusHandlerMock.Object));
			_testContext.Services.Add(new ServiceDescriptor(typeof(SingletonComponentService<GdprBanner>), new SingletonComponentService<GdprBanner>()));
			_testContext.Services.Add(new ServiceDescriptor(typeof(SingletonComponentService<GdprModal>), new SingletonComponentService<GdprModal>()));
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

			var rendered = _testContext.RenderComponent<GdprModal>();
			rendered.MarkupMatches("");

			_dprConsentServiceMock.Verify(v => v.GetGdprConsentDataAsync(), Times.Once);
		}

		[Ignore] //TODO: does not work because of StateHasChanged() and dialog opens up in Render event.
		[TestMethod]
		public void ModalDialog_should_rendered_correctly_html_attributes()
		{
			var rendered = _testContext.RenderComponent<GdprModal>(
				("id", "id1"), //HTML attributes
				("title", "text"), //HTML attributes
				(nameof(ModalDialog.OverlayOpacity), 0.5)
				);


			var div = rendered.Find("div");
			rendered.Render();

			Assert.IsNotNull(div);

			rendered.WaitForAssertion(() => rendered.MarkupMatches(@"<div class=""bmodal fade"" style=""opacity: 1; background-color: rgba(128, 128, 128, 0.90)"" id=""id1"" title=""text"">
		  <div class=""bmodal-content dynamicStyle"" tabindex=""100"">
			<div class=""bmodal-header"">
				  <button type = ""button""  class=""close"">
					<span aria-hidden=""true"">x</span>
					<span class=""sr-only"">Close</span>
				  </button>
				</div>
				<div class=""bmodal-body""></div>
			  </div>
			</div>
			<style>
				.fade {
					transition: opacity 0.25s linear;
				}
				.dynamicStyle {
					top: calc(15% &#x2B; 0px);
					left: 50%;
					min-width:200px;
					min-height:200px;
					width:auto;
					height:auto;
					transition: top 0.25s ease-in-out;
				}
			</style>"));
		}

		[TestMethod]
		public async Task GdprModal_should_SaveChoice_as_user_choosen_ConsentDetails()
		{
			_dprConsentServiceMock.Setup(s => s.GetGdprConsentDataAsync())
				.ReturnsAsync(new GdprConsentData()
				{
					AnsweredAt = DateTime.Now,
					AnswerValidUntil = DateTime.Now.AddDays(1),
				});
			var details = new GdprConsentDetail[]
				{
					new GdprConsentDetail() { ConsentName = "All", IsAccepted = false },
					new GdprConsentDetail() { ConsentName = "Tracking", IsAccepted = true },
					new GdprConsentDetail() { ConsentName = "Session", IsAccepted = true },
				};

			var rendered = _testContext.RenderComponent<GdprModal>(parameters => parameters
				.Add(p => p.ConsentDetails, details));

			await rendered.Instance.SaveChoice();

			_dprConsentServiceMock.Verify(v => v.SetGdprConsentDataAsync(It.Is<GdprConsentData>(v => !details[0].IsAccepted && details[1].IsAccepted && details[2].IsAccepted)),
				Times.Once);
		}
	}
}