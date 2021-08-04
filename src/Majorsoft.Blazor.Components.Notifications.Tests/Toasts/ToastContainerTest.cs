using Majorsoft.Blazor.Components.CssEvents.Transition;

using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using Majorsoft.Blazor.Components.Timer;
using System;
using Majorsoft.Blazor.Components.CommonTestsBase;

namespace Majorsoft.Blazor.Components.Notifications.Tests.Toasts
{
	[TestClass]
	public class ToastContainerTest : ComponentsTestBase<ToastContainer>
	{
		private Mock<ITransitionEventsService> _transitionMock;
		private Mock<IToastService> _toastServiceMock;

		[TestInitialize]
		public void Init()
		{
			var logger = new Mock<ILogger<AdvancedTimer>>();
			var logger2 = new Mock<ILogger<Toast>>();
			_transitionMock = new Mock<ITransitionEventsService>();
			_toastServiceMock = new Mock<IToastService>();

			_testContext.Services.Add(new ServiceDescriptor(typeof(ILogger<AdvancedTimer>), logger.Object));
			_testContext.Services.Add(new ServiceDescriptor(typeof(ILogger<Toast>), logger2.Object));
			_testContext.Services.Add(new ServiceDescriptor(typeof(ITransitionEventsService), _transitionMock.Object));
			_testContext.Services.Add(new ServiceDescriptor(typeof(IToastService), _toastServiceMock.Object));
		}

		[TestMethod]
		public void Alert_should_not_rendered_anything_until_has_Toasts()
		{
			var rendered = _testContext.RenderComponent<ToastContainer>(
				("id", "id1"), //HTML attributes
				("title", "text") //HTML attributes
				);

			rendered.MarkupMatches("");
		}

