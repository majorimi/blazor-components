using System.Threading.Tasks;

using Majorsoft.Blazor.Components.CssEvents.Transition;

using Bunit;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using Majorsoft.Blazor.Components.Timer;
using System;

namespace Majorsoft.Blazor.Components.Notifications.Tests
{
	[TestClass]
	public class AlertTest
	{
		private Bunit.TestContext _testContext;
		private Mock<ITransitionEventsService> _transitionMock;

		[TestInitialize]
		public void Init()
		{
			_testContext = new Bunit.TestContext();

			var mock = new Mock<ILogger<Alert>>();
			var mock2 = new Mock<ILogger<AdvancedTimer>>();
			_transitionMock = new Mock<ITransitionEventsService>();

			_testContext.Services.Add(new ServiceDescriptor(typeof(ILogger<Alert>), mock.Object));
			_testContext.Services.Add(new ServiceDescriptor(typeof(ILogger<AdvancedTimer>), mock2.Object));
			_testContext.Services.Add(new ServiceDescriptor(typeof(ITransitionEventsService), _transitionMock.Object));
		}

		[TestCleanup]
		public void Cleanup()
		{
			_testContext?.Dispose();
		}

		[TestMethod]
		public void Alert_should_not_rendered_anything_until_IsVisible()
		{
			var rendered = _testContext.RenderComponent<Alert>(
				("id", "id1"), //HTML attributes
				("title", "text"), //HTML attributes
				(nameof(Alert.AutoClose), false)
				);

			Assert.AreEqual(false, rendered.Instance.IsVisible);
			rendered.MarkupMatches("");
		}

