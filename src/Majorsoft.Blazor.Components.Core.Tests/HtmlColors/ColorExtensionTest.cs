using System.Drawing;

using Majorsoft.Blazor.Components.Core.HtmlColors;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Majorsoft.Blazor.Components.Core.Tests.HtmlColors
{
	[TestClass]
	public class ColorExtensionTest
	{
		[TestMethod]
		public void ColorExtension_should_have_correct_ToString()
		{
			var rgb = Color.FromArgb(0, 0, 0);
			var rgb2 = Color.FromArgb(255, 255, 255);
			var rgb3 = Color.FromArgb(26, 78, 55);

			Assert.AreEqual("0, 0, 0", rgb.ToRgbString());
			Assert.AreEqual("255, 255, 255", rgb2.ToRgbString());
			Assert.AreEqual("26, 78, 55", rgb3.ToRgbString());
		}

		[TestMethod]
		public void ColorExtension_should_have_correct_ToHex()
		{
			var rgb = Color.FromArgb(0, 0, 0);
			var rgb2 = Color.FromArgb(255, 255, 255);
			var rgb3 = Color.FromArgb(26, 78, 55);

			Assert.AreEqual("000000", rgb.ToHex());
			Assert.AreEqual("FFFFFF", rgb2.ToHex());
			Assert.AreEqual("1A4E37", rgb3.ToHex());
		}

		[TestMethod]
		public void ColorExtension_should_have_correct_ToHtmlHex()
		{
			var rgb = Color.FromArgb(0, 0, 0);
			var rgb2 = Color.FromArgb(255, 255, 255);
			var rgb3 = Color.FromArgb(26, 78, 55);

			Assert.AreEqual("#000000", rgb.ToHtmlHex());
			Assert.AreEqual("#FFFFFF", rgb2.ToHtmlHex());
			Assert.AreEqual("#1A4E37", rgb3.ToHtmlHex());
		}
	}
}