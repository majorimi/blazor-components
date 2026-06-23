using System;
using System.Drawing;

namespace Majorsoft.Blazor.Components.Core.HtmlColors
{
	public class HsvColor
	{
		public double Hue { get; init; }
		public double Saturation { get; init; }
		public double Value { get; init; }

		/// <summary>
		/// HSV stands for hue, saturation, and value (a.k.a. brightness). It is the model used by 2D
		/// square color pickers where one axis is saturation and the other is value/brightness.
		/// </summary>
		/// <param name="hue">Hue is a degree on the color wheel from 0 to 360. 0 is red, 120 is green, 240 is blue.</param>
		/// <param name="saturation">Saturation is a percentage value; 0% means a shade of gray and 100% is the full color.</param>
		/// <param name="value">Value/brightness is also a percentage; 0% is black, 100% is the brightest the color can be.</param>
		public HsvColor(double hue, double saturation, double value)
		{
			Hue = CheckRange(hue, 360.0);
			Saturation = CheckRange(saturation, 100.0);
			Value = CheckRange(value, 100.0);
		}

		public override string ToString()
		{
			return $"H: {Hue:#0.##} S: {Saturation:#0.##} V: {Value:#0.##}";
		}

		public string ToHsvString()
		{
			return $"{Math.Round(Hue)}°, {Math.Round(Saturation)}%, {Math.Round(Value)}%";
		}

		public string ToRGBString()
		{
			var color = (Color)this;
			return $"R: {color.R:#0.##} G: {color.G:#0.##} B: {color.B:#0.##}";
		}

		public static implicit operator Color(HsvColor hsvColor)
		{
			return HsvToRgb(hsvColor.Hue, hsvColor.Saturation, hsvColor.Value);
		}

		private static Color HsvToRgb(double hue, double saturation, double value)
		{
			hue %= 360.0;
			if (hue < 0.0)
			{
				hue += 360.0;
			}

			var s = saturation / 100.0;
			var v = value / 100.0;

			var c = v * s;
			var x = c * (1.0 - Math.Abs((hue / 60.0) % 2.0 - 1.0));
			var m = v - c;

			double r, g, b;
			if (hue < 60.0) { r = c; g = x; b = 0.0; }
			else if (hue < 120.0) { r = x; g = c; b = 0.0; }
			else if (hue < 180.0) { r = 0.0; g = c; b = x; }
			else if (hue < 240.0) { r = 0.0; g = x; b = c; }
			else if (hue < 300.0) { r = x; g = 0.0; b = c; }
			else { r = c; g = 0.0; b = x; }

			return Color.FromArgb(
				(int)Math.Round((r + m) * 255.0),
				(int)Math.Round((g + m) * 255.0),
				(int)Math.Round((b + m) * 255.0));
		}

		private double CheckRange(double value, double max)
		{
			if (value < 0.0)
				value = 0.0;
			else if (value > max)
				value = max;

			return value;
		}

		public static HsvColor FromRgb(Color rgb)
		{
			double r = rgb.R / 255.0;
			double g = rgb.G / 255.0;
			double b = rgb.B / 255.0;

			var max = Math.Max(r, Math.Max(g, b));
			var min = Math.Min(r, Math.Min(g, b));
			var delta = max - min;

			var hue = 0.0;
			if (delta > 0.0)
			{
				if (max == r)
				{
					hue = 60.0 * (((g - b) / delta) % 6.0);
				}
				else if (max == g)
				{
					hue = 60.0 * (((b - r) / delta) + 2.0);
				}
				else
				{
					hue = 60.0 * (((r - g) / delta) + 4.0);
				}
			}
			if (hue < 0.0)
			{
				hue += 360.0;
			}

			var saturation = max == 0.0 ? 0.0 : delta / max;

			return new HsvColor(hue, saturation * 100.0, max * 100.0);
		}
	}
}
