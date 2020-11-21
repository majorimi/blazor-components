using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;

namespace Blazor.Components.PermaLink
{
	/// <summary>
	/// Event handler to fix blinking icon
	/// https://stackoverflow.com/questions/63265941/blazor-3-1-nested-onmouseover-events
	/// </summary>
	[EventHandler("onmouseleave", typeof(MouseEventArgs), true, true)]
	[EventHandler("onmouseenter", typeof(MouseEventArgs), true, true)]
	public static class EventHandlers
	{
	}
}