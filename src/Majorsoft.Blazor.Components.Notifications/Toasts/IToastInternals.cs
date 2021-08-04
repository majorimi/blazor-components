using System;
using System.Collections.Generic;

namespace Majorsoft.Blazor.Components.Notifications
{
	/// <summary>
	/// Internal interface to trigger individual Toast elements events.
	/// </summary>
	internal interface IToastInternals
	{
		IEnumerable<ToastSettings> AllToasts { get; }

		void TriggerToastOpen(Guid id);
		void TriggerToastClosed(Guid id);
		void TriggerToastCloseButtonClicked(Guid id);
	}
}