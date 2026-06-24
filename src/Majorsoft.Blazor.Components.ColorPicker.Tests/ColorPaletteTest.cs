using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

using Bunit;

using Majorsoft.Blazor.Components.Common.JsInterop.Clipboard;
using Majorsoft.Blazor.Components.Common.JsInterop.GlobalMouseEvents;
using Majorsoft.Blazor.Components.CommonTestsBase;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Majorsoft.Blazor.Components.ColorPicker.Tests
{
	[TestClass]
	public class ColorPaletteTest : ComponentsTestBase<ColorPalette>
	{
		private Mock<IGlobalMouseEventHandler> _mouseHandlerMock;
		private Mock<IClipboardHandler> _clipboardHandlerMock;

		[TestInitialize]
		public void Init()
		{
			_mouseHandlerMock = new Mock<IGlobalMouseEventHandler>();
			_clipboardHandlerMock = new Mock<IClipboardHandler>();

			//Loose mode so the first-render JS module import + isEyeDropperSupported check return defaults.
			_testContext.JSInterop.Mode = JSRuntimeMode.Loose;

			_testContext.Services.Add(new ServiceDescriptor(typeof(IGlobalMouseEventHandler), _mouseHandlerMock.Object));
			_testContext.Services.Add(new ServiceDescriptor(typeof(IClipboardHandler), _clipboardHandlerMock.Object));
		}

		[TestMethod]
		public void ColorPalette_should_render_root_container()
		{
			var rendered = _testContext.Render<ColorPalette>();

			var root = rendered.Find(".majorsoft-color-picker");
			Assert.IsNotNull(root);
			Assert.IsNotNull(rendered.Find(".picker-container"));
			Assert.IsNotNull(rendered.Find(".hue-slider"));
		}

		[TestMethod]
		public void ColorPalette_should_render_default_color_as_hex()
		{
			var rendered = _testContext.Render<ColorPalette>();

			//Default selected color is 66,135,245 => #4287F5
			var hexInput = rendered.Find(".hex-wrapper input");
			Assert.AreEqual("#4287F5", hexInput.GetAttribute("value"));
		}

		[TestMethod]
		public void ColorPalette_should_render_rgb_inputs_for_selected_color()
		{
			var rendered = _testContext.Render<ColorPalette>(parameters => parameters
				.Add(p => p.SelectedColor, Color.FromArgb(10, 20, 30)));

			Assert.AreEqual("10", rendered.Find("input[aria-label=Red]").GetAttribute("value"));
			Assert.AreEqual("20", rendered.Find("input[aria-label=Green]").GetAttribute("value"));
			Assert.AreEqual("30", rendered.Find("input[aria-label=Blue]").GetAttribute("value"));
		}

		[TestMethod]
		public void ColorPalette_should_hide_info_area_when_ShowInfoArea_false()
		{
			var rendered = _testContext.Render<ColorPalette>(parameters => parameters
				.Add(p => p.ShowInfoArea, false));

			Assert.AreEqual(0, rendered.FindAll(".info-area").Count);
		}

		[TestMethod]
		public void ColorPalette_should_hide_color_name_when_ShowColorName_false()
		{
			var rendered = _testContext.Render<ColorPalette>(parameters => parameters
				.Add(p => p.ShowColorName, false));

			Assert.AreEqual(0, rendered.FindAll(".name-wrapper").Count);
		}

		[TestMethod]
		public void ColorPalette_should_not_render_alpha_slider_by_default()
		{
			var rendered = _testContext.Render<ColorPalette>();

			Assert.AreEqual(0, rendered.FindAll(".alpha-slider").Count);
		}

		[TestMethod]
		public void ColorPalette_should_render_alpha_slider_when_EnableAlpha()
		{
			var rendered = _testContext.Render<ColorPalette>(parameters => parameters
				.Add(p => p.EnableAlpha, true));

			Assert.IsNotNull(rendered.Find(".alpha-slider"));
			//A % input also appears in the RGB row
			Assert.IsNotNull(rendered.Find("input[aria-label='Alpha percent']"));
		}

		[TestMethod]
		public void ColorPalette_should_render_default_palette_swatches()
		{
			var rendered = _testContext.Render<ColorPalette>();

			var swatches = rendered.FindAll(".palette-swatch");
			Assert.AreEqual(20, swatches.Count); //default palette has 20 colors
		}

		[TestMethod]
		public void ColorPalette_should_render_custom_palette_swatches()
		{
			var custom = new[] { Color.Red, Color.Lime, Color.Blue };
			var rendered = _testContext.Render<ColorPalette>(parameters => parameters
				.Add(p => p.PaletteColors, custom));

			var swatches = rendered.FindAll(".palette-swatch");
			Assert.AreEqual(3, swatches.Count);
		}

		[TestMethod]
		public void ColorPalette_should_hide_palette_when_ShowPalette_false()
		{
			var rendered = _testContext.Render<ColorPalette>(parameters => parameters
				.Add(p => p.ShowPalette, false));

			Assert.AreEqual(0, rendered.FindAll(".palette-swatch").Count);
		}

		[TestMethod]
		public async Task ColorPalette_should_emit_color_when_swatch_clicked()
		{
			Color? changed = null;
			var custom = new[] { Color.FromArgb(255, 12, 34, 56) };

			var rendered = _testContext.Render<ColorPalette>(parameters => parameters
				.Add(p => p.PaletteColors, custom)
				.Add(p => p.OnColorChanged, c => changed = c));

			await rendered.Find(".palette-swatch").ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

			Assert.IsNotNull(changed);
			Assert.AreEqual(12, changed.Value.R);
			Assert.AreEqual(34, changed.Value.G);
			Assert.AreEqual(56, changed.Value.B);
		}

		[TestMethod]
		public async Task ColorPalette_should_support_two_way_binding_on_swatch_click()
		{
			Color? bound = null;
			var custom = new[] { Color.FromArgb(255, 200, 100, 50) };

			var rendered = _testContext.Render<ColorPalette>(parameters => parameters
				.Add(p => p.PaletteColors, custom)
				.Add(p => p.SelectedColorChanged, c => bound = c));

			await rendered.Find(".palette-swatch").ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

			Assert.IsNotNull(bound);
			Assert.AreEqual(200, bound.Value.R);
			Assert.AreEqual(100, bound.Value.G);
			Assert.AreEqual(50, bound.Value.B);
		}

		[TestMethod]
		public async Task ColorPalette_should_update_color_from_hex_input()
		{
			Color? changed = null;

			var rendered = _testContext.Render<ColorPalette>(parameters => parameters
				.Add(p => p.OnColorChanged, c => changed = c));

			var hexInput = rendered.Find(".hex-wrapper input");
			await hexInput.ChangeAsync(new Microsoft.AspNetCore.Components.ChangeEventArgs { Value = "#FF0000" });

			Assert.IsNotNull(changed);
			Assert.AreEqual(255, changed.Value.R);
			Assert.AreEqual(0, changed.Value.G);
			Assert.AreEqual(0, changed.Value.B);
		}

		[TestMethod]
		public async Task ColorPalette_should_keep_last_valid_color_on_invalid_hex()
		{
			var rendered = _testContext.Render<ColorPalette>(parameters => parameters
				.Add(p => p.SelectedColor, Color.FromArgb(255, 0, 0)));

			var hexInput = rendered.Find(".hex-wrapper input");
			await hexInput.ChangeAsync(new Microsoft.AspNetCore.Components.ChangeEventArgs { Value = "not-a-color" });

			//Reverts to last valid value
			Assert.AreEqual("#FF0000", rendered.Find(".hex-wrapper input").GetAttribute("value"));
		}

		[TestMethod]
		public async Task ColorPalette_should_update_color_from_rgb_input()
		{
			Color? changed = null;

			var rendered = _testContext.Render<ColorPalette>(parameters => parameters
				.Add(p => p.SelectedColor, Color.FromArgb(0, 0, 0))
				.Add(p => p.OnColorChanged, c => changed = c));

			var redInput = rendered.Find("input[aria-label=Red]");
			await redInput.ChangeAsync(new Microsoft.AspNetCore.Components.ChangeEventArgs { Value = "128" });

			Assert.IsNotNull(changed);
			Assert.AreEqual(128, changed.Value.R);
		}

		[TestMethod]
		public void ColorPalette_should_apply_custom_class_and_style()
		{
			var rendered = _testContext.Render<ColorPalette>(parameters => parameters
				.Add(p => p.Class, "my-palette")
				.Add(p => p.Style, "margin: 5px;"));

			var root = rendered.Find(".majorsoft-color-picker");
			Assert.IsTrue(root.ClassList.Contains("my-palette"));
			Assert.IsTrue(root.GetAttribute("style").Contains("margin: 5px;"));
		}

		[TestMethod]
		public void ColorPalette_should_pass_unmatched_attributes_to_root()
		{
			var rendered = _testContext.Render<ColorPalette>(parameters => parameters
				.AddUnmatched("data-test", "value"));

			var root = rendered.Find(".majorsoft-color-picker");
			Assert.IsTrue(root.HasAttribute("data-test"));
			Assert.AreEqual("value", root.GetAttribute("data-test"));
		}

		[TestMethod]
		public void ColorPalette_should_clamp_dimensions_to_minimums()
		{
			var rendered = _testContext.Render<ColorPalette>(parameters => parameters
				.Add(p => p.HueAreaWidth, 10)    //below 200 min
				.Add(p => p.HueAreaHeight, 10)   //below 100 min
				.Add(p => p.HueSliderWidth, 10));

			var root = rendered.Find(".majorsoft-color-picker");
			Assert.IsTrue(root.GetAttribute("style").Contains("width:200px"));

			var pickerContainer = rendered.Find(".picker-container");
			Assert.IsTrue(pickerContainer.GetAttribute("style").Contains("height:100px"));
		}

		[TestMethod]
		public async Task ColorPalette_should_copy_hex_to_clipboard()
		{
			_clipboardHandlerMock
				.Setup(x => x.CopyTextToClipboardAsync(It.IsAny<string>()))
				.ReturnsAsync(true);

			var rendered = _testContext.Render<ColorPalette>(parameters => parameters
				.Add(p => p.SelectedColor, Color.FromArgb(255, 0, 0)));

			//Copy button is the action button with copy title
			var copyButton = rendered.FindAll(".picker-action-btn")
				.First(b => b.GetAttribute("title")!.Contains("Copy"));
			await copyButton.ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

			_clipboardHandlerMock.Verify(x => x.CopyTextToClipboardAsync("#FF0000"), Times.Once);
		}

		[TestMethod]
		public void ColorPalette_should_show_color_name_for_known_color()
		{
			var rendered = _testContext.Render<ColorPalette>(parameters => parameters
				.Add(p => p.SelectedColor, Color.FromArgb(255, 0, 0))); //Red

			var nameInput = rendered.Find(".name-wrapper input");
			Assert.AreEqual("Red", nameInput.GetAttribute("value"));
		}
	}
}
