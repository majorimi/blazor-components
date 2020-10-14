using System;

using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Blazor.Server.Logging.Console
{
	internal class BrowserConsoleLogger<T> : BrowserConsoleLogger, ILogger<T>
	{
		public BrowserConsoleLogger(IJSRuntime runtime, string name, Func<string, LogLevel, bool> filter, IExternalScopeProvider scopeProvider = null)
			: base(runtime, name, filter, scopeProvider)
		{
		}
	}

	internal class BrowserConsoleLogger : ILogger
	{
		private IJSRuntime _jsRuntime;
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

		public BrowserConsoleLogger(IJSRuntime runtime, string name, Func<string, LogLevel, bool> filter, IExternalScopeProvider scopeProvider = null)
		{
			if (name == null)
			{
				throw new ArgumentNullException(nameof(name));
			}

			_jsRuntime = runtime;
			Name = name;
			Filter = filter ?? ((category, logLevel) => true);
			ScopeProvider = scopeProvider;
		}

		public void SetIJSRuntime(IJSRuntime runtime) //HACK: This is a hack IJSRuntime should be injected but cannot be done...
		{
			_jsRuntime = runtime;
		}

		public bool IsEnabled(LogLevel logLevel)
		{
			if (logLevel == LogLevel.None)
			{
				return false;
			}

			return Filter(Name, logLevel);
		}

		public async void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
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
				try
				{
					if (_jsRuntime is { })
					{

#if DEBUG
						var jsName = "Majorsoft.Blazor.Server.Logging.Console/blazor.server.logging.console.js";
#else
						var jsName = "Majorsoft.Blazor.Server.Logging.Console/blazor.server.logging.console.min.js";
#endif
						await using (var module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import",
							$"./_content/{jsName}"))
						{
							await ServerConsoleLogging.LogConsole(module, message);
						}
					}
				}
				catch (InvalidOperationException e)
				{
					//hack
				}
			}
		}

		public IDisposable BeginScope<TState>(TState state) => ScopeProvider?.Push(state) ?? NullScope.Instance;
	}
}