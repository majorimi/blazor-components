using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

using System.Threading.Tasks;

namespace Blazor.Server.Logging.Console
{
	/*internal*/ public class ServerConsoleLogging
	{
		public static ValueTask<string> LogConsole(IJSObjectReference jSObjectReference, string message, LogLevel logLevel)
		{
			if (jSObjectReference != null)
			{
				return jSObjectReference.InvokeAsync<string>("consoleLogger", message, logLevel);
			}

			return ValueTask.FromResult<string>(null);
		}
	}
}