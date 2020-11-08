using Blazor.Components.Core.HtmlColors;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Blazor.Components.Core.Tests.HtmlColors
{
	[TestClass]
	public class HtmlColorTest
	{
		[TestMethod]
		public void HtmlColor_should_be_created_with_rgb_bytes()
		{
			var color = new HtmlColor((byte)200, (byte)55, (byte)27);

			Assert.IsNotNull(color);
			Assert.AreEqual(null, color.OriginalValue);
			Assert.AreEqual(false, color.IsNamedColor);
			Assert.AreEqual(true, color.IsValid);
			Assert.IsNotNull(color.RgbColor);
			Assert.AreEqual("#C8371B", color.RgbColor.ToHtmlHex());
			Assert.AreEqual("#C8371B", color.HexColor);
		}

		[TestMethod]
		public void HtmlColor_should_be_created_named_color_with_rgb_bytes()
		{
			var color = new HtmlColor((byte)255, (byte)0, (byte)0);

			Assert.IsNotNull(color);
			Assert.AreEqual(null, color.OriginalValue);
			Assert.AreEqual(true, color.IsNamedColor);
			Assert.AreEqual("Red", color.ColorName);
			Assert.AreEqual(true, color.IsValid);
			Assert.IsNotNull(color.RgbColor);
			Assert.AreEqual("#FF0000", color.RgbColor.ToHtmlHex());
			Assert.AreEqual("#FF0000", color.HexColor);
		}

		[TestMethod]
		public void HtmlColor_should_handle_null()
		{
			var color = new HtmlColor(null);

			Assert.IsNotNull(color);
			Assert.AreEqual(null, color.OriginalValue);
			Assert.AreEqual(false, color.IsNamedColor);
			Assert.AreEqual(false, color.IsValid);
			Assert.IsNotNull(color.RgbColor);
			Assert.AreEqual(null, color.HexColor);
		}

		[TestMethod]
		public void HtmlColor_should_handle_empty_string()
		{
			var color = new HtmlColor(string.Empty);

			Assert.IsNotNull(color);
			Assert.AreEqual(string.Empty, color.OriginalValue);
			Assert.AreEqual(false, color.IsNamedColor);
			Assert.AreEqual(false, color.IsValid);
			Assert.IsNotNull(color.RgbColor);
			Assert.AreEqual(null, color.HexColor);
		}

		[TestMethod]
		public void HtmlColor_should_handle_whitespace_string()
		{
			var color = new HtmlColor("       ");

			Assert.IsNotNull(color);
			Assert.AreEqual("       ", color.OriginalValue);
			Assert.AreEqual(false, color.IsNamedColor);
			Assert.AreEqual(false, color.IsValid);
			Assert.IsNotNull(color.RgbColor);
			Assert.AreEqual(null, color.HexColor);
		}

		[TestMethod]
		public void HtmlColor_should_handle_invalid_string()
		{
			var color = new HtmlColor("asfjdosdfjcx");

			Assert.IsNotNull(color);
			Assert.AreEqual("asfjdosdfjcx", color.OriginalValue);
			Assert.AreEqual(false, color.IsNamedColor);
			Assert.AreEqual(false, color.IsValid);
			Assert.IsNotNull(color.RgbColor);
			Assert.AreEqual(null, color.HexColor);
		}

		[TestMethod]
		public void HtmlColor_should_handle_valid_nameColor()
		{
			var color = new HtmlColor("LightBluE");

			Assert.IsNotNull(color);
			Assert.AreEqual("LightBluE", color.OriginalValue);
			Assert.AreEqual("LightBlue", color.ColorName);
			Assert.AreEqual(true, color.IsNamedColor);
			Assert.AreEqual(true, color.IsValid);
			Assert.IsNotNull(color.RgbColor);
			Assert.AreEqual("#ADD8E6", color.RgbColor.ToHtmlHex());
			Assert.AreEqual("#ADD8E6", color.HexColor);
		}

		[TestMethod]
		public void HtmlColor_should_handle_valid_hexColor()
		{
			var color = new HtmlColor("c8371B");

			Assert.IsNotNull(color);
			Assert.AreEqual("c8371B", color.OriginalValue);
			Assert.AreEqual(null, color.ColorName);
			Assert.AreEqual(false, color.IsNamedColor);
			Assert.AreEqual(true, color.IsValid);
			Assert.IsNotNull(color.RgbColor);
			Assert.AreEqual("#C8371B", color.RgbColor.ToHtmlHex());
			Assert.AreEqual("#C8371B", color.HexColor);
		}

		[TestMethod]
		public void HtmlColor_should_handle_valid_nameColor_from_hex()
		{
			var color = new HtmlColor("#fF0000");

			Assert.IsNotNull(color);
			Assert.AreEqual("#fF0000", color.OriginalValue);
			Assert.AreEqual("Red", color.ColorName);
			Assert.AreEqual(true, color.IsNamedColor);
			Assert.AreEqual(true, color.IsValid);
			Assert.IsNotNull(color.RgbColor);
			Assert.AreEqual("#FF0000", color.RgbColor.ToHtmlHex());
			Assert.AreEqual("#FF0000", color.HexColor);
		}

		[TestMethod]
		public void HtmlColor_should_handle_valid_spaced_rgbColor()
		{
			var color = new HtmlColor(" 200,55,     27");

			Assert.IsNotNull(color);
			Assert.AreEqual(" 200,55,     27", color.OriginalValue);
			Assert.AreEqual(null, color.ColorName);
			Assert.AreEqual(false, color.IsNamedColor);
			Assert.AreEqual(true, color.IsValid);
			Assert.IsNotNull(color.RgbColor);
			Assert.AreEqual("#C8371B", color.RgbColor.ToHtmlHex());
			Assert.AreEqual("#C8371B", color.HexColor);
		}

		[TestMethod]
		public void HtmlColor_should_handle_valid_nameColor_from_rgb()
		{
			var color = new HtmlColor(" 000   ,  0   255");

			Assert.IsNotNull(color);
			Assert.AreEqual(" 000   ,  0   255", color.OriginalValue);
			Assert.AreEqual("Blue", color.ColorName);
			Assert.AreEqual(true, color.IsNamedColor);
			Assert.AreEqual(true, color.IsValid);
			Assert.IsNotNull(color.RgbColor);
			Assert.AreEqual("#0000FF", color.RgbColor.ToHtmlHex());
			Assert.AreEqual("#0000FF", color.HexColor);
		}

		[TestMethod]
		public void HtmlColor_should_handle_valid_rgbColor()
		{
			var color = new HtmlColor("128,128,128");
			var color2 = new HtmlColor("219 0 0");

			Assert.IsNotNull(color);
			Assert.AreEqual("128,128,128", color.OriginalValue);
			Assert.AreEqual("Gray", color.ColorName);
			Assert.AreEqual(true, color.IsNamedColor);
			Assert.AreEqual(true, color.IsValid);
			Assert.IsNotNull(color.RgbColor);
			Assert.AreEqual("#808080", color.RgbColor.ToHtmlHex());
			Assert.AreEqual("#808080", color.HexColor);

			Assert.IsNotNull(color2);
			Assert.AreEqual("#DB0000", color2.HexColor);
		}

		[TestMethod]
		public void HtmlColor_should_handle_valid_hslColor()
		{
			var color = new HtmlColor("120° , 100% , 50");
			var color2 = new HtmlColor("240 0 0%");
			var color3 = new HtmlColor("360° 100% 50%");
			var color4 = new HtmlColor("217°, 90%, 61%");

			Assert.IsNotNull(color);
			Assert.AreEqual("120° , 100% , 50", color.OriginalValue);
			Assert.AreEqual("Lime", color.ColorName);
			Assert.AreEqual(true, color.IsNamedColor);
			Assert.AreEqual(true, color.IsValid);
			Assert.IsNotNull(color.RgbColor);
			Assert.AreEqual("#00FF00", color.RgbColor.ToHtmlHex());
			Assert.AreEqual("#00FF00", color.HexColor);

			Assert.IsNotNull(color2);
			Assert.AreEqual("Black", color2.ColorName);
			Assert.AreEqual(true, color2.IsNamedColor);
			Assert.AreEqual(true, color2.IsValid);
			Assert.AreEqual("#000000", color2.HexColor);

			Assert.IsNotNull(color3);
			Assert.AreEqual("Red", color3.ColorName);
			Assert.AreEqual(true, color3.IsNamedColor);
			Assert.AreEqual(true, color3.IsValid);
			Assert.AreEqual("#FF0000", color3.HexColor);

			Assert.IsNotNull(color4);
			Assert.AreEqual(null, color4.ColorName);
			Assert.AreEqual(false, color4.IsNamedColor);
			Assert.AreEqual(true, color4.IsValid);
			Assert.AreEqual("#4287F5", color4.HexColor);
		}
	}
}