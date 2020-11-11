using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Blazor.Server.Logging.Console
{
	public interface IBlazorServerConsoleLoggingHub
	{
		string HubUrl { get; }

		Task WriteConsoleLogAsync(string message, LogLevel logLevel);
	}

	public class BlazorServerConsoleLoggingHub : Hub, IBlazorServerConsoleLoggingHub
	{
		public const string HUB_URL = "/logging";

		public string HubUrl => HUB_URL;

		public async Task WriteConsoleLogAsync(string message, LogLevel logLevel)
		{
			await Clients.All.SendAsync("WriteConsoleLogAsync", message, logLevel);
		}

		public override Task OnConnectedAsync()
		{
			//Console.WriteLine($"{Context.ConnectionId} connected");
			return base.OnConnectedAsync();
		}

		public override async Task OnDisconnectedAsync(Exception e)
		{
			//Console.WriteLine($"Disconnected {e?.Message} {Context.ConnectionId}");
			await base.OnDisconnectedAsync(e);
		}
	}
}