Blazor Tooltips Components
============
[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-build-check)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=6)
[![Package Version](https://img.shields.io/nuget/v/Majorsoft.Blazor.Components.Tooltips?label=Latest%20Version)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Tooltips/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Majorsoft.Blazor.Components.Tooltips?label=Downloads)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Tooltips/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/majorimi/blazor-components/blob/master/LICENSE)

# About

Blazor component that renders a simple Tooltip with plain text any element on hover or focus. And customizable Popover (small popup) attached to any element type which clicked. It supports 4 positions, configurable show/hide delays, plain text or rich HTML content, an optional arrow, custom styling and show/hide events. 
**All components work with WebAssembly and Server hosted models**. 
For code examples [see usage](https://github.com/majorimi/blazor-components/blob/master/src/Majorsoft.Blazor.Components.TestApps.Common/Components/Tooltips.razor).

You can try it out by using the [demo app](https://blazorextensions.z6.web.core.windows.net/tooltips).

![Tooltip demo](https://raw.githubusercontent.com/majorimi/blazor-components-docs/main/github/docs/gifs/tooltip.gif)

![Popover demo](https://raw.githubusercontent.com/majorimi/blazor-components-docs/main/github/docs/gifs/popover.gif)

## Features

- Wrap **any element or component** and show a popup attached to it.
- **`Tooltip`**: lightweight, text or rich HTML, shown on **hover or keyboard focus** (accessible).
- **`Popover`**: richer, **click-triggered** anchored panel with optional header and close button, able to host any content (e.g. a color picker or a date picker).
- **4 positions**: `Top`, `Right`, `Bottom`, `Left` relative to the trigger element.
- Configurable **show/hide delays** (Tooltip) with anti-flicker handling.
- Optional **arrow** pointing to the trigger (Tooltip).
- **Programmatic open/close** and two-way `@bind-IsOpen` (Popover).
- Close on **outside click** and/or **Esc** key (Popover).
- Plain text or rich `RenderFragment` content, custom CSS class/style on both the trigger and the popup.
- Show/hide (open/close) **event callbacks**.

# Components

- **`Tooltip`**: renders a small popup with plain text or rich HTML attached to any element, shown on hover or focus.
- **`Popover`**: renders a click-triggered, anchored popup panel with an optional header and a content body, meant to host richer content.

Both components share the **`TooltipPositions`** enum used to place the popup relative to the trigger: `{ Top, Right, Bottom, Left }`.

## `Tooltip` component (See: [demo app](https://blazorextensions.z6.web.core.windows.net/tooltips#simpleTooltips))
Wrap any content with `<Tooltip>` and set the `Text` (or `TooltipContent`) parameter. The Tooltip is shown when the trigger is hovered or receives keyboard focus, and hidden when the pointer leaves or focus is lost. Quick enter/leave is debounced so the popup does not flicker.

### Properties
- **`ChildContent`: `RenderFragment` HTML content - Required** <br />
The trigger content (element) the Tooltip is attached to and shown for on hover/focus.
- **`Text`: `string { get; set; }` (default: "")** <br />
Plain text content shown inside the Tooltip popup. Ignored when `TooltipContent` is set.
- **`TooltipContent`: `RenderFragment? { get; set; }` (default: null)** <br />
Optional rich (HTML) content shown inside the Tooltip popup. Takes precedence over `Text`.
- **`Position`: `TooltipPositions { get; set; }` (default: TooltipPositions.Top)** <br />
Position of the Tooltip popup relative to the trigger element. Values: `{ Top, Right, Bottom, Left }`.
- **`DelayBeforeShow`: `int { get; set; }` (default: 0)** <br />
Delay in milliseconds before the Tooltip is shown after the pointer enters the trigger.
- **`DelayBeforeHide`: `int { get; set; }` (default: 250)** <br />
Delay in milliseconds before the Tooltip is hidden after the pointer leaves the trigger.
- **`Disabled`: `bool { get; set; }` (default: false)** <br />
When `true` the Tooltip is never shown (the trigger content still renders normally).
- **`ShowArrow`: `bool { get; set; }` (default: true)** <br />
Shows or hides the small arrow pointing from the Tooltip popup to the trigger element.
- **`Class`: `string? { get; set; }` (default: null)** <br />
Custom CSS class(es) applied to the root (trigger) container element.
- **`Style`: `string? { get; set; }` (default: null)** <br />
Custom inline style applied to the root (trigger) container element.
- **`TooltipClass`: `string? { get; set; }` (default: null)** <br />
Custom CSS class(es) applied to the Tooltip popup element.
- **`TooltipStyle`: `string? { get; set; }` (default: null)** <br />
Custom inline style applied to the Tooltip popup element. Use it to override the background color, text color, corner radius, font size, etc.

**Arbitrary HTML attributes e.g.: `tabindex="1"` will be passed to the corresponding rendered root (trigger) HTML element `<div>`**.

### Events
- **`OnShow`: `EventCallback` delegate** <br />
Callback function called when the Tooltip is showing.
- **`OnHide`: `EventCallback` delegate** <br />
Callback function called when the Tooltip is hiding.

## `Popover` component (See: [demo app](https://blazorextensions.z6.web.core.windows.net/tooltips#popover))
Wrap any content with `<Popover>` to attach a click-triggered, anchored panel. Clicking the trigger toggles the panel. The panel can show an optional header (text or rich content) with a close button and renders `PopoverContent` in its body. It is meant to host richer content such as a color selector or a date picker. The Popover can also be opened/closed programmatically and supports two-way binding via `@bind-IsOpen`.

### Properties
- **`ChildContent`: `RenderFragment` HTML content - Required** <br />
The trigger content (element) the Popover is attached to. Clicking it toggles the Popover (unless `Disabled` is `true`).
- **`PopoverContent`: `RenderFragment` HTML content - Required** <br />
The HTML content rendered inside the Popover panel body. Can host any component, e.g. a color selector or date picker.
- **`HeaderText`: `string? { get; set; }` (default: null)** <br />
Plain text shown in the Popover header. Ignored when `HeaderContent` is set.
- **`HeaderContent`: `RenderFragment? { get; set; }` (default: null)** <br />
Optional rich (HTML) header content. Takes precedence over `HeaderText`.
- **`ShowCloseButton`: `bool { get; set; }` (default: true)** <br />
Shows or hides the header close (**x**) button. The header is rendered when the close button is shown or any header content/text is set.
- **`Position`: `TooltipPositions { get; set; }` (default: TooltipPositions.Bottom)** <br />
Position of the Popover panel relative to the trigger element. Values: `{ Top, Right, Bottom, Left }`.
- **`Width`: `int { get; set; }` (default: 0)** <br />
Fixed Popover panel width in pixels. When `0` the width is sized to the content.
- **`Height`: `int { get; set; }` (default: 0)** <br />
Fixed Popover panel height in pixels. When `0` the height is sized to the content.
- **`CloseOnOutsideClick`: `bool { get; set; }` (default: true)** <br />
When `true` the Popover closes when the user clicks outside of it.
- **`CloseOnEscapeKey`: `bool { get; set; }` (default: true)** <br />
When `true` the Popover closes when the **Esc** (Escape) key is pressed while it has focus.
- **`Disabled`: `bool { get; set; }` (default: false)** <br />
When `true` the trigger does not open the Popover and programmatic open requests are ignored.
- **`Class`: `string? { get; set; }` (default: null)** <br />
Custom CSS class(es) applied to the root (trigger) container element.
- **`Style`: `string? { get; set; }` (default: null)** <br />
Custom inline style applied to the root (trigger) container element.
- **`PopoverClass`: `string? { get; set; }` (default: null)** <br />
Custom CSS class(es) applied to the Popover panel element.
- **`PopoverStyle`: `string? { get; set; }` (default: null)** <br />
Custom inline style applied to the Popover panel element.
- **`IsOpen`: `bool { get; set; }` (default: false)** <br />
Gets or sets whether the Popover is open. Supports two-way binding with `@bind-IsOpen`. Set to `true` to open it programmatically, `false` to close it. Reading it returns the current open/closed state.

**Arbitrary HTML attributes e.g.: `id="pop1"` will be passed to the corresponding rendered root (trigger) HTML element `<div>`**.

### Events
- **`OnOpen`: `EventCallback` delegate** <br />
Callback function called when the Popover is opening.
- **`OnClose`: `EventCallback` delegate** <br />
Callback function called when the Popover is closing.
- **`IsOpenChanged`: `EventCallback<bool>` delegate** <br />
Callback for two-way binding. Invoked with the new open/closed state whenever the Popover opens or closes. Used by `@bind-IsOpen`.

# Configuration

## Installation

**Majorsoft.Blazor.Components.Tooltips** is available on [NuGet](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Tooltips/). 

```sh
dotnet add package Majorsoft.Blazor.Components.Tooltips
```
Use the `--version` option to specify a [preview version](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Tooltips/absoluteLatest) to install.

## Usage

Add using statement to your Blazor `<component/page>.razor` file. Or globally reference it into `_Imports.razor` file.
```
@using Majorsoft.Blazor.Components.Tooltips
```

### Dependences
**Majorsoft.Blazor.Components.Tooltips** package depends on other Majorsoft Nuget packages:
- [Majorsoft.Blazor.Components.Common.JsInterop](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Common.JsInterop)
which the `Popover` uses to detect outside clicks for closing.

### Register services
The `Tooltip` component works without any service registration. The `Popover` component relies on the JS Interop extensions to handle outside-click detection, so register them when you use `Popover`.

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

### `Tooltip` usage

Following code example shows how to use the **`Tooltip`** component. Wrap the trigger element and set the `Text`. The Tooltip is shown on hover or keyboard focus.

```
<Tooltip Text="I am a simple text Tooltip.">
	<span style="border-bottom: 1px dotted #555; cursor: help;">Hover this text</span>
</Tooltip>

<Tooltip Text="Tooltips work on any element, e.g. buttons. Focus me with the Tab key too.">
	<button class="btn btn-primary">Hover or focus this button</button>
</Tooltip>
```

This example shows a fully customized Tooltip with a custom position, delays, rich HTML content and custom styling plus show/hide events.

```
<Tooltip Position="@_position"
		 DelayBeforeShow="@_delayBeforeShow"
		 DelayBeforeHide="@_delayBeforeHide"
		 ShowArrow="@_showArrow"
		 Disabled="@_disabled"
		 TooltipStyle="@_customStyle"
		 OnShow="@OnTooltipShow"
		 OnHide="@OnTooltipHide">
	<ChildContent>
		<button class="btn btn-success">Hover me</button>
	</ChildContent>
	<TooltipContent>
		<div class="text-left">
			<strong>Rich Tooltip</strong>
			<hr class="my-1" />
			Tooltips can render any markup, e.g. an icon &#9733; or a
			<a href="https://github.com/majorimi/blazor-components" target="_blank">link</a>.
		</div>
	</TooltipContent>
</Tooltip>

@code {
	private TooltipPositions _position = TooltipPositions.Top;
	private int _delayBeforeShow = 0;
	private int _delayBeforeHide = 250;
	private bool _showArrow = true;
	private bool _disabled = false;
	private string _customStyle = "background-color: #6f42c1; color: #fff; border-radius: 6px; font-size: 14px;";

	private void OnTooltipShow()
	{
		//Write your event handling code here...
	}
	private void OnTooltipHide()
	{
		//Write your event handling code here...
	}
}
```

### `Popover` usage

Following code example shows how to use the **`Popover`** component with a header, programmatic control via `@bind-IsOpen` and open/close events.

```
<Popover @bind-IsOpen="_open"
		 HeaderText="Popover header"
		 Position="@TooltipPositions.Bottom"
		 Width="280"
		 ShowCloseButton="true"
		 CloseOnOutsideClick="true"
		 CloseOnEscapeKey="true"
		 OnOpen="@OnPopoverOpen"
		 OnClose="@OnPopoverClose">
	<ChildContent>
		<button class="btn btn-primary">Toggle Popover</button>
	</ChildContent>
	<PopoverContent>
		<p class="mb-0">
			Popovers can contain any HTML content. Click outside, press <kbd>Esc</kbd> or use the
			close button to dismiss (depending on the settings).
		</p>
	</PopoverContent>
</Popover>

@*Open/close the Popover programmatically through the bound field*@
<button class="btn btn-sm btn-success" @onclick="@(() => _open = true)">Open</button>
<button class="btn btn-sm btn-danger" @onclick="@(() => _open = false)">Close</button>
<button class="btn btn-sm btn-secondary" @onclick="@(() => _open = !_open)">Toggle</button>

@code {
	private bool _open;

	private void OnPopoverOpen()
	{
		//Write your event handling code here...
	}
	private void OnPopoverClose()
	{
		//Write your event handling code here...
	}
}
```

The `Popover` is well suited to host richer content. The example below hosts a `ColorPalette` (from the `Majorsoft.Blazor.Components.Core`/Color package) and keeps the Popover open while the user picks a color, closing it only on **Apply**, the close button or an outside click.

```
<Popover @bind-IsOpen="_colorOpen" HeaderText="Pick a color" Position="@TooltipPositions.Bottom" Width="340">
	<ChildContent>
		<button class="btn btn-outline-secondary">@_pickedColor.ToHtmlHex()</button>
	</ChildContent>
	<PopoverContent>
		<ColorPalette SelectedColor="@_pickedColor" OnColorChanged="@(c => _pickedColor = c)" />
		<div class="text-right mt-2">
			<button class="btn btn-sm btn-primary" @onclick="@(() => _colorOpen = false)">Apply</button>
		</div>
	</PopoverContent>
</Popover>

@code {
	private bool _colorOpen;
	private System.Drawing.Color _pickedColor = System.Drawing.Color.FromArgb(66, 135, 245);
}
```
