<img align="left" width="110" height="110" src="https://github.com/majorimi/blazor-components/blob/master/.github/Images/blazor.components.png" />

Blazor Components
============

[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-build-check)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=6)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/majorimi/blazor-components/blob/master/LICENSE)
![GitHub last commit](https://img.shields.io/github/last-commit/majorimi/blazor-components)
![GitHub Release Date](https://img.shields.io/github/release-date/majorimi/blazor-components)
![GitHub Repo stars](https://img.shields.io/github/stars/majorimi/blazor-components)
[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-Nuget?branchName=master)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=7&branchName=master)

Majorsoft Blazor Components is a set of UI Components and other useful Extensions for [Blazor](https://blazor.net) applications.
All components are available on [NuGet](https://www.nuget.org/profiles/Blazor.Components). 

## About the project
**Majorsoft Blazor Components** is one of the newest but the most modern Blazor library. The main goal of this project is to provide an easy to use, feature reach full set of components with other 
useful extensions. Which can boost Blazor app developments by:
- Providing reusable components which are the main building blocks of Blazor.
- Hiding CSS details but allowing component customizations as well (no dependency on CSS libraries).
- Hiding JS implementations but exposing many reusable JS functionality and events via new C# APIs (no dependency on JS libraries).
- All running on the fastest ever .NET framework: **.NET 5**. Fully leveraging CSS and JS isolation, JS object reference and modul exports, etc.
- Modular project each package has "single responsibility" install only what you need, reduce download size.
- As simple as possible setup (custom JS referencing not required) all documented with usage examples and demo app.
- All components work on Blazor Server and Clint side.
- Components are extensible, provided extensions and services can be used in other components.
- All components and extensions written in C# (with some required JS) and unit tested with [bUnit](https://github.com/egil/bUnit).

## Prerequisites
- .NET 5
- Visual Studio 2019.

## Majorsoft Blazor Components and Extensions

Detailed descriptions and usage code samples are available on separated docs files. 
Please follow the link provided on each bullet points. Also you can try out all components and extensions by launching the [demo app](https://blazorextensions.z6.web.core.windows.net/).

Check out our planned components and extensions on the project [Wiki page](https://github.com/majorimi/blazor-components/wiki).

### Blazor Extensions

Blazor Extensions are providing useful features to develop Balazor applications:

- **Blazor.Server.Logging.Console**: Enables [browser conole logging](https://github.com/majorimi/blazor-components/blob/master/.github/docs/ServerHostedLogging.md) for Blazor applications using **Server Hosted model**.
- **Blazor.WebAssembly.Logging.Console**: Enables [browser conole logging](https://github.com/majorimi/blazor-components/blob/master/.github/docs/WebAssemblyHostedLogging.md) for Blazor applications using **WebAssembly Hosting model**.

### Blazor Components

Blazor Components are providing custom UI components to develop Balazor applications:

- **Blazor.Components.JsInterop**: [Js Interop components and extensions](https://github.com/majorimi/blazor-components/blob/master/.github/docs/JsInterop.md) that provide useful functionality which can be achieved only with Js Interop.
- **Blazor.Components.Debounce**: [Debounce component](https://github.com/majorimi/blazor-components/blob/master/.github/docs/DebounceInputs.md) that renders an Input, Textarea or other element with debounced onChange.
- **Blazor.Components.Typeahead**: [Typeahead component](https://github.com/majorimi/blazor-components/blob/master/.github/docs/Typeahead.md) that renders an HTML Input or InputText with Typeahead panel.
- **Blazor.Components.Loading**: [Loading components](https://github.com/majorimi/blazor-components/blob/master/.github/docs/Loading.md) that renders Overlay for page load. HTML Button with customizable content for showing async operation in progress/loaing...
- **Blazor.Components.Timer**: [Timer component](https://github.com/majorimi/blazor-components/blob/master/.github/docs/Timer.md) that can be used for scheduled and periodically repeated tasks to call custom code.
- **Blazor.Components.CssEvents**: [CSS Transition and Animation events](https://github.com/majorimi/blazor-components/blob/master/.github/docs/CssEvents.md) Extensions and Components wrapper to notify on CSS Transition and Animation events.
- **Blazor.Components.Modal**: [Modal dialog component](https://github.com/majorimi/blazor-components/blob/master/.github/docs/Modal.md) that can be used to render Modal dialog window with customizable content and parameterized Overlay, etc.
- **Blazor.Components.PermaLink**: [PermaLink component and extension](https://github.com/majorimi/blazor-components/blob/master/.github/docs/PermaLink.md) that can be used to create navigation element inside Blazor pages (#permalink).

