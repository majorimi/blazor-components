using Majorsoft.Blazor.Components.Common.JsInterop;
using Majorsoft.Blazor.Components.CssEvents;
using Majorsoft.Blazor.Components.DragAndDrop;
using Majorsoft.Blazor.Components.GdprConsent;
using Majorsoft.Blazor.Components.Maps;
using Majorsoft.Blazor.Components.Notifications;
using Majorsoft.Blazor.Components.PermaLink;
using Majorsoft.Blazor.Components.TestServerApp.Components;
using Majorsoft.Blazor.Extensions.Analytics;
using Majorsoft.Blazor.Extensions.BrowserStorage;
using Majorsoft.Blazor.Server.Logging.Console;

using Microsoft.AspNetCore.Components;

namespace Majorsoft.Blazor.Components.TestServerApp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Logging.AddBrowserConsole()
					.SetMinimumLevel(LogLevel.Trace).AddFilter("Microsoft", LogLevel.Information);

			// Add services to the container.
			builder.Services.AddRazorComponents()
				.AddInteractiveServerComponents();

			builder.Services.AddTransient(sp =>
			{
				NavigationManager navigation = sp.GetRequiredService<NavigationManager>();
				return new HttpClient { BaseAddress = new Uri(navigation.BaseUri) };
			});
			// Add Majorsoft services to the container.
			builder.Services.AddRazorPages();
			builder.Services.AddServerSideBlazor();
			builder.Services.AddCssEvents();
			builder.Services.AddJsInteropExtensions();
			builder.Services.AddPermaLinkWatcher();
			builder.Services.AddMapExtensions();
			builder.Services.AddBrowserStorage();
			builder.Services.AddGoogleAnalytics();
			builder.Services.AddGdprConsent();
			builder.Services.AddNotifications();
			builder.Services.AddDragAndDrop();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
			app.UseHttpsRedirection();

			app.UseAntiforgery();

			//Add Blazor and Server Console Logging Hub
			app.UseRouting();
			app.MapBlazorHub("/App/_blazor").WithOrder(-1);
			app.MapHub<BlazorServerConsoleLoggingHub>(BlazorServerConsoleLoggingHub.HubUrl);

			app.MapStaticAssets();
			app.MapRazorComponents<App>()
				.AddInteractiveServerRenderMode();

			app.Run();
		}
	}
}
