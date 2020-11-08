using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Blazor.Components.Core.HtmlColors
{
	public record HtmlColor
	{
		public string OriginalValue { get; init; }

		public Color RgbColor { get; init; }
		public HslColor HslColor { get; init; }
		public string ColorName { get; init; }
		public string HexColor { get; init; }

		public bool IsValid => RgbColor != default 
			&& (!string.IsNullOrWhiteSpace(ColorName) || !string.IsNullOrWhiteSpace(HexColor));

		public bool IsNamedColor => !string.IsNullOrWhiteSpace(ColorName);

		public HtmlColor(byte r, byte g, byte b)
		{
			RgbColor = Color.FromArgb(r, g, b);
			HslColor = HslColor.FromRgb(RgbColor);
			HexColor = RgbColor.ToHtmlHex();

			ColorName = HtmlColorHelper.NamedHtmlColors
				.SingleOrDefault(x => x.Value == RgbColor.ToHex()).Key;
		}

		public HtmlColor(int h, int s, int l)
		{
			HslColor = new HslColor(h, s, l);
			RgbColor = (Color)HslColor;
			HexColor = RgbColor.ToHtmlHex();

			ColorName = HtmlColorHelper.NamedHtmlColors
				.SingleOrDefault(x => x.Value == RgbColor.ToHex()).Key;
		}

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

			RgbColor = HexToHtmlRgb(value);
			if(RgbColor != default) //Hex color
			{
				HexColor = RgbColor.ToHtmlHex();
				HslColor = HslColor.FromRgb(RgbColor);
				ColorName = HtmlColorHelper.NamedHtmlColors
					.SingleOrDefault(x => x.Value == RgbColor.ToHex()).Key;

				return;
			}
			
			if(IsRgbColor(value)) //RGB
			{
				value = value.Replace(' ', ',');
				var parts = value.Split(',').Where(s => !string.IsNullOrEmpty(s) && s != ",")
					.Select(s => byte.Parse(s.Trim()))
					.ToArray();

				RgbColor = Color.FromArgb(parts[0], parts[1], parts[2]);
				HexColor = RgbColor.ToHtmlHex();
				HslColor = HslColor.FromRgb(RgbColor);

				ColorName = HtmlColorHelper.NamedHtmlColors
					.FirstOrDefault(x => x.Value == RgbColor.ToHex()).Key;

				return;
			}

			var match = IsHslColor(value);
			if (match.Success) //HSL
			{
				var parts = match.Groups.Values.Skip(1)
					.Select(s => s.Value?.Trim())
					.Where(x => !string.IsNullOrWhiteSpace(x) && x != "," && x != "%" && x != "°")
					.Select(s => int.Parse(s.Trim()))
					.ToArray();

				HslColor = new HslColor(parts[0], parts[1], parts[2]);
				RgbColor = (Color)HslColor;
				HexColor = RgbColor.ToHtmlHex();

				ColorName = HtmlColorHelper.NamedHtmlColors
					.SingleOrDefault(x => x.Value == RgbColor.ToHex()).Key;
			}
		}

		private Color HexToHtmlRgb(string value)
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

		private bool IsHtmlHexColor(string value)
		{
			var regex = new Regex("^#?[0-9a-fA-F]{6}$", RegexOptions.Compiled | RegexOptions.Singleline);
			return regex.IsMatch(value?.Trim());
		}

		private bool IsRgbColor(string value)
		{
			const string numberPattern = @"(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)";
			const string separatorPattern = @"(\s*|\s*,{1}\s*)";
			
			var regex = new Regex($"^{numberPattern}{separatorPattern}{numberPattern}{separatorPattern}{numberPattern}$", RegexOptions.Compiled | RegexOptions.Singleline);
			return regex.IsMatch(value?.Trim());
		}


		private Match IsHslColor(string value)
		{
			const string huePattern = @"(360|3[0-5][0-9]|[012]?[0-9][0-9]?)(\s*°)?";
			const string percentagePattern = @"(100|[0]?[0-9][0-9]?)(\s*%)?";
			const string separatorPattern = @"(\s*|\s*,{1}\s*)";

			var regex = new Regex($"^{huePattern}{separatorPattern}{percentagePattern}{separatorPattern}{percentagePattern}$", RegexOptions.Compiled | RegexOptions.Singleline);
			return regex.Match(value?.Trim());
		}

		private int? HexToInt(string hex)
		{
			if (int.TryParse(hex, NumberStyles.HexNumber, null, out var intValue))
			{
				return intValue;
			}

			return null;
		}

		private byte? HexToByte(string hex)
		{
			if (byte.TryParse(hex, NumberStyles.HexNumber, null, out var val))
			{
				return val;
			}

			return null;
		}
	}
}