using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Blazor.Components.Core.HtmlColors
{
	public record HtmlColor
	{
		public string OriginalValue { get; init; }

		public Rgb RgbColor { get; init; }
		public string ColorName { get; init; }
		public string HexColor { get; init; }

		public bool IsValid => RgbColor is not null 
			&& (!string.IsNullOrWhiteSpace(ColorName) || !string.IsNullOrWhiteSpace(HexColor));

		public bool IsNamedColor => !string.IsNullOrWhiteSpace(ColorName);

		public HtmlColor(byte r, byte g, byte b)
		{
			RgbColor = new Rgb(r, g, b);
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
				return;
			}

			RgbColor = HexToHtmlRgb(value);
			if(RgbColor is not null) //Hex color
			{
				HexColor = RgbColor.ToHtmlHex();
				ColorName = HtmlColorHelper.NamedHtmlColors
					.SingleOrDefault(x => x.Value == RgbColor.ToHex()).Key;

				return;
			}
			
			if(IsHtmlRgbColor(value))//RGB
			{
				var parts = value.Split(',')
					.Select(s => byte.Parse(s.Trim()))
					.ToArray();

				RgbColor = new Rgb(parts[0], parts[1], parts[2]);
				HexColor = RgbColor.ToHtmlHex();

				ColorName = HtmlColorHelper.NamedHtmlColors
					.FirstOrDefault(x => x.Value == RgbColor.ToHex()).Key;
			}
		}

		private Rgb HexToHtmlRgb(string value)
		{
			if (!IsHtmlHexColor(value))
			{
				return null;
			}

			var chunkSize = 2;
			var hex = value.Trim().TrimStart('#');
			var parts = Enumerable.Range(0, hex.Length / chunkSize)
				.Select(i => HexToByte(hex.Substring(i * chunkSize, chunkSize)))
				.ToArray();

			return new Rgb(parts[0].Value, parts[1].Value, parts[2].Value);
		}

		private bool IsHtmlHexColor(string value)
		{
			var regex = new Regex("^#?[0-9a-fA-F]{6}$", RegexOptions.Compiled | RegexOptions.Singleline);
			return regex.IsMatch(value?.Trim());
		}

		private bool IsHtmlRgbColor(string value)
		{
			const string numberPattern = @"(\d{1,2}|(0|1)\d{2}|2[0-5]{2})";
			const string separatorPattern = @"(\s*,{1}\s*)";
			var regex = new Regex($"^{numberPattern}{separatorPattern}{numberPattern}{separatorPattern}{numberPattern}$", RegexOptions.Compiled | RegexOptions.Singleline);
			return regex.IsMatch(value?.Trim());
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