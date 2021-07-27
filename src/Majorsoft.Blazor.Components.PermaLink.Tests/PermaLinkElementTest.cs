using Majorsoft.Blazor.Components.Common.JsInterop.Clipboard;
using Majorsoft.Blazor.Components.Common.JsInterop.ElementInfo;

using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using Majorsoft.Blazor.Components.CommonTestsBase;

namespace Majorsoft.Blazor.Components.PermaLink.Tests
{
	[TestClass]
	public class PermaLinkElementTest : ComponentsTestBase<PermaLinkElement>
	{
		private Mock<IClipboardHandler> _clipboardJsMock;
		private BunitJSModuleInterop _jsInteropModul;

		[TestInitialize]
		public void Init()
		{
			_clipboardJsMock = new Mock<IClipboardHandler>();

			_testContext.JSInterop.Mode = JSRuntimeMode.Strict;

#if DEBUG
			_jsInteropModul = _testContext.JSInterop.SetupModule("./_content/Majorsoft.Blazor.Components.Common.JsInterop/elementInfo.js");
#else
			_jsInteropModul = _testContext.JSInterop.SetupModule("./_content/Majorsoft.Blazor.Components.Common.JsInterop/elementInfo.min.js");
#endif
			_jsInteropModul.Setup<DomRect>("getBoundingClientRect", _ => true).SetResult(new DomRect());

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
			var rendered = _testContext.RenderComponent<PermaLinkElement>(
				("title", "Test"), //HTML attributes
				("style", "style") //HTML attributes
				);

			var input = rendered.Find("div");

			Assert.IsNotNull(input);
			input.MarkupMatches(@$"<div id=""{input.Id}"" tabindex=""1000"" class=""permaDivRight"" title=""Test"" style=""style""><a></a></div>");
		}

		[TestMethod]
		public void PermaLinkElement_should_rendered_Content_correctly()
		{
			var rendered = _testContext.RenderComponent<PermaLinkElement>(parameters => parameters
					.Add(p => p.Content, "<h2>Hower over</h2>"));

			var input = rendered.Find("div");

			Assert.IsNotNull(input);
			input.MarkupMatches(@$"<div id=""{input.Id}"" tabindex=""1000"" class=""permaDivRight""><a></a> <h2>Hower over</h2> </div>");
		}

		[TestMethod]
		public void PermaLinkElement_should_rendered_PermaLinkName_correctly()
		{
			var rendered = _testContext.RenderComponent<PermaLinkElement>(parameters => parameters
					.Add(p => p.PermaLinkName, "#linkName"));

			var input = rendered.Find("div");

			Assert.IsNotNull(input);
			input.MarkupMatches(@$"<div id=""{input.Id}"" tabindex=""1000"" class=""permaDivRight""><a name=""#linkName""></a> </div>");
		}

		[TestMethod]
		public void PermaLinkElement_should_rendered_IconMarginTop_correctly()
		{
			var rendered = _testContext.RenderComponent<PermaLinkElement>(parameters => parameters
					.Add(p => p.ShowIcon, ShowPermaLinkIcon.Always)
					.Add(p => p.IconMarginTop, 8));

			var input = rendered.Find("div");

			Assert.IsNotNull(input);
			input.MarkupMatches(@$"<div id=""{input.Id}"" tabindex=""1000"" class=""permaDivRight""><a></a> <img style=""margin-top: 8px;"" width=""16"" height=""16"" class=""permaLinkIcon"" src=""_content/Majorsoft.Blazor.Components.PermaLink/link2.svg""> </div>");
		}

		[TestMethod]
		public void PermaLinkElement_should_rendered_IconSize_correctly()
		{
			var rendered = _testContext.RenderComponent<PermaLinkElement>(parameters => parameters
					.Add(p => p.ShowIcon, ShowPermaLinkIcon.Always)
					.Add(p => p.IconSize, 34));

			var input = rendered.Find("div");

			Assert.IsNotNull(input);
			input.MarkupMatches(@$"<div id=""{input.Id}"" tabindex=""1000"" class=""permaDivRight""><a></a> <img style=""margin-top: 0px;"" width=""34"" height=""34"" class=""permaLinkIcon"" src=""_content/Majorsoft.Blazor.Components.PermaLink/link2.svg""> </div>");
		}

