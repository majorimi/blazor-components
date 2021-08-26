using System;
using System.Threading.Tasks;

using Bunit;

using Majorsoft.Blazor.Components.CommonTestsBase;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Majorsoft.Blazor.Components.Tabs.Tests
{
	[TestClass]
	public class TabsPanelTest : ComponentsTestBase<TabsPanel>
	{
		[TestInitialize]
		public void Init()
		{
			var logger = new Mock<ILogger<TabItem>>();
			_testContext.Services.Add(new ServiceDescriptor(typeof(ILogger<TabItem>), logger.Object));
		}

		[TestMethod]
		public void TabsPanel_should_rendered_correctly_html_attributes()
		{
			var rendered = _testContext.RenderComponent<TabsPanel>(
				("title", "text") //HTML attributes
				);

			var div = rendered.Find("div");

			Assert.IsNotNull(div);
			Assert.IsNotNull(div.Id);
			Assert.IsTrue(div.HasAttribute("title"));

			var id = div.GetAttribute("id");
			div.MarkupMatches(@$"<div id=""{id}"" class=""tabsPanel"" tabindex=""200"" title=""text""  >
				  <div class=""tabsHeader left"" ></div>
				</div>");
		}

		[TestMethod]
		public void TabsPanel_should_rendered_Disabled_correctly()
		{
			var rendered = _testContext.RenderComponent<TabsPanel>(parameters => parameters
					.Add(p => p.Disabled, true));

			var div = rendered.Find("div");

			Assert.IsNotNull(div);
			Assert.IsTrue(div.HasAttribute("disabled"));
			Assert.AreEqual(0, rendered.Instance.TabCount);
		}

		[TestMethod]
		public void TabsPanel_should_add_TabItem_correctly()
		{
			var rendered = _testContext.RenderComponent<TabsPanel>(parameters => parameters
					.Add(p => p.Disabled, true));

			var btn1 = _testContext.RenderComponent<TabItem>(parameters => parameters
					.Add(p => p.Parent, rendered.Instance));
			var btn2 = _testContext.RenderComponent<TabItem>(parameters => parameters
					.Add(p => p.Parent, rendered.Instance));

			var div = rendered.Find("div");

			Assert.IsNotNull(div);
			Assert.IsTrue(div.HasAttribute("disabled"));
			Assert.AreEqual(2, rendered.Instance.TabCount);
		}

		[TestMethod]
		public void TabsPanel_should_render_correct_html()
		{
			var rendered = _testContext.RenderComponent<TabsPanel>(parameters => parameters
					.Add(p => p.TabItemsHeight, 20)
					.Add(p => p.TabItemsWidth, 200));

			var btn1 = _testContext.RenderComponent<TabItem>(parameters => parameters
					.Add(p => p.Parent, rendered.Instance));
			var btn2 = _testContext.RenderComponent<TabItem>(parameters => parameters
					.Add(p => p.Parent, rendered.Instance));

			var div = rendered.Find("div");
			Assert.IsNotNull(div);

			var id = div.GetAttribute("id");
			div.MarkupMatches(@$"<div id=""{id}"" class=""tabsPanel"" tabindex=""200""  >
			  <div class=""tabsHeader left"" >
				<button type=""button"" parent=""{id}""  class=""tabItem active animate"" style=""width: 200px; height: 20px; background-color: rgb(211,211,211);""   ></button>
				<button type=""button"" parent=""{id}""  class=""tabItem animate"" style=""width: 200px; height: 20px; background-color: rgb(255, 255, 255);""   ></button>
			  </div>
			</div>");
		}

		[TestMethod]
		public void TabsPanel_should_render_correct_ActiveColor()
		{
			var rendered = _testContext.RenderComponent<TabsPanel>(parameters => parameters
					.Add(p => p.ActiveColor, "red"));

			var btn1 = _testContext.RenderComponent<TabItem>(parameters => parameters
					.Add(p => p.Parent, rendered.Instance));
			var btn2 = _testContext.RenderComponent<TabItem>(parameters => parameters
					.Add(p => p.Parent, rendered.Instance));

			var div = rendered.Find("div");
			Assert.IsNotNull(div);

			var id = div.GetAttribute("id");
			div.MarkupMatches(@$"<div id=""{id}"" class=""tabsPanel"" tabindex=""200""  >
			  <div class=""tabsHeader left"" >
				<button type=""button"" parent=""{id}""  class=""tabItem active animate"" style=""width: 100px; height: 40px; background-color: rgb(255, 0, 0);""   ></button>
				<button type=""button"" parent=""{id}""  class=""tabItem animate"" style=""width: 100px; height: 40px; background-color: rgb(255, 255, 255);""   ></button>
			  </div>
			</div>");
		}

		[TestMethod]
		public void TabsPanel_should_render_correct_InactiveColor()
		{
			var rendered = _testContext.RenderComponent<TabsPanel>(parameters => parameters
					.Add(p => p.InactiveColor, "red"));

			var btn1 = _testContext.RenderComponent<TabItem>(parameters => parameters
					.Add(p => p.Parent, rendered.Instance));
			var btn2 = _testContext.RenderComponent<TabItem>(parameters => parameters
					.Add(p => p.Parent, rendered.Instance));

			var div = rendered.Find("div");
			Assert.IsNotNull(div);

			var id = div.GetAttribute("id");
			div.MarkupMatches(@$"<div id=""{id}"" class=""tabsPanel"" tabindex=""200""  >
			  <div class=""tabsHeader left"" >
				<button type=""button"" parent=""{id}""  class=""tabItem active animate"" style=""width: 100px; height: 40px; background-color: rgb(211,211,211);""   ></button>
				<button type=""button"" parent=""{id}""  class=""tabItem animate"" style=""width: 100px; height: 40px; background-color: rgb(255, 0, 0);""   ></button>
			  </div>
			</div>");
		}

		[TestMethod]
		public async Task TabsPanel_should_render_correct_HoverColor_on_active_Tab()
		{
			var rendered = _testContext.RenderComponent<TabsPanel>(parameters => parameters
					.Add(p => p.HoverColor, "red"));

			var btn1 = _testContext.RenderComponent<TabItem>(parameters => parameters
					.Add(p => p.Parent, rendered.Instance));
			var btn2 = _testContext.RenderComponent<TabItem>(parameters => parameters
					.Add(p => p.Parent, rendered.Instance));

			var div = rendered.Find("div");
			Assert.IsNotNull(div);

			var id = div.GetAttribute("id");
			div.MarkupMatches(@$"<div id=""{id}"" class=""tabsPanel"" tabindex=""200""  >
			  <div class=""tabsHeader left"" >
				<button type=""button"" parent=""{id}""  class=""tabItem active animate"" style=""width: 100px; height: 40px; background-color: rgb(211,211,211);""></button>
				<button type=""button"" parent=""{id}""  class=""tabItem animate"" style=""width: 100px; height: 40px; background-color: rgb(255, 255, 255);""></button>
			  </div>
			</div>");

			var buttons = rendered.FindAll("button");
			await buttons[0].TriggerEventAsync("onmouseenter", new MouseEventArgs()); //active tab
			rendered.WaitForAssertion(() =>
			{
				div.MarkupMatches(@$"<div id=""{id}"" class=""tabsPanel"" tabindex=""200""  >
				  <div class=""tabsHeader left"" >
					<button type=""button"" parent=""{id}""  class=""tabItem active animate"" style=""width: 100px; height: 40px; background-color: rgb(211,211,211);""></button>
					<button type=""button"" parent=""{id}""  class=""tabItem animate"" style=""width: 100px; height: 40px; background-color: rgb(255, 255, 255);""></button>
				  </div>
				</div>");
			});
			await buttons[0].TriggerEventAsync("onmouseleave", new MouseEventArgs());
		}

		[TestMethod]
		public async Task TabsPanel_should_render_correct_HoverColor_on_inactive_Tab()
		{
			var rendered = _testContext.RenderComponent<TabsPanel>(parameters => parameters
					.Add(p => p.HoverColor, "red"));

			var btn1 = _testContext.RenderComponent<TabItem>(parameters => parameters
					.Add(p => p.Parent, rendered.Instance));
			var btn2 = _testContext.RenderComponent<TabItem>(parameters => parameters
					.Add(p => p.Parent, rendered.Instance));

			var div = rendered.Find("div");
			Assert.IsNotNull(div);

			var id = div.GetAttribute("id");
			div.MarkupMatches(@$"<div id=""{id}"" class=""tabsPanel"" tabindex=""200""  >
			  <div class=""tabsHeader left"" >
				<button type=""button"" parent=""{id}""  class=""tabItem active animate"" style=""width: 100px; height: 40px; background-color: rgb(211,211,211);""></button>
				<button type=""button"" parent=""{id}""  class=""tabItem animate"" style=""width: 100px; height: 40px; background-color: rgb(255, 255, 255);""></button>
			  </div>
			</div>");

			var buttons = rendered.FindAll("button");
			await buttons[1].TriggerEventAsync("onmouseenter", new MouseEventArgs()); //inactive tab
			rendered.WaitForAssertion(() =>
			{
				div.MarkupMatches(@$"<div id=""{id}"" class=""tabsPanel"" tabindex=""200""  >
				  <div class=""tabsHeader left"" >
					<button type=""button"" parent=""{id}""  class=""tabItem active animate"" style=""width: 100px; height: 40px; background-color: rgb(211,211,211);""></button>
					<button type=""button"" parent=""{id}""  class=""tabItem animate"" style=""width: 100px; height: 40px; background-color: rgb(255, 0, 0);""></button>
				  </div>
				</div>");
			});
			await buttons[1].TriggerEventAsync("onmouseleave", new MouseEventArgs());
		}

		[TestMethod]
		public void TabsPanel_should_render_non_Animate()
		{
			var rendered = _testContext.RenderComponent<TabsPanel>(parameters => parameters
					.Add(p => p.Animate, false));

			var btn1 = _testContext.RenderComponent<TabItem>(parameters => parameters
					.Add(p => p.Parent, rendered.Instance));
			var btn2 = _testContext.RenderComponent<TabItem>(parameters => parameters
					.Add(p => p.Parent, rendered.Instance));

			var div = rendered.Find("div");
			Assert.IsNotNull(div);

			var id = div.GetAttribute("id");
			div.MarkupMatches(@$"<div id=""{id}"" class=""tabsPanel"" tabindex=""200""  >
			  <div class=""tabsHeader left"" >
				<button type=""button"" parent=""{id}""  class=""tabItem active"" style=""width: 100px; height: 40px; background-color: rgb(211,211,211);""   ></button>
				<button type=""button"" parent=""{id}""  class=""tabItem"" style=""width: 100px; height: 40px; background-color: rgb(255, 255, 255);""   ></button>
			  </div>
			</div>");
		}

		[TestMethod]
		public void TabsPanel_should_render_correct_TabItemsHeight()
		{
			var rendered = _testContext.RenderComponent<TabsPanel>(parameters => parameters
					.Add(p => p.TabItemsHeight, 0));

			var btn1 = _testContext.RenderComponent<TabItem>(parameters => parameters
					.Add(p => p.Parent, rendered.Instance));
			var btn2 = _testContext.RenderComponent<TabItem>(parameters => parameters
					.Add(p => p.Parent, rendered.Instance));

			var div = rendered.Find("div");
			Assert.IsNotNull(div);

			var id = div.GetAttribute("id");
			div.MarkupMatches(@$"<div id=""{id}"" class=""tabsPanel"" tabindex=""200""  >
			  <div class=""tabsHeader left"" >
				<button type=""button"" parent=""{id}""  class=""tabItem active animate"" style=""width: 100px; height: auto; background-color: rgb(211,211,211);""   ></button>
				<button type=""button"" parent=""{id}""  class=""tabItem animate"" style=""width: 100px; height: auto; background-color: rgb(255, 255, 255);""   ></button>
			  </div>
			</div>");
		}

		[TestMethod]
		public void TabsPanel_should_render_correct_TabItemsWidth()
		{
			var rendered = _testContext.RenderComponent<TabsPanel>(parameters => parameters
					.Add(p => p.TabItemsWidth, 0));

			var btn1 = _testContext.RenderComponent<TabItem>(parameters => parameters
					.Add(p => p.Parent, rendered.Instance));
			var btn2 = _testContext.RenderComponent<TabItem>(parameters => parameters
					.Add(p => p.Parent, rendered.Instance));

			var div = rendered.Find("div");
			Assert.IsNotNull(div);

			var id = div.GetAttribute("id");
			div.MarkupMatches(@$"<div id=""{id}"" class=""tabsPanel"" tabindex=""200""  >
			  <div class=""tabsHeader left"" >
				<button type=""button"" parent=""{id}""  class=""tabItem active animate"" style=""width: auto; height: 40px; background-color: rgb(211,211,211);""   ></button>
				<button type=""button"" parent=""{id}""  class=""tabItem animate"" style=""width: auto; height: 40px; background-color: rgb(255, 255, 255);""   ></button>
			  </div>
			</div>");
		}

		[TestMethod]
		public async Task TabsPanel_should_render_correct_active_Tab_on_click()
		{
			var rendered = _testContext.RenderComponent<TabsPanel>();

			var btn1 = _testContext.RenderComponent<TabItem>(parameters => parameters
					.Add(p => p.Parent, rendered.Instance));
			var btn2 = _testContext.RenderComponent<TabItem>(parameters => parameters
					.Add(p => p.Parent, rendered.Instance));

			var div = rendered.Find("div");
			Assert.IsNotNull(div);

			var id = div.GetAttribute("id");
			div.MarkupMatches(@$"<div id=""{id}"" class=""tabsPanel"" tabindex=""200""  >
			  <div class=""tabsHeader left"" >
				<button type=""button"" parent=""{id}""  class=""tabItem active animate"" style=""width: 100px; height: 40px; background-color: rgb(211,211,211);""   ></button>
				<button type=""button"" parent=""{id}""  class=""tabItem animate"" style=""width: 100px; height: 40px; background-color: rgb(255, 255, 255);""   ></button>
			  </div>
			</div>");

			var buttons = rendered.FindAll("button");
			await buttons[1].TriggerEventAsync("onclick", new MouseEventArgs()); //inactive tab
			rendered.WaitForAssertion(() =>
			{
				div.MarkupMatches(@$"<div id=""{id}"" class=""tabsPanel"" tabindex=""200""  >
				  <div class=""tabsHeader left"" >
					<button type=""button"" parent=""{id}""  class=""tabItem animate"" style=""width: 100px; height: 40px; background-color: rgb(255, 255, 255);""   ></button>
					<button type=""button"" parent=""{id}""  class=""tabItem active animate"" style=""width: 100px; height: 40px; background-color: rgb(211,211,211);""   ></button>
				  </div>
				</div>");
			});
		}

		[TestMethod]
		public void TabsPanel_should_render_correct_TabPositon()
		{
			var rendered = _testContext.RenderComponent<TabsPanel>(parameters => parameters
					.Add(p => p.TabPositon, TabPositons.Left));

			var btn1 = _testContext.RenderComponent<TabItem>(parameters => parameters
					.Add(p => p.Parent, rendered.Instance));
			var btn2 = _testContext.RenderComponent<TabItem>(parameters => parameters
					.Add(p => p.Parent, rendered.Instance));

			var div = rendered.Find("div");
			Assert.IsNotNull(div);

			var id = div.GetAttribute("id");
			foreach (var item in Enum.GetValues<TabPositons>())
			{
				rendered.SetParametersAndRender(parameters => parameters
					.Add(p => p.TabPositon, item));

				div.MarkupMatches(@$"<div id=""{id}"" class=""tabsPanel"" tabindex=""200""  >
			  <div class=""tabsHeader {item.ToString().ToLower()}"" >
				<button type=""button"" parent=""{id}""  class=""tabItem active animate"" style=""width: 100px; height: 40px; background-color: rgb(211,211,211);""   ></button>
				<button type=""button"" parent=""{id}""  class=""tabItem animate"" style=""width: 100px; height: 40px; background-color: rgb(255, 255, 255);""   ></button>
			  </div>
			</div>");
			}
		}
	}
}