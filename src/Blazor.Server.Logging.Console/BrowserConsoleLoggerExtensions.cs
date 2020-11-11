using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Blazor.Server.Logging.Console
{
	public static class BrowserConsoleLoggerExtensions
	{

		/// <summary>
		/// Adds a <see cref="IBrowserConsoleLoggerService"/> as injectable service.
		/// </summary>
		/// <param name="builder">The <see cref="IServiceCollection"/> to use.</param>
		public static IServiceCollection AddBrowserConsoleLoggerService(this IServiceCollection services)
		{
			services.AddTransient<IBrowserConsoleLoggerService, BrowserConsoleLoggerService>();
			return services;
		}

		/// <summary>
		/// Adds a console logger named 'Console' to the factory.
		/// </summary>
		/// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
		public static ILoggingBuilder AddBrowserConsole(this ILoggingBuilder builder)
		{
			builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, BrowserConsoleLoggerProvider>());
			
			builder.Services.AddBrowserConsoleLoggerService();
			return builder;
		}

		/// <summary>
		/// Adds a console logger that is enabled for <see cref="LogLevel"/>.Information or higher.
		/// </summary>
		/// <param name="factory">The <see cref="ILoggerFactory"/> to use.</param>
		public static ILoggerFactory AddBrowserConsole(this ILoggerFactory factory)
		{
			return factory.AddBrowserConsole(LogLevel.Information);
		}

		/// <summary>
		/// Adds a browser console logger that is enabled for <see cref="LogLevel"/>s of minLevel or higher.
		/// </summary>
		/// <param name="factory">The <see cref="ILoggerFactory"/> to use.</param>
		/// <param name="minLevel">The minimum <see cref="LogLevel"/> to be logged</param>
		public static ILoggerFactory AddBrowserConsole(this ILoggerFactory factory, LogLevel minLevel)
		{
			factory.AddBrowserConsole((category, logLevel) => logLevel >= minLevel);
			return factory;
		}

		/// <summary>
		/// Adds a console logger that is enabled as defined by the filter function.
		/// </summary>
		/// <param name="factory">The <see cref="ILoggerFactory"/> to use.</param>
		/// <param name="filter">The category filter to apply to logs.</param>
		/// in the output.</param>
		public static ILoggerFactory AddBrowserConsole(this ILoggerFactory factory, Func<string, LogLevel, bool> filter)
		{
			factory.AddProvider(new BrowserConsoleLoggerProvider(filter));
			return factory;
		}
	}
}