using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

using System.Threading.Tasks;

namespace Majorsoft.Blazor.Server.Logging.Console
{
	internal class ServerConsoleLogging
	{
		public static ValueTask LogConsole(IJSObjectReference jSObjectReference, string message, LogLevel logLevel)
		{
			if (jSObjectReference != null)
			{
				return jSObjectReference.InvokeVoidAsync("consoleLogger", message, logLevel);
			}

			return ValueTask.CompletedTask;
		}
	}
}