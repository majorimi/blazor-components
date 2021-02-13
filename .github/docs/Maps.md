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

:warning: **To use any of the Map components you must proved a _Token_ or _API Key_.** 
It is available in the service provider (Google, Microsoft, etc.) developer sites.

**NOTE: None of the Majorsoft Maps component tracking or exposing you _Token_ or _API Key_!
Injecting and protecting this _Token_ or _API Key_ in your Blazor application is YOUR responsibility!**

# Components

- **`GoogleStaticMap`**: component is wrapping **Google Static Maps services** into Blazor components.
- **`GoogleMap`**: component is wrapping **Google JavaScript Maps services** into Blazor components.

Maps using `IGeolocationService` (see "Dependences") to center current position.
It can be omitted and injected separately to your components as well to get or track device location. 
To see how it works please check **Geo JS** [documentation](https://github.com/majorimi/blazor-components/blob/master/.github/docs/JsInterop.md#geolocation-js-see-demo-app) and [demo](https://blazorextensions.z6.web.core.windows.net/jsinterop#geo-js).

## `GoogleStaticMap` component (See: [demo app](https://blazorextensions.z6.web.core.windows.net/maps#google-static-maps))

:warning: **To use Google Maps Platform, you must have a billing account. The billing account is used to track costs associated with your projects.**

The Maps Static API returns an image (either GIF, PNG or JPEG) in response to an HTTP request via a URL. For each request, you can specify the location of the map, the size of the image, the zoom level, the type of map, and the placement of optional markers at locations on the map. You can additionally label your markers using alphanumeric characters.

A Maps Static API image is embedded within an `<img>` tag's `src` attribute, or its equivalent in other programming languages.
You can learn about Google Static Maps features and usage [here](https://developers.google.com/maps/documentation/maps-static/overview).

![Modal demo](https://github.com/majorimi/blazor-components/raw/master/.github/docs/gifs/maps_googleStatic.gif)

### Properties
- **`ZoomLevel`: `int { get; set; }` (default: 12)** <br />
Defines the zoom level of the map, which determines the magnification level of the map.
- **`Width`: `int { get; set; }` (default: 400)** <br />
Maps image Width in px.
- **`Height`: `int { get; set; }` (default: 300)** <br />
Maps image Height in px.
- **`HighResolution`: `bool { get; set; }` (default: false)** <br />
Affects the number of pixels that are returned. scale=2 returns twice as many pixels as scale=1 while retaining the
same coverage area and level of detail (i.e. the contents of the map don't change). This is useful when developing for high-resolution displays.
- **`MapType`: `GoogleMapTypes { get; set; }` (default: Roadmap)** <br />
Defines the type of map to construct. There are several possible maptype values, including roadmap, satellite, hybrid, and terrain.
- **`ImageFormat`: `GoogleStaticMapImageFormats { get; set; }` (default: Gif)** <br />
Defines the format of the resulting image. By default, the Maps Static API creates PNG images. There are several possible formats including GIF, JPEG and PNG types.
- **`Language`: `string { get; set; }`** <br />
Defines the language to use for display of labels on map tiles. Note that this parameter is only supported for some country tiles.
- **`Region`: `string { get; set; }`** <br />
Defines the appropriate borders to display, based on geo-political sensitivities.
- **`Center`: `GeolocationData? { get; set; }` (default: _NULL_)** <br />
Maps center position set by the given Coordinates or Address.
Also can be set the device location by setting `CenterCurrentLocationOnLoad` to `true`.
- **`CenterCurrentLocationOnLoad`: `bool { get; set; }` (default: false)** <br />
If set `true` then Maps try to detect device location by using `IGeolocationService` and center on the Map once when Map was first loaded.
**Note:** it will override <see cref="Center"/> location, but detecting geolocation is an `async` operation. It means map might be centered after some time the page rendered or location might fail!
- **`ApiKey`: `string { get; set; }` - Required** <br />
Required allows you to monitor your application's API usage in the Google Cloud Console.
- **`Signature`: `string { get; set; }`** <br />
A digital signature used to verify that any site generating requests using your API key is authorized to do so. Requests without a digital signature might fail.
- **`Markers`: `IEnumerable<GoogleStaticMapMarker>? { get; set; }` (default: _NULL_)** <br />
The markers parameter defines a set of one or more markers (map pins) at a set of locations `GoogleStaticMapMarker`.
- **`Path`: `IEnumerable<GeolocationData>? { get; set; }` (default: _NULL_)** <br />
Defines a single path of two or more connected points to overlay on the image at specified locations `GeolocationData`. 
- **`VisibleLocations`: `IEnumerable<GeolocationData>? { get; set; }` (default: _NULL_)** <br />
Specifies one or more locations that should remain visible on the map, though no markers or other indicators will be displayed `GeolocationData`.
- **`Style`: `string { get; set; }`** <br />
Defines a custom style to alter the presentation of a specific feature (roads, parks, and other features) of the map.
- **`InnerElementReference`: `ElementReference { get; }`** <br />
Exposes a Blazor `ElementReference` of the wrapped around HTML element. It can be used e.g. for JS interop, etc.

**Arbitrary HTML attributes e.g.: `tabindex="20"` will be passed to the corresponding rendered root HTML wrapper element `<div>`**.

### Events
- **`OnCurrentLocationDetected`: `EventCallback<GeolocationData>`** <br />
Callback function called when location successfully detected with `IGeolocationService`.
Device position will be supplied in the event which should be used to override `Center` parameter value.

### Functions
- **`CenterCurrentLocationOnMapAsync()`: `Task CenterCurrentLocationOnMapAsync()`** <br />
Starts an async operation to try to detect device location by using `IGeolocationService`.
Once operation has finished successfully `OnLocationDetected` event will be fired.
- **`DisposeAsync()`: `Task DisposeAsync()`** <br />
Component implements `IAsyncDisposable` interface Blazor framework will call it when parent removed from render tree.


## `GoogleMap` component (See: [demo app](https://blazorextensions.z6.web.core.windows.net/maps#google-js-maps))

:warning: **To use Google Maps Platform, you must have a billing account. The billing account is used to track costs associated with your projects.**

![Modal demo](https://github.com/majorimi/blazor-components/raw/master/.github/docs/gifs/maps_googleJs.gif)

### Properties
- **`InnerElementReference`: `ElementReference { get; }`** <br />
Exposes a Blazor `ElementReference` of the wrapped around HTML element. It can be used e.g. for JS interop, etc.

**Arbitrary HTML attributes e.g.: `tabindex="20"` will be passed to the corresponding rendered root HTML wrapper element `<div>`**.

### Events

### Functions
- **`CenterCurrentLocationOnMapAsync()`: `Task CenterCurrentLocationOnMapAsync()`** <br />
Starts an async operation to try to detect device location by using `IGeolocationService`.
Once operation has finished successfully `OnLocationDetected` event will be fired.
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