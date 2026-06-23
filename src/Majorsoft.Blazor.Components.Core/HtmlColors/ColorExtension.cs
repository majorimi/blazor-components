using System;
using System.Drawing;
using System.Globalization;

namespace Majorsoft.Blazor.Components.Core.HtmlColors
{
	public static class ColorExtension
	{
		public static string ToHtmlHex(this Color c)
		{
			return ColorTranslator.ToHtml(c);
		}

		public static string ToHex(this Color c)
		{
			return $"{c.R:X2}{c.G:X2}{c.B:X2}".ToUpper();
		}

		public static string ToRgbString(this Color c)
		{
			return $"{c.R}, {c.G}, {c.B}";
		}

		public static int ParseInt(object? value, int fallback, int min, int max)
		{
			if (int.TryParse(value?.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
			{
				return Math.Clamp(result, min, max);
			}
			return fallback;
		}

		public static bool TryParseHex(string? text, out Color color)
		{
			color = Color.Black;
			if (string.IsNullOrWhiteSpace(text))
			{
				return false;
			}

			text = text.Trim().TrimStart('#');
			if (text.Length is not (3 or 6 or 8))
			{
				return false;
			}

			if (text.Length == 3) //#RGB shorthand
			{
				text = $"{text[0]}{text[0]}{text[1]}{text[1]}{text[2]}{text[2]}";
			}

			try
			{
				var r = Convert.ToInt32(text.Substring(0, 2), 16);
				var g = Convert.ToInt32(text.Substring(2, 2), 16);
				var b = Convert.ToInt32(text.Substring(4, 2), 16);
				var a = text.Length == 8 ? Convert.ToInt32(text.Substring(6, 2), 16) : 255;
				color = Color.FromArgb(a, r, g, b);
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}