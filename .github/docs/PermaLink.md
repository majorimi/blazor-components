Blazor Components PermaLink control and extension
============
[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-build-check)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=6)
[![Package Version](https://img.shields.io/nuget/v/Majorsoft.Blazor.Components.PermaLink?label=Latest%20Version)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.PermaLink/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Majorsoft.Blazor.Components.PermaLink?label=Downloads)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.PermaLink/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/majorimi/blazor-components/blob/master/LICENSE)

# About

Blazor **injectable `IPermaLinkWatcherService` service** and `PermaLinkElement` wrapper component which allows navigation inside Blazor pages (#permalink). 

**All components work with WebAssembly and Server hosted models** (Blazor server side configuration is different). 
For code examples [see usage](https://github.com/majorimi/blazor-components/blob/master/src/Majorsoft.Blazor.Components.TestApps.Common/Components/Permalink.razor).

You can try it out by using the [demo app](https://blazorextensions.z6.web.core.windows.net/permalink#main).

![Modal demo](https://github.com/majorimi/blazor-components-docs/raw/main/github/docs/gifs/permaLink.gif)

# Components and Services

- **`PermaLinkElement`**: is a wrapper component it renders the given content with `<a>` tag and will add anchor icon with on hover activated Link copy function. 
Hover over the top Header item to copy or navigate to URL as well.
- **`IPermaLinkWatcherService`**: is registered as Singleton and should be injected only once for the whole application. 
Best way to use `MainLayout.razor`.
- **`PermaLinkBlazorServerInitializer`**: (from v1.4.0) convenient wrapper component to initialize navigation watcher in your Blazor Server App `MainLayout.razor` page. **Only one Initializer component allowed per Application.**
- **`PermalinkBlazorWasmInitializer`**: (from v1.4.0) convenient wrapper component to initialize navigation watcher in your Blazor WebAssembly App `MainLayout.razor` page. **Only one Initializer component allowed per Application.**

## `IPermaLinkWatcherService` extension
This is the main service which makes Permalink navigation possible. **Should be used as a Singleton** only in `MainLayout.razor` file.
For more details see Usage section.

### Functions
- **`WatchPermaLinks()`**: **`void WatchPermaLinks()`** <br />
Starts a navigation watcher which will check for Permalinks in the URLs.
- **`ChangePermalink()`**: **`void ChangePermalink(string? newPermalink, bool doNotNavigate)`** <br />
Modify the current URL with given new peralink value and trigger navigation or just update browser History.
- **`CheckPermalink()`**: **`string? CheckPermalink(bool triggerEvent = false)`** <br />
Checks the current URL for permalink again and re-triggers `PermalinkDetected` event if requested.
- **`Dispose()`: `@implements IDisposable` interface** <br />
Component implements `IDisposable` interface Blazor framework will call it when parent removed from render tree.

### Events
- **`PermalinkDetected`: `event EventHandler<PermalinkDetectedEventArgs>** <br />
Event handler for parmalinks detected on navigation.

## `PermaLinkElement` component

### Properties
- **`Id`: `string { get; }`** <br />
Map HTML container Id. It can be used when multiple Permalinks added to one page.
- **`Content`**: **`RenderFragment` HTML content - Required**  <br />
Required HTML content to render with mouse enter/leave events to show Permalink icon. **NOTE: it can be any arbitrary HTML elemnt but should be just a header text e.g. `<h1> - <h6>` element.**
- **`PermaLinkName`**: **`string PermaLinkName { get; set; }` - Required**  <br />
Value of the rendered `<a>` HTML tag. **NOTE: it is required and will be part of the URL, do not add `#` use non URL friendly characters!**
- **`ShowIcon`**: **`ShowPermaLinkIcon ShowIcon { get; set; }` - (default: enum `ShowPermaLinkIcon.OnHover`)**  <br />
Enum value which sets how to show clickable permalink the icon: `No`, `OnHover`, `Always`.
- **`IconPosition`**: **`PermaLinkIconPosition IconPosition { get; set; }` - (default: enum `PermaLinkIconPosition.Right`)**  <br />
Enum value which sets where to show the icon: `Left`, `Right`.
- **`IconMarginTop`**: **`double IconMarginTop { get; set; }` - (default: 0)**  <br />
Sets the icon `margin-top` CSS property in `px`. With this icon can be centered.
- **`IconSize`**: **`int IconSize { get; set; }` - (default: 16)**  <br />
Sets the icon `<img width="" height="" />` values.
- **`IconStyle`**: **`PermaLinkStyle IconStyle { get; set; }` - (default: enum `PermaLinkStyle.Normal`)**  <br />
Enum value which sets the  displayed icon type: `Normal`, `Bold`.
- **`IconActions`**: **`PermaLinkIconActions IconActions { get; set; }` - (default: FLAG enum `PermaLinkIconActions.Copy`)**  <br />
FLAG Enum value which sets the behaviour of the icon click: `Copy`, `Navigate`. Flag values can be combined: `PermaLinkIconActions.Copy|PermaLinkIconActions.Navigate`.
- **`InnerElementReference`: `ElementReference { get; }`** <br />
  Exposes a Blazor `ElementReference` of the wrapped around HTML element. It can be used e.g. for JS interop, etc.

**Arbitrary HTML attributes e.g.: id="btn1" will be passed to the corresponding rendered <div> HTML element wrapper.**

### Events
- **`OnLoading`**: **`EventCallback` delegate**  <br />
Callback function called when Permalink icon clicked and **`PermaLinkIconActions.Copy`** feature was set.

## PermaLinkBlazorServerInitializer
Available from v1.4.0 It is convenient wrapper component to initialize navigation watcher in your Blazor Server App `MainLayout.razor` page. **Only one Initializer component allowed per Application.**

### Properties
- **`SmootScroll`: `bool { get; set; }` (default: false)** <br />
Scroll should be jump or smoothly scroll. **Note: smooth scroll on element level might not supported by all browsers.**

## PermalinkBlazorWasmInitializer
Available from v1.4.0 It is convenient wrapper component to initialize navigation watcher in your Blazor WebAssembly App `MainLayout.razor` page. **Only one Initializer component allowed per Application.**

### Properties
- **`SmootScroll`: `bool { get; set; }` (default: false)** <br />
Scroll should be jump or smoothly scroll. **Note: smooth scroll on element level might not supported by all browsers.**

# Configuration

## Installation

**Majorsoft.Blazor.Components.PermaLink** is available on [NuGet](https://www.nuget.org/packages/Majorsoft.Blazor.Components.PermaLink/). 

```sh
dotnet add package Majorsoft.Blazor.Components.PermaLink
```
Use the `--version` option to specify a [preview version](https://www.nuget.org/packages/Majorsoft.Blazor.Components.PermaLink/absoluteLatest) to install.

## Usage

Add using statement to your Blazor <component/page>.razor file. Or globally reference it into `_Imports.razor` file.
```
@using Majorsoft.Blazor.Components.PermaLink
```

### Dependences
**Majorsoft.Blazor.Components.PermaLink** package depends on other Majorsoft Nuget packages:
- [Majorsoft.Blazor.Components.Common.JsInterop](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Common.JsInterop)
which handles JS Interop for many features e.g. scrolling, etc.

#### WebAssembly projects

**In case of WebAssembly project register services in your `Program.cs` file:**
```
using Majorsoft.Blazor.Components.Common.JsInterop;
using Majorsoft.Blazor.Components.PermaLink;
...
public static async Task Main(string[] args)
{
	var builder = WebAssemblyHostBuilder.CreateDefault(args);

	//Register dependencies
	builder.Services.AddJsInteropExtensions();
	//Register service
	builder.Services.AddPermaLinkWatcher();
}
```

**Use `MainLayout.razor` file for WebAssembly project to activate Permalink watcher only once:**
Inject the single instance of `IPermaLinkWatcherService` to activate Permalink watcher in `OnInitialized` method. 
Also instance should be disposed.

```
@using Majorsoft.Blazor.Components.PermaLink

@inject IPermaLinkWatcherService _permalinkWatcher

@implements IDisposable

@code {
	protected override void OnInitialized()
	{
		_permalinkWatcher.WatchPermaLinks();
	}

	public void Dispose()
	{
		_permalinkWatcher.Dispose();
	}
}
```

**From v1.4.0 a simpler initializer is available!**
**Only one Initializer component allowed per Application.**
```
@*Permalink initialize*@
@using Majorsoft.Blazor.Components.PermaLink
<PermalinkBlazorWasmInitializer  SmoothScroll="false" />
```

#### Server hosted projects

**In case of Server hosted project register dependency services in your `Startup.cs` file:**

**NOTE: PermaLinkWatcher cannot be added here!**
```
using Majorsoft.Blazor.Components.Common.JsInterop;
...

public void ConfigureServices(IServiceCollection services)
{
	//Register dependencies
	services.AddJsInteropExtensions();
	//DO NOT Register AddPermaLinkWatcher here!!!!
}
```

**Use `MainLayout.razor` file for Server hosted project to activate Permalink watcher only once:**

**Imprortant Note: `IPermaLinkWatcherService` instance cannot be injected in case of Server hosted projects!**
It has to be instantiated manually by using the following code. Also instance should be disposed:

```
@using Majorsoft.Blazor.Components.PermaLink
@using Majorsoft.Blazor.Components.Common.JsInterop.Scroll
@using Microsoft.AspNetCore.Components.Routing

@inject IScrollHandler _scrollHandler
@inject NavigationManager _navigationManager
@inject ILogger<IPermaLinkWatcherService> _logger

@implements IDisposable
@implements IAsyncDisposable

@code{
	private IPermaLinkWatcherService _permalinkWatcher;
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if(firstRender)
		{
			//setup permalink
			_permalinkWatcher = new PermaLinkWatcherService(_scrollHandler, _navigationManager, _logger);
			_permalinkWatcher.WatchPermaLinks();
		}
	}

	public async ValueTask DisposeAsync()
	{
		if (_scrollHandler is not null)
		{
			await _scrollHandler.DisposeAsync();
		}
	}
	public void Dispose()
	{
		_permalinkWatcher?.Dispose();
	}
}
```

**From v1.4.0 a simpler initializer is available!** **Only one Initializer component allowed per Application.**
```
@*Permalink initialize*@
@using Majorsoft.Blazor.Components.PermaLink
<PermaLinkBlazorServerInitializer SmoothScroll="false" />
```

#### Creating permalink (#) navigation points inside a Blazor page

This is a standard HTML `a` tag. Apply the well known **`<a name="your-section-name"></a>`** anchor element in your document
which should be activated when Navigation happened.

**NOTE: Please do not add `#` or ` ` (space) or any other characters which are not URL compatible. Or you have to encode it.**

#### Using `PermaLinkElement` component

It will render the given content and also generate the same standard `a` tag before it. One advantage of using this component it can render
a clickable **Link** icon which will copy the URL and/or navigate to the permalink.

```
<PermaLinkElement 
	PermaLinkName="PermaLink-Element-wrapper" 
	ShowIcon="@_showIcon"
	IconPosition="@_iconPosition"
	IconActions="@(_action)"
	IconStyle="_iconStyle"
	IconSize="18"
	IconMarginTop="7"
	OnPermaLinkCopied="@LinkCopied">
	<Content>
		<h3>Header item with permalink action icon</h3>
	</Content>
</PermaLinkElement>

@code{
	private ShowPermaLinkIcon _showIcon = ShowPermaLinkIcon.OnHover;
	private PermaLinkIconPosition _iconPosition = PermaLinkIconPosition.Right;
	private PermaLinkStyle _iconStyle;
	private PermaLinkIconActions _action = PermaLinkIconActions.Copy|PermaLinkIconActions.Navigate;

	private async Task LinkCopied(string uri)
	{
		//Custom code for copy to Clipboard...
	}
}

```