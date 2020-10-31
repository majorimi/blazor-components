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
			var color = new HtmlColor(200, 55, 27);

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
			var color = new HtmlColor(255, 0, 0);

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
			Assert.IsNull(color.RgbColor);
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
			Assert.IsNull(color.RgbColor);
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
			Assert.IsNull(color.RgbColor);
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
			Assert.IsNull(color.RgbColor);
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
			var color = new HtmlColor(" 000   ,  0 ,  255");

			Assert.IsNotNull(color);
			Assert.AreEqual(" 000   ,  0 ,  255", color.OriginalValue);
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

			Assert.IsNotNull(color);
			Assert.AreEqual("128,128,128", color.OriginalValue);
			Assert.AreEqual("Gray", color.ColorName);
			Assert.AreEqual(true, color.IsNamedColor);
			Assert.AreEqual(true, color.IsValid);
			Assert.IsNotNull(color.RgbColor);
			Assert.AreEqual("#808080", color.RgbColor.ToHtmlHex());
			Assert.AreEqual("#808080", color.HexColor);
		}
	}
}