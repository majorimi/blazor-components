namespace Blazor.Components.Core.HtmlColors
{
	public record Rgb(byte R, byte G, byte B)
	{
		public string ToHtmlHex()
		{
			return $"#{ToHex()}";
		}

		public string ToHex()
		{
			return $"{R:X2}{G:X2}{B:X2}".ToUpper();
		}

		public override string ToString()
		{
			return $"{R}, {G}, {B}";
		}
	}
}