Blazor Browser Storage Extensions
============
[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-build-check)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=6)
[![Package Version](https://img.shields.io/nuget/v/Majorsoft.Blazor.Extensions.BrowserStorage?label=Latest%20Version)](https://www.nuget.org/packages/Majorsoft.Blazor.Extensions.BrowserStorage/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Majorsoft.Blazor.Extensions.BrowserStorage?label=Downloads)](https://www.nuget.org/packages/Majorsoft.Blazor.Extensions.BrowserStorage/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/majorimi/blazor-components/blob/master/LICENSE)

# About

Blazor extension which Enables Browser Storage (Local/Session/Cookies) access for Blazor applications.
**All components work with WebAssembly and Server hosted models**. 
For code examples [see usage](https://github.com/majorimi/blazor-components/blob/master/src/Majorsoft.Blazor.Components.TestApps.Common/Components/BrowserStorage.razor).

You can try it out by using the [demo app](https://blazorextensions.z6.web.core.windows.net/browserstorage).

# Extensions

- **`LoadingPage`**: Renders an  Overlay `<div>` layer with customizable background color and content for showing Page loading...
- **`LoadingElement`**: Renders an Overlay `<div>` layer for the wrapped element (Table, Grid, etc.) with customizable content for showing loading...
- **`LoadingButton`**: Renders a HTML `<button>` with customizable Content and LoadingContent for showing during async operation in progress/loading...

## `LoadingPage` component (See [demo app](https://blazorextensions.z6.web.core.windows.net/loading#loading-page))
Renders an  Overlay `<div>` layer with customizable background color and content for showing Page loading...
It is useful when you want to show some content with full page overlay meanwhile your page is loading (waiting API response, etc.).

### Functions
- **`Set()`: `void Set()`** <br />
Sets the component to Loading state. Shows overlay `<div>` with specified content.
- **`Reset()`: `void Reset()`** <br />
Resets the component to the original state. Hides overlay `<div>`.

## `LoadingElement` component (See [demo app](https://blazorextensions.z6.web.core.windows.net/loading#loading-element))
Renders an Overlay `<div>` layer for the wrapped element (Table, Grid, etc.) with customizable content for showing loading...
It is useful when you want to show some content with overlay on a Table or Grid, etc. meanwhile data is being fetched from server.

### Functions


## `LoadingButton` component (See [demo app](https://blazorextensions.z6.web.core.windows.net/loading#loading-button))
Renders a HTML `<button>` with customizable Content and LoadingContent for showing during async operation in progress/loading...

### Functions
- **`Set()`: `void Set()`** <br />
Sets the component to Loading state. Shows the specified `LoadingContent` and disables button, if configured to.
- **`Reset()`: `void Reset()`** <br />
Resets the component to the original state. Shows the specified default `Content` and enables button.

# Configuration

## Installation

**Majorsoft.Blazor.Extensions.BrowserStorage** is available on [NuGet](https://www.nuget.org/packages/Majorsoft.Blazor.Extensions.BrowserStorage/). 

```sh
dotnet add package Majorsoft.Blazor.Extensions.BrowserStorage
```
Use the `--version` option to specify a [preview version](https://www.nuget.org/packages/Majorsoft.Blazor.Extensions.BrowserStorage/absoluteLatest) to install.

## Usage

Add using statement to your Blazor `<component/page>.razor` file. Or globally reference it into `_Imports.razor` file.
```
@using Majorsoft.Blazor.Extensions.BrowserStorage
```

### `LoadingPage` usage


```

```

### `LoadingElement` usage

```

```

### `LoadingButton` usage

```


```
