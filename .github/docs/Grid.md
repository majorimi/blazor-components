Blazor Grid Components
============
[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-build-check)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=6)
[![Package Version](https://img.shields.io/nuget/v/Majorsoft.Blazor.Components.Grid?label=Latest%20Version)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Grid/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Majorsoft.Blazor.Components.Grid?label=Downloads)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Grid/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/majorimi/blazor-components/blob/master/LICENSE)

# About

Blazor component that renders a customizable data Grid (HTML table) from any `IEnumerable<TItem>` data source.
**All components work with WebAssembly and Server hosted models**.
For code examples [see usage](https://github.com/majorimi/blazor-components/blob/master/src/Majorsoft.Blazor.Components.TestApps.Common/Components/GridDemo.razor).

You can try it out by using the [demo app](https://blazorextensions.z6.web.core.windows.net/grid).

Grid supports two usage "modes":
- **Basic**: just define columns with `Field` names, Grid renders a plain HTML table.
- **Advanced**: enable `AllowSorting` and `AllowPaging` features, use custom Header templates (e.g. with icons),
custom cell templates and custom styling.

# Components

- **`DataGrid<TItem>`**: renders customizable data Grid (table) with optional sorting, paging and styling.
- **`DataGridColumn<TItem>`**: defines a column of the `DataGrid` with data field, header, templates, sorting and styling settings.

## `DataGrid<TItem>` component

### Properties
- **`Items`: `IEnumerable<TItem>?` - Required** <br />
Data source of the Grid.
- **`ChildContent`: `RenderFragment` - Required** <br />
HTML content to define `DataGridColumn` columns.
- **`EmptyDataTemplate`: `RenderFragment?`** <br />
Optional HTML content rendered when the Grid has no data to show.
- **`AllowSorting`: `bool { get; set; }` (default: false)** <br />
Enables or disables column sorting for the whole Grid.
- **`AllowPaging`: `bool { get; set; }` (default: false)** <br />
Enables or disables paging of the Grid data.
- **`AllowColumnResize`: `bool { get; set; }` (default: false)** <br />
Enables or disables column resizing with native CSS `resize` for the whole Grid. Drag the handle at the bottom right corner of the column Header.
- **`PageSize`: `int { get; set; }` (default: 10)** <br />
Number of data rows to show per page when `AllowPaging` was enabled.
- **`CurrentPage`: `int { get; set; }` (default: 1)** <br />
Gets or sets the current page number (1 based). Supports `@bind-CurrentPage` two-way binding.
- **`ShowPagerInfo`: `bool { get; set; }` (default: true)** <br />
Determines whether the pager should show page info text or not.
- **`PagerButtonCount`: `int { get; set; }` (default: 5)** <br />
Number of page buttons to show in the pager.
- **`Striped`: `bool { get; set; }` (default: false)** <br />
Renders the Grid with striped rows using `StripeColor`.
- **`Bordered`: `bool { get; set; }` (default: true)** <br />
Renders the Grid with borders on all cells using `BorderColor`.
- **`Hoverable`: `bool { get; set; }` (default: true)** <br />
Enables row highlighting with `HoverColor` when mouse pointer is over a data row.
- **`HeaderBackgroundColor`: `string { get; set; }` (default: "WhiteSmoke")** <br />
Sets the `background-color` of the Grid Header. Use HTML specified: **Color Names**, **RGB**, **HEX** or with **HSL** values.
- **`HeaderTextColor`: `string { get; set; }` (default: "Black")** <br />
Sets the text `color` of the Grid Header.
- **`StripeColor`: `string { get; set; }` (default: "WhiteSmoke")** <br />
Sets the `background-color` of even data rows when `Striped` was enabled.
- **`HoverColor`: `string { get; set; }` (default: light blue)** <br />
Sets the `background-color` of hovered data row when `Hoverable` was enabled.
- **`BorderColor`: `string { get; set; }` (default: "LightGray")** <br />
Sets the `border-color` of the Grid when `Bordered` was enabled.
- **`TableCssClass`: `string? { get; set; }`** <br />
Custom CSS class(es) applied to the rendered `table` HTML element e.g. Bootstrap: "table table-sm".
- **`Columns`: `IEnumerable<DataGridColumn<TItem>> { get; }`** <br />
Returns all the column references added to the Grid.
- **`SortedColumn`: `DataGridColumn<TItem>? { get; }`** <br />
Returns the currently sorted column or NULL.
- **`SortDirection`: `SortDirections { get; }`** <br />
Returns the currently applied sort direction.
- **`TotalItemCount`: `int { get; }`** <br />
Returns the total number of data items in the Grid data source.
- **`PageCount`: `int { get; }`** <br />
Returns the total number of pages.
- **`InnerElementReference`: `ElementReference { get; }`** <br />
Exposes a Blazor `ElementReference` of the wrapped around HTML element.
- **`AllOtherAttributes`: `Dictionary<string, object>`** <br />
Blazor capture for any unmatched HTML attributes applied to the wrapper `div` element.

### Events
- **`OnRowClicked`: `EventCallback<TItem>`** <br />
Callback function called when a data row was clicked. Clicked data item is the callback parameter.
- **`OnSortChanged`: `EventCallback<DataGridSortEventArgs>`** <br />
Callback function called when Grid sorting was changed.
- **`CurrentPageChanged`: `EventCallback<int>`** <br />
Callback function called when the current page was changed.

### Functions
- **`SortByAsync(DataGridColumn<TItem> column, SortDirections sortDirection)`: `Task`** <br />
Sorts the Grid by the given column with the given direction.
- **`GoToPageAsync(int page)`: `Task`** <br />
Navigates the Grid to the given page number (1 based).
- **`Refresh()`: `void`** <br />
Re-renders the Grid. Can be used when the data source was changed externally.

## `DataGridColumn<TItem>` component

### Properties
- **`Field`: `string? { get; set; }`** <br />
Name of the data Field (public property of the data item Type) rendered to the cells. Also used as fallback for Header
text and for sorting when `SortKeySelector` was not provided.
- **`Title`: `string? { get; set; }`** <br />
Header text of the column. When not set `Field` name is used.
- **`HeaderTemplate`: `RenderFragment?`** <br />
Optional HTML content to render custom Header e.g. with icons. It overrides `Title` text.
- **`CellTemplate`: `RenderFragment<TItem>?`** <br />
Optional HTML content to render custom cell content with the current data item as context.
- **`Format`: `string? { get; set; }`** <br />
Standard .NET Format string applied to the cell value when `CellTemplate` was not provided e.g. "{0:d}", "{0:C2}", etc.
- **`Sortable`: `bool { get; set; }` (default: true)** <br />
Determines whether the column can be sorted or not. NOTE: it only works when `AllowSorting` was enabled on the parent `DataGrid`.
- **`SortKeySelector`: `Func<TItem, object?>?`** <br />
Optional Func to provide custom sort key for the column.
- **`Resizable`: `bool { get; set; }` (default: true)** <br />
Determines whether the column can be resized or not. NOTE: it only works when `AllowColumnResize` was enabled on the parent `DataGrid`.
- **`Hidden`: `bool { get; set; }` (default: false)** <br />
Determines whether the current rendered column should be hidden or not.
- **`Width`: `string? { get; set; }`** <br />
Sets the width of the column with CSS value e.g. "100px", "20%", "auto", etc.
- **`TextAlign`: `TextAligns { get; set; }` (default: Left)** <br />
Determines the text alignment of the rendered cells {Left, Center, Right}.
- **`HeaderCssClass`, `HeaderStyle`, `CellCssClass`, `CellStyle`: `string? { get; set; }`** <br />
Custom CSS class and style values applied to the column Header `th` and data cell `td` HTML elements.

# Configuration

## Installation

**Majorsoft.Blazor.Components.Grid** is available on [NuGet](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Grid/).

```sh
dotnet add package Majorsoft.Blazor.Components.Grid
```
Use the `--version` option to specify a [preview version](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Grid/absoluteLatest) to install.

## Usage

Add using statement to your Blazor `<component/page>.razor` file. Or globally reference it into `_Imports.razor` file.

```
@using Majorsoft.Blazor.Components.Grid
```

**Basic Grid:**

```
<DataGrid Items="_employees">
	<DataGridColumn TItem="Employee" Field="Name" />
	<DataGridColumn TItem="Employee" Field="Position" />
	<DataGridColumn TItem="Employee" Field="Salary" Format="{0:C0}" TextAlign="TextAligns.Right" />
</DataGrid>
```

**Advanced Grid** with sorting, paging, header icons, custom cell template and styling:

```
<DataGrid Items="_employees"
		  AllowSorting="true"
		  AllowPaging="true"
		  PageSize="5"
		  Striped="true"
		  HeaderBackgroundColor="navy"
		  HeaderTextColor="white"
		  OnRowClicked="e => _selected = e">
	<DataGridColumn TItem="Employee" Field="Name">
		<HeaderTemplate><i class="fa fa-user"></i> Name</HeaderTemplate>
	</DataGridColumn>
	<DataGridColumn TItem="Employee" Field="BirthDate" Title="Birth date" Format="{0:d}" />
	<DataGridColumn TItem="Employee" Title="Actions" Sortable="false">
		<CellTemplate Context="employee">
			<button class="btn btn-sm btn-primary" @onclick="() => Edit(employee)">Edit</button>
		</CellTemplate>
	</DataGridColumn>
</DataGrid>
```
