namespace Blazor.Components.Core.HtmlColors
{
	public record Rgb(byte R, byte G, byte B)
	{
		public override string ToString()
		{
			return $"{R}, {G}, {B}";
		}
	}
}