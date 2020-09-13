using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Blazor.Server.Logging.Console;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Blazor.Components.TestServerApp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.ConfigureServices(config =>
					{
						//config.AddSingleton<Microsoft.JSInterop.JSRuntime>();
					});

					webBuilder.ConfigureLogging(logger =>
					{
						logger.SetMinimumLevel(LogLevel.Debug);
						logger.AddBrowserConsole();
					});
					webBuilder.UseStartup<Startup>();
				});
	}
}
