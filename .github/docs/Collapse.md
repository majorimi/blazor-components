Blazor Collapse Components
============
[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-build-check)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=6)
[![Package Version](https://img.shields.io/nuget/v/Majorsoft.Blazor.Components.Collapse?label=Latest%20Version)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Collapse/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Majorsoft.Blazor.Components.Collapse?label=Downloads)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Collapse/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/majorimi/blazor-components/blob/master/LICENSE)

# About

Blazor component that renders customizable Collapsible/Expandable panel and Accordion with many but only one active panel also custom content and header.
**All components work with WebAssembly and Server hosted models**. 
For code examples [see usage](https://github.com/majorimi/blazor-components/blob/master/src/Majorsoft.Blazor.Components.TestApps.Common/Components/Collapse.razor).

You can try it out by using the [demo app](https://blazorextensions.z6.web.core.windows.net/Collapse).

# Components

- **`CollapsePanel`**: Renders `CollapsePanel` component which is an **Expandable and Collapsible panel** with customizable header and content.
- **`Accordion`**: Renders a **set of `CollapsePanel` components** which is an Expandable and Collapsible panel with customizable header and content. But **`Accordion` allows only one Expanded (active)** panel.

## `CollapsePanel` component (See: [demo app](https://blazorextensions.z6.web.core.windows.net/collapse#collapsePanel))
Blazor component that renders `CollapsePanel` component which is an **Expandable and Collapsible panel** with customizable header and content.
Multiple components can be added to a Balzor component each will act and work independently.

![CollapsePanel demo](https://github.com/majorimi/blazor-components-docs/raw/main/github/docs/gifs/collapse.gif)

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
Sets the height (in reality sets max-height because of CSS transition issues) of Content panel in `px`. 0 is auto.
- **`MaxAllowedContentHeight`: `int { get; set; }` (default: 200) - Required** <br />
Sets the max-height if Content panel `ContentHeight` is set to 0 (auto).
- **`Animate`: `bool { get; set; }` (default: true)** <br />
Determines to apply CSS animation and transition on Collapse state changes or not.
**Note: in case of Content panel `ContentHeight` is set to 0 (auto), then use `MaxAllowedContentHeight` to set max-height CSS property which will be animated.
Also important based on max-height value transition speed for expand/collapse might differ!.**
- **`Disabled`: `bool { get; set; }` (default: false)** <br />
Determines whether the rendered HTML `<input>` element should be disabled or not.
- **`InnerElementReference`: `ElementReference { get; }`** <br />
Exposes a Blazor `ElementReference` of the wrapped around HTML element. It can be used e.g. for JS interop, etc.

**Arbitrary HTML attributes e.g.: `tabindex="1"` will be passed to the corresponding rendered HTML element `<div>`**.

### Events
- **`OnCollapseChanged`: `EventCallback<bool>` delegate** <br />
Callback function called when panel collapsed or expanded. Collapsed state is the callback parameter.

## `Accordion` component (See: [demo app](https://blazorextensions.z6.web.core.windows.net/collapse#Accordion))
Blazor component that renders a **set of `CollapsePanel` components**. It is only a wrapper element for `CollapsePanel`s.
Each `CollapsePanel` act as individual components so they should be configured directly (use variables to change parameter for all at once).
**`Accordion` allows only one Expanded (active)** panel.

![Accordion button demo](https://github.com/majorimi/blazor-components-docs/raw/main/github/docs/gifs/accordion.gif)

### Properties
- **`CollapsePanels`: `RenderFragment` HTML content - Required**
Required HTML content to set `CollapsePanel`s as `RenderFragment`.
- **`CollapsePanelItems`: `IEnumerable<CollapsePanel> { get; }`**
Returns all the `CollapsePanel` reference added to the group. It can be used for activating any of the panels.
- **`CollapsePanelCount`: `int { get; }`**
Returns the number of `CollapsePanel` int the given `Accordion`.
- **`ActiveCollapsePanel`: `CollapsePanel { get; set; }` (default: NULL)**
Returns currently active `CollapsePanel` element ref also can be used to set which panel should be Expanded "active".
- **`Disabled`: `bool { get; set; }` (default: false)** <br />
Determines whether the rendered HTML elements should be disabled or not.
- **`InnerElementReference`: `ElementReference { get; }`** <br />
Exposes a Blazor `ElementReference` of the wrapped around HTML element. It can be used e.g. for JS interop, etc.

### Events
- **`OnCollapsePanelChanged`: `EventCallback<CollapsePanel>` delegate** <br />
Callback function called when other `CollapsePanel` activated. Active `CollapsePanel` is the callback parameter.

**Arbitrary HTML attributes e.g.: `id="ac1"` will be passed to the corresponding rendered HTML element `<input>`**.

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

### `Accordion` usage

Following code example shows how to use **`Accordion`** component in your Blazor App. 

```
<Accordion @ref="_accordion" id="ac1"
			ActiveCollapsePanel="@_activePanel"
			Disabled="@_isAccordionDisabled"
			OnCollapsePanelChanged="OnAccordionChanged">
	<CollapsePanels>
		<CollapsePanel @ref="_panel1" style="margin-bottom: 10px;" CollapsedColor="@_accordionCollapsedColor"
						ExpandedColor="@_accordionExpandedColor"
						HoverColor="@_accordionHoverColor">
			<CollapsedHeaderContent>
				<div class="w-100">
					<h5>Panel 1 - Expand me</h5>
					<span class="fa fa-lg fa-chevron-circle-down" aria-hidden="true"></span>
				</div>
			</CollapsedHeaderContent>
			<ExpandedHeaderContent>
				<div class="w-100">
					<h5>Active panel 1 - Collapse me</h5>
					<span class="fa fa-lg fa-chevron-circle-up" aria-hidden="true"></span>
				</div>
			</ExpandedHeaderContent>
			<Content>
				<div style="border: 1px solid gray; height: 100%;">
					<h4>This is the content</h4>
				</div>
			</Content>
		</CollapsePanel>
		<CollapsePanel @ref="_panel2" style="margin-bottom: 10px;" CollapsedColor="@_accordionCollapsedColor"
						ExpandedColor="@_accordionExpandedColor"
						HoverColor="@_accordionHoverColor">
			<CollapsedHeaderContent>
				<div class="w-100">
					<h5>Panel 2 - Expand me</h5>
					<span class="fa fa-lg fa-chevron-circle-down" aria-hidden="true"></span>
				</div>
			</CollapsedHeaderContent>
			<ExpandedHeaderContent>
				<div class="w-100">
					<h5>Active panel 2 - Collapse me</h5>
					<span class="fa fa-lg fa-chevron-circle-up" aria-hidden="true"></span>
				</div>
			</ExpandedHeaderContent>
			<Content>
				<div style="border: 1px solid gray; height: 100%;">
					<h4>This is the content</h4>
				</div>
			</Content>
		</CollapsePanel>
		<CollapsePanel @ref="_panel3" style="margin-bottom: 10px;" CollapsedColor="@_accordionCollapsedColor"
						ExpandedColor="@_accordionExpandedColor"
						HoverColor="@_accordionHoverColor">
			<CollapsedHeaderContent>
				<div class="w-100">
					<h5>Panel 3 - Expand me</h5>
					<span class="fa fa-lg fa-chevron-circle-down" aria-hidden="true"></span>
				</div>
			</CollapsedHeaderContent>
			<ExpandedHeaderContent>
				<div class="w-100">
					<h5>Active panel 3 - Collapse me</h5>
					<span class="fa fa-lg fa-chevron-circle-up" aria-hidden="true"></span>
				</div>
			</ExpandedHeaderContent>
			<Content>
				<div style="border: 1px solid gray; height: 100%;">
					<h4>This is the content</h4>
				</div>
			</Content>
		</CollapsePanel>
	</CollapsePanels>
</Accordion>

@code {
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			//Accordion
			_activePanel = _panel2;
			_collapseCount = _accordion.CollapsePanelCount;
			StateHasChanged();
		}
	}

	private string _accordionCollapsedColor = "green";
	private string _accordionExpandedColor = "lightGreen";
	private string _accordionHoverColor = "lime";
	private bool _isAccordionDisabled = false;
	private int _collapseCount;

	private Accordion _accordion;
	private CollapsePanel _activePanel;
	private CollapsePanel _panel1;
	private CollapsePanel _panel2;
	private CollapsePanel _panel3;

	private async Task OnAccordionChanged(CollapsePanel active)
	{
		_activePanel = active;
		var index = _accordion.CollapsePanelItems.ToList().IndexOf(active);
	}
}
```