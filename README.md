<img align="left" width="140" height="140" src="https://github.com/majorimi/blazor-components/blob/master/.github/Images/blazor.components.png" />

Majorsoft Blazor Components
============

[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-build-check)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=6)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/majorimi/blazor-components/blob/master/LICENSE)
![GitHub last commit](https://img.shields.io/github/last-commit/majorimi/blazor-components)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/majorimi/blazor-components)
![GitHub Release Date](https://img.shields.io/github/release-date/majorimi/blazor-components)
![GitHub Repo stars](https://img.shields.io/github/stars/majorimi/blazor-components)
[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-Nuget?branchName=master)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=7&branchName=master)

Majorsoft Blazor Components is a set of UI Components and other useful Extensions for [Blazor](https://blazor.net) applications.
All components are free and available on [NuGet](https://www.nuget.org/profiles/Blazor.Components). 

You can try out all components and extensions by launching the [demo app](https://blazorextensions.z6.web.core.windows.net/). **Note: this app is hosted with the _Azure Static website_ feature, which uses aggressive caching. You might have to use HARD reload (CTRL + F5 or CMD + SHIFT + R), or clear the browser cache.**

## About the project
**Majorsoft Blazor Components** is one of the newest and most modern Blazor libraries. It uses the fastest ever .NET 8+ Framework leveraging CSS and JS isolation. 
The main goal of this project is to provide an easy to use, feature rich set of customizable components with other 
useful extensions, which can boost Blazor App development by:

- Providing reusable components which are the main building blocks of Blazor.
- Hiding CSS details but allowing component customizations as well (no dependency on CSS libraries).
- Hiding JS implementations but exposing many reusable JS functionalities and events via new C# APIs (no dependency on JS libraries).
- All running on the fastest ever .NET framework: **.NET 8+**. Fully leveraging CSS and JS isolation, JS object references and module exports, etc.
- Modular project: each package has a "single responsibility", install only what you need and reduce download size.
- As simple as possible setup (custom JS referencing is not required), all documented with usage examples and a demo app.
- All components work on Blazor Server and Client side.
- Components are extensible, provided extensions and services can be used in other components.
- All components and extensions written in C# (with some required JS) and unit tested with [bUnit](https://github.com/egil/bUnit).

## Releases
![GitHub release (latest by date)](https://img.shields.io/github/v/release/majorimi/blazor-components)
![GitHub Release Date](https://img.shields.io/github/release-date/majorimi/blazor-components)

:warning: For the full **release history with detailed change description and _breaking change announcements_** please see: [release notes](https://github.com/majorimi/blazor-components/releases).  

## Prerequisites
- .NET 8.0 SDK or later.
- Visual Studio 2026/Visual Studio Code.

## Majorsoft Blazor Components and Extensions

Detailed descriptions and usage code samples are available in separate docs files. 
Please follow the link provided in each bullet point. Also you can try out all components and extensions by launching the [demo app](https://blazorextensions.z6.web.core.windows.net/).

Check out our planned components and extensions on the project [Wiki page](https://github.com/majorimi/blazor-components/wiki). If you would like a component to be prioritized or you have new component ideas please submit them.

### **Majorsoft Blazor Extensions**

**Majorsoft Blazor Extensions are providing useful features to develop Blazor applications:**

- **Majorsoft.Blazor.Server.Logging.Console**: Enables [Browser console logging](https://github.com/majorimi/blazor-components/blob/master/.github/docs/ServerHostedLogging.md) for Blazor applications using **Server Hosted model**.
- **Majorsoft.Blazor.WebAssembly.Logging.Console**: Enables [Browser console logging](https://github.com/majorimi/blazor-components/blob/master/.github/docs/WebAssemblyHostedLogging.md) for Blazor applications using **WebAssembly Hosting model**.
- **Majorsoft.Blazor.Extensions.BrowserStorage**: Enables [Browser Local and Session storages and Cookies store](https://github.com/majorimi/blazor-components/blob/master/.github/docs/BrowserStorage.md) access for Blazor applications.
- **Majorsoft.Blazor.Extensions.Analytics**: Enables [Analytics services usage](https://github.com/majorimi/blazor-components/blob/master/.github/docs/Analytics.md) for Blazor applications e.g. Google Analytics, etc.

### **Majorsoft Blazor Components**

**Majorsoft Blazor Components are providing custom UI components to develop Blazor applications:**

- **Majorsoft.Blazor.Components.Common.JsInterop**: [Js Interop components, injectable services and extensions](https://github.com/majorimi/blazor-components/blob/master/.github/docs/JsInterop.md) that provides useful functionality and event notifications which can be achieved only with JS Interop e.g. scroll, clipboard, focus, resize, language detection, Geolocation, HTML Head (title, meta, SEO), etc.
- **Majorsoft.Blazor.Components.Debounce**: [Debounce components](https://github.com/majorimi/blazor-components/blob/master/.github/docs/DebounceInputs.md) that renders an Input, InputText, Textarea or InputTextarea, etc. element with debounced `onChange` event.
- **Majorsoft.Blazor.Components.Typeahead**: [Typeahead components](https://github.com/majorimi/blazor-components/blob/master/.github/docs/Typeahead.md) that renders an HTML Input or InputText with Typeahead panel.
- **Majorsoft.Blazor.Components.Inputs**: [Inputs components](https://github.com/majorimi/blazor-components/blob/master/.github/docs/Inputs.md) that renders MaxLength Input, InputText, Textarea or InputTextarea, etc. elements with `maxlength` set and counter to show remaining characters. Also provides a `MarkdownEditor` with live HTML preview, a Word-like WYSIWYG `RichTextEditor` (both sharing one Markdown format and a customizable sectioned toolbar), a `PasswordInput` with customizable mask character and reveal (eye) button and a `DateTimePicker` with a culture aware calendar and time editor shown in a Popover.
- **Majorsoft.Blazor.Components.Loading**: [Loading and Overlay components](https://github.com/majorimi/blazor-components/blob/master/.github/docs/Loading.md) that renders Overlays for the whole page on load or for specific element on custom event. Also HTML `button` with customizable content for showing async operation in progress/loading state.
- **Majorsoft.Blazor.Components.Timer**: [Timer component](https://github.com/majorimi/blazor-components/blob/master/.github/docs/Timer.md) that can be used for scheduled and periodically repeated tasks to call custom code.
- **Majorsoft.Blazor.Components.CssEvents**: [CSS Transition and Animation events](https://github.com/majorimi/blazor-components/blob/master/.github/docs/CssEvents.md) injectable Services and wrapper Components to notify on CSS Transition and Animation events.
- **Majorsoft.Blazor.Components.Modal**: [Modal dialog component](https://github.com/majorimi/blazor-components/blob/master/.github/docs/Modal.md) that can be used to render Modal dialog window with customizable content and parameterized Overlay, etc.
- **Majorsoft.Blazor.Components.PermaLink**: [PermaLink component and extension](https://github.com/majorimi/blazor-components/blob/master/.github/docs/PermaLink.md) that can be used to create navigation element inside Blazor pages (#permalink).
- **Majorsoft.Blazor.Components.Toggle**: [Toggle components](https://github.com/majorimi/blazor-components/blob/master/.github/docs/Toggle.md) that can be used to render customizable Toggle switch and Toggle button components.
- **Majorsoft.Blazor.Components.Tabs**: [Tabs components](https://github.com/majorimi/blazor-components/blob/master/.github/docs/Tabs.md) that renders customizable Tabs panel with many tabs and custom content.
- **Majorsoft.Blazor.Components.Collapse**: [Collapse components](https://github.com/majorimi/blazor-components/blob/master/.github/docs/Collapse.md) that renders customizable Collapsible/Expandable panel and Accordion with many but only one active panel also custom content and header.
- **Majorsoft.Blazor.Components.Maps**: [Google/Bing Maps components](https://github.com/majorimi/blazor-components/blob/master/.github/docs/Maps.md) that renders **Google/Bing maps** wrapped into Blazor components allowing to control and manage maps with .NET code.
- **Majorsoft.Blazor.Components.GdprConsent**: [GDPR Consent components](https://github.com/majorimi/blazor-components/blob/master/.github/docs/GdprConsent.md) injectable service and components that renders a customizable GDPR consent Banner or Popup with Accept/Reject for cookie settings, chosen value is persisted to Browser storage.
- **Majorsoft.Blazor.Components.Notifications**: [Notification components](https://github.com/majorimi/blazor-components/blob/master/.github/docs/Notifications.md) injectable INotificationService service to handle HTML5 Notifications and ServiceWorker Notifications and components that renders customizable Alert and Toast notification message elements.
- **Majorsoft.Blazor.Components.Tooltips**: [Tooltip and Popover components](https://github.com/majorimi/blazor-components/blob/master/.github/docs/Tooltip.md) that renders a simple text Tooltip on hover or focus and a customizable, click-triggered Popover panel attached to any element with 4 positions, rich HTML content and show/hide events.
- **Majorsoft.Blazor.Components.DragAndDrop**: [Drag and Drop components](https://github.com/majorimi/blazor-components/blob/master/.github/docs/DragAndDrop.md) `Draggable` and `DropZone` wrapper components exposing the full HTML Drag and Drop API with strongly-typed payload transfer, accept conditions, custom drag image and dropped file access.
- **Majorsoft.Blazor.Components.ColorPicker**: [ColorPicker components](https://github.com/majorimi/blazor-components/blob/master/.github/docs/ColorPicker.md) that renders a fully featured HTML Color Picker: HSV Saturation/Brightness area, Hue and Alpha sliders, editable HEX/RGB/HSL inputs, EyeDropper and predefined palette. Available as an always-visible `ColorPalette` or a compact `ColorPicker` hosted in a Popover.
- **Majorsoft.Blazor.Components.Canvas**: [Canvas and WebGL components](https://github.com/majorimi/blazor-components/blob/master/.github/docs/Canvas.md) that wrap the HTML `canvas` element: `Canvas2D` exposing the Canvas 2D drawing API and `WebGLCanvas` exposing the WebGL/WebGL2 GPU rendering API to .NET code via JS interop, with command batching and render loop support.
- **Majorsoft.Blazor.Components.Media**: [Media components](https://github.com/majorimi/blazor-components/blob/master/.github/docs/Media.md) for capturing and playing media in the browser: `CameraCapture` with live preview for taking photos and recording video, `AudioRecorder` for microphone recording with live input level metering, `MediaFileCapture` for the native camera app on mobile devices, `VideoPlayer` and `AudioPlayer` fully controllable from .NET code, an `AudioVisualizer` rendering an FFT spectrum or waveform onto a `canvas` and an injectable `IMediaDeviceService` to list devices and request permissions.
- **Majorsoft.Blazor.Components.Grid**: [Grid components](https://github.com/majorimi/blazor-components/blob/master/.github/docs/Grid.md) that render a customizable data Grid (HTML table) from any `IEnumerable<TItem>` data source. `DataGrid` supports sorting, paging, column resizing, custom Header and cell templates, empty data template and full styling (striped, bordered, hoverable, custom colors), columns are defined with `DataGridColumn`.
- **Majorsoft.Blazor.Components.WASM.AppLoader**: [WebAssembly App Loader component](https://github.com/majorimi/blazor-components/blob/master/.github/docs/AppLoader.md) that can be used to display a loading indicator (Splash Screen) while the Blazor WebAssembly app is initializing.

## Community
- [Contributing](CONTRIBUTING.md)
- [Report an issue or ask new features](https://github.com/majorimi/blazor-components/issues/new)
- ⭐[Thanks for Star ⭐ giving users](https://github.com/majorimi/blazor-components/stargazers) **You can Give a Star ⭐**
- [Support project](https://github.com/majorimi/blazor-components/funding_links?fragment=1) 🙏👍 🍕☕

## Other info
- [Docs](.github/docs)
- [Wiki page](https://github.com/majorimi/blazor-components/wiki)
- [License](LICENSE)
