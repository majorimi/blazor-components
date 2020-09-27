using Blazor.Components.Core.HtmlColors;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Blazor.Components.Core.Tests
{
	[TestClass]
	public class HtmlColorTest
	{
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
		public void HtmlColor_should_handle_valid_namedColor()
		{
			var color = new HtmlColor("LightBluE");

			Assert.IsNotNull(color);
			Assert.AreEqual("LightBluE", color.OriginalValue);
			Assert.AreEqual(true, color.IsNamedColor);
			Assert.AreEqual(true, color.IsValid);
			Assert.IsNotNull(color.RgbColor);
			Assert.AreEqual("#ADD8E6", color.HexColor);
		}
	}
}