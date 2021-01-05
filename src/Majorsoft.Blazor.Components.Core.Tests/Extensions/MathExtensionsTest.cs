using System;
using System.Drawing;

using Majorsoft.Blazor.Components.Core.Extensions;
using Majorsoft.Blazor.Components.Core.HtmlColors;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Majorsoft.Blazor.Components.Core.Tests.Extensions
{
	[TestClass]
	public class MathExtensionsTest
	{
		[TestMethod]
		public void BilinearInterpolationCalculator_Top_Left_should_return_correct_color()
		{
			//Top-left white corner with blue hue
			var x = 0;
			var y = 0;

			var r = MathExtensions.BilinearInterpolationCalculator(0, 0, 450, 250, x, y, 255, 0, 0, 0);
			var g = MathExtensions.BilinearInterpolationCalculator(0, 0, 450, 250, x, y, 255, 0, 0, 0);
			var b = MathExtensions.BilinearInterpolationCalculator(0, 0, 450, 250, x, y, 255, 0, 255, 0);

			Assert.AreEqual(255, r);
			Assert.AreEqual(255, g);
			Assert.AreEqual(255, b);
		}

		[TestMethod]
		public void BilinearInterpolationCalculator_Bottom_Left_should_return_correct_color()
		{
			//Bottom-left black corner with blue hue
			var x = 0;
			var y = 250;

			var r = MathExtensions.BilinearInterpolationCalculator(0, 0, 450, 250, x, y, 255, 0, 0, 0);
			var g = MathExtensions.BilinearInterpolationCalculator(0, 0, 450, 250, x, y, 255, 0, 0, 0);
			var b = MathExtensions.BilinearInterpolationCalculator(0, 0, 450, 250, x, y, 255, 0, 255, 0);

			Assert.AreEqual(0, r);
			Assert.AreEqual(0, g);
			Assert.AreEqual(0, b);
		}

		[TestMethod]
		public void BilinearInterpolationCalculator_Bottom_Right_should_return_correct_color()
		{
			//Bottom-right black corner with blue hue
			var x = 450;
			var y = 250;

			var r = MathExtensions.BilinearInterpolationCalculator(0, 0, 450, 250, x, y, 255, 0, 0, 0);
			var g = MathExtensions.BilinearInterpolationCalculator(0, 0, 450, 250, x, y, 255, 0, 0, 0);
			var b = MathExtensions.BilinearInterpolationCalculator(0, 0, 450, 250, x, y, 255, 0, 255, 0);

			Assert.AreEqual(0, r);
			Assert.AreEqual(0, g);
			Assert.AreEqual(0, b);
		}

		[TestMethod]
		public void BilinearInterpolationCalculator_Top_Right_should_return_correct_color()
		{
			//Bottom-right black corner with blue hue
			var x = 450;
			var y = 0;

			var r = MathExtensions.BilinearInterpolationCalculator(0, 0, 450, 250, x, y, 255, 0, 0, 0);
			var g = MathExtensions.BilinearInterpolationCalculator(0, 0, 450, 250, x, y, 255, 0, 0, 0);
			var b = MathExtensions.BilinearInterpolationCalculator(0, 0, 450, 250, x, y, 255, 0, 255, 0);

			Assert.AreEqual(0, r);
			Assert.AreEqual(0, g);
			Assert.AreEqual(255, b);
		}

		[TestMethod]
		public void BilinearInterpolationCalculator_Custom_coordinates_should_return_correct_color()
		{
			//Custom click with blue hue
			var x = 167;
			var y = 14;

			var r = MathExtensions.BilinearInterpolationCalculator(0, 0, 450, 250, x, y, 255, 0, 0, 0);
			var g = MathExtensions.BilinearInterpolationCalculator(0, 0, 450, 250, x, y, 255, 0, 0, 0);
			var b = MathExtensions.BilinearInterpolationCalculator(0, 0, 450, 250, x, y, 255, 0, 255, 0);

			Assert.AreEqual(151.39, Math.Round(r, 2));
			Assert.AreEqual(151.39, Math.Round(g, 2));
			Assert.AreEqual(240.72, Math.Round(b, 2));
		}

		[TestMethod]
		public void LinearInterpolationCalculator_Custom_coordinates_should_return_correct_value()
		{
			var x = 225;

			var r = MathExtensions.LinearInterpolationCalculator(0, 0, 450, 255, x);
			var g = MathExtensions.LinearInterpolationCalculator(0, 0, 255, 255, x);
			var b = MathExtensions.LinearInterpolationCalculator(0, 0, 450, 255, 0);

			Assert.AreEqual(Math.Round(r, 2), 127.5);
			Assert.AreEqual(Math.Round(g, 2), x);
			Assert.AreEqual(Math.Round(b, 2), 0);
		}

		[TestMethod]
		public void ReverseLinearInterpolationCalculator_Custom_coordinates_should_return_correct_coordinate()
		{
			var y = 127.5;

			var r = MathExtensions.ReverseLinearInterpolationCalculator(0, 0, 450, 255, y);
			var g = MathExtensions.ReverseLinearInterpolationCalculator(0, 0, 255, 255, y);
			var b = MathExtensions.ReverseLinearInterpolationCalculator(0, 0, 450, 255, 0);

			Assert.AreEqual(225, Math.Round(r, 2));
			Assert.AreEqual(y, Math.Round(g, 2));
			Assert.AreEqual(0, Math.Round(b, 2));
		}

		[TestMethod]
		public void ReverseLinearInterpolationCalculator_Custom_coordinates_should_return_correct_color_coordinates()
		{
			var color = Color.FromArgb(66, 135, 245);
			var hsl = HslColor.FromRgb(color);
			var hue = (int)Math.Round(hsl.Hue);

			var hslColor = new HslColor(hue, 100, 50);
			var hueColor = (Color)hslColor;

			var rx = MathExtensions.ReverseLinearInterpolationCalculator(0, 255, 450, hueColor.R, color.R);
			var gx = MathExtensions.ReverseLinearInterpolationCalculator(0, 255, 450, hueColor.G, color.G);
			var bx = MathExtensions.ReverseLinearInterpolationCalculator(0, 255, 450, hueColor.B, color.B);

			var ry = MathExtensions.ReverseLinearInterpolationCalculator(0, hueColor.R, 250, 0, color.R);
			var gy = MathExtensions.ReverseLinearInterpolationCalculator(0, hueColor.G, 250, 0, color.G);
			var by = MathExtensions.ReverseLinearInterpolationCalculator(0, hueColor.B, 250, 0, color.B);

			//X: '329', Y: '9,40625'
			Assert.AreEqual(333.53, Math.Round(rx, 2)); //Correct when Hue RGB component is 0
			Assert.AreEqual(343.95, Math.Round(gx, 2));
			Assert.AreEqual(double.NegativeInfinity, Math.Round(bx, 2));

			Assert.AreEqual(double.PositiveInfinity, Math.Round(ry, 2));
			Assert.AreEqual(-94.39, Math.Round(gy, 2));
			Assert.AreEqual(9.8, Math.Round(by, 2)); //Correct when Hue RGB component is 0
		}
	}
}