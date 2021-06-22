using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Majorsoft.Blazor.WebAssembly.Logging.Console;
using Majorsoft.Blazor.Components.CssEvents;
using Majorsoft.Blazor.Components.Common.JsInterop;
using Majorsoft.Blazor.Components.PermaLink;
using Majorsoft.Blazor.Components.Maps;
using Majorsoft.Blazor.Extensions.BrowserStorage;
using Majorsoft.Blazor.Extensions.Analytics;

namespace Majorsoft.Blazor.Components.TestApp
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
			builder.Services.AddMapExtensions();
			builder.Services.AddBrowserStorage();

			builder.Services.AddGoogleAnalytics();

			builder.Logging.AddBrowserConsole()
				.SetMinimumLevel(LogLevel.Debug).AddFilter("Microsoft", LogLevel.Information);

			await builder.Build().RunAsync();
		}
	}
}