		[TestMethod]
		public void PermaLinkElement_should_rendered_IconPosition_correctly()
		{
			foreach (var item in Enum.GetValues<PermaLinkIconPosition>())
			{
				var rendered = _testContext.RenderComponent<PermaLinkElement>(parameters => parameters
					.Add(p => p.ShowIcon, ShowPermaLinkIcon.Always)
					.Add(p => p.IconPosition, item));

				//rendered.SetParametersAndRender(parameters => parameters.Add(p => p.IconPosition, item));
				var input = rendered.Find("div");

				Assert.IsNotNull(input);

				var imgStyle = item == PermaLinkIconPosition.Left ? "left: 0px; position: absolute;" : "margin-top: 0px;";
				input.MarkupMatches(@$"<div id=""{input.Id}"" tabindex=""1000"" class=""permaDiv{item}""><a></a> <img style=""{imgStyle} margin-top: 0px;"" width=""16"" height=""16"" class=""permaLinkIcon"" src=""_content/Majorsoft.Blazor.Components.PermaLink/link2.svg""> </div>");
			}
		}

		[TestMethod]
		public void PermaLinkElement_should_rendered_IconStyle_correctly()
		{
			foreach (var item in Enum.GetValues<PermaLinkStyle>())
			{
				var rendered = _testContext.RenderComponent<PermaLinkElement>(parameters => parameters
					.Add(p => p.ShowIcon, ShowPermaLinkIcon.Always)
					.Add(p => p.IconStyle, item));

				var input = rendered.Find("div");

				Assert.IsNotNull(input);

				var icon = item == PermaLinkStyle.Normal ? "link2.svg" : "link.svg";
				input.MarkupMatches(@$"<div id=""{input.Id}"" tabindex=""1000"" class=""permaDivRight""><a></a> <img style=""margin-top: 0px;"" width=""16"" height=""16"" class=""permaLinkIcon"" src=""_content/Majorsoft.Blazor.Components.PermaLink/{icon}""> </div>");
			}
		}

		[TestMethod]
		public void PermaLinkElement_should_rendered_ShowPermaLinkIcon_No_correctly()
		{
			var rendered = _testContext.RenderComponent<PermaLinkElement>(parameters => parameters
					.Add(p => p.ShowIcon, ShowPermaLinkIcon.No));

			var input = rendered.Find("div");

			Assert.IsNotNull(input);
			input.MarkupMatches(@$"<div id=""{input.Id}"" tabindex=""1000"" class=""""><a></a> </div>");
		}
		[TestMethod]
		public void PermaLinkElement_should_rendered_ShowPermaLinkIcon_Always_correctly()
		{
			var rendered = _testContext.RenderComponent<PermaLinkElement>(parameters => parameters
					.Add(p => p.ShowIcon, ShowPermaLinkIcon.Always));

			var input = rendered.Find("div");

			Assert.IsNotNull(input);
			input.MarkupMatches(@$"<div id=""{input.Id}"" tabindex=""1000"" class=""permaDivRight""><a></a> <img style=""margin-top: 0px;"" width=""16"" height=""16"" class=""permaLinkIcon"" src=""_content/Majorsoft.Blazor.Components.PermaLink/link2.svg""> </div>");
		}
		[TestMethod]
		public async Task PermaLinkElement_should_rendered_ShowPermaLinkIcon_OnHover_correctly()
		{
			var rendered = _testContext.RenderComponent<PermaLinkElement>(parameters => parameters
					.Add(p => p.ShowIcon, ShowPermaLinkIcon.OnHover));

			var input = rendered.Find("div");

			Assert.IsNotNull(input);
			input.MarkupMatches(@$"<div id=""{input.Id}"" tabindex=""1000"" class=""permaDivRight""><a></a> </div>");

			await input.TriggerEventAsync("onmouseenter", new MouseEventArgs());
			rendered.WaitForAssertion(() =>
			{
				input.MarkupMatches(@$"<div id=""{input.Id}"" tabindex=""1000"" class=""permaDivRight""><a></a> <img style=""margin-top: 0px;"" width=""16"" height=""16"" class=""permaLinkIcon"" src=""_content/Majorsoft.Blazor.Components.PermaLink/link2.svg""> </div>");
			});

			await input.TriggerEventAsync("onmouseleave", new MouseEventArgs());
			rendered.WaitForAssertion(() =>
			{
				input.MarkupMatches(@$"<div id=""{input.Id}"" tabindex=""1000"" class=""permaDivRight""><a></a> </div>");
			});
		}
	}
}