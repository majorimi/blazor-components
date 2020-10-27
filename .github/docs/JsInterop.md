Blazor Js Interop components and extensions
============
[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-build-check)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=6)
[![Package Version](https://img.shields.io/nuget/v/Majorsoft.Blazor.Components.Common.JsInterop?label=Latest%20Version)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Common.JsInterop/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Majorsoft.Blazor.Components.Common.JsInterop?label=Downloads)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Common.JsInterop/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/majorimi/blazor-components/blob/master/LICENSE)

# About

Collection of Blazor components and extension methods that provide useful functionality which can be achieved only with Js Interop.
**All components work with WebAssembly and Server hosted models**. 
For code examples [see usage](https://github.com/majorimi/blazor-components/blob/master/src/Blazor.Components.TestApps.Common/Components/JSInterop.razor).

You can try it out by using the [demo app](https://blazorextensions.z6.web.core.windows.net/jsinterop).

# Features

- **Click JS**: is a component which wraps the given content to a DIV and subscribes to all click events: `OnOutsideClick`, `OnInsideClick`. 
 Also an **injectable `IClickBoundariesHandler` service** for callback event handlers.
- **Focus JS**: is an injectable `IFocusHandler` service. **Focus JS is able to identify and restore focus on ANY DOM element without using Blazor `@ref=""` tag.**
- **Element info JS**: is a set of **Extension methods** for `ElementReference` objects.
- **Scroll JS**: is a set of **Extension methods** for `ElementReference` objects. Also an **injectable `IScrollHandler` service** for non element level functions and callback event handlers.


# Configuration

## Installation

Blazor.Components.Common.JsInterop is available on [NuGet](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Common.JsInterop/). 

```sh
dotnet add package Majorsoft.Blazor.Components.Common.JsInterop
```
Use the `--version` option to specify a [preview version](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Common.JsInterop/absoluteLatest) to install.

## Usage

Add using statement to your Blazor <component/page>.razor file. Or globally reference it into `_Imports.razor` file.
```
@using Blazor.Components.Common.JsInterop
@*Only if you want to use Scroll*@
@using Blazor.Components.Common.JsInterop.Scroll
@*Only if you want to use Focus*@
@using Blazor.Components.Common.JsInterop.Focus
@*Only if you want to use Click*@
@using Blazor.Components.Common.JsInterop.Click
@*Only if you want to use ElementInfo*@
@using Blazor.Components.Common.JsInterop.ElementInfo
```


**In case of WebAssembly project register CSS events services in your `Program.cs` file:**
```
using Blazor.Components.CssEvents;
...
public static async Task Main(string[] args)
{
	var builder = WebAssemblyHostBuilder.CreateDefault(args);

	builder.Services.AddJsInteropExtensions();
}
```

**In case of Server hosted project register CSS events services in your `Startup.cs` file:**
```
using Blazor.Components.CssEvents;
...

public void ConfigureServices(IServiceCollection services)
{
	services.AddJsInteropExtensions();
}
```