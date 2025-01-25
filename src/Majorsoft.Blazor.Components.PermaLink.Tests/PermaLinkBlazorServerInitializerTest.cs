using System;
using Bunit;
using Majorsoft.Blazor.Components.Common.JsInterop.Navigation;
using Majorsoft.Blazor.Components.Common.JsInterop.Scroll;
using Majorsoft.Blazor.Components.CommonTestsBase;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Majorsoft.Blazor.Components.PermaLink.Tests;

[TestClass]
public class PermaLinkBlazorServerInitializerTest
	: ComponentsTestBase<PermaLinkBlazorServerInitializer>
{
	private IPermaLinkWatcherService _permaLinkWatcherServiceMock;
	private IScrollHandler _scrollHandlerMock;
	private INavigationHistoryService _navigationHistoryServiceMock;

	[TestInitialize]
	public void Init()
	{
		var logger = Substitute.For<ILogger<IPermaLinkWatcherService>>();
		_permaLinkWatcherServiceMock = Substitute.For<IPermaLinkWatcherService>();
		_scrollHandlerMock = Substitute.For<IScrollHandler>();
		_navigationHistoryServiceMock = Substitute.For<INavigationHistoryService>();

		_testContext.Services.Add(
			new ServiceDescriptor(typeof(ILogger<IPermaLinkWatcherService>), logger)
		);
		_testContext.Services.Add(
			new ServiceDescriptor(typeof(IPermaLinkWatcherService), _permaLinkWatcherServiceMock)
		);
		_testContext.Services.Add(
			new ServiceDescriptor(typeof(IScrollHandler), _scrollHandlerMock)
		);
		_testContext.Services.Add(
			new ServiceDescriptor(typeof(INavigationHistoryService), _navigationHistoryServiceMock)
		);
		_testContext.Services.Add(
			new ServiceDescriptor(
				typeof(SingletonComponentService<PermaLinkBlazorServerInitializer>),
				new SingletonComponentService<PermaLinkBlazorServerInitializer>()
			)
		);
		_testContext.Services.Add(
			new ServiceDescriptor(
				typeof(SingletonComponentService<PermalinkBlazorWasmInitializer>),
				new SingletonComponentService<PermalinkBlazorWasmInitializer>()
			)
		);
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
