
Blazor Toggle Components
============
[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-build-check)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=6)
[![Package Version](https://img.shields.io/nuget/v/Majorsoft.Blazor.Components.Toggle?label=Latest%20Version)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Toggle/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Majorsoft.Blazor.Components.Toggle?label=Downloads)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Toggle/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/majorimi/blazor-components/blob/master/LICENSE)

# About

Blazor component that renders customizable Toggle Switch and Toggle Button components. 
**All components work with WebAssembly and Server hosted models**. 
For code examples [see usage](https://github.com/majorimi/blazor-components/blob/master/src/Blazor.Components.TestApps.Common/Components/Toggle.razor).

You can try it out by using the [demo app](https://blazorextensions.z6.web.core.windows.net/Toggle).

![Toggle demo](https://github.com/majorimi/blazor-components/raw/master/.github/docs/gifs/Toggle.gif)

# Components

- **`ToggleSwitch`**: Renders **HTML `<input>` styled as Toggle switch** with customizable size and color, etc.
- **`ToggleButton`**: 
- **`ToggleButtonGroup`**: 

## `ToggleSwitch` component
Blazor component to represent a `boolean` value as an ON/OFF toggle switch.

### Properties
- **`Checked`: `bool { get; set; }` (default: true) - Required** <br />
Represents Toggle switch value: **ON**: `true`, **OFF**: `false`.
- **`OnColor`: `string { get; set; }` (default: "blue") - Required** <br />
Sets the `style` of the HTML `<input>` `background-color` when switch is ON (bool value `true`). Use HTML specified: **Color Names**, **RGB**, **HEX** or with **HSL** values.
- **`OffColor`: `string { get; set; }` (default: "darkgray") - Required** <br />
Sets the `style` of the HTML `<input>` `background-color` when switch is OFF (bool value `false`). Use HTML specified: **Color Names**, **RGB**, **HEX** or with **HSL** values.
- **`Height`: `int { get; set; }` (default: 30) - Required** <br />
HTML `<input>` element height in `px`.
- **`Width`: `int { get; set; }` (default: 80) - Required** <br />
HTML `<input>` element width in `px`.
- **`Disabled`: `bool { get; set; }` (default: false)** <br />
Determines whether the rendered HTML `<input>` element should be disabled or not.
- **`HandleStyle`: `ToggleSwitchStyle { get; set; }` (default: ToggleSwitchStyle.Circle)** <br />
Renders Toggle switch handle with different shaped styles. `{ Ellipse, Circle, Square }`
- **`InnerElementReference`: `ElementReference { get; }`** <br />
Exposes a Blazor `ElementReference` of the wrapped around HTML element. It can be used e.g. for JS interop, etc.

**Arbitrary HTML attributes e.g.: `tabindex="1"` will be passed to the corresponding rendered HTML element `<input>`**.

### Events
- **`OnToggleChanged`: `EventCallback<bool>` delegate** <br />
Callback function called when component toggled. Actual toggle `Value` is the callback `bool` parameter. 

## `ToggleButton` component

### Properties
- **`Checked`: `bool { get; set; }` (default: true) - Required** <br />
Represents Toggle button value: **ON**: `true`, **OFF**: `false`.
- **`OnColor`: `string { get; set; }` (default: "white") - Required** <br />
Sets the `style` of the HTML `<button>` `background-color` when button is ON (bool value `true`). Use HTML specified: **Color Names**, **RGB**, **HEX** or with **HSL** values.
- **`OffColor`: `string { get; set; }` (default: "lightgray") - Required** <br />
Sets the `style` of the HTML `<button>` `background-color` when button is OFF (bool value `false`). Use HTML specified: **Color Names**, **RGB**, **HEX** or with **HSL** values.
- **`HoverColor`: `string { get; set; }` (default: "whitesmoke") - Required** <br />
Sets the `style` of the HTML `<button>` `background-color` when button is hovered over with mouse. Use HTML specified: **Color Names**, **RGB**, **HEX** or with **HSL** values.
- **`Content`: `RenderFragment` HTML content - Required**
Required HTML content to show in Toggle button. It can be any text or image (please use transparent background image).
- **`Height`: `int { get; set; }` (default: 30) - Required** <br />
HTML `<button>` element height in `px`.
- **`Width`: `int { get; set; }` (default: 80) - Required** <br />
HTML `<button>` element width in `px`.
- **`Disabled`: `bool { get; set; }` (default: false)** <br />
Determines whether the rendered HTML `<button>` element should be disabled or not.
- **`InnerElementReference`: `ElementReference { get; }`** <br />
Exposes a Blazor `ElementReference` of the wrapped around HTML element. It can be used e.g. for JS interop, etc.

### Events
- **`OnToggleChanged`: `EventCallback<bool>` delegate** <br />
Callback function called when component toggled. Actual toggle `Value` is the callback `bool` parameter. 

**Arbitrary HTML attributes e.g.: `tabindex="1"` will be passed to the corresponding rendered HTML element `<input>`**.

## `ToggleButtonGroup` component

### Properties


**Arbitrary HTML attributes e.g.: `tabindex="1"` will be passed to the corresponding rendered HTML element `<input>`**.

# Configuration

## Installation

Blazor.Components.Toggle is available on [NuGet](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Toggle/). 

```sh
dotnet add package Majorsoft.Blazor.Components.Toggle
```
Use the `--version` option to specify a [preview version](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Toggle/absoluteLatest) to install.

## Usage

Add using statement to your Blazor `<component/page>.razor` file. Or globally reference it into `_Imports.razor` file.
```
@using Blazor.Components.Toggle
```

### `ToggleSwitch` usage

Following code example shows how to use **`ToggleSwitch`** component in your Blazor App. 

```
<ToggleSwitch
	@ref="_toggleSwitch"
	Value="_value"
	OnColor="@_onColor"
	OffColor="@_offColor"
	Width="_widht"
	Height="_height"
	HandleStyle="_styleType"
	Disabled="_disabled"
	OnValueChanged="OnToggleSwitched">
</ToggleSwitch>

@code {
	private ToggleSwitch _toggleSwitch;
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			await _toggleSwitch.InnerElementReference.FocusAsync();
		}
	}

	private string _onColor = "DodgerBlue";
	private string _offColor = "DarkGray";
	private int _widht = 50;
	private int _height = 22;
	private bool _value = true;
	private bool _disabled = false;
	private ToggleSwitchStyle _styleType = ToggleSwitchStyle.Circle;

	private ElementReference log1;
	private string _swithch1Log;

	private async Task OnToggleSwitched(bool val)
	{
		_value = val;
		_swithch1Log +=  $"Toggle Switched event current value: {val}";
	}
}
```

### `ToggleButton` usage

Following code example shows how to use **`ToggleButton`** component in your Blazor App. 

```


```