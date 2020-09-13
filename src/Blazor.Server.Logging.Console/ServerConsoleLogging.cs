using Microsoft.JSInterop;

using System.Threading.Tasks;

namespace Blazor.Server.Logging.Console
{
	internal class ServerConsoleLogging
	{
		public static ValueTask<string> LogConsole(IJSRuntime jsRuntime, string message)
		{
			if (jsRuntime != null)
			{
				return jsRuntime.InvokeAsync<string>("serverConsoleLogging.log", message);
			}

			return ValueTask.FromResult<string>(null);
		}
	}
}