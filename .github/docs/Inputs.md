Blazor Components Inputs controls
============
[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-build-check)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=6)
[![Package Version](https://img.shields.io/nuget/v/Majorsoft.Blazor.Components.Inputs?label=Latest%20Version)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Inputs/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Majorsoft.Blazor.Components.Inputs?label=Downloads)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Inputs/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/majorimi/blazor-components/blob/master/LICENSE)

# About

Blazor components that renders an HTML `<input>`, `<textarea>` elements also extending `InputText` and `InputTextArea` Blazor provided components with `maxlength` set and counter to show remaining characters.
**All components work with WebAssembly and Server hosted models**. 
For code examples [see usage](https://github.com/majorimi/blazor-components/blob/master/src/Majorsoft.Blazor.Components.TestApps.Common/Components/MaxLengthInputPage.razor).

You can try it out by using the [demo app](https://blazorextensions.z6.web.core.windows.net/maxLengthInput).

![Inputs demo](https://github.com/majorimi/blazor-components-docs/raw/main/github/docs/gifs/maxLengthInput.gif)

# Components

- **`MaxLengthInput`**: wraps and renders HTML `<input>` field and sets `maxlength` property with notification onChange.
- **`MaxLengthInputText`**: extends `InputText` Blazor provided component (it supports form validation and `@bind-Value=`) and sets `maxlength` property with notification onChange.
- **`MaxLengthTextarea`**: wraps and renders HTML `<textarea>` field and sets `maxlength` property with notification onChange.
- **`MaxLengthInputTextArea`**: extends `InputTextArea` Blazor provided component (it supports form validation and `@bind-Value=`) and sets `maxlength` property with notification onChange.

## `MaxLengthInput` and `MaxLengthInputText` components

Blazor components that are wrapping around standard  HTML `<input>`, `<textarea>` elements and sets `maxlength` property with notification onChange.
Can fit for any Blazor App e.g. when DB field has limited length and on UI same limited chars should be set. And on each chars entered the remaining allowed chars counter will be shown.

## `MaxLengthTextarea` and `MaxLengthInputTextArea` components

Blazor components that are wrapping around standard exiting Blazor components. Rendered HTML result will be standard `<input>`, `<textarea>` as well.
Can fit for any Blazor App but use this only when you need Blazor provided FROM validation as well.

### Properties

All components have exactly the same features only rendering different HTML elements, hence 
they have the same **properties**, **events** and **functions** as well.

- **`Value`: `string? { get; set; }`** <br />
  Value of the rendered HTML element. Initial field value can be set to given string or omitted (leave empty). 
  Also control actual value can be read out.
  <br />**\* Note**: in case of `DebounceInputText` and `DebounceInputTextArea` this property is inherited from Blazor
  component and requires slightly different code with `@bind`, see usage example differences.
- **`MaxAllowedChars`: `int { get; set; }` (default: 50)** <br />
  Maximum allowed characters to type in.
- **`CountdownText`: `string { get; set; }` (default: "Remaining characters: ")** <br />
 Countdown label text to change or localize message.
- **`ShowRemainingChars`: `bool { get; set; }` (default: true)** <br />
Should show remaining character values at the end of the `CountdownText` or not.
**Note**: RemainingCharacters value can be acquired from `OnRemainingCharsChanged` event parameter.
- **`CountdownTextClass`: `string { get; set; }` (default: "")** <br />
  Countdown label and value CSS class property to style message.
- **`InnerElementReference`: `ElementReference { get; }`** <br />
  Exposes a Blazor `ElementReference` of the wrapped around HTML element. It can be used e.g. for JS interop, etc.

**Arbitrary HTML attributes e.g.: `id="input1" class="form-control w-25"` will be passed to the corresponding rendered HTML element `<input>` or `<textarea>`**.

```
<MaxLengthInput 
	id="in1"
	class="form-control w-25" 
	placeholder="@($"You can type maximum: {_maxLengthInputTextAreaAllowed} char(s)")"
	... />
```

**Will be rendered to:**
```
<input id="in1" class="form-control w-25" placeholder="You can type maximum: 20 char(s)" ... />
```

### Events
- **`OnValueChanged`: `EventCallback<string>` delegate** <br />
  Callback function called when value was changed.
- **`OnRemainingCharsChanged`: `EventCallback<int>` delegate** <br />
  Callback function called when HTML control received keyboard inputs remaining allowed chars calculated and sent as even args.
  Can be used to style or change the Countdown message.

# Configuration

## Installation

**Majorsoft.Blazor.Components.Inputs** is available on [NuGet](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Inputs/). 

```sh
dotnet add package Majorsoft.Blazor.Components.Inputs
```
Use the `--version` option to specify a [preview version](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Inputs/absoluteLatest) to install.

## Usage

Add using statement to your Blazor <component/page>.razor file. Or globally reference it into `_Imports.razor` file.
```
@using Majorsoft.Blazor.Components.Inputs
```

### `MaxLengthInput` and `MaxLengthTextarea` usage

Following code example shows how to use **`MaxLengthInput`** component in your Blazor App with model binding
on specific `OnInput` event. So it will enable two way binding between components. You can omit it and use `Value` directly for one way binding.

**Note**: using **`MaxLengthTextarea`** component basically the same but it will render HTML `<textarea>`.

```
<style>
	.countDownText {
		font-size: small;
		color: gray;
	}
	.countDownText.red {
		color: red;
	}
	.countDownText.right {
		width: 100%;
		text-align: right;
	}
</style>

<div class="row pb-2">
	<div class="col-12 col-lg-8 col-xl-5">
		<MaxLengthInput id="maxInput1" class="form-control w-100" placeholder="@($"You can type maximum: {_maxLengthInputAllowed} char(s)")"
						@bind-Value="@_maxLengthInputValue" @bind-Value:event="OnInput"
						MaxAllowedChars="@_maxLengthInputAllowed"
						CountdownTextClass="@_maxLengthInputTextClass"
						OnRemainingCharsChanged="MaxLengthInputRemainingCharsChanged" />
	</div>
</div>

<div>Actual value: @_maxLengthInputValue</div>

@code {
	//MaxLengthInput
	private int _maxLengthInputAllowed = 10;
	private string _maxLengthInputValue = "";
	private string _maxLengthInputTextClass = "countDownText";

	private void MaxLengthInputRemainingCharsChanged(int remainingChars)
	{
		_maxLengthInputTextClass = "countDownText";

		if (_maxLengthInputAllowed * 0.2 >= remainingChars)
		{
			_maxLengthInputTextClass = "countDownText red";
		}
	}
}	
```

### `MaxLengthInputText` and `MaxLengthInputTextArea` usage

Following code example shows how to use **`DebounceInputText`** component with model binding and form validation in your Blazor App.

**Note**: using **`MaxLengthInputTextArea`** component basically the same but it will render HTML `<textarea>`.

```
<style>
	.countDownText {
		font-size: small;
		color: gray;
	}
	.countDownText.red {
		color: red;
	}
	.countDownText.right {
		width: 100%;
		text-align: right;
	}
</style>

<EditForm Model="@exampleModel">
	<DataAnnotationsValidator />
	<ValidationSummary />
	<div class="row pb-2">
		<div class="col-12 col-lg-8 col-xl-5">
			<MaxLengthInputText id="maxInput2" class="form-control w-100" placeholder="@($"You can type maximum: {_maxLengthInputTextAllowed} char(s)")"
								@bind-Value="exampleModel.Name"
								MaxAllowedChars="@_maxLengthInputTextAllowed"
								CountdownTextClass="@_maxLengthInputTextTextClass"
								OnRemainingCharsChanged="MaxLengthInputTextRemainingCharsChanged" />
		</div>
	</div>
	<div class="pb-2">
		<button class="btn btn-primary" type="submit">Submit</button>
	</div>
</EditForm>
		
<div>Actual value: @(exampleModel.Name)</div>

@code {
	//MaxLengthInputText
	private int _maxLengthInputTextAllowed = 10;
	private string _maxLengthInputTextTextClass = "countDownText";

	private void MaxLengthInputTextRemainingCharsChanged(int remainingChars)
	{
		_maxLengthInputTextTextClass = "countDownText";

		if (_maxLengthInputTextAllowed * 0.2 >= remainingChars)
		{
			_maxLengthInputTextTextClass = "countDownText red";
		}
	}

	//Form model
	private ExampleModel exampleModel = new ExampleModel() { Name = "" };
	public class ExampleModel
	{
		[Required]
		[StringLength(10, ErrorMessage = "Name is too long.")]
		public string Name { get; set; }
	}
}
```