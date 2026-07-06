using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Components.Canvas
{
	/// <summary>
	/// .NET wrapper of the JS <c>CanvasRenderingContext2D</c> of a <see cref="Canvas2D"/> component.
	/// All drawing commands are forwarded via JS interop; use <see cref="CanvasContextBase.BeginBatch"/> and
	/// <see cref="CanvasContextBase.EndBatchAsync"/> to execute many commands in a single interop round trip.
	/// </summary>
	public sealed class Canvas2DContext : CanvasContextBase
	{
		internal Canvas2DContext(IJSObjectReference module, int id)
			: base(module, id)
		{
		}

		#region State

		/// <summary>Saves the entire canvas state (styles, transform, clip) onto a stack.</summary>
		public Task SaveAsync() => CallVoidAsync("save");

		/// <summary>Restores the most recently saved canvas state.</summary>
		public Task RestoreAsync() => CallVoidAsync("restore");

		#endregion

		#region Styles

		/// <summary>Sets the fill style to a CSS color, e.g. "red", "#ff0000", "rgba(255,0,0,0.5)".</summary>
		public Task SetFillStyleAsync(string color) => SetAsync("fillStyle", color);

		/// <summary>Sets the fill style to a gradient.</summary>
		public Task SetFillStyleAsync(CanvasGradient gradient) => SetAsync("fillStyle", gradient);

		/// <summary>Sets the fill style to a pattern.</summary>
		public Task SetFillStyleAsync(CanvasPattern pattern) => SetAsync("fillStyle", pattern);

		/// <summary>Sets the stroke style to a CSS color.</summary>
		public Task SetStrokeStyleAsync(string color) => SetAsync("strokeStyle", color);

		/// <summary>Sets the stroke style to a gradient.</summary>
		public Task SetStrokeStyleAsync(CanvasGradient gradient) => SetAsync("strokeStyle", gradient);

		/// <summary>Sets the stroke style to a pattern.</summary>
		public Task SetStrokeStyleAsync(CanvasPattern pattern) => SetAsync("strokeStyle", pattern);

		/// <summary>Sets the line width in pixels.</summary>
		public Task SetLineWidthAsync(double width) => SetAsync("lineWidth", width);

		/// <summary>Sets the shape of line endings.</summary>
		public Task SetLineCapAsync(LineCap cap) => SetAsync("lineCap", cap.ToString().ToLowerInvariant());

		/// <summary>Sets the shape of line joints.</summary>
		public Task SetLineJoinAsync(LineJoin join) => SetAsync("lineJoin", join.ToString().ToLowerInvariant());

		/// <summary>Sets the miter limit ratio used with <see cref="LineJoin.Miter"/>.</summary>
		public Task SetMiterLimitAsync(double limit) => SetAsync("miterLimit", limit);

		/// <summary>Sets the line dash pattern: alternating lengths of lines and gaps, empty array for solid lines.</summary>
		public Task SetLineDashAsync(double[] segments) => CallVoidAsync("setLineDash", segments);

		/// <summary>Sets the phase offset of the line dash pattern.</summary>
		public Task SetLineDashOffsetAsync(double offset) => SetAsync("lineDashOffset", offset);

		/// <summary>Sets the global alpha (0..1) applied to all drawing operations.</summary>
		public Task SetGlobalAlphaAsync(double alpha) => SetAsync("globalAlpha", alpha);

		/// <summary>
		/// Sets the compositing (blend) mode, e.g. "source-over" (default), "multiply", "screen", "destination-out".
		/// </summary>
		public Task SetGlobalCompositeOperationAsync(string operation) => SetAsync("globalCompositeOperation", operation);

		/// <summary>Sets a CSS filter applied to drawing operations, e.g. "blur(4px)" or "none".</summary>
		public Task SetFilterAsync(string filter) => SetAsync("filter", filter);

		/// <summary>Sets the shadow blur level (not pixels, default 0).</summary>
		public Task SetShadowBlurAsync(double blur) => SetAsync("shadowBlur", blur);

		/// <summary>Sets the shadow color (CSS color, default fully transparent black).</summary>
		public Task SetShadowColorAsync(string color) => SetAsync("shadowColor", color);

		/// <summary>Sets the horizontal shadow offset in pixels.</summary>
		public Task SetShadowOffsetXAsync(double offset) => SetAsync("shadowOffsetX", offset);

		/// <summary>Sets the vertical shadow offset in pixels.</summary>
		public Task SetShadowOffsetYAsync(double offset) => SetAsync("shadowOffsetY", offset);

		/// <summary>Enables or disables image smoothing when scaling images (default true).</summary>
		public Task SetImageSmoothingEnabledAsync(bool enabled) => SetAsync("imageSmoothingEnabled", enabled);

		#endregion

		#region Gradients and patterns

		/// <summary>Creates a linear gradient along the line (x0,y0)-(x1,y1). Add stops with <see cref="CanvasGradient.AddColorStopAsync"/>.</summary>
		public async Task<CanvasGradient> CreateLinearGradientAsync(double x0, double y0, double x1, double y1)
			=> new CanvasGradient((await CallHandleResultAsync("createLinearGradient", x0, y0, x1, y1))!.Value, this);

		/// <summary>Creates a radial gradient between two circles.</summary>
		public async Task<CanvasGradient> CreateRadialGradientAsync(double x0, double y0, double r0, double x1, double y1, double r1)
			=> new CanvasGradient((await CallHandleResultAsync("createRadialGradient", x0, y0, r0, x1, y1, r1))!.Value, this);

		/// <summary>Creates a conic gradient around the point (x,y) starting at the given angle in radians.</summary>
		public async Task<CanvasGradient> CreateConicGradientAsync(double startAngle, double x, double y)
			=> new CanvasGradient((await CallHandleResultAsync("createConicGradient", startAngle, x, y))!.Value, this);

		/// <summary>Creates a repeating pattern from an image URL.</summary>
		public async Task<CanvasPattern> CreatePatternAsync(string imageUrl, PatternRepetition repetition = PatternRepetition.Repeat)
		{
			var repeat = repetition switch
			{
				PatternRepetition.RepeatX => "repeat-x",
				PatternRepetition.RepeatY => "repeat-y",
				PatternRepetition.NoRepeat => "no-repeat",
				_ => "repeat",
			};

			var dto = await InvokeHelperAsync<JsHandleDto>("createPatternUrl", imageUrl, repeat);
			return new CanvasPattern(dto.Handle);
		}

		#endregion

		#region Rectangles

		/// <summary>Draws a filled rectangle with the current fill style.</summary>
		public Task FillRectAsync(double x, double y, double width, double height) => CallVoidAsync("fillRect", x, y, width, height);

		/// <summary>Draws a rectangle outline with the current stroke style.</summary>
		public Task StrokeRectAsync(double x, double y, double width, double height) => CallVoidAsync("strokeRect", x, y, width, height);

		/// <summary>Erases the given rectangle to transparent black.</summary>
		public Task ClearRectAsync(double x, double y, double width, double height) => CallVoidAsync("clearRect", x, y, width, height);

		#endregion

		#region Paths

		/// <summary>Starts a new path, emptying the current sub-path list.</summary>
		public Task BeginPathAsync() => CallVoidAsync("beginPath");

		/// <summary>Connects the last point back to the start of the current sub-path.</summary>
		public Task ClosePathAsync() => CallVoidAsync("closePath");

		/// <summary>Moves the pen to (x,y) without drawing.</summary>
		public Task MoveToAsync(double x, double y) => CallVoidAsync("moveTo", x, y);

		/// <summary>Adds a straight line to (x,y).</summary>
		public Task LineToAsync(double x, double y) => CallVoidAsync("lineTo", x, y);

		/// <summary>Adds a circular arc centered at (x,y). Angles are in radians.</summary>
		public Task ArcAsync(double x, double y, double radius, double startAngle, double endAngle, bool counterclockwise = false)
			=> CallVoidAsync("arc", x, y, radius, startAngle, endAngle, counterclockwise);

		/// <summary>Adds an arc connecting two tangent lines defined by the control points.</summary>
		public Task ArcToAsync(double x1, double y1, double x2, double y2, double radius)
			=> CallVoidAsync("arcTo", x1, y1, x2, y2, radius);

		/// <summary>Adds an elliptical arc centered at (x,y). Angles are in radians.</summary>
		public Task EllipseAsync(double x, double y, double radiusX, double radiusY, double rotation, double startAngle, double endAngle, bool counterclockwise = false)
			=> CallVoidAsync("ellipse", x, y, radiusX, radiusY, rotation, startAngle, endAngle, counterclockwise);

		/// <summary>Adds a rectangle sub-path.</summary>
		public Task RectAsync(double x, double y, double width, double height) => CallVoidAsync("rect", x, y, width, height);

		/// <summary>Adds a rounded rectangle sub-path with a single corner radius.</summary>
		public Task RoundRectAsync(double x, double y, double width, double height, double radius)
			=> CallVoidAsync("roundRect", x, y, width, height, radius);

		/// <summary>Adds a rounded rectangle sub-path with per-corner radii (1-4 values, CSS corner order).</summary>
		public Task RoundRectAsync(double x, double y, double width, double height, double[] radii)
			=> CallVoidAsync("roundRect", x, y, width, height, radii);

		/// <summary>Adds a cubic Bézier curve to (x,y) using two control points.</summary>
		public Task BezierCurveToAsync(double cp1x, double cp1y, double cp2x, double cp2y, double x, double y)
			=> CallVoidAsync("bezierCurveTo", cp1x, cp1y, cp2x, cp2y, x, y);

		/// <summary>Adds a quadratic Bézier curve to (x,y) using one control point.</summary>
		public Task QuadraticCurveToAsync(double cpx, double cpy, double x, double y)
			=> CallVoidAsync("quadraticCurveTo", cpx, cpy, x, y);

		/// <summary>Fills the current path with the current fill style.</summary>
		public Task FillAsync() => CallVoidAsync("fill");

		/// <summary>Strokes the current path with the current stroke style.</summary>
		public Task StrokeAsync() => CallVoidAsync("stroke");

		/// <summary>Turns the current path into the clipping region.</summary>
		public Task ClipAsync() => CallVoidAsync("clip");

		/// <summary>Returns whether the given point is inside the current path.</summary>
		public Task<bool> IsPointInPathAsync(double x, double y) => CallAsync<bool>("isPointInPath", x, y);

		/// <summary>Returns whether the given point is on the stroke of the current path.</summary>
		public Task<bool> IsPointInStrokeAsync(double x, double y) => CallAsync<bool>("isPointInStroke", x, y);

		#endregion

		#region Text

		/// <summary>Sets the font in CSS shorthand, e.g. "bold 16px Arial".</summary>
		public Task SetFontAsync(string font) => SetAsync("font", font);

		/// <summary>Sets the horizontal text alignment.</summary>
		public Task SetTextAlignAsync(TextAlign align) => SetAsync("textAlign", align.ToString().ToLowerInvariant());

		/// <summary>Sets the vertical text baseline.</summary>
		public Task SetTextBaselineAsync(TextBaseline baseline) => SetAsync("textBaseline", baseline.ToString().ToLowerInvariant());

		/// <summary>Draws filled text at (x,y). Optional maxWidth compresses the text horizontally.</summary>
		public Task FillTextAsync(string text, double x, double y, double? maxWidth = null)
			=> maxWidth is null ? CallVoidAsync("fillText", text, x, y) : CallVoidAsync("fillText", text, x, y, maxWidth);

		/// <summary>Draws text outline at (x,y).</summary>
		public Task StrokeTextAsync(string text, double x, double y, double? maxWidth = null)
			=> maxWidth is null ? CallVoidAsync("strokeText", text, x, y) : CallVoidAsync("strokeText", text, x, y, maxWidth);

		/// <summary>Returns the width of the given text in pixels using the current font.</summary>
		public Task<double> MeasureTextAsync(string text) => InvokeHelperAsync<double>("measureText", text);

		#endregion

		#region Transforms

		/// <summary>Moves the canvas origin by (x,y).</summary>
		public Task TranslateAsync(double x, double y) => CallVoidAsync("translate", x, y);

		/// <summary>Rotates the canvas around the current origin. Angle is in radians.</summary>
		public Task RotateAsync(double angle) => CallVoidAsync("rotate", angle);

		/// <summary>Scales the canvas units.</summary>
		public Task ScaleAsync(double x, double y) => CallVoidAsync("scale", x, y);

		/// <summary>Multiplies the current transform with the given matrix (a b c d e f).</summary>
		public Task TransformAsync(double a, double b, double c, double d, double e, double f)
			=> CallVoidAsync("transform", a, b, c, d, e, f);

		/// <summary>Replaces the current transform with the given matrix (a b c d e f).</summary>
		public Task SetTransformAsync(double a, double b, double c, double d, double e, double f)
			=> CallVoidAsync("setTransform", a, b, c, d, e, f);

		/// <summary>Resets the current transform to the identity matrix.</summary>
		public Task ResetTransformAsync() => CallVoidAsync("resetTransform");

		#endregion

		#region Images and pixels

		/// <summary>Draws an image loaded from URL at (dx,dy) in its natural size.</summary>
		public Task DrawImageAsync(string imageUrl, double dx, double dy)
			=> InvokeHelperVoidAsync("drawImageUrl", imageUrl, new object[] { dx, dy });

		/// <summary>Draws an image loaded from URL scaled into the destination rectangle.</summary>
		public Task DrawImageAsync(string imageUrl, double dx, double dy, double dWidth, double dHeight)
			=> InvokeHelperVoidAsync("drawImageUrl", imageUrl, new object[] { dx, dy, dWidth, dHeight });

		/// <summary>Draws a cropped region of an image loaded from URL scaled into the destination rectangle.</summary>
		public Task DrawImageAsync(string imageUrl, double sx, double sy, double sWidth, double sHeight, double dx, double dy, double dWidth, double dHeight)
			=> InvokeHelperVoidAsync("drawImageUrl", imageUrl, new object[] { sx, sy, sWidth, sHeight, dx, dy, dWidth, dHeight });

		/// <summary>Draws another element (e.g. an image, video or other canvas by Blazor <see cref="ElementReference"/>) at (dx,dy).</summary>
		public Task DrawImageAsync(ElementReference element, double dx, double dy)
			=> InvokeHelperVoidAsync("drawImageElement", element, new object[] { dx, dy });

		/// <summary>Draws another element scaled into the destination rectangle.</summary>
		public Task DrawImageAsync(ElementReference element, double dx, double dy, double dWidth, double dHeight)
			=> InvokeHelperVoidAsync("drawImageElement", element, new object[] { dx, dy, dWidth, dHeight });

		/// <summary>
		/// Returns the raw RGBA pixel bytes (4 bytes per pixel, row-major) of the given canvas region.
		/// </summary>
		public Task<byte[]> GetImageDataAsync(int x, int y, int width, int height)
			=> InvokeHelperAsync<byte[]>("getImageData", x, y, width, height);

		/// <summary>
		/// Writes raw RGBA pixel bytes (4 bytes per pixel, row-major) to the canvas at (dx,dy).
		/// </summary>
		public Task PutImageDataAsync(byte[] data, int width, int height, int dx, int dy)
			=> InvokeHelperVoidAsync("putImageData", data, width, height, dx, dy);

		#endregion
	}
}
