using System;

using Blazor.Components.Core.Extensions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Blazor.Components.Core.Tests.Extensions
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

			Assert.AreEqual(r, 255);
			Assert.AreEqual(g, 255);
			Assert.AreEqual(b, 255);
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

			Assert.AreEqual(r, 0);
			Assert.AreEqual(g, 0);
			Assert.AreEqual(b, 0);
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

			Assert.AreEqual(r, 0);
			Assert.AreEqual(g, 0);
			Assert.AreEqual(b, 0);
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

			Assert.AreEqual(r, 0);
			Assert.AreEqual(g, 0);
			Assert.AreEqual(b, 255);
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

			Assert.AreEqual(Math.Round(r, 2), 151.39);
			Assert.AreEqual(Math.Round(g, 2), 151.39);
			Assert.AreEqual(Math.Round(b, 2), 240.72);
		}
	}
}