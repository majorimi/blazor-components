using System.Threading.Tasks;

using Majorsoft.Blazor.Components.CssEvents.Transition;

using Bunit;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

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
			_transitionMock = new Mock<ITransitionEventsService>();
			//_focusHandlerMock = new Mock<IFocusHandler>();

			_testContext.Services.Add(new ServiceDescriptor(typeof(ILogger<Alert>), mock.Object));
			_testContext.Services.Add(new ServiceDescriptor(typeof(ITransitionEventsService), _transitionMock.Object));
			//_testContext.Services.Add(new ServiceDescriptor(typeof(IFocusHandler), _focusHandlerMock.Object));
		}

		[TestCleanup]
		public void Cleanup()
		{
			_testContext?.Dispose();
		}

		[TestMethod]
		public void Alert_should_not_rendered_anything_until_opened()
		{
			var rendered = _testContext.RenderComponent<Alert>(
				("id", "id1"), //HTML attributes
				("title", "text"), //HTML attributes
				(nameof(Alert.AutoClose), false)
				);

			Assert.AreEqual(false, rendered.Instance.IsVisible);
			rendered.MarkupMatches("");
		}
	}
}
