using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Components.Canvas.WebGL
{
	/// <summary>
	/// Blazor component that renders an HTML &lt;canvas&gt; element and exposes the WebGL / WebGL2 rendering
	/// API to .NET code as <see cref="WebGLContext"/>. By default a WebGL2 context is requested with automatic
	/// fallback to WebGL 1.0 (check <see cref="WebGLContext.IsWebGL2"/>).
	/// Subscribe to <see cref="OnContextCreated"/> to set up GPU resources and start rendering.
	/// </summary>
	public partial class WebGLCanvas : CanvasComponentBase
	{
		/// <summary>
		/// The WebGL context of the canvas. Null until the component was rendered and the context created
		/// (see <see cref="OnContextCreated"/>) or when the browser does not support WebGL.
		/// </summary>
		public WebGLContext? Context { get; private set; }

		/// <summary>Requested context version. Default is <see cref="WebGLContextType.WebGL2"/> with fallback to WebGL 1.0.</summary>
		[Parameter] public WebGLContextType ContextType { get; set; } = WebGLContextType.WebGL2;

		/// <summary>Optional context creation attributes (alpha, antialias, preserveDrawingBuffer, etc.).</summary>
		[Parameter] public WebGLContextAttributes? ContextAttributes { get; set; }

		/// <summary>Callback invoked with the created <see cref="WebGLContext"/> after first render. Set up GPU resources here.</summary>
		[Parameter] public EventCallback<WebGLContext> OnContextCreated { get; set; }

		private protected override async Task InitializeContextAsync(IJSObjectReference module)
		{
			var isWebGL2 = ContextType == WebGLContextType.WebGL2;
			var id = await CreateJsContextAsync(module, isWebGL2 ? "webgl2" : "webgl", ContextAttributes);

			if (id == 0 && isWebGL2)
			{
				//WebGL2 not supported, fall back to WebGL 1.0.
				isWebGL2 = false;
				id = await CreateJsContextAsync(module, "webgl", ContextAttributes);
			}

			if (id == 0)
			{
				return;
			}

			Context = new WebGLContext(module, id, isWebGL2);
			_contextBase = Context;

			if (OnContextCreated.HasDelegate)
			{
				await OnContextCreated.InvokeAsync(Context);
			}
		}
	}
}
