Blazor Server Hosted model console logging
============
[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-build-check)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=6)
[![Package Version](https://img.shields.io/nuget/v/Majorsoft.Blazor.Components.CssEvents?label=Latest%20Version)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.CssEvents/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Majorsoft.Blazor.Components.CssEvents?label=Downloads)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.CssEvents/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/majorimi/blazor-components/blob/master/LICENSE)

# About

Blazor Extensions and Components wrapper to notify on CSS **Transition** and **Animation** events.
This is useful when you want to wait for a CSS **Transition** or **Animation** to finish and then continue run C# code, e.g.: hide the element, etc.
 **All components work with WebAssembly and Server hosted models**. 

You can try it out by using the [demo app](https://blazorextensions.z6.web.core.windows.net/cssevents).

# Components and Services

- **`AnimationElement`**: Convenient Blazor component which uses `IAnimationEventsService` to wrap around the given Content and listens for all or one animation Name.
- **`IAnimationEventsService`**: : Low level injectable service which has more features e.g. can aggregate multiple events from multiple HTML elements but must be Disposed manually.
- **`TransitionElement`**:  Convenient Blazor component which uses `ITransitionEventsService` to wrap around the given Content and listens for all or one event property.
- **`ITransitionEventsService`**: Low level injectable service which has more features e.g. can aggregate multiple events from multiple HTML elements but must be Disposed manually.

## CSS Animation events
Blazor Extension and Component wrapper to notify Blazor apps on CSS supported Animation events: `animationstart`, `animationiteration`, `animationend`. 
This is useful when you want to wait for a CSS Animations to finish/restart, etc. and then continue run C# code, e.g.: hide the element, etc.

### `AnimationElement` component
Convenient Blazor component which uses `IAnimationEventsService` to wrap around the given Content and listens for all or one event by animation name.
It can listen to all animations or can have only a single name filter via: `AnimationName`. Default is empty so all animation events captured.
In case of nested content with animations. All events will bubble up to `AnimationElement` component and event callbacks will be called on each element's animation events.

#### Properties
- **`Content`: `RenderFragment` HTML content - Required** <br />
Required HTML content which will be wrapped into a div which has the Transition event listener registered.
- **`AnimationName`: `string { get; set; }` (default: "")** <br />
Animation name `@keyframe <name>` to filter events. Default is empty ("") which means all events will be captured.

**Arbitrary HTML attributes e.g.: `id="load1"` will be passed to the corresponding rendered HTML wrapper element `<div>`**.

#### Events
- **`OnAnimationStart`: `EventCallback<AnimationEventArgs>` delegate** <br />
Callback function called when component corresponding element animation with given name filer (empty string will not filter) has started. 
`AnimationEventArgs` describes the event's parameters.
- **`OnAnimationIteration`: `EventCallback<AnimationEventArgs>` delegate** <br />
Callback function called when component corresponding element animation with given name filer (empty string will not filter) has 'restarted' new iteration started. 
`AnimationEventArgs` describes the event's parameters.
- **`OnAnimationEnded`: `EventCallback<AnimationEventArgs>` delegate** <br />
Callback function called when component corresponding element animation with given name filer (empty string will not filter) has ended. 
`AnimationEventArgs` describes the event's parameters.

#### Functions
- **`DisposeAsync()`: `Task DisposeAsync()`** <br />
Component implements `IAsyncDisposable` interface **your component also should implement it and call `ITransitionEventsService` instance DisposeAsync**.

### `IAnimationEventsService` extension
Low level injectable service which has more features e.g. can aggregate multiple events from multiple HTML elements but must be DisposeAsync() manually.
In case of registering a top level element which has nested content with transitions. One element can be registered multiple times with different transition property names.
All events will bubble up to `TransitionElement` component and event callback will be called on each element's transition event.

#### Functions
- **`RegisterAnimationStartedAsync()`: `Task RegisterAnimationStartedAsync(ElementReference elementRef, Func<AnimationEventArgs, Task> onStartedCallback, string animationName = "")`** <br />
Adds event listener for `animationstart` HTML event for the given element with Animation name filter.
Async function it takes an element (use name of `@ref=""` value) and a `Func` as event callback. Animation name filter can be specified.
- **`RemoveAnimationStartedAsync()`: `Task RemoveAnimationStartedAsync(ElementReference elementRef, string animationName = "")`** <br />
Removes event listener for `animationstart` HTML event for the given element with Animation name filter.
- **`RegisterAnimationIterationAsync()`: `Task RegisterAnimationIterationAsync(ElementReference elementRef, Func<AnimationEventArgs, Task> onIterationCallback, string animationName = "")`** <br />
Adds event listener for `animationiteration` HTML event for the given element with Animation name filter.
- **`RemoveAnimationIterationAsync()`: `Task RemoveAnimationIterationAsync(ElementReference elementRef, string animationName = "")`** <br />
Removes event listener for `animationiteration` HTML event for the given element with Animation name filter.
- **`RegisterAnimationEndedAsync()`: `Task RegisterAnimationEndedAsync(ElementReference elementRef, Func<AnimationEventArgs, Task> onEndedCallback, string animationName = "")`** <br />
Adds event listener for `animationend` HTML event for the given element with Animation name filter.
- **`RemoveAnimationEndedAsync()`: `Task RemoveAnimationEndedAsync(ElementReference elementRef, string animationName = "")`** <br />
Removes event listener for `animationend` HTML event for the given element with Animation name filter.
- **`RegisterAllAnimationEventsAsync()`: `Task RegisterAllAnimationEventsAsync(ElementReference elementRef, Func<AnimationEventArgs, Task> onEventCallback, string animationName = "")`** <br />
Adds event listeners with single callback for all supported HTML events for the given element with Animation name filter.
- **`RegisterAllAnimationEventsAsync()`: `Task RegisterAllAnimationEventsAsync(ElementReference elementRef, Func<AnimationEventArgs, Task> onStartedCallback, Func<AnimationEventArgs, Task> onIterationCallback, Func<AnimationEventArgs, Task> onEndedCallback, string animationName = "")`** <br />
Adds event listeners with different callbacks for all supported HTML events for the given element with Animation name filter.
- **`RemoveAllAnimationEventsAsync()`: `Task RemoveAllAnimationEventsAsync(ElementReference elementRef, string animationName = "")`** <br />
Removes event listener for all supported HTML event for the given element with Animation name filter.
- **`RegisterAnimationsWhenAllEndedAsync()`: `Task RegisterAnimationsWhenAllEndedAsync(Func<AnimationEventArgs[], Task> onEndedCallback, params KeyValuePair<ElementReference, string>[] elementRefsWithProperties)`** <br />
Adds event listeners for `animationend` HTML event for the given elements with Animation names filters.
- **`RemoveAnimationsWhenAllEndedAsync()`: `Task RemoveAnimationsWhenAllEndedAsync(params KeyValuePair<ElementReference, string>[] elementRefsWithProperties)`** <br />
Removes event listeners for `animationend` HTML event for the given elements with Animation names filters.
- **`DisposeAsync()`: `Task DisposeAsync()`** <br />
Component implements `IAsyncDisposable` interface **your component also should implement it and call `ITransitionEventsService` instance DisposeAsync**.

### `AnimationEventArgs` event arguments
`TransitionEventArgs` is a new event argument for Animation events.

```
public sealed class AnimationEventArgs : EventArgs
{
	public ElementReference Element { get; set; }
	public string OriginalAnimationNameFilter { get; set; }
	public string AnimationName { get; set; }

	public bool Composed { get; set; }
	public double ElapsedTime { get; set; }
	public int EventPhase { get; set; }
	public bool ReturnValue { get; set; }
	public string Type { get; set; }
}
```

## CSS Transition events
Blazor Extension and Component wrapper to notify Blazor apps on CSS supported Transition event: `transitionend`. 
This is useful when you want to wait for a CSS Transition to finish and then continue run C# code, e.g.: hide the element, etc.

### `TransitionElement` component
Convenient Blazor component which uses `ITransitionEventsService` to wrap around the given Content and listens for all or one event property.
It can listen to all transitions properties or can have only a single property name filter via: `TransitionPropertyName`. Default is empty so all transition events captured.
In case of nested content with transitions. All events will bubble up to `TransitionElement` component and event callbacks will be called on each element's transition events.

#### Properties
- **`Content`: `RenderFragment` HTML content - Required** <br />
Required HTML content which will be wrapped into a div which has the Transition event listener registered.
- **`TransitionPropertyName`: `string { get; set; }` (default: "")** <br />
Transition property name to filter events e.g. `opacity`, `top`, etc. Default is empty ("") which means all events will be captured.

**Arbitrary HTML attributes e.g.: `id="load1"` will be passed to the corresponding rendered HTML wrapper element `<div>`**.

#### Events
- **`OnTransitionEnded`: `EventCallback<TransitionEventArgs>` delegate** <br />
Callback function called when component corresponding element transition with given property filer (empty string will not filter) has finished. 
`TransitionEventArgs` describes the event's parameters.

#### Functions
- **`DisposeAsync()`: `async Task DisposeAsync()`** <br />
Component implements `IAsyncDisposable` interface Blazor framework will call it when parent removed from render tree.

### `ITransitionEventsService` extension
Low level injectable service which has more features e.g. can aggregate multiple events from multiple HTML elements but must be DisposeAsync() manually.
In case of registering a top level element which has nested content with transitions. One element can be registered multiple times with different transition property names.
All events will bubble up to `TransitionElement` component and event callback will be called on each element's transition event.

#### Functions
- **`RegisterTransitionEndedAsync()`: `Task RegisterTransitionEndedAsync(ElementReference elementRef, Func<TransitionEventArgs, Task> onEndedCallback, string transitionPropertyName = "")`**
Adds event listener for `transitionend` HTML event for the given element with property filter. 
Async function it takes an element (use name of `@ref=""` value) and a `Func` as event callback. Transition property filter can be specified.
- **`RegisterTransitionsWhenAllEndedAsync()`: `Task RegisterTransitionsWhenAllEndedAsync(Func<TransitionEventArgs[], Task> onEndedCallback, params KeyValuePair<ElementReference, string>[] elementRefsWithProperties)`**
Adds event listeners for `transitionend` HTML event for the given elements with property filters.
Async function it takes an array of elements (use name of `@ref=""` value) with optional transition property filters and a single `Func` as aggregated event callback.
- **`RemoveTransitionEndedAsync()`: `Task RemoveTransitionEndedAsync(ElementReference elementRef, string transitionPropertyName = "")`**
Removes event listener for `transitionend` HTML event for the given element with property filter.
Async function it takes an element (use name of `@ref=""` value) and optional transition property filter to remove registered event.
- **`RemoveTransitionsWhenAllEndedAsync()`: `Task RemoveTransitionsWhenAllEndedAsync(params KeyValuePair<ElementReference, string>[] elementRefsWithProperties)`**
Removes event listeners for `transitionend` HTML event for the given elements with property filters.
Async function it takes an array of elements (use name of `@ref=""` value) with optional transition property filters to remove registered events.
- **`DisposeAsync()`: `Task DisposeAsync()`** <br />
Component implements `IAsyncDisposable` interface **your component also should implement it and call `ITransitionEventsService` instance DisposeAsync**.

### `TransitionEventArgs` event arguments
`TransitionEventArgs` is a new event argument for Transition events.

```
public sealed class CssBaseEventArgs : EventArgs
{
	public ElementReference Element { get; set; }
	public string OriginalPropertyNameFilter { get; set; }
	public string PropertyName { get; set; }

	public bool Composed { get; set; }
	public double ElapsedTime { get; set; }
	public int EventPhase { get; set; }
	public bool ReturnValue { get; set; }
	public string Type { get; set; }
}
```

# Configuration

## Installation

Blazor.Components.CssEvents is available on [NuGet](https://www.nuget.org/packages/Majorsoft.Blazor.Components.CssEvents). 

```sh
dotnet add package Majorsoft.Blazor.Components.CssEvents
```
Use the `--version` option to specify a [preview version](https://www.nuget.org/packages/Majorsoft.Blazor.Components.CssEvents/absoluteLatest) to install.

## Usage

Add using statement to your Blazor `<component/page>.razor` file. Or globally reference it into `_Imports.razor` file.**
**Components have complex usage examples depending on CSS classes, @keyframes and events, etc. So best way to understand is to check Demo app code.**

```
@using Blazor.Components.CssEvents
@*Only if you want to use Animations*@
@using Blazor.Components.CssEvents.Animation
@*Only if you want to use Transitions*@
@using Blazor.Components.CssEvents.Transition
```


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

### `AnimationElement` usage

Following code example shows how to use **`AnimationElement`** component with registered all 3 events to the same callback.

```
<Content>
	<div id="divTransition1" class="divAnimation animateInfinite">
		<label id="lbTransition1" class="lbAnimation animateInfinite">Infinite Animation will never end...</label>
	</div>
</Content>

@code {
	private void OnAnimationEvent1(AnimationEventArgs e)
	{
		//Write your code here...
	}
}
```

### `IAnimationEventsService` usage
Following code example shows how to use **`IAnimationEventsService`** service with registered all 3 events to the same callback.

```
<div id="divTransition4" @ref="_div4" @onclick="StartAnimation3" class="divAnimation @(_animateDiv4 ? _animateCss : "")">
	<label id="lbTransition4" class="lbAnimation @(_animateDiv4 ? _animateCss : "")">Click me to start Animation</label>
</div>
<button class="btn btn-primary mt-3" @onclick="HandleEvent3">@_eventsText1</button>

@implements IAsyncDisposable
@inject IAnimationEventsService _animationService;
@code {
	private ElementReference _div4;
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			await _animationService.RegisterAllAnimationEventsAsync(_div4, OnAnimationEvent4);
		}
	}

	private async Task OnAnimationEvent4(AnimationEventArgs e)
	{
		//Write your code here...
	}

	public async ValueTask DisposeAsync()
	{
		if(_animationService is not null)
		{
			await _animationService.DisposeAsync();
		}
	}
}
```

### `TransitionElement` usage
Following code example shows how to use **`TransitionElement`** component with registered event to a callback.

```
<TransitionElement OnTransitionEnded="OnTransitionEnded1">
	<Content>
		<div id="divTransition1" class="divTransition">
			<label id="lbTransition1" class="lbTransition m-1">Hover over to start Transition</label>
		</div>
	</Content>
</TransitionElement>

@code {
	private void OnTransitionEnded1(TransitionEventArgs e)
	{
		//Write your code here...
	}
}
```

### `ITransitionEventsService` usage
Following code example shows how to use **`ITransitionEventsService`** service with registered event to a callback.

```
<div id="divTransition3" @ref="_div3" class="divTransition">
	<label id="lbTransition3" class="lbTransition m-1">Hover over to start Transition</label>
</div>
<button class="btn btn-primary mt-3" @onclick="HandleEvent1">@_eventsText1</button>

@implements IAsyncDisposable
@inject ITransitionEventsService _transitionService;
@code {
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			await _transitionService.RegisterTransitionEndedAsync(_div3, OnTransitionEnded3);
		}
	}

	private ElementReference _div3;
	private async Task OnTransitionEnded3(TransitionEventArgs e)
	{
		//Write your code here...
	}

	public async ValueTask DisposeAsync()
	{
		if(_transitionService is not null)
		{
			await _transitionService.DisposeAsync();
		}
	}
}
```