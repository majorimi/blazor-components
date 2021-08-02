using System;

namespace Majorsoft.Blazor.Components.Notifications
{
	/// <summary>
	/// Internal interface to trigger individual Toast elements events.
	/// </summary>
	internal interface IToastEvents
	{
		void TriggerToastOpen(Guid id);
		void TriggerToastClosed(Guid id);
		void TriggerToastCloseButtonClicked(Guid id);
	}
}