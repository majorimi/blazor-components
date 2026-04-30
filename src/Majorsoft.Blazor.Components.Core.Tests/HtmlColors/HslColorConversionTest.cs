using System;
using System.Drawing;
using Majorsoft.Blazor.Components.Core.HtmlColors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Majorsoft.Blazor.Components.Core.Tests.HtmlColors
{
    [TestClass]
    public class HslColorConversionTest
    {
        [TestMethod]
        public void HslColor_should_convert_to_black_color()
        {
            var hsl = new HslColor(0, 0, 0);
            var color = (Color)hsl;

            Assert.AreEqual(0, color.R);
            Assert.AreEqual(0, color.G);
            Assert.AreEqual(0, color.B);
        }

        [TestMethod]
        public void HslColor_should_convert_to_white_color()
        {
            var hsl = new HslColor(0, 0, 100);
            var color = (Color)hsl;

            Assert.AreEqual(255, color.R);
            Assert.AreEqual(255, color.G);
            Assert.AreEqual(255, color.B);
        }

        [TestMethod]
        public void HslColor_should_convert_red_correctly()
        {
            var hsl = new HslColor(0, 100, 50);
            var color = (Color)hsl;

            Assert.AreEqual(255, color.R);
            Assert.AreEqual(0, color.G);
            Assert.AreEqual(0, color.B);
        }

        [TestMethod]
        public void HslColor_should_convert_green_correctly()
        {
            var hsl = new HslColor(120, 100, 50);
            var color = (Color)hsl;

            Assert.AreEqual(0, color.R);
            Assert.AreEqual(255, color.G);
            Assert.AreEqual(0, color.B);
        }

        [TestMethod]
        public void HslColor_should_convert_blue_correctly()
        {
            var hsl = new HslColor(240, 100, 50);
            var color = (Color)hsl;

            Assert.AreEqual(0, color.R);
            Assert.AreEqual(0, color.G);
            Assert.AreEqual(255, color.B);
        }

        [TestMethod]
        public void HslColor_should_handle_hue_360_as_red()
        {
            var hsl = new HslColor(360, 100, 50);
            var color = (Color)hsl;

            Assert.AreEqual(255, color.R);
            Assert.AreEqual(0, color.G);
            Assert.AreEqual(0, color.B);
        }

        [TestMethod]
        public void HslColor_should_handle_gray_color()
        {
            var hsl = new HslColor(0, 0, 50);
            var color = (Color)hsl;

            Assert.AreEqual(128, color.R);
            Assert.AreEqual(128, color.G);
            Assert.AreEqual(128, color.B);
        }

        [TestMethod]
        public void HslColor_should_clamp_invalid_hue_values()
        {
            var hslNegative = new HslColor(-10, 50, 50);
            var hslExcessive = new HslColor(400, 50, 50);

            Assert.AreEqual(0, hslNegative.Hue);
            Assert.AreEqual(360, hslExcessive.Hue);
        }

        [TestMethod]
        public void HslColor_should_clamp_invalid_saturation_values()
        {
            var hslNegative = new HslColor(0, -10, 50);
            var hslExcessive = new HslColor(0, 110, 50);

            Assert.AreEqual(0, hslNegative.Saturation);
            Assert.AreEqual(100, hslExcessive.Saturation);
        }

        [TestMethod]
        public void HslColor_should_clamp_invalid_luminosity_values()
        {
            var hslNegative = new HslColor(0, 50, -10);
            var hslExcessive = new HslColor(0, 50, 110);

            Assert.AreEqual(0, hslNegative.Luminosity);
            Assert.AreEqual(100, hslExcessive.Luminosity);
        }
    }
}
