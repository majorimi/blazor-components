Blazor Js Interop components and extensions
============
[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-build-check)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=6)
[![Package Version](https://img.shields.io/nuget/v/Majorsoft.Blazor.Components.Common.JsInterop?label=Latest%20Version)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Common.JsInterop/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Majorsoft.Blazor.Components.Common.JsInterop?label=Downloads)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Common.JsInterop/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/majorimi/blazor-components/blob/master/LICENSE)

# About

Collection of Blazor components, injectable services and extension methods that provides useful functionality and event notifications which can be achieved only with JS Interop e.g. 
	scroll, clipboard, focus, resize, language detection, Geolocation, etc..
**All components work with WebAssembly and Server hosted models**. 
For code examples [see usage](https://github.com/majorimi/blazor-components/blob/master/src/Majorsoft.Blazor.Components.TestApps.Common/Components/JSInterop.razor).

You can try it out by using the [demo app](https://blazorextensions.z6.web.core.windows.net/jsinterop).

# Features

- **Click JS**: `ClickBoundariesElement` is a component which wraps the given content to a DIV and subscribes to all click events: `OnOutsideClick`, `OnInsideClick`. 
 Also an **injectable `IClickBoundariesHandler` service** for callback event handlers.
- **Global Mouse JS**: is an **injectable `IGlobalMouseEventHandler` service** for global mouse callback event handlers.
- **Focus JS**: is an injectable `IFocusHandler` service. **Focus JS is able to identify and restore focus on ANY DOM element without using Blazor `@ref=""` tag.**
- **Element info JS**: is a set of **Extension methods** for `ElementReference` objects.
- **Scroll JS**: is a set of **Extension methods** for `ElementReference` objects. Also an **injectable `IScrollHandler` service** for non element level functions and callback event handlers.
- **Resize JS**: is an **injectable `IResizeHandler` service** for Window (global) and HTML Elements resize event callback handlers.
- **Clipboard JS**: is an **injectable `IClipboardHandler` service** for accessing computer Clipboard from Blazor Application.
- **Language JS**: is an **injectable `ILanguageService` service** for detect the browser language preference.
- **Geo JS**: is an **injectable `IGeolocationService` service** for detect the device Geolocation (GPS position, speed, heading, etc.).

## Click JS (See: [demo app](https://blazorextensions.z6.web.core.windows.net/jsinterop#click-js))
**NOTE: Blazor supports `@onclick` event which is equivalent with `OnInsideClick`. 
This component useful when need to detect if click happened outside (anywhere in the document) of the component with `OnOutsideClick`.**

### `ClickBoundariesElement` component
`ClickBoundariesElement` is a component which wraps the given content to a DIV and subscribes to all click events: `OnOutsideClick`, `OnInsideClick`. 

#### Properties
- **`Content`: `RenderFragment` HTML content - Required** <br />
Required HTML content which will be wrapped into a `<span>` which has the Click events listener registered.
- **`OnOutsideClick`: `EventCallback<MouseEventArgs>` delegate** <br />
Callback function called when clicked outside of the given element.
- **`OnInsideClick`: `EventCallback<MouseEventArgs>` delegate** <br />
Callback function called when clicked inside of the given element.

**Arbitrary HTML attributes e.g.: `id="load1"` will be passed to the corresponding rendered HTML wrapper element `<span>`**.

#### Functions
- **`DisposeAsync()`: `ValueTask IAsyncDisposable()` interface** <br />
Component implements `IAsyncDisposable` interface Blazor framework components also can `@implements IAsyncDisposable` where the injected service should be Disposed.

### `IClickBoundariesHandler` service
**Injectable `IClickBoundariesHandler` service** to handle JS 'click' events for the whole document. 

#### Functions
- **`RegisterClickBoundariesAsync`**: **`Task RegisterClickBoundariesAsync(ElementReference elementRef, Func<MouseEventArgs, Task> outsideClickCallback = null, Func<MouseEventArgs, Task> insideClickCallback = null)`** <br />
 Adds event listener for 'click' HTML event for the given element with property filter.
- **`RemoveClickBoundariesAsync`**: **`Task RemoveClickBoundariesAsync(ElementReference elementRef)`** <br />
Removes event listener for 'click' HTML event for the given element.
- **`DisposeAsync()`: `ValueTask IAsyncDisposable()` interface** <br />
Implements `IAsyncDisposable` interface the injected service should be Disposed.

## Global Mouse JS (See: [demo app](https://blazorextensions.z6.web.core.windows.net/jsinterop#mouse-js))
**Injectable `IGlobalMouseEventHandler` service** for global mouse callback event handlers.
Blazor supports `@onclick`, `@onmousemove`, etc. events but only for Blazor rendered elements. **With this service you can get similar event notifications for the whole document/window.**

**NOTE: this is similar to `Click JS` which can detect global mouse `click` events as well but usage is different, 
events are registered to a given Blazor `ElementReference`.**

### Functions
- **`RegisterPageMouseMoveAsync`**: **`Task<string> RegisterPageMouseMoveAsync(Func<MouseEventArgs, Task> mouseMoveCallback)`** <br />
Adds event listener for mouse 'move' HTML event for the whole document/window.
- **`RemovePageMouseMoveAsync`**: **`Task RemovePageMouseMoveAsync(string eventId)`** <br />
Removes event listener for mouse 'move' HTML event for the whole document/window by the given event Id.
- **`RegisterPageMouseDownAsync`**: **`Task<string> RegisterPageMouseDownAsync(Func<MouseEventArgs, Task> mouseDownCallback)`** <br />
Adds event listener for mouse 'down' HTML event for the whole document/window.
- **`RemovePageMouseDownAsync`**: **`Task RemovePageMouseDownAsync(string eventId)`** <br />
- **`RegisterPageMouseUpAsync`**: **`Task<string> RegisterPageMouseUpAsync(Func<MouseEventArgs, Task> mouseUpCallback)`** <br />
Adds event listener for mouse 'up' HTML event for the whole document/window.
- **`RemovePageMouseUpAsync`**: **`Task RemovePageMouseUpAsync(string eventId)`** <br />
Removes event listener for mouse 'up' HTML event for the whole document/window by the given event Id.

## Focus JS (See: [demo app](https://blazorextensions.z6.web.core.windows.net/jsinterop#focus-js))
Injectable `IFocusHandler` service to handle JS 'focus' Interops.
**Focus JS is able to identify and restore focus on ANY DOM element without using Blazor `@ref=""` tag.** 
It is useful when **focus should be restored on a parent component but element reference not stored or it's on a different component**.

### Functions
- **`GetFocusedElementAsync`**: **`Task<IJSObjectReference> GetFocusedElementAsync()`** <br />
Returns the actually focused HTML DOM element reference. It works with outside element of a Blazor component.
**Note: `IJSObjectReference` object is disposable.**
- **`FocusElementAsync`**: **`Task FocusElementAsync(IJSObjectReference objectReference)`** <br />
Sets focus on the given HTML DOM element reference. **Note: `IJSObjectReference` object is disposable.**
- **`StoreFocusedElementAsync`**: **`Task StoreFocusedElementAsync()`** <br />
Stores the actually focused HTML DOM element reference into a JS variable. This can be restored by calling `RestoreStoredElementFocusAsync` method.
- **`RestoreStoredElementFocusAsync`**: **`Task RestoreStoredElementFocusAsync(bool clearElementRef = true)`** <br />
Restores the HTML DOM element reference stored by calling `StoreFocusedElementAsync` method.
- **`DisposeAsync()`: `ValueTask IAsyncDisposable()` interface** <br />
Implements `IAsyncDisposable` interface the injected service should be Disposed.

## Element info JS (See: [demo app](https://blazorextensions.z6.web.core.windows.net/jsinterop#info-js))
**Element info JS is a set of Extension methods for `ElementReference` objects.**

### Functions
- **`GetClientRectAsync`**: **`Task<DomRect> GetClientRectAsync(this ElementReference elementReference)`** <br />
Returns the given HTML element ClintBoundRect data as `DomRect`.

## Scroll JS (See: [demo app](https://blazorextensions.z6.web.core.windows.net/jsinterop#scroll-js))
**Scroll JS** is a set of **Extension methods** for `ElementReference` objects. 
Also an **injectable `IScrollHandler` service** for non element level functions and callback event handlers.

### `IScrollHandler` Functions
- **`ScrollToElementAsync`**: **`Task ScrollToElementAsync(ElementReference elementReference)`**<br />
Scrolls the given element into the page view area.
- **`ScrollToElementByIdAsync`**: **`Task ScrollToElementByIdAsync(string id)`**<br />
Finds element by Id and scrolls the given element into the page view area.
- **`ScrollToElementByNameAsync`**: **`Task ScrollToElementByNameAsync(string name)`**<br />
Finds element by name and scrolls the given element into the page view area.
- **`ScrollToPageEndAsync`**: **`Task ScrollToPageEndAsync()`**<br />
Scrolls to end of the page (X bottom).
- **`ScrollToPageTopAsync`**: **`Task ScrollToPageTopAsync()`**<br />
Scrolls to top of the page (X top).
- **`ScrollToPageXAsync`**: **`Task ScrollToPageXAsync(double x)`**<br />
Scrolls to X position on the page.
- **`ScrollToPageYAsync`**: **`Task ScrollToPageYAsync(double y)`**<br />
Scrolls to Y position on the page.
- **`GetPageScrollPosAsync`**: **`Task<ScrollEventArgs> GetPageScrollPosAsync()`**<br />
Returns page X,Y scroll position as `ScrollEventArgs`.
- **`RegisterPageScrollAsync`**: **`Task<string> RegisterPageScrollAsync(Func<ScrollEventArgs, Task> scrollCallback)`**<br />
Adds event listener for 'scroll' HTML event for the whole document/window. **Also returns with Event Id event id to unsubscribe from event.**
- **`RemovePageScrollAsync`**: **`Task RemovePageScrollAsync(string eventId)`**<br />
Removes event listener for 'scroll' HTML event for the whole document/window by the given event Id.
- **`DisposeAsync()`: `ValueTask IAsyncDisposable()` interface** <br />
Implements `IAsyncDisposable` interface the injected service should be Disposed.

### `ElementReference` extensions
- **`ScrollToElementAsync`**: **`Task ScrollToElementAsync(this ElementReference elementReference)`**<br />
- **`ScrollToEndAsync`**: **`Task ScrollToEndAsync(this ElementReference elementReference)`**<br />
- **`ScrollToTopAsync`**: **`Task ScrollToTopAsync(this ElementReference elementReference)`**<br />
- **`ScrollToXAsync`**: **`Task ScrollToXAsync(this ElementReference elementReference, double xPos)`**<br />
- **`ScrollToYAsync`**: **`Task ScrollToYAsync(this ElementReference elementReference, double yPos)`**<br />
- **`GetScrollPositionAsync`**: **`Task<double> GetScrollPositionAsync(this ElementReference elementReference)`**<br />
- **`IsElementHiddenAsync`**: **`Task<bool> IsElementHiddenAsync(this ElementReference elementReference)`**<br />
- **`IsElementHiddenBelowAsync`**: **`Task<bool> IsElementHiddenBelowAsync(this ElementReference elementReference)`**<br />
- **`IsElementHiddenAboveAsync`**: **`Task<bool> IsElementHiddenAboveAsync(this ElementReference elementReference)`**<br />
- **`ScrollToElementInParentAsync`**: **`Task ScrollToElementInParentAsync(this ElementReference parent, ElementReference innerElement)`**<br />
- **`ScrollInParentByIdAsync`**: **`Task ScrollInParentByIdAsync(this ElementReference parent, string id)`**<br />
- **`ScrollInParentByClassAsync`**: **`Task ScrollInParentByClassAsync(this ElementReference parent, string className)`**<br />

## Resize JS (See: [demo app](https://blazorextensions.z6.web.core.windows.net/jsinterop#resize-js))
**Resize JS** is an **injectable `IResizeHandler` service** for Window (global) and HTML Elements resize event callback handlers.

### Functions
- **`GetPageSizeAsync`**: **`Task<PageSize> GetPageSizeAsync()`**<br />
Returns Browser Window size (height and width in Pixel). It is useful to call when page loaded, then use `RegisterPageResizeAsync` to get notifications 
on each page resize.
- **`RegisterPageResizeAsync`**: **`Task<string> RegisterPageResizeAsync(Func<ResizeEventArgs, Task> resizeCallback)`**<br />
Adds event listener for 'resize' HTML event for the whole document/window.
- **`RemovePageResizeAsync`**: **`Task RemovePageResizeAsync(string eventId)`**<br />
Removes event listener for 'resize' HTML event for the whole document/window by the given event Id.
- **`RegisterResizeAsync`**: **`Task RegisterResizeAsync(ElementReference elementRef, Func<ResizeEventArgs, Task> resizeCallback = null)`**<br />
Adds event listener for 'resize' HTML event for the given element with property filter.
- **`RemoveResizeAsync`**: **`Task RemoveResizeAsync(ElementReference elementRef)`**<br />
Removes event listener for 'resize' HTML event for the given element.
- **`DisposeAsync()`: `ValueTask IAsyncDisposable()` interface** <br />
Implements `IAsyncDisposable` interface the injected service should be Disposed.

## Clipboard JS (See: [demo app](https://blazorextensions.z6.web.core.windows.net/jsinterop#clipboard-js))
Injectable `IClipboardHandler` service to handle JS 'copy' to clipboard Interop for accessing computer Clipboard from Blazor Application.

### `IClipboardHandler` Functions
- **`CopyElementTextToClipboardAsync`**: **`Task<bool> CopyElementTextToClipboardAsync(ElementReference elementReference)`** <br />
Copies the given element text content to clipboard.
- **`CopyTextToClipboardAsync`**: **`Task<bool> CopyTextToClipboardAsync(string text)`** <br />
Copies the given text content to clipboard.
- **`DisposeAsync()`: `ValueTask IAsyncDisposable()` interface** <br />
Implements `IAsyncDisposable` interface the injected service should be Disposed.

### `ElementReference` extensions
- **`CopyElementTextToClipboardAsync`**: **`Task<bool> CopyElementTextToClipboardAsync(this ElementReference elementReference)`** <br />
Copies the given element text content to clipboard.

## Browser Language JS (See: [demo app](https://blazorextensions.z6.web.core.windows.net/jsinterop#lang-js))
**Browser Language JS** is an injectable `ILanguageService` service for detect the **browser language preference**.

### Functions
- **`GetBrowserLanguageAsync`**: **`Task<CultureInfo> GetBrowserLanguageAsync()`** <br />
Returns the given user's Browser language preference as .NET `CultureInfo`.
- **`DisposeAsync`: `ValueTask IAsyncDisposable()` interface** <br />
Implements `IAsyncDisposable` interface the injected service should be Disposed.


## Geolocation JS (See: [demo app](https://blazorextensions.z6.web.core.windows.net/jsinterop#geo-js))
**Geolocation JS** is an injectable `IGeolocationService` service for **detect the device Geolocation (GPS position, speed, heading, etc.)**. 
It is using the Geolocation API which allows users to provide their location to web applications if they desire.

**NOTE:** Geolocation only accurate for devices with GPS, e.g. smartphones.
**In most cases users have to enable it and grant permission to access location data!**
Also some properties of the response might be not available like `Speed`, `Heading` because of required hardwares: GPS, compass, etc.

### Functions
- **`GetCurrentPosition`: `Task GetCurrentPosition(Func<GeolocationResult, Task> locationResultCallback, bool highAccuracy = false, TimeSpan? timeout = null, TimeSpan? cacheTime = null)`** <br />
Get the current position of the device.
- **`AddGeolocationWatcher`: `Task<int> AddGeolocationWatcher(Func<GeolocationResult, Task> locationEventsCallback, bool highAccuracy = false, TimeSpan? timeout = null, TimeSpan? cacheTime = null)`** <br />
Register a handler function that will be **called automatically each time the position of the device changes**.
- **`RemoveGeolocationWatcher`: `Task RemoveGeolocationWatcher(int handlerId)`** <br />
Unregister location/error monitoring handlers previously installed using `AddGeolocationWatcher`.
- **`DisposeAsync`: `ValueTask IAsyncDisposable()` interface** <br />
Implements `IAsyncDisposable` interface the injected service should be Disposed.


# Configuration

## Installation
**Majorsoft.Blazor.Components.Common.JsInterop** is available on [NuGet](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Common.JsInterop/). 

```sh
dotnet add package Majorsoft.Blazor.Components.Common.JsInterop
```
Use the `--version` option to specify a [preview version](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Common.JsInterop/absoluteLatest) to install.

## Usage

Add using statement to your Blazor <component/page>.razor file. Or globally reference it into `_Imports.razor` file.
```
@using Majorsoft.Blazor.Components.Common.JsInterop
@*Only if you want to use Scroll*@
@using Majorsoft.Blazor.Components.Common.JsInterop.Scroll
@*Only if you want to use Focus*@
@using Majorsoft.Blazor.Components.Common.JsInterop.Focus
@*Only if you want to use Click*@
@using Majorsoft.Blazor.Components.Common.JsInterop.Click
@*Only if you want to use Resize*@
@using Majorsoft.Blazor.Components.Common.JsInterop.Resize
@*Only if you want to use ElementInfo*@
@using Majorsoft.Blazor.Components.Common.JsInterop.ElementInfo
@*Only if you want to use Clipboard*@
@using Majorsoft.Blazor.Components.Common.JsInterop.Clipboard
@*Only if you want to use Language*@
@using Majorsoft.Blazor.Components.Common.JsInterop.Language
@*Only if you want to use Geolocation*@
@using Majorsoft.Blazor.Components.Common.JsInterop.Geo
```


**In case of WebAssembly project register CSS events services in your `Program.cs` file:**
```
using Majorsoft.Blazor.Components.CssEvents;
...
public static async Task Main(string[] args)
{
	var builder = WebAssemblyHostBuilder.CreateDefault(args);

	builder.Services.AddJsInteropExtensions();
}
```

**In case of Server hosted project register CSS events services in your `Startup.cs` file:**
```
using Majorsoft.Blazor.Components.CssEvents;
...

public void ConfigureServices(IServiceCollection services)
{
	services.AddJsInteropExtensions();
}
```

### Examples
**Majorsoft.Blazor.Components.Common.JsInterop** package is a very big set of useful small JS Interop functionalities. Which generates a huge sample code base.
For code examples [see usage](https://github.com/majorimi/blazor-components/blob/master/src/Blazor.Components.TestApps.Common/Components/JSInterop.razor).
Also you can understand the behavior by trying out the [demo app](https://blazorextensions.z6.web.core.windows.net/jsinterop).