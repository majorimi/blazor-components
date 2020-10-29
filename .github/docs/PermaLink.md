Blazor Components PermaLink control and extension
============
[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-build-check)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=6)
[![Package Version](https://img.shields.io/nuget/v/Majorsoft.Blazor.Components.PermaLink?label=Latest%20Version)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.PermaLink/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Majorsoft.Blazor.Components.PermaLink?label=Downloads)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.PermaLink/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/majorimi/blazor-components/blob/master/LICENSE)

# About

Blazor **injectable `IPermaLinkWatcherService` service** and `PermaLinkElement` wrapper component which allows navigation inside Blazor pages (#permalink). 
**All components work with WebAssembly and Server hosted models** (Blazor server side configuration is different). 
For code examples [see usage](https://github.com/majorimi/blazor-components/blob/master/src/Blazor.Components.TestApps.Common/Components/PermaLink.razor).

You can try it out by using the [demo app](https://blazorextensions.z6.web.core.windows.net/permalink#main).

# Components and Services

- **`PermaLinkElement`**: is a wrapper component it renders the given content with `<a>` tag and will add anchor icon with on hover activated Link copy function. 
Hover over the top Header item to copy or navigate to URL as well.
- **`IPermaLinkWatcherService`**: . It is registered as Singleton and should be injected only once for the whole application. 
Best way to use `MainLayout.razor`.

# Configuration

## Installation
Blazor.Components.PermaLink is available on [NuGet](https://www.nuget.org/packages/Majorsoft.Blazor.Components.PermaLink/). 

```sh
dotnet add package Majorsoft.Blazor.Components.PermaLink
```
Use the `--version` option to specify a [preview version](https://www.nuget.org/packages/Majorsoft.Blazor.Components.PermaLink/absoluteLatest) to install.

## Usage

Add using statement to your Blazor <component/page>.razor file. Or globally reference it into `_Imports.razor` file.
```
@using Blazor.Components.PermaLink
```

### Dependences
**Majorsoft.Blazor.Components.PermaLink** package depends on other Majorsoft Nuget packages:
- [Majorsoft.Blazor.Components.Common.JsInterop](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Common.JsInterop)
which handles JS Interop for many features e.g. scrolling, etc.

**In case of WebAssembly project register services in your `Program.cs` file:**
```
using Blazor.Components.Common.JsInterop;
using Blazor.Components.PermaLink;
...
public static async Task Main(string[] args)
{
	var builder = WebAssemblyHostBuilder.CreateDefault(args);

	//Register dependencies
	builder.Services.AddJsInteropExtensions();
	builder.Services.AddPermaLinkWatcher();
}
```

**In case of Server hosted project register dependency services in your `Startup.cs` file:**

**NOTE: PermaLinkWatcher cannot be added here!**
```
using Blazor.Components.Common.JsInterop;
...

public void ConfigureServices(IServiceCollection services)
{
	//Register dependencies
	services.AddJsInteropExtensions();
	//DO NOT Register AddPermaLinkWatcher here!!!!
}
```