using System;
using System.Threading.Tasks;

using Majorsoft.Blazor.Components.Common.JsInterop.Clipboard;

using Bunit;
using Bunit.TestDoubles;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Majorsoft.Blazor.Components.PermaLink.Tests
{
	[TestClass]
	public class PermaLinkElementTest
	{
		private Bunit.TestContext _testContext;
		private Mock<IClipboardHandler> _clipboardJsMock;

		[TestInitialize]
		public void Init()
		{
			_testContext = new Bunit.TestContext();

			var mock = new Mock<ILogger<PermaLinkElement>>();
			_clipboardJsMock = new Mock<IClipboardHandler>();

			_testContext.Services.AddMockJSRuntime(JSRuntimeMockMode.Strict);
			_testContext.Services.Add(new ServiceDescriptor(typeof(ILogger<PermaLinkElement>), mock.Object));
			_testContext.Services.Add(new ServiceDescriptor(typeof(IClipboardHandler), _clipboardJsMock.Object));
		}

		[TestCleanup]
		public void Cleanup()
		{
			_testContext?.Dispose();
		}

		[TestMethod]
		public void PermaLinkElement_should_rendered_correctly_html_attributes()
		{
			//Test does not work unit new bUnit version
			//https://github.com/egil/bUnit/issues/231

			//var rendered = _testContext.RenderComponent<PermaLinkElement>(
			//	("title", "t"), //HTML attributes
			//	("style", "style") //HTML attributes
			//	);

			//var input = rendered.Find("div");

			//Assert.IsNotNull(input);
			//input.MarkupMatches(@"<input id=""id1"" class=""form-control w-100"" />");
		}
	}
}