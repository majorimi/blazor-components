using System;

using Bunit;

using Majorsoft.Blazor.Components.Common.JsInterop.Scroll;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Majorsoft.Blazor.Components.PermaLink.Tests
{
	[TestClass]
	public class PermaLinkBlazorServerInitializerTest
	{
		private Bunit.TestContext _testContext;
		private Mock<IPermaLinkWatcherService> _permaLinkWatcherServiceMock;
		private Mock<IScrollHandler> _scrollHandlerMock;

		[TestInitialize]
		public void Init()
		{
			_testContext = new Bunit.TestContext();

			var mock = new Mock<ILogger<IPermaLinkWatcherService>>();
			_permaLinkWatcherServiceMock = new Mock<IPermaLinkWatcherService>();
			_scrollHandlerMock = new Mock<IScrollHandler>();

			_testContext.Services.Add(new ServiceDescriptor(typeof(ILogger<IPermaLinkWatcherService>), mock.Object));
			_testContext.Services.Add(new ServiceDescriptor(typeof(IPermaLinkWatcherService), _permaLinkWatcherServiceMock.Object));
			_testContext.Services.Add(new ServiceDescriptor(typeof(IScrollHandler), _scrollHandlerMock.Object));
		}

		[TestCleanup]
		public void Cleanup()
		{
			_testContext?.Dispose();
		}

		[TestMethod]
		public void PermaLinkBlazorServerInitializer_should_not_rendered_Content()
		{
			var rendered = _testContext.RenderComponent<PermaLinkBlazorServerInitializer>();
			rendered.MarkupMatches("");
		}

		[ExpectedException(typeof(ApplicationException))]
		[TestMethod]
		public void PermaLinkBlazorServerInitializer_should_not_rendered_mulitple_instances()
		{
			var rendered = _testContext.RenderComponent<PermaLinkBlazorServerInitializer>();

			var rendered2 = _testContext.RenderComponent<PermaLinkBlazorServerInitializer>();
		}
	}
}