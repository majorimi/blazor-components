using System;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Bunit;
using Majorsoft.Blazor.Components.CommonTestsBase;
using Majorsoft.Blazor.Components.PermaLink;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Majorsoft.Blazor.Components.Tabs.Tests;

[TestClass]
public class TabsPanelTest : ComponentsTestBase<TabsPanel>
{
	private IPermaLinkWatcherService _peramalinkMock;

	[TestInitialize]
	public void Init()
	{
		ILogger<TabItem> logger = Substitute.For<ILogger<TabItem>>();
		_peramalinkMock = Substitute.For<IPermaLinkWatcherService>();

		_testContext.Services.Add(new ServiceDescriptor(typeof(ILogger<TabItem>), logger));
		_testContext.Services.Add(
			new ServiceDescriptor(typeof(IPermaLinkWatcherService), _peramalinkMock)
		);
	}

	[TestMethod]
	public void TabsPanel_should_rendered_correctly_html_attributes()
	{
		IRenderedComponent<TabsPanel> rendered = _testContext.RenderComponent<TabsPanel>(
			("title", "text") //HTML attributes
		);

		IElement div = rendered.Find("div");

		Assert.IsNotNull(div);
		Assert.IsNotNull(div.Id);
		Assert.IsTrue(div.HasAttribute("title"));

		string id = div.GetAttribute("id");
		div.MarkupMatches(
			@$"<div id=""{id}"" class=""tabsPanel"" tabindex=""200"" title=""text""  >
				  <div class=""tabsHeader left"" ></div>
				</div>"
		);
	}

	[TestMethod]
	public void TabsPanel_should_rendered_Disabled_correctly()
	{
		IRenderedComponent<TabsPanel> rendered = _testContext.RenderComponent<TabsPanel>(parameters =>
			parameters.Add(p => p.Disabled, true)
		);

		IElement div = rendered.Find("div");

		Assert.IsNotNull(div);
		Assert.IsTrue(div.HasAttribute("disabled"));
		Assert.AreEqual(0, rendered.Instance.TabCount);
	}

	[TestMethod]
	public void TabsPanel_should_add_TabItem_correctly()
	{
		IRenderedComponent<TabsPanel> rendered = _testContext.RenderComponent<TabsPanel>(parameters =>
			parameters.Add(p => p.Disabled, true)
		);

		IRenderedComponent<TabItem> tab1 = _testContext.RenderComponent<TabItem>(parameters =>
			parameters.Add(p => p.Parent, rendered.Instance)
		);
		IRenderedComponent<TabItem> tab2 = _testContext.RenderComponent<TabItem>(parameters =>
			parameters.Add(p => p.Parent, rendered.Instance)
		);

		IElement div = rendered.Find("div");

		Assert.IsNotNull(div);
		Assert.IsTrue(div.HasAttribute("disabled"));
		Assert.AreEqual(2, rendered.Instance.TabCount);
	}

	[TestMethod]
	public void TabsPanel_should_render_correct_html()
	{
		IRenderedComponent<TabsPanel> rendered = _testContext.RenderComponent<TabsPanel>(parameters =>
			parameters.Add(p => p.TabItemsHeight, 20).Add(p => p.TabItemsWidth, 200)
		);

		IRenderedComponent<TabItem> tab1 = _testContext.RenderComponent<TabItem>(parameters =>
			parameters.Add(p => p.Parent, rendered.Instance)
		);
		IRenderedComponent<TabItem> tab2 = _testContext.RenderComponent<TabItem>(parameters =>
			parameters.Add(p => p.Parent, rendered.Instance)
		);

		rendered.SetParametersAndRender(parameters =>
			parameters.Add(p => p.ActiveTab, tab1.Instance)
		); //This works automatically but Unit tests not render components at once

		IElement div = rendered.Find("div");
		Assert.IsNotNull(div);

		string id = div.GetAttribute("id");
		div.MarkupMatches(
			@$"<div id=""{id}"" class=""tabsPanel"" tabindex=""200""  >
			  <div class=""tabsHeader left"" >
				<button type=""button"" parent=""{id}""  class=""tabItem active animate"" style=""width: 200px; height: 20px; background-color: rgb(211,211,211);""   ></button>
				<button type=""button"" parent=""{id}""  class=""tabItem animate"" style=""width: 200px; height: 20px; background-color: rgb(255, 255, 255);""   ></button>
			  </div>
			</div>"
		);
	}

