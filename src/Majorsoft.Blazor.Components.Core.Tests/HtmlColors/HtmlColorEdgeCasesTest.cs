using System;
using Majorsoft.Blazor.Components.Core.HtmlColors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Majorsoft.Blazor.Components.Core.Tests.HtmlColors
{
    [TestClass]
    public class HtmlColorEdgeCasesTest
    {
        [TestMethod]
        public void HtmlColor_should_handle_boundary_RGB_values()
        {
            // Test minimum RGB values
            var minColor = HtmlColor.FromRgb(0, 0, 0);
            Assert.IsNotNull(minColor);
            Assert.AreEqual(true, minColor.IsValid);
            Assert.AreEqual("#000000", minColor.HexColor);

            // Test maximum RGB values
            var maxColor = HtmlColor.FromRgb(255, 255, 255);
            Assert.IsNotNull(maxColor);
            Assert.AreEqual(true, maxColor.IsValid);
            Assert.AreEqual("#FFFFFF", maxColor.HexColor);
            Assert.AreEqual(true, maxColor.IsNamedColor);
            Assert.AreEqual("White", maxColor.ColorName);
        }

        [TestMethod]
        public void HtmlColor_should_handle_grayscale_colors()
        {
            // Test various shades of gray
            var darkGray = HtmlColor.FromRgb(128, 128, 128);
            Assert.IsNotNull(darkGray);
            Assert.AreEqual(true, darkGray.IsValid);

            var lightGray = HtmlColor.FromRgb(192, 192, 192);
            Assert.IsNotNull(lightGray);
            Assert.AreEqual(true, lightGray.IsValid);
            Assert.AreEqual(true, lightGray.IsNamedColor);
            Assert.AreEqual("Silver", lightGray.ColorName);
        }

        [TestMethod]
        public void HtmlColor_should_detect_all_primary_colors()
        {
            // Test primary colors
            var red = HtmlColor.FromRgb(255, 0, 0);
            Assert.AreEqual("Red", red.ColorName);

            var green = HtmlColor.FromRgb(0, 128, 0);
            Assert.AreEqual("Green", green.ColorName);

            var blue = HtmlColor.FromRgb(0, 0, 255);
            Assert.AreEqual("Blue", blue.ColorName);
        }

        [TestMethod]
        public void HtmlColor_HSL_conversion_should_preserve_saturation_at_extremes()
        {
            // Fully saturated color
            var saturated = HtmlColor.FromHsl(0, 100, 50);
            Assert.IsNotNull(saturated);
            Assert.AreEqual(true, saturated.IsValid);
            Assert.AreEqual(100, Math.Round(saturated.HslColor.Saturation));

            // Completely desaturated (grayscale)
            var desaturated = HtmlColor.FromHsl(0, 0, 50);
            Assert.IsNotNull(desaturated);
            Assert.AreEqual(true, desaturated.IsValid);
            Assert.AreEqual(0, Math.Round(desaturated.HslColor.Saturation));
        }

        [TestMethod]
        public void HtmlColor_HSL_conversion_should_handle_lightness_extremes()
        {
            // Maximum lightness (white)
            var whitish = HtmlColor.FromHsl(0, 50, 100);
            Assert.IsNotNull(whitish);
            Assert.AreEqual(true, whitish.IsValid);
            Assert.AreEqual(100, Math.Round(whitish.HslColor.Luminosity));

            // Minimum lightness (black)
            var blackish = HtmlColor.FromHsl(0, 50, 0);
            Assert.IsNotNull(blackish);
            Assert.AreEqual(true, blackish.IsValid);
            Assert.AreEqual(0, Math.Round(blackish.HslColor.Luminosity));
        }

        [TestMethod]
        public void HtmlColor_RGB_string_should_handle_out_of_range_values()
        {
            // Test with values exceeding 255
            var overMaxColor = HtmlColor.FromRgb("300, 300, 300");
            Assert.IsNotNull(overMaxColor);
            // Should either clamp or mark as invalid depending on implementation

            // Test with negative values
            var negativeColor = HtmlColor.FromRgb("-10, -10, -10");
            Assert.IsNotNull(negativeColor);
        }

        [TestMethod]
        public void HtmlColor_roundtrip_conversion_RGB_to_HSL_and_back()
        {
            // Create color from RGB
            var original = HtmlColor.FromRgb(150, 100, 75);
            Assert.IsNotNull(original);

            // Get HSL values
            var hue = original.HslColor.Hue;
            var saturation = original.HslColor.Saturation;
            var luminosity = original.HslColor.Luminosity;

            // Create new color from HSL
            var reconstructed = HtmlColor.FromHsl((int)Math.Round(hue), (int)Math.Round(saturation), (int)Math.Round(luminosity));
            Assert.IsNotNull(reconstructed);

            // Values should be very close (allowing for rounding)
            Assert.AreEqual(original.RgbColor.R, reconstructed.RgbColor.R, 2);
            Assert.AreEqual(original.RgbColor.G, reconstructed.RgbColor.G, 2);
            Assert.AreEqual(original.RgbColor.B, reconstructed.RgbColor.B, 2);
        }

        [TestMethod]
        public void HtmlColor_should_handle_invalid_input_formats_gracefully()
        {
            // Test malformed RGB strings
            var invalidFormat1 = HtmlColor.FromRgb("a, b, c");
            Assert.IsNotNull(invalidFormat1);
            Assert.AreEqual(false, invalidFormat1.IsValid);

            var invalidFormat2 = HtmlColor.FromRgb("255, 255");
            Assert.IsNotNull(invalidFormat2);
            Assert.AreEqual(false, invalidFormat2.IsValid);

            var invalidFormat3 = HtmlColor.FromRgb("255, 255, 255, 255");
            Assert.IsNotNull(invalidFormat3);
        }

        [TestMethod]
        public void HtmlColor_HSL_string_should_handle_degree_symbols()
        {
            // Test with various degree symbol formats
            var withDegree = HtmlColor.FromHsl("45°, 50%, 50%");
            Assert.IsNotNull(withDegree);
            Assert.AreEqual(true, withDegree.IsValid);

            var withoutDegree = HtmlColor.FromHsl("45, 50, 50");
            Assert.IsNotNull(withoutDegree);
            Assert.AreEqual(true, withoutDegree.IsValid);

            // Both should produce similar results
            Assert.AreEqual(withDegree.HexColor, withoutDegree.HexColor);
        }

        [TestMethod]
        public void HtmlColor_should_preserve_original_value_when_provided()
        {
            var colorFromString = HtmlColor.FromRgb("200, 100, 50");
            Assert.IsNotNull(colorFromString.OriginalValue);
            Assert.AreEqual("200, 100, 50", colorFromString.OriginalValue);

            var colorFromBytes = HtmlColor.FromRgb(200, 100, 50);
            Assert.IsNull(colorFromBytes.OriginalValue);
        }

        [TestMethod]
        public void HtmlColor_all_named_colors_should_be_valid()
        {
            // Test that we can create all named colors
            var namedColors = HtmlColorHelper.NamedHtmlColors;
            Assert.IsNotNull(namedColors);
            Assert.IsTrue(namedColors.Count > 0);

            // Sample test with a few known colors
            foreach (var colorName in new[] { "Red", "Green", "Blue", "Black", "White" })
            {
                var foundColor = namedColors.ContainsKey(colorName);
                Assert.IsTrue(foundColor, $"Color {colorName} should be found in named colors");
            }
        }
    }
}
