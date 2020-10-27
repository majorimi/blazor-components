Blazor Js Interop components and extensions
============
[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-build-check)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=6)
[![Package Version](https://img.shields.io/nuget/v/Majorsoft.Blazor.Components.JsInterop?label=Latest%20Version)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.JsInterop/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Majorsoft.Blazor.Components.JsInterop?label=Downloads)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.JsInterop/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/majorimi/blazor-components/blob/master/LICENSE)

# About

Collection of Blazor components and extension methods that provide useful functionality which can be achieved only with Js Interop.
**All components work with WebAssembly and Server hosted models**. 
For code examples [see usage](https://github.com/majorimi/blazor-components/blob/master/src/Blazor.Components.TestApps.Common/Components/JSInterop.razor).

You can try it out by using the [demo app](https://blazorextensions.z6.web.core.windows.net/jsinterop).

# Features

- **Click JS**: is a component which wraps the given content to a DIV and subscribes to all click events: `OnOutsideClick`, `OnInsideClick`. 
 Also an **injectable `IClickBoundariesHandler` service** for callback event handlers.
- **Focus JS**: is an injectable `IFocusHandler` service. **Focus JS is able to identify and restore focus on ANY DOM element without using Blazor `@ref=""` tag.**
- **Element info JS**: is a set of **Extension methods** for `ElementReference` objects.
- **Scroll JS**: is a set of **Extension methods** for `ElementReference` objects. Also an **injectable `IScrollHandler` service** for non element level functions and callback event handlers.