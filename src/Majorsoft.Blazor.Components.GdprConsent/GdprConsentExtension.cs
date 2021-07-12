using System;

using Majorsoft.Blazor.Extensions.BrowserStorage;

using Microsoft.Extensions.DependencyInjection;

namespace Majorsoft.Blazor.Components.GdprConsent
{
	/// <summary>
	/// Extension methods to register required JS Interop services into IServiceCollection
	/// </summary>
	public static class GdprConsentExtension
	{
		/// <summary>
		/// Registers required JS Interop services into IServiceCollection
		/// </summary>
		/// <param name="services">IServiceCollection instance</param>
		public static IServiceCollection AddGdprConsent(this IServiceCollection services)
		{
			if (services == null)
			{
				throw new ArgumentNullException(nameof(services));
			}

			services.AddBrowserStorage();
			services.AddTransient<IGdprConsentService, GdprConsentService>();
			services.AddSingleton<IGdprConsentNotificationService, GdprConsentNotificationService>();

			return services;
		}
	}
}