	[TestMethod]
	public void TabsPanel_should_render_correct_TabItems_Header_and_Content()
	{
		IRenderedComponent<TabsPanel> rendered = _testContext.RenderComponent<TabsPanel>(parameters =>
			parameters.Add(p => p.Animate, false)
		);

		IRenderedComponent<TabItem> tab1 = _testContext.RenderComponent<TabItem>(parameters =>
			parameters
				.Add(p => p.Parent, rendered.Instance)
				.Add(p => p.Header, "Tab header 1")
				.Add(p => p.Content, "tab content 1")
		);
		IRenderedComponent<TabItem> tab2 = _testContext.RenderComponent<TabItem>(parameters =>
			parameters
				.Add(p => p.Parent, rendered.Instance)
				.Add(p => p.Header, "Tab header 2")
				.Add(p => p.Content, "tab content 2")
		);

		rendered.SetParametersAndRender(parameters =>
			parameters.Add(p => p.ActiveTab, tab1.Instance)
		); //This works automatically but Unit tests not render components at once

		IElement div = rendered.Find("div");
		Assert.IsNotNull(div);
		Assert.AreEqual(rendered.Instance.ActiveTab, tab1.Instance);

		rendered.SetParametersAndRender(parameters =>
			parameters.Add(p => p.ActiveTab, tab1.Instance)
		);
		tab1.Render();
		tab2.Render();

		string id = div.GetAttribute("id");

		rendered.WaitForAssertion(() => //TODO: this should fail since Content should be rendered...
		{
			div.MarkupMatches(
				@$"<div id=""{id}"" class=""tabsPanel"" tabindex=""200""  >
				  <div class=""tabsHeader left"" >
					<button type=""button"" parent=""{id}""  class=""tabItem active"" style=""width: 100px; height: 40px; background-color: rgb(211,211,211);""   >Tab header 1</button>
					<button type=""button"" parent=""{id}""  class=""tabItem"" style=""width: 100px; height: 40px; background-color: rgb(255, 255, 255);""   >Tab header 2</button>
				  </div>
				</div>"
			);
		});
	}

