using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Blazor.WebAssembly.Logging.Console;
using Blazor.Components.CssEvents;
using Blazor.Components.Common.JsInterop;
using Blazor.Components.PermaLink;

namespace Blazor.Components.DemoApp
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("#app");

			builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

			builder.Services.AddCssEvents();
			builder.Services.AddJsInteropExtensions();
			builder.Services.AddPermaLinkWatcher();

			builder.Logging.AddBrowserConsole()
				.SetMinimumLevel(LogLevel.Debug);

			await builder.Build().RunAsync();
		}
	}
}
