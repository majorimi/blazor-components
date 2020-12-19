using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

using System.Threading.Tasks;

namespace Majorsoft.Blazor.Server.Logging.Console
{
	internal class BrowserConsoleLoggerService : IBrowserConsoleLoggerService
	{
		private readonly IJSRuntime _jsRuntime;
		private readonly NavigationManager _navigationManager;

		private HubConnection _hubConnection;

		public BrowserConsoleLoggerService(IJSRuntime jsRuntime, NavigationManager navigationManager)
		{
			_jsRuntime = jsRuntime;
			_navigationManager = navigationManager;
		}

		public async Task StartLoggerAsync()
		{
			//setup SignalR
			var _hubUrl = _navigationManager.BaseUri.TrimEnd('/') + BlazorServerConsoleLoggingHub.HubUrl;
			_hubConnection = new HubConnectionBuilder()
				.WithUrl(_hubUrl)
				.Build();

			_hubConnection.On<string, LogLevel>("WriteConsoleLogAsync", WriteBrowserLog);
			await _hubConnection.StartAsync();
		}

		private async Task WriteBrowserLog(string message, LogLevel logLevel)
		{
#if DEBUG
			var jsName = "Majorsoft.Blazor.Server.Logging.Console/blazor.server.logging.console.js";
#else
			var jsName = "Majorsoft.Blazor.Server.Logging.Console/blazor.server.logging.console.min.js";
#endif
			await using (var module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", $"./_content/{jsName}"))
			{
				await ServerConsoleLogging.LogConsole(module, message, logLevel);
			}
		}

		public async ValueTask DisposeAsync()
		{
			if (_hubConnection is not null)
			{
				await _hubConnection.DisposeAsync();
			}
		}
	}
}