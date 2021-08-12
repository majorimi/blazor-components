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
- **`IHtmlNotificationService`**: Injectable `IHtmlNotificationService` service which is a **wrapper for Notification API** to handle HTML5 notifications and ServiceWorker Notifications with Custom actions handled by registered Service Worker.

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

**Arbitrary HTML attributes e.g.: `id="diag1"` will be passed to the corresponding rendered root HTML element `<div>`**.

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

**NOTE**: in order to show System notifications user Consent is required for the given Website per Browser. Also Operating System must allow Notification to show e.g.: Notification center Turned On, Focus assist allowing notifications to shown by the App in the given time, etc...

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

**In case of WebAssembly project register services in your `Program.cs` file:**
```
using Majorsoft.Blazor.Components.CssEvents;
...
public static async Task Main(string[] args)
{
	var builder = WebAssemblyHostBuilder.CreateDefault(args);

	//Register dependencies
	builder.Services.AddCssEvents();
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
}
```

### `ModalDialog` usage
Following code example shows how to use **`Alert`** component.

This example shows a simple dialog with Content message. **Blazor does not support empty content check.** Which means if you want to skip, remove **Header** and **Footer** 
you should not define it. **To achieve this do not place Header or Footer into the HTML markup.**
```

```



```


@code {

}
```