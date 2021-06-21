using Majorsoft.Blazor.Extensions.Analytics.Google;

using Microsoft.Extensions.DependencyInjection;

namespace Majorsoft.Blazor.Extensions.Analytics
{
	/// <summary>
	/// Extension methods to register required Analytic services into IServiceCollection
	/// </summary>
	public static class AnalyticsExtension
	{
		/// <summary>
		/// Registers required Google Analytic services into IServiceCollection
		/// DO NOT CALL with <see cref="trackingId"/> value in case of Blazor Server!
		/// </summary>
		/// <param name="services">IServiceCollection instance</param>
		/// <param name="trackingId">Google Tracking Id when provided <see cref="IGoogleAnalyticsService.InitializeAsync(string)"/> will be called. DO NOT CALL with value in case of Blazor Server!</param>
		/// <returns>IServiceCollection</returns>
		public static IServiceCollection AddGoogleAnalytics(this IServiceCollection services, string trackingId = "")
		{
			services.AddTransient<IGoogleAnalyticsService, GoogleAnalyticsService>();
			if (!string.IsNullOrWhiteSpace(trackingId))
			{
				services.BuildServiceProvider().GetRequiredService<IGoogleAnalyticsService>()?.InitializeAsync(trackingId);
			}

			return services;
		}

		/// <summary>
		/// Registers required Google Analytic services into IServiceCollection for Blazor Server
		/// </summary>
		/// <param name="services">IServiceCollection instance</param>
		/// <returns>IServiceCollection</returns>
		public static IServiceCollection AddGoogleAnalyticsForBlazorServer(this IServiceCollection services)
		{
			return services.AddGoogleAnalytics();
		}
	}
}