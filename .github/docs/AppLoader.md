Blazor WASM AppLoader Components
============
[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-build-check)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=6)
[![Package Version](https://img.shields.io/nuget/v/Majorsoft.Blazor.Components.WASM.AppLoader?label=Latest%20Version)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.WASM.AppLoader/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Majorsoft.Blazor.Components.WASM.AppLoader?label=Downloads)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.WASM.AppLoader/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/majorimi/blazor-components/blob/master/LICENSE)

# About

Blazor component that can render a customized Loader screen during WASM application loading time (used to could be added to the removed Index.html file).
**This components work with WebAssembly (WASM). For Server side render there is no loader needed since no Framework and Application donwload and delay**. 
For code examples [see usage](https://github.com/majorimi/blazor-components/blob/master/src/Majorsoft.Blazor.Components.TestApp.Client/Layout/MainLayout.razor).

You can try it out by using the [demo app](https://blazorextensions.z6.web.core.windows.net/) loading the application WASM AppLoader used.

# Components

- **`AppLoader`**: Renders `AppLoader` component which is a customizable "Splash" screen. Previously it was used as a part of the `Index.html` file but now it is a 
Blazor component that can be used in the `MainLayout.razor` file. It is rendered during the application loading time and it is hidden when the application is fully loaded.

![WASM AppLoader demo](https://raw.githubusercontent.com/majorimi/blazor-components-docs/main/github/docs/gifs/AppLoader.gif)

### Properties
- **`ChildContent`: `RenderFragment` HTML content - Required** <br />
This is the content that will be displayed within the loader in the center of the screen.
- **`DelayTimeOut`: `int` - Delay time in milliseconds to show the loader** <br />
This is the time while the loader is visible. Default is 1500ms.

# Configuration

## Installation

**Majorsoft.Blazor.Components.WASM.AppLoader** is available on [NuGet](https://www.nuget.org/packages/Majorsoft.Blazor.Components.WASM.AppLoader/). 

```sh
dotnet add package Majorsoft.Blazor.Components.WASM.AppLoader
```
Use the `--version` option to specify a [preview version](https://www.nuget.org/packages/Majorsoft.Blazor.Components.WASM.AppLoader/absoluteLatest) to install.

## Usage

Add using statement to your Blazor `<component/page>.razor` file. Or globally reference it into `_Imports.razor` file.
```
@using Majorsoft.Blazor.Components.WASM.AppLoader
```

### `AppLoader` usage

Following code example shows how to use **`AppLoader`** component in your <App>.Client project.  In the 'Layout/MainLayout.razor' file add the following code
near the `@Body` tag (no conditions needed for visibility it is automatically handled internally in the component):

```
    <AppLoader DelayTimeOut="1500">
        @* Customize Splash screen here *@
        <img style="margin: 20px;" src="./blazor.app.png" />
        <h4>MY APPLIVATION <br /> Loading...</h4>
    </AppLoader>

    @Body
```
