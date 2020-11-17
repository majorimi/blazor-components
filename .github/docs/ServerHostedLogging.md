Blazor Server Hosted model console logging
============
[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-build-check)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=6)
[![Package Version](https://img.shields.io/nuget/v/Majorsoft.Blazor.Server.Logging.Console?label=Latest%20Version)](https://www.nuget.org/packages/Majorsoft.Blazor.Server.Logging.Console/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Majorsoft.Blazor.Server.Logging.Console?label=Downloads)](https://www.nuget.org/packages/Majorsoft.Blazor.Server.Logging.Console/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/majorimi/blazor-components/blob/master/LICENSE)

# About

Blazor extension for logging to browser console. **Important NOTE**: this package only works for apps using **Server Hosting Model**!

# Features

## Logger

This package implements [Microsoft Extensions Logging ](https://github.com/dotnet/extensions/tree/master/src/Logging) abstraction to 
support using of `ILogger` and `ILogger<T>` interface for **Server Blazor** Console logging.

When this package installed and configured all logs written by `ILogger` and `ILogger<T>` will reach
Browser console logger and log messages will appear in the browser's developer tools Console tab.

## Log levels

The logger supports the LogLevels defined by Microsoft [LogLevel enum](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.loglevel?view=dotnet-plat-ext-3.1&viewFallbackFrom=netcore-3.1).

# Configuration

## Installation

Blazor.Components.Deboudnce.Input is available on [NuGet](https://www.nuget.org/packages/Majorsoft.Blazor.Server.Logging.Console/). 

```sh
dotnet add package Majorsoft.Blazor.Server.Logging.Console
```
Use the `--version` option to specify a [preview version](https://www.nuget.org/packages/Majorsoft.Blazor.Server.Logging.Consol/absoluteLatest) to install.

## Setup

**Blazor Server hosted model requires more complicated setup. Since your code runs on the Sever and logs has to be transfered to the Browser.**

**Follow the next 3 required steps to make it work.**

#### Step 1. Register service and LoggerProvider

Add the following code snippet to your Server hosted Blazor Application. 
Into the **`Program.cs`** file 'CreateHostBuilder' method.
```
using Blazor.Server.Logging.Console;

...

public static IHostBuilder CreateHostBuilder(string[] args) =>
	Host.CreateDefaultBuilder(args)
		.ConfigureLogging(logger =>
		{
			logger.AddBrowserConsole()
			  .SetMinimumLevel(LogLevel.Trace) //Setting LogLevel is optional NOTE: appsettings.json config overrides this
			  .AddFilter("Microsoft", LogLevel.Information); //System logs can be filtered
		})
		.ConfigureWebHostDefaults(webBuilder =>
		{
			webBuilder.UseStartup<Startup>();
		});
```

#### Step 2. Register SignalR Hub

All SignalR Hubs must be registered with `MapHub` method in the **`Startup.cs`** file `Configure` method.

```
public void Configure(IApplicationBuilder app)
{
	...
	app.UseEndpoints(endpoints =>
	{
		//Blazor server code
		endpoints.MapBlazorHub();
		endpoints.MapFallbackToPage("/_Host");
		//Logging setup
		endpoints.MapHub<BlazorServerConsoleLoggingHub>(BlazorServerConsoleLoggingHub.HubUrl);
	});
}
```


#### Step 3. Start SignalR connection

Initialize and start SignalR connection with Sever. It should be executed only once.
Best place to do it is in **`MainLayout.razor`** file.
```
@using Blazor.Server.Logging.Console
@inject IBrowserConsoleLoggerService _browserConsoleLogger

@implements IAsyncDisposable

@code{
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if(firstRender)
		{
			//setup console log
			await _browserConsoleLogger.StartLoggerAsync();
		}
	}

	public async ValueTask DisposeAsync()
	{
		if (_browserConsoleLogger is not null)
		{
			await _browserConsoleLogger.DisposeAsync();
		}
	}
}
```

## Usage

After setup usage is very simple. Just use by standard logging with injected `ILogger` object. The following code snippet shows how to use logger in a Blazor component.

```
@using Microsoft.Extensions.Logging

@inject ILogger<Index> _logger
@code {
	protected override void OnInitialized()
	{
		_logger.LogDebug("Index init");
	}
}
```

The following code snippet shows how to use logger in `.cs` files.
```
using Microsoft.Extensions.Logging;

...

public class CustomCode
{
	private readonly ILogger<CustomCode> _logger;
	public CustomCode(ILogger<CustomCode> logger)
	{
		_logger = logger;
		_logger.LogDebug("CustomCode init");
	}
}
```

