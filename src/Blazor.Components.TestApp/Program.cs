using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Blazor.WebAssembly.Logging.Console;
using Blazor.Components.CssEvents;
using Blazor.Components.Common.JsInterop;
using Blazor.Components.PermaLink;

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
			builder.Services.AddJsInteropExtensions();
			builder.Services.AddPermaLinkWatcher();

			builder.Logging.AddBrowserConsole()
				.SetMinimumLevel(LogLevel.Debug).AddFilter("Microsoft", LogLevel.Warning);

			await builder.Build().RunAsync();
		}
	}
}