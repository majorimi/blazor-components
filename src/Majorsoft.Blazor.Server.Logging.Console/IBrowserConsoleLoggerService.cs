using System;
using System.Threading.Tasks;

namespace Majorsoft.Blazor.Server.Logging.Console
{
	public interface IBrowserConsoleLoggerService : IAsyncDisposable
	{
		Task StartLoggerAsync();
	}
}