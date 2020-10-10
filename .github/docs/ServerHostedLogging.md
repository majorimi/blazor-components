Blazor Server Hosted model console logging
============
[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-build-check)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=6)
[![Package Version](https://img.shields.io/nuget/v/Majorsoft.Blazor.Server.Logging.Console?label=Latest%20Version)](https://www.nuget.org/packages/Majorsoft.Blazor.Server.Logging.Console/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Majorsoft.Blazor.Server.Logging.Console?label=Downloads)](https://www.nuget.org/packages/Majorsoft.Blazor.Server.Logging.Console/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/majorimi/blazor-components/blob/master/LICENSE)

# About

Blazor extension for logging to browser console. **Important NOTE**: this package only works for apps using **Server Hosting Model**!

You can try it out by using the [demo app](https://blazorextensions.z6.web.core.windows.net/extensions).

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

Add the following code snippet to your WebAssembly hosted (client side) Blazor Application. 
Into the `Program.cs` file 'CreateHostBuilder' method.
```
using Blazor.Server.Logging.Console;

...

public static IHostBuilder CreateHostBuilder(string[] args) =>
	Host.CreateDefaultBuilder(args)
		.ConfigureWebHostDefaults(webBuilder =>
		{
			webBuilder.ConfigureLogging(logger =>
			{
				logger.SetMinimumLevel(LogLevel.Debug); //Setting LogLevel is optional
				logger.AddBrowserConsole();
			});
			webBuilder.UseStartup<Startup>();
		});
```

**Hack solution in v0.9 package!!!**

Currently Blazor.Server.Logging.Console package using 'IJSRuntime'. However 'ILogger' created during startup before any JS runtime created.
So the current solution is to 're-inject' a valid instans of `IJSRuntime` when it is ready. Best place to do it is in `MainLayout.razor` file.

```
@using Blazor.Server.Logging.Console;
@inject IJSRuntime _runtime;

@code{
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if(firstRender)
		{
			//setup logger
			BrowserConsoleLoggerProvider.SetLogger(_runtime);
		}
	}
}
```

## Usage

After correct setup usage is very simple by logging with standard injected `ILogger` object. The following code snippet shows how to use logger in a Blazor component.
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

