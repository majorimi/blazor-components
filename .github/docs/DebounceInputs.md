Blazor Components Debounce Input controls
============
[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-build-check)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=6)
[![Package Version](https://img.shields.io/nuget/v/Majorsoft.Blazor.Components.Debounce?label=Latest%20Version)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Debounce/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Majorsoft.Blazor.Components.Debounce?label=Downloads)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Debounce/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/majorimi/blazor-components/blob/master/LICENSE)

# About

Blazor components that are rendering HTML `<input>`, `<textarea>` elements also extending `InputText` and `InputTextArea` Blazor provided components with debounced (delay) event for onChange. **All components work with WebAssembly and Server hosted models**. 
For code examples [see usage](https://github.com/majorimi/blazor-components/blob/master/src/Blazor.Components.TestApps.Common/Components/Debounce.razor).

You can try it out by using the [demo app](https://blazorextensions.z6.web.core.windows.net/debounceinput).

# Components

- **`DebounceInput`**: wraps and renders HTML `<input>` field with debounced (delay) event for onChange.
- **`DebounceInputText`**: extends `InputText` Blazor provided component (it supports form validation and `@bind-Value=`) and adds debounced value changed event notification.
- **`DebounceTextArea`**: wraps and renders HTML `<textarea>` field with debounced (delay) event for onChange.
- **`DebounceInputTextArea`**: extends `InputTextArea` Blazor provided component (it supports form validation and `@bind-Value=`) and adds debounced value changed event notification.

## `DebounceInput` and `DebounceTextArea` components

Blazor components that are wrapping around standard  HTML `<input>`, `<textarea>` elements and provide debounced (delay) notification functionality.
Can fit for any Blazor app e.g. when making async server calls on user input (search) but you don't want to waste resources by sending requests on each key pressed. 

## `DebounceTextArea` and `DebounceInputTextArea` components

Blazor components that are wrapping around standard exiting Blazor components. Rendered HTML result will be standard `<input>`, `<textarea>` as well.
Can fit for any Blazor app but use this only when you need Blazor provided FROM validation as well.

### Properties

All components have exactly the same features only rendering different HTML elements, hence 
they have the same **properties**, **events** and **functions** as well.

- **`Value`: `string? { get; set; }`** <br />
  Value of the rendered HTML element. Initial field value can be set to given string or omitted (leave empty). 
  Also control actual value can be read out (_useful when MinLenght not reached_).
  <br />**\* Note**: in case of `DebounceInputText` and `DebounceInputTextArea` this property is inherited from Blazor
  component and requires slightly different code with `@bind`, see usage example differences.
- **`DebounceTime`: `int { get; set; }` (default: 200)** <br />
  Notification debounce timeout in ms. If set to `0` notifications happens immediately. `-1` disables automatic notification completely. Notification will only happen by pressing `Enter` key or `onblur`, if set.
- **`MinLength`: `int { get; set; }` (default: 0)** <br />
  Minimal length of text to start notify, if value is shorter than MinLength, there will be notifications with empty value `""`.
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

### Events
- **`OnValueChanged`: `EventCallback<string>` delegate - Required** <br />
  Callback function called when value was changed (debounced) with field value passed into.

### Functions

- **`Dispose()`: `@implements IDisposable` interface** <br />
Component implements `IDisposable` interface Blazor framework will call it when parent removed from render tree.

# Configuration

## Installation

Blazor.Components.Debounce is available on [NuGet](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Debounce/). 

```sh
dotnet add package Majorsoft.Blazor.Components.Debounce
```
Use the `--version` option to specify a [preview version](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Debounce/absoluteLatest) to install.

## Usage

Add using statement to your Blazor <component/page>.razor file. Or globally reference it into `_Imports.razor` file.
```
@using Blazor.Components.Debounce
```

### `DebounceInput` and `DebounceTextArea` usage

Following code example shows how to use **`DebounceInput`** component in your Blazor App with model binding
on specific `OnInput` event. So it will enable two way binding between components. You can omit it and use `Value` directly for one way binding.

**Note**: using **`DebounceTextArea`** component basically the same but it will render HTML `<textarea>`.

```
<DebounceInput id="in1" class="form-control w-100" placeholder="@("Please type in at least: " + _minCharsLength + " char(s)")" autocomplete="off"
	@ref="input1"
	@bind-Value="@_debounceInputValue" @bind-Value:event="OnInput"
	DebounceTime="@_debounceMilisec"
	MinLength="@_minCharsLength"
	OnValueChanged="e => { _notifiedInputValue = e; }"
	ForceNotifyByEnter="@_forceNotifyByEnter"
	ForceNotifyOnBlur="@_forceNotifyOnBlur" />

<div>Notified value: @_notifiedInputValue</div>
<div>Actual value: @_debounceInputValue</div>
<input type="button" class="btn btn-primary" value="Read out actual value" @onclick="(_ => { _debounceInputValue = input1.Value; })" />
	
@code {
	//DebounceInput
	private string _debounceInputValue = "";
	private string _notifiedInputValue = "";
	private int _debounceMilisec = 1000;
	private int _minCharsLength = 2;
	private bool _forceNotifyByEnter = true;
	private bool _forceNotifyOnBlur = true;
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
 
 <div class="row pb-2">
	<div class="col-12 col-lg-8 col-xl-5">
		<DebounceInputText class="form-control w-100" placeholder="@("Please type in at least: " + _debounceInputTextMinCharsLength + " char(s)")"
			@ref="inputText1"
			@bind-Value="exampleModel.Name"
			DebounceTime="@_debounceInputTextDebounceMilisec"
			MinLength="@_debounceInputTextMinCharsLength"
			OnValueChanged="e => { _debounceInputTextValue = e; }"
			ForceNotifyByEnter="@_forceNotifyByEnter2"
			ForceNotifyOnBlur="@_forceNotifyOnBlur2" />
	</div>
 </div>
 <div class="pb-2">
	<button class="btn btn-primary" type="submit">Submit</button>
 </div>
</EditForm>

<div>Notified value: @_debounceInputTextValue</div>
<div>Actual value: @(exampleModel.Name)</div>

@code {
	//DebounceInputText
	private string _debounceInputTextValue = "";
	private int _debounceInputTextDebounceMilisec = 1000;
	private int _debounceInputTextMinCharsLength = 2;
	private bool _forceNotifyByEnter2 = true;
	private bool _forceNotifyOnBlur2 = true;
	private DebounceInputText inputText1;
	
	//Form model
	private ExampleModel exampleModel = new ExampleModel();
	public class ExampleModel
	{
		[Required]
		[StringLength(10, ErrorMessage = "Name is too long.")]
		public string Name { get; set; }
	}
}
```
