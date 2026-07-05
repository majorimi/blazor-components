using System;
using System.Drawing;
using System.Threading.Tasks;

using Bunit;

using Majorsoft.Blazor.Components.Common.JsInterop.Click;
using Majorsoft.Blazor.Components.Common.JsInterop.Clipboard;
using Majorsoft.Blazor.Components.Common.JsInterop.GlobalMouseEvents;
using Majorsoft.Blazor.Components.CommonTestsBase;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Tooltips = Majorsoft.Blazor.Components.Tooltips;

namespace Majorsoft.Blazor.Components.ColorPicker.Tests
{
	[TestClass]
	public class ColorPickerTest : ComponentsTestBase<ColorPicker>
	{
		private Mock<IClickBoundariesHandler> _clickHandlerMock;
		private Mock<IGlobalMouseEventHandler> _mouseHandlerMock;
		private Mock<IClipboardHandler> _clipboardHandlerMock;

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

			_mouseHandlerMock = new Mock<IGlobalMouseEventHandler>();
			_clipboardHandlerMock = new Mock<IClipboardHandler>();

			//Loose mode so the inner ColorPalette's first-render JS module import returns defaults.
			_testContext.JSInterop.Mode = JSRuntimeMode.Loose;

			//Inner Popover requires its own logger and click handler.
			var popoverLogger = new Mock<ILogger<Tooltips.Popover>>();
			_testContext.Services.Add(new ServiceDescriptor(typeof(ILogger<Tooltips.Popover>), popoverLogger.Object));
			//Inner ColorPalette requires its own logger.
			var paletteLogger = new Mock<ILogger<ColorPalette>>();
			_testContext.Services.Add(new ServiceDescriptor(typeof(ILogger<ColorPalette>), paletteLogger.Object));

			_testContext.Services.Add(new ServiceDescriptor(typeof(IClickBoundariesHandler), _clickHandlerMock.Object));
			_testContext.Services.Add(new ServiceDescriptor(typeof(IGlobalMouseEventHandler), _mouseHandlerMock.Object));
			_testContext.Services.Add(new ServiceDescriptor(typeof(IClipboardHandler), _clipboardHandlerMock.Object));
		}

		[TestMethod]
		public void ColorPicker_should_start_closed_with_trigger_button()
		{
			var rendered = _testContext.Render<ColorPicker>();

			Assert.IsFalse(rendered.Instance.IsOpen);
			Assert.IsNotNull(rendered.Find(".bcolorpicker-trigger"));
			Assert.IsNotNull(rendered.Find(".bcolorpicker-preview"));
			//Palette is only rendered once the Popover opens.
			Assert.AreEqual(0, rendered.FindAll(".majorsoft-color-picker").Count);
		}

		[TestMethod]
		public void ColorPicker_should_render_hex_on_trigger_by_default()
		{
			var rendered = _testContext.Render<ColorPicker>(parameters => parameters
				.Add(p => p.SelectedColor, Color.FromArgb(255, 0, 0)));

			var hex = rendered.Find(".bcolorpicker-hex");
			Assert.AreEqual("#FF0000", hex.TextContent.Trim());
		}

		[TestMethod]
		public void ColorPicker_should_hide_hex_when_ShowHex_false()
		{
			var rendered = _testContext.Render<ColorPicker>(parameters => parameters
				.Add(p => p.ShowHex, false));

			Assert.AreEqual(0, rendered.FindAll(".bcolorpicker-hex").Count);
		}

		[TestMethod]
		public void ColorPicker_should_apply_preview_size()
		{
			var rendered = _testContext.Render<ColorPicker>(parameters => parameters
				.Add(p => p.PreviewSize, 50));

			var preview = rendered.Find(".bcolorpicker-preview");
			var style = preview.GetAttribute("style");
			Assert.IsTrue(style.Contains("width:50px"));
			Assert.IsTrue(style.Contains("height:50px"));
		}

		[TestMethod]
		public async Task ColorPicker_should_open_on_trigger_click_and_render_palette()
		{
			var rendered = _testContext.Render<ColorPicker>();

			await rendered.Find(".bpopover-trigger").TriggerEventAsync("onclick", new MouseEventArgs());

			Assert.IsTrue(rendered.Instance.IsOpen);
			Assert.IsNotNull(rendered.Find(".majorsoft-color-picker"));
		}

