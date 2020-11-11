using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Blazor.Server.Logging.Console
{
	public class BlazorServerConsoleLoggingHub : Hub
	{
		public const string HubUrl = "/logging";

		public async Task WriteConsoleLogAsync(string message, LogLevel logLevel)
		{
			await Clients.All.SendAsync("WriteConsoleLogAsync", message, logLevel);
		}
	}
}