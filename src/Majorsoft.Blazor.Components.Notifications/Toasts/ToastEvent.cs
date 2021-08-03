using System;

namespace Majorsoft.Blazor.Components.Notifications
{
	/// <summary>
	/// Delegate for Toast items event handlers.
	/// </summary>
	/// <param name="toastId">Toast element Id</param>
	public delegate void ToastEvent(Guid toastId);
}