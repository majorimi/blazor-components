Blazor Loading Components
============
[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-build-check)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=6)
[![Package Version](https://img.shields.io/nuget/v/Majorsoft.Blazor.Components.Loading?label=Latest%20Version)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Loading/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Majorsoft.Blazor.Components.Loading?label=Downloads)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Loading/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/majorimi/blazor-components/blob/master/LICENSE)

# About

Blazor components that renders Overlay for page load. HTML `<button>` with customizable content for showing async operation in progress/loading state. **All components work with WebAssembly and Server hosted models**. For code examples [see usage](https://github.com/majorimi/blazor-components/blob/master/src/Blazor.Components.Tests.Common/Loading.razor).

You can try it out by using the [demo app](https://blazorextensions.z6.web.core.windows.net/loading).

# Features

## Components

- **`LoadingPage`**: Renders a `<div>` Overlay layer with customizable content for showing Page loading...
- **`LoadingButton`**: Renders HTML `<button>` with customizable content and loading content for showing async operation in progress/loading...

## LoadingPage component
Renders a `<div>` Overlay layer with customizable content for showing Page loading...

### Properties
- LoadingContent:
- OnLoading:
- OverlayBackgroundColor:

**Arbitrary HTML attributes e.g.: `id="load1"` will be passed to the corresponding rendered HTML element `<div>`**.

### Functions
- Set():
- Reset():

## LoadingButton component
Renders HTML `<button>` with customizable content and loading content for showing async operation in progress/loading...

### Properties
- Content:
- LoadingContent:
- OnClicked:
- Type:

**Arbitrary HTML attributes e.g.: `id="btn1" class="btn btn-primary"` will be passed to the corresponding rendered HTML element `<button>`**.

### Functions
- Set():
- Reset():

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

### LoadingPage usage

Following code example showw how to use **`LoadingPage`** component with 'automatic' mode in your Blazor App. When subcribed to `OnLoading` event the component will notify and call your registered method when 

```
<LoadingPage OnLoading="@SendRequest">
		<LoadingContent>
			<i class="fa fa-spinner fa-3x fa-spin"></i> 
			<h2 class="m-3">Loading...</h2>
		</LoadingContent>
	</LoadingPage>
  
@code {
  private async Task SendRequest()
	{
		await Task.Delay(1500); //write your code here... Component uses Try/Finally internally.
	}
}
```

Following code example showw how to use **`LoadingPage`** component with 'manual' mode in your Blazor App.

```
<LoadingPage @ref="_loadingPage" OverlayBackgroundColor="lightblue">
		<LoadingContent>
			<i class="fa fa-refresh fa-3x fa-spin"></i> 
			<h2 class="m-3">Refreshing...</h2>
		</LoadingContent>
	</LoadingPage>
  
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

### LoadingButton usage

```
<LoadingButton class="btn btn-primary" Type="ButtonTypes.Submit" OnClicked="@SendRequest">
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
