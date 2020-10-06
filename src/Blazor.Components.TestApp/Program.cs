using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Blazor.WebAssembly.Logging.Console;
using Blazor.Components.CssEvents;

namespace Blazor.Components.TestApp
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("app");

			builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
			builder.Services.AddCssEvents();

			builder.Logging.AddBrowserConsole()
				.SetMinimumLevel(LogLevel.Debug);

			await builder.Build().RunAsync();
		}
	}
}