using System.Threading.Tasks;

using Majorsoft.Blazor.Components.CssEvents.Transition;

using Bunit;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using Majorsoft.Blazor.Components.Timer;

namespace Majorsoft.Blazor.Components.Notifications.Tests
{
	[TestClass]
	public class AlertTest
	{
		private Bunit.TestContext _testContext;
		private Mock<ITransitionEventsService> _transitionMock;
		//private Mock<IFocusHandler> _focusHandlerMock;

		[TestInitialize]
		public void Init()
		{
			_testContext = new Bunit.TestContext();

			var mock = new Mock<ILogger<Alert>>();
			var mock2 = new Mock<ILogger<AdvancedTimer>>();
			_transitionMock = new Mock<ITransitionEventsService>();
			//_focusHandlerMock = new Mock<IFocusHandler>();

			_testContext.Services.Add(new ServiceDescriptor(typeof(ILogger<Alert>), mock.Object));
			_testContext.Services.Add(new ServiceDescriptor(typeof(ILogger<AdvancedTimer>), mock2.Object));
			_testContext.Services.Add(new ServiceDescriptor(typeof(ITransitionEventsService), _transitionMock.Object));
			//_testContext.Services.Add(new ServiceDescriptor(typeof(IFocusHandler), _focusHandlerMock.Object));
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
		public async Task Alert_should_rendered_correctly_html_attributes_when_IsVisible()
		{
			var rendered = _testContext.RenderComponent<Alert>(
				("id", "id1"), //HTML attributes
				("title", "text") //HTML attributes
				);

			//Open
			rendered.Instance.IsVisible = true;
			rendered.Render();

			var div = rendered.Find("div");
			Assert.IsNotNull(div);
			Assert.AreEqual(true, rendered.Instance.IsVisible);

			rendered.WaitForAssertion(() => rendered.MarkupMatches(@"<div class=""balert-main bnotify-normal-primary"" style=""opacity: 1;"" tabindex=""750""  >
	  <div class=""balert-body"" id=""id1"" title=""text"" >
		<div >
		  <div ></div>
		  <div class=""balert-text"" ></div>
		</div>
		<button type=""button""  class=""close normal"" >
		  <span aria-hidden=""true"" >×</span>
		  <span class=""sr-only"" >Close</span>
		</button>
	  </div>
	  <div class=""balert-progress primary start"" style=""transition: width 10s linear;"" ></div>
	</div>"));
		}
	}
}
