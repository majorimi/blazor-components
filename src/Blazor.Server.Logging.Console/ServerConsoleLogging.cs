using Microsoft.JSInterop;

using System.Threading.Tasks;

namespace Blazor.Server.Logging.Console
{
	internal class ServerConsoleLogging
	{
		public static ValueTask<string> LogConsole(JSObjectReference jSObjectReference, string message)
		{
			if (jSObjectReference != null)
			{
				return jSObjectReference.InvokeAsync<string>("consoleLogger", message);
			}

			return ValueTask.FromResult<string>(null);
		}
	}
}