using System;
using Microsoft.Extensions.Logging;

namespace Blazor.WebAssembly.Logging.Console
{
	internal class BrowserConsoleLogger<T> : BrowserConsoleLogger, ILogger<T>
	{
		public BrowserConsoleLogger(string name, Func<string, LogLevel, bool> filter, IExternalScopeProvider scopeProvider = null)
			: base(name, filter, scopeProvider)
		{
		}
	}

	internal class BrowserConsoleLogger : ILogger
	{
		private Func<string, LogLevel, bool> _filter;
		private IExternalScopeProvider ScopeProvider { get; set; }

		public string Name { get; }
		public Func<string, LogLevel, bool> Filter
		{
			get { return _filter; }
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value));
				}

				_filter = value;
			}
		}

		public BrowserConsoleLogger(string name, Func<string, LogLevel, bool> filter, IExternalScopeProvider scopeProvider = null)
		{
			if (name == null)
			{
				throw new ArgumentNullException(nameof(name));
			}

			Name = name;
			Filter = filter ?? ((category, logLevel) => true);
			ScopeProvider = scopeProvider;
		}

		public bool IsEnabled(LogLevel logLevel)
		{
			if (logLevel == LogLevel.None)
			{
				return false;
			}

			return Filter(Name, logLevel);
		}

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			if (!IsEnabled(logLevel))
			{
				return;
			}

			if (formatter == null)
			{
				throw new ArgumentNullException(nameof(formatter));
			}

			var message = formatter(state, exception);

			if (!string.IsNullOrEmpty(message) || exception != null)
			{
				System.Console.WriteLine(message); //Works only in WebAssembly mode...
				System.Diagnostics.Debug.WriteLine(message);
			}
		}

		public IDisposable BeginScope<TState>(TState state) => ScopeProvider?.Push(state) ?? NullScope.Instance;
	}
}