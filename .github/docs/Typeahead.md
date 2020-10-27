Blazor Components Typeahead controls
============
[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-build-check)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=6)
[![Package Version](https://img.shields.io/nuget/v/Majorsoft.Blazor.Components.Typeahead?label=Latest%20Version)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Typeahead/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Majorsoft.Blazor.Components.Typeahead?label=Downloads)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Typeahead/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/majorimi/blazor-components/blob/master/LICENSE)

# About

Blazor component that renders an HTML `<input>` or **Blazor** provided `InputText` with Typeahead panel with optional debounce (delay) and minimal required chars. **All components work with WebAssembly and Server hosted models**. 
For code examples [see usage](https://github.com/majorimi/blazor-components/blob/master/src/Blazor.Components.TestApps.Common/Components/Typeahead.razor).

You can try it out by using the [demo app](https://blazorextensions.z6.web.core.windows.net/typeahead).

# Components
- **`DebounceInput`**: Wraps around HTML `<input>` control and adds Typeahead control with optional debounce (delay) and minimal required chars. 
- **`DebounceInput`**: Wraps around **Blazor InputText** control which enables form validation and adds Typeahead control with optional debounce (delay) and minimal required chars.


# Configuration

## Installation

Blazor.Components.Typeahead is available on [NuGet](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Typeahead/). 

```sh
dotnet add package Majorsoft.Blazor.Components.Typeahead
```
Use the `--version` option to specify a [preview version](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Typeahead/absoluteLatest) to install.

## Usage

Add using statement to your Blazor <component/page>.razor file. Or globally reference it into `_Imports.razor` file.
```
@using Blazor.Components.Typeahead
```

### Dependences
**Majorsoft.Blazor.Components.Typeahead** package depends on other Majorsoft Nuget packages:
- [Majorsoft.Blazor.Components.Debounce](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Debounce)
which provides the base input control with Debounce feature.
- [Majorsoft.Blazor.Components.Common.JsInterop](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Common.JsInterop)
which handles JS Interop for many features e.g. scrolling, etc.

**In case of WebAssembly project register services in your `Program.cs` file:**
```
using Blazor.Components.CssEvents;
...
public static async Task Main(string[] args)
{
	var builder = WebAssemblyHostBuilder.CreateDefault(args);

	//Register dependencies
	builder.Services.AddJsInteropExtensions();
}
```

**In case of Server hosted project register services in your `Startup.cs` file:**
```
using Blazor.Components.CssEvents;
...

public void ConfigureServices(IServiceCollection services)
{
	//Register dependencies
	services.AddJsInteropExtensions();
}
```