		[TestMethod]
		public void ColorPicker_should_open_and_close_via_IsOpen_parameter()
		{
			var rendered = _testContext.Render<ColorPicker>();

			rendered.Render(parameters => parameters.Add(p => p.IsOpen, true));
			Assert.IsTrue(rendered.Instance.IsOpen);
			Assert.IsNotNull(rendered.Find(".majorsoft-color-picker"));

			rendered.Render(parameters => parameters.Add(p => p.IsOpen, false));
			Assert.IsFalse(rendered.Instance.IsOpen);
			Assert.AreEqual(0, rendered.FindAll(".majorsoft-color-picker").Count);
		}

		[TestMethod]
		public void ColorPicker_should_not_open_via_IsOpen_parameter_when_Disabled()
		{
			var rendered = _testContext.Render<ColorPicker>(parameters => parameters
				.Add(p => p.Disabled, true));

			rendered.Render(parameters => parameters.Add(p => p.IsOpen, true));

			Assert.IsFalse(rendered.Instance.IsOpen);
			Assert.AreEqual(0, rendered.FindAll(".majorsoft-color-picker").Count);
		}

		[TestMethod]
		public async Task ColorPicker_should_relay_IsOpenChanged_on_trigger_open_and_apply_close()
		{
			var states = new System.Collections.Generic.List<bool>();
			var custom = new[] { Color.FromArgb(255, 1, 2, 3) };

			var rendered = _testContext.Render<ColorPicker>(parameters => parameters
				.Add(p => p.RequireApply, true)
				.Add(p => p.PaletteColors, custom)
				.Add(p => p.IsOpenChanged, (bool v) => states.Add(v)));

			await rendered.Find(".bpopover-trigger").TriggerEventAsync("onclick", new MouseEventArgs());
			await rendered.Find(".bcolorpicker-apply").ClickAsync(new MouseEventArgs());

			CollectionAssert.AreEqual(new[] { true, false }, states);
		}

		[TestMethod]
		public async Task ColorPicker_should_not_open_when_Disabled()
		{
			var rendered = _testContext.Render<ColorPicker>(parameters => parameters
				.Add(p => p.Disabled, true));

			await rendered.Find(".bpopover-trigger").TriggerEventAsync("onclick", new MouseEventArgs());
			Assert.IsFalse(rendered.Instance.IsOpen);

			//Trigger button is rendered disabled.
			Assert.IsTrue(rendered.Find(".bcolorpicker-trigger").HasAttribute("disabled"));
		}

		[TestMethod]
		public void ColorPicker_should_fire_OnOpen_and_OnClose_callbacks()
		{
			var opened = false;
			var closed = false;

			var rendered = _testContext.Render<ColorPicker>(parameters => parameters
				.Add(p => p.OnOpen, () => opened = true)
				.Add(p => p.OnClose, () => closed = true));

			rendered.Render(parameters => parameters.Add(p => p.IsOpen, true));
			Assert.IsTrue(opened);

			rendered.Render(parameters => parameters.Add(p => p.IsOpen, false));
			Assert.IsTrue(closed);
		}

		[TestMethod]
		public async Task ColorPicker_should_auto_commit_on_palette_change_when_RequireApply_false()
		{
			Color? selected = null;
			var custom = new[] { Color.FromArgb(255, 10, 20, 30) };

			var rendered = _testContext.Render<ColorPicker>(parameters => parameters
				.Add(p => p.PaletteColors, custom)
				.Add(p => p.OnColorSelected, c => selected = c));

			rendered.Render(parameters => parameters.Add(p => p.IsOpen, true));

			await rendered.Find(".palette-swatch").ClickAsync(new MouseEventArgs());

			Assert.IsNotNull(selected);
			Assert.AreEqual(10, selected.Value.R);
			Assert.AreEqual(20, selected.Value.G);
			Assert.AreEqual(30, selected.Value.B);
			//Auto-commit keeps the Popover open.
			Assert.IsTrue(rendered.Instance.IsOpen);
		}

		[TestMethod]
		public async Task ColorPicker_should_always_fire_OnColorChanged_on_palette_change()
		{
			Color? changed = null;
			var custom = new[] { Color.FromArgb(255, 90, 80, 70) };

			var rendered = _testContext.Render<ColorPicker>(parameters => parameters
				.Add(p => p.RequireApply, true)
				.Add(p => p.PaletteColors, custom)
				.Add(p => p.OnColorChanged, c => changed = c));

			rendered.Render(parameters => parameters.Add(p => p.IsOpen, true));

			await rendered.Find(".palette-swatch").ClickAsync(new MouseEventArgs());

			Assert.IsNotNull(changed);
			Assert.AreEqual(90, changed.Value.R);
		}

