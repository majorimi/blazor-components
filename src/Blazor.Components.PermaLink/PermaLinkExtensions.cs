using System;

using Microsoft.Extensions.DependencyInjection;

namespace Blazor.Components.PermaLink
{
	/// <summary>
	/// Extension methods to register required Permalink services into IServiceCollection
	/// </summary>
	public static class PermaLinkExtensions
	{
		/// <summary>
		/// Registers required Permalink services into IServiceCollection
		/// </summary>
		/// <param name="services">IServiceCollection instance</param>
		public static IServiceCollection AddPermaLinkWatcher(this IServiceCollection services)
		{
			if (services == null)
			{
				throw new ArgumentNullException(nameof(services));
			}

			services.AddSingleton<IPermaLinkWatcherService, PermaLinkWatcherService>();

			return services;
		}
	}
}