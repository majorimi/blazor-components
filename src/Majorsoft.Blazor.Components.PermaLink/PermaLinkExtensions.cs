using System;
using System.Runtime.CompilerServices;

using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("Majorsoft.Blazor.Components.PermaLink.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Majorsoft.Blazor.Components.PermaLink
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
			services.AddScoped<SingletonComponentService<PermaLinkBlazorServerInitializer>>();
			services.AddScoped<SingletonComponentService<PermalinkBlazorWasmInitializer>>();

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