		[TestMethod]
		public void Alert_should_rendered_correctly_html_when_has_Toasts()
		{
			_toastServiceMock.SetupGet(g => g.Toasts).Returns(new ToastSettings[] { new ToastSettings() });
			_toastServiceMock.SetupGet(g => g.GlobalSettings).Returns(new  ToastContainerGlobalSettings());

			var rendered = _testContext.RenderComponent<ToastContainer>(
				("id", "id1"), //HTML attributes
				("title", "text") //HTML attributes
				);

			var div = rendered.Find("div");
			Assert.IsNotNull(div);

			rendered.WaitForAssertion(() => rendered.MarkupMatches(@"<div style=""max-width: 400px; width: 400px;"" class=""btoast-container position-topright"" id=""id1"" title=""text"" >
			  <div class=""btoast-main bnotify-normal-primary"" style=""opacity: 1;
							margin-bottom: 17px;
							box-shadow: 1px 5px 20px 0px #c7c7c7;"" tabindex=""750""  >
				<div class=""btoast-body"" >
				  <div >
					<svg class=""btoast-img"" focusable=""false"" viewBox=""0 0 24 24"" aria-hidden=""true"" >
					  <path fill-rule=""evenodd"" clip-rule=""evenodd"" d=""m18.598 2.865c0-0.552-0.447-1-1-1s-1 0.448-1 1v2c0 0.552 0.447 1 1 1s1-0.448 1-1v-2zm-12 2c0 0.552-0.447 1-1 1s-1-0.448-1-1v-2c0-0.552 0.447-1 1-1s1 0.448 1 1v2zm13 5v10h-16v-10h16zm2-6h-2v1c0 1.103-0.897 2-2 2s-2-0.897-2-2v-1h-8v1c0 1.103-0.897 2-2 2s-2-0.897-2-2v-1h-2v18h20v-18zm-13 10h-2v-2h2v2zm4 0h-2v-2h2v2zm4 0h-2v-2h2v2zm-8 4h-2v-2h2v2zm4 0h-2v-2h2v2zm4 0h-2v-2h2v2z"" ></path>
					</svg>
				  </div>
				  <div class=""btoast-text"" ></div>
				  <button type=""button""  class=""close normal"" >
					<span aria-hidden=""true"" >×</span>
					<span class=""sr-only"" >Close</span>
				  </button>
				</div>
				<div class=""btoast-progress primary start"" style=""transition: width 10s linear;"" ></div>
			  </div>
			</div>"));
		}

		[TestMethod]
		public void Alert_should_not_render_Settings_Width()
		{
			_toastServiceMock.SetupGet(g => g.Toasts).Returns(new ToastSettings[] { new ToastSettings() });
			_toastServiceMock.SetupGet(g => g.GlobalSettings).Returns(new ToastContainerGlobalSettings() { Width = 50 });

			var rendered = _testContext.RenderComponent<ToastContainer>();

			var div = rendered.Find("div");
			Assert.IsNotNull(div);

			rendered.WaitForAssertion(() => rendered.MarkupMatches(@"<div style=""max-width: 50px; width: 50px;"" class=""btoast-container position-topright"" >
			  <div class=""btoast-main bnotify-normal-primary"" style=""opacity: 1;
							margin-bottom: 17px;
							box-shadow: 1px 5px 20px 0px #c7c7c7;"" tabindex=""750""  >
				<div class=""btoast-body"" >
				  <div >
					<svg class=""btoast-img"" focusable=""false"" viewBox=""0 0 24 24"" aria-hidden=""true"" >
					  <path fill-rule=""evenodd"" clip-rule=""evenodd"" d=""m18.598 2.865c0-0.552-0.447-1-1-1s-1 0.448-1 1v2c0 0.552 0.447 1 1 1s1-0.448 1-1v-2zm-12 2c0 0.552-0.447 1-1 1s-1-0.448-1-1v-2c0-0.552 0.447-1 1-1s1 0.448 1 1v2zm13 5v10h-16v-10h16zm2-6h-2v1c0 1.103-0.897 2-2 2s-2-0.897-2-2v-1h-8v1c0 1.103-0.897 2-2 2s-2-0.897-2-2v-1h-2v18h20v-18zm-13 10h-2v-2h2v2zm4 0h-2v-2h2v2zm4 0h-2v-2h2v2zm-8 4h-2v-2h2v2zm4 0h-2v-2h2v2zm4 0h-2v-2h2v2z"" ></path>
					</svg>
				  </div>
				  <div class=""btoast-text"" ></div>
				  <button type=""button""  class=""close normal"" >
					<span aria-hidden=""true"" >×</span>
					<span class=""sr-only"" >Close</span>
				  </button>
				</div>
				<div class=""btoast-progress primary start"" style=""transition: width 10s linear;"" ></div>
			  </div>
			</div>"));
		}

		[TestMethod]
		public void Alert_should_not_render_Settings_Position()
		{
			_toastServiceMock.SetupGet(g => g.Toasts).Returns(new ToastSettings[] { new ToastSettings() });
			_toastServiceMock.SetupGet(g => g.GlobalSettings).Returns(new ToastContainerGlobalSettings());

			var rendered = _testContext.RenderComponent<ToastContainer>();

			foreach (var item in Enum.GetValues<ToastPositions>())
			{
				_toastServiceMock.SetupGet(g => g.GlobalSettings).Returns(new ToastContainerGlobalSettings() { Position = item });
				rendered.Render();

				var div = rendered.Find("div");
				Assert.IsNotNull(div);

				rendered.WaitForAssertion(() => rendered.MarkupMatches($@"<div style=""max-width: 400px; width: 400px;"" class=""btoast-container position-{item.ToString().ToLower()}"" >
			  <div class=""btoast-main bnotify-normal-primary"" style=""opacity: 1;
							margin-bottom: 17px;
							box-shadow: 1px 5px 20px 0px #c7c7c7;"" tabindex=""750""  >
				<div class=""btoast-body"" >
				  <div >
					<svg class=""btoast-img"" focusable=""false"" viewBox=""0 0 24 24"" aria-hidden=""true"" >
					  <path fill-rule=""evenodd"" clip-rule=""evenodd"" d=""m18.598 2.865c0-0.552-0.447-1-1-1s-1 0.448-1 1v2c0 0.552 0.447 1 1 1s1-0.448 1-1v-2zm-12 2c0 0.552-0.447 1-1 1s-1-0.448-1-1v-2c0-0.552 0.447-1 1-1s1 0.448 1 1v2zm13 5v10h-16v-10h16zm2-6h-2v1c0 1.103-0.897 2-2 2s-2-0.897-2-2v-1h-8v1c0 1.103-0.897 2-2 2s-2-0.897-2-2v-1h-2v18h20v-18zm-13 10h-2v-2h2v2zm4 0h-2v-2h2v2zm4 0h-2v-2h2v2zm-8 4h-2v-2h2v2zm4 0h-2v-2h2v2zm4 0h-2v-2h2v2z"" ></path>
					</svg>
				  </div>
				  <div class=""btoast-text"" ></div>
				  <button type=""button""  class=""close normal"" >
					<span aria-hidden=""true"" >×</span>
					<span class=""sr-only"" >Close</span>
				  </button>
				</div>
				<div class=""btoast-progress primary start"" style=""transition: width 10s linear;"" ></div>
			  </div>
			</div>"));
			}
		}

		[TestMethod]
		public void Alert_should_not_render_non_Visible_Toasts()
		{
			_toastServiceMock.SetupGet(g => g.Toasts).Returns(new ToastSettings[] { new ToastSettings() { IsVisible = true }, new ToastSettings() { IsVisible = false } });
			_toastServiceMock.SetupGet(g => g.GlobalSettings).Returns(new ToastContainerGlobalSettings());

			var rendered = _testContext.RenderComponent<ToastContainer>();

			var div = rendered.Find("div");
			Assert.IsNotNull(div);

			rendered.WaitForAssertion(() => rendered.MarkupMatches(@"<div style=""max-width: 400px; width: 400px;"" class=""btoast-container position-topright"" >
			  <div class=""btoast-main bnotify-normal-primary"" style=""opacity: 1;
							margin-bottom: 17px;
							box-shadow: 1px 5px 20px 0px #c7c7c7;"" tabindex=""750""  >
				<div class=""btoast-body"" >
				  <div >
					<svg class=""btoast-img"" focusable=""false"" viewBox=""0 0 24 24"" aria-hidden=""true"" >
					  <path fill-rule=""evenodd"" clip-rule=""evenodd"" d=""m18.598 2.865c0-0.552-0.447-1-1-1s-1 0.448-1 1v2c0 0.552 0.447 1 1 1s1-0.448 1-1v-2zm-12 2c0 0.552-0.447 1-1 1s-1-0.448-1-1v-2c0-0.552 0.447-1 1-1s1 0.448 1 1v2zm13 5v10h-16v-10h16zm2-6h-2v1c0 1.103-0.897 2-2 2s-2-0.897-2-2v-1h-8v1c0 1.103-0.897 2-2 2s-2-0.897-2-2v-1h-2v18h20v-18zm-13 10h-2v-2h2v2zm4 0h-2v-2h2v2zm4 0h-2v-2h2v2zm-8 4h-2v-2h2v2zm4 0h-2v-2h2v2zm4 0h-2v-2h2v2z"" ></path>
					</svg>
				  </div>
				  <div class=""btoast-text"" ></div>
				  <button type=""button""  class=""close normal"" >
					<span aria-hidden=""true"" >×</span>
					<span class=""sr-only"" >Close</span>
				  </button>
				</div>
				<div class=""btoast-progress primary start"" style=""transition: width 10s linear;"" ></div>
			  </div>
			</div>"));
		}

		[TestMethod]
		public void Alert_should_not_render_Toast_with_Settings()
		{
			_toastServiceMock.SetupGet(g => g.Toasts).Returns(new ToastSettings[] { new ToastSettings() 
				{ 
					ShowIcon = false,
					NotificationStyle = NotificationStyles.Outlined,
					ShowCloseCountdownProgress = false,
					ShowCloseButton = false
				} 
			});
			_toastServiceMock.SetupGet(g => g.GlobalSettings).Returns(new ToastContainerGlobalSettings() { });

			var rendered = _testContext.RenderComponent<ToastContainer>();

			var div = rendered.Find("div");
			Assert.IsNotNull(div);

			rendered.WaitForAssertion(() => rendered.MarkupMatches(@"<div style=""max-width: 400px; width: 400px;"" class=""btoast-container position-topright"" >
			  <div class=""btoast-main bnotify-outlined-primary"" style=""opacity: 1;
							margin-bottom: 17px;
							box-shadow: 1px 5px 20px 0px #c7c7c7;"" tabindex=""750""  >
				<div class=""btoast-body"" >
				  <div class=""btoast-text"" ></div>
			  </div>
			</div>"));
		}
	}
}