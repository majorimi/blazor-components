
Blazor Loading Components
============
[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-build-check)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=6)
[![Package Version](https://img.shields.io/nuget/v/Majorsoft.Blazor.Components.Loading?label=Latest%20Version)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Loading/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Majorsoft.Blazor.Components.Loading?label=Downloads)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Loading/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/majorimi/blazor-components/blob/master/LICENSE)

# About

Blazor components that renders Overlay for page load. HTML `<button>` with customizable content to show during async operation progress/loading state. 
**All components work with WebAssembly and Server hosted models**. 
For code examples [see usage](https://github.com/majorimi/blazor-components/blob/master/src/Blazor.Components.TestApps.Common/Components/Loading.razor).

You can try it out by using the [demo app](https://blazorextensions.z6.web.core.windows.net/loading).

# Components

- **`LoadingPage`**: Renders an  Overlay `<div>` layer with customizable background color and content for showing Page loading...
- **`LoadingButton`**: Renders a HTML `<button>` with customizable Content and LoadingContent for showing during async operation in progress/loading...

## `LoadingPage` component
Renders an  Overlay `<div>` layer with customizable background color and content for showing Page loading...
It is useful when you want to show 

### Properties
- **`LoadingContent`: `RenderFragment` HTML content - Required** <br />
Required HTML content to show on top of the overlay `<div>`.
- **`OnLoading`: `EventCallback` delegate** <br />
Function called when component `OnInitializedAsync` Blazor event triggered. 
Subscribe to this event and place your page initializer code to the event handler when using **'automatic'** mode. Otherwise can be omitted.
- **`OverlayBackgroundColor`: `string { get; set; }` (default: "gray")** <br />
Sets the `style` of the HTML `<div>` `background-color`. Use HTML specified: **Color Names**, **RGB** or with **HEX** values.
- **`OverlayOpacity`: `double OverlayOpacity { get; set; }` (default: 0.9)** <br />
Opacity of the overlay `<div>`. Value should be **between 0..1**. Where 0 means the overlay layer is not visible.

**Arbitrary HTML attributes e.g.: `id="load1"` will be passed to the corresponding rendered HTML element `<div>`**.

### Functions
- **`Set()`: `void Set()`** <br />
Sets the component to Loading state. Shows overlay `<div>` with specified content.
- **`Reset()`: `void Reset()`** <br />
Resets the component to the original state. Hides overlay `<div>`.

## `LoadingButton` component
Renders a HTML `<button>` with customizable Content and LoadingContent for showing during async operation in progress/loading...

### Properties
- **`Content`: `RenderFragment` HTML content - Required** <br />
Required HTML content to show as default `<button>` content.
- **`LoadingContent`: `RenderFragment` HTML content - Required** <br />
Required HTML content to show when operation is in progress `<button>` is in loading state.
- **`OnClicked`: `EventCallback` delegate** <br />
Function called when component `onclick` Blazor event triggered. 
Subscribe to this event and place your code to the event handler when using **'automatic'** mode. Otherwise can be omitted.
- **`DisabledWhenLoading`: `bool { get; set; }` (default: true)** <br />
Determines whether the button should be disabled during loading state or not.
- **`Type`: `ButtonTypes { get; set; }` enum (default: ButtonTypes.Button)** <br />
Intelisense supported type safe values to render HTML `<button>` `type=""` attribute.

**Arbitrary HTML attributes e.g.: `id="btn1" class="btn btn-primary"` will be passed to the corresponding rendered HTML element `<button>`**.

### Functions
- **`Set()`: `void Set()`** <br />
Sets the component to Loading state. Shows the specified `LoadingContent` and disables button, if configured to.
- **`Reset()`: `void Reset()`** <br />
Resets the component to the original state. Shows the specified default `Content` and enables button.

# Configuration

## Installation

Blazor.Components.Loading is available on [NuGet](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Loading/). 

```sh
dotnet add package Majorsoft.Blazor.Components.Loading
```
Use the `--version` option to specify a [preview version](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Loading/absoluteLatest) to install.

## Usage

Add using statement to your Blazor `<component/page>.razor` file. Or globally reference it into `_Imports.razor` file.
```
@using Blazor.Components.Loading
```

### `LoadingPage` usage

Following code example shows how to use **`LoadingPage`** component with **'automatic' mode** in your Blazor App. 

The component works by subscribing to `OnInitializedAsync` Blazor event. Usually all Blazor pages should subscribe to this event.
And fetch page content on that event handler e.g. by making async HTTP calls to a server.
You can skip that event registration when subscribed to `OnLoading` event of LoadingPage component. Just write your async method and place all initializer code here (in the code example `LoadPageData()`).

In this case when `LoadingPage` component got initialized it will show the overlay `<div>` with 
your specified `LoadingContent`. Component will call your `async` event handler and use `await` to wait for method execution. 
When your code run overlay will be hidden. 

All your code runs in Try/Finally block so you should not worry about 'infinity' loading state.

```
<LoadingPage OnLoading="@LoadPageData">
  <LoadingContent>
	<i class="fa fa-spinner fa-3x fa-spin"></i> 
	<h2 class="m-3">Loading...</h2>
  </LoadingContent>
</LoadingPage>
  
@code {
  private async Task LoadPageData()
  {
	await Task.Delay(1500); //write your code here... Component uses Try/Finally internally.
  }
}
```

Following code example shows how to use **`LoadingPage`** component with **'manual' mode** in your Blazor App.

In manual mode overlay won't be rendered on page load. **Do not subscribe to the component `OnLoading` event.**
Instead you have to show/hide overlay in your event handler e.g. `OnInitializedAsync`, `onclick`, etc.

Use `@ref=""` tag and declare `LoadingPage` type variable in your code with the given name in ref. 
And call `Set()` and `Reset()` functions in your handler (in code example `LoadForm()`).

**Important: you should put your code in Try/Finally block to avoid 'infinite' loading state in case of any errors.**

```
<LoadingPage @ref="_loadingPage" OverlayBackgroundColor="lightblue">
	<LoadingContent>
		<i class="fa fa-refresh fa-3x fa-spin"></i> 
		<h2 class="m-3">Refreshing...</h2>
	</LoadingContent>
</LoadingPage>

<button class="btn btn-primary" @onclick="LoadForm">Prompt loader...</button>

@code {
	//Use it when loader should be manually triggered otherwise use LoadingPage.OnLoading event.
	private LoadingPage _loadingPage;
	private async Task LoadForm()
	{
		try
		{
			_loadingPage.Set(); //Set the layout to Loading state

			await Task.Delay(1200); //write your code here...
		}
		finally
		{
			_loadingPage.Reset(); //Reset layout to default state in FINALLY block to avoid infinity loading state in case of any error!
		}
	}
}
```

### `LoadingButton` usage

Following code example shows how to use **`LoadingButton`** component with **'automatic' mode** in your Blazor App. 

The component works by subscribing to button `onclick` Blazor event. Component exposes this onclick via `OnClicked` event.
You have to subscribe to this event and specify the button default `Content` and `LoadingContent`.

In this case when `LoadingButton` component got clicked it will switch state and content. Also will be disabled if configured to. 
Component will call your `async` event handler and use `await` to wait for method execution. 
When your code run default content will be shown and button becomes enabled again.

All your code runs in Try/Finally block so you should not worry about 'infinity' loading state.

```
<LoadingButton class="btn btn-primary" Type="ButtonTypes.Button" OnClicked="@SendRequest">
		<Content>
			Fetch data
		</Content>
		<LoadingContent>
			<i class="fa fa-spinner fa-spin"></i> Loading...
		</LoadingContent>
	</LoadingButton>
  
@code {
	private async Task SendRequest()
	{
		await Task.Delay(1500); //write your code here... Component uses Try/Finally internally.
	}
}

```

Following code example shows how to use **`LoadingButton`** component with **'manual' mode** in your Blazor App. (It is the only option when using it in `EditForm`)

In manual mode button state and content won't be changed when clicked. **Do not subscribe to the component `OnClicked` event.**
Instead you have to switch button state in your event handler most probably in a form validation events  e.g. `OnValidSubmit`, etc.

Use `@ref=""` tag and declare `LoadingButton` type variable in your code with the given name in ref. 
And call `Set()` and `Reset()` functions in your handler (in code example `FormOnValidSubmit()`).


**Important: you should put your code in Try/Finally block to avoid 'infinite' loading state in case of any errors.**

```

<EditForm Model="@exampleModel" OnValidSubmit="@FormOnValidSubmit">
  <DataAnnotationsValidator />
  <ValidationSummary />

  <div class="pb-2">
	<InputText @bind-Value="exampleModel.Name" class="form-control w-25" />
  </div>
  <div class="pb-2">
	<LoadingButton @ref="_loadingButton" class="btn btn-secondary" Type="ButtonTypes.Submit">
	  <Content>
		Submit
	  </Content>
	  <LoadingContent>
		<i class="fa fa-circle-o-notch fa-spin"></i> Loading...
	  </LoadingContent>
	</LoadingButton>
  </div>
</EditForm>
  
@code {
  //Use it with EditForm otherwise use LoadingButton.OnClicked event.
	private LoadingButton _loadingButton;
	private async Task FormOnValidSubmit()
	{
		try
		{
			_loadingButton.Set(); //Set the button to Loading state

			await Task.Delay(1500); //write your code here...
		}
		finally
		{
			_loadingButton.Reset(); //Reset button to default state in FINALLY block to avoid infinity loading state in case of any error!
		}
	}

	//Form model
	private ExampleModel exampleModel = new ExampleModel();
	public class ExampleModel
	{
		[Required]
		public string Name { get; set; }
	}
}

```
