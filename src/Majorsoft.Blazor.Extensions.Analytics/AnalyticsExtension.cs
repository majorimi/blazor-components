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
		/// <returns>IServiceCollection</returns>
		public static IServiceCollection AddGoogleAnalytics(this IServiceCollection services)
		{
			services.AddTransient<IGoogleAnalyticsService, GoogleAnalyticsService>();
			return services;
		}
	}
}