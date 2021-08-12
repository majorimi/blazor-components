using System;
using System.Runtime.CompilerServices;

using Majorsoft.Blazor.Components.Common.JsInterop;
using Majorsoft.Blazor.Components.CssEvents;
using Majorsoft.Blazor.Extensions.BrowserStorage;

using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("Majorsoft.Blazor.Components.GdprConsent.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
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
			services.AddJsInteropExtensions();
			services.AddCssEvents();

			services.AddTransient<IGdprConsentService, GdprConsentService>();
			services.AddSingleton<IGdprConsentNotificationService, GdprConsentNotificationService>();

			services.AddScoped<SingletonComponentService<GdprBanner>>();
			services.AddScoped<SingletonComponentService<GdprModal>>();

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