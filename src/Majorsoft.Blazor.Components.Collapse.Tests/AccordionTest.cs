using Bunit;

using Majorsoft.Blazor.Components.CommonTestsBase;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Majorsoft.Blazor.Components.Collapse.Tests
{
	[TestClass]
	public class AccordionTest : ComponentsTestBase<Accordion>
	{
		[TestInitialize]
		public void Init()
		{
			var logger = new Mock<ILogger<CollapsePanel>>();
			_testContext.Services.Add(new ServiceDescriptor(typeof(ILogger<CollapsePanel>), logger.Object));
		}

		[TestMethod]
		public void Accordion_should_rendered_correctly_html_attributes()
		{
			var rendered = _testContext.RenderComponent<Accordion>(
				("title", "text") //HTML attributes
				);

			var div = rendered.Find("div");

			Assert.IsNotNull(div);
			Assert.IsTrue(div.HasAttribute("title"));

			div.MarkupMatches(@$"<div class=""accordionPanel"" tabindex=""200"" title=""text""  ></div>");
		}

		[TestMethod]
		public void Accordion_should_rendered_correctly_CollapsePanel()
		{
			var rendered = _testContext.RenderComponent<Accordion>();

			var c1 = _testContext.RenderComponent<CollapsePanel>(parameters => parameters
					.Add(p => p.Parent, rendered.Instance)
					.Add(p => p.CommonHeader, "Header 1")
					.Add(p => p.Content, "content 1"));
			var c2 = _testContext.RenderComponent<CollapsePanel>(parameters => parameters
					.Add(p => p.Parent, rendered.Instance)
					.Add(p => p.CommonHeader, "Header 2")
					.Add(p => p.Content, "content 2"));

			var div = rendered.Find("div");
			Assert.IsNotNull(div);
			Assert.AreEqual(2, rendered.Instance.CollapsePanelCount);
		}

		[TestMethod]
		public void Accordion_should_rendered_correctly__only_one_active_CollapsePanel()
		{
			var rendered = _testContext.RenderComponent<Accordion>();

			var c1 = _testContext.RenderComponent<CollapsePanel>(parameters => parameters
					.Add(p => p.Parent, rendered.Instance)
					.Add(p => p.CommonHeader, "Header 1")
					.Add(p => p.Content, "content 1")
					.Add(p => p.Collapsed, false));
			var c2 = _testContext.RenderComponent<CollapsePanel>(parameters => parameters
					.Add(p => p.Parent, rendered.Instance)
					.Add(p => p.CommonHeader, "Header 2")
					.Add(p => p.Content, "content 2")
					.Add(p => p.Collapsed, false));

			var div = rendered.Find("div");
			Assert.IsNotNull(div);
			Assert.AreEqual(2, rendered.Instance.CollapsePanelCount);
			Assert.IsTrue(c1.Instance.Collapsed);
			Assert.IsTrue(c2.Instance.Collapsed);

			rendered.SetParametersAndRender(parameters => parameters
					.Add(p => p.ActiveCollapsePanel, c2.Instance));

			Assert.IsFalse(c2.Instance.Collapsed);
			Assert.AreEqual(rendered.Instance.ActiveCollapsePanel, c2.Instance);
		}
	}
}