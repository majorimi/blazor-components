using System;
using System.Drawing;

using Majorsoft.Blazor.Components.Core.HtmlColors;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Majorsoft.Blazor.Components.Core.Tests.HtmlColors
{
	[TestClass]
	public class HslColorTest
	{
		[TestMethod]
		public void Should_Hsl_validate_input()
		{
			var hsl = new HslColor(-1, -1, -1);
			var hsl2 = new HslColor(400, 110, 110);

			Assert.AreEqual(0, hsl.Hue);
			Assert.AreEqual(0, hsl.Saturation);
			Assert.AreEqual(0, hsl.Luminosity);

			Assert.AreEqual(360, hsl2.Hue);
			Assert.AreEqual(100, hsl2.Saturation);
			Assert.AreEqual(100, hsl2.Luminosity);
		}

		[TestMethod]
		public void Should_convert_from_Hsl_to_Rgb()
		{
			var hsl = new HslColor(0, 0, 0);
			var hsl2 = new HslColor(0, 100, 50);
			var hsl3 = new HslColor(120, 100, 50);
			var hsl4 = new HslColor(240, 100, 50);
			var hsl5 = new HslColor(360, 100, 50);
			var hsl6 = new HslColor(37, 59, 19);

			Assert.AreEqual(0, hsl.Hue);
			Assert.AreEqual(0, hsl.Saturation);
			Assert.AreEqual(0, hsl.Luminosity);
			Assert.AreEqual(Color.FromArgb(0, 0, 0).Name, ((Color)hsl).Name);

			Assert.AreEqual(0, hsl2.Hue);
			Assert.AreEqual(100, hsl2.Saturation);
			Assert.AreEqual(50, hsl2.Luminosity);
			Assert.AreEqual(Color.FromArgb(255, 0, 0).Name, ((Color)hsl2).Name);

			Assert.AreEqual(120, hsl3.Hue);
			Assert.AreEqual(100, hsl3.Saturation);
			Assert.AreEqual(50, hsl3.Luminosity);
			Assert.AreEqual(Color.FromArgb(0, 255, 0).Name, ((Color)hsl3).Name);

			Assert.AreEqual(240, hsl4.Hue);
			Assert.AreEqual(100, hsl4.Saturation);
			Assert.AreEqual(50, hsl4.Luminosity);
			Assert.AreEqual(Color.FromArgb(0, 0, 255).Name, ((Color)hsl4).Name);

			Assert.AreEqual(360, hsl5.Hue);
			Assert.AreEqual(100, hsl5.Saturation);
			Assert.AreEqual(50, hsl5.Luminosity);
			Assert.AreEqual(Color.FromArgb(255, 0, 0).Name, ((Color)hsl5).Name);

			Assert.AreEqual(37, hsl6.Hue);
			Assert.AreEqual(59, hsl6.Saturation);
			Assert.AreEqual(19, hsl6.Luminosity);
			Assert.AreEqual(Color.FromArgb(77, 55, 20).Name, ((Color)hsl6).Name);
		}

		[TestMethod]
		public void Should_Hsl_ToString_formatted()
		{
			var hsl = new HslColor(77, 100, 50);

			Assert.AreEqual(77, hsl.Hue);
			Assert.AreEqual(100, hsl.Saturation);
			Assert.AreEqual(50, hsl.Luminosity);
			Assert.AreEqual("H: 77 S: 100 L: 50", hsl.ToString());
		}

		[TestMethod]
		public void Should_Hsl_ToHslString_formatted()
		{
			var hsl = new HslColor(77, 100, 50);

			Assert.AreEqual(77, hsl.Hue);
			Assert.AreEqual(100, hsl.Saturation);
			Assert.AreEqual(50, hsl.Luminosity);
			Assert.AreEqual("77°, 100%, 50%", hsl.ToHslString());
		}

		[TestMethod]
		public void Should_Hsl_ToRGBString_formatted()
		{
			var hsl = new HslColor(77, 100, 50);

			Assert.AreEqual(77, hsl.Hue);
			Assert.AreEqual(100, hsl.Saturation);
			Assert.AreEqual(50, hsl.Luminosity);
			Assert.AreEqual("R: 183 G: 255 B: 0", hsl.ToRGBString());
		}

		[TestMethod]
		public void Should_convert_from_RGB_to_HSL()
		{
			var hsl = HslColor.FromRgb(Color.FromArgb(77, 55, 20));
			var hsl2 = HslColor.FromRgb(Color.FromArgb(255, 0, 0));
			var hsl3 = HslColor.FromRgb(Color.FromArgb(0, 255, 0));
			var hsl4 = HslColor.FromRgb(Color.FromArgb(0, 0, 255));
			var hsl5 = HslColor.FromRgb(Color.FromArgb(0, 0, 0));

			Assert.AreEqual(37, Math.Round(hsl.Hue));
			Assert.AreEqual(59, Math.Round(hsl.Saturation));
			Assert.AreEqual(19, Math.Round(hsl.Luminosity));
			Assert.AreEqual(Color.FromArgb(77, 55, 20).Name, ((Color)hsl).Name);

			Assert.AreEqual(360, Math.Round(hsl2.Hue));
			Assert.AreEqual(100, Math.Round(hsl2.Saturation));
			Assert.AreEqual(50, Math.Round(hsl2.Luminosity));
			Assert.AreEqual(Color.FromArgb(255, 0, 0).Name, ((Color)hsl2).Name);

			Assert.AreEqual(120, Math.Round(hsl3.Hue));
			Assert.AreEqual(100, Math.Round(hsl3.Saturation));
			Assert.AreEqual(50, Math.Round(hsl3.Luminosity));
			Assert.AreEqual(Color.FromArgb(0, 255, 0).Name, ((Color)hsl3).Name);

			Assert.AreEqual(240, Math.Round(hsl4.Hue));
			Assert.AreEqual(100, Math.Round(hsl4.Saturation));
			Assert.AreEqual(50, Math.Round(hsl4.Luminosity));
			Assert.AreEqual(Color.FromArgb(0, 0, 255).Name, ((Color)hsl4).Name);

			Assert.AreEqual(0, Math.Round(hsl5.Hue));
			Assert.AreEqual(0, Math.Round(hsl5.Saturation));
			Assert.AreEqual(0, Math.Round(hsl5.Luminosity));
			Assert.AreEqual(Color.FromArgb(0, 0, 0).Name, ((Color)hsl5).Name);
		}
	}
}