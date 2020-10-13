Blazor Server Hosted model console logging
============
[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-build-check)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=6)
[![Package Version](https://img.shields.io/nuget/v/Majorsoft.Blazor.Components.CssEvents?label=Latest%20Version)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.CssEvents/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Majorsoft.Blazor.Components.CssEvents?label=Downloads)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.CssEvents/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/majorimi/blazor-components/blob/master/LICENSE)

# About

Blazor Extensions and Components wrapper to notify on CSS **Transition** and **Animation** events.
This is useful when you want to wait for a CSS **Transition** or **Animation** to finish and then continue run C# code, e.g.: hide the element, etc.
 **All components work with WebAssembly and Server hosted models**. 

You can try it out by using the [demo app](https://blazorextensions.z6.web.core.windows.net/cssevents).

# Components

- **`AnimationElement`**: Convinient Blazor component which uses `IAnimationEventsService` to wrap around the given Content and listens for all or one animation Name.
- **`IAnimationEventsService`**: : Low level injectable service which has more features e.g. can aggregate multiple events from multiple HTML elements but must be Disposed manually.
- **`TransitionElement`**:  Convinient Blazor component which uses `ITransitionEventsService` to wrap around the given Content and listens for all or one event property.
- **`ITransitionEventsService`**: Low level injectable service which has more features e.g. can aggregate multiple events from multiple HTML elements but must be Disposed manually.

## CSS Animation events

Blazor Extension and Component wrapper to notify Blazor apps on CSS supported Animation events: `animationstart`, `animationiteration`, `animationend`. 
This is useful when you want to wait for a CSS Animations to finish/restart, etc. and then continue run C# code, e.g.: hide the element, etc.

## CSS Transition events

Blazor Extension and Component wrapper to notify Blazor apps on CSS supported Transition event: `transitionend`. 
This is useful when you want to wait for a CSS Transition to finish and then continue run C# code, e.g.: hide the element, etc.

### `TransitionEventArgs` event arguments
`TransitionEventArgs` is a new EventA


# Configuration

## Installation

Blazor.Components.CssEvents is available on [NuGet](https://www.nuget.org/packages/Majorsoft.Blazor.Components.CssEvents). 

```sh
dotnet add package Majorsoft.Blazor.Components.CssEvents
```
Use the `--version` option to specify a [preview version](https://www.nuget.org/packages/Majorsoft.Blazor.Components.CssEvents/absoluteLatest) to install.

### Usage

Add using statement to your Blazor `<component/page>.razor` file. Or globally reference it into `_Imports.razor` file.

```
@using Blazor.Components.CssEvents
@*Only if you want to use Animations*@
@using Blazor.Components.CssEvents.Animation
@*Only if you want to use Transitions*@
@using Blazor.Components.CssEvents.Transition
```


**In case of WebAssembly project register CSS events services in your `Program.cs` file:**
```
using Blazor.Components.CssEvents;
...
public static async Task Main(string[] args)
{
	var builder = WebAssemblyHostBuilder.CreateDefault(args);

	builder.Services.AddCssEvents();
}
```

**In case of Server hosted project register CSS events services in your `Startup.cs` file:**
```
using Blazor.Components.CssEvents;
...

public void ConfigureServices(IServiceCollection services)
{
	services.AddCssEvents();
}
```