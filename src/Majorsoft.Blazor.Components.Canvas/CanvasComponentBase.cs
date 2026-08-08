using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Components.Canvas
{
	/// <summary>
	/// Base class of the <see cref="Canvas2D"/> and <see cref="WebGL.WebGLCanvas"/> components. Renders the
	/// HTML &lt;canvas&gt; element, loads the JS interop module, creates the rendering context after first
	/// render and optionally runs a <c>requestAnimationFrame</c> render loop firing <see cref="OnFrame"/>.
	/// </summary>
	public abstract class CanvasComponentBase : ComponentBase, IAsyncDisposable
	{
		private IJSObjectReference? _module;
		private DotNetObjectReference<CanvasComponentBase>? _dotNetRef;
		private bool _loopRunning;

		[Inject] private IJSRuntime _jsRuntime { get; set; } = default!;
		[Inject] private ILogger<CanvasComponentBase> _logger { get; set; } = default!;

		private protected ElementReference _canvasRef;
		private protected CanvasContextBase? _contextBase;

		/// <summary>
		/// Exposes the Blazor <see cref="ElementReference"/> of the rendered &lt;canvas&gt; element.
		/// Useful for JS interop or as source of <see cref="Canvas2DContext.DrawImageAsync(ElementReference, double, double)"/>.
		/// </summary>
		public ElementReference InnerElementReference => _canvasRef;

		/// <summary>Width of the canvas in px or % (see <see cref="IsCanvasDimensionInPixels"/>). Default is 300.</summary>
		[Parameter] public int Width { get; set; } = 300;

		/// <summary>Height of the canvas in px or % (see <see cref="IsCanvasDimensionInPixels"/>). Default is 150.</summary>
		[Parameter] public int Height { get; set; } = 150;

		/// <summary>
		/// Gets or sets True indicating whether the canvas dimensions are specified in pixels, False means %. Default is true.
		/// In pixel mode <see cref="Width"/>/<see cref="Height"/> set the drawing buffer size directly (HTML attributes).
		/// In % mode they are applied as CSS size and the drawing buffer is synced to the resulting layout size after
		/// the context was created; call <see cref="SyncCanvasSizeAsync"/> to re-sync after layout changes
		/// (note: re-syncing clears the canvas and resets 2D context state).
		/// </summary>
		[Parameter] public bool IsCanvasDimensionInPixels { get; set; } = true;

		/// <summary>
		/// Rendered as the HTML <c>tabindex</c> attribute of the &lt;canvas&gt; element, null (default) renders no attribute.
		/// Set to 0 to make the canvas focusable — required for keyboard input (canvas elements are not focusable by default).
		/// </summary>
		[Parameter] public int? TabIndex { get; set; }

		/// <summary>Custom CSS class(es) applied to the &lt;canvas&gt; element.</summary>
		[Parameter] public string? Class { get; set; }

		/// <summary>Custom inline style applied to the &lt;canvas&gt; element.</summary>
		[Parameter] public string? Style { get; set; }

		/// <summary>
		/// Callback fired on every animation frame (<c>requestAnimationFrame</c>) while the render loop runs.
		/// When set, the loop is started automatically after the context was created; it can also be controlled
		/// with <see cref="StartRenderLoopAsync"/> and <see cref="StopRenderLoopAsync"/>.
		/// </summary>
		[Parameter] public EventCallback<CanvasFrameEventArgs> OnFrame { get; set; }

		/// <summary>Arbitrary HTML attributes (including Blazor event handlers like @onmousemove) applied to the &lt;canvas&gt; element.</summary>
		[Parameter(CaptureUnmatchedValues = true)]
		public Dictionary<string, object>? AdditionalAttributes { get; set; }

		/// <summary>True while the render loop is running.</summary>
		public bool IsRenderLoopRunning => _loopRunning;

		//HTML width/height attributes are only rendered in pixel mode (they are drawing buffer pixels by spec).
		private protected string? CanvasWidthAttribute => IsCanvasDimensionInPixels ? Width.ToString() : null;
		private protected string? CanvasHeightAttribute => IsCanvasDimensionInPixels ? Height.ToString() : null;

		//In % mode the dimensions are applied as CSS size in front of the user provided Style.
		private protected string? CanvasStyle => IsCanvasDimensionInPixels
			? Style
			: $"width: {Width}%; height: {Height}%; {Style}";

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (!firstRender)
			{
				return;
			}

#if DEBUG
			_module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Majorsoft.Blazor.Components.Canvas/canvas.js");
#else
			_module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Majorsoft.Blazor.Components.Canvas/canvas.min.js");
#endif

			await InitializeContextAsync(_module);

			if (_contextBase is not null && OnFrame.HasDelegate)
			{
				await StartRenderLoopAsync();
			}
		}

		/// <summary>Creates the concrete JS rendering context and fires the context created event.</summary>
		private protected abstract Task InitializeContextAsync(IJSObjectReference module);

		private protected async Task<int> CreateJsContextAsync(IJSObjectReference module, string contextType, object? contextAttributes)
		{
			var id = await module.InvokeAsync<int>("createContext", _canvasRef, contextType, contextAttributes);
			if (id == 0)
			{
				_logger.LogWarning($"Component {GetType()}: could not create '{contextType}' rendering context, it is not supported by the browser.");
			}
			else if (!IsCanvasDimensionInPixels)
			{
				//Match the drawing buffer to the % based CSS layout size before the user starts drawing.
				await module.InvokeAsync<CanvasSize>("syncSize", id);
			}
			return id;
		}

		/// <summary>Returns the current drawing buffer size of the canvas in pixels.</summary>
		public async Task<CanvasSize> GetCanvasSizeAsync()
		{
			EnsureContext();
			return await _module!.InvokeAsync<CanvasSize>("getSize", _contextBase!.Id);
		}

		/// <summary>
		/// Syncs the drawing buffer size to the CSS layout size of the canvas element and returns the new size.
		/// Only useful with % dimensions (<see cref="IsCanvasDimensionInPixels"/> false), e.g. after a window resize.
		/// Note: resizing clears the canvas and resets 2D context state; the WebGL viewport is updated automatically.
		/// </summary>
		public async Task<CanvasSize> SyncCanvasSizeAsync()
		{
			EnsureContext();
			return await _module!.InvokeAsync<CanvasSize>("syncSize", _contextBase!.Id);
		}

		private void EnsureContext()
		{
			if (_module is null || _contextBase is null)
			{
				throw new InvalidOperationException("The canvas rendering context was not created yet, the component must be rendered first.");
			}
		}

		/// <summary>Render loop callback invoked from JS on every animation frame. Do not call it directly.</summary>
		[JSInvokable("FrameAsync")]
		public async Task FrameAsync(double deltaTime, double totalTime, CanvasInputState? input)
		{
			if (OnFrame.HasDelegate)
			{
				await OnFrame.InvokeAsync(new CanvasFrameEventArgs(deltaTime, totalTime, input));
			}
		}

		/// <summary>Sets browser focus to the &lt;canvas&gt; element (needed for keyboard input, see <see cref="TabIndex"/>).</summary>
		public async Task FocusAsync() => await _canvasRef.FocusAsync();

		/// <summary>
		/// Starts tracking keyboard/mouse/gamepad input state on the canvas. While active, every render loop frame
		/// carries a snapshot in <see cref="CanvasFrameEventArgs.Input"/>; outside the loop read it with
		/// <see cref="GetInputStateAsync"/>. Keyboard capture requires a focusable canvas (<see cref="TabIndex"/>).
		/// </summary>
		public async Task StartInputCaptureAsync(CanvasInputOptions? options = null)
		{
			EnsureContext();
			await _module!.InvokeVoidAsync("startInputCapture", _contextBase!.Id, options ?? new CanvasInputOptions());
		}

		/// <summary>Stops input tracking and removes all input event listeners.</summary>
		public async Task StopInputCaptureAsync()
		{
			EnsureContext();
			await _module!.InvokeVoidAsync("stopInputCapture", _contextBase!.Id);
		}

		/// <summary>
		/// Reads the current input state on demand. Movement and wheel deltas accumulate between reads and reset on read;
		/// prefer <see cref="CanvasFrameEventArgs.Input"/> inside the render loop (no extra interop call).
		/// </summary>
		public async Task<CanvasInputState?> GetInputStateAsync()
		{
			EnsureContext();
			return await _module!.InvokeAsync<CanvasInputState?>("getInputState", _contextBase!.Id);
		}

		/// <summary>
		/// Requests pointer lock on the canvas (FPS style relative mouse movement, cursor hidden).
		/// Browsers only grant it from a user gesture, e.g. a click handler. Track state via
		/// <see cref="CanvasInputState.IsPointerLocked"/>; Esc releases it.
		/// </summary>
		public async Task RequestPointerLockAsync()
		{
			EnsureContext();
			await _module!.InvokeVoidAsync("requestPointerLock", _contextBase!.Id);
		}

		/// <summary>Exits pointer lock.</summary>
		public async Task ExitPointerLockAsync()
		{
			EnsureContext();
			await _module!.InvokeVoidAsync("exitPointerLock");
		}

		/// <summary>Starts the <c>requestAnimationFrame</c> render loop firing <see cref="OnFrame"/>.</summary>
		public async Task StartRenderLoopAsync()
		{
			if (_module is null || _contextBase is null || _loopRunning)
			{
				return;
			}

			_loopRunning = true;
			_dotNetRef ??= DotNetObjectReference.Create(this);
			await _module.InvokeVoidAsync("startRenderLoop", _contextBase.Id, _dotNetRef);
		}

		/// <summary>Stops the render loop.</summary>
		public async Task StopRenderLoopAsync()
		{
			if (_module is null || _contextBase is null || !_loopRunning)
			{
				return;
			}

			_loopRunning = false;
			await _module.InvokeVoidAsync("stopRenderLoop", _contextBase.Id);
		}

		/// <summary>Releases the JS context, render loop and interop module.</summary>
		public async ValueTask DisposeAsync()
		{
			_dotNetRef?.Dispose();

			try
			{
				if (_contextBase is not null)
				{
					await _contextBase.DisposeAsync();
				}
				if (_module is not null)
				{
					await _module.DisposeAsync();
				}
			}
			catch (JSDisconnectedException)
			{
				//Circuit or page already gone, nothing to release.
			}
		}
	}
}
