using System;

using Bunit;

using Majorsoft.Blazor.Components.CommonTestsBase;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Majorsoft.Blazor.Components.PermaLink.Tests
{
	[TestClass]
	public class PermalinkBlazorWasmInitializerTest : ComponentsTestBase<PermalinkBlazorWasmInitializer>
	{
		private Mock<IPermaLinkWatcherService> _permaLinkWatcherServiceMock;

		[TestInitialize]
		public void Init()
		{
			_permaLinkWatcherServiceMock = new Mock<IPermaLinkWatcherService>();
			_permaLinkWatcherServiceMock.Setup(s => s.WatchPermaLinksAsync());

			_testContext.Services.Add(new ServiceDescriptor(typeof(IPermaLinkWatcherService), _permaLinkWatcherServiceMock.Object));
			_testContext.Services.Add(new ServiceDescriptor(typeof(SingletonComponentService<PermaLinkBlazorServerInitializer>), new SingletonComponentService<PermaLinkBlazorServerInitializer>()));
			_testContext.Services.Add(new ServiceDescriptor(typeof(SingletonComponentService<PermalinkBlazorWasmInitializer>), new SingletonComponentService<PermalinkBlazorWasmInitializer>()));
		}

		[TestMethod]
		public void PermalinkBlazorWasmInitializer_should_not_rendered_Content()
		{
			var rendered = _testContext.Render<PermalinkBlazorWasmInitializer>();
			rendered.MarkupMatches("");

			_permaLinkWatcherServiceMock.Verify(v => v.WatchPermaLinksAsync(), Times.Once);
		}

		[TestMethod]
		public void PermalinkBlazorWasmInitializer_should_not_rendered_mulitple_instances()
		{
			Assert.ThrowsExactly<ApplicationException>(() =>
			{
				var rendered = _testContext.Render<PermalinkBlazorWasmInitializer>();
				var rendered2 = _testContext.Render<PermalinkBlazorWasmInitializer>();
			});
		}
	}
}