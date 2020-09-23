Blazor Components Debounce Input controls
============
[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-build-check)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=6)
[![Package Version](https://img.shields.io/nuget/v/Majorsoft.Blazor.Components.Debounce.Input?label=Latest%20Version)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Debounce.Input/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Majorsoft.Blazor.Components.Debounce.Input?label=Downloads)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Debounce.Input/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/majorimi/blazor-components/blob/master/LICENSE)

# About

Blazor components that are rendering HTML `<input>`, `<textarea>` elements also extending `InputText` and `InputTextArea` Blazor provided components with debounced (delay) event for onChange. **All components work with WebAssembly and Server hosted models**. For code examples [see usage](https://github.com/majorimi/blazor-components/blob/master/src/Blazor.Components.Tests.Common/DebounceInputPage.razor).

You can try it out by using the [demo app](https://blazorextensions.z6.web.core.windows.net/).

# Features

## Components

- **`DebounceInput`**: wraps and renders HTML `<input>` field with debounced (delay) event for onChange.
- **`DebounceInputText`**: extends `InputText` Blazor provided component (it supports form validaion and `@bind-Value=`) and adds debounced value changed event notification.
- **`DebounceTextArea`**: wraps and renders HTML `<textarea>` field with debounced (delay) event for onChange.
- **`DebounceInputTextArea`**: extends `InputTextArea` Blazor provided component (it supports form validaion and `@bind-Value=`) and adds debounced value changed event notification.

## Options

- **`OnValueChanged`: EventCallback\<string\> delegate **\
  Function called when value was changed (debounced) with field value passed into.
- **`CurrentValue`: **
- **`Delay`: ** 
- **`MinLength`: ** 
- **`ForceNotifyByEnter`: ** 
- **`ForceNotifyOnBlur`:  ** 

**Arbitrary HTML attributes e.g.: `id="input1" class="form-control w-25"` will be passed to the corresponding rendered HTML element `<input>` or `<textarea>`**.

# Configuration

## Installation

Blazor.Components.Debounce.Input is available on [NuGet](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Debounce.Input/). 

```sh
dotnet add package Majorsoft.Blazor.Components.Debounce.Input
```
Use the `--version` option to specify a [preview version](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Debounce.Input/absoluteLatest) to install.

## Usage

```
@using Blazor.Components.Debounce.Input;
```
