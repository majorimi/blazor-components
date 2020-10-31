using System;
using System.Drawing;

namespace Blazor.Components.Core.HtmlColors
{
	public class HslColor
	{
		public double Hue { get; init; }
		public double Saturation { get; init; }
		public double Luminosity { get; init; }

		/// <summary>
		/// HSL stands for hue, saturation, and lightness.
		/// </summary>
		/// <param name="hue">Hue is a degree on the color wheel from 0 to 360. 0 is red, 120 is green, 240 is blue.</param>
		/// <param name="saturation">Saturation is a percentage value; 0% means a shade of gray and 100% is the full color.</param>
		/// <param name="luminosity">Luminosity/lightness is also a percentage; 0% is black, 100% is white, 50.0 (normal value)</param>
		public HslColor(double hue, double saturation, double luminosity)
		{
			Hue = CheckRange(hue, 360.0);
			Saturation = CheckRange(saturation, 100.0);
			Luminosity = CheckRange(luminosity, 100.0);
		}

		public override string ToString()
		{
			return $"H: {Hue:#0.##} S: {Saturation:#0.##} L: {Luminosity:#0.##}";
		}

		public string ToRGBString()
		{
			var color = (Color)this;
			return $"R: {color.R:#0.##} G: {color.G:#0.##} B: {color.B:#0.##}";
		}

		public static implicit operator Color(HslColor hslColor)
		{
			return HslToRgb(hslColor.Hue, hslColor.Saturation, hslColor.Luminosity);
		}

		private static Color HslToRgb(double hue, double saturation, double luminosity)
		{
			double red, green, blue;

			var h = hue / 360.0;
			var s = saturation / 100.0;
			var l = luminosity / 100.0;

			if (Math.Abs(s - 0.0) < 0.0001)
			{
				red = l;
				green = l;
				blue = l;
			}
			else
			{
				double var2;

				if (l < 0.5)
				{
					var2 = l * (1.0 + s);
				}
				else
				{
					var2 = (l + s) - (s * l);
				}

				var var1 = 2.0 * l - var2;

				red = hue2Rgb(var1, var2, h + (1.0 / 3.0));
				green = hue2Rgb(var1, var2, h);
				blue = hue2Rgb(var1, var2, h - (1.0 / 3.0));
			}

			var nRed = (int)Math.Round(red * 255.0);
			var nGreen = (int)Math.Round(green * 255.0);
			var nBlue = (int)Math.Round(blue * 255.0);

			return Color.FromArgb(nRed, nGreen, nBlue);
		}

		private static double hue2Rgb(double v1, double v2, double vH)
		{
			if (vH < 0.0)
			{
				vH += 1.0;
			}
			if (vH > 1.0)
			{
				vH -= 1.0;
			}
			if ((6.0 * vH) < 1.0)
			{
				return (v1 + (v2 - v1) * 6.0 * vH);
			}
			if ((2.0 * vH) < 1.0)
			{
				return (v2);
			}
			if ((3.0 * vH) < 2.0)
			{
				return (v1 + (v2 - v1) * ((2.0 / 3.0) - vH) * 6.0);
			}

			return (v1);
		}

		private double CheckRange(double value, double max)
		{
			if (value < 0.0)
				value = 0.0;
			else if (value > max)
				value = max;

			return value;
		}
	}
}