Blazor Timer Component
============
[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-build-check)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=6)
[![Package Version](https://img.shields.io/nuget/v/Majorsoft.Blazor.Components.Timer?label=Latest%20Version)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Timer/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Majorsoft.Blazor.Components.Timer?label=Downloads)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Timer/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/majorimi/blazor-components/blob/master/LICENSE)

# About

Blazor component that can be used as a simple scheduler or performing periodically repeated tasks 
by calling custom async code. **All components work with WebAssembly and Server hosted models**. 
For code examples [see usage](https://github.com/majorimi/blazor-components/blob/master/src/Blazor.Components.TestApps.Common/Components/TimerComponent.razor).


You can try it out by using the [demo app](https://blazorextensions.z6.web.core.windows.net/timer).

![Timer demo](https://github.com/majorimi/blazor-components/raw/master/.github/docs/gifs/timer.gif)

# Components

- **`AdvancedTimer`**: Timer object wrapped into a Blazor component to perform async operations on  elapsed event.

## `AdvancedTimer` component

This component does not render any HTML element. It is wrapped into a component for simpler usage. 
Component will allow you to call `async` operations, resources automatically disposed by the framework, etc.
It is useful when you need to update UI periodically, e.g. refresh a dashboard in every 30 sec by calling an API endpoint.

**NOTE: this technique called 'polling'. Which is not the most efficient way to notify client. Nowadays you can use 
much more modern techniques. 'push' based communication like: SignalR or WebSecket, etc. Make sure you have no other options than 'polling'.**

### Properties
- **`IntervalInMilisec`: `double { get; set; }` (default: 200)** <br />
  Notification timeout in ms. If set to `0 or less` it set to 1 ms.
- **`DelayInMilisec`: `double { get; set; }` (default: 0)** <br />
  Delay in ms. before the timer will start. If set to `0` timer will start immediately.
- **`AutoStart`: `bool { get; set; }` (default: true)** <br />
 If `true` timer will start when component `OnInitialized` event run, otherwise timer must be started by calling `Start()` method.  
- **`Occurring`: `Times { get; set; }` (default: Times.Once())** <br />
  Number of times elapsed even will be fired. See `Times` record description.
- **`IsEnabled`: `bool { get; }`** <br />
Returns the inner state of the timer. `True` if timer is running otherwise `false`.

**Arbitrary HTML attributes e.g.: `id="load1"` can be applied but won't result in HTLM DOM**.

### Events
- **`OnIntervalElapsed`: `EventCallback<ulong>` delegate - Required** <br />
  Timer event this Function called when specified timeout elapsed, parameter is the iteration count.

### Functions
- **`Start()`: `void Start()`** <br />
Starts the internal timer which will start after the set delay and fire event for the given occurrence times.
- **`Stop()`: `void Stop()`** <br />
Stops the internal timer and no more event will be fired.
- **`Reset()`: `void Reset()`** <br />
Restarts the internal timer and resets the occurrence counter to 0. Events will be fired for the given occurrence times.
- **`Dispose()`: `implements IDisposable` interface** <br />
Component implements `IDisposable` interface Blazor framework will call it when parent removed from render tree.

### Times record
It is record object wrapping an `ulong` value to setting the `AdvancedTimer` `Occurring` property.
#### Properties
- **`IntervalInMilisec`: `ulong { get; }` - Required**
Returns the set value. Timer will use it for counting elapsed events.

#### Functions
- **`Once()`: `Times Once()`** <br />
Factory method to create a new instance of `Times` with value: `1`.
- **`Infinite()`: `Times Infinite()`** <br />
Factory method to create a new instance of `Times` with value: `ulong.MaxValue`.
- **`Exactly()`: `Times Exactly(ulong count)`** <br />
Factory method to create a new instance of `Times` with the given parameter value.

# Configuration

## Installation

**Majorsoft.Blazor.Components.Timer** is available on [NuGet](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Timer/). 

```sh
dotnet add package Majorsoft.Blazor.Components.Timer
```
Use the `--version` option to specify a [preview version](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Timer/absoluteLatest) to install.

## Usage

Add using statement to your Blazor `<component/page>.razor` file. Or globally reference it into `_Imports.razor` file.

```
@using Majorsoft.Blazor.Components.Timer
```

Following code example shows how to use **`AdvancedTimer`** component in your Blazor App. With 2 sec. delay
1 sec. interval occurring only 10 times and with Reset function.

```
<span>Delayed counter (starts after 2 sec.): <strong>@_count</strong></span>
<AdvancedTimer @ref="_counter" IntervalInMilisec="1000" DelayInMilisec="2000" Occurring="Times.Exactly(10)" OnIntervalElapsed="@(c => Counter(c))" />

<br />
<button class="btn btn-sm btn-primary" @onclick="CounterReset">Reset</button>

@code {
	//Counter
	private AdvancedTimer _counter;
	private ulong _count = 0;
	private void Counter(ulong count)
	{
		_count = count;
	}
	private void CounterReset() => _counter.Reset();
}
```

Following code example shows how to use **`AdvancedTimer`** component in your Blazor App. With infinite loop and settable 
interval on UI and Start/Stop function used.

```
<div>
	<input type="range" min="100" max="2000" @bind="clockInterval" /> Clock interval: @clockInterval ms.
</div>
<span>Infinite clock (Manual Start): <strong>@_time</strong></span>
<AdvancedTimer @ref="_clock" IntervalInMilisec="@clockInterval" Occurring="Times.Infinite()" AutoStart="false" OnIntervalElapsed="@Clock" />

<br />
<button class="btn btn-sm btn-primary" @onclick="StartStopClock">@_buttonText</button>

@code {
	//Clock
	private double clockInterval = 300;
	private string _time = DateTime.Now.ToString("hh:mm:ss.f");
	private AdvancedTimer _clock;
	private string _buttonText = "Start";
	private void Clock()
	{
		_time = DateTime.Now.ToString("hh:mm:ss.f");
	}
	private void StartStopClock()
	{
		if (_clock.IsEnabled)
		{
			_clock.Stop();
			_buttonText = "Start";
		}
		else
		{
			_clock.Start();
			_buttonText = "Stop";
		}
	}
}

```
