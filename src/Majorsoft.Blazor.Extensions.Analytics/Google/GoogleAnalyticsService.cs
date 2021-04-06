using System;
using System.Threading.Tasks;

using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Extensions.Analytics.Google
{
	/// <summary>
	/// 
	/// </summary>
	public interface IGoogleAnalyticsService : IAsyncDisposable
	{

	}

	/// <summary>
	/// 
	/// </summary>
	public class GoogleAnalyticsService : IGoogleAnalyticsService
	{
		private readonly Lazy<Task<IJSObjectReference>> moduleTask;

		public GoogleAnalyticsService(IJSRuntime jsRuntime)
		{
			moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
			   "import", "./_content/Majorsoft.Blazor.Analytics/googleAnalytics.js").AsTask());
		}

		public async ValueTask Configure(string trackingId)
		{
			var module = await moduleTask.Value;
			await module.InvokeVoidAsync("configure", trackingId);
		}

		public async ValueTask DisposeAsync()
		{
			if (moduleTask.IsValueCreated)
			{
				var module = await moduleTask.Value;
				await module.DisposeAsync();
			}
		}
	}
}
