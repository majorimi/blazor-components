using Blazor.Components.Core.HtmlColors;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Blazor.Components.Core.Tests.HtmlColors
{
	[TestClass]
	public class RgbTest
	{
		[TestMethod]
		public void Rgb_should_have_correct_ToString()
		{
			var rgb = new Rgb(0, 0, 0);
			var rgb2 = new Rgb(255, 255, 255);
			var rgb3 = new Rgb(26, 78, 55);

			Assert.AreEqual("0, 0, 0", rgb.ToString());
			Assert.AreEqual("255, 255, 255", rgb2.ToString());
			Assert.AreEqual("26, 78, 55", rgb3.ToString());
		}

		[TestMethod]
		public void Rgb_should_have_correct_ToHex()
		{
			var rgb = new Rgb(0, 0, 0);
			var rgb2 = new Rgb(255, 255, 255);
			var rgb3 = new Rgb(26, 78, 55);

			Assert.AreEqual("000000", rgb.ToHex());
			Assert.AreEqual("FFFFFF", rgb2.ToHex());
			Assert.AreEqual("1A4E37", rgb3.ToHex());
		}

		[TestMethod]
		public void Rgb_should_have_correct_ToHtmlHex()
		{
			var rgb = new Rgb(0, 0, 0);
			var rgb2 = new Rgb(255, 255, 255);
			var rgb3 = new Rgb(26, 78, 55);

			Assert.AreEqual("#000000", rgb.ToHtmlHex());
			Assert.AreEqual("#FFFFFF", rgb2.ToHtmlHex());
			Assert.AreEqual("#1A4E37", rgb3.ToHtmlHex());
		}
	}
}