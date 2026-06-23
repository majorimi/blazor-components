using System.Drawing;
using Majorsoft.Blazor.Components.Core.HtmlColors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Majorsoft.Blazor.Components.Core.Tests.HtmlColors
{
    [TestClass]
    public class HsvColorConversionTest
    {
        [TestMethod]
        public void HsvColor_should_convert_to_black_color()
        {
            var hsv = new HsvColor(0, 0, 0);
            var color = (Color)hsv;

            Assert.AreEqual(0, color.R);
            Assert.AreEqual(0, color.G);
            Assert.AreEqual(0, color.B);
        }

        [TestMethod]
        public void HsvColor_should_convert_to_white_color()
        {
            var hsv = new HsvColor(0, 0, 100);
            var color = (Color)hsv;

            Assert.AreEqual(255, color.R);
            Assert.AreEqual(255, color.G);
            Assert.AreEqual(255, color.B);
        }

        [TestMethod]
        public void HsvColor_should_convert_red_correctly()
        {
            var hsv = new HsvColor(0, 100, 100);
            var color = (Color)hsv;

            Assert.AreEqual(255, color.R);
            Assert.AreEqual(0, color.G);
            Assert.AreEqual(0, color.B);
        }

        [TestMethod]
        public void HsvColor_should_convert_green_correctly()
        {
            var hsv = new HsvColor(120, 100, 100);
            var color = (Color)hsv;

            Assert.AreEqual(0, color.R);
            Assert.AreEqual(255, color.G);
            Assert.AreEqual(0, color.B);
        }

        [TestMethod]
        public void HsvColor_should_convert_blue_correctly()
        {
            var hsv = new HsvColor(240, 100, 100);
            var color = (Color)hsv;

            Assert.AreEqual(0, color.R);
            Assert.AreEqual(0, color.G);
            Assert.AreEqual(255, color.B);
        }

        [TestMethod]
        public void HsvColor_should_handle_hue_360_as_red()
        {
            var hsv = new HsvColor(360, 100, 100);
            var color = (Color)hsv;

            Assert.AreEqual(255, color.R);
            Assert.AreEqual(0, color.G);
            Assert.AreEqual(0, color.B);
        }

        [TestMethod]
        public void HsvColor_should_handle_gray_color()
        {
            var hsv = new HsvColor(0, 0, 50);
            var color = (Color)hsv;

            Assert.AreEqual(128, color.R);
            Assert.AreEqual(128, color.G);
            Assert.AreEqual(128, color.B);
        }

        [TestMethod]
        public void HsvColor_zero_value_is_black_regardless_of_saturation()
        {
            var hsv = new HsvColor(200, 100, 0);
            var color = (Color)hsv;

            Assert.AreEqual(0, color.R);
            Assert.AreEqual(0, color.G);
            Assert.AreEqual(0, color.B);
        }

        [TestMethod]
        public void HsvColor_should_clamp_invalid_hue_values()
        {
            var hsvNegative = new HsvColor(-10, 50, 50);
            var hsvExcessive = new HsvColor(400, 50, 50);

            Assert.AreEqual(0, hsvNegative.Hue);
            Assert.AreEqual(360, hsvExcessive.Hue);
        }

        [TestMethod]
        public void HsvColor_should_clamp_invalid_saturation_values()
        {
            var hsvNegative = new HsvColor(0, -10, 50);
            var hsvExcessive = new HsvColor(0, 110, 50);

            Assert.AreEqual(0, hsvNegative.Saturation);
            Assert.AreEqual(100, hsvExcessive.Saturation);
        }

        [TestMethod]
        public void HsvColor_should_clamp_invalid_value_values()
        {
            var hsvNegative = new HsvColor(0, 50, -10);
            var hsvExcessive = new HsvColor(0, 50, 110);

            Assert.AreEqual(0, hsvNegative.Value);
            Assert.AreEqual(100, hsvExcessive.Value);
        }
    }
}
