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
- **Global Mouse JS**: is an **injectable `IGlobalMouseEventHandler` service** for global mouse callback event handlers.
- **Focus JS**: is an injectable `IFocusHandler` service. **Focus JS is able to identify and restore focus on ANY DOM element without using Blazor `@ref=""` tag.**
- **Element info JS**: is a set of **Extension methods** for `ElementReference` objects.
- **Scroll JS**: is a set of **Extension methods** for `ElementReference` objects. Also an **injectable `IScrollHandler` service** for non element level functions and callback event handlers.
- **Clipboard JS**: is an **injectable `IClipboardHandler` service** for accessing computer Clipboard from Blazor Application.

## Click JS (See: [demo app](https://blazorextensions.z6.web.core.windows.net/jsinterop#click-js))

**Injectable `IClickBoundariesHandler` service** to handle JS 'click' events for the whole document. 
**NOTE: Blazor supports `@onclick` event which is equivalent with `OnInsideClick`. 
This component useful when need to detect if click happened outside (anywhere in the document) of the component with `OnOutsideClick`.**

### Functions
- **`RegisterClickBoundariesAsync`**: **`Task RegisterClickBoundariesAsync(ElementReference elementRef, Func<MouseEventArgs, Task> outsideClickCallback = null, Func<MouseEventArgs, Task> insideClickCallback = null)`** <br />
 Adds event listener for 'click' HTML event for the given element with property filter.
- **`RemoveClickBoundariesAsync`**: **`Task RemoveClickBoundariesAsync(ElementReference elementRef)`** <br />
Removes event listener for 'click' HTML event for the given element.
- **`DisposeAsync()`: `ValueTask IAsyncDisposable()` interface** <br />
Component implements `IAsyncDisposable` interface Blazor framework components also can `@implements IAsyncDisposable` where the injected service should be Disposed.

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

## Element info JS (See: [demo app](https://blazorextensions.z6.web.core.windows.net/jsinterop#info-js))
**Element info JS is a set of Extension methods for `ElementReference` objects.**

### Functions
- **`GetClientRectAsync`**: **`Task<DomRect> GetClientRectAsync(this ElementReference elementReference)`** <br />
Returns the given HTML element ClintBoundRect data as `DomRect`.

## Scroll JS (See: [demo app](https://blazorextensions.z6.web.core.windows.net/jsinterop#scroll-js))
**Scroll JS**is a set of **Extension methods** for `ElementReference` objects. 
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


## Clipboard JS (See: [demo app](https://blazorextensions.z6.web.core.windows.net/jsinterop#clipboard-js))
Injectable `IClipboardHandler` service to handle JS 'copy' to clipboard Interop for accessing computer Clipboard from Blazor Application.

### `IClipboardHandler` Functions
- **`CopyElementTextToClipboardAsync`**: **`Task<bool> CopyElementTextToClipboardAsync(ElementReference elementReference)`** <br />
Copies the given element text content to clipboard.
- **`CopyTextToClipboardAsync`**: **`Task<bool> CopyTextToClipboardAsync(string text)`** <br />
Copies the given text content to clipboard.

### `ElementReference` extensions
- **`CopyElementTextToClipboardAsync`**: **`Task<bool> CopyElementTextToClipboardAsync(this ElementReference elementReference)`** <br />
Copies the given element text content to clipboard.

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
@*Only if you want to use Clipboard*@
@using Blazor.Components.Common.JsInterop.Clipboard
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

### Examples
**Majorsoft.Blazor.Components.Common.JsInterop** package is a very big set of useful small JS Interop functionalities. Which generates a huge sample code base.
For code examples [see usage](https://github.com/majorimi/blazor-components/blob/master/src/Blazor.Components.TestApps.Common/Components/JSInterop.razor).
Also you can understand the behavior by trying out the [demo app](https://blazorextensions.z6.web.core.windows.net/jsinterop).