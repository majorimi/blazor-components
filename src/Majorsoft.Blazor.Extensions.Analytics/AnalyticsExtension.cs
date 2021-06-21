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
		/// </summary>
		/// <param name="services">IServiceCollection instance</param>
		/// <param name="trackingId">Google Tracking Id when provided <see cref="IGoogleAnalyticsService.Initialize(string)"/> will be called. DO NOT CALL with value in case of Blazor Server!</param>
		/// <returns>IServiceCollection</returns>
		public static IServiceCollection AddGoogleAnalytics(this IServiceCollection services, string trackingId = "")
		{
			services.AddTransient<IGoogleAnalyticsService, GoogleAnalyticsService>();
			if (!string.IsNullOrWhiteSpace(trackingId))
			{
				services.BuildServiceProvider().GetRequiredService<IGoogleAnalyticsService>()?.Initialize(trackingId);
			}

			return services;
		}
	}
}