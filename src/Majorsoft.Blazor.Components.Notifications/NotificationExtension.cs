using System;

using Microsoft.Extensions.DependencyInjection;

namespace Majorsoft.Blazor.Components.Notifications
{
	/// <summary>
	/// Extension methods to register required Notification services into IServiceCollection
	/// </summary>
	public static class NotificationExtension
	{
		/// <summary>
		/// Registers required Notification services into IServiceCollection
		/// </summary>
		/// <param name="services">IServiceCollection instance</param>
		/// <returns>IServiceCollection</returns>
		public static IServiceCollection AddNotifications(this IServiceCollection services)
		{
			if (services == null)
			{
				throw new ArgumentNullException(nameof(services));
			}

			services.AddTransient<IHtmlNotificationService, HtmlNotificationService>();

			services.AddSingleton<ToastService>();
			services.AddSingleton<IToastService>(sp => sp.GetRequiredService<ToastService>());
			services.AddSingleton<IToastInternals>(sp => sp.GetRequiredService<ToastService>());

			services.AddScoped<SingletonComponentService<ToastContainer>>();

			return services;
		}
	}

	/// <summary>
	/// Service for checking component was used only once and to be registered as Scoped. 
	/// In WASM it is Singleton, in Server side it will be per 'Session'
	/// </summary>
	/// <typeparam name="T"></typeparam>
	internal class SingletonComponentService<T>
	{
		public bool Initialized { get; set; } = false;
	}
}