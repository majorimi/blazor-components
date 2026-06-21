using Majorsoft.Blazor.Components.Common.JsInterop;
using Majorsoft.Blazor.Components.CssEvents;
using Majorsoft.Blazor.Components.GdprConsent;
using Majorsoft.Blazor.Components.Maps;
using Majorsoft.Blazor.Components.Notifications;
using Majorsoft.Blazor.Components.PermaLink;
using Majorsoft.Blazor.Extensions.Analytics;
using Majorsoft.Blazor.Extensions.BrowserStorage;
using Majorsoft.Blazor.WebAssembly.Logging.Console;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Majorsoft.Blazor.Components.DemoApp
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);

			builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

			builder.Services.AddCssEvents();
			builder.Services.AddJsInteropExtensions();
			builder.Services.AddPermaLinkWatcher();
			builder.Services.AddMapExtensions();
			builder.Services.AddBrowserStorage();

			builder.Services.AddGoogleAnalytics();

			builder.Services.AddGdprConsent();
			builder.Services.AddNotifications();

			builder.Logging.AddBrowserConsole()
				.SetMinimumLevel(LogLevel.Debug).AddFilter("Microsoft", LogLevel.Information);

			builder.RootComponents.Add<App>("#app");
			builder.RootComponents.Add<HeadOutlet>("head::after");

			await builder.Build().RunAsync();
		}
	}
}
