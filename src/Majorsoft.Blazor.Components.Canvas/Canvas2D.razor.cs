using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Components.Canvas
{
	/// <summary>
	/// Blazor component that renders an HTML &lt;canvas&gt; element and exposes the Canvas 2D drawing API
	/// (<c>CanvasRenderingContext2D</c>) to .NET code as <see cref="Canvas2DContext"/>.
	/// Subscribe to <see cref="OnContextCreated"/> to start drawing.
	/// </summary>
	public partial class Canvas2D : CanvasComponentBase
	{
		/// <summary>
		/// The 2D drawing context of the canvas. Null until the component was rendered and the context created
		/// (see <see cref="OnContextCreated"/>) or when the browser does not support it.
		/// </summary>
		public Canvas2DContext? Context { get; private set; }

		/// <summary>Callback invoked with the created <see cref="Canvas2DContext"/> after first render. Start drawing here.</summary>
		[Parameter] public EventCallback<Canvas2DContext> OnContextCreated { get; set; }

		private protected override async Task InitializeContextAsync(IJSObjectReference module)
		{
			var id = await CreateJsContextAsync(module, "2d", null);
			if (id == 0)
			{
				return;
			}

			Context = new Canvas2DContext(module, id);
			_contextBase = Context;

			if (OnContextCreated.HasDelegate)
			{
				await OnContextCreated.InvokeAsync(Context);
			}
		}
	}
}
