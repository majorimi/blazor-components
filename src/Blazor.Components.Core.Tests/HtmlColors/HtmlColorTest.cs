using System;

using Blazor.Components.Core.HtmlColors;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Blazor.Components.Core.Tests.HtmlColors
{
	[TestClass]
	public class HtmlColorTest
	{
		[TestMethod]
		public void HtmlColor_should_be_created_FromRgb_bytes()
		{
			var color = HtmlColor.FromRgb(200, 55, 27);

			Assert.IsNotNull(color);
			Assert.AreEqual(null, color.OriginalValue);
			Assert.AreEqual(false, color.IsNamedColor);
			Assert.AreEqual(true, color.IsValid);

			Assert.IsNotNull(color.RgbColor);
			Assert.AreEqual(200, color.RgbColor.R);
			Assert.AreEqual(55, color.RgbColor.G);
			Assert.AreEqual(27, color.RgbColor.B);

			Assert.IsNotNull(color.HslColor);
			Assert.AreEqual(10, Math.Round(color.HslColor.Hue));
			Assert.AreEqual(76, Math.Round(color.HslColor.Saturation));
			Assert.AreEqual(45, Math.Round(color.HslColor.Luminosity));

			Assert.AreEqual("#C8371B", color.RgbColor.ToHtmlHex());
			Assert.AreEqual("#C8371B", color.HexColor);
		}

		[TestMethod]
		public void HtmlColor_should_be_created_named_color_FromRgb_bytes()
		{
			var color = HtmlColor.FromRgb(255, 0, 0);

			Assert.IsNotNull(color);
			Assert.AreEqual(null, color.OriginalValue);
			Assert.AreEqual(true, color.IsNamedColor);
			Assert.AreEqual("Red", color.ColorName);
			Assert.AreEqual(true, color.IsValid);

			Assert.IsNotNull(color.RgbColor);
			Assert.AreEqual(255, color.RgbColor.R);
			Assert.AreEqual(0, color.RgbColor.G);
			Assert.AreEqual(0, color.RgbColor.B);

			Assert.IsNotNull(color.HslColor);
			Assert.AreEqual(360, Math.Round(color.HslColor.Hue));
			Assert.AreEqual(100, Math.Round(color.HslColor.Saturation));
			Assert.AreEqual(50, Math.Round(color.HslColor.Luminosity));

			Assert.AreEqual("#FF0000", color.RgbColor.ToHtmlHex());
			Assert.AreEqual("#FF0000", color.HexColor);
		}

		[TestMethod]
		public void HtmlColor_should_be_created_FromRgb_string()
		{
			var color = HtmlColor.FromRgb("200, 55, 27");
			var color2 = HtmlColor.FromRgb(null);
			var color3 = HtmlColor.FromRgb("");
			var color4 = HtmlColor.FromRgb("   ");
			var color5 = HtmlColor.FromRgb("200,55,27");
			var color6 = HtmlColor.FromRgb("200 55  27");
			var color7 = HtmlColor.FromRgb("200 ,55  ,  27");

			Assert.IsNotNull(color);
			Assert.AreEqual("200, 55, 27", color.OriginalValue);
			Assert.AreEqual(false, color.IsNamedColor);
			Assert.AreEqual(true, color.IsValid);

			Assert.IsNotNull(color.RgbColor);
			Assert.AreEqual(200, color.RgbColor.R);
			Assert.AreEqual(55, color.RgbColor.G);
			Assert.AreEqual(27, color.RgbColor.B);

			Assert.IsNotNull(color.HslColor);
			Assert.AreEqual(10, Math.Round(color.HslColor.Hue));
			Assert.AreEqual(76, Math.Round(color.HslColor.Saturation));
			Assert.AreEqual(45, Math.Round(color.HslColor.Luminosity));

			Assert.AreEqual("#C8371B", color.RgbColor.ToHtmlHex());
			Assert.AreEqual("#C8371B", color.HexColor);

			Assert.IsNotNull(color2);
			Assert.AreEqual(false, color2.IsValid);
			Assert.AreEqual(null, color2.HexColor);

			Assert.IsNotNull(color3);
			Assert.AreEqual(false, color3.IsValid);
			Assert.AreEqual(null, color3.HexColor);

			Assert.IsNotNull(color4);
			Assert.AreEqual(false, color4.IsValid);
			Assert.AreEqual(null, color4.HexColor);

			Assert.IsNotNull(color5);
			Assert.AreEqual(true, color5.IsValid);
			Assert.AreEqual("#C8371B", color5.HexColor);

			Assert.IsNotNull(color6);
			Assert.AreEqual(true, color6.IsValid);
			Assert.AreEqual("#C8371B", color6.HexColor);

			Assert.IsNotNull(color7);
			Assert.AreEqual(true, color7.IsValid);
			Assert.AreEqual("#C8371B", color7.HexColor);
		}

		[TestMethod]
		public void HtmlColor_should_be_created_FromHsl_ints()
		{
			var color = HtmlColor.FromHsl(10, 76, 45);

			Assert.IsNotNull(color);
			Assert.AreEqual(null, color.OriginalValue);
			Assert.AreEqual(false, color.IsNamedColor);
			Assert.AreEqual(true, color.IsValid);

			Assert.IsNotNull(color.RgbColor); //Calculation differences...
			Assert.AreEqual(202, color.RgbColor.R);
			Assert.AreEqual(57, color.RgbColor.G);
			Assert.AreEqual(28, color.RgbColor.B);

			Assert.IsNotNull(color.HslColor);
			Assert.AreEqual(10, Math.Round(color.HslColor.Hue));
			Assert.AreEqual(76, Math.Round(color.HslColor.Saturation));
			Assert.AreEqual(45, Math.Round(color.HslColor.Luminosity));

			Assert.AreEqual("#CA391C", color.RgbColor.ToHtmlHex());
			Assert.AreEqual("#CA391C", color.HexColor);
		}

		[TestMethod]
		public void HtmlColor_should_be_created_FromHsl_string()
		{
			var color = HtmlColor.FromHsl("10, 76, 45");
			var color2 = HtmlColor.FromHsl(null);
			var color3 = HtmlColor.FromHsl("");
			var color4 = HtmlColor.FromHsl("    ");
			var color5 = HtmlColor.FromHsl("10°, 76%, 45%");
			var color6 = HtmlColor.FromHsl("10° 76% 45%");
			var color7 = HtmlColor.FromHsl("10°,76%,45%");
			var color8 = HtmlColor.FromHsl("10°76%45%");
			var color9 = HtmlColor.FromHsl("10°,76,45%");
			var color10 = HtmlColor.FromHsl("10,76,45%");
			var color11 = HtmlColor.FromHsl("10°  76 ,45%");

			Assert.IsNotNull(color);
			Assert.AreEqual("10, 76, 45", color.OriginalValue);
			Assert.AreEqual(false, color.IsNamedColor);
			Assert.AreEqual(true, color.IsValid);

			Assert.IsNotNull(color.RgbColor); //Calculation differences...
			Assert.AreEqual(202, color.RgbColor.R);
			Assert.AreEqual(57, color.RgbColor.G);
			Assert.AreEqual(28, color.RgbColor.B);

			Assert.IsNotNull(color.HslColor);
			Assert.AreEqual(10, Math.Round(color.HslColor.Hue));
			Assert.AreEqual(76, Math.Round(color.HslColor.Saturation));
			Assert.AreEqual(45, Math.Round(color.HslColor.Luminosity));

			Assert.AreEqual("#CA391C", color.RgbColor.ToHtmlHex());
			Assert.AreEqual("#CA391C", color.HexColor);

			Assert.IsNotNull(color2);
			Assert.AreEqual(false, color2.IsValid);
			Assert.AreEqual(null, color2.HexColor);

			Assert.IsNotNull(color3);
			Assert.AreEqual(false, color3.IsValid);
			Assert.AreEqual(null, color3.HexColor);

			Assert.IsNotNull(color4);
			Assert.AreEqual(false, color4.IsValid);
			Assert.AreEqual(null, color4.HexColor);

			Assert.IsNotNull(color5);
			Assert.AreEqual(true, color5.IsValid);
			Assert.AreEqual("#CA391C", color5.HexColor);

			Assert.IsNotNull(color6);
			Assert.AreEqual(true, color6.IsValid);
			Assert.AreEqual("#CA391C", color6.HexColor);

			Assert.IsNotNull(color7);
			Assert.AreEqual(true, color7.IsValid);
			Assert.AreEqual("#CA391C", color7.HexColor);

			Assert.IsNotNull(color8);
			Assert.AreEqual(true, color8.IsValid);
			Assert.AreEqual("#CA391C", color8.HexColor);

			Assert.IsNotNull(color9);
			Assert.AreEqual(true, color9.IsValid);
			Assert.AreEqual("#CA391C", color9.HexColor);

			Assert.IsNotNull(color10);
			Assert.AreEqual(true, color10.IsValid);
			Assert.AreEqual("#CA391C", color10.HexColor);

			Assert.IsNotNull(color11);
			Assert.AreEqual(true, color11.IsValid);
			Assert.AreEqual("#CA391C", color11.HexColor);
		}

		[TestMethod]
		public void HtmlColor_should_be_created_named_color_FromHsl_ints()
		{
			var color = HtmlColor.FromHsl(0, 100, 50);

			Assert.IsNotNull(color);
			Assert.AreEqual(null, color.OriginalValue);
			Assert.AreEqual(true, color.IsNamedColor);
			Assert.AreEqual("Red", color.ColorName);
			Assert.AreEqual(true, color.IsValid);

			Assert.IsNotNull(color.RgbColor);
			Assert.AreEqual(255, color.RgbColor.R);
			Assert.AreEqual(0, color.RgbColor.G);
			Assert.AreEqual(0, color.RgbColor.B);

			Assert.IsNotNull(color.HslColor);
			Assert.AreEqual(0, Math.Round(color.HslColor.Hue));
			Assert.AreEqual(100, Math.Round(color.HslColor.Saturation));
			Assert.AreEqual(50, Math.Round(color.HslColor.Luminosity));

			Assert.AreEqual("#FF0000", color.RgbColor.ToHtmlHex());
			Assert.AreEqual("#FF0000", color.HexColor);
		}

		[TestMethod]
		public void HtmlColor_should_be_created_FromHex()
		{
			var color = HtmlColor.FromHex("#ff32C5");
			var color2 = HtmlColor.FromHex(null);
			var color3 = HtmlColor.FromHex(string.Empty);
			var color4 = HtmlColor.FromHex("   ");
			var color5 = HtmlColor.FromHex("12345");
			var color6 = HtmlColor.FromHex("ff32C5");

			Assert.IsNotNull(color);
			Assert.AreEqual("#ff32C5", color.OriginalValue);
			Assert.AreEqual(false, color.IsNamedColor);
			Assert.AreEqual(true, color.IsValid);

			Assert.IsNotNull(color.RgbColor); //Calculation differences...
			Assert.AreEqual(255, color.RgbColor.R);
			Assert.AreEqual(50, color.RgbColor.G);
			Assert.AreEqual(197, color.RgbColor.B);

			Assert.IsNotNull(color.HslColor);
			Assert.AreEqual(317, Math.Round(color.HslColor.Hue));
			Assert.AreEqual(100, Math.Round(color.HslColor.Saturation));
			Assert.AreEqual(60, Math.Round(color.HslColor.Luminosity));

			Assert.AreEqual("#FF32C5", color.RgbColor.ToHtmlHex());
			Assert.AreEqual("#FF32C5", color.HexColor);

			Assert.IsNotNull(color2);
			Assert.AreEqual(false, color2.IsValid);

			Assert.IsNotNull(color3);
			Assert.AreEqual(false, color3.IsValid);

			Assert.IsNotNull(color4);
			Assert.AreEqual(false, color4.IsValid);

			Assert.IsNotNull(color5);
			Assert.AreEqual(false, color5.IsValid);

			Assert.IsNotNull(color6);
			Assert.AreEqual(true, color6.IsValid);
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