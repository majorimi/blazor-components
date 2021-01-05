using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Majorsoft.Blazor.Components.Core.HtmlColors
{
	public record HtmlColor
	{
		public string OriginalValue { get; private set; }

		public Color RgbColor { get; private set; }
		public HslColor HslColor { get; private set; }
		public string ColorName { get; private set; }
		public string HexColor { get; private set; }

		public bool IsValid => RgbColor != default
			&& (!string.IsNullOrWhiteSpace(ColorName) || !string.IsNullOrWhiteSpace(HexColor));

		public bool IsNamedColor => !string.IsNullOrWhiteSpace(ColorName);

		private HtmlColor() { }

		public HtmlColor(string value)
		{
			OriginalValue = value;

			if(string.IsNullOrWhiteSpace(value))
			{
				return;
			}

			if (HtmlColorHelper.NamedHtmlColors.ContainsKey(value)) //Named color
			{
				ColorName = HtmlColorHelper.NamedHtmlColors.Keys.Single(x => x.Equals(value, StringComparison.OrdinalIgnoreCase));
				HexColor = $"#{HtmlColorHelper.NamedHtmlColors[value]}";
				RgbColor = HexToHtmlRgb(HexColor);
				HslColor = HslColor.FromRgb(RgbColor);

				return;
			}

			var ret = HtmlColor.FromHex(value); //HEX
			if(!ret.IsValid)
			{
				ret = HtmlColor.FromRgb(value); //RGB

				if (!ret.IsValid)
				{
					ret = HtmlColor.FromHsl(value); //HSL
				}
			}

			if(ret.IsValid)
			{
				RgbColor = ret.RgbColor;
				HexColor = ret.HexColor;
				HslColor = ret.HslColor;
				ColorName = ret.ColorName;
			}
		}

		public static HtmlColor FromHex(string value)
		{
			var ret = new HtmlColor();
			ret.OriginalValue = value;
			if (string.IsNullOrWhiteSpace(value))
			{
				return ret;
			}

			ret.RgbColor = HexToHtmlRgb(value);
			if (ret.RgbColor != default) //Hex color
			{
				ret.HexColor = ret.RgbColor.ToHtmlHex();
				ret.HslColor = HslColor.FromRgb(ret.RgbColor);
				ret.ColorName = HtmlColorHelper.NamedHtmlColors
					.SingleOrDefault(x => x.Value == ret.RgbColor.ToHex()).Key;
			}

			return ret;
		}

		public static HtmlColor FromRgb(string value)
		{
			var ret = new HtmlColor();
			ret.OriginalValue = value;
			if (string.IsNullOrWhiteSpace(value))
			{
				return ret;
			}

			if (IsRgbColor(value)) //RGB
			{
				value = value.Replace(' ', ',');
				var parts = value.Split(',').Where(s => !string.IsNullOrEmpty(s) && s != ",")
					.Select(s => byte.Parse(s.Trim()))
					.ToArray();

				ret.RgbColor = Color.FromArgb(parts[0], parts[1], parts[2]);
				ret.HexColor = ret.RgbColor.ToHtmlHex();
				ret.HslColor = HslColor.FromRgb(ret.RgbColor);

				ret.ColorName = HtmlColorHelper.NamedHtmlColors
					.FirstOrDefault(x => x.Value == ret.RgbColor.ToHex()).Key;
			}

			return ret;
		}
		public static HtmlColor FromRgb(byte r, byte g, byte b)
		{
			var ret = new HtmlColor();

			ret.RgbColor = Color.FromArgb(r, g, b);
			ret.HslColor = HslColor.FromRgb(ret.RgbColor);
			ret.HexColor = ret.RgbColor.ToHtmlHex();

			ret.ColorName = HtmlColorHelper.NamedHtmlColors
				.SingleOrDefault(x => x.Value == ret.RgbColor.ToHex()).Key;

			return ret;
		}

		public static HtmlColor FromHsl(string value)
		{
			var ret = new HtmlColor();
			ret.OriginalValue = value;
			if (string.IsNullOrWhiteSpace(value))
			{
				return ret;
			}

			var match = IsHslColor(value);
			if (match.Success) //HSL
			{
				var parts = match.Groups.Values.Skip(1)
					.Select(s => s.Value?.Trim())
					.Where(x => !string.IsNullOrWhiteSpace(x) && x != "," && x != "%" && x != "°")
					.Select(s => int.Parse(s.Trim()))
					.ToArray();

				ret.HslColor = new HslColor(parts[0], parts[1], parts[2]);
				ret.RgbColor = (Color)ret.HslColor;
				ret.HexColor = ret.RgbColor.ToHtmlHex();

				ret.ColorName = HtmlColorHelper.NamedHtmlColors
					.SingleOrDefault(x => x.Value == ret.RgbColor.ToHex()).Key;
			}

			return ret;
		}
		public static HtmlColor FromHsl(int h, int s, int l)
		{
			var ret = new HtmlColor();
			ret.HslColor = new HslColor(h, s, l);
			ret.RgbColor = (Color)ret.HslColor;
			ret.HexColor = ret.RgbColor.ToHtmlHex();
			ret.ColorName = HtmlColorHelper.NamedHtmlColors
				.SingleOrDefault(x => x.Value == ret.RgbColor.ToHex()).Key;

			return ret;
		}


		private static Color HexToHtmlRgb(string value)
		{
			if (!IsHtmlHexColor(value))
			{
				return default;
			}

			var chunkSize = 2;
			var hex = value.Trim().TrimStart('#');
			var parts = Enumerable.Range(0, hex.Length / chunkSize)
				.Select(i => HexToByte(hex.Substring(i * chunkSize, chunkSize)))
				.ToArray();

			return Color.FromArgb(parts[0].Value, parts[1].Value, parts[2].Value);
		}

		private static bool IsHtmlHexColor(string value)
		{
			var regex = new Regex("^#?[0-9a-fA-F]{6}$", RegexOptions.Compiled | RegexOptions.Singleline);
			return regex.IsMatch(value?.Trim());
		}

		private static bool IsRgbColor(string value)
		{
			const string numberPattern = @"(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)";
			const string separatorPattern = @"(\s*|\s*,{1}\s*)";
			
			var regex = new Regex($"^{numberPattern}{separatorPattern}{numberPattern}{separatorPattern}{numberPattern}$", RegexOptions.Compiled | RegexOptions.Singleline);
			return regex.IsMatch(value?.Trim());
		}

		private static Match IsHslColor(string value)
		{
			const string huePattern = @"(360|3[0-5][0-9]|[012]?[0-9][0-9]?)(\s*°)?";
			const string percentagePattern = @"(100|[0]?[0-9][0-9]?)(\s*%)?";
			const string separatorPattern = @"(\s*|\s*,{1}\s*)";

			var regex = new Regex($"^{huePattern}{separatorPattern}{percentagePattern}{separatorPattern}{percentagePattern}$", RegexOptions.Compiled | RegexOptions.Singleline);
			return regex.Match(value?.Trim());
		}

		private static int? HexToInt(string hex)
		{
			if (int.TryParse(hex, NumberStyles.HexNumber, null, out var intValue))
			{
				return intValue;
			}

			return null;
		}

		private static byte? HexToByte(string hex)
		{
			if (byte.TryParse(hex, NumberStyles.HexNumber, null, out var val))
			{
				return val;
			}

			return null;
		}
	}
}