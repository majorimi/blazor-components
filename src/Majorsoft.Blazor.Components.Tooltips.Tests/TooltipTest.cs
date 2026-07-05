using System.Threading.Tasks;

using Bunit;

using Majorsoft.Blazor.Components.CommonTestsBase;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Majorsoft.Blazor.Components.Tooltips.Tests
{
	[TestClass]
	public class TooltipTest : ComponentsTestBase<Tooltip>
	{
		[TestMethod]
		public void Tooltip_should_render_container_with_child_content()
		{
			var rendered = _testContext.Render<Tooltip>(parameters => parameters
				.AddChildContent("<span>trigger</span>"));

			var container = rendered.Find(".btooltip-container");
			Assert.IsNotNull(container);
			Assert.IsNotNull(container.QuerySelector("span"));
			Assert.AreEqual("trigger", container.QuerySelector("span").TextContent);
		}

		[TestMethod]
		public void Tooltip_should_not_render_popup_initially()
		{
			var rendered = _testContext.Render<Tooltip>(parameters => parameters
				.Add(p => p.Text, "Hello")
				.AddChildContent("<span>trigger</span>"));

			Assert.AreEqual(0, rendered.FindAll(".btooltip").Count);
		}

		[TestMethod]
		public async Task Tooltip_should_show_popup_on_mouseenter()
		{
			var rendered = _testContext.Render<Tooltip>(parameters => parameters
				.Add(p => p.Text, "Hello")
				.AddChildContent("<span>trigger</span>"));

			var container = rendered.Find(".btooltip-container");
			await container.TriggerEventAsync("onmouseenter", new MouseEventArgs());

			var popup = rendered.Find(".btooltip");
			Assert.IsNotNull(popup);
			Assert.AreEqual("Hello", popup.TextContent.Trim());
			Assert.AreEqual("tooltip", popup.GetAttribute("role"));
		}

		[TestMethod]
		public async Task Tooltip_should_hide_popup_on_mouseleave()
		{
			var rendered = _testContext.Render<Tooltip>(parameters => parameters
				.Add(p => p.Text, "Hello")
				.Add(p => p.DelayBeforeHide, 0)
				.AddChildContent("<span>trigger</span>"));

			var container = rendered.Find(".btooltip-container");
			await container.TriggerEventAsync("onmouseenter", new MouseEventArgs());
			Assert.AreEqual(1, rendered.FindAll(".btooltip").Count);

			await container.TriggerEventAsync("onmouseleave", new MouseEventArgs());
			Assert.AreEqual(0, rendered.FindAll(".btooltip").Count);
		}

		[TestMethod]
		public async Task Tooltip_should_show_popup_on_focusin_and_hide_on_focusout()
		{
			var rendered = _testContext.Render<Tooltip>(parameters => parameters
				.Add(p => p.Text, "Hello")
				.Add(p => p.DelayBeforeHide, 0)
				.AddChildContent("<span>trigger</span>"));

			var container = rendered.Find(".btooltip-container");
			await container.TriggerEventAsync("onfocusin", new FocusEventArgs());
			Assert.AreEqual(1, rendered.FindAll(".btooltip").Count);

			await container.TriggerEventAsync("onfocusout", new FocusEventArgs());
			Assert.AreEqual(0, rendered.FindAll(".btooltip").Count);
		}

		[TestMethod]
		public async Task Tooltip_should_not_show_when_Disabled()
		{
			var rendered = _testContext.Render<Tooltip>(parameters => parameters
				.Add(p => p.Text, "Hello")
				.Add(p => p.Disabled, true)
				.AddChildContent("<span>trigger</span>"));

			var container = rendered.Find(".btooltip-container");
			await container.TriggerEventAsync("onmouseenter", new MouseEventArgs());

			Assert.AreEqual(0, rendered.FindAll(".btooltip").Count);
		}

		[TestMethod]
		[DataRow(TooltipPositions.Top, "btooltip-top")]
		[DataRow(TooltipPositions.Right, "btooltip-right")]
		[DataRow(TooltipPositions.Bottom, "btooltip-bottom")]
		[DataRow(TooltipPositions.Left, "btooltip-left")]
		public async Task Tooltip_should_apply_position_css_class(TooltipPositions position, string expectedClass)
		{
			var rendered = _testContext.Render<Tooltip>(parameters => parameters
				.Add(p => p.Text, "Hello")
				.Add(p => p.Position, position)
				.AddChildContent("<span>trigger</span>"));

			var container = rendered.Find(".btooltip-container");
			await container.TriggerEventAsync("onmouseenter", new MouseEventArgs());

			var popup = rendered.Find(".btooltip");
			Assert.IsTrue(popup.ClassList.Contains(expectedClass));
		}

		[TestMethod]
		public async Task Tooltip_should_render_arrow_by_default_and_omit_when_disabled()
		{
			var withArrow = _testContext.Render<Tooltip>(parameters => parameters
				.Add(p => p.Text, "Hello")
				.AddChildContent("<span>trigger</span>"));
			await withArrow.Find(".btooltip-container").TriggerEventAsync("onmouseenter", new MouseEventArgs());
			Assert.IsTrue(withArrow.Find(".btooltip").ClassList.Contains("with-arrow"));

			var noArrow = _testContext.Render<Tooltip>(parameters => parameters
				.Add(p => p.Text, "Hello")
				.Add(p => p.ShowArrow, false)
				.AddChildContent("<span>trigger</span>"));
			await noArrow.Find(".btooltip-container").TriggerEventAsync("onmouseenter", new MouseEventArgs());
			Assert.IsFalse(noArrow.Find(".btooltip").ClassList.Contains("with-arrow"));
		}

		[TestMethod]
		public async Task Tooltip_should_prefer_TooltipContent_over_Text()
		{
			var rendered = _testContext.Render<Tooltip>(parameters => parameters
				.Add(p => p.Text, "plain")
				.Add(p => p.TooltipContent, (RenderFragment)(builder => builder.AddMarkupContent(0, "<b>rich</b>")))
				.AddChildContent("<span>trigger</span>"));

			var container = rendered.Find(".btooltip-container");
			await container.TriggerEventAsync("onmouseenter", new MouseEventArgs());

			var popup = rendered.Find(".btooltip");
			Assert.IsNotNull(popup.QuerySelector("b"));
			Assert.AreEqual("rich", popup.QuerySelector("b").TextContent);
		}

		[TestMethod]
		public async Task Tooltip_should_invoke_OnShow_and_OnHide_callbacks()
		{
			var shown = false;
			var hidden = false;

			var rendered = _testContext.Render<Tooltip>(parameters => parameters
				.Add(p => p.Text, "Hello")
				.Add(p => p.DelayBeforeHide, 0)
				.Add(p => p.OnShow, () => shown = true)
				.Add(p => p.OnHide, () => hidden = true)
				.AddChildContent("<span>trigger</span>"));

			var container = rendered.Find(".btooltip-container");
			await container.TriggerEventAsync("onmouseenter", new MouseEventArgs());
			Assert.IsTrue(shown);

			await container.TriggerEventAsync("onmouseleave", new MouseEventArgs());
			Assert.IsTrue(hidden);
		}

		[TestMethod]
		public async Task Tooltip_should_apply_custom_classes_and_styles()
		{
			var rendered = _testContext.Render<Tooltip>(parameters => parameters
				.Add(p => p.Text, "Hello")
				.Add(p => p.Class, "my-trigger")
				.Add(p => p.Style, "color: red;")
				.Add(p => p.TooltipClass, "my-popup")
				.Add(p => p.TooltipStyle, "color: blue;")
				.AddChildContent("<span>trigger</span>"));

			var container = rendered.Find(".btooltip-container");
			Assert.IsTrue(container.ClassList.Contains("my-trigger"));
			Assert.AreEqual("color: red;", container.GetAttribute("style"));

			await container.TriggerEventAsync("onmouseenter", new MouseEventArgs());

			var popup = rendered.Find(".btooltip");
			Assert.IsTrue(popup.ClassList.Contains("my-popup"));
			Assert.AreEqual("color: blue;", popup.GetAttribute("style"));
		}

		[TestMethod]
		public void Tooltip_should_pass_unmatched_attributes_to_container()
		{
			var rendered = _testContext.Render<Tooltip>(parameters => parameters
				.Add(p => p.Text, "Hello")
				.AddUnmatched("data-test", "value")
				.AddChildContent("<span>trigger</span>"));

			var container = rendered.Find(".btooltip-container");
			Assert.IsTrue(container.HasAttribute("data-test"));
			Assert.AreEqual("value", container.GetAttribute("data-test"));
		}
	}
}
