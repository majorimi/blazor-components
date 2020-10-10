Blazor WebAssembly Hosted model console logging
============
[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-build-check)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=6)
[![Package Version](https://img.shields.io/nuget/v/Majorsoft.Blazor.WebAssembly.Logging.Console?label=Latest%20Version)](https://www.nuget.org/packages/Majorsoft.Blazor.WebAssembly.Logging.Console/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Majorsoft.Blazor.WebAssembly.Logging.Console?label=Downloads)](https://www.nuget.org/packages/Majorsoft.Blazor.WebAssembly.Logging.Console/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/majorimi/blazor-components/blob/master/LICENSE)

# About

Blazor extension for logging to browser console. **Important NOTE**: this package only works for apps using **WebAssemly Hosting Model**!

You can try it out by using the [demo app](https://blazorextensions.z6.web.core.windows.net/extensions).

# Features

## Logger

This package implements [Microsoft Extensions Logging ](https://github.com/dotnet/extensions/tree/master/src/Logging) abstraction to 
support using of `ILogger` and `ILogger<T>` interface for **WebAssemly Blazor** Console logging.

When this package installed and configured all logs written by `ILogger` and `ILogger<T>` will reach
Browser console logger and log messages will appear in the browser's developer tools Console tab.

## Log levels

The logger supports the LogLevels defined by Microsoft [LogLevel enum](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.loglevel?view=dotnet-plat-ext-3.1&viewFallbackFrom=netcore-3.1).

# Configuration

## Installation

Blazor.Components.Deboudnce.Input is available on [NuGet](https://www.nuget.org/packages/Majorsoft.Blazor.WebAssembly.Logging.Console/). 

```sh
dotnet add package Majorsoft.Blazor.WebAssembly.Logging.Console
```
Use the `--version` option to specify a [preview version](https://www.nuget.org/packages/Majorsoft.Blazor.WebAssembly.Logging.Console/absoluteLatest) to install.

## Setup

Add the following code snippet to your WebAssembly hosted (client side) Blazor Application. 
Into the `Program.cs` file 'Main' method.
```
using Blazor.WebAssembly.Logging.Console;

...

builder.Logging.AddBrowserConsole()
	.SetMinimumLevel(LogLevel.Debug); //Setting LogLevel is optional
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
