Blazor Components Analytics extension
============
[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-build-check)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=6)
[![Package Version](https://img.shields.io/nuget/v/Majorsoft.Blazor.Extensions.Analytics?label=Latest%20Version)](https://www.nuget.org/packages/Majorsoft.Blazor.Extensions.Analytics/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Majorsoft.Blazor.Extensions.Analytics?label=Downloads)](https://www.nuget.org/packages/Majorsoft.Blazor.Extensions.Analytics/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/majorimi/blazor-components/blob/master/LICENSE)

# About

Blazor extension that enables analytics services usage for Blazor applications e.g. Google Analytics, etc.  
**All components work with WebAssembly and Server hosted models** (Blazor server side configuration is different). 
For code examples [see usage](https://github.com/majorimi/blazor-components/blob/master/src/Majorsoft.Blazor.Components.TestApps.Common/Components/SiteAnalytics.razor).

You can try it out by using the [demo app](https://blazorextensions.z6.web.core.windows.net/analytics).

# Services

- **`Google Analytics`**: is a web analytics service offered by Google that tracks and reports website traffic, etc. inside the Google Marketing Platform.
`IGoogleAnalyticsService` is an injectable service for enabling [Google Analytics](https://support.google.com/analytics/answer/1008015?hl=en#) page tracking in Blazor Apps.
To make the initialization simple use `GoogleAnalyticsInitializer` component in your `MainLayout.razor` page and provide Google Analytics `TrackingId`.

## `Google Analytics` extension
This is a JS wrapper for web analytics service offered by Google that tracks and reports website traffic, etc. inside the Google Marketing Platform.

### `GoogleAnalyticsInitializer` component
A convenient wrapper component for `IGoogleAnalyticsService` to make Google Analytics initialize simple.

#### Properties
- **`TrackingId`**: **`string TrackingId { get; set; }` - Required**  <br />
Google Analytics TrackingId provided on Google Analytics manage page.

### `IGoogleAnalyticsService` service
Injectable service to handle Google analytics `gtag.js`.

#### Properties
- **`TrackingId`**: **`string TrackingId { get; set; }` - Required**  <br />
Google analytics uniquely Id which was used in `InitializeAsync(string)` method.

#### Functions
- **`InitializeAsync()`**: **`ValueTask InitializeAsync(string trackingId)`** <br />
Initialize Google analytics by registering gtag.js to the HTML document. **Should be called once.
 Do not call this method if you used `GoogleAnalyticsInitializer`.**
- **`ConfigAsync()`**: **`ValueTask ConfigAsync(string trackingId = "", ExpandoObject? configInfo = null)`** <br />
Allows you to add additional configuration information to targets. This is typically product-specific configuration for a product
such as Google Ads or Google Analytics.
- **`GetAsync()`**: **``** <br />
Allows you to get various values from gtag.js including values set with the set command.
- **`SetAsync()`**: **`ValueTask SetAsync(ExpandoObject parameters)`** <br />
 Allows you to set values that persist across all the subsequent gtag() calls on the page.
- **`EventAsync()`**: **`ValueTask EventAsync(GoogleAnalyticsEventTypes eventType, ExpandoObject eventParams)`** <br />
Use the event command to send event data.
- **`CustomEventAsync()`**: **`ValueTask CustomEventAsync(string customEventName, GoogleAnalyticsCustomEventArgs eventData)`** <br /> 
Use the event command to send custom event data.

# Configuration

## Installation

**Majorsoft.Blazor.Extensions.Analytics** is available on [NuGet](https://www.nuget.org/packages/Majorsoft.Blazor.Extensions.Analytics/). 

```sh
dotnet add package Majorsoft.Blazor.Extensions.Analytics
```
Use the `--version` option to specify a [preview version](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Extensions/absoluteLatest) to install.

## Usage

Add using statement to your Blazor <component/page>.razor file. Or globally reference it into `_Imports.razor` file.
```
@using Majorsoft.Blazor.Extensions.Analytics
@*Google Analytics*@
@using Majorsoft.Blazor.Extensions.Analytics.Google
```

#### WebAssembly projects

**In case of WebAssembly project register services in your `Program.cs` file:**
```
using Majorsoft.Blazor.Extensions.Analytics;
...
public static async Task Main(string[] args)
{
	var builder = WebAssemblyHostBuilder.CreateDefault(args);

	//Register service
	builder.Services.AddGoogleAnalytics();
}
```

**Use `MainLayout.razor` file for WebAssembly project to initialize Google gtag.js only once:**

```
@*Google Analytics initialize*@
<GoogleAnalyticsInitializer TrackingId="<TrackingId here...>" />
```

#### Server hosted projects
**In case of Server hosted project register dependency services in your `Startup.cs` file:**

```
@using Majorsoft.Blazor.Extensions.Analytics
...

public void ConfigureServices(IServiceCollection services)
{
	//Register service
	services.AddGoogleAnalytics();
}
```

**Use `MainLayout.razor` file for WebAssembly project to initialize Google gtag.js only once:**
```
@*Google Analytics initialize*@
<GoogleAnalyticsInitializer TrackingId="<TrackingId here...>" />
```

#### Using `IGoogleAnalyticsService` service

Following code example shows how to Set and Get custom values. Also shows sending events and custom events. 
For full features supported by Google Analytics please see [docs page](https://developers.google.com/gtagjs/reference/api).

```
@using System.Dynamic
@inject IGoogleAnalyticsService _googleAnalytincsService
@implements IAsyncDisposable

@code {
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if(firstRender)
		{
			await _googleAnalytincsService.GetAsync("session_id", async (res) => { _logger.LogInformation($"Google analytics Get result: {res}"); });

			//Custom set
			dynamic exp = new ExpandoObject();
			exp.test = 27;

			await _googleAnalytincsService.SetAsync(exp);

			//Get cutoms set value
			await _googleAnalytincsService.GetAsync("test", async (res) => { _logger.LogInformation($"Google analytics Get result: {res}"); });

			//Built in Search event usage
			dynamic exp2 = new ExpandoObject();
			exp2.search_term = "Searching custom event...";
			await _googleAnalytincsService.EventAsync(GoogleAnalyticsEventTypes.search, exp2);

			//Custom event usage
			await _googleAnalytincsService.CustomEventAsync("testEvent", new GoogleAnalyticsCustomEventArgs()
			{
				Action = "Test action",
				Category = "Test category",
				Label = "Test label",
				Value = 1234
			});


	public async ValueTask DisposeAsync()
	{
		if(_googleAnalytincsService is not null)
		{
			await _googleAnalytincsService.DisposeAsync();
		}
	}
}
```
