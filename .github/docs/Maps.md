Blazor Components Maps control
============
[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-build-check)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=6)
[![Package Version](https://img.shields.io/nuget/v/Majorsoft.Blazor.Components.Maps?label=Latest%20Version)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Maps/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Majorsoft.Blazor.Components.Maps?label=Downloads)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Maps/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/majorimi/blazor-components/blob/master/LICENSE)

# About

Blazor components that renders **Google/Bing maps** wrapped into Blazor components allowing to control and mange maps with .Net code. 
**All components work with WebAssembly and Server hosted models**. 
For code examples [see usage](https://github.com/majorimi/blazor-components/blob/master/src/Majorsoft.Blazor.Components.TestApps.Common/Components/Maps.razor).

You can try it out by using the [demo app](https://blazorextensions.z6.web.core.windows.net/maps).

![Modal demo](https://github.com/majorimi/blazor-components/raw/master/.github/docs/gifs/maps.gif)

:warning: **To use any of the Map components you must proved a _Token_ or _API Key_.** 
It is available in the service provider (Google, Microsoft, etc.) developer sites.

:warning: **None of the Majorsoft Maps component tracking or exposing you _Token_ or _API Key_!
Injecting and protecting this _Token_ or _API Key_ in your Blazor application is YOUR responsibility!**

# Components

- **`GoogleStaticMap`**: component is wrapping **Google Static Maps services** into Blazor components.
- **`GoogleMap`**: component is wrapping **Google JavaScript Maps services** into Blazor components.

Maps using `IGeolocationService` (see "Dependences") to center current position.
It can be omitted and injected separately to your components as well to get or track device location. 
To see how it works please check **Geo JS** [documentation](https://github.com/majorimi/blazor-components/blob/master/.github/docs/JsInterop.md#geolocation-js-see-demo-app) and [demo](https://blazorextensions.z6.web.core.windows.net/jsinterop#geo-js).

## `GoogleStaticMap` component (See: [demo app](https://blazorextensions.z6.web.core.windows.net/maps#google-static-maps))

:warning: **To use Google Maps Platform, you must have a billing account. The billing account is used to track costs associated with your projects.**

### Properties
- **`Header`: `RenderFragment` HTML content** <br />
HTML content to show on the Modal header (top), right to the close button (if visible). Can be any valid HTML but should be only Title text. 
**Must not be defined if you want to leave it out. Also `ShowCloseButton` must be set to `false`**
- **`Content`: `RenderFragment` HTML content - Required** <br />
Required HTML content to show on the Modal dialog. Can be any valid HTML.

**Arbitrary HTML attributes e.g.: `tabindex="20"` will be passed to the corresponding rendered root HTML wrapper element `<div>`**.

### Events
- **`OnOpen`: `EventCallback`** <br />
Callback function called when the Modal dialog is opening.
- **`OnClose`: `EventCallback`** <br />
Callback function called when the Modal dialog is closing.

### Functions
- **`CenterCurrentLocationOnMapAsync()`: `Task CenterCurrentLocationOnMapAsync()`** <br />
When method called Modal dialog will be opened. It should be `await`-ed.
- **`DisposeAsync()`: `Task DisposeAsync()`** <br />
Component implements `IAsyncDisposable` interface Blazor framework will call it when parent removed from render tree.




# Configuration

## Installation

**Majorsoft.Blazor.Components.Maps** is available on [NuGet](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Maps). 

```sh
dotnet add package Majorsoft.Blazor.Components.Maps
```
Use the `--version` option to specify a [preview version](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Maps/absoluteLatest) to install.

## Usage

Add using statement to your Blazor `<component/page>.razor` file. Or globally reference it into `_Imports.razor` file.

```
@using Majorsoft.Blazor.Components.Maps

@*Only if you want to use Google Maps*@
@using Majorsoft.Blazor.Components.Maps.Google

@*Only if you want to use Bing Maps*@
@using Majorsoft.Blazor.Components.Maps.Bing

@*Other Maps dependencies*@
@using Majorsoft.Blazor.Components.Common.JsInterop.Geo
@using Majorsoft.Blazor.Components.Core.Extensions
```

### Dependences
**Majorsoft.Blazor.Components.Maps** package depends on other Majorsoft Nuget packages:
- [Majorsoft.Blazor.Components.Common.JsInterop](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Common.JsInterop)
which handles JS Interop for Geolocation services.

**In case of WebAssembly project register services in your `Program.cs` file:**
```

```

**In case of Server hosted project register services in your `Startup.cs` file:**
```

```

### `ModalDialog` usage
Following code example shows how to use **`ModalDialog`** component. Most important is you have to **add `@ref=""` Blazor tag** to the component in order to access it in your code.

This example shows a simple dialog with Content message. **Blazor does not support empty content check.** Which means if you want to skip, remove **Header** and **Footer** 
you should not define it. **To achieve this do not place Header or Footer into the HTML markup.**
```

```


This example shows a fully customized dialog with **Header**, **Content** and **Footer** sections.

```
```