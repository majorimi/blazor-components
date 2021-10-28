using System.Threading.Tasks;

using Bunit;

using Majorsoft.Blazor.Components.CommonTestsBase;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Majorsoft.Blazor.Components.Collapse.Tests
{
	[TestClass]
	public class CollapsePanelTest : ComponentsTestBase<CollapsePanel>
	{
		[TestMethod]
		public void CollapsePanel_should_rendered_correctly_html_attributes()
		{
			var rendered = _testContext.RenderComponent<CollapsePanel>(
				("title", "text") //HTML attributes
				);

			var div = rendered.Find("div");

			Assert.IsNotNull(div);
			Assert.IsTrue(div.HasAttribute("title"));

			var id = div.FirstElementChild.GetAttribute("id");
			div.MarkupMatches(@$"<div class=""collapsePanel"" tabindex=""200"" title=""text""  >
			  <div id=""{id}"" class=""collapseHeader animate"" style=""background-color: rgb(211,211,211);""    ></div>
			  <div class=""collapseContent animate"" style=""opacity: 1; overflow: hidden; max-height: 200px;"" ></div>
			</div>");
		}

		[TestMethod]
		public void CollapsePanel_should_rendered_correctly_Disabled()
		{
			var rendered = _testContext.RenderComponent<CollapsePanel>(parameters => parameters
					.Add(p => p.Disabled, true));

			var div = rendered.Find("div").FirstElementChild;
			Assert.IsNotNull(div);

			var id = div.GetAttribute("id");
			rendered.MarkupMatches(@$"<div class=""collapsePanel"" tabindex=""200"" disabled=""""  >
			  <div id=""{id}"" class=""collapseHeader animate"" style=""background-color: rgb(211,211,211);""    ></div>
			  <div class=""collapseContent animate"" style=""opacity: 1; overflow: hidden; max-height: 200px;"" ></div>
			</div>");
		}

		[TestMethod]
		public void CollapsePanel_should_rendered_correctly_CommonHeader()
		{
			var rendered = _testContext.RenderComponent<CollapsePanel>(parameters => parameters
					.Add(p => p.CommonHeader, "Common header")
					.Add(p => p.ExpandedHeaderContent, "Expanded header")
					.Add(p => p.CollapsedHeaderContent, "Collapsed header"));

			var div = rendered.Find("div").FirstElementChild;
			Assert.IsNotNull(div);

			var id = div.GetAttribute("id");
			rendered.MarkupMatches(@$"<div class=""collapsePanel"" tabindex=""200"" >
			  <div id=""{id}"" class=""collapseHeader animate"" style=""background-color: rgb(211,211,211);"">Common header</div>
			  <div class=""collapseContent animate"" style=""opacity: 1; overflow: hidden; max-height: 200px;"" ></div>
			</div>");

			div.Click();
			rendered.MarkupMatches(@$"<div class=""collapsePanel"" tabindex=""200"" >
			  <div id=""{id}"" class=""collapseHeader animate"" style=""background-color: rgb(211,211,211);"">Common header</div>
			  <div class=""collapseContent animate"" style=""opacity: 0; overflow: hidden; max-height: 0px;"" ></div>
			</div>");
		}

		[TestMethod]
		public void CollapsePanel_should_rendered_correctly_ExpandedHeaderContent()
		{
			var rendered = _testContext.RenderComponent<CollapsePanel>(parameters => parameters
					.Add(p => p.ExpandedHeaderContent, "Expanded header"));

			var div = rendered.Find("div").FirstElementChild;
			Assert.IsNotNull(div);

			var id = div.GetAttribute("id");
			rendered.MarkupMatches(@$"<div class=""collapsePanel"" tabindex=""200"" >
			  <div id=""{id}"" class=""collapseHeader animate"" style=""background-color: rgb(211,211,211);"">Expanded header</div>
			  <div class=""collapseContent animate"" style=""opacity: 1; overflow: hidden; max-height: 200px;"" ></div>
			</div>");

			div.Click();
			rendered.MarkupMatches(@$"<div class=""collapsePanel"" tabindex=""200"" >
			  <div id=""{id}"" class=""collapseHeader animate"" style=""background-color: rgb(211,211,211);""></div>
			  <div class=""collapseContent animate"" style=""opacity: 0; overflow: hidden; max-height: 0px;"" ></div>
			</div>");
		}

		[TestMethod]
		public void CollapsePanel_should_rendered_correctly_CollapsedHeaderContent()
		{
			var rendered = _testContext.RenderComponent<CollapsePanel>(parameters => parameters
					.Add(p => p.CollapsedHeaderContent, "Collapsed header"));

			var div = rendered.Find("div").FirstElementChild;
			Assert.IsNotNull(div);

			var id = div.GetAttribute("id");
			rendered.MarkupMatches(@$"<div class=""collapsePanel"" tabindex=""200"" >
			  <div id=""{id}"" class=""collapseHeader animate"" style=""background-color: rgb(211,211,211);""></div>
			  <div class=""collapseContent animate"" style=""opacity: 1; overflow: hidden; max-height: 200px;"" ></div>
			</div>");

			div.Click();
			rendered.MarkupMatches(@$"<div class=""collapsePanel"" tabindex=""200"" >
			  <div id=""{id}"" class=""collapseHeader animate"" style=""background-color: rgb(211,211,211);"">Collapsed header</div>
			  <div class=""collapseContent animate"" style=""opacity: 0; overflow: hidden; max-height: 0px;"" ></div>
			</div>");
		}

		[TestMethod]
		public void CollapsePanel_should_rendered_correctly_ExpandedColor()
		{
			var rendered = _testContext.RenderComponent<CollapsePanel>(parameters => parameters
					.Add(p => p.ExpandedHeaderContent, "Expanded header")
					.Add(p => p.ExpandedColor, "red"));

			var div = rendered.Find("div").FirstElementChild;
			Assert.IsNotNull(div);

			var id = div.GetAttribute("id");
			rendered.MarkupMatches(@$"<div class=""collapsePanel"" tabindex=""200"" >
			  <div id=""{id}"" class=""collapseHeader animate"" style=""background-color: rgb(255,0,0);"">Expanded header</div>
			  <div class=""collapseContent animate"" style=""opacity: 1; overflow: hidden; max-height: 200px;"" ></div>
			</div>");

			div.Click();
			rendered.MarkupMatches(@$"<div class=""collapsePanel"" tabindex=""200"" >
			  <div id=""{id}"" class=""collapseHeader animate"" style=""background-color: rgb(211,211,211);""></div>
			  <div class=""collapseContent animate"" style=""opacity: 0; overflow: hidden; max-height: 0px;"" ></div>
			</div>");
		}

		[TestMethod]
		public void CollapsePanel_should_rendered_correctly_CollapsedColor()
		{
			var rendered = _testContext.RenderComponent<CollapsePanel>(parameters => parameters
					.Add(p => p.CollapsedHeaderContent, "Collapsed header")
					.Add(p => p.CollapsedColor, "red"));

			var div = rendered.Find("div").FirstElementChild;
			Assert.IsNotNull(div);

			var id = div.GetAttribute("id");
			rendered.MarkupMatches(@$"<div class=""collapsePanel"" tabindex=""200"" >
			  <div id=""{id}"" class=""collapseHeader animate"" style=""background-color: rgb(211,211,211);""></div>
			  <div class=""collapseContent animate"" style=""opacity: 1; overflow: hidden; max-height: 200px;"" ></div>
			</div>");

			div.Click();
			rendered.MarkupMatches(@$"<div class=""collapsePanel"" tabindex=""200"" >
			  <div id=""{id}"" class=""collapseHeader animate"" style=""background-color: rgb(255,0,0);"">Collapsed header</div>
			  <div class=""collapseContent animate"" style=""opacity: 0; overflow: hidden; max-height: 0px;"" ></div>
			</div>");
		}

		[TestMethod]
		public async Task CollapsePanel_should_rendered_correctly_HoverColor()
		{
			var rendered = _testContext.RenderComponent<CollapsePanel>(parameters => parameters
					.Add(p => p.HoverColor, "red"));

			var div = rendered.Find("div").FirstElementChild;
			Assert.IsNotNull(div);

			var id = div.GetAttribute("id");
			rendered.MarkupMatches(@$"<div class=""collapsePanel"" tabindex=""200"" >
			  <div id=""{id}"" class=""collapseHeader animate"" style=""background-color: rgb(211,211,211);""></div>
			  <div class=""collapseContent animate"" style=""opacity: 1; overflow: hidden; max-height: 200px;"" ></div>
			</div>");

			await div.TriggerEventAsync("onmouseenter", new MouseEventArgs());
			rendered.MarkupMatches(@$"<div class=""collapsePanel"" tabindex=""200"" >
			  <div id=""{id}"" class=""collapseHeader animate"" style=""background-color: rgb(255,0,0);""></div>
			  <div class=""collapseContent animate"" style=""opacity: 1; overflow: hidden; max-height: 200px;"" ></div>
			</div>");
			await div.TriggerEventAsync("onmouseleave", new MouseEventArgs());


			div.Click();
			rendered.MarkupMatches(@$"<div class=""collapsePanel"" tabindex=""200"" >
			  <div id=""{id}"" class=""collapseHeader animate"" style=""background-color: rgb(211,211,211);""></div>
			  <div class=""collapseContent animate"" style=""opacity: 0; overflow: hidden; max-height: 0px;"" ></div>
			</div>");

			await div.TriggerEventAsync("onmouseenter", new MouseEventArgs());
			rendered.MarkupMatches(@$"<div class=""collapsePanel"" tabindex=""200"" >
			  <div id=""{id}"" class=""collapseHeader animate"" style=""background-color: rgb(255,0,0);""></div>
			  <div class=""collapseContent animate"" style=""opacity: 0; overflow: hidden; max-height: 0px;"" ></div>
			</div>");
		}

		[TestMethod]
		public void CollapsePanel_should_rendered_correctly_Content()
		{
			var rendered = _testContext.RenderComponent<CollapsePanel>(parameters => parameters
					.Add(p => p.Content, "Content...")
					.Add(p => p.ExpandedHeaderContent, "Expanded header")
					.Add(p => p.CollapsedHeaderContent, "Collapsed header"));

			var div = rendered.Find("div").FirstElementChild;
			Assert.IsNotNull(div);

			var id = div.GetAttribute("id");
			rendered.MarkupMatches(@$"<div class=""collapsePanel"" tabindex=""200"" >
			  <div id=""{id}"" class=""collapseHeader animate"" style=""background-color: rgb(211,211,211);"">Expanded header</div>
			  <div class=""collapseContent animate"" style=""opacity: 1; overflow: hidden; max-height: 200px;"" >Content...</div>
			</div>");

			rendered.SetParametersAndRender(parameters => parameters
					.Add(p => p.Collapsed, true));
			rendered.MarkupMatches(@$"<div class=""collapsePanel"" tabindex=""200"" >
			  <div id=""{id}"" class=""collapseHeader animate"" style=""background-color: rgb(211,211,211);"">Collapsed header</div>
			  <div class=""collapseContent animate"" style=""opacity: 0; overflow: hidden; max-height: 0px;"" >Content...</div>
			</div>");
		}

		[TestMethod]
		public void CollapsePanel_should_rendered_correctly_ContentHeight()
		{
			var rendered = _testContext.RenderComponent<CollapsePanel>(parameters => parameters
					.Add(p => p.ContentHeight, 55));

			var div = rendered.Find("div").FirstElementChild;
			Assert.IsNotNull(div);

			var id = div.GetAttribute("id");
			rendered.MarkupMatches(@$"<div class=""collapsePanel"" tabindex=""200"" >
			  <div id=""{id}"" class=""collapseHeader animate"" style=""background-color: rgb(211,211,211);"" ></div>
			  <div class=""collapseContent animate"" style=""opacity: 1; overflow: hidden; max-height: 55px;"" ></div>
			</div>");
		}

		[TestMethod]
		public void CollapsePanel_should_rendered_correctly_Animate()
		{
			var rendered = _testContext.RenderComponent<CollapsePanel>(parameters => parameters
					.Add(p => p.Animate, false));

			var div = rendered.Find("div").FirstElementChild;
			Assert.IsNotNull(div);

			var id = div.GetAttribute("id");
			rendered.MarkupMatches(@$"<div class=""collapsePanel"" tabindex=""200"" >
			  <div id=""{id}"" class=""collapseHeader"" style=""background-color: rgb(211,211,211);"" ></div>
			  <div class=""collapseContent"" style=""opacity: 1; overflow: hidden; max-height: 200px;"" ></div>
			</div>");
		}

		[TestMethod]
		public void CollapsePanel_should_rendered_correctly_ShowContentOverflow()
		{
			var rendered = _testContext.RenderComponent<CollapsePanel>(parameters => parameters
					.Add(p => p.Animate, false)
					.Add(p => p.ShowContentOverflow, true));

			var div = rendered.Find("div").FirstElementChild;
			Assert.IsNotNull(div);

			var id = div.GetAttribute("id");
			rendered.MarkupMatches(@$"<div class=""collapsePanel"" tabindex=""200"" >
			  <div id=""{id}"" class=""collapseHeader"" style=""background-color: rgb(211,211,211);"" ></div>
			  <div class=""collapseContent"" style=""opacity: 1; overflow: auto; max-height: 200px;"" ></div>
			</div>");
		}
	}
}