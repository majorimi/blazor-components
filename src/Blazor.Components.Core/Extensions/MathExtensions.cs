namespace Blazor.Components.Core.Extensions
{
	public static class MathExtensions
	{

		/// <summary>
		/// https://www.ajdesigner.com/phpinterpolation/bilinear_interpolation_equation.php#ajscroll
		/// </summary>
		/// <param name="x1"></param>
		/// <param name="y1"></param>
		/// <param name="x2"></param>
		/// <param name="y2"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="q11"></param>
		/// <param name="q12"></param>
		/// <param name="q21"></param>
		/// <param name="q22"></param>
		/// <returns></returns>
		public static double BilinearInterpolationCalculator(
			int x1, int y1, int x2, int y2, //corner point coordinates
			double x, double y,                 //click coordinates
			int q11, int q12, int q21, int q22) //corner values (R,G,B)
		{
			double ret =
			((((x2 - x) * (y2 - y)) / ((x2 - x1) * (y2 - y1))) * q11) +
			((((x - x1) * (y2 - y)) / ((x2 - x1) * (y2 - y1))) * q21) +
			((((x2 - x) * (y - y1)) / ((x2 - x1) * (y2 - y1))) * q12) +
			((((x - x1) * (y - y1)) / ((x2 - x1) * (y2 - y1))) * q22);

			return ret;
		}

		public static double LinearInterpolationCalculator(
			int x1, double y1, int x3, double y3, //linear points with values
			int x2) //middle coordinate
		{
			double y2 = (((x2 - x1) * (y3 - y1)) / (x3 - x1)) + y1;

			return y2;
		}

		public static double ReverseLinearInterpolationCalculator(
			int x1, double y1, int x3, double y3, //linear points with values
			double y2) //middle value
		{
			double x2 = (((y2 - y1) * (x3 - x1)) / (y3 - y1)) + x1;

			return x2;
		}
	}
}