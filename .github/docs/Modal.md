Blazor Server Hosted model console logging
============
[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-build-check)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=6)
[![Package Version](https://img.shields.io/nuget/v/Majorsoft.Blazor.Components.Modal?label=Latest%20Version)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Modal/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Majorsoft.Blazor.Components.Modal?label=Downloads)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Modal/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/majorimi/blazor-components/blob/master/LICENSE)

# About

Blazor component that can be used for prompting Modal dialog window for lightboxes, user notifications or with fully customizable **Header**, **Content** and **Footer** parameterized Overlay, etc..
 **All components work with WebAssembly and Server hosted models**. 

You can try it out by using the [demo app](https://blazorextensions.z6.web.core.windows.net/modal).

# Components

- **`ModalDialog`**: renders Modal dialog window with fully customizable **Header**, **Content** and **Footer** and parameterized Overlay, etc...

## `ModalDialog` component

Modal dialogs are positioned over everything else in the document with optional Overlay (customizable opacity and color). 
Modal dialogs are removed from DOM when closed. **Only one modal window can be shown at a time.**
Dialog automatically (can be disabled) focuses itself to capture keyboard events when closed will refocus the last element.

### Properties
- **`Header`: `RenderFragment` HTML content** <br />
HTML content to show on the Modal header (top), right to the close button (if visible). Can be any valid HTML but should be only Title text. 
**Must not be defined if you want to leave it out. Also `ShowCloseButton` must be set to `false`**
- **`Content`: `RenderFragment` HTML content - Required** <br />
Required HTML content to show on the Modal dialog. Can be any valid HTML.
- **`Footer`: `RenderFragment` HTML content** <br />
HTML content to show on the Modal footer (bottom). Can be any valid HTML but should be only custom action buttons. 
**Must not be defined if you want to leave it out.**
- **`OverlayBackgroundColor`: `string { get; set; }` (default: "gray")** <br />
  Sets the `style` of the HTML `<div>` `background-color`. Use HTML specified: **Color Names**, **RGB** or with **HEX** values.
- **`OverlayOpacity`: `double { get; set; }` (default: 0.9)** <br />
Opacity of the overlay `<div>`. Value should be **between 0..1**. Where 0 means the overlay layer is not visible.
- **`CloseOnOverlayClick`: `bool { get; set; }` (default: true)** <br />
When `true` Modal dialog will be closed when Overlay (background) clicked. It works even if Overlay not visible (Opacity is set to 0)
- **`CloseOnEscapeKey`: `bool { get; set; }` (default: true)** <br />
When `true` Modal dialog will be closed when **Esc** (Escape) key pressed.
- **`Height`: `double { get; set; }` (default: 0)** <br />
Modal dialog window Height in **px** if set to **0** Height is set **auto**.
- **`Width`: `double { get; set; }` (default: 0)** <br />
Modal dialog window Width in **px** if set to **0** Width is set **auto**.
- **`MinHeight`: `double { get; set; }` (default: 200)** <br />
Modal dialog window minimum Height in **px**.
- **`MinWidth`: `double { get; set; }` (default: 200)** <br />
Modal dialog window minimum Width in **px**.
- **`Focus`: `bool { get; set; }` (default: true)** <br />
When `true` Modal dialog will automatically set focus to itself when it opens, and set it bact to the last focused element when it closes. 
In general this should never be set to false as it makes the Modal less accessible to screen-readers, etc.
- **`Animate`: `bool { get; set; }` (default: true)** <br />
When `true` Modal dialog will appear and disappear by using smooth CSS slide and fade transitions.
- **`Centered`: `bool { get; set; }` (default: false)** <br />
When `true` Modal dialog will be vertically centered, otherwise shown near to the top. Modal dialog horizontally always centered.
- **`ShowCloseButton`: `bool { get; set; }` (default: true)** <br />
 When `true` Modal dialog will show Header (even if Header is not defined) with closed **x** button.
- **`IsOpen`: `bool { get; }`** <br />
 Returns `true` if the Modal dialog is opened, otherwise `false`.

**Arbitrary HTML attributes e.g.: `id="diag1"` will be passed to the corresponding rendered root HTML element Overlay `<div>`**.

### Events
- **`OnOpen`: `EventCallback`** <br />
Callback function called when the Modal dialog is opening.
- **`OnClose`: `EventCallback`** <br />
Callback function called when the Modal dialog is closing.
- **`OnCloseButtonClicked`: `EventCallback<MouseEventArgs>`** <br />
Callback function called when close **x** button was clicked.
- **`OnOverlayClicked`: `EventCallback<MouseEventArgs>`** <br />
Callback function called when Overlay (background) was clicked.
- **`OnEscapeKeyPress`: `EventCallback<KeyboardEventArgs>`** <br />
Callback function called when **Esc** key was pressed.
- **`OnTransitionEnded`: `EventCallback<TransitionEventArgs[]>`** <br />
Callback function called when CSS transitions are ended. It will be triggered when dialog opened or closed.

### Functions
- **`Open()`: `Task Open()`** <br />
When method called Modal dialog will be opened. It should be `await`-ed.
- **`Close()`: `Task Close()`** <br />
When method called Modal dialog will be closed. It should be `await`-ed.
- **`DisposeAsync()`: `Task DisposeAsync()`** <br />
Component implements `IAsyncDisposable` interface Blazor framework will call it when parent removed from render tree.

# Configuration

## Installation

Blazor.Components.Modal is available on [NuGet](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Modal). 

```sh
dotnet add package Majorsoft.Blazor.Components.Modal
```
Use the `--version` option to specify a [preview version](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Modal/absoluteLatest) to install.

## Usage

Add using statement to your Blazor `<component/page>.razor` file. Or globally reference it into `_Imports.razor` file.

```
@using Blazor.Components.Modal
```

### Dependences
**Majorsoft.Blazor.Components.Modal** package depends on [Majorsoft.Blazor.Components.CssEvents](https://www.nuget.org/packages/Majorsoft.Blazor.Components.CssEvents)
which handles CSS Transition and Animation events for the dialog animation.

**In case of WebAssembly project register CSS events services in your `Program.cs` file:**
```
using Blazor.Components.CssEvents;
...
public static async Task Main(string[] args)
{
	var builder = WebAssemblyHostBuilder.CreateDefault(args);

	builder.Services.AddCssEvents();
}
```

**In case of Server hosted project register CSS events services in your `Startup.cs` file:**
```
using Blazor.Components.CssEvents;
...

public void ConfigureServices(IServiceCollection services)
{
	services.AddCssEvents();
}
```

### `ModalDialog` usage
Following code example shows how to use **`ModalDialog`** component. Most important is you have to **add `@ref=""` Blazor tag** to the component in order to access it in your code.

This example shows a simple dialog with Content message. **Blazor does not support empty content check.** Which means if you want to skip, remove **Header** and **Footer** 
you should not define it. **To achieve this do not place Header or Footer into the HTML markup.**
```
<button class="btn btn-primary mb-2" @onclick="@(() => _simpledialog1.Open())">Default dialog</button>

<ModalDialog @ref="_simpledialog1" MinHeight="50">
	<Content>
		Welcome to Blazor default ModalDialog...
	</Content>
</ModalDialog>

@code {
	private ModalDialog _simpledialog1;
}
```


This example shows a fully customized dialog with **Header**, **Content** and **Footer** sections.

```
<ModalDialog @ref="_dialog"
	OverlayBackgroundColor="@_overlayColor" 
	OverlayOpacity="@(_overlayOpacity/1000)"
	Height="@_modalHeight"
	Width="@_modalWitdth"
	MinHeight="@_modalMinHeight"
	MinWidth="@_modalMinWitdth"
	CloseOnOverlayClick="_modalCloseOnClick"
	CloseOnEscapeKey="_modalCloseOnEsc"
	Focus="_modalFocus"
	Animate="_modalAnimate"
	Centered="_modalCentered"
	ShowCloseButton="_modalShowClose"
	OnOpen="OnOpen"
	OnClose="OnClose"
	OnCloseButtonClicked="OnCloseButtonClicked"
	OnOverlayClicked="OnOverlayClicked"
	OnEscapeKeyPress="OnEscapeKeyPress"
	OnTransitionEnded="OnTransitionEnded">
	<Header>
		@*If you want to hide Header remove the whole Header definition and set ShowCloseButton="false"*@
		<h4>@_modalTitle</h4>
	</Header>
	<Content>
		<div class="container">
			<div class="row pb-2">
				@_modalText
			</div>
		</div>
	</Content>
	<Footer>
		@*If you want to hide Header remove the whole Footer definition"*@
		<button class="btn btn-warning ml-2" @onclick="CancelDialog">Cancel</button>
		<button class="btn btn-primary ml-2" @onclick="AcceptDialog">Ok</button>
	</Footer>
</ModalDialog>


@code {
	//Fully customized dialog
	private string _overlayColor = "128,128,128"; //gray
	private double _overlayOpacity = 0.5;
	private double _modalHeight = 270;
	private double _modalWitdth = 500;
	private double _modalMinHeight = 100;
	private double _modalMinWitdth = 100;
	private bool _modalAnimate = true;
	private bool _modalCloseOnClick = true;
	private bool _modalCloseOnEsc = true;
	private bool _modalFocus = true;
	private bool _modalCentered = true;
	private bool _modalShowClose = true;
	private string _modalTitle = "Modal title";
	private string _modalText = "Congratulations to your first modal!";

	private ModalDialog _dialog;
	private async Task OpenDialog()
	{
		await _dialog.Open();
	}

	private async Task AcceptDialog()
	{
		//Dialog accepted code
		await _dialog.Close();
	}
	private async Task CancelDialog()
	{
		//Dialog cancelled code
		await _dialog.Close();
	}

	//Dialog events
	public async Task OnOpen()
	{
		//Write your event handling code here...
	}
	public async Task OnClose()
	{
		//Write your event handling code here...
	}
	private async Task OnCloseButtonClicked(MouseEventArgs e)
	{
		//Write your event handling code here...
	}
	private async Task OnOverlayClicked(MouseEventArgs e)
	{
		//Write your event handling code here...
	}
	private async Task OnEscapeKeyPress(KeyboardEventArgs e)
	{
		//Write your event handling code here...
	}
	private async Task OnTransitionEnded(TransitionEventArgs[] e)
	{
		//Write your event handling code here...
	}
}
```