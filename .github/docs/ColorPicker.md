Blazor ColorPicker Components
============
[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-build-check)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=6)
[![Package Version](https://img.shields.io/nuget/v/Majorsoft.Blazor.Components.ColorPicker?label=Latest%20Version)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.ColorPicker/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Majorsoft.Blazor.Components.ColorPicker?label=Downloads)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.ColorPicker/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/majorimi/blazor-components/blob/master/LICENSE)

# About

Blazor components that render a fully featured HTML Color Picker control: an HSV Saturation/Brightness selection area, Hue and Alpha (opacity) sliders, editable HEX/RGB/HSL inputs, a known color name, a copy-to-clipboard button, a screen color picker (EyeDropper) and a predefined color palette. It comes in two flavours: an always-visible `ColorPalette` and a compact, click-triggered `ColorPicker` that hosts the palette inside a Popover. 
**All components work with WebAssembly and Server hosted models**. 
For code examples [see usage](https://github.com/majorimi/blazor-components/blob/master/src/Majorsoft.Blazor.Components.TestApps.Common/Components/ColorPickerDemo.razor).

You can try it out by using the [demo app](https://blazorextensions.z6.web.core.windows.net/colorpicker).

## Features

- **HSV Saturation/Brightness** selection area with a draggable handle.
- **Hue slider** and optional **Alpha (opacity) slider** with ARGB output.
- Editable **HEX**, **RGB(A)** and **HSL** inputs, all kept in sync.
- **Known color name** field (e.g. `RebeccaPurple`) for named HTML colors.
- **Copy to clipboard** button for the current HEX value.
- **Screen color picker (EyeDropper)** button &mdash; rendered only when the browser supports the [EyeDropper API](https://developer.mozilla.org/docs/Web/API/EyeDropper_API).
- **Predefined color palette** of swatches, customizable via `PaletteColors`.
- Works with the standard **`System.Drawing.Color`** type and supports **two-way binding** with `@bind-SelectedColor`.
- Fully sizable areas (picker area, info area, slider height).
- `ColorPicker` adds a compact trigger button with a color preview, a Popover host, **Apply** support and open/close events.

# Components

- **`ColorPalette`**: renders the full, always-visible color selector (HSV area, sliders, HEX/RGB/HSL info and palette).
- **`ColorPicker`**: renders a compact trigger button showing the selected color and opens a `ColorPalette` inside a Popover, with optional Apply-to-commit behavior.

Both components work with the **`System.Drawing.Color`** type. Add `@using System.Drawing` and (for the `ToHtmlHex()` / `ToRgbString()` helpers used in the examples) `@using Majorsoft.Blazor.Components.Core.HtmlColors`.

## `ColorPalette` component (See: [demo app](https://blazorextensions.z6.web.core.windows.net/colorpicker#colorPalette))
The always-visible color selector. Pick a color in the HSV area, drag the Hue/Alpha sliders or type a HEX/RGB/HSL value; every change raises `OnColorChanged` and updates the two-way bound `SelectedColor`.

### Properties
- **`SelectedColor`: `Color { get; set; }` (default: Color.FromArgb(66, 135, 245))** <br />
Currently selected `System.Drawing.Color`. Supports two-way binding with `@bind-SelectedColor`.
- **`HueAreaWidth`: `int { get; set; }` (default: 300, min: 200)** <br />
Width of the Saturation/Brightness selection area in pixels.
- **`HueAreaHeight`: `int { get; set; }` (default: 250, min: 100)** <br />
Height of the Saturation/Brightness selection area in pixels.
- **`InfoAreaWidth`: `int { get; set; }` (default: 300, min: 200)** <br />
Width of the info area (Hue/Alpha sliders, color info and predefined palette) in pixels.
- **`HueSliderHeight`: `int { get; set; }` (default: 12)** <br />
Height of the Hue and Alpha sliders in pixels.
- **`ShowInfoArea`: `bool { get; set; }` (default: true)** <br />
Shows or hides the HEX/RGB/HSL info and input area.
- **`ShowColorName`: `bool { get; set; }` (default: true)** <br />
Shows or hides the known color Name field. Requires `ShowInfoArea`.
- **`EnableAlpha`: `bool { get; set; }` (default: false)** <br />
Enables the Alpha (opacity) slider and ARGB output.
- **`ShowPalette`: `bool { get; set; }` (default: true)** <br />
Shows or hides the predefined color palette (swatches).
- **`ShowEyeDropper`: `bool { get; set; }` (default: true)** <br />
Shows or hides the screen color picker (EyeDropper) button. Only rendered when the browser supports the EyeDropper API.
- **`ShowCopyButton`: `bool { get; set; }` (default: true)** <br />
Shows or hides the 'copy to clipboard' button.
- **`PaletteColors`: `IEnumerable<Color>? { get; set; }` (default: null)** <br />
Predefined color swatches to show. When not set a default Material-style palette is used.
- **`Class`: `string? { get; set; }` (default: null)** <br />
Custom CSS class applied to the root element.
- **`Style`: `string? { get; set; }` (default: null)** <br />
Custom inline style applied to the root element.

**Arbitrary HTML attributes e.g.: `id="palette1"` will be passed to the corresponding rendered root HTML element `<div>`**.

### Events
- **`SelectedColorChanged`: `EventCallback<Color>` delegate** <br />
Callback for two-way binding. Invoked with the new `Color` when the selection changes. Used by `@bind-SelectedColor`.
- **`OnColorChanged`: `EventCallback<Color>` delegate** <br />
Notification callback invoked with the new `Color` when the selection changes.

## `ColorPicker` component (See: [demo app](https://blazorextensions.z6.web.core.windows.net/colorpicker#colorPicker))
A compact trigger button that shows a color preview (and optional HEX text) and opens a `ColorPalette` inside a Popover. By default every change is committed immediately while the Popover stays open; set `RequireApply` to only commit when the **Apply** button is clicked. Most `ColorPalette` options are forwarded to the inner palette.

### Properties
- **`SelectedColor`: `Color { get; set; }` (default: Color.FromArgb(66, 135, 245))** <br />
Currently selected (committed) `System.Drawing.Color`. Supports two-way binding with `@bind-SelectedColor`.
- **`RequireApply`: `bool { get; set; }` (default: false)** <br />
When `true` the selected color is only committed (and the Popover closed) after the Apply button is clicked. When `false` every change is committed immediately and the Popover stays open.
- **`ApplyButtonText`: `string { get; set; }` (default: "Apply")** <br />
Text shown on the Apply button. Only used when `RequireApply` is `true`.
- **`HeaderText`: `string { get; set; }` (default: "Pick a color")** <br />
Header text shown on the color selector Popover.
- **`Position`: `TooltipPositions { get; set; }` (default: TooltipPositions.Top)** <br />
Position of the Popover relative to the trigger button. Values: `{ Top, Right, Bottom, Left }`.
- **`CloseOnOutsideClick`: `bool { get; set; }` (default: true)** <br />
When `true` the Popover closes when the user clicks outside of it.
- **`Disabled`: `bool { get; set; }` (default: false)** <br />
When `true` the picker is disabled and cannot be opened.
- **`ShowHex`: `bool { get; set; }` (default: true)** <br />
Shows or hides the HEX value text next to the color preview on the trigger button.
- **`PreviewSize`: `int { get; set; }` (default: 28)** <br />
Size (width and height) of the color preview square on the trigger button, in pixels.
- **`IsOpen`: `bool { get; set; }` (default: false)** <br />
Gets or sets whether the color selector Popover is open. Supports two-way binding with `@bind-IsOpen`. Set to `true` to open it, `false` to close it.

The following properties are forwarded to the inner `ColorPalette` and behave exactly as documented above: **`EnableAlpha`** (default: false), **`ShowInfoArea`** (default: true), **`ShowColorName`** (default: true), **`ShowPalette`** (default: true), **`ShowEyeDropper`** (default: true), **`ShowCopyButton`** (default: true), **`HueAreaWidth`** (default: 300), **`HueAreaHeight`** (default: 250), **`InfoAreaWidth`** (default: 300), **`HueSliderHeight`** (default: 12) and **`PaletteColors`** (default: null). **`Class`** and **`Style`** are applied to the root (trigger) container element.

**Arbitrary HTML attributes e.g.: `id="picker1"` will be passed to the corresponding rendered root HTML element**.

### Events
- **`SelectedColorChanged`: `EventCallback<Color>` delegate** <br />
Callback for two-way binding. Invoked with the committed `Color` (on Apply when `RequireApply` is `true`, otherwise on every change). Used by `@bind-SelectedColor`.
- **`OnColorSelected`: `EventCallback<Color>` delegate** <br />
Notification callback invoked with the committed `Color` (on Apply when `RequireApply` is `true`, otherwise on every change).
- **`OnColorChanged`: `EventCallback<Color>` delegate** <br />
Notification callback invoked with the live `Color` on every change inside the palette, even before it is applied.
- **`OnOpen`: `EventCallback` delegate** <br />
Callback function called when the color selector Popover is opening.
- **`OnClose`: `EventCallback` delegate** <br />
Callback function called when the color selector Popover is closing.
- **`IsOpenChanged`: `EventCallback<bool>` delegate** <br />
Callback for two-way binding. Invoked with the new open/closed state whenever the Popover opens or closes. Used by `@bind-IsOpen`.

# Configuration

## Installation

**Majorsoft.Blazor.Components.ColorPicker** is available on [NuGet](https://www.nuget.org/packages/Majorsoft.Blazor.Components.ColorPicker/). 

```sh
dotnet add package Majorsoft.Blazor.Components.ColorPicker
```
Use the `--version` option to specify a [preview version](https://www.nuget.org/packages/Majorsoft.Blazor.Components.ColorPicker/absoluteLatest) to install.

## Usage

Add using statement to your Blazor `<component/page>.razor` file. Or globally reference it into `_Imports.razor` file.
```
@using Majorsoft.Blazor.Components.ColorPicker
```

### Dependences
**Majorsoft.Blazor.Components.ColorPicker** package depends on other Majorsoft Nuget packages:
- [Majorsoft.Blazor.Components.Common.JsInterop](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Common.JsInterop)
which handles JS Interop for the drag handle (global mouse events), the copy-to-clipboard button and the Popover outside-click detection.
- [Majorsoft.Blazor.Components.Tooltips](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Tooltips)
which provides the `Popover` used by the `ColorPicker`.

### Register services
Both components rely on the JS Interop extensions (global mouse events, clipboard and outside-click handling), so register them once during application startup.

**In case of WebAssembly project register services in your `Program.cs` file:**
```
using Majorsoft.Blazor.Components.Common.JsInterop;
...
public static async Task Main(string[] args)
{
	var builder = WebAssemblyHostBuilder.CreateDefault(args);

	//Register dependencies
	builder.Services.AddJsInteropExtensions();
}
```

**In case of Server hosted project register services in your `Startup.cs` file:**
```
using Majorsoft.Blazor.Components.Common.JsInterop;
...

public void ConfigureServices(IServiceCollection services)
{
	//Register dependencies
	services.AddJsInteropExtensions();
}
```

### `ColorPalette` usage

Following code example shows how to use the **`ColorPalette`** component with all features enabled and two-way binding via `@bind-SelectedColor`.

```
@using System.Drawing
@using Majorsoft.Blazor.Components.Core.HtmlColors

<ColorPalette @bind-SelectedColor="_selectedColor"
			  ShowInfoArea="true"
			  ShowColorName="true"
			  EnableAlpha="true"
			  ShowPalette="true"
			  ShowEyeDropper="true"
			  ShowCopyButton="true"
			  HueAreaWidth="320"
			  HueAreaHeight="250"
			  InfoAreaWidth="320"
			  HueSliderHeight="12"
			  OnColorChanged="@OnColorChanged" />

<p>
	Selected color:
	<span style="display:inline-block; width:24px; height:24px; vertical-align:middle; border:1px solid #ccc; background:@_selectedColor.ToHtmlHex();"></span>
	<strong>@_selectedColor.ToHtmlHex()</strong> (@_selectedColor.ToRgbString())
</p>

@code {
	private Color _selectedColor = Color.FromArgb(66, 135, 245);

	private void OnColorChanged(Color color)
	{
		//Write your event handling code here...
	}
}
```

You can also supply your own palette swatches through `PaletteColors`:

```
<ColorPalette @bind-SelectedColor="_selectedColor"
			  PaletteColors="_swatches" />

@code {
	private Color _selectedColor = Color.Red;
	private readonly Color[] _swatches = new[]
	{
		Color.Red, Color.Green, Color.Blue, Color.Orange, Color.Purple, Color.Black, Color.White,
	};
}
```

### `ColorPicker` usage

Following code example shows how to use the compact **`ColorPicker`** component. With `RequireApply` the color is only committed when the Apply button is clicked; otherwise it is committed on every change while the Popover stays open.

```
@using System.Drawing
@using Majorsoft.Blazor.Components.Core.HtmlColors

<ColorPicker @bind-SelectedColor="_color"
			 RequireApply="true"
			 ApplyButtonText="Apply"
			 HeaderText="Pick a color"
			 Position="@TooltipPositions.Bottom"
			 CloseOnOutsideClick="true"
			 ShowHex="true"
			 PreviewSize="28"
			 EnableAlpha="false"
			 OnColorSelected="@OnColorSelected"
			 OnColorChanged="@OnColorChanged"
			 OnOpen="@OnOpen"
			 OnClose="@OnClose" />

<p>
	Selected color:
	<span style="display:inline-block; width:24px; height:24px; vertical-align:middle; border:1px solid #ccc; background:@_color.ToHtmlHex();"></span>
	<strong>@_color.ToHtmlHex()</strong>
</p>

@code {
	private Color _color = Color.FromArgb(66, 135, 245);

	//Committed value (on Apply, or on every change when RequireApply is false)
	private void OnColorSelected(Color color)
	{
		//Write your event handling code here...
	}

	//Live value on every change, even before it is applied
	private void OnColorChanged(Color color)
	{
		//Write your event handling code here...
	}

	private void OnOpen()
	{
		//Write your event handling code here...
	}
	private void OnClose()
	{
		//Write your event handling code here...
	}
}
```
