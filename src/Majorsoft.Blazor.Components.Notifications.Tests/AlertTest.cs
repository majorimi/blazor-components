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
		<svg class=""balert-img"" focusable=""false"" viewBox=""0 0 24 24"" aria-hidden=""true"" >
		  <path fill-rule=""evenodd"" clip-rule=""evenodd"" d=""M17 13h-10v-1h10v1zm0 2h-10v1h10v-1zm0 3h-10v1h10v-1zm5-16v22h-20v-22h3v1c0 1.103.897 2 2 2s2-.897 2-2v-1h6v1c0 1.103.897 2 2 2s2-.897 2-2v-1h3zm-2 7h-16v13h16v-13zm-12-8c0-.552-.447-1-1-1s-1 .448-1 1v2c0 .552.447 1 1 1s1-.448 1-1v-2zm10 0c0-.552-.447-1-1-1s-1 .448-1 1v2c0 .552.447 1 1 1s1-.448 1-1v-2z"" ></path>
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
	}
}
