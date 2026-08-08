namespace Majorsoft.Blazor.Components.Canvas.WebGL
{
	/// <summary>.NET reference to a JS <c>WebGLBuffer</c> object.</summary>
	public sealed class WebGLBuffer : JsObjectRef
	{
		internal WebGLBuffer(int handle) : base(handle) { }
	}

	/// <summary>.NET reference to a JS <c>WebGLShader</c> object.</summary>
	public sealed class WebGLShader : JsObjectRef
	{
		internal WebGLShader(int handle) : base(handle) { }
	}

	/// <summary>.NET reference to a JS <c>WebGLProgram</c> object.</summary>
	public sealed class WebGLProgram : JsObjectRef
	{
		internal WebGLProgram(int handle) : base(handle) { }
	}

	/// <summary>.NET reference to a JS <c>WebGLTexture</c> object.</summary>
	public sealed class WebGLTexture : JsObjectRef
	{
		internal WebGLTexture(int handle) : base(handle) { }
	}

	/// <summary>.NET reference to a JS <c>WebGLUniformLocation</c> object.</summary>
	public sealed class WebGLUniformLocation : JsObjectRef
	{
		internal WebGLUniformLocation(int handle) : base(handle) { }
	}

	/// <summary>.NET reference to a JS <c>WebGLVertexArrayObject</c> (WebGL2, or WebGL 1.0 via the OES_vertex_array_object extension).</summary>
	public sealed class WebGLVertexArrayObject : JsObjectRef
	{
		internal WebGLVertexArrayObject(int handle) : base(handle) { }
	}

	/// <summary>.NET reference to a JS <c>WebGLFramebuffer</c> object.</summary>
	public sealed class WebGLFramebuffer : JsObjectRef
	{
		internal WebGLFramebuffer(int handle) : base(handle) { }
	}

	/// <summary>.NET reference to a JS <c>WebGLRenderbuffer</c> object.</summary>
	public sealed class WebGLRenderbuffer : JsObjectRef
	{
		internal WebGLRenderbuffer(int handle) : base(handle) { }
	}

	/// <summary>Requested WebGL context version of the <see cref="WebGLCanvas"/> component.</summary>
	public enum WebGLContextType
	{
		/// <summary>WebGL 2.0 (OpenGL ES 3.0 based). Falls back to WebGL 1.0 when not supported.</summary>
		WebGL2,
		/// <summary>WebGL 1.0 (OpenGL ES 2.0 based).</summary>
		WebGL
	}

	/// <summary>
	/// WebGL context creation attributes, see the JS <c>canvas.getContext()</c> <c>WebGLContextAttributes</c> parameter.
	/// Property names are serialized camelCased to match the JS attribute names.
	/// </summary>
	public class WebGLContextAttributes
	{
		/// <summary>Whether the drawing buffer has an alpha channel. Default is true.</summary>
		public bool Alpha { get; set; } = true;

		/// <summary>Whether antialiasing is performed when supported. Default is true.</summary>
		public bool Antialias { get; set; } = true;

		/// <summary>Whether the drawing buffer has a depth buffer of at least 16 bits. Default is true.</summary>
		public bool Depth { get; set; } = true;

		/// <summary>Whether the drawing buffer has a stencil buffer of at least 8 bits. Default is false.</summary>
		public bool Stencil { get; set; }

		/// <summary>Whether the page compositor assumes premultiplied alpha in the drawing buffer. Default is true.</summary>
		public bool PremultipliedAlpha { get; set; } = true;

		/// <summary>
		/// Whether the drawing buffer content is preserved between frames. Default is false.
		/// Set to true when the canvas content is read back (e.g. <c>ToDataURLAsync</c>) outside the render loop.
		/// </summary>
		public bool PreserveDrawingBuffer { get; set; }

		/// <summary>GPU power preference: "default", "high-performance" or "low-power". Default is "default".</summary>
		public string PowerPreference { get; set; } = "default";
	}
}
