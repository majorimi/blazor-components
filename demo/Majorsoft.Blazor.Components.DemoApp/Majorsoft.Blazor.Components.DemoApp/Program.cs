using Majorsoft.Blazor.Components.Common.JsInterop;
using Majorsoft.Blazor.Components.CssEvents;
using Majorsoft.Blazor.Components.DemoApp.Components;
using Majorsoft.Blazor.Components.GdprConsent;
using Majorsoft.Blazor.Components.Maps;
using Majorsoft.Blazor.Components.Notifications;
using Majorsoft.Blazor.Components.PermaLink;
using Majorsoft.Blazor.Extensions.Analytics;
using Majorsoft.Blazor.Extensions.BrowserStorage;

using Microsoft.AspNetCore.Components;

namespace Majorsoft.Blazor.Components.DemoApp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddRazorComponents()
				.AddInteractiveWebAssemblyComponents();

			//builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
			builder.Services.AddTransient(sp =>
			{
				NavigationManager navigation = sp.GetRequiredService<NavigationManager>();
				return new HttpClient { BaseAddress = new Uri(navigation.BaseUri) };
			});
			builder.Services.AddCssEvents();
			builder.Services.AddJsInteropExtensions();
			builder.Services.AddPermaLinkWatcher();
			builder.Services.AddMapExtensions();
			builder.Services.AddBrowserStorage();
			builder.Services.AddGoogleAnalytics();
			builder.Services.AddGdprConsent();
			builder.Services.AddNotifications();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseWebAssemblyDebugging();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
			app.UseHttpsRedirection();

			app.UseAntiforgery();

			app.MapStaticAssets();
			app.MapRazorComponents<App>()
				.AddInteractiveWebAssemblyRenderMode()
				.AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

			app.Run();
		}
	}
}
