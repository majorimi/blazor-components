using System.Threading.Tasks;

namespace Majorsoft.Blazor.Components.Canvas
{
	/// <summary>
	/// .NET reference to a JS <c>CanvasGradient</c> created by
	/// <see cref="Canvas2DContext.CreateLinearGradientAsync"/>, <see cref="Canvas2DContext.CreateRadialGradientAsync"/>
	/// or <see cref="Canvas2DContext.CreateConicGradientAsync"/>. Can be used as fill or stroke style.
	/// </summary>
	public sealed class CanvasGradient : JsObjectRef
	{
		private readonly Canvas2DContext _context;

		internal CanvasGradient(int handle, Canvas2DContext context) : base(handle)
		{
			_context = context;
		}

		/// <summary>Adds a color stop to the gradient at the given offset (0..1).</summary>
		public async Task AddColorStopAsync(double offset, string color)
			=> await _context.CallOnHandleVoidAsync(this, "addColorStop", offset, color);
	}

	/// <summary>
	/// .NET reference to a JS <c>CanvasPattern</c> created by <see cref="Canvas2DContext.CreatePatternAsync"/>.
	/// Can be used as fill or stroke style.
	/// </summary>
	public sealed class CanvasPattern : JsObjectRef
	{
		internal CanvasPattern(int handle) : base(handle)
		{
		}
	}

	/// <summary>Shape of line endings, value of the JS <c>lineCap</c> property.</summary>
	public enum LineCap
	{
		/// <summary>Lines end exactly at the endpoint.</summary>
		Butt,
		/// <summary>Lines end with a semicircle.</summary>
		Round,
		/// <summary>Lines end with a half square extension.</summary>
		Square
	}

	/// <summary>Shape of line joints, value of the JS <c>lineJoin</c> property.</summary>
	public enum LineJoin
	{
		/// <summary>Sharp corner.</summary>
		Miter,
		/// <summary>Rounded corner.</summary>
		Round,
		/// <summary>Flattened corner.</summary>
		Bevel
	}

	/// <summary>Horizontal text alignment, value of the JS <c>textAlign</c> property.</summary>
	public enum TextAlign
	{
		/// <summary>Text starts at the given position (writing direction aware).</summary>
		Start,
		/// <summary>Text ends at the given position (writing direction aware).</summary>
		End,
		/// <summary>Text is left aligned to the given position.</summary>
		Left,
		/// <summary>Text is right aligned to the given position.</summary>
		Right,
		/// <summary>Text is centered on the given position.</summary>
		Center
	}

	/// <summary>Vertical text alignment, value of the JS <c>textBaseline</c> property.</summary>
	public enum TextBaseline
	{
		/// <summary>Top of the em square.</summary>
		Top,
		/// <summary>Hanging baseline (used by Tibetan and other Indic scripts).</summary>
		Hanging,
		/// <summary>Middle of the em square.</summary>
		Middle,
		/// <summary>Normal alphabetic baseline, the default.</summary>
		Alphabetic,
		/// <summary>Ideographic baseline (used by CJK scripts).</summary>
		Ideographic,
		/// <summary>Bottom of the em square.</summary>
		Bottom
	}

	/// <summary>Pattern repetition mode of <see cref="Canvas2DContext.CreatePatternAsync"/>.</summary>
	public enum PatternRepetition
	{
		/// <summary>Repeats in both directions, the default.</summary>
		Repeat,
		/// <summary>Repeats horizontally only.</summary>
		RepeatX,
		/// <summary>Repeats vertically only.</summary>
		RepeatY,
		/// <summary>No repetition.</summary>
		NoRepeat
	}

	/// <summary>
	/// Size of the canvas drawing buffer in pixels.
	/// </summary>
	public sealed class CanvasSize
	{
		/// <summary>Width of the drawing buffer in pixels.</summary>
		public int Width { get; set; }

		/// <summary>Height of the drawing buffer in pixels.</summary>
		public int Height { get; set; }
	}

	/// <summary>
	/// Event args of the render loop frame callback (<c>OnFrame</c>).
	/// </summary>
	public sealed class CanvasFrameEventArgs
	{
		/// <summary>Elapsed time since the previous frame in seconds.</summary>
		public double DeltaTime { get; }

		/// <summary>Elapsed time since the render loop started in seconds.</summary>
		public double TotalTime { get; }

		/// <summary>
		/// Input state snapshot taken with this frame. Null unless input capture was started with
		/// <c>StartInputCaptureAsync</c>. Delivered with the frame callback, so reading it costs no extra interop call.
		/// </summary>
		public CanvasInputState? Input { get; }

		internal CanvasFrameEventArgs(double deltaTime, double totalTime, CanvasInputState? input)
		{
			DeltaTime = deltaTime;
			TotalTime = totalTime;
			Input = input;
		}
	}
}
