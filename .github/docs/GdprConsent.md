Blazor GDPR Consent controls
============
[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-build-check)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=6)
[![Package Version](https://img.shields.io/nuget/v/Majorsoft.Blazor.Components.GdprConsent?label=Latest%20Version)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.GdprConsent/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Majorsoft.Blazor.Components.GdprConsent?label=Downloads)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.GdprConsent/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/majorimi/blazor-components/blob/master/LICENSE)

# About

Blazor injectable `IGdprConsentService` service and components that renders a customizable GDPR consent Banner or Popup witch Accept/Reject for cookie settings chosen value is persisted to Browser storage.
To initialize GDPR Consents use `GdprBanner` or `GdprModal` only once in your Blazor App MainLayout.razor page or any common place. 

**All components work with WebAssembly and Server hosted models**. 
For code examples [see usage](https://github.com/majorimi/blazor-components/blob/master/src/Majorsoft.Blazor.Components.TestApps.Common/Components/GdprConsents.razor).

You can try it out by using the [demo app](https://blazorextensions.z6.web.core.windows.net/gdpr).

![GDPR Consent demo](https://github.com/majorimi/blazor-components-docs/raw/main/github/docs/gifs/gdprConsents.gif)

# Components and Services

- **`GdprBanner`**: renders a small Overlay layer at the bottom of the page with customizable content for showing the given GDPR message.
- **`GdprModal`**: renders a Modal dialog with Overlay layer for the whole page with customizable content for showing the given GDPR message.
- **`IGdprConsentService`**: injectable service to handle GDPR Consent actions.
- **`IGdprConsentNotificationService`**: injectable singleton service to handle GDPR Consent changes.

## `GdprBanner` component

Modal dialogs are positioned over everything else in the document with optional Overlay (customizable opacity and color). 
Modal dialogs are removed from DOM when closed. **Only one modal window can be shown at a time.**
Dialog automatically (can be disabled) focuses itself to capture keyboard events when closed will refocus the last element.

### Properties
- **`Header`: `RenderFragment` HTML content** <br />
HTML content to show on the Modal header (top), right to the close button (if visible). Can be any valid HTML but should be only Title text. 
**Must not be defined if you want to leave it out. Also `ShowCloseButton` must be set to `false`**
- **`Content`: `RenderFragment` HTML content - Required** <br />
Required HTML content to show on the Modal dialog. Can be any valid HTML.
- **`Footer`: `RenderFragment` HTML content** <br />
HTML content to show on the Modal footer (bottom). Can be any valid HTML but should be only custom action buttons. 
**Must not be defined if you want to leave it out.**
- **`OverlayBackgroundColor`: `string { get; set; }` (default: "gray")** <br />
  Sets the `style` of the HTML `<div>` `background-color`. Use HTML specified: **Color Names**, **RGB** or with **HEX** values.
- **`OverlayOpacity`: `double { get; set; }` (default: 0.9)** <br />
Opacity of the overlay `<div>`. Value should be **between 0..1**. Where 0 means the overlay layer is not visible.
- **`CloseOnOverlayClick`: `bool { get; set; }` (default: true)** <br />
When `true` Modal dialog will be closed when Overlay (background) clicked. It works even if Overlay not visible (Opacity is set to 0)
- **`CloseOnEscapeKey`: `bool { get; set; }` (default: true)** <br />
When `true` Modal dialog will be closed when **Esc** (Escape) key pressed.
- **`Height`: `double { get; set; }` (default: 0)** <br />
Modal dialog window Height in **px** if set to **0** Height is set **auto**.
- **`Width`: `double { get; set; }` (default: 0)** <br />
Modal dialog window Width in **px** if set to **0** Width is set **auto**.
- **`MinHeight`: `double { get; set; }` (default: 200)** <br />
Modal dialog window minimum Height in **px**.
- **`MinWidth`: `double { get; set; }` (default: 200)** <br />
Modal dialog window minimum Width in **px**.
- **`Focus`: `bool { get; set; }` (default: true)** <br />
When `true` Modal dialog will automatically set focus to itself when it opens, and set it bact to the last focused element when it closes. 
In general this should never be set to false as it makes the Modal less accessible to screen-readers, etc.
- **`Animate`: `bool { get; set; }` (default: true)** <br />
When `true` Modal dialog will appear and disappear by using smooth CSS slide and fade transitions.
- **`Centered`: `bool { get; set; }` (default: false)** <br />
When `true` Modal dialog will be vertically centered, otherwise shown near to the top. Modal dialog horizontally always centered.
- **`ShowCloseButton`: `bool { get; set; }` (default: true)** <br />
 When `true` Modal dialog will show Header (even if Header is not defined) with closed **x** button.
- **`IsOpen`: `bool { get; }`** <br />
 Returns `true` if the Modal dialog is opened, otherwise `false`.

**Arbitrary HTML attributes e.g.: `id="diag1"` will be passed to the corresponding rendered root HTML element Overlay `<div>`**.

### Functions
- **`Open()`: `Task Open()`** <br />
When method called Modal dialog will be opened. It should be `await`-ed.
- **`Close()`: `Task Close()`** <br />
When method called Modal dialog will be closed. It should be `await`-ed.
- **`DisposeAsync()`: `Task DisposeAsync()`** <br />
Component implements `IAsyncDisposable` interface Blazor framework will call it when parent removed from render tree.

## `GdprModal` component

Modal dialogs are positioned over everything else in the document with optional Overlay (customizable opacity and color). 
Modal dialogs are removed from DOM when closed. **Only one modal window can be shown at a time.**
Dialog automatically (can be disabled) focuses itself to capture keyboard events when closed will refocus the last element.

### Properties
- **`Header`: `RenderFragment` HTML content** <br />
HTML content to show on the Modal header (top), right to the close button (if visible). Can be any valid HTML but should be only Title text. 
**Must not be defined if you want to leave it out. Also `ShowCloseButton` must be set to `false`**
- **`Content`: `RenderFragment` HTML content - Required** <br />
Required HTML content to show on the Modal dialog. Can be any valid HTML.
- **`Footer`: `RenderFragment` HTML content** <br />
HTML content to show on the Modal footer (bottom). Can be any valid HTML but should be only custom action buttons. 
**Must not be defined if you want to leave it out.**
- **`OverlayBackgroundColor`: `string { get; set; }` (default: "gray")** <br />
  Sets the `style` of the HTML `<div>` `background-color`. Use HTML specified: **Color Names**, **RGB** or with **HEX** values.
- **`OverlayOpacity`: `double { get; set; }` (default: 0.9)** <br />
Opacity of the overlay `<div>`. Value should be **between 0..1**. Where 0 means the overlay layer is not visible.
- **`CloseOnOverlayClick`: `bool { get; set; }` (default: true)** <br />
When `true` Modal dialog will be closed when Overlay (background) clicked. It works even if Overlay not visible (Opacity is set to 0)
- **`CloseOnEscapeKey`: `bool { get; set; }` (default: true)** <br />
When `true` Modal dialog will be closed when **Esc** (Escape) key pressed.
- **`Height`: `double { get; set; }` (default: 0)** <br />
Modal dialog window Height in **px** if set to **0** Height is set **auto**.
- **`Width`: `double { get; set; }` (default: 0)** <br />
Modal dialog window Width in **px** if set to **0** Width is set **auto**.
- **`MinHeight`: `double { get; set; }` (default: 200)** <br />
Modal dialog window minimum Height in **px**.
- **`MinWidth`: `double { get; set; }` (default: 200)** <br />
Modal dialog window minimum Width in **px**.
- **`Focus`: `bool { get; set; }` (default: true)** <br />
When `true` Modal dialog will automatically set focus to itself when it opens, and set it bact to the last focused element when it closes. 
In general this should never be set to false as it makes the Modal less accessible to screen-readers, etc.
- **`Animate`: `bool { get; set; }` (default: true)** <br />
When `true` Modal dialog will appear and disappear by using smooth CSS slide and fade transitions.
- **`Centered`: `bool { get; set; }` (default: false)** <br />
When `true` Modal dialog will be vertically centered, otherwise shown near to the top. Modal dialog horizontally always centered.
- **`ShowCloseButton`: `bool { get; set; }` (default: true)** <br />
 When `true` Modal dialog will show Header (even if Header is not defined) with closed **x** button.
- **`IsOpen`: `bool { get; }`** <br />
 Returns `true` if the Modal dialog is opened, otherwise `false`.

**Arbitrary HTML attributes e.g.: `id="diag1"` will be passed to the corresponding rendered root HTML element Overlay `<div>`**.

### Functions
- **`Open()`: `Task Open()`** <br />
When method called Modal dialog will be opened. It should be `await`-ed.
- **`Close()`: `Task Close()`** <br />
When method called Modal dialog will be closed. It should be `await`-ed.
- **`DisposeAsync()`: `Task DisposeAsync()`** <br />
Component implements `IAsyncDisposable` interface Blazor framework will call it when parent removed from render tree.


# Configuration

## Installation

**Majorsoft.Blazor.Components.GdprConsent** is available on [NuGet](https://www.nuget.org/packages/Majorsoft.Blazor.Components.GdprConsent). 

```sh
dotnet add package Majorsoft.Blazor.Components.GdprConsent
```
Use the `--version` option to specify a [preview version](https://www.nuget.org/packages/Majorsoft.Blazor.Components.GdprConsent/absoluteLatest) to install.

## Usage

Add using statement to your Blazor `<component/page>.razor` file. Or globally reference it into `_Imports.razor` file.

```
@using Majorsoft.Blazor.Components.Toggle
@using Majorsoft.Blazor.Extensions.BrowserStorage
@using Majorsoft.Blazor.Components.Modal
@using Majorsoft.Blazor.Components.Common.JsInterop.Focus
@using Majorsoft.Blazor.Components.CssEvents
@using Majorsoft.Blazor.Components.GdprConsent
```

### Dependences
**Majorsoft.Blazor.Components.GdprConsent** package depends on other Majorsoft Nuget packages:
- [Majorsoft.Blazor.Components.CssEvents](https://www.nuget.org/packages/Majorsoft.Blazor.Components.CssEvents)
which handles CSS Transition and Animation events for the dialog animation.
- [Majorsoft.Blazor.Components.Common.JsInterop](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Common.JsInterop)
which handles JS Interop for focusing previous elements.
- [Majorsoft.Blazor.Components.CssEvents](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Modal)
which renders Model dialog window for `GdprModal`.
- [Majorsoft.Blazor.Components.CssEvents](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Toggle)
which can be used to customize `` with ON/OFF switches to allow Cookie or not.
- [Majorsoft.Blazor.Components.CssEvents](https://www.nuget.org/packages/)
which stores user provided Consents in the Browser's Local storage.

**In case of WebAssembly project register services in your `Program.cs` file:**
```
using Majorsoft.Blazor.Components.GdprConsent;
...
public static async Task Main(string[] args)
{
	var builder = WebAssemblyHostBuilder.CreateDefault(args);

	//Register dependencies
	builder.Services.AddGdprConsent();
}
```

**In case of Server hosted project register services in your `Startup.cs` file:**
```
using Majorsoft.Blazor.Components.GdprConsent;
...

public void ConfigureServices(IServiceCollection services)
{
	//Register dependencies
	services.AddGdprConsent();
}
```

### `GdprBanner` usage

```

```

### `GdprModal` usage

```

```