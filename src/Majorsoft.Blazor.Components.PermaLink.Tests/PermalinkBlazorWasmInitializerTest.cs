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
	public class PermalinkBlazorWasmInitializerTest
	{
		private Bunit.TestContext _testContext;
		private Mock<IPermaLinkWatcherService> _permaLinkWatcherServiceMock;

		[TestInitialize]
		public void Init()
		{
			_testContext = new Bunit.TestContext();

			_permaLinkWatcherServiceMock = new Mock<IPermaLinkWatcherService>();
			_permaLinkWatcherServiceMock.Setup(s => s.WatchPermaLinks());


			_testContext.Services.Add(new ServiceDescriptor(typeof(IPermaLinkWatcherService), _permaLinkWatcherServiceMock.Object));
		}

		[TestCleanup]
		public void Cleanup()
		{
			_testContext?.Dispose();
		}

		[TestMethod]
		public void PermalinkBlazorWasmInitializer_should_not_rendered_Content()
		{
			var rendered = _testContext.RenderComponent<PermalinkBlazorWasmInitializer>();
			rendered.MarkupMatches("");

			_permaLinkWatcherServiceMock.Verify(v => v.WatchPermaLinks(), Times.Once);
		}

		[ExpectedException(typeof(ApplicationException))]
		[TestMethod]
		public void PermalinkBlazorWasmInitializer_should_not_rendered_mulitple_instances()
		{
			var rendered = _testContext.RenderComponent<PermalinkBlazorWasmInitializer>();

			var rendered2 = _testContext.RenderComponent<PermalinkBlazorWasmInitializer>();
		}
	}
}