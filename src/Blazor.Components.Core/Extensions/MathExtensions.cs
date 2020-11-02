namespace Blazor.Components.Core.Extensions
{
	public static class MathExtensions
	{
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
	}
}