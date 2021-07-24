using System;

using Microsoft.Extensions.DependencyInjection;

namespace Majorsoft.Blazor.Components.Notifications
{
	/// <summary>
	/// Extension methods to register required JS Interop services into IServiceCollection
	/// </summary>
	public static class NotificationExtension
	{
		/// <summary>
		/// Registers required JS Interop services into IServiceCollection
		/// </summary>
		/// <param name="services">IServiceCollection instance</param>
		/// <returns>IServiceCollection</returns>
		public static IServiceCollection AddHtmlNotification(this IServiceCollection services)
		{
			if (services == null)
			{
				throw new ArgumentNullException(nameof(services));
			}

			services.AddTransient<IHtmlNotificationService, HtmlNotificationService>();

			return services;
		}
	}
}