		[TestMethod]
		public void Alert_should_rendered_correctly_html_when_IsVisible()
		{
			var rendered = _testContext.RenderComponent<Alert>(
				("id", "id1"), //HTML attributes
				("title", "text") //HTML attributes
				);

			//Open
			rendered.SetParametersAndRender(parameters => parameters.Add(p => p.IsVisible, true));

			var div = rendered.Find("div");
			Assert.IsNotNull(div);
			Assert.AreEqual(true, rendered.Instance.IsVisible);

			rendered.WaitForAssertion(() => rendered.MarkupMatches(@"<div class=""balert-main bnotify-normal-primary"" style=""opacity: 1;"" tabindex=""750"" id=""id1"" title=""text"" >
		  <div class=""balert-body"" >
			<svg class=""balert-img"" focusable=""false"" viewBox=""0 0 24 24"" aria-hidden=""true"" >
			  <path fill-rule=""evenodd"" clip-rule=""evenodd"" d=""M19 3h-1V1h-2v2H8V1H6v2H5c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h14c1.1 0 2-.9 2-2V5c0-1.1-.9-2-2-2zm0 16H5V9h14v10zM5 7V5h14v2H5zm2 4h10v2H7zm0 4h7v2H7z"" ></path>
			</svg>
			<div class=""balert-text"" ></div>
			<button type=""button""  class=""close normal"" >
			  <span aria-hidden=""true"" >&times;</span>
			  <span class=""sr-only"" >Close</span>
			</button>
		  </div>
		  <div class=""balert-progress primary start"" style=""transition: width 10s linear;"" ></div>
		</div>"));
		}

		[TestMethod]
		public void Alert_should_not_render_ShowIcon_false()
		{
			var rendered = _testContext.RenderComponent<Alert>(parameters => parameters
				.Add(p => p.ShowIcon, false));

			//Open
			rendered.Instance.IsVisible = true;
			rendered.Render();

			var div = rendered.Find("div");
			Assert.IsNotNull(div);
			Assert.AreEqual(true, rendered.Instance.IsVisible);

			rendered.WaitForAssertion(() => rendered.MarkupMatches(@"<div class=""balert-main bnotify-normal-primary"" style=""opacity: 1;"" tabindex=""750""  >
		  <div class=""balert-body"" >
			<div class=""balert-text"" ></div>
			<button type=""button""  class=""close normal"" >
			  <span aria-hidden=""true"" >&times;</span>
			  <span class=""sr-only"" >Close</span>
			</button>
		  </div>
		  <div class=""balert-progress primary start"" style=""transition: width 10s linear;"" ></div>
		</div>"));
		}

		[TestMethod]
		public void Alert_should_not_render_ShowCloseButton_false()
		{
			var rendered = _testContext.RenderComponent<Alert>(parameters => parameters
				.Add(p => p.IsVisible, true)
				.Add(p => p.ShowIcon, false)
				.Add(p => p.ShowCloseButton, false));

			var div = rendered.Find("div");
			Assert.IsNotNull(div);
			Assert.AreEqual(true, rendered.Instance.IsVisible);

			rendered.WaitForAssertion(() => rendered.MarkupMatches(@"<div class=""balert-main bnotify-normal-primary"" style=""opacity: 1;"" tabindex=""750""  >
		  <div class=""balert-body"" >
			<div class=""balert-text"" ></div>
		  </div>
		  <div class=""balert-progress primary start"" style=""transition: width 10s linear;"" ></div>
		</div>"));
		}

		[TestMethod]
		public void Alert_should_render_Content()
		{
			var rendered = _testContext.RenderComponent<Alert>(parameters => parameters
				.Add(p => p.IsVisible, true)
				.Add(p => p.ShowIcon, false)
				.Add(p => p.ShowCloseButton, false)
				.Add(p => p.Content, "<strong>Hi..</strong>"));

			var div = rendered.Find("div");
			Assert.IsNotNull(div);
			Assert.AreEqual(true, rendered.Instance.IsVisible);

			rendered.WaitForAssertion(() => rendered.MarkupMatches(@"<div class=""balert-main bnotify-normal-primary"" style=""opacity: 1;"" tabindex=""750""  >
		  <div class=""balert-body"" >
			<div class=""balert-text"" >
				<strong>Hi..</strong>
			</div>
		  </div>
		  <div class=""balert-progress primary start"" style=""transition: width 10s linear;"" ></div>
		</div>"));
		}

		[TestMethod]
		public void Alert_should_render_CustomIconSvgPath()
		{
			var rendered = _testContext.RenderComponent<Alert>(parameters => parameters
				.Add(p => p.IsVisible, true)
				.Add(p => p.ShowIcon, true)
				.Add(p => p.ShowCloseButton, false)
				.Add(p => p.Content, "<strong>Hi..</strong>")
				.Add(p => p.CustomIconSvgPath, "svg path value"));

			var div = rendered.Find("div");
			Assert.IsNotNull(div);
			Assert.AreEqual(true, rendered.Instance.IsVisible);

			rendered.WaitForAssertion(() => rendered.MarkupMatches(@"<div class=""balert-main bnotify-normal-primary"" style=""opacity: 1;"" tabindex=""750""  >
		  <div class=""balert-body"" >
			<svg class=""balert-img"" focusable=""false"" viewBox=""0 0 24 24"" aria-hidden=""true"" >
			  <path fill-rule=""evenodd"" clip-rule=""evenodd"" d=""svg path value"" ></path>
			</svg>
			<div class=""balert-text"" >
				<strong>Hi..</strong>
			</div>
		  </div>
		  <div class=""balert-progress primary start"" style=""transition: width 10s linear;"" ></div>
		</div>"));
		}

		[TestMethod]
		public void Alert_should_render_Severity_levels()
		{
			var rendered = _testContext.RenderComponent<Alert>(parameters => parameters
				.Add(p => p.IsVisible, true)
				.Add(p => p.ShowIcon, false)
				.Add(p => p.ShowCloseButton, false));

			var div = rendered.Find("div");
			Assert.IsNotNull(div);
			Assert.AreEqual(true, rendered.Instance.IsVisible);

			foreach (var item in Enum.GetValues<SeverityLevel>())
			{
				rendered.SetParametersAndRender(parameters => parameters.Add(p => p.Severity, item));

				rendered.WaitForAssertion(() => rendered.MarkupMatches($@"<div class=""balert-main bnotify-normal-{item.ToString().ToLower()}"" style=""opacity: 1;"" tabindex=""750""  >
			  <div class=""balert-body"" >
				<div class=""balert-text"" >
				</div>
			  </div>
			  <div class=""balert-progress {item.ToString().ToLower()} start"" style=""transition: width 10s linear;"" ></div>
			</div>"));
			}
		}

		[TestMethod]
		public void Alert_should_render_NotificationStyles()
		{
			var rendered = _testContext.RenderComponent<Alert>(parameters => parameters
				.Add(p => p.IsVisible, true)
				.Add(p => p.ShowIcon, false)
				.Add(p => p.ShowCloseButton, false));

			var div = rendered.Find("div");
			Assert.IsNotNull(div);
			Assert.AreEqual(true, rendered.Instance.IsVisible);

			foreach (var severity in Enum.GetValues<SeverityLevel>())
			{
				rendered.SetParametersAndRender(parameters => parameters.Add(p => p.Severity, severity));

				foreach (var style in Enum.GetValues<NotificationStyles>())
				{
					rendered.SetParametersAndRender(parameters => parameters
						.Add(p => p.NotificationStyle, style));

					var progress = style != NotificationStyles.Strong ? $" { severity.ToString().ToLower()}" : " strong";

					rendered.WaitForAssertion(() => rendered.MarkupMatches($@"<div class=""balert-main bnotify-{style.ToString().ToLower()}-{severity.ToString().ToLower()}"" style=""opacity: 1;"" tabindex=""750""  >
				  <div class=""balert-body"" >
					<div class=""balert-text"" >
					</div>
				  </div>
				  <div class=""balert-progress {progress} start"" style=""transition: width 10s linear;"" ></div>
				</div>"));
				}
			}
		}

		[TestMethod]
		public void Alert_should_not_AutoClose()
		{
			var rendered = _testContext.RenderComponent<Alert>(parameters => parameters
				.Add(p => p.IsVisible, true)
				.Add(p => p.ShowIcon, false)
				.Add(p => p.ShowCloseButton, false)
				.Add(p => p.AutoClose, false)
				.Add(p => p.AutoCloseInSec, (uint)1)
				.Add(p => p.Content, "<strong>Hi..</strong>"));

			var div = rendered.Find("div");
			Assert.IsNotNull(div);
			Assert.AreEqual(true, rendered.Instance.IsVisible);

			rendered.WaitForAssertion(() => rendered.MarkupMatches(@"<div class=""balert-main bnotify-normal-primary"" style=""opacity: 1;"" tabindex=""750""  >
		  <div class=""balert-body"" >
			<div class=""balert-text"" >
				<strong>Hi..</strong>
			</div>
		  </div>
		</div>"));
		}

		[Ignore] //TODO: fix later
		[TestMethod]
		public async Task Alert_should_AutoClose()
		{
			_transitionMock.Setup(s => s.RegisterTransitionEndedAsync(It.IsAny<ElementReference>(), It.IsAny<Func<TransitionEventArgs, Task>>(), It.IsAny<string>()))
				.Returns(Task.CompletedTask);

			var rendered = _testContext.RenderComponent<Alert>(parameters => parameters
				.Add(p => p.IsVisible, true)
				.Add(p => p.ShowIcon, false)
				.Add(p => p.ShowCloseButton, false)
				.Add(p => p.AutoClose, true)
				.Add(p => p.AutoCloseInSec, (uint)1)
				.Add(p => p.Content, "<strong>Hi..</strong>"));

			var div = rendered.Find("div");
			Assert.IsNotNull(div);
			Assert.AreEqual(true, rendered.Instance.IsVisible);

			rendered.WaitForAssertion(() => rendered.MarkupMatches(@"<div class=""balert-main bnotify-normal-primary"" style=""opacity: 0;"" tabindex=""750""  >
		  <div class=""balert-body"" >
			<div class=""balert-text"" >
				<strong>Hi..</strong>
			</div>
		  </div>
		  <div class=""balert-progress primary start"" style=""transition: width 1s linear;"" ></div>
		</div>"), TimeSpan.FromSeconds(2)); //

			rendered.Render();

			rendered.WaitForAssertion(() =>
			{
				rendered.MarkupMatches("");
			}, TimeSpan.FromSeconds(2));
		}
	}
}
