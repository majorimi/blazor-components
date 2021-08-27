using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Majorsoft.Blazor.Components.Toggle.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace Majorsoft.Blazor.Components.Toggle
{
	/// <summary>
	/// Toggle switch handle style 
	/// </summary>
	public enum ToggleSwitchStyle
	{
		/// <summary>
		/// Ellipse handle style
		/// </summary>
		Ellipse = 0,
		/// <summary>
		/// Circle handle style
		/// </summary>
		Circle = 1,
		/// <summary>
		/// Square handle style
		/// </summary>
		Square = 2,
	}
}