	[TestMethod]
	public void TabsPanel_should_render_correct_TabItem_Disabled()
	{
		IRenderedComponent<TabsPanel> rendered = _testContext.RenderComponent<TabsPanel>(parameters =>
			parameters.Add(p => p.Animate, false)
		);

		IRenderedComponent<TabItem> tab1 = _testContext.RenderComponent<TabItem>(parameters =>
			parameters
				.Add(p => p.Parent, rendered.Instance)
				.Add(p => p.Header, "Tab header 1")
				.Add(p => p.Content, "tab content 1")
		);
		IRenderedComponent<TabItem> tab2 = _testContext.RenderComponent<TabItem>(parameters =>
			parameters
				.Add(p => p.Parent, rendered.Instance)
				.Add(p => p.Header, "Tab header 2")
				.Add(p => p.Content, "tab content 2")
				.Add(p => p.Disabled, true)
		);

		rendered.SetParametersAndRender(parameters =>
			parameters.Add(p => p.ActiveTab, tab1.Instance)
		); //This works automatically but Unit tests not render components at once

		IElement div = rendered.Find("div");
		Assert.IsNotNull(div);
		Assert.AreEqual(rendered.Instance.ActiveTab, tab1.Instance);

		rendered.Render();
		tab1.Render();
		tab2.Render();

		string id = div.GetAttribute("id");
		div.MarkupMatches(
			@$"<div id=""{id}"" class=""tabsPanel"" tabindex=""200""  >
			  <div class=""tabsHeader left"" >
				<button type=""button"" parent=""{id}""  class=""tabItem active"" style=""width: 100px; height: 40px; background-color: rgb(211,211,211);""   >Tab header 1</button>
				<button type=""button"" parent=""{id}"" disabled="""" class=""tabItem"" style=""width: 100px; height: 40px; background-color: rgb(255, 255, 255);""   >Tab header 2</button>
			  </div>
			</div>"
		);
	}

	[TestMethod]
	public void TabsPanel_should_render_correct_TabItem_Hidden()
	{
		IRenderedComponent<TabsPanel> rendered = _testContext.RenderComponent<TabsPanel>(parameters =>
			parameters.Add(p => p.Animate, false)
		);

		IRenderedComponent<TabItem> tab1 = _testContext.RenderComponent<TabItem>(parameters =>
			parameters
				.Add(p => p.Parent, rendered.Instance)
				.Add(p => p.Header, "Tab header 1")
				.Add(p => p.Content, "tab content 1")
		);
		IRenderedComponent<TabItem> tab2 = _testContext.RenderComponent<TabItem>(parameters =>
			parameters
				.Add(p => p.Parent, rendered.Instance)
				.Add(p => p.Header, "Tab header 2")
				.Add(p => p.Content, "tab content 2")
				.Add(p => p.Hidden, true)
		);

		rendered.SetParametersAndRender(parameters =>
			parameters.Add(p => p.ActiveTab, tab1.Instance)
		); //This works automatically but Unit tests not render components at once

		IElement div = rendered.Find("div");
		Assert.IsNotNull(div);
		Assert.AreEqual(rendered.Instance.ActiveTab, tab1.Instance);

		rendered.Render();
		tab1.Render();
		tab2.Render();

		string id = div.GetAttribute("id");
		div.MarkupMatches(
			@$"<div id=""{id}"" class=""tabsPanel"" tabindex=""200""  >
			  <div class=""tabsHeader left"" >
				<button type=""button"" parent=""{id}""  class=""tabItem active"" style=""width: 100px; height: 40px; background-color: rgb(211,211,211);""   >Tab header 1</button>
			  </div>
			</div>"
		);
	}

	[TestMethod]
	public void TabsPanel_should_render_correct_ActiveColor()
	{
		IRenderedComponent<TabsPanel> rendered = _testContext.RenderComponent<TabsPanel>(parameters =>
			parameters.Add(p => p.ActiveColor, "red")
		);

		IRenderedComponent<TabItem> tab1 = _testContext.RenderComponent<TabItem>(parameters =>
			parameters.Add(p => p.Parent, rendered.Instance)
		);
		IRenderedComponent<TabItem> tab2 = _testContext.RenderComponent<TabItem>(parameters =>
			parameters.Add(p => p.Parent, rendered.Instance)
		);

		rendered.SetParametersAndRender(parameters =>
			parameters.Add(p => p.ActiveTab, tab1.Instance)
		); //This works automatically but Unit tests not render components at once

		IElement div = rendered.Find("div");
		Assert.IsNotNull(div);

		string id = div.GetAttribute("id");
		div.MarkupMatches(
			@$"<div id=""{id}"" class=""tabsPanel"" tabindex=""200""  >
			  <div class=""tabsHeader left"" >
				<button type=""button"" parent=""{id}""  class=""tabItem active animate"" style=""width: 100px; height: 40px; background-color: rgb(255, 0, 0);""   ></button>
				<button type=""button"" parent=""{id}""  class=""tabItem animate"" style=""width: 100px; height: 40px; background-color: rgb(255, 255, 255);""   ></button>
			  </div>
			</div>"
		);
	}

	[TestMethod]
	public void TabsPanel_should_render_correct_InactiveColor()
	{
		IRenderedComponent<TabsPanel> rendered = _testContext.RenderComponent<TabsPanel>(parameters =>
			parameters.Add(p => p.InactiveColor, "red")
		);

		IRenderedComponent<TabItem> tab1 = _testContext.RenderComponent<TabItem>(parameters =>
			parameters.Add(p => p.Parent, rendered.Instance)
		);
		IRenderedComponent<TabItem> tab2 = _testContext.RenderComponent<TabItem>(parameters =>
			parameters.Add(p => p.Parent, rendered.Instance)
		);

		rendered.SetParametersAndRender(parameters =>
			parameters.Add(p => p.ActiveTab, tab1.Instance)
		); //This works automatically but Unit tests not render components at once

		IElement div = rendered.Find("div");
		Assert.IsNotNull(div);

		string id = div.GetAttribute("id");
		div.MarkupMatches(
			@$"<div id=""{id}"" class=""tabsPanel"" tabindex=""200""  >
			  <div class=""tabsHeader left"" >
				<button type=""button"" parent=""{id}""  class=""tabItem active animate"" style=""width: 100px; height: 40px; background-color: rgb(211,211,211);""   ></button>
				<button type=""button"" parent=""{id}""  class=""tabItem animate"" style=""width: 100px; height: 40px; background-color: rgb(255, 0, 0);""   ></button>
			  </div>
			</div>"
		);
	}

	[TestMethod]
	public async Task TabsPanel_should_render_correct_HoverColor_on_active_Tab()
	{
		IRenderedComponent<TabsPanel> rendered = _testContext.RenderComponent<TabsPanel>(parameters =>
			parameters.Add(p => p.HoverColor, "red")
		);

		IRenderedComponent<TabItem> tab1 = _testContext.RenderComponent<TabItem>(parameters =>
			parameters.Add(p => p.Parent, rendered.Instance)
		);
		IRenderedComponent<TabItem> tab2 = _testContext.RenderComponent<TabItem>(parameters =>
			parameters.Add(p => p.Parent, rendered.Instance)
		);

		rendered.SetParametersAndRender(parameters =>
			parameters.Add(p => p.ActiveTab, tab1.Instance)
		); //This works automatically but Unit tests not render components at once

		IElement div = rendered.Find("div");
		Assert.IsNotNull(div);

		string id = div.GetAttribute("id");
		div.MarkupMatches(
			@$"<div id=""{id}"" class=""tabsPanel"" tabindex=""200""  >
			  <div class=""tabsHeader left"" >
				<button type=""button"" parent=""{id}""  class=""tabItem active animate"" style=""width: 100px; height: 40px; background-color: rgb(211,211,211);""></button>
				<button type=""button"" parent=""{id}""  class=""tabItem animate"" style=""width: 100px; height: 40px; background-color: rgb(255, 255, 255);""></button>
			  </div>
			</div>"
		);

		IRefreshableElementCollection<IElement> buttons = rendered.FindAll("button");
		await buttons[0].TriggerEventAsync("onmouseenter", new MouseEventArgs()); //active tab
		rendered.WaitForAssertion(() =>
		{
			div.MarkupMatches(
				@$"<div id=""{id}"" class=""tabsPanel"" tabindex=""200""  >
				  <div class=""tabsHeader left"" >
					<button type=""button"" parent=""{id}""  class=""tabItem active animate"" style=""width: 100px; height: 40px; background-color: rgb(211,211,211);""></button>
					<button type=""button"" parent=""{id}""  class=""tabItem animate"" style=""width: 100px; height: 40px; background-color: rgb(255, 255, 255);""></button>
				  </div>
				</div>"
			);
		});
		await buttons[0].TriggerEventAsync("onmouseleave", new MouseEventArgs());
	}

	[TestMethod]
	public async Task TabsPanel_should_render_correct_HoverColor_on_inactive_Tab()
	{
		IRenderedComponent<TabsPanel> rendered = _testContext.RenderComponent<TabsPanel>(parameters =>
			parameters.Add(p => p.HoverColor, "red")
		);

		IRenderedComponent<TabItem> tab1 = _testContext.RenderComponent<TabItem>(parameters =>
			parameters.Add(p => p.Parent, rendered.Instance)
		);
		IRenderedComponent<TabItem> tab2 = _testContext.RenderComponent<TabItem>(parameters =>
			parameters.Add(p => p.Parent, rendered.Instance)
		);

		rendered.SetParametersAndRender(parameters =>
			parameters.Add(p => p.ActiveTab, tab1.Instance)
		); //This works automatically but Unit tests not render components at once

		IElement div = rendered.Find("div");
		Assert.IsNotNull(div);

		string id = div.GetAttribute("id");
		div.MarkupMatches(
			@$"<div id=""{id}"" class=""tabsPanel"" tabindex=""200""  >
			  <div class=""tabsHeader left"" >
				<button type=""button"" parent=""{id}""  class=""tabItem active animate"" style=""width: 100px; height: 40px; background-color: rgb(211,211,211);""></button>
				<button type=""button"" parent=""{id}""  class=""tabItem animate"" style=""width: 100px; height: 40px; background-color: rgb(255, 255, 255);""></button>
			  </div>
			</div>"
		);

		IRefreshableElementCollection<IElement> buttons = rendered.FindAll("button");
		await buttons[1].TriggerEventAsync("onmouseenter", new MouseEventArgs()); //inactive tab
		rendered.WaitForAssertion(() =>
		{
			div.MarkupMatches(
				@$"<div id=""{id}"" class=""tabsPanel"" tabindex=""200""  >
				  <div class=""tabsHeader left"" >
					<button type=""button"" parent=""{id}""  class=""tabItem active animate"" style=""width: 100px; height: 40px; background-color: rgb(211,211,211);""></button>
					<button type=""button"" parent=""{id}""  class=""tabItem animate"" style=""width: 100px; height: 40px; background-color: rgb(255, 0, 0);""></button>
				  </div>
				</div>"
			);
		});
		await buttons[1].TriggerEventAsync("onmouseleave", new MouseEventArgs());
	}

	[TestMethod]
	public void TabsPanel_should_render_non_Animate()
	{
		IRenderedComponent<TabsPanel> rendered = _testContext.RenderComponent<TabsPanel>(parameters =>
			parameters.Add(p => p.Animate, false)
		);

		IRenderedComponent<TabItem> tab1 = _testContext.RenderComponent<TabItem>(parameters =>
			parameters.Add(p => p.Parent, rendered.Instance)
		);
		IRenderedComponent<TabItem> tab2 = _testContext.RenderComponent<TabItem>(parameters =>
			parameters.Add(p => p.Parent, rendered.Instance)
		);

		rendered.SetParametersAndRender(parameters =>
			parameters.Add(p => p.ActiveTab, tab1.Instance)
		); //This works automatically but Unit tests not render components at once

		IElement div = rendered.Find("div");
		Assert.IsNotNull(div);

		string id = div.GetAttribute("id");
		div.MarkupMatches(
			@$"<div id=""{id}"" class=""tabsPanel"" tabindex=""200""  >
			  <div class=""tabsHeader left"" >
				<button type=""button"" parent=""{id}""  class=""tabItem active"" style=""width: 100px; height: 40px; background-color: rgb(211,211,211);""   ></button>
				<button type=""button"" parent=""{id}""  class=""tabItem"" style=""width: 100px; height: 40px; background-color: rgb(255, 255, 255);""   ></button>
			  </div>
			</div>"
		);
	}

	[TestMethod]
	public void TabsPanel_should_render_correct_TabItemsHeight()
	{
		IRenderedComponent<TabsPanel> rendered = _testContext.RenderComponent<TabsPanel>(parameters =>
			parameters.Add(p => p.TabItemsHeight, 0)
		);

		IRenderedComponent<TabItem> tab1 = _testContext.RenderComponent<TabItem>(parameters =>
			parameters.Add(p => p.Parent, rendered.Instance)
		);
		IRenderedComponent<TabItem> tab2 = _testContext.RenderComponent<TabItem>(parameters =>
			parameters.Add(p => p.Parent, rendered.Instance)
		);

		rendered.SetParametersAndRender(parameters =>
			parameters.Add(p => p.ActiveTab, tab1.Instance)
		); //This works automatically but Unit tests not render components at once

		IElement div = rendered.Find("div");
		Assert.IsNotNull(div);

		string id = div.GetAttribute("id");
		div.MarkupMatches(
			$"""
			 <div id="{id}" class="tabsPanel" tabindex="200"  >
			 			  <div class="tabsHeader left" >
			 				<button type="button" parent="{id}"  class="tabItem active animate" style="width: 100px; height: auto; background-color: rgb(211,211,211);"   ></button>
			 				<button type="button" parent="{id}"  class="tabItem animate" style="width: 100px; height: auto; background-color: rgb(255, 255, 255);"   ></button>
			 			  </div>
			 			</div>
			 """
		);
	}

	[TestMethod]
	public void TabsPanel_should_render_correct_TabItemsWidth()
	{
		IRenderedComponent<TabsPanel> rendered = _testContext.RenderComponent<TabsPanel>(parameters =>
			parameters.Add(p => p.TabItemsWidth, 0)
		);

		IRenderedComponent<TabItem> tab1 = _testContext.RenderComponent<TabItem>(parameters =>
			parameters.Add(p => p.Parent, rendered.Instance)
		);
		IRenderedComponent<TabItem> tab2 = _testContext.RenderComponent<TabItem>(parameters =>
			parameters.Add(p => p.Parent, rendered.Instance)
		);

		rendered.SetParametersAndRender(parameters =>
			parameters.Add(p => p.ActiveTab, tab1.Instance)
		); //This works automatically but Unit tests not render components at once

		IElement div = rendered.Find("div");
		Assert.IsNotNull(div);

		string id = div.GetAttribute("id");
		div.MarkupMatches(
			@$"<div id=""{id}"" class=""tabsPanel"" tabindex=""200""  >
			  <div class=""tabsHeader left"" >
				<button type=""button"" parent=""{id}""  class=""tabItem active animate"" style=""width: auto; height: 40px; background-color: rgb(211,211,211);""   ></button>
				<button type=""button"" parent=""{id}""  class=""tabItem animate"" style=""width: auto; height: 40px; background-color: rgb(255, 255, 255);""   ></button>
			  </div>
			</div>"
		);
	}

	[TestMethod]
	public async Task TabsPanel_should_render_correct_active_Tab_on_click()
	{
		IRenderedComponent<TabsPanel> rendered = _testContext.RenderComponent<TabsPanel>();

		IRenderedComponent<TabItem> tab1 = _testContext.RenderComponent<TabItem>(parameters =>
			parameters.Add(p => p.Parent, rendered.Instance)
		);
		IRenderedComponent<TabItem> tab2 = _testContext.RenderComponent<TabItem>(parameters =>
			parameters.Add(p => p.Parent, rendered.Instance)
		);

		rendered.SetParametersAndRender(parameters =>
			parameters.Add(p => p.ActiveTab, tab1.Instance)
		); //This works automatically but Unit tests not render components at once

		IElement div = rendered.Find("div");
		Assert.IsNotNull(div);

		string id = div.GetAttribute("id");
		div.MarkupMatches(
			$"""
			 <div id="{id}" class="tabsPanel" tabindex="200"  >
			 			  <div class="tabsHeader left" >
			 				<button type="button" parent="{id}"  class="tabItem active animate" style="width: 100px; height: 40px; background-color: rgb(211,211,211);"   ></button>
			 				<button type="button" parent="{id}"  class="tabItem animate" style="width: 100px; height: 40px; background-color: rgb(255, 255, 255);"   ></button>
			 			  </div>
			 			</div>
			 """
		);

		IRefreshableElementCollection<IElement> buttons = rendered.FindAll("button");
		await buttons[1].TriggerEventAsync("onclick", new MouseEventArgs()); //inactive tab
		rendered.WaitForAssertion(() =>
		{
			div.MarkupMatches(
				$"""
				 <div id="{id}" class="tabsPanel" tabindex="200"  >
				 				  <div class="tabsHeader left" >
				 					<button type="button" parent="{id}"  class="tabItem animate" style="width: 100px; height: 40px; background-color: rgb(255, 255, 255);"   ></button>
				 					<button type="button" parent="{id}"  class="tabItem active animate" style="width: 100px; height: 40px; background-color: rgb(211,211,211);"   ></button>
				 				  </div>
				 				</div>
				 """
			);
		});

		_peramalinkMock.Received(0).ChangePermalink(Arg.Any<string?>(), Arg.Any<bool>());
	}

	[TestMethod]
	public void TabsPanel_should_render_correct_TabPositon()
	{
		IRenderedComponent<TabsPanel> rendered = _testContext.RenderComponent<TabsPanel>(parameters =>
			parameters.Add(p => p.TabPositon, TabPositons.Left)
		);

		IRenderedComponent<TabItem> tab1 = _testContext.RenderComponent<TabItem>(parameters =>
			parameters.Add(p => p.Parent, rendered.Instance)
		);
		IRenderedComponent<TabItem> tab2 = _testContext.RenderComponent<TabItem>(parameters =>
			parameters.Add(p => p.Parent, rendered.Instance)
		);

		rendered.SetParametersAndRender(parameters =>
			parameters.Add(p => p.ActiveTab, tab1.Instance)
		); //This works automatically but Unit tests not render components at once

		IElement div = rendered.Find("div");
		Assert.IsNotNull(div);

		string id = div.GetAttribute("id");
		foreach (TabPositons item in Enum.GetValues<TabPositons>())
		{
			rendered.SetParametersAndRender(parameters => parameters.Add(p => p.TabPositon, item));

			div.MarkupMatches(
				$"""
				 <div id="{id}" class="tabsPanel" tabindex="200"  >
				 			  <div class="tabsHeader {item.ToString().ToLower()}" >
				 				<button type="button" parent="{id}"  class="tabItem active animate" style="width: 100px; height: 40px; background-color: rgb(211,211,211);"   ></button>
				 				<button type="button" parent="{id}"  class="tabItem animate" style="width: 100px; height: 40px; background-color: rgb(255, 255, 255);"   ></button>
				 			  </div>
				 			</div>
				 """
			);
		}
	}

	[TestMethod]
	public void TabsPanel_should_render_correct_TabActivation()
	{
		_peramalinkMock.When(x => x.ChangePermalink(Arg.Any<string?>(), Arg.Any<bool>()));
		_peramalinkMock.When(x =>
			x.PermalinkDetected += Arg.Any<EventHandler<PermalinkDetectedEventArgs>>()
		);
		_peramalinkMock.When(x =>
			x.PermalinkDetected -= Arg.Any<EventHandler<PermalinkDetectedEventArgs>>()
		);

		IRenderedComponent<TabsPanel> rendered = _testContext.RenderComponent<TabsPanel>(parameters =>
			parameters.Add(p => p.AllowTabActivationByPermalink, true)
		);

		IRenderedComponent<TabItem> tab1 = _testContext.RenderComponent<TabItem>(parameters =>
			parameters.Add(p => p.Parent, rendered.Instance).Add(p => p.Permalink, "tab1")
		);
		IRenderedComponent<TabItem> tab2 = _testContext.RenderComponent<TabItem>(parameters =>
			parameters.Add(p => p.Parent, rendered.Instance).Add(p => p.Permalink, "tab2")
		);

		_peramalinkMock.PermalinkDetected += Raise.EventWith(
			new PermalinkDetectedEventArgs(null, "tab2")
		);
		rendered.Render();

		IElement div = rendered.Find("div");
		Assert.IsNotNull(div);

		string id = div.GetAttribute("id");
		div.MarkupMatches(
			$"""
			 <div id="{id}" class="tabsPanel" tabindex="200"  >
			 			  <div class="tabsHeader left" >
			 				<button type="button" parent="{id}" class="tabItem animate" style="width: 100px; height: 40px; background-color: rgb(255, 255, 255);"   ></button>
			 				<button type="button" parent="{id}" class="tabItem active animate" style="width: 100px; height: 40px; background-color: rgb(211,211,211);"   ></button>
			 			  </div>
			 			</div>
			 """
		);

		_peramalinkMock.Received(1).ChangePermalink("tab2", true);
	}

	[TestMethod]
	public void TabsPanel_should_render_correct_TabActivation_disabled()
	{
		IRenderedComponent<TabsPanel> rendered = _testContext.RenderComponent<TabsPanel>(parameters =>
			parameters.Add(p => p.AllowTabActivationByPermalink, false)
		);

		IRenderedComponent<TabItem> tab1 = _testContext.RenderComponent<TabItem>(parameters =>
			parameters.Add(p => p.Parent, rendered.Instance).Add(p => p.Permalink, "tab1")
		);
		IRenderedComponent<TabItem> tab2 = _testContext.RenderComponent<TabItem>(parameters =>
			parameters.Add(p => p.Parent, rendered.Instance).Add(p => p.Permalink, "tab2")
		);

		_peramalinkMock.PermalinkDetected += null;
		_peramalinkMock.PermalinkDetected += Raise.EventWith(new PermalinkDetectedEventArgs(null, "tab2"));
		rendered.Render();

		IElement div = rendered.Find("div");
		Assert.IsNotNull(div);

		string id = div.GetAttribute("id");
		div.MarkupMatches(
			$"""
			 <div id="{id}" class="tabsPanel" tabindex="200"  >
			 			  <div class="tabsHeader left" >
			 				<button type="button" parent="{id}" class="tabItem animate" style="width: 100px; height: 40px; background-color: rgb(255, 255, 255);"   ></button>
			 				<button type="button" parent="{id}" class="tabItem animate" style="width: 100px; height: 40px; background-color: rgb(255, 255, 255);"   ></button>
			 			  </div>
			 			</div>
			 """
		);

		_peramalinkMock.DidNotReceive().ChangePermalink("tab2", true);
	}
}
