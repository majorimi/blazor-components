using Majorsoft.Blazor.Server.Logging.Console;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Majorsoft.Blazor.Components.TestServerApp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureLogging(logger =>
				{
					logger.AddBrowserConsole()
						.SetMinimumLevel(LogLevel.Trace).AddFilter("Microsoft", LogLevel.Information);
				})
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}