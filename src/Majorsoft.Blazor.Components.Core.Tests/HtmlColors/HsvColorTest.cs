using System;
using System.Drawing;

using Majorsoft.Blazor.Components.Core.HtmlColors;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Majorsoft.Blazor.Components.Core.Tests.HtmlColors
{
	[TestClass]
	public class HsvColorTest
	{
		[TestMethod]
		public void Should_Hsv_validate_input()
		{
			var hsv = new HsvColor(-1, -1, -1);
			var hsv2 = new HsvColor(400, 110, 110);

			Assert.AreEqual(0, hsv.Hue);
			Assert.AreEqual(0, hsv.Saturation);
			Assert.AreEqual(0, hsv.Value);

			Assert.AreEqual(360, hsv2.Hue);
			Assert.AreEqual(100, hsv2.Saturation);
			Assert.AreEqual(100, hsv2.Value);
		}

		[TestMethod]
		public void Should_convert_from_Hsv_to_Rgb()
		{
			var hsv = new HsvColor(0, 0, 0);
			var hsv2 = new HsvColor(0, 100, 100);
			var hsv3 = new HsvColor(120, 100, 100);
			var hsv4 = new HsvColor(240, 100, 100);
			var hsv5 = new HsvColor(360, 100, 100);
			var hsv6 = new HsvColor(0, 0, 100);

			Assert.AreEqual(0, hsv.Hue);
			Assert.AreEqual(0, hsv.Saturation);
			Assert.AreEqual(0, hsv.Value);
			Assert.AreEqual(Color.FromArgb(0, 0, 0).Name, ((Color)hsv).Name);

			Assert.AreEqual(0, hsv2.Hue);
			Assert.AreEqual(100, hsv2.Saturation);
			Assert.AreEqual(100, hsv2.Value);
			Assert.AreEqual(Color.FromArgb(255, 0, 0).Name, ((Color)hsv2).Name);

			Assert.AreEqual(120, hsv3.Hue);
			Assert.AreEqual(100, hsv3.Saturation);
			Assert.AreEqual(100, hsv3.Value);
			Assert.AreEqual(Color.FromArgb(0, 255, 0).Name, ((Color)hsv3).Name);

			Assert.AreEqual(240, hsv4.Hue);
			Assert.AreEqual(100, hsv4.Saturation);
			Assert.AreEqual(100, hsv4.Value);
			Assert.AreEqual(Color.FromArgb(0, 0, 255).Name, ((Color)hsv4).Name);

			Assert.AreEqual(360, hsv5.Hue);
			Assert.AreEqual(100, hsv5.Saturation);
			Assert.AreEqual(100, hsv5.Value);
			Assert.AreEqual(Color.FromArgb(255, 0, 0).Name, ((Color)hsv5).Name);

			Assert.AreEqual(0, hsv6.Hue);
			Assert.AreEqual(0, hsv6.Saturation);
			Assert.AreEqual(100, hsv6.Value);
			Assert.AreEqual(Color.FromArgb(255, 255, 255).Name, ((Color)hsv6).Name);
		}

		[TestMethod]
		public void Should_Hsv_ToString_formatted()
		{
			var hsv = new HsvColor(77, 100, 50);

			Assert.AreEqual(77, hsv.Hue);
			Assert.AreEqual(100, hsv.Saturation);
			Assert.AreEqual(50, hsv.Value);
			Assert.AreEqual("H: 77 S: 100 V: 50", hsv.ToString());
		}

		[TestMethod]
		public void Should_Hsv_ToHsvString_formatted()
		{
			var hsv = new HsvColor(77, 100, 50);

			Assert.AreEqual(77, hsv.Hue);
			Assert.AreEqual(100, hsv.Saturation);
			Assert.AreEqual(50, hsv.Value);
			Assert.AreEqual("77°, 100%, 50%", hsv.ToHsvString());
		}

		[TestMethod]
		public void Should_Hsv_ToRGBString_formatted()
		{
			var hsv = new HsvColor(77, 100, 50);

			Assert.AreEqual(77, hsv.Hue);
			Assert.AreEqual(100, hsv.Saturation);
			Assert.AreEqual(50, hsv.Value);
			Assert.AreEqual("R: 91 G: 128 B: 0", hsv.ToRGBString());
		}

		[TestMethod]
		public void Should_convert_from_RGB_to_HSV()
		{
			var hsv = HsvColor.FromRgb(Color.FromArgb(77, 55, 20));
			var hsv2 = HsvColor.FromRgb(Color.FromArgb(255, 0, 0));
			var hsv3 = HsvColor.FromRgb(Color.FromArgb(0, 255, 0));
			var hsv4 = HsvColor.FromRgb(Color.FromArgb(0, 0, 255));
			var hsv5 = HsvColor.FromRgb(Color.FromArgb(0, 0, 0));
			var hsv6 = HsvColor.FromRgb(Color.FromArgb(255, 255, 255));

			Assert.AreEqual(37, Math.Round(hsv.Hue));
			Assert.AreEqual(74, Math.Round(hsv.Saturation));
			Assert.AreEqual(30, Math.Round(hsv.Value));

			Assert.AreEqual(0, Math.Round(hsv2.Hue));
			Assert.AreEqual(100, Math.Round(hsv2.Saturation));
			Assert.AreEqual(100, Math.Round(hsv2.Value));
			Assert.AreEqual(Color.FromArgb(255, 0, 0).Name, ((Color)hsv2).Name);

			Assert.AreEqual(120, Math.Round(hsv3.Hue));
			Assert.AreEqual(100, Math.Round(hsv3.Saturation));
			Assert.AreEqual(100, Math.Round(hsv3.Value));
			Assert.AreEqual(Color.FromArgb(0, 255, 0).Name, ((Color)hsv3).Name);

			Assert.AreEqual(240, Math.Round(hsv4.Hue));
			Assert.AreEqual(100, Math.Round(hsv4.Saturation));
			Assert.AreEqual(100, Math.Round(hsv4.Value));
			Assert.AreEqual(Color.FromArgb(0, 0, 255).Name, ((Color)hsv4).Name);

			Assert.AreEqual(0, Math.Round(hsv5.Hue));
			Assert.AreEqual(0, Math.Round(hsv5.Saturation));
			Assert.AreEqual(0, Math.Round(hsv5.Value));
			Assert.AreEqual(Color.FromArgb(0, 0, 0).Name, ((Color)hsv5).Name);

			Assert.AreEqual(0, Math.Round(hsv6.Hue));
			Assert.AreEqual(0, Math.Round(hsv6.Saturation));
			Assert.AreEqual(100, Math.Round(hsv6.Value));
			Assert.AreEqual(Color.FromArgb(255, 255, 255).Name, ((Color)hsv6).Name);
		}

		[TestMethod]
		public void Should_keep_hue_for_grayscale_when_round_tripping()
		{
			//Saturation 0 means gray: Hue is undefined and reported as 0.
			var hsv = HsvColor.FromRgb(Color.FromArgb(128, 128, 128));

			Assert.AreEqual(0, Math.Round(hsv.Hue));
			Assert.AreEqual(0, Math.Round(hsv.Saturation));
			Assert.AreEqual(50, Math.Round(hsv.Value));
		}
	}
}
