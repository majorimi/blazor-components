using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Majorsoft.Blazor.Components.Tabs.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace Majorsoft.Blazor.Components.Tabs
{
	/// <summary>
	/// Determines the positions of tabs.
	/// </summary>
	public enum TabPositons
	{
		/// <summary>
		/// Left side
		/// </summary>
		Left = 0,
		/// <summary>
		/// Centered
		/// </summary>
		Center = 1,
		/// <summary>
		/// Right side
		/// </summary>
		Right = 2,
	}
}