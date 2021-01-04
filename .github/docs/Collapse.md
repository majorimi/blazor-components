Blazor Collapse Components
============
[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-build-check)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=6)
[![Package Version](https://img.shields.io/nuget/v/Majorsoft.Blazor.Components.Collapse?label=Latest%20Version)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Collapse/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Majorsoft.Blazor.Components.Collapse?label=Downloads)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Collapse/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/majorimi/blazor-components/blob/master/LICENSE)

# About

Blazor component that renders customizable Collapsible/Expandable panel and Accordion with many but only one active panel also custom content and header.
**All components work with WebAssembly and Server hosted models**. 
For code examples [see usage](https://github.com/majorimi/blazor-components/blob/master/src/Blazor.Components.TestApps.Common/Components/Collapse.razor).

You can try it out by using the [demo app](https://blazorextensions.z6.web.core.windows.net/Collapse).

# Components

- **`CollapsePanel`**: Renders `CollapsePanel` component which is an **Expandable and Collapsible panel** with customizable header and content.
- **`Accordion`**: Renders a **set of `CollapsePanel` components** which is an Expandable and Collapsible panel with customizable header and content. But **`Accordion` allows only one Expanded (active)** panel.

## `CollapsePanel` component (See: [demo app](https://blazorextensions.z6.web.core.windows.net/collapse#collapsePanel))
Blazor component that renders `CollapsePanel` component which is an **Expandable and Collapsible panel** with customizable header and content.
Multiple components can be added to a Balzor component each will act and work independently.

![CollapsePanel demo](https://github.com/majorimi/blazor-components/raw/master/.github/docs/gifs/Collapse.gif)

### Properties
- **`CommonHeader`: `RenderFragment` HTML content - Required or see specific headers** <br />
Common header for Collapse control it will be shown for any collapsed state. It overrides `ExpandedHeaderContent`, `CollapsedHeaderContent`.
- **`ExpandedHeaderContent`: `RenderFragment` HTML content** <br />
Special header for Collapse control it is shown when item Expanded. **Note: if `CommonHeader` declared it will override specific headers.**
- **`CollapsedHeaderContent`: `RenderFragment` HTML content** <br />
Special header for Collapse control it is shown when item Collapsed. **Note: if `CommonHeader` declared it will override specific headers.**
- **`Content`: `RenderFragment` HTML content - Required** <br />
HTML Content of the collapse panel.
- **`Collapsed`: `bool { get; set; }` (default: false) - Required** <br />
Can be set if panel should be collapsed `true` or expanded `false`.
- **`ExpandedColor`: `string { get; set; }` (default: "lightGray") - Required** <br />
Sets the `style` of the `background-color` when tab is Active. Use HTML specified: **Color Names**, **RGB**, **HEX** or with **HSL** values.
- **`CollapsedColor`: `string { get; set; }` (default: "lightGray") - Required** <br />
Sets the `style` of the `background-color` when tab is not the Active tab. Use HTML specified: **Color Names**, **RGB**, **HEX** or with **HSL** values.
- **`HoverColor`: `string { get; set; }` (default: "WhiteSmoke") - Required** <br />
Sets the `style` of the `background-color` when button is hovered over with mouse. Use HTML specified: **Color Names**, **RGB**, **HEX** or with **HSL** values.
- **`ContentHeight`: `int { get; set; }` (default: 200) - Required** <br />
Sets the height of Content panel in `px`. 0 is auto.
- **`Animate`: `bool { get; set; }` (default: true)** <br />
Determines to apply CSS animation and transition on Collapse state changes or not.
**Note: in case of `auto` height some animation won't work.**
- **`Disabled`: `bool { get; set; }` (default: false)** <br />
Determines whether the rendered HTML `<input>` element should be disabled or not.
- **`InnerElementReference`: `ElementReference { get; }`** <br />
Exposes a Blazor `ElementReference` of the wrapped around HTML element. It can be used e.g. for JS interop, etc.

**Arbitrary HTML attributes e.g.: `tabindex="1"` will be passed to the corresponding rendered HTML element `<div>`**.

### Events
- **`OnCollapseChanged`: `EventCallback<bool>` delegate** <br />
Callback function called when panel collapsed or expanded. Collapsed state is the callback parameter.

## `Accordion` component (See: [demo app](https://blazorextensions.z6.web.core.windows.net/collapse#Accordion))
Blazor component that renders a **set of `CollapsePanel` components** which is an Expandable and Collapsible panel with customizable header and content. But **`Accordion` allows only one Expanded (active)** panel.

![Accordion button demo](https://github.com/majorimi/blazor-components/raw/master/.github/docs/gifs/Accordion.gif)

### Properties
- **`Content`: `RenderFragment` HTML content - Required**
Required HTML content to show in Toggle button. It can be any text or image (please use transparent background image).
- **`Checked`: `bool { get; set; }` (default: true) - Required** <br />
Represents Toggle button value: **ON**: `true`, **OFF**: `false`.
- **`OnColor`: `string { get; set; }` (default: "white") - Required** <br />
Sets the `style` of the HTML `<button>` `background-color` when button is ON (bool value `true`). Use HTML specified: **Color Names**, **RGB**, **HEX** or with **HSL** values.
- **`OffColor`: `string { get; set; }` (default: "lightgray") - Required** <br />
Sets the `style` of the HTML `<button>` `background-color` when button is OFF (bool value `false`). Use HTML specified: **Color Names**, **RGB**, **HEX** or with **HSL** values.
- **`HoverColor`: `string { get; set; }` (default: "whitesmoke") - Required** <br />
Sets the `style` of the HTML `<button>` `background-color` when button is hovered over with mouse. Use HTML specified: **Color Names**, **RGB**, **HEX** or with **HSL** values.
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

# Configuration

## Installation

**Majorsoft.Blazor.Components.Collapse** is available on [NuGet](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Collapse/). 

```sh
dotnet add package Majorsoft.Blazor.Components.Collapse
```
Use the `--version` option to specify a [preview version](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Collapse/absoluteLatest) to install.

## Usage

Add using statement to your Blazor `<component/page>.razor` file. Or globally reference it into `_Imports.razor` file.
```
@using Majorsoft.Blazor.Components.Collapse
```

### `CollapsePanel` usage

Following code example shows how to use **`CollapsePanel`** component in your Blazor App. 

```
<CollapsePanel CollapsedColor="@_collapsedColor"
				ExpandedColor="@_expandedColor"
				HoverColor="@_hoverColor"
				Disabled="@_isCollapseDisabled"
				Collapsed="@_isCollapsed"
				ContentHeight="@_height"
				Animate="@_isAnimated"
				ShowContentOverflow="@_showOverflow"
				OnCollapseChanged="OnCollapsed">
	<CollapsedHeaderContent>
		<div class="w-100">
			<h5>Expand me</h5>
			<span class="fa fa-lg fa-chevron-circle-down" aria-hidden="true"></span>
		</div>
	</CollapsedHeaderContent>
	<ExpandedHeaderContent>
		<div class="w-100">
			<h5>Collapse me</h5>
			<span class="fa fa-lg fa-chevron-circle-up" aria-hidden="true"></span>
		</div>
	</ExpandedHeaderContent>
	<Content>
		<div style="border: 1px solid gray; height: 100%;">
			<h4>This is the content</h4>
		</div>
	</Content>
</CollapsePanel>

@code {
	private string _collapsedColor = "DodgerBlue";
	private string _expandedColor = "LightBlue";
	private string _hoverColor = "LightGray";
	private bool _isAnimated = true;
	private bool _isCollapseDisabled = false;
	private bool _isCollapsed = false;
	private bool _showOverflow = false;
	private int _height = 200;

	private string _collapseLog;

	private async Task OnCollapsed(bool state)
	{
		_isCollapsed = state;
		_collapseLog = WriteLog(_collapseLog, $"Collapse panel state has changed event Collapsed: {state}");
	}
}
```

### `ToggleButton` usage

Following code example shows how to use **`ToggleButton`** component in your Blazor App. 

```
<ToggleButton @ref="_toggleButton"
				Checked="@_isButtonChecked"
				OnColor="@_buttonOnColor"
				OffColor="@_buttonOffColor"
				HoverColor="@_buttonHoverColor"
				Width="@_buttonWidth"
				Height="@_buttonHeight"
				Disabled="@_buttonDisabled"
				OnToggleChanged="OnToggleClicked">
	<Content>
		<img src="https://raw.githubusercontent.com/majorimi/blazor-components/master/src/Blazor.Components.TestApps.Common/wwwroot/place-marker.png" width="@(_buttonWidth - 5)px" height="@(_buttonHeight - 5)px" />
	</Content>
</ToggleButton>

<ToggleButton>
	<Content><strong>B</strong></Content>
</ToggleButton>
<ToggleButton>
	<Content><i>I</i></Content>
</ToggleButton>
<ToggleButton>
	<Content><u>U</u></Content>
</ToggleButton>

@code {
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			await _toggleButton.InnerElementReference.FocusAsync();
		}
	}

	//Button
	private ToggleButton _toggleButton;

	private string _buttonOnColor = "lightGray";
	private string _buttonOffColor = "white";
	private string _buttonHoverColor = "WhiteSmoke";
	private int _buttonWidth = 30;
	private int _buttonHeight = 30;
	private bool _isButtonChecked = true;
	private bool _buttonDisabled = false;

	private async Task OnToggleClicked(bool val)
	{
		_isButtonChecked = val;
	}
}

```

### `ToggleButtonGroup` usage

Following code example shows how to use **`ToggleButtonGroup`** component in your Blazor App. 

```
@*ToggleButtons can be configured as regular ToggleButton*@
<ToggleButtonGroup @ref="_btnGroup" MustToggled="_mustToggled" OnToggleChanged="OnToggleGroupClicked"
			Disabled="@_buttonGroupDisabled" ActiveButton="@_activeButton">
	<ToggleButtons>
		<ToggleButton @ref="_btn1">
			<Content><strong>1</strong></Content>
		</ToggleButton>
		<ToggleButton @ref="_btn2">
			<Content><strong>2</strong></Content>
		</ToggleButton>
		<ToggleButton @ref="_btn3">
			<Content><strong>3</strong></Content>
		</ToggleButton>
	</ToggleButtons>
</ToggleButtonGroup>

@code {
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			_activeButton = _btn1;
			_btnCount = _btnGroup.ButtonCount;
			StateHasChanged();
		}
	}

	private ToggleButtonGroup _btnGroup;
	private ToggleButton _activeButton;
	private ToggleButton _btn1;
	private ToggleButton _btn2;
	private ToggleButton _btn3;
	private bool _buttonGroupDisabled = false;
	private bool _mustToggled = false;
	private int _btnCount = 0;

	private ElementReference _log3;
	private string _buttonGroupLog;

	private async Task OnToggleGroupClicked(ToggleButton active)
	{
		_activeButton = active;
		var index = _btnGroup.Buttons.ToList().IndexOf(active);
	}
}
```