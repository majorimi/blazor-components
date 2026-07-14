using System;
using System.Runtime.CompilerServices;

using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("Majorsoft.Blazor.Components.Media.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Majorsoft.Blazor.Components.Media
{
	/// <summary>
	/// Extension methods to register required Media services into IServiceCollection
	/// </summary>
	public static class MediaExtension
	{
		/// <summary>
		/// Registers required Media services into IServiceCollection
		/// </summary>
		/// <param name="services">IServiceCollection instance</param>
		/// <returns>IServiceCollection</returns>
		public static IServiceCollection AddMediaComponents(this IServiceCollection services)
		{
			if (services == null)
			{
				throw new ArgumentNullException(nameof(services));
			}

			services.AddTransient<IMediaDeviceService, MediaDeviceService>();

			return services;
		}
	}
}
