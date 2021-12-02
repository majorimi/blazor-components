using System;

using Bunit;

using Majorsoft.Blazor.Components.Common.JsInterop.Navigation;
using Majorsoft.Blazor.Components.Common.JsInterop.Scroll;
using Majorsoft.Blazor.Components.CommonTestsBase;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Majorsoft.Blazor.Components.PermaLink.Tests
{
	[TestClass]
	public class PermaLinkBlazorServerInitializerTest : ComponentsTestBase<PermaLinkBlazorServerInitializer>
	{
		private Mock<IPermaLinkWatcherService> _permaLinkWatcherServiceMock;
		private Mock<IScrollHandler> _scrollHandlerMock;
		private Mock<INavigationHistoryService> _navigationHistoryServiceMock;

		[TestInitialize]
		public void Init()
		{
			var logger = new Mock<ILogger<IPermaLinkWatcherService>>();
			_permaLinkWatcherServiceMock = new Mock<IPermaLinkWatcherService>();
			_scrollHandlerMock = new Mock<IScrollHandler>();
			_navigationHistoryServiceMock = new Mock<INavigationHistoryService>();

			_testContext.Services.Add(new ServiceDescriptor(typeof(ILogger<IPermaLinkWatcherService>), logger.Object));
			_testContext.Services.Add(new ServiceDescriptor(typeof(IPermaLinkWatcherService), _permaLinkWatcherServiceMock.Object));
			_testContext.Services.Add(new ServiceDescriptor(typeof(IScrollHandler), _scrollHandlerMock.Object));
			_testContext.Services.Add(new ServiceDescriptor(typeof(INavigationHistoryService), _navigationHistoryServiceMock.Object));
			_testContext.Services.Add(new ServiceDescriptor(typeof(SingletonComponentService<PermaLinkBlazorServerInitializer>), new SingletonComponentService<PermaLinkBlazorServerInitializer>()));
			_testContext.Services.Add(new ServiceDescriptor(typeof(SingletonComponentService<PermalinkBlazorWasmInitializer>), new SingletonComponentService<PermalinkBlazorWasmInitializer>()));
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