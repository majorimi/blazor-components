using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Majorsoft.Blazor.Components.CssEvents;
using Majorsoft.Blazor.Components.Common.JsInterop;
using Majorsoft.Blazor.Server.Logging.Console;
using Majorsoft.Blazor.Components.Maps;
using Majorsoft.Blazor.Extensions.BrowserStorage;
using Majorsoft.Blazor.Extensions.Analytics;
using Majorsoft.Blazor.Components.GdprConsent;
using Majorsoft.Blazor.Components.Notifications;

namespace Majorsoft.Blazor.Components.TestServerApp
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddRazorPages();
			services.AddServerSideBlazor();
			services.AddCssEvents();
			services.AddJsInteropExtensions();
			services.AddMapExtensions();
			services.AddBrowserStorage();
			services.AddGoogleAnalytics();
			services.AddGdprConsent();
			services.AddHtmlNotification();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapBlazorHub();
				endpoints.MapFallbackToPage("/_Host");
				endpoints.MapHub<BlazorServerConsoleLoggingHub>(BlazorServerConsoleLoggingHub.HubUrl);
			});
		}
	}
}
