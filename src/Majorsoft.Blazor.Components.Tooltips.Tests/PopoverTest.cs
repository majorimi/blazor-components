using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Bunit;

using Majorsoft.Blazor.Components.Common.JsInterop.Click;
using Majorsoft.Blazor.Components.CommonTestsBase;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Majorsoft.Blazor.Components.Tooltips.Tests
{
	[TestClass]
	public class PopoverTest : ComponentsTestBase<Popover>
	{
		private Mock<IClickBoundariesHandler> _clickHandlerMock;

		[TestInitialize]
		public void Init()
		{
			_clickHandlerMock = new Mock<IClickBoundariesHandler>();
			_clickHandlerMock
				.Setup(x => x.RegisterClickBoundariesAsync(It.IsAny<ElementReference>(), It.IsAny<Func<MouseEventArgs, Task>>(), It.IsAny<Func<MouseEventArgs, Task>>()))
				.Returns(Task.CompletedTask);
			_clickHandlerMock
				.Setup(x => x.RemoveClickBoundariesAsync(It.IsAny<ElementReference>()))
				.Returns(Task.CompletedTask);

			_testContext.Services.Add(new ServiceDescriptor(typeof(IClickBoundariesHandler), _clickHandlerMock.Object));
		}

		[TestMethod]
		public void Popover_should_start_closed()
		{
			var rendered = _testContext.Render<Popover>(parameters => parameters
				.AddChildContent("<button>open</button>"));

			Assert.IsFalse(rendered.Instance.IsOpen);
			Assert.AreEqual(0, rendered.FindAll(".bpopover").Count);
			Assert.IsNotNull(rendered.Find(".bpopover-container"));
			Assert.IsNotNull(rendered.Find(".bpopover-trigger"));
		}

		[TestMethod]
		public async Task Popover_should_open_on_trigger_click()
		{
			var rendered = _testContext.Render<Popover>(parameters => parameters
				.Add(p => p.PopoverContent, (RenderFragment)(builder => builder.AddMarkupContent(0, "<p>body</p>")))
				.AddChildContent("<button>open</button>"));

			await rendered.Find(".bpopover-trigger").TriggerEventAsync("onclick", new MouseEventArgs());

			Assert.IsTrue(rendered.Instance.IsOpen);
			var panel = rendered.Find(".bpopover");
			Assert.IsNotNull(panel);
			Assert.AreEqual("dialog", panel.GetAttribute("role"));
			Assert.AreEqual("body", rendered.Find(".bpopover-body").TextContent.Trim());
		}

		[TestMethod]
		public async Task Popover_should_toggle_open_close_on_repeated_clicks()
		{
			var rendered = _testContext.Render<Popover>(parameters => parameters
				.AddChildContent("<button>open</button>"));

			var trigger = rendered.Find(".bpopover-trigger");

			await trigger.TriggerEventAsync("onclick", new MouseEventArgs());
			Assert.IsTrue(rendered.Instance.IsOpen);

			await rendered.Find(".bpopover-trigger").TriggerEventAsync("onclick", new MouseEventArgs());
			Assert.IsFalse(rendered.Instance.IsOpen);
		}

		[TestMethod]
		public void Popover_should_open_and_close_via_IsOpen_parameter()
		{
			var rendered = _testContext.Render<Popover>(parameters => parameters
				.AddChildContent("<button>open</button>"));

			rendered.Render(parameters => parameters.Add(p => p.IsOpen, true));
			Assert.IsTrue(rendered.Instance.IsOpen);
			Assert.IsNotNull(rendered.Find(".bpopover"));

			rendered.Render(parameters => parameters.Add(p => p.IsOpen, false));
			Assert.IsFalse(rendered.Instance.IsOpen);
			Assert.AreEqual(0, rendered.FindAll(".bpopover").Count);
		}

		[TestMethod]
		public async Task Popover_should_not_open_when_Disabled()
		{
			var rendered = _testContext.Render<Popover>(parameters => parameters
				.Add(p => p.Disabled, true)
				.AddChildContent("<button>open</button>"));

			await rendered.Find(".bpopover-trigger").TriggerEventAsync("onclick", new MouseEventArgs());
			Assert.IsFalse(rendered.Instance.IsOpen);

			rendered.Render(parameters => parameters.Add(p => p.IsOpen, true));
			Assert.IsFalse(rendered.Instance.IsOpen);
		}

		[TestMethod]
		public async Task Popover_should_render_header_text_and_close_button()
		{
			var rendered = _testContext.Render<Popover>(parameters => parameters
				.Add(p => p.HeaderText, "My header")
				.AddChildContent("<button>open</button>"));

			await rendered.Find(".bpopover-trigger").TriggerEventAsync("onclick", new MouseEventArgs());

			Assert.AreEqual("My header", rendered.Find(".bpopover-title").TextContent.Trim());
			var closeButton = rendered.Find(".bpopover-close");
			Assert.IsNotNull(closeButton);
			Assert.AreEqual("Close", closeButton.GetAttribute("aria-label"));
		}

		[TestMethod]
		public async Task Popover_should_close_when_header_close_button_clicked()
		{
			var rendered = _testContext.Render<Popover>(parameters => parameters
				.Add(p => p.HeaderText, "My header")
				.AddChildContent("<button>open</button>"));

			await rendered.Find(".bpopover-trigger").TriggerEventAsync("onclick", new MouseEventArgs());
			Assert.IsTrue(rendered.Instance.IsOpen);

			await rendered.Find(".bpopover-close").TriggerEventAsync("onclick", new MouseEventArgs());
			Assert.IsFalse(rendered.Instance.IsOpen);
		}

		[TestMethod]
		public async Task Popover_should_hide_header_when_no_header_content_and_no_close_button()
		{
			var rendered = _testContext.Render<Popover>(parameters => parameters
				.Add(p => p.ShowCloseButton, false)
				.AddChildContent("<button>open</button>"));

			await rendered.Find(".bpopover-trigger").TriggerEventAsync("onclick", new MouseEventArgs());

			Assert.AreEqual(0, rendered.FindAll(".bpopover-header").Count);
		}

		[TestMethod]
		public async Task Popover_should_close_on_Escape_key_when_enabled()
		{
			var rendered = _testContext.Render<Popover>(parameters => parameters
				.AddChildContent("<button>open</button>"));

			await rendered.Find(".bpopover-trigger").TriggerEventAsync("onclick", new MouseEventArgs());
			Assert.IsTrue(rendered.Instance.IsOpen);

			await rendered.Find(".bpopover").TriggerEventAsync("onkeydown", new KeyboardEventArgs { Key = "Escape" });
			Assert.IsFalse(rendered.Instance.IsOpen);
		}

		[TestMethod]
		public async Task Popover_should_not_close_on_Escape_key_when_disabled()
		{
			var rendered = _testContext.Render<Popover>(parameters => parameters
				.Add(p => p.CloseOnEscapeKey, false)
				.AddChildContent("<button>open</button>"));

			await rendered.Find(".bpopover-trigger").TriggerEventAsync("onclick", new MouseEventArgs());
			await rendered.Find(".bpopover").TriggerEventAsync("onkeydown", new KeyboardEventArgs { Key = "Escape" });

			Assert.IsTrue(rendered.Instance.IsOpen);
		}

		[TestMethod]
		[DataRow(TooltipPositions.Top, "bpopover-top")]
		[DataRow(TooltipPositions.Right, "bpopover-right")]
		[DataRow(TooltipPositions.Bottom, "bpopover-bottom")]
		[DataRow(TooltipPositions.Left, "bpopover-left")]
		public async Task Popover_should_apply_position_css_class(TooltipPositions position, string expectedClass)
		{
			var rendered = _testContext.Render<Popover>(parameters => parameters
				.Add(p => p.Position, position)
				.AddChildContent("<button>open</button>"));

			await rendered.Find(".bpopover-trigger").TriggerEventAsync("onclick", new MouseEventArgs());

			Assert.IsTrue(rendered.Find(".bpopover").ClassList.Contains(expectedClass));
		}

		[TestMethod]
		public async Task Popover_should_apply_fixed_width_and_height()
		{
			var rendered = _testContext.Render<Popover>(parameters => parameters
				.Add(p => p.Width, 340)
				.Add(p => p.Height, 200)
				.AddChildContent("<button>open</button>"));

			await rendered.Find(".bpopover-trigger").TriggerEventAsync("onclick", new MouseEventArgs());

			var style = rendered.Find(".bpopover").GetAttribute("style");
			Assert.IsTrue(style.Contains("width:340px;"));
			Assert.IsTrue(style.Contains("height:200px;"));
		}

		[TestMethod]
		public async Task Popover_should_invoke_OnOpen_and_OnClose_callbacks()
		{
			var opened = false;
			var closed = false;

			var rendered = _testContext.Render<Popover>(parameters => parameters
				.Add(p => p.OnOpen, () => opened = true)
				.Add(p => p.OnClose, () => closed = true)
				.AddChildContent("<button>open</button>"));

			await rendered.Find(".bpopover-trigger").TriggerEventAsync("onclick", new MouseEventArgs());
			Assert.IsTrue(opened);

			rendered.Render(parameters => parameters.Add(p => p.IsOpen, false));
			Assert.IsTrue(closed);
		}

		[TestMethod]
		public void Popover_should_pass_unmatched_attributes_to_container()
		{
			var rendered = _testContext.Render<Popover>(parameters => parameters
				.AddUnmatched("data-test", "value")
				.AddChildContent("<button>open</button>"));

			var container = rendered.Find(".bpopover-container");
			Assert.IsTrue(container.HasAttribute("data-test"));
			Assert.AreEqual("value", container.GetAttribute("data-test"));
		}

		[TestMethod]
		public void Popover_should_open_when_IsOpen_parameter_set_true()
		{
			var rendered = _testContext.Render<Popover>(parameters => parameters
				.Add(p => p.IsOpen, true)
				.AddChildContent("<button>open</button>"));

			Assert.IsTrue(rendered.Instance.IsOpen);
			Assert.IsNotNull(rendered.Find(".bpopover"));
		}

		[TestMethod]
		public void Popover_should_close_when_IsOpen_parameter_set_false()
		{
			var rendered = _testContext.Render<Popover>(parameters => parameters
				.Add(p => p.IsOpen, true)
				.AddChildContent("<button>open</button>"));
			Assert.IsTrue(rendered.Instance.IsOpen);

			rendered.Render(parameters => parameters
				.Add(p => p.IsOpen, false)
				.AddChildContent("<button>open</button>"));

			Assert.IsFalse(rendered.Instance.IsOpen);
			Assert.AreEqual(0, rendered.FindAll(".bpopover").Count);
		}

		[TestMethod]
		public void Popover_should_not_open_via_IsOpen_parameter_when_Disabled()
		{
			var rendered = _testContext.Render<Popover>(parameters => parameters
				.Add(p => p.Disabled, true)
				.Add(p => p.IsOpen, true)
				.AddChildContent("<button>open</button>"));

			Assert.IsFalse(rendered.Instance.IsOpen);
			Assert.AreEqual(0, rendered.FindAll(".bpopover").Count);
		}

		[TestMethod]
		public async Task Popover_should_invoke_IsOpenChanged_on_open_and_close()
		{
			var states = new List<bool>();

			var rendered = _testContext.Render<Popover>(parameters => parameters
				.Add(p => p.IsOpenChanged, (bool v) => states.Add(v))
				.AddChildContent("<button>open</button>"));

			await rendered.Find(".bpopover-trigger").TriggerEventAsync("onclick", new MouseEventArgs());
			await rendered.Find(".bpopover-trigger").TriggerEventAsync("onclick", new MouseEventArgs());

			CollectionAssert.AreEqual(new[] { true, false }, states);
		}

		[TestMethod]
		public void Popover_should_invoke_IsOpenChanged_when_opened_via_IsOpen_parameter()
		{
			var states = new List<bool>();

			var rendered = _testContext.Render<Popover>(parameters => parameters
				.Add(p => p.IsOpenChanged, (bool v) => states.Add(v))
				.AddChildContent("<button>open</button>"));

			rendered.Render(parameters => parameters.Add(p => p.IsOpen, true));

			CollectionAssert.AreEqual(new[] { true }, states);
		}
	}
}
