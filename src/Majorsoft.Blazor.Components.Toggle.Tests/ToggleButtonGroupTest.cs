using Bunit;

using Majorsoft.Blazor.Components.CommonTestsBase;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Majorsoft.Blazor.Components.Toggle.Tests
{
	[TestClass]
	public class ToggleButtonGroupTest : ComponentsTestBase<ToggleButtonGroup>
	{
		[TestInitialize]
		public void Init()
		{
			var logger = new Mock<ILogger<ToggleButton>>();
			_testContext.Services.Add(new ServiceDescriptor(typeof(ILogger<ToggleButton>), logger.Object));
		}

		[TestMethod]
		public void ToggleButtonGroup_should_rendered_correctly_html_attributes()
		{
			var rendered = _testContext.RenderComponent<ToggleButtonGroup>(
				("title", "text") //HTML attributes
				);

			var div = rendered.Find("div");

			Assert.IsNotNull(div);
			Assert.IsNotNull(div.Id);
			Assert.IsTrue(div.HasAttribute("title"));
		}

		[TestMethod]
		public void ToggleButtonGroup_should_rendered_Disabled_correctly()
		{
			var rendered = _testContext.RenderComponent<ToggleButtonGroup>(parameters => parameters
					.Add(p => p.Disabled, true));

			var div = rendered.Find("div");

			Assert.IsNotNull(div);
			Assert.IsTrue(div.HasAttribute("disabled"));
			Assert.AreEqual(0, rendered.Instance.ButtonCount);
		}

		[TestMethod]
		public void ToggleButtonGroup_should_add_Buttons_correctly()
		{
			var rendered = _testContext.RenderComponent<ToggleButtonGroup>(parameters => parameters
					.Add(p => p.Disabled, true));

			var btn1 = _testContext.RenderComponent<ToggleButton>(parameters => parameters
					.Add(p => p.Parent, rendered.Instance));
			var btn2 = _testContext.RenderComponent<ToggleButton>(parameters => parameters
					.Add(p => p.Parent, rendered.Instance));

			var div = rendered.Find("div");

			Assert.IsNotNull(div);
			Assert.IsTrue(div.HasAttribute("disabled"));
			Assert.AreEqual(2, rendered.Instance.ButtonCount);
		}

		[TestMethod]
		public void ToggleButtonGroup_should_MustToggled_correctly()
		{
			var rendered = _testContext.RenderComponent<ToggleButtonGroup>(parameters => parameters
					.Add(p => p.MustToggled, true));

			var btn1 = _testContext.RenderComponent<ToggleButton>(parameters => parameters
					.Add(p => p.Parent, rendered.Instance));
			var btn2 = _testContext.RenderComponent<ToggleButton>(parameters => parameters
					.Add(p => p.Parent, rendered.Instance));

			var div = rendered.Find("div");

			Assert.IsNotNull(div);
			Assert.AreEqual(2, rendered.Instance.ButtonCount);

			Assert.IsNotNull(rendered.Instance.ActiveButton);
			Assert.AreEqual(btn1.Instance, rendered.Instance.ActiveButton);

			btn1.Find("button").Click();
			Assert.IsNotNull(rendered.Instance.ActiveButton);
			Assert.AreEqual(btn1.Instance, rendered.Instance.ActiveButton);
		}

		[TestMethod]
		public void ToggleButtonGroup_should_have_only_one_active_button()
		{
			var rendered = _testContext.RenderComponent<ToggleButtonGroup>(parameters => parameters
					.Add(p => p.MustToggled, false));

			var btn1 = _testContext.RenderComponent<ToggleButton>(parameters => parameters
					.Add(p => p.Parent, rendered.Instance));
			var btn2 = _testContext.RenderComponent<ToggleButton>(parameters => parameters
					.Add(p => p.Parent, rendered.Instance));

			var div = rendered.Find("div");

			Assert.IsNotNull(div);
			Assert.AreEqual(2, rendered.Instance.ButtonCount);
			Assert.IsNull(rendered.Instance.ActiveButton);

			btn1.Find("button").Click();
			Assert.IsNotNull(rendered.Instance.ActiveButton);
			Assert.AreEqual(btn1.Instance, rendered.Instance.ActiveButton);

			btn2.Find("button").Click();
			Assert.IsNotNull(rendered.Instance.ActiveButton);
			Assert.AreEqual(btn2.Instance, rendered.Instance.ActiveButton);

			btn2.Find("button").Click();
			Assert.IsNull(rendered.Instance.ActiveButton);
		}

		[Ignore]
		[TestMethod]
		public void ToggleButtonGroup_should_rendered_ToggleButtons_correctly()
		{
			var rendered = _testContext.RenderComponent<ToggleButtonGroup>();
			var btn1 = _testContext.RenderComponent<ToggleButton>(parameters => parameters
					.Add(p => p.Parent, rendered.Instance)
					.Add(p => p.Content, "<strong>1</strong>"));
			var btn2 = _testContext.RenderComponent<ToggleButton>(parameters => parameters
					.Add(p => p.Parent, rendered.Instance)
					.Add(p => p.Content, "<strong>2</strong>"));
			var btn3 = _testContext.RenderComponent<ToggleButton>(parameters => parameters
					.Add(p => p.Parent, rendered.Instance)
					.Add(p => p.Content, "<strong>3</strong>"));

			var div = rendered.Find("div");
			Assert.AreEqual(3, rendered.Instance.ButtonCount);
			Assert.IsNull(rendered.Instance.ActiveButton);

			//rendered.SetParametersAndRender(); //refrsh

			Assert.IsNotNull(div);
			Assert.IsFalse(div.HasAttribute("disabled"));

			var id = div.GetAttribute("id");
			div.MarkupMatches(@$"<div id=""{id}"" tabindex=""99""  >
				<button>
				  <strong>1</strong>
				</button>
				<button>
				  <strong>2</strong>
				</button>
				<button>
				  <strong>3</strong>
				</button>
			</div>");
		}
	}
}