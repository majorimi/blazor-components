Blazor Components Notification controls and HTML 5 service
============
[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-build-check)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=6)
[![Package Version](https://img.shields.io/nuget/v/Majorsoft.Blazor.Components.Notifications?label=Latest%20Version)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Notifications/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Majorsoft.Blazor.Components.Notifications?label=Downloads)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Notifications/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/majorimi/blazor-components/blob/master/LICENSE)

# About

Blazor injectable `INotificationService` service to handle `HTML5 Notifications` and `ServiceWorker` Notifications and components that renders **customizable** `Alert` and `Toast` notification message elements.

**All components work with WebAssembly and Server hosted models**. 
For code examples [see usage](https://github.com/majorimi/blazor-components/blob/master/src/Majorsoft.Blazor.Components.TestApps.Common/Components/Notifications.razor).

You can try it out by using the [demo app](https://blazorextensions.z6.web.core.windows.net/Notifications).

# Components and services

- **`Alert`**: Renders `Alert` component which **is a banner to show important application messages**. Importance can be emphasized by Type and NotificationStyle styling with customizable content.
- **`ToastContainer` and `Toast`**: Renders single `ToastContainer` **component per application** which can be placed to 6 different places of a page. With injectable `IToastService` service individual `Toast` components can be added, removed or events tracked. By using Global settings or override all values per Toast notification.
- **`IHtmlNotificationService`**: Injectable `IHtmlNotificationService` service which is a **wrapper for [Notification API](https://developer.mozilla.org/en-US/docs/Web/API/notification)** to handle HTML5 notifications and ServiceWorker Notifications with Custom actions handled by registered Service Worker.

## `Alert` component (See [demo app](https://blazorextensions.z6.web.core.windows.net/notifications#alerts))
Renders `Alert` component which is a banner to show important application messages. 
Importance can be emphasized by `Type` and `NotificationStyle` styling with customizable content. 
Alerts can close itself automatically or let user close them, etc.

![Alert demo](https://github.com/majorimi/blazor-components-docs/raw/main/github/docs/gifs/Notification_Alert.gif)

### Properties
- **`IsVisible`: `bool { get; set; }` (default: false)** <br />
Determines weather the Alert message should be visible on UI or not.
- **`Content`: `RenderFragment` HTML content - Required** <br />
Required HTML Content of the Alert notification. Can be any valid HTML.
- **`Type`: `NotificationTypes { get; set; }` (default: NotificationTypes.Primary)** <br />
Notification type or severity level.
- **`NotificationStyle`: `NotificationStyles { get; set; }` (default: NotificationStyles.Normal)** <br />
Notification style to show different variant of the same `Type` of `Alert`.
- **`ShowIcon`: `bool { get; set; }` (default: true)** <br />
 When true Alert will show an icon corresponding to the <see cref="NotificationTypes"/>. Default icon can be overwritten.
- **`CustomIconSvgPath`: `string { get; set; }` (default: "")** <br />
Icon customization it accepts an SVG `Path` value to override the default icon. When empty or NULL it is omitted and default used.
- **`ShowCloseButton`: `bool { get; set; }` (default: true)** <br />
 When `true` Alert will show close "x" button.

**Arbitrary HTML attributes e.g.: `id="alert1"` will be passed to the corresponding rendered root HTML element `<div>`**.

### Events
- **`OnShow`: `EventCallback`** <br />
Callback function called when the `Alert` is showing.
- **`OnClose`: `EventCallback`** <br />
Callback function called when the `Alert` is closing.
- **`OnCloseButtonClicked`: `EventCallback<MouseEventArgs>`** <br />
Callback function called when close **x** button was clicked.
- **`IsVisibleChanged`: `EventCallback<bool>`** <br />
Callback function for two way bindings of `IsVisible` property.

### Functions
- **`DisposeAsync()`: `Task DisposeAsync()`** <br />
Component implements `IAsyncDisposable` interface Blazor framework will call it when parent removed from render tree.

## `ToastContainer` and `Toast` components (See [demo app](https://blazorextensions.z6.web.core.windows.net/notifications#Toasts))
Renders single **`ToastContainer` component per application (place it to a global component after all HTML elements to be able to work with `position: relative;` elements)** 
which can be placed to 6 different places of a page. With **injectable `IToastService` service** individual Toast components can be added, removed or events tracked. 
By using **Global settings or override all values per `Toast` notification**. Importance can be emphasized by `Type` and `NotificationStyle` styling with customizable content.
<br />All Toast components will close itself automatically when given timeout elapsed.

![Toast demo](https://github.com/majorimi/blazor-components-docs/raw/main/github/docs/gifs/Notification_Toast.gif)

### ToastContainer
As the name suggests it is a container element for individual `Toast` notifications. Only 1 container per application is allowed.**
**Place it to a global (Layout) component after all HTML elements to be able to work with `position: relative;` elements**

#### Properties

#### Events

#### Functions

### IToastService

#### Properties

#### Events

#### Functions
- **`ShowToast()`: `Guid ShowToast(string message, NotificationTypes notificationType = NotificationTypes.Info, NotificationStyles? notificationStyle = null)`** <br />
Service implements `IDisposable` interface.
- **`ShowToast()`: `Guid ShowToast(RenderFragment content, NotificationTypes notificationType = NotificationTypes.Info, NotificationStyles? notificationStyle = null)`** <br />
Service implements `IDisposable` interface.
- **`ShowToast()`: `Guid ShowToast(ToastSettings toastSettings)`** <br />
Shows a new Toast notification with given `ToastSettings`. It can be override all default values.
- **`RemoveToast()`: `void RemoveToast(Guid id)`** <br />
Removes a `Toast` notification from collection and UI with given Id.
- **`ClearAll()`: `void ClearAll()`** <br />
Clears `Toast`s collections and Removes all Toast notifications from UI.
- **`Dispose()`: `void Dispose()`** <br />
Service implements `IDisposable` interface.

### ToastSettings

#### Properties

#### Events

#### Functions

### Toast
An individual `Toast` showed in order of creation inside the `ToastContainer`. <br />
**This component should not be accessed, created or manipulated directly! Please use injected `IToastService` instance `ShowToast()` methods.**
Properties are not explained see `IToastService`, `ToastContainer` and `ToastSettings` above.

## `IHtmlNotificationService` service (See [demo app](https://blazorextensions.z6.web.core.windows.net/notifications#htmlnotification))
Injectable `IHtmlNotificationService` service which is a **wrapper for Notification API** to handle `HTML5 notifications` and `ServiceWorker` Notifications with Custom actions handled by registered Service Worker.

**NOTE**: *in order to show system notifications user consent is required for the given website per browser. also operating system must allow notification to show e.g.: notification center turned on, focus assist allowing notifications to shown by the app in the given time, etc...*

![HTML5 Notification demo](https://github.com/majorimi/blazor-components-docs/raw/main/github/docs/gifs/Notification_htmlnotification.gif)

### Functions
- **`RequestPermissionAsync()`: `ValueTask RequestPermissionAsync(Func<HtmlNotificationPermissionTypes, Task> callback)`** <br />
Prompts User Consent permission request popup to the User to decide whether to allow Notifications or not.
- **`CheckPermissionAsync()`: `ValueTask<HtmlNotificationPermissionTypes> CheckPermissionAsync()`** <br />
Checks the Notification User Consent status.
- **`CheckMaxActionsAsync()`: `ValueTask<int> CheckMaxActionsAsync()`** <br />
Returns maxActions attribute of the Notification interface returns the maximum number of actions supported by the device and the User Agent. 
- **`IsBrowserSupportedAsync()`: `ValueTask<bool> IsBrowserSupportedAsync()`** <br />
Checks if Browser supports HTML Notifications API or not.
- **`ShowsAsync()`: `ValueTask<Guid> ShowsAsync(HtmlNotificationOptions notificationOptions)`** <br />
Shows a Notification with the given `HtmlNotificationOptions` options data.
- **`ShowsWithActionsAsync()`: `ValueTask ShowsWithActionsAsync(HtmlServiceWorkerNotificationOptions notificationOptions)`** <br />
Registers the given `ServiceWorker` from the URL and prompts Notifications with Options provided.
**ServiceWorker can handle Custom events with custom data provided.**
- **`DisposeAsync()`: `Task DisposeAsync()`** <br />
Service implements `IAsyncDisposable` interface.

# Configuration

## Installation

**Majorsoft.Blazor.Components.Notifications** is available on [NuGet](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Notifications). 

```sh
dotnet add package Majorsoft.Blazor.Components.Notifications
```
Use the `--version` option to specify a [preview version](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Notifications/absoluteLatest) to install.

## Usage

Add using statement to your Blazor `<component/page>.razor` file. Or globally reference it into `_Imports.razor` file.

```
@using Majorsoft.Blazor.Components.Notifications
```

### Dependences
**Majorsoft.Blazor.Components.Notifications** package depends on other Majorsoft Nuget packages:
- [Majorsoft.Blazor.Components.CssEvents](https://www.nuget.org/packages/Majorsoft.Blazor.Components.CssEvents)
which handles CSS Transition and Animation events for the dialog animation.
- [Majorsoft.Blazor.Components.Timer](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Timer)
which handles Auto close timing.

### Register services
**In case of WebAssembly project register services in your `Program.cs` file:**
```
using Majorsoft.Blazor.Components.CssEvents;
...
public static async Task Main(string[] args)
{
	var builder = WebAssemblyHostBuilder.CreateDefault(args);

	//Register dependencies
	builder.Services.AddCssEvents();
	builder.Services.AddNotifications();
}
```

**In case of Server hosted project register services in your `Startup.cs` file:**
```
using Majorsoft.Blazor.Components.CssEvents;
...

public void ConfigureServices(IServiceCollection services)
{
	//Register dependencies
	services.AddCssEvents();
	services.AddNotifications();
}
```

### `Alert` usage
Following code example shows how to use **`Alert`** component.

`Alert` component must be placed to HTML DOM it cannot be added dynamically. But Alert can be hidden. Content of the alert with many more
settings can be customized. See properties for more details.

```
<Alert Type="@_alertTypeLevel"
	NotificationStyle="@_alertStyle"
	@bind-IsVisible="_alertIsVisible"
	AutoClose="@_alertAutoClose"
	AutoCloseInSec="@_alertAutoCloseInSec"
	ShowCloseButton="@_alertShowCloseButton"
	ShowIcon="@_alertShowIcon"
	ShadowEffect="@_alertShadowEffect"
	ShowCloseCountdownProgress="@_alertShowCountdownProgress"
	CustomIconSvgPath="@_alertCustomSvg"
	OnShow="OnShow"
	OnClose="OnClose"
	OnCloseButtonClicked="OnCloseButtonClicked">
	<Content>
		@((MarkupString)_alertText)
	</Content>
</Alert>

@code {
	private bool _alertIsVisible = false;
	private bool _alertAutoClose = true;
	private bool _alertShowIcon = true;
	private bool _alertShowCloseButton = true;
	private bool _alertShowCountdownProgress = true;
	private uint _alertAutoCloseInSec = 5;
	private uint _alertShadowEffect = 0;
	private string _alertText = $@"<strong>Alert:</strong> This is a(n) {nameof(NotificationTypes.Primary)} alert...";
	private string _alertCustomSvg;
	private NotificationTypes _alertTypeLevel = NotificationTypes.Primary;
	private NotificationStyles _alertStyle = NotificationStyles.Normal;

	//Alert events
	public async Task OnShow()
	{
		//TODO: handle Alert event
	}
	public async Task OnClose()
	{
		//TODO: handle Alert event
	}
	private async Task OnCloseButtonClicked(MouseEventArgs e)
	{
		//TODO: handle Alert event
	}
}
```

### `ToastContainer` and `Toast` usage
Following code example shows how to use **`ToastContainer` and `Toast`** component.

```


@code {

}
```

### `IHtmlNotificationService` usage
Following code example shows how to use **`IHtmlNotificationService`** service.

**NOTE**: *in order to show System notifications user Consent is required for the given Website per Browser. 
Also Operating System must allow Notification to show e.g.: Notification center Turned On, Focus assist allowing notifications to shown by the App in the given time, etc...*

This example shows how to **check HTML notification is supported by Browser and ask User consent (permission) to prompt HTML notification**:
```
HTML5 Notification supported: @_notificationSupported
Notification user consent: @_htmlNotificationPermission

@if (_htmlNotificationPermission != HtmlNotificationPermissionTypes.Granted)
{
	<button class="btn btn-primary" @onclick="RequestPermission">Request user Permission</button>
}


@inject IHtmlNotificationService _notificationService
@implements IAsyncDisposable

@code {
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			_notificationSupported = await _notificationService.IsBrowserSupportedAsync();
			_htmlNotificationPermission = await _notificationService.CheckPermissionAsync();
			StateHasChanged();
		}
	}

	//HTML5 notification
	private bool _notificationSupported;
	private HtmlNotificationPermissionTypes _htmlNotificationPermission;

	private async Task RequestPermission()
	{
		await _notificationService.RequestPermissionAsync(async res =>
		{
			_htmlNotificationPermission = res;
			StateHasChanged();
		});
	}

	public async ValueTask DisposeAsync()
	{
		if (_notificationService is not null)
		{
			await _notificationService.DisposeAsync();
		}
	}
```

This example shows how to prompt "simple" (**without Custom Actions with callbacks**) HTML notification. In this case Notification events are available 
(**Important NOTE: events are "non reliable" e.g. same events might not supported by some OS, or when Notification auto dismissed and later "clicked" in "Action Center" meanwhile your application already closed**).
```
@inject IHtmlNotificationService _notificationService
@implements IAsyncDisposable

@code {
	//HTML5 notification
	private string _notificationTitle = "Notification Title";
	private string _notificationBody = "Notification Body";
	private string _notificationIcon = "blazor.components.png";

	private async Task ShowSimpeNotification()
	{
		var options = new HtmlNotificationOptions(_notificationTitle)
		{
			Body = _notificationBody,
			Icon = _notificationIcon,
			Vibrate = new int[] { 100, 200, 100},
			//events
			OnOpenCallback = OnOpen,
			OnCloseCallback = OnClose,
			OnErrorCallback = OnError,
			OnClickCallback = OnClick,
		};

		var id = await _notificationService.ShowsAsync(options);
	}

	public async Task OnOpen(Guid id)
	{
		//TODO: handle Alert event
	}
	public async Task OnClose(Guid id)
	{
		//TODO: handle Alert event
	}
	public async Task OnError(Guid id)
	{
		//TODO: handle Alert event
	}
	public async Task OnClick(Guid id)
	{
		//TODO: handle Alert event
	}

	public async ValueTask DisposeAsync()
	{
		if (_notificationService is not null)
		{
			await _notificationService.DisposeAsync();
		}
	}
}
```

This example shows how to prompt "simple" (**Custom Actions with callbacks are possible with registered [Service Worker](https://developer.mozilla.org/en-US/docs/Web/API/ServiceWorkerRegistration/showNotification)**) 
HTML notification.
```
@inject IHtmlNotificationService _notificationService
@implements IAsyncDisposable

@code {
	private string _notificationTitle = "Notification Title";
	private string _notificationBody = "Notification Body";
	private string _notificationIcon = "blazor.components.png";

	private async Task ShowWithActionsNotification()
	{
		var options = new HtmlServiceWorkerNotificationOptions(_notificationTitle, "_content/Majorsoft.Blazor.Components.TestApps.Common/sw.js")
		{
			Body = _notificationBody,
			Icon = _notificationIcon,
			//actions
			Actions = new NotificationAction[]
			{
			   new NotificationAction() { Action = "action1", Title = "Custom action"},
			   new NotificationAction() { Action = "action2", Title = "Other action"},
			},
			Vibrate = new int[] { 100, 200, 100 },
			Data = "any type of object.."
		};

		await _notificationService.ShowsWithActionsAsync(options);
	}

	public async ValueTask DisposeAsync()
	{
		if (_notificationService is not null)
		{
			await _notificationService.DisposeAsync();
		}
	}
}
```