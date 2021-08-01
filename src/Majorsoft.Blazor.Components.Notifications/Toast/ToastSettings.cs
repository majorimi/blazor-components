using System;

using Microsoft.AspNetCore.Components;

namespace Majorsoft.Blazor.Components.Notifications
{
	/// <summary>
	/// Properties for <see cref="Toast"/> Notification.
	/// </summary>
	public class ToastSettings
	{
		/// <summary>
		/// Internal Toast Id
		/// </summary>
		internal Guid Id { get; set; }
		/// <summary>
		/// Internal Toast created time
		/// </summary>
		internal DateTime NotificationTime { get; set; }


		/// <summary>
		/// HTML Content of the collapse panel.
		/// </summary>
		public MarkupString Content { get; set; }

		/// <summary>
		/// Notification type or severity level.
		/// </summary>
		public NotificationTypes Type { get; set; }

		/// <summary>
		/// Notification style to show different variant of the same <see cref="Type"/> Toast.
		/// </summary>
		public NotificationStyles NotificationStyle { get; set; }

		/// <summary>
		/// When true Toast will show an icon corresponding to the <see cref="NotificationTypes"/>.
		/// </summary>
		public bool ShowIcon { get; set; } = ToastContainerGlobalSettings.DefaultToastsShowIcon;
		/// <summary>
		/// Icon customization it accepts an SVG `Path` value to override the default icon.
		/// </summary>
		public string CustomIconSvgPath { get; set; } = "";
		/// <summary>
		/// When true Toast will show close "x" button.
		/// </summary>
		public bool ShowCloseButton { get; set; } = true;

		/// <summary>
		/// Toast will close after set time elapsed in Sec.
		/// </summary>
		public uint AutoCloseInSec { get; set; } = ToastContainerGlobalSettings.DefaultToastsAutoCloseInSec;

		/// <summary>
		/// When it's true a progress bar will show the remaining time until Alert closes.
		/// </summary>
		public bool ShowCloseCountdownProgress { get; set; } = ToastContainerGlobalSettings.DefaultToastsShowCloseCountdownProgress;

		private uint _shadowEffect = ToastContainerGlobalSettings.DefaultToastsShadowEffect;
		/// <summary>
		/// Determines the shadow effect strongness which makes Alert elevated. Value should be between 0 and 20.
		/// </summary>
		public uint ShadowEffect
		{
			get => _shadowEffect;
			set
			{
				if (value > 20)
				{
					_shadowEffect = 20;
				}
				else
				{
					_shadowEffect = value;
				}
			}
		}
	}
}