using System;
using Bunit;
using Majorsoft.Blazor.Components.CommonTestsBase;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Majorsoft.Blazor.Components.PermaLink.Tests;

[TestClass]
public class PermalinkBlazorWasmInitializerTest : ComponentsTestBase<PermalinkBlazorWasmInitializer>
{
	private IPermaLinkWatcherService _permaLinkWatcherServiceMock;

	[TestInitialize]
	public void Init()
	{
		_permaLinkWatcherServiceMock = Substitute.For<IPermaLinkWatcherService>();
		_permaLinkWatcherServiceMock.When(s => s.WatchPermaLinks()).DoNotCallBase();

		_testContext.Services.Add(
			new ServiceDescriptor(typeof(IPermaLinkWatcherService), _permaLinkWatcherServiceMock)
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
	public void PermalinkBlazorWasmInitializer_should_not_rendered_Content()
	{
		var rendered = _testContext.RenderComponent<PermalinkBlazorWasmInitializer>();
		rendered.MarkupMatches("");

		_permaLinkWatcherServiceMock.Received(1).WatchPermaLinks();
	}

	[ExpectedException(typeof(ApplicationException))]
	[TestMethod]
	public void PermalinkBlazorWasmInitializer_should_not_rendered_mulitple_instances()
	{
		var rendered = _testContext.RenderComponent<PermalinkBlazorWasmInitializer>();

		var rendered2 = _testContext.RenderComponent<PermalinkBlazorWasmInitializer>();
	}
}
