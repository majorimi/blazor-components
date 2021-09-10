
Blazor Tabs Components
============
[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-build-check)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=6)
[![Package Version](https://img.shields.io/nuget/v/Majorsoft.Blazor.Components.Tabs?label=Latest%20Version)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Tabs/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Majorsoft.Blazor.Components.Tabs?label=Downloads)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Tabs/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/majorimi/blazor-components/blob/master/LICENSE)

# About

Blazor component that renders customizable Tabs element panel with many tabs and custom content.
**All components work with WebAssembly and Server hosted models**. 
For code examples [see usage](https://github.com/majorimi/blazor-components/blob/master/src/Majorsoft.Blazor.Components.TestApps.Common/Components/Tabs.razor).

You can try it out by using the [demo app](https://blazorextensions.z6.web.core.windows.net/Tabs).

# Components
- **`TabsPanel`**: Renders **HTML `<div>` container** for `TabItem` components also customizable size, color, etc. for Tab Items.
- **`TabItem`**: Renders **HTML `<button>` styled as Tab** with custom header and content. **Must be placed inside a `TabsPanel` component.**

![Tabs demo](https://github.com/majorimi/blazor-components-docs/raw/main/github/docs/gifs/tabs.gif)

## `TabsPanel` component

### Properties
- **`TabItems`: `RenderFragment` HTML content - Required** <br />
Required HTML content to set `TabItem`s as `RenderFragment`.
- **`ActiveColor`: `string { get; set; }` (default: "white") - Required** <br />
Sets the `style` of the HTML `<button>` `background-color` when Tab is Active (selected). Use HTML specified: **Color Names**, **RGB**, **HEX** or with **HSL** values.
- **`InactiveColor`: `string { get; set; }` (default: "lightgray") - Required** <br />
Sets the `style` of the HTML `<button>` `background-color` when Tab is Inactive (not selected). Use HTML specified: **Color Names**, **RGB**, **HEX** or with **HSL** values.
- **`HoverColor`: `string { get; set; }` (default: "whitesmoke") - Required** <br />
Sets the `style` of the HTML `<button>` `background-color` when Tab is hovered over with mouse. Use HTML specified: **Color Names**, **RGB**, **HEX** or with **HSL** values.
- **`TabItemsHeight`: `int { get; set; }` (default: 40) - Required** <br />
Sets all `TabItem` elements height in `px`.
- **`TabItemsWidth`: `int { get; set; }` (default: 100) - Required** <br />
Sets all `TabItem` elements element width in `px`.
- **`Disabled`: `bool { get; set; }` (default: false)** <br />
Determines whether all the rendered HTML elements should be disabled or not.
- **`AllowTabActivationByPermalink`: `bool { get; set; }` (default: true)** <br />
Enables or disables `TabItem` activation with URL Permalink fragment.
**NOTE: in order to make TabActivation work `Majorsoft.Blazor.Components.PermaLink` component is used and it MUST [set up correctly](https://github.com/majorimi/blazor-components/blob/master/.github/docs/PermaLink.md#configuration)!**
- **`Animate`: `bool { get; set; }` (default: true)** <br />
Determines to apply CSS animation and transion on Tab changes or not.
- **`TabPositon`: `TabPositons { get; set; }` (default: TabPositons.Left)** <br />
Determines TabItems vertical positon, values: {Left, Center, Right }
- **`Tabs`: `IEnumerable<TabItem> { get; }`**
Returns all the `TabItem` reference added to the group. It can be used for activating any of the tabs.
- **`TabCount`: `int { get; }`**
Returns the number of `TabItem`s int the given `TabsPanel`.
- **`ActiveTab`: `TabItem { get; set; }` (default: NULL or first added TabItem ref)**
Returns currently active `TabItem` element ref also can be used to set which Tab should be active "selected".
- **`InnerElementReference`: `ElementReference { get; }`** <br />
Exposes a Blazor `ElementReference` of the wrapped around HTML element. It can be used e.g. for JS interop, etc.

**Arbitrary HTML attributes e.g.: `tabindex="1"` will be passed to the corresponding rendered HTML element `<input>`**.

### Events
- **`OnTabChanged`: `EventCallback<TabItem>` delegate** <br />
Callback function called when other tab activated. Active `TabItem` is the callback parameter.

## `TabItem` component

### Properties
- **`Header`: `RenderFragment` HTML content - Required**
Required HTML content to show Header content of current TabItem.
- **`Content`: `RenderFragment` HTML content - Required**
Required HTML content to show content of current TabItem.
- **`Disabled`: `bool { get; set; }` (default: false)** <br />
Determines whether the current rendered TabItem should be disabled or not.
- **`Hidden`: `bool { get; set; }` (default: false)** <br />
Determines whether the current rendered TabItem should be hidden or not.
- **`Permalink`: `string { get; set; }` (default: "")** <br />
Permalink value to append to the URL and activate the `TabItem` based on matching value.
**NOTE: in order to make TabActivation work `Majorsoft.Blazor.Components.PermaLink` component is used and it MUST [set up correctly](https://github.com/majorimi/blazor-components/blob/master/.github/docs/PermaLink.md#configuration)!**

**Arbitrary HTML attributes e.g.: `tabindex="1"` will be passed to the corresponding rendered HTML element `<input>`**.

# Configuration

## Installation

**Majorsoft.Blazor.Components.Tabs** is available on [NuGet](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Tabs/). 

```sh
dotnet add package Majorsoft.Blazor.Components.Tabs
```
Use the `--version` option to specify a [preview version](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Tabs/absoluteLatest) to install.

## Usage

Add using statement to your Blazor `<component/page>.razor` file. Or globally reference it into `_Imports.razor` file.
```
@using Majorsoft.Blazor.Components.Tabs
```

### Dependences
**Majorsoft.Blazor.Components.Tabs** package "partially" depends on other Majorsoft Nuget packages:
- [Majorsoft.Blazor.Components.Common.JsInterop](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Common.JsInterop)
which handles JS Interop for many features e.g. scrolling, etc.
- [Majorsoft.Blazor.Components.Common.PermaLink](https://www.nuget.org/packages/Majorsoft.Blazor.Components.PermaLink)
which track navigations (URL changes) and identify permalink elements.

**NOTE: only TabItem activation feature depend on Permalink. If you don't want to use that feature just leave `Permalink` parameters empty and do not setup PermalinkWatcher.
Also later this feature can be disabled by `AllowTabActivationByPermalink = false`.**

### `TabsPanel` and `TabItem` usage

Following code example shows how to use **`TabsPanel`**  with **`TabItem`** component in your Blazor App. 

**NOTE: to use TabActivation feature `Permalink="Tab1"` must be set and Permalink services must be [configured correctly](https://github.com/majorimi/blazor-components/blob/master/.github/docs/PermaLink.md#configuration)!**

```
@*Simple tab usage*@
<TabsPanel>
	<TabItems>
		<TabItem>
			<Header>Tab1</Header>
			<Content>Tab1</Content>
		</TabItem>
		<TabItem>
			<Header>Tab2</Header>
			<Content>Tab2</Content>
		</TabItem>
		<TabItem>
			<Header>Tab3</Header>
			<Content>Tab3</Content>
		</TabItem>
	</TabItems>
</TabsPanel>

@*Advanced tab usage*@
<TabsPanel @ref="_tabs"
		ActiveColor="@_activeColor"
		InactiveColor="@_inactiveColor"
		HoverColor="@_hoverColor"
		ActiveTab="@_activeTab"
		TabItemsHeight="@_height"
		TabItemsWidth="@_width"
		Disabled="@_allTabsDisabled"
		TabPositon="@_tabPositon"
		Animate="@_isAnimated"
		OnTabChanged="OnTabChanged">
	<TabItems>
		<TabItem id="tab1" @ref="_tab1" Disabled="false" Permalink="Tab1" Hidden="false">
			<Header><strong>Tab 1</strong></Header>
			<Content>
				<h1>The first tab</h1>
			</Content>
		</TabItem>
		<TabItem @ref="_tab2" Disabled="false" Permalink="Tab2" Hidden="false">
			<Header><i>Tab 2</i></Header>
			<Content>
				<h1>The second tab</h1>
			</Content>
		</TabItem>
		<TabItem id="tab3" @ref="_tab3" Disabled="@_isTabDisabled" Permalink="Tab3" Hidden="@_isTabHidden">
			<Header><u>Can disable</u></Header>
			<Content>
				<h1>This tab can be disabled</h1>
				<p>And also any <code>TabItem</code> can be disabled by using <code>Disabled</code> property.</p>
			</Content>
		</TabItem>
		<TabItem id="tab4" @ref="_tab4" Disabled="false" Permalink="Tab4" Hidden="false">
			<Header>Header icon <i class="fa fa-home"></i></Header>
			<Content>
				<h1>Tab with icon in header</h1>
			</Content>
		</TabItem>
	</TabItems>
</TabsPanel>

@using System.Linq;

@code {
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			await _tabs.InnerElementReference.FocusAsync();

			//Group
			_activeTab = _tab2;
			_tabsCount = _tabs.TabCount;
			StateHasChanged();
		}
	}

	private string _activeColor = "DodgerBlue";
	private string _inactiveColor = "White";
	private string _hoverColor = "WhiteSmoke";
	private int _width = 135;
	private int _height = 40;
	private TabPositons _tabPositon = TabPositons.Left;
	private bool _isAnimated = false;
	private bool _allTabsDisabled = false;
	private bool _isTabDisabled = false;
	private bool _isTabHidden = false;

	private int _tabsCount;

	private TabsPanel _tabs;
	private TabItem _activeTab;
	private TabItem _tab1;
	private TabItem _tab2;
	private TabItem _tab3;
	private TabItem _tab4;

	private async Task OnTabChanged(TabItem tab)
	{
		_activeTab = tab;
		var index = _tabs.Tabs.ToList().IndexOf(tab);
	}
```