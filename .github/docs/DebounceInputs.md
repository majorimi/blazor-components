Blazor Components Debounce Input controls
============
[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-build-check)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=6)
[![Package Version](https://img.shields.io/nuget/v/Majorsoft.Blazor.Components.Debounce.Input?label=Latest%20Version)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Debounce.Input/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Majorsoft.Blazor.Components.Debounce.Input?label=Downloads)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Debounce.Input/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/majorimi/blazor-components/blob/master/LICENSE)

# About

Blazor components that are rendering HTML `<input>`, `<textarea>` elements also extending `InputText` and `InputTextArea` Blazor provided components with debounced (delay) event for onChange. **All components work with WebAssembly and Server hosted models**. 
For code examples [see usage](https://github.com/majorimi/blazor-components/blob/master/src/Blazor.Components.TestApps.Common/Components/Debounce.razor).

You can try it out by using the [demo app](https://blazorextensions.z6.web.core.windows.net/debounceinput).

# Features

## Components

- **`DebounceInput`**: wraps and renders HTML `<input>` field with debounced (delay) event for onChange.
- **`DebounceInputText`**: extends `InputText` Blazor provided component (it supports form validation and `@bind-Value=`) and adds debounced value changed event notification.
- **`DebounceTextArea`**: wraps and renders HTML `<textarea>` field with debounced (delay) event for onChange.
- **`DebounceInputTextArea`**: extends `InputTextArea` Blazor provided component (it supports form validation and `@bind-Value=`) and adds debounced value changed event notification.

## Properties

- **`OnValueChanged`: `EventCallback<string>` delegate - Required** <br />
  Function called when value was changed (debounced) with field value passed into.
- **`CurrentValue`: `string { get; set; }`** <br />
  Value of the rendered HTML element. Initial value can be set, with `@ref=""` (_useful when MinLenght not reached_) control value can be read out or can be omitted.
- **`DebounceTime`: `int { get; set; }` (default: 200)** <br />
  Notification debounce timeout in ms. If set to `0 or less`, disables automatic notification completely. Notification will only happen by pressing `Enter` key or `onblur`, if set.
- **`MinLength`: `int { get; set; }` (default: 0)** <br />
  Minimal length of text to start notify, if value is shorter than MinLength, there will be notifications with empty value "".
- **`ForceNotifyByEnter`: `bool { get; set; }` (default: true)** <br />
  Notification of current value will be sent immediately by hitting Enter key. Enabled by-default. Notification will obey MinLength rule, if length is less, then empty value "" will be sent back.
- **`ForceNotifyOnBlur`:  `bool { get; set; }` (default: true)** <br />
  Same as `ForceNotifyByEnter` but notification triggered `onblur` event, when focus leaves the input field.

**Arbitrary HTML attributes e.g.: `id="input1" class="form-control w-25"` will be passed to the corresponding rendered HTML element `<input>` or `<textarea>`**.

```
<DebounceInput 
    id="in1"
    class="form-control w-25" 
    OnValueChanged="e => { DebounceInputValue = e; }" 
    placeholder="@("Please type in at least: " + @MinCharsLength + " char(s)")"
    ... />
```

**Will be rendered to:**
```
<input id="in1" class="form-control w-25" placeholder="Please type in at least: 2 char(s)" ... />
```

### Functions

- **`Dispose()`: `@implements IDisposable` interface** <br />
Component implements `IDisposable` interface Blazor framework will call it when parent removed from render tree.

# Configuration

## Installation

Blazor.Components.Debounce.Input is available on [NuGet](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Debounce.Input/). 

```sh
dotnet add package Majorsoft.Blazor.Components.Debounce.Input
```
Use the `--version` option to specify a [preview version](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Debounce.Input/absoluteLatest) to install.

## Usage

Add using statement to your Blazor <component/page>.razor file. Or globally reference it into `_Imports.razor` file.
```
@using Blazor.Components.Debounce.Input
```

### `DebounceInput` and `DebounceTextArea` usage

Following code example shows how to use **`DebounceInput`** component in your Blazor App. 
**Note**: using **`DebounceTextArea`** component basically the same but it will render HTML `<textarea>`.

```
<div class="pb-2">
    <DebounceInput id="in1" class="form-control w-25" placeholder="@("Please type in at least: " + @MinCharsLength + " char(s)")"
        @ref="input1"
        CurrentValue="@DebounceInputValue" 
        DebounceTime="@DebounceMilisec" 
        MinLength="@MinCharsLength"
        OnValueChanged="e => { DebounceInputValue = e; }" 
        ForceNotifyByEnter="@ForceNotifyByEnter"
        ForceNotifyOnBlur ="@ForceNotifyOnBlur" />
</div>

<div>Notified value: @DebounceInputValue</div>
<div>Actual value: @(input1?.CurrentValue ?? DebounceInputValue)</div>
    
@code {
    //DebounceInput
    private string DebounceInputValue = "";
    private int DebounceMilisec = 1000;
    private int MinCharsLength = 2;
    private bool ForceNotifyByEnter = true;
    private bool ForceNotifyOnBlur = true;
    private DebounceInput input1;
}
    
```

### `DebounceInputText` and `DebounceInputTextArea` usage

Following code example shows how to use **`DebounceInputText`** component with model binding and form validation in your Blazor App.
 **Note**: using **`DebounceInputTextArea`** component basically the same but it will render HTML `<textarea>`.

```
<EditForm Model="@exampleModel">
    <DataAnnotationsValidator />
    <ValidationSummary />

    <div class="pb-2">
        <DebounceInputText @bind-Value="exampleModel.Name" class="form-control w-25" placeholder="@("Please type in at least: " + @DebounceInputTextMinCharsLength + " char(s)")"
            @ref="inputText1"
            DebounceTime="@DebounceInputTextDebounceMilisec" 
            MinLength="@DebounceInputTextMinCharsLength"
            OnValueChanged="e => { DebounceInputTextValue = e; }" 
            ForceNotifyByEnter="@ForceNotifyByEnter2"
            ForceNotifyOnBlur ="@ForceNotifyOnBlur2" />
    </div>
    <div class="pb-2">
        <button class="btn btn-primary" type="submit">Submit</button>
    </div>
</EditForm>

<div>Notified value: @DebounceInputTextValue</div>
<div>Actual value: @(exampleModel.Name)</div>

@code {
    //DebounceInputText
    private string DebounceInputTextValue = "";
    private int DebounceInputTextDebounceMilisec = 1000;
    private int DebounceInputTextMinCharsLength = 2;
    private bool ForceNotifyByEnter2 = true;
    private bool ForceNotifyOnBlur2 = true;
    private DebounceInputText inputText1;
    
    //Form model
    private ExampleModel exampleModel = new ExampleModel();
    private ExampleModel exampleModel2 = new ExampleModel();
    public class ExampleModel
    {
        [Required]
        [StringLength(10, ErrorMessage = "Name is too long.")]
        public string Name { get; set; }
    }
}
```
