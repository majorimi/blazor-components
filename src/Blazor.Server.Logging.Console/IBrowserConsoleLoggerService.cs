using System;
using System.Threading.Tasks;

namespace Blazor.Server.Logging.Console
{
	public interface IBrowserConsoleLoggerService : IAsyncDisposable
	{
		Task StartLoggerAsync();
	}
}