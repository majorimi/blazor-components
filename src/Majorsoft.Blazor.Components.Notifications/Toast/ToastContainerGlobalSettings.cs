namespace Majorsoft.Blazor.Components.Notifications
{
	/// <summary>
	/// Global Toast Container and common Toasts settings with default values.
	/// </summary>
	public class ToastContainerGlobalSettings
	{
		/// <summary>
		/// Determines if Toast notifications should be removed when user navigates to other page.
		/// Note: in order make it work <see cref="ToastContainer"/> component must be applied only once per application in a common place e.g.: Layour.razor, etc.
		/// </summary>
		public bool RemoveToastsOnNavigation { get; set; } = true;

		/// <summary>
		/// Toast Container position on screen <see cref="ToastPositions"/>.
		/// </summary>
		public ToastPositions Position { get; set; } = ToastPositions.TopRight;

		/// <summary>
		/// Toast Container width in `px` it will determine the shown Toast widht as well.
		/// </summary>
		public int Width { get; set; } = 350;

		/// <summary>
		/// Global config applied to all Toasts if not set otherwise.
		/// When true Toast will show an icon corresponding to the <see cref="NotificationTypes"/>.
		/// </summary>
		public static bool DefaultToastsShowIcon { get; set; }

		/// <summary>
		/// Global config applied to all Toasts if not set otherwise.
		/// When true Toast will show close "x" button.
		/// </summary>
		public static bool DefaultToastsShowCloseButton { get; set; } = true;

		/// <summary>
		/// Global config applied to all Toasts if not set otherwise.
		/// Toast will close after set time elapsed in Sec.
		/// </summary>
		public static uint DefaultToastsAutoCloseInSec { get; set; } = 10;

		/// <summary>
		/// Global config applied to all Toasts if not set otherwise.
		/// When it's true a progress bar will show the remaining time until Alert closes.
		/// </summary>
		public static bool DefaultToastsShowCloseCountdownProgress { get; set; } = true;

		private static uint _shadowEffect = 5;
		/// <summary>
		/// Global config applied to all Toasts if not set otherwise.
		/// Determines the shadow effect strongness which makes Alert elevated. Value should be between 0 and 20.
		/// </summary>
		public static uint DefaultToastsShadowEffect
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