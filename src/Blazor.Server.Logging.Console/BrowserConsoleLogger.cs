using System;

using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace Majorsoft.Blazor.Server.Logging.Console
{
	internal class BrowserConsoleLogger<T> : BrowserConsoleLogger, ILogger<T>
	{
		public BrowserConsoleLogger(IServiceProvider serviceProvider, string name, Func<string, LogLevel, bool> filter, IExternalScopeProvider scopeProvider = null)
			: base(serviceProvider, name, filter, scopeProvider)
		{
		}
	}

	internal class BrowserConsoleLogger : ILogger
	{
		private Func<string, LogLevel, bool> _filter;
		private readonly IServiceProvider _serviceProvider;

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

		public BrowserConsoleLogger(IServiceProvider serviceProvider, string name, Func<string, LogLevel, bool> filter, IExternalScopeProvider scopeProvider = null)
		{
			if (serviceProvider is null)
			{
				throw new ArgumentNullException(nameof(serviceProvider));
			}
			if (name is null)
			{
				throw new ArgumentNullException(nameof(name));
			}

			_serviceProvider = serviceProvider;
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
					var hub = _serviceProvider.GetService(typeof(IHubContext<BlazorServerConsoleLoggingHub>)) as IHubContext<BlazorServerConsoleLoggingHub>;
					hub?.Clients.All.SendAsync("WriteConsoleLogAsync", message, logLevel);
				}
				catch (Exception ex)
				{
					//HACK: cannot log error in logger...
				}
			}
		}

		public IDisposable BeginScope<TState>(TState state) => ScopeProvider?.Push(state) ?? NullScope.Instance;
	}
}