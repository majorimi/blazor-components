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
		ValueTask Initialize(string trackingId);
	}

	/// <summary>
	/// 
	/// </summary>
	public class GoogleAnalyticsService : IGoogleAnalyticsService
	{
		private readonly Lazy<Task<IJSObjectReference>> moduleTask;

		public GoogleAnalyticsService(IJSRuntime jsRuntime)
		{
			string js = "";
#if DEBUG
			js = "./_content/Majorsoft.Blazor.Extensions.Analytics/googleAnalytics.js";
#else
			js = "./_content/Majorsoft.Blazor.Extensions.Analytics/googleAnalytics.min.js";
#endif

			moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>("import", js).AsTask());
		}

		public async ValueTask Configure(string trackingId)
		{
			var module = await moduleTask.Value;
			await module.InvokeVoidAsync("configure", trackingId);
		}

		public async ValueTask Initialize(string trackingId)
		{
			var module = await moduleTask.Value;
			await module.InvokeVoidAsync("init", trackingId);
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