		[TestMethod]
		public async Task ColorPicker_should_not_commit_until_Apply_when_RequireApply_true()
		{
			Color? selected = null;
			var custom = new[] { Color.FromArgb(255, 1, 2, 3) };

			var rendered = _testContext.Render<ColorPicker>(parameters => parameters
				.Add(p => p.RequireApply, true)
				.Add(p => p.PaletteColors, custom)
				.Add(p => p.OnColorSelected, c => selected = c));

			rendered.Render(parameters => parameters.Add(p => p.IsOpen, true));

			//Apply button should be rendered.
			Assert.IsNotNull(rendered.Find(".bcolorpicker-apply"));

			await rendered.Find(".palette-swatch").ClickAsync(new MouseEventArgs());
			//Not committed yet.
			Assert.IsNull(selected);

			await rendered.Find(".bcolorpicker-apply").ClickAsync(new MouseEventArgs());
			Assert.IsNotNull(selected);
			Assert.AreEqual(1, selected.Value.R);
			//Apply closes the Popover.
			Assert.IsFalse(rendered.Instance.IsOpen);
		}

		[TestMethod]
		public void ColorPicker_should_not_render_apply_button_when_RequireApply_false()
		{
			var rendered = _testContext.Render<ColorPicker>();

			rendered.Render(parameters => parameters.Add(p => p.IsOpen, true));

			Assert.AreEqual(0, rendered.FindAll(".bcolorpicker-apply").Count);
		}

		[TestMethod]
		public void ColorPicker_should_use_custom_apply_button_text()
		{
			var rendered = _testContext.Render<ColorPicker>(parameters => parameters
				.Add(p => p.RequireApply, true)
				.Add(p => p.ApplyButtonText, "Save"));

			rendered.Render(parameters => parameters.Add(p => p.IsOpen, true));

			Assert.AreEqual("Save", rendered.Find(".bcolorpicker-apply").TextContent.Trim());
		}

		[TestMethod]
		public void ColorPicker_should_render_header_text_in_popover()
		{
			var rendered = _testContext.Render<ColorPicker>(parameters => parameters
				.Add(p => p.HeaderText, "Choose color"));

			rendered.Render(parameters => parameters.Add(p => p.IsOpen, true));

			Assert.AreEqual("Choose color", rendered.Find(".bpopover-title").TextContent.Trim());
		}

		[TestMethod]
		public void ColorPicker_should_apply_custom_class()
		{
			var rendered = _testContext.Render<ColorPicker>(parameters => parameters
				.Add(p => p.Class, "my-picker"));

			//Class is forwarded to the Popover root container along with bcolorpicker.
			var container = rendered.Find(".bcolorpicker");
			Assert.IsTrue(container.ClassList.Contains("my-picker"));
		}

		[TestMethod]
		public void ColorPicker_should_pass_unmatched_attributes_to_root()
		{
			var rendered = _testContext.Render<ColorPicker>(parameters => parameters
				.AddUnmatched("data-test", "value"));

			var container = rendered.Find(".bcolorpicker");
			Assert.IsTrue(container.HasAttribute("data-test"));
			Assert.AreEqual("value", container.GetAttribute("data-test"));
		}

		[TestMethod]
		public void ColorPicker_should_render_alpha_hex_on_trigger_when_EnableAlpha()
		{
			var rendered = _testContext.Render<ColorPicker>(parameters => parameters
				.Add(p => p.EnableAlpha, true)
				.Add(p => p.SelectedColor, Color.FromArgb(128, 255, 0, 0)));

			//8-digit hex with alpha (#RRGGBBAA).
			var hex = rendered.Find(".bcolorpicker-hex");
			Assert.AreEqual("#FF000080", hex.TextContent.Trim());
		}

		[TestMethod]
		public void ColorPicker_should_update_committed_color_from_parameter()
		{
			var rendered = _testContext.Render<ColorPicker>(parameters => parameters
				.Add(p => p.SelectedColor, Color.FromArgb(255, 0, 0)));

			Assert.AreEqual("#FF0000", rendered.Find(".bcolorpicker-hex").TextContent.Trim());

			rendered.Render(parameters => parameters
				.Add(p => p.SelectedColor, Color.FromArgb(0, 255, 0)));

			Assert.AreEqual("#00FF00", rendered.Find(".bcolorpicker-hex").TextContent.Trim());
		}
	}
}
