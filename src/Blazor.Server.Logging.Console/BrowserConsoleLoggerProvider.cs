using System;
using System.Collections.Concurrent;

using Microsoft.Extensions.Logging;

namespace Blazor.Server.Logging.Console
{
	[ProviderAlias("ServerBrowserConsole")]
	public class BrowserConsoleLoggerProvider : ILoggerProvider
	{
		private static readonly Func<string, LogLevel, bool> TrueFilter = (cat, level) => true;

		private readonly static ConcurrentDictionary<string, BrowserConsoleLogger> _loggers = new ConcurrentDictionary<string, BrowserConsoleLogger>();
		private readonly Func<string, LogLevel, bool> _filter;

		private readonly IServiceProvider _serviceProvider;

		public BrowserConsoleLoggerProvider(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public BrowserConsoleLoggerProvider(Func<string, LogLevel, bool> filter)
		{
			_filter = filter ?? throw new ArgumentNullException(nameof(filter));
		}

		public ILogger CreateLogger(string categoryName)
		{
			if (string.IsNullOrWhiteSpace(categoryName))
			{
				throw new ArgumentNullException(nameof(categoryName));
			}

			return _loggers.GetOrAdd(categoryName, CreateLoggerImplementation);
		}

		private BrowserConsoleLogger CreateLoggerImplementation(string name)
		{
			return new BrowserConsoleLogger(_serviceProvider, name, GetFilter());
		}

		private Func<string, LogLevel, bool> GetFilter()
		{
			if (_filter != null)
			{
				return _filter;
			}

			return TrueFilter;
		}

		public void Dispose()
		{
			_loggers?.Clear();
		}
	}
}