using System;

using Microsoft.Extensions.DependencyInjection;

namespace Majorsoft.Blazor.Extensions.BrowserStorage
{
	/// <summary>
	/// Extension methods to register required JS Interop services into IServiceCollection
	/// </summary>
	public static class StorageExtension
	{
		/// <summary>
		/// Registers required JS Interop services into IServiceCollection
		/// </summary>
		/// <param name="services">IServiceCollection instance</param>
		public static IServiceCollection AddBrowserStorage(this IServiceCollection services)
		{
			if (services == null)
			{
				throw new ArgumentNullException(nameof(services));
			}

			services.AddTransient<ILocalStorageService, LocalStorageService>();
			services.AddTransient<ISessionStorageService, SessionStorageService>();

			return services;
		}
	}
}