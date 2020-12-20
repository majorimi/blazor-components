using System;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Majorsoft.Blazor.WebAssembly.Logging.Console
{
	[ProviderAlias("BrowserConsole")]
	public class BrowserConsoleLoggerProvider : ILoggerProvider
	{
		private static readonly Func<string, LogLevel, bool> TrueFilter = (cat, level) => true;

		private readonly ConcurrentDictionary<string, BrowserConsoleLogger> _loggers = new ConcurrentDictionary<string, BrowserConsoleLogger>();
		private readonly Func<string, LogLevel, bool> _filter;

		public BrowserConsoleLoggerProvider()
			: this(TrueFilter)
		{
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
			return new BrowserConsoleLogger(name, GetFilter());
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