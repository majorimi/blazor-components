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
			HexColor = $"#{r:X2}{g:X2}{b:X2}".ToUpper();

			ColorName = HtmlColorHelper.NamedHtmlColors
				.SingleOrDefault(x => x.Value == HexColor.Trim('#')).Key;
		}

		public HtmlColor(string value)
		{
			OriginalValue = value;

			if(string.IsNullOrWhiteSpace(value))
			{
				return;
			}

			var key = value?.ToUpper();
			if (HtmlColorHelper.NamedHtmlColors.ContainsKey(key)) //Named color
			{
				ColorName = key;
				HexColor = $"#{HtmlColorHelper.NamedHtmlColors[key]}";
				RgbColor = HexToHtmlRgb(HexColor);
				return;
			}

			RgbColor = HexToHtmlRgb(value);
			if(RgbColor is not null) //Hex color
			{
				HexColor = $"#{value.Trim().TrimStart('#').ToUpper()}";
				return;
			}

			//RGB
		}

		private Rgb HexToHtmlRgb(string value)
		{
			if (!IsHtmlHex(value))
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

		private bool IsHtmlHex(string value)
		{
			var regex = new Regex("^#?[0-9a-fA-F]{6}$", RegexOptions.Compiled | RegexOptions.Singleline);
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