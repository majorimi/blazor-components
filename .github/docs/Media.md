Blazor Components Media controls
============
[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-build-check)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=6)
[![Package Version](https://img.shields.io/nuget/v/Majorsoft.Blazor.Components.Media?label=Latest%20Version)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Media/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Majorsoft.Blazor.Components.Media?label=Downloads)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Media/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/majorimi/blazor-components/blob/master/LICENSE)

# About

Blazor components for **capturing and playing media** in the browser:
camera **photo and video capture** with live preview (`getUserMedia` + `MediaRecorder` APIs), **microphone audio
recording** with live input level metering, a **simple HTML file capture** input (native camera app on mobile devices),
**video/audio player** components fully controllable from .NET code and a live **audio visualizer**
(FFT frequency spectrum / waveform) for recorders and players.
**All components work with WebAssembly and Server hosted models**.
For code examples [see usage](https://github.com/majorimi/blazor-components/blob/master/src/Majorsoft.Blazor.Components.TestApps.Common/Components/MediaDemo.razor).

You can try it out by using the [demo app](https://blazorextensions.z6.web.core.windows.net/media).

**Note**: camera and microphone access requires a **secure context** (HTTPS or localhost) and is granted by the user per site.

# Components and services

## Capture

- **`CameraCapture`**: renders an HTML `<video>` element with **live camera preview** (`getUserMedia`).
  Take **photos** from the live stream in full camera resolution (`TakePhotoAsync`, returns a Blob URL to show
  and `GetPhotoStreamAsync` for the bytes) and record **video with optional audio** using the `MediaRecorder` API
  (`StartRecordingAsync`, `PauseRecordingAsync`, `ResumeRecordingAsync`, `StopRecordingAsync`).
  Camera can be selected by device Id or facing mode (front/rear). Preview `Width`/`Height` can be given
  in **px or %** (`IsCameraDimensionInPixels`).
- **`AudioRecorder`**: records **audio from a microphone** with the `MediaRecorder` API. Renders no HTML, use it
  with `@ref` from code: `OpenMicrophoneAsync` then `StartRecordingAsync`. Provides a live **input level meter**
  (`OnAudioLevelChanged`, RMS level 0..1) e.g. for volume bars.
- **`MediaFileCapture`**: the simple HTML way, no JS interop: renders `<input type="file">` with `accept` and
  `capture` attributes. Mobile browsers open the **native camera/recorder app**, desktop browsers a file picker.
  Captured media arrives as `IBrowserFile` (`OnFilesCaptured` event).

Recordings are kept in the browser as a `Blob`: the reported `MediaRecordingInfo` contains a **Blob URL** which can be
played back directly by the player components, and the raw bytes can be downloaded to .NET with `GetRecordingStreamAsync()`
(e.g. to upload or save the recording).

## Playback

- **`VideoPlayer`**: renders an HTML `<video>` element with playback **events** (`OnPlay`, `OnPause`, `OnEnded`,
  `OnTimeUpdate`, `OnMetadataLoaded`, `OnError`, ...) and **control API** (`PlayAsync`, `PauseAsync`, `SeekAsync`,
  `SetVolumeAsync`, `SetMutedAsync`, `SetPlaybackRateAsync`, `GetStateAsync`), plus `RequestFullscreenAsync`,
  `TogglePictureInPictureAsync` and `CaptureFrameAsync` (current frame as image). `Width`/`Height` can be given
  in **px or %** (`IsPlayerDimensionInPixels`).
- **`AudioPlayer`**: the same for an HTML `<audio>` element.

Player sources: regular URL (`Source` parameter or `SetSourceAsync(url)`), **Blob URL of a recording**
(`MediaRecordingInfo.Url`) or **.NET stream** (`SetSourceAsync(stream, mimeType)` e.g. media from a database).

## Visualization

- **`AudioVisualizer`**: renders a live **audio visualization onto an HTML `<canvas>`** using the Web Audio
  `AnalyserNode` API. Two modes: **FFT frequency spectrum bars** (spectrum analyzer) and **time domain waveform**
  (oscilloscope). Attach it to a capture component (`CaptureSource`, e.g. `AudioRecorder` microphone input) or
  to a player component (`PlayerSource`, e.g. `AudioPlayer` playback). Drawing runs entirely in JS with
  `requestAnimationFrame` (60 fps, **no interop traffic**); the FFT reduced to configurable bands (0..1 values)
  can additionally be reported to .NET via `OnSpectrumChanged` on a throttled timer for custom rendering.
  Configurable: `FftSize`, `SmoothingTimeConstant`, `MinDecibels`/`MaxDecibels`, `BarCount`, logarithmic/linear
  frequency scale, colors with optional gradient. `Width`/`Height` can be given in **px or %**
  (`IsVisualizerDimensionInPixels`), the drawing resolution follows the CSS size automatically. **Note**: cross-origin player sources need CORS headers,
  Blob URL recordings and .NET stream sources always work.

## Services

- **`IMediaDeviceService`**: injectable service to list available **cameras, microphones and speakers**
  (`GetMediaDevicesAsync`), **request media permissions** (`RequestPermissionsAsync`), check `getUserMedia`/`MediaRecorder`
  **browser support** and list the **supported recording MIME types** (`GetSupportedMimeTypesAsync`).

# Setup

Install the [Majorsoft.Blazor.Components.Media](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Media/) NuGet package:

```sh
dotnet add package Majorsoft.Blazor.Components.Media
```

Register the services (only required for `IMediaDeviceService`):

```csharp
using Majorsoft.Blazor.Components.Media;
...
builder.Services.AddMediaComponents();
```

Add the following usings to your components or `_Imports.razor`:

```csharp
@using Majorsoft.Blazor.Components.Media
@using Majorsoft.Blazor.Components.Media.Capture
@using Majorsoft.Blazor.Components.Media.Players
@using Majorsoft.Blazor.Components.Media.Visualization
```

# Usage

## Camera photo and video capture

```razor
<CameraCapture @ref="_camera" Width="640" Height="480" RecordAudio="true"
			   OnPhotoTaken="PhotoTaken" OnRecordingFinished="RecordingFinished" OnCaptureError="CaptureError" />

<button @onclick="() => _camera.StartCameraAsync()">Start camera</button>
<button @onclick="() => _camera.TakePhotoAsync()">Take photo</button>
<button @onclick="() => _camera.StartRecordingAsync()">Record</button>
<button @onclick="() => _camera.StopRecordingAsync()">Stop</button>

@if (_photo is not null)
{
	<img src="@_photo.Url" width="320" />
}

<VideoPlayer @ref="_player" Width="640" Height="360" />

@code {
	private CameraCapture _camera;
	private VideoPlayer _player;
	private MediaPhotoInfo _photo;

	private void PhotoTaken(MediaPhotoInfo photo) => _photo = photo; //bytes: await _camera.GetPhotoStreamAsync()

	private async Task RecordingFinished(MediaRecordingInfo recording)
	{
		await _player.SetSourceAsync(recording.Url); //play back the recording from Blob URL
		//or download the bytes: await using var stream = await _camera.GetRecordingStreamAsync();
	}

	private void CaptureError(string error) { }
}
```

## Microphone recording with level meter

```razor
<AudioRecorder @ref="_recorder" OnAudioLevelChanged="level => _level = level"
			   OnRecordingFinished="RecordingFinished" OnCaptureError="CaptureError" />

<button @onclick="() => _recorder.OpenMicrophoneAsync()">Open microphone</button>
<button @onclick="() => _recorder.StartRecordingAsync()">Record</button>
<button @onclick="() => _recorder.StopRecordingAsync()">Stop</button>

<progress max="1" value="@_level"></progress>

<AudioPlayer @ref="_audioPlayer" />

@code {
	private AudioRecorder _recorder;
	private AudioPlayer _audioPlayer;
	private double _level;

	private async Task RecordingFinished(MediaRecordingInfo recording)
		=> await _audioPlayer.SetSourceAsync(recording.Url);

	private void CaptureError(string error) { }
}
```

## Voice recorder with FFT spectrum analyzer

`AudioVisualizer` starts automatically when the attached source becomes active (`AutoStart`, default true).

```razor
<AudioRecorder @ref="_recorder" OnRecordingFinished="RecordingFinished" OnCaptureError="CaptureError" />
<AudioVisualizer CaptureSource="_recorder" Width="640" Height="120"
				 BarColor="#2196f3" BarGradientColor="#f321a7" BarCount="48" />

<button @onclick="() => _recorder.OpenMicrophoneAsync()">Open microphone</button>
<button @onclick="() => _recorder.StartRecordingAsync()">Record</button>
<button @onclick="() => _recorder.StopRecordingAsync()">Stop</button>

@code {
	private AudioRecorder _recorder;

	private async Task RecordingFinished(MediaRecordingInfo recording) { /* play or upload it */ }
	private void CaptureError(string error) { }
}
```

The same for playback, e.g. a player showing the spectrum of the played audio (works with recordings,
.NET stream sources and same-origin/CORS enabled URLs):

```razor
<AudioPlayer @ref="_player" Source="music.mp3" />
<AudioVisualizer PlayerSource="_player" Mode="AudioVisualizerModes.FrequencyBars" />
```

Waveform (oscilloscope) mode and .NET side FFT band data for custom rendering:

```razor
<AudioVisualizer CaptureSource="_recorder" Mode="AudioVisualizerModes.Waveform"
				 OnSpectrumChanged="bands => _bands = bands" SpectrumBandCount="16" SpectrumIntervalMs="100" />

@code {
	private double[] _bands; //16 values 0..1, e.g. for custom bars/LED meters
}
```

## Simple HTML capture

```razor
<MediaFileCapture MediaType="CaptureMediaTypes.Photo" CaptureMode="CameraFacingModes.Environment"
				  OnFilesCaptured="FilesCaptured" />

@code {
	private async Task FilesCaptured(IReadOnlyList<IBrowserFile> files)
	{
		await using var stream = files[0].OpenReadStream(maxAllowedSize: 50 * 1024 * 1024);
		//process the captured photo/video/audio file
	}
}
```

## Device enumeration

```razor
@inject IMediaDeviceService _mediaDeviceService

@code {
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			//Permission needed to see device labels.
			var error = await _mediaDeviceService.RequestPermissionsAsync(audio: true, video: true);
			var devices = await _mediaDeviceService.GetMediaDevicesAsync();
			var cameras = devices.Where(d => d.IsVideoInput);
		}
	}
}
```
