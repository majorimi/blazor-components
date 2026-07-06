Blazor Components Canvas and WebGL controls
============
[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-build-check)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=6)
[![Package Version](https://img.shields.io/nuget/v/Majorsoft.Blazor.Components.Canvas?label=Latest%20Version)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Canvas/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Majorsoft.Blazor.Components.Canvas?label=Downloads)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Canvas/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/majorimi/blazor-components/blob/master/LICENSE)

# About

Blazor components that wrap the HTML `<canvas>` element and expose its rendering APIs to .NET code via JS interop:
the **`Canvas2D`** component exposes the **Canvas 2D drawing API** (`CanvasRenderingContext2D`) and the
**`WebGLCanvas`** component exposes the **WebGL/WebGL2 GPU rendering API**. Both support operation **batching**
(many commands in one JS interop round trip) and an optional **render loop** (`requestAnimationFrame` based
`OnFrame` callback) for animations.
**All components work with WebAssembly and Server hosted models**.
For code examples [see usage](https://github.com/majorimi/blazor-components/blob/master/src/Majorsoft.Blazor.Components.TestApps.Common/Components/CanvasDemo.razor).

You can try it out by using the [demo app](https://blazorextensions.z6.web.core.windows.net/canvas).

# Components

- **`Canvas2D`**: renders an HTML `<canvas>` and exposes a `Canvas2DContext` with the Canvas 2D API: rectangles,
  paths, text, gradients, patterns, images, transforms, pixel access (`GetImageDataAsync`/`PutImageDataAsync`) and export (`ToDataURLAsync`).
- **`WebGLCanvas`**: renders an HTML `<canvas>` and exposes a `WebGLContext` with the core WebGL API: buffers,
  shaders, programs, attributes, uniforms, textures and draw calls, with constants in the `GL` static class.
  Requests WebGL2 by default and falls back to WebGL 1.0 (check `WebGLContext.IsWebGL2`).

## Shared component properties and events

- **`Width`: `int { get; set; }` (default: 300)** — width of the canvas in px or %.
- **`Height`: `int { get; set; }` (default: 150)** — height of the canvas in px or %.
- **`IsCanvasDimensionInPixels`: `bool { get; set; }` (default: true)** — whether `Width`/`Height` are specified
  in pixels, false means %. In pixel mode they set the drawing buffer size directly; in % mode they are applied
  as CSS size and the drawing buffer is automatically synced to the resulting layout size after the context is
  created (% height requires a parent element with a set height).
- **`GetCanvasSizeAsync()` / `SyncCanvasSizeAsync()`** — returns the drawing buffer size in pixels / re-syncs the
  buffer to the CSS layout size (useful with % dimensions after a window resize; resizing clears the canvas and
  resets 2D context state, the WebGL viewport is updated automatically).
- **`Class` / `Style`: `string? { get; set; }`** — custom CSS class(es) / inline style of the `<canvas>` element.
- **`OnFrame`: `EventCallback<CanvasFrameEventArgs>`** — render loop callback fired on every animation frame
  with `DeltaTime` and `TotalTime` (seconds). When set the loop starts automatically after the context is created.
- **`AdditionalAttributes`** — arbitrary HTML attributes **including Blazor event handlers** (e.g. `@onmousemove`)
  splatted onto the `<canvas>` element.
- **`TabIndex`: `int? { get; set; }` (default: null)** — rendered as the HTML `tabindex` attribute. Set to 0 to make
  the canvas focusable, required for keyboard input (canvas elements are not focusable by default).
- **`InnerElementReference`: `ElementReference { get; }`** — the rendered `<canvas>` element reference.
- **`StartRenderLoopAsync()` / `StopRenderLoopAsync()`** — render loop control, **`IsRenderLoopRunning`** state.
- **`FocusAsync()`** — sets browser focus to the canvas.

### Input capture

For apps that require input, like drawing tools, editors or games, the components can track input state directly on the canvas:

- **`StartInputCaptureAsync(CanvasInputOptions?)` / `StopInputCaptureAsync()`** — starts/stops tracking keyboard
  (physical `KeyboardEvent.code` values like "KeyW", layout independent), mouse buttons/position (in drawing buffer
  pixels)/movement/wheel and connected gamepads (polled). Options: `Keyboard`, `Mouse`, `Gamepads`, `PreventDefault`
  (stops arrows/space scrolling the page and enables right click as custom input).
- **`CanvasFrameEventArgs.Input`** — while capture is active, every `OnFrame` callback carries a
  **`CanvasInputState`** snapshot (no extra interop call): `Keys` (held), `PressedKeys` (fire-once since the
  previous snapshot — short taps are never missed), `Buttons`, `MouseX/Y`, accumulated `MovementX/Y` and
  `WheelDelta` (reset on read), `MousePath` (every recorded position while a button was held, as flat x,y pairs —
  fast freehand strokes lose no points between frames), `IsPointerLocked`, `Gamepads` (axes + analog button
  values), with helpers `IsKeyDown("KeyW")`, `WasKeyPressed("Space")`, `IsButtonDown(0)`, `GamepadState.IsPressed(0)`.
- **`GetInputStateAsync()`** — reads a snapshot on demand outside the render loop.
- **`RequestPointerLockAsync()` / `ExitPointerLockAsync()`** — FPS style relative mouse (must be called from a user
  gesture, e.g. a button or click handler; Esc releases the lock).

```
<WebGLCanvas @ref="_canvas" TabIndex="0" OnContextCreated="OnReady" OnFrame="OnFrame" />

@code {
	private WebGLCanvas _canvas;

	private async Task OnReady(WebGLContext gl) => await _canvas.StartInputCaptureAsync();

	private async Task OnFrame(CanvasFrameEventArgs args)
	{
		if (args.Input?.IsKeyDown("KeyW") == true) { /* move forward by args.DeltaTime */ }
	}
}
```

## `Canvas2D` component

- **`Context`: `Canvas2DContext?`** — the 2D drawing context, created after first render.
- **`OnContextCreated`: `EventCallback<Canvas2DContext>`** — invoked when the context is ready, start drawing here.

`Canvas2DContext` exposes the Canvas 2D API as async methods, e.g.: `SetFillStyleAsync` (color, `CanvasGradient`,
`CanvasPattern`), `FillRectAsync`, `BeginPathAsync`, `ArcAsync`, `RoundRectAsync`, `BezierCurveToAsync`, `FillAsync`,
`StrokeAsync`, `ClipAsync`, `FillTextAsync`, `MeasureTextAsync`, `TranslateAsync`, `RotateAsync`, `ScaleAsync`,
`DrawImageAsync` (URL or `ElementReference`), `GetImageDataAsync`/`PutImageDataAsync` (raw RGBA bytes),
`CreateLinearGradientAsync`/`CreateRadialGradientAsync`/`CreateConicGradientAsync`, `CreatePatternAsync`,
`ToDataURLAsync` and more.

Use **`BeginBatch()`** / **`EndBatchAsync()`** to send many drawing commands in a **single JS interop call** —
important for performance, especially on Blazor Server where every interop call is a SignalR message.

```
<Canvas2D @ref="_canvas" Width="400" Height="200" OnContextCreated="OnCanvasReady" />

@code {
	private Canvas2D _canvas;

	private async Task OnCanvasReady(Canvas2DContext ctx)
	{
		ctx.BeginBatch();

		var gradient = await ctx.CreateLinearGradientAsync(0, 0, 400, 0); //Value returning calls flush the batch.
		await gradient.AddColorStopAsync(0, "#1b6ec2");
		await gradient.AddColorStopAsync(1, "#7b2cbf");

		await ctx.SetFillStyleAsync(gradient);
		await ctx.FillRectAsync(0, 0, 400, 200);
		await ctx.SetFillStyleAsync("white");
		await ctx.SetFontAsync("bold 28px Arial");
		await ctx.FillTextAsync("Hello Canvas!", 20, 60);

		await ctx.EndBatchAsync();
	}
}
```

## `WebGLCanvas` component

- **`Context`: `WebGLContext?`** — the WebGL context, created after first render.
- **`ContextType`: `WebGLContextType { get; set; }` (default: `WebGL2`)** — requested version, falls back to WebGL 1.0.
- **`ContextAttributes`: `WebGLContextAttributes?`** — context creation attributes (`Alpha`, `Antialias`, `Depth`,
  `Stencil`, `PreserveDrawingBuffer`, `PowerPreference`, etc.).
- **`OnContextCreated`: `EventCallback<WebGLContext>`** — invoked when the context is ready, set up GPU resources here.

`WebGLContext` exposes the WebGL API: `CreateBufferAsync`, `BindBufferAsync`, `BufferDataAsync`/`BufferSubDataAsync`
(`float[]`/`ushort[]`/`int[]` uploaded as typed arrays), **`CompileProgramAsync(vs, fs)`** (compile + link in one
interop call, throws with the shader info log on error), `GetAttribLocationAsync`, `GetUniformLocationAsync`,
`VertexAttribPointerAsync`, `UniformAsync` overloads, uniform arrays (`Uniform1fvAsync`...`Uniform4fvAsync`,
`Uniform1ivAsync`) and matrices (`UniformMatrix2fvAsync`/`3fv`/`4fv`), textures (`TexImage2DAsync` from URL, raw
RGBA bytes or null to allocate render targets, `TexSubImage2DAsync`, `CopyTexImage2DAsync`, cube map faces,
`GenerateMipmapAsync`), **framebuffers and renderbuffers** for render to texture (`CreateFramebufferAsync`,
`FramebufferTexture2DAsync`, `CheckFramebufferStatusAsync`, `RenderbufferStorageAsync`), `ReadPixelsAsync`,
**instancing** (`DrawArraysInstancedAsync`, `DrawElementsInstancedAsync`, `VertexAttribDivisorAsync`),
**vertex array objects**, full render state (scissor, stencil, blend equations/factors/color, color/depth masks,
polygon offset) and extension queries (`GetExtensionAsync`, `GetSupportedExtensionsAsync`).
On WebGL 1.0 contexts instancing and vertex array objects are automatically backed by the
`ANGLE_instanced_arrays` / `OES_vertex_array_object` extensions when available, so the .NET API stays uniform.
GL enum values are in the **`GL`** static class. GPU objects (buffers, shaders, programs, textures, framebuffers,
renderbuffers, uniform locations) are returned as .NET handle objects (`WebGLBuffer`, `WebGLProgram`, ...).

```
<WebGLCanvas Width="400" Height="300" OnContextCreated="OnGlReady" OnFrame="OnFrame" />

@code {
	private WebGLContext _gl;
	private WebGLUniformLocation _uAngle;

	private async Task OnGlReady(WebGLContext gl)
	{
		_gl = gl;
		var program = await gl.CompileProgramAsync(
@"attribute vec2 aPos; attribute vec3 aColor; uniform float uAngle; varying vec3 vColor;
void main() { float c = cos(uAngle); float s = sin(uAngle);
	gl_Position = vec4(mat2(c, -s, s, c) * aPos, 0.0, 1.0); vColor = aColor; }",
@"precision mediump float; varying vec3 vColor;
void main() { gl_FragColor = vec4(vColor, 1.0); }");

		var buffer = await gl.CreateBufferAsync();
		await gl.BindBufferAsync(GL.ARRAY_BUFFER, buffer);
		await gl.BufferDataAsync(GL.ARRAY_BUFFER, new float[] {
			//x, y, r, g, b
			 0.0f,  0.6f, 1f, 0f, 0f,
			-0.6f, -0.6f, 0f, 1f, 0f,
			 0.6f, -0.6f, 0f, 0f, 1f }, GL.STATIC_DRAW);

		await gl.UseProgramAsync(program);
		var aPos = await gl.GetAttribLocationAsync(program, "aPos");
		var aColor = await gl.GetAttribLocationAsync(program, "aColor");
		_uAngle = await gl.GetUniformLocationAsync(program, "uAngle");
		await gl.EnableVertexAttribArrayAsync(aPos);
		await gl.VertexAttribPointerAsync(aPos, 2, GL.FLOAT, false, 20, 0);
		await gl.EnableVertexAttribArrayAsync(aColor);
		await gl.VertexAttribPointerAsync(aColor, 3, GL.FLOAT, false, 20, 8);
	}

	private async Task OnFrame(CanvasFrameEventArgs args)
	{
		_gl.BeginBatch(); //One JS interop call per frame.
		await _gl.UniformAsync(_uAngle, (float)args.TotalTime);
		await _gl.ClearColorAsync(0.1f, 0.1f, 0.15f, 1f);
		await _gl.ClearAsync(GL.COLOR_BUFFER_BIT);
		await _gl.DrawArraysAsync(GL.TRIANGLES, 0, 3);
		await _gl.EndBatchAsync();
	}
}
```

**\* Note**: keep per-frame work batched. Every non-batched call is one JS interop round trip; on Server hosted
apps that is a network message. For very heavy scenes consider keeping the render loop fully in JS.

# Configuration

## Installation

**Majorsoft.Blazor.Components.Canvas** is available on [NuGet](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Canvas/).

```sh
dotnet add package Majorsoft.Blazor.Components.Canvas
```
Use the `--version` option to specify a [preview version](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Canvas/absoluteLatest) to install.

## Usage

Add using statement to your Blazor `<component/page>.razor` file. Or globally reference it into `_Imports.razor` file.
```
@using Majorsoft.Blazor.Components.Canvas
@* For WebGL: *@
@using Majorsoft.Blazor.Components.Canvas.WebGL
```

No service registration or stylesheet link is required.
