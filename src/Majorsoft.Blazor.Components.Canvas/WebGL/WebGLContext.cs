using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Components.Canvas.WebGL
{
	/// <summary>
	/// .NET wrapper of the JS <c>WebGLRenderingContext</c> / <c>WebGL2RenderingContext</c> of a
	/// <see cref="WebGLCanvas"/> component. Exposes the core WebGL API (buffers, shaders, programs, uniforms,
	/// textures, draw calls) via JS interop with constants in <see cref="GL"/>. Use
	/// <see cref="CanvasContextBase.BeginBatch"/> / <see cref="CanvasContextBase.EndBatchAsync"/> to send
	/// per-frame command streams in a single interop round trip.
	/// </summary>
	public sealed class WebGLContext : CanvasContextBase
	{
		/// <summary>True when the created context is WebGL 2.0, false when WebGL 1.0.</summary>
		public bool IsWebGL2 { get; }

		internal WebGLContext(IJSObjectReference module, int id, bool isWebGL2)
			: base(module, id)
		{
			IsWebGL2 = isWebGL2;
		}

		#region Global state

		/// <summary>Sets the color used by <see cref="ClearAsync"/>. Components are 0..1.</summary>
		public Task ClearColorAsync(float red, float green, float blue, float alpha) => CallVoidAsync("clearColor", red, green, blue, alpha);

		/// <summary>Clears the given buffers, e.g. <c>GL.COLOR_BUFFER_BIT | GL.DEPTH_BUFFER_BIT</c>.</summary>
		public Task ClearAsync(uint mask) => CallVoidAsync("clear", mask);

		/// <summary>Sets the viewport transform from clip space to canvas pixels.</summary>
		public Task ViewportAsync(int x, int y, int width, int height) => CallVoidAsync("viewport", x, y, width, height);

		/// <summary>Enables a capability, e.g. <c>GL.DEPTH_TEST</c> or <c>GL.BLEND</c>.</summary>
		public Task EnableAsync(uint capability) => CallVoidAsync("enable", capability);

		/// <summary>Disables a capability.</summary>
		public Task DisableAsync(uint capability) => CallVoidAsync("disable", capability);

		/// <summary>Sets the source and destination blend factors.</summary>
		public Task BlendFuncAsync(uint sourceFactor, uint destinationFactor) => CallVoidAsync("blendFunc", sourceFactor, destinationFactor);

		/// <summary>Sets the depth comparison function, e.g. <c>GL.LEQUAL</c>.</summary>
		public Task DepthFuncAsync(uint func) => CallVoidAsync("depthFunc", func);

		/// <summary>Sets which face is culled, e.g. <c>GL.BACK</c>.</summary>
		public Task CullFaceAsync(uint mode) => CallVoidAsync("cullFace", mode);

		/// <summary>Sets the front face winding order, <c>GL.CCW</c> (default) or <c>GL.CW</c>.</summary>
		public Task FrontFaceAsync(uint mode) => CallVoidAsync("frontFace", mode);

		/// <summary>Sets the rasterized line width (support above 1 is driver dependent).</summary>
		public Task LineWidthAsync(float width) => CallVoidAsync("lineWidth", width);

		/// <summary>Sets a pixel storage parameter, e.g. <c>GL.UNPACK_FLIP_Y_WEBGL</c>.</summary>
		public Task PixelStoreiAsync(uint parameter, int value) => CallVoidAsync("pixelStorei", parameter, value);

		/// <summary>Restricts drawing to the given rectangle when <c>GL.SCISSOR_TEST</c> is enabled.</summary>
		public Task ScissorAsync(int x, int y, int width, int height) => CallVoidAsync("scissor", x, y, width, height);

		/// <summary>Enables or disables writing of the individual color channels.</summary>
		public Task ColorMaskAsync(bool red, bool green, bool blue, bool alpha) => CallVoidAsync("colorMask", red, green, blue, alpha);

		/// <summary>Enables or disables writing into the depth buffer.</summary>
		public Task DepthMaskAsync(bool enabled) => CallVoidAsync("depthMask", enabled);

		/// <summary>Sets the depth value used by <see cref="ClearAsync"/> (0..1, default 1).</summary>
		public Task ClearDepthAsync(float depth) => CallVoidAsync("clearDepth", depth);

		/// <summary>Sets the stencil value used by <see cref="ClearAsync"/> (default 0).</summary>
		public Task ClearStencilAsync(int stencil) => CallVoidAsync("clearStencil", stencil);

		/// <summary>Sets the stencil test function, reference value and mask.</summary>
		public Task StencilFuncAsync(uint func, int reference, uint mask) => CallVoidAsync("stencilFunc", func, reference, mask);

		/// <summary>Sets the stencil actions for fail, zfail and zpass, e.g. <c>GL.KEEP</c>, <c>GL.REPLACE</c>, <c>GL.INCR</c>.</summary>
		public Task StencilOpAsync(uint fail, uint zfail, uint zpass) => CallVoidAsync("stencilOp", fail, zfail, zpass);

		/// <summary>Sets the bit mask enabling writing of individual stencil bits.</summary>
		public Task StencilMaskAsync(uint mask) => CallVoidAsync("stencilMask", mask);

		/// <summary>Sets the blend equation, e.g. <c>GL.FUNC_ADD</c> (default) or <c>GL.FUNC_REVERSE_SUBTRACT</c>.</summary>
		public Task BlendEquationAsync(uint mode) => CallVoidAsync("blendEquation", mode);

		/// <summary>Sets separate blend factors for RGB and alpha.</summary>
		public Task BlendFuncSeparateAsync(uint srcRgb, uint dstRgb, uint srcAlpha, uint dstAlpha)
			=> CallVoidAsync("blendFuncSeparate", srcRgb, dstRgb, srcAlpha, dstAlpha);

		/// <summary>Sets the constant blend color used with <c>GL.CONSTANT_COLOR</c> factors.</summary>
		public Task BlendColorAsync(float red, float green, float blue, float alpha) => CallVoidAsync("blendColor", red, green, blue, alpha);

		/// <summary>Sets the polygon offset factor and units used with <c>GL.POLYGON_OFFSET_FILL</c>.</summary>
		public Task PolygonOffsetAsync(float factor, float units) => CallVoidAsync("polygonOffset", factor, units);

		/// <summary>Sets multi-sample coverage parameters.</summary>
		public Task SampleCoverageAsync(float value, bool invert) => CallVoidAsync("sampleCoverage", value, invert);

		/// <summary>Sets an implementation hint, e.g. <c>HintAsync(GL.GENERATE_MIPMAP_HINT, GL.NICEST)</c>.</summary>
		public Task HintAsync(uint target, uint mode) => CallVoidAsync("hint", target, mode);

		/// <summary>Returns whether the given capability is enabled.</summary>
		public Task<bool> IsEnabledAsync(uint capability) => CallAsync<bool>("isEnabled", capability);

		/// <summary>Returns the first error flag raised since the last call, <c>GL.NO_ERROR</c> when none.</summary>
		public Task<uint> GetErrorAsync() => CallAsync<uint>("getError");

		/// <summary>Enables the given WebGL extension and returns whether it is supported, e.g. "OES_texture_float".</summary>
		public Task<bool> GetExtensionAsync(string name) => InvokeHelperAsync<bool>("hasExtension", name);

		/// <summary>Returns the names of all supported WebGL extensions.</summary>
		public Task<string[]> GetSupportedExtensionsAsync() => CallAsync<string[]>("getSupportedExtensions");

		/// <summary>Returns a context parameter, e.g. <c>GetParameterAsync&lt;string&gt;(GL.VERSION)</c>.</summary>
		public Task<T> GetParameterAsync<T>(uint parameter) => CallAsync<T>("getParameter", parameter);

		/// <summary>Returns the current width and height of the drawing buffer in pixels.</summary>
		public async Task<(int Width, int Height)> GetDrawingBufferSizeAsync()
			=> (await GetAsync<int>("drawingBufferWidth"), await GetAsync<int>("drawingBufferHeight"));

		#endregion

		#region Buffers

		/// <summary>Creates a GPU buffer object.</summary>
		public async Task<WebGLBuffer> CreateBufferAsync()
			=> new WebGLBuffer((await CallHandleResultAsync("createBuffer"))!.Value);

		/// <summary>Binds a buffer to a target (<c>GL.ARRAY_BUFFER</c> or <c>GL.ELEMENT_ARRAY_BUFFER</c>). Null unbinds.</summary>
		public Task BindBufferAsync(uint target, WebGLBuffer? buffer) => CallVoidAsync("bindBuffer", target, buffer);

		/// <summary>Uploads float vertex data to the bound buffer (JS Float32Array).</summary>
		public Task BufferDataAsync(uint target, float[] data, uint usage)
			=> CallVoidAsync("bufferData", target, new Float32ArrayArg { Data = data }, usage);

		/// <summary>Uploads unsigned short index data to the bound buffer (JS Uint16Array).</summary>
		public Task BufferDataAsync(uint target, ushort[] data, uint usage)
			=> CallVoidAsync("bufferData", target, new Uint16ArrayArg { Data = data.Select(x => (int)x).ToArray() }, usage);

		/// <summary>Uploads int data to the bound buffer (JS Int32Array).</summary>
		public Task BufferDataAsync(uint target, int[] data, uint usage)
			=> CallVoidAsync("bufferData", target, new Int32ArrayArg { Data = data }, usage);

		/// <summary>Allocates an uninitialized buffer of the given byte size.</summary>
		public Task BufferDataAsync(uint target, int sizeInBytes, uint usage)
			=> CallVoidAsync("bufferData", target, sizeInBytes, usage);

		/// <summary>Updates a region of the bound buffer with float data starting at the given byte offset.</summary>
		public Task BufferSubDataAsync(uint target, int offsetInBytes, float[] data)
			=> CallVoidAsync("bufferSubData", target, offsetInBytes, new Float32ArrayArg { Data = data });

		/// <summary>Updates a region of the bound buffer with unsigned short data starting at the given byte offset.</summary>
		public Task BufferSubDataAsync(uint target, int offsetInBytes, ushort[] data)
			=> CallVoidAsync("bufferSubData", target, offsetInBytes, new Uint16ArrayArg { Data = data.Select(x => (int)x).ToArray() });

		/// <summary>Deletes a buffer object and releases its .NET handle.</summary>
		public async Task DeleteBufferAsync(WebGLBuffer buffer)
		{
			await CallVoidAsync("deleteBuffer", buffer);
			await ReleaseHandleAsync(buffer);
		}

		#endregion

		#region Shaders and programs

		/// <summary>
		/// Convenience method: compiles the vertex and fragment shader sources and links them into a program
		/// in one JS interop call. Throws <see cref="JSException"/> with the shader/program info log on failure.
		/// </summary>
		public async Task<WebGLProgram> CompileProgramAsync(string vertexShaderSource, string fragmentShaderSource)
		{
			var dto = await InvokeHelperAsync<JsHandleDto>("compileProgram", vertexShaderSource, fragmentShaderSource);
			return new WebGLProgram(dto.Handle);
		}

		/// <summary>Creates a shader object of the given type (<c>GL.VERTEX_SHADER</c> or <c>GL.FRAGMENT_SHADER</c>).</summary>
		public async Task<WebGLShader> CreateShaderAsync(uint type)
			=> new WebGLShader((await CallHandleResultAsync("createShader", type))!.Value);

		/// <summary>Sets the GLSL source code of a shader.</summary>
		public Task ShaderSourceAsync(WebGLShader shader, string source) => CallVoidAsync("shaderSource", shader, source);

		/// <summary>Compiles a shader. Check <c>GetShaderParameterAsync&lt;bool&gt;(shader, GL.COMPILE_STATUS)</c>.</summary>
		public Task CompileShaderAsync(WebGLShader shader) => CallVoidAsync("compileShader", shader);

		/// <summary>Returns a shader parameter, e.g. compile status.</summary>
		public Task<T> GetShaderParameterAsync<T>(WebGLShader shader, uint parameter) => CallAsync<T>("getShaderParameter", shader, parameter);

		/// <summary>Returns the compile log of a shader.</summary>
		public Task<string> GetShaderInfoLogAsync(WebGLShader shader) => CallAsync<string>("getShaderInfoLog", shader);

		/// <summary>Deletes a shader object and releases its .NET handle.</summary>
		public async Task DeleteShaderAsync(WebGLShader shader)
		{
			await CallVoidAsync("deleteShader", shader);
			await ReleaseHandleAsync(shader);
		}

		/// <summary>Creates an empty program object.</summary>
		public async Task<WebGLProgram> CreateProgramAsync()
			=> new WebGLProgram((await CallHandleResultAsync("createProgram"))!.Value);

		/// <summary>Attaches a compiled shader to a program.</summary>
		public Task AttachShaderAsync(WebGLProgram program, WebGLShader shader) => CallVoidAsync("attachShader", program, shader);

		/// <summary>Links the attached shaders into an executable program.</summary>
		public Task LinkProgramAsync(WebGLProgram program) => CallVoidAsync("linkProgram", program);

		/// <summary>Returns a program parameter, e.g. link status.</summary>
		public Task<T> GetProgramParameterAsync<T>(WebGLProgram program, uint parameter) => CallAsync<T>("getProgramParameter", program, parameter);

		/// <summary>Returns the link log of a program.</summary>
		public Task<string> GetProgramInfoLogAsync(WebGLProgram program) => CallAsync<string>("getProgramInfoLog", program);

		/// <summary>Makes the program the active one for subsequent draw calls.</summary>
		public Task UseProgramAsync(WebGLProgram? program) => CallVoidAsync("useProgram", program);

		/// <summary>Deletes a program object and releases its .NET handle.</summary>
		public async Task DeleteProgramAsync(WebGLProgram program)
		{
			await CallVoidAsync("deleteProgram", program);
			await ReleaseHandleAsync(program);
		}

		#endregion

		#region Attributes and uniforms

		/// <summary>Returns the location index of a vertex attribute, -1 when not found.</summary>
		public Task<int> GetAttribLocationAsync(WebGLProgram program, string name) => CallAsync<int>("getAttribLocation", program, name);

		/// <summary>Returns the location of a uniform variable, null when not found or optimized away.</summary>
		public async Task<WebGLUniformLocation?> GetUniformLocationAsync(WebGLProgram program, string name)
		{
			var handle = await CallHandleResultAsync("getUniformLocation", program, name);
			return handle is null ? null : new WebGLUniformLocation(handle.Value);
		}

		/// <summary>Enables a vertex attribute array at the given location.</summary>
		public Task EnableVertexAttribArrayAsync(int index) => CallVoidAsync("enableVertexAttribArray", index);

		/// <summary>Disables a vertex attribute array at the given location.</summary>
		public Task DisableVertexAttribArrayAsync(int index) => CallVoidAsync("disableVertexAttribArray", index);

		/// <summary>
		/// Describes how the bound <c>GL.ARRAY_BUFFER</c> data is read for a vertex attribute:
		/// size components of the given type per vertex, with byte stride and offset.
		/// </summary>
		public Task VertexAttribPointerAsync(int index, int size, uint type, bool normalized, int stride, int offset)
			=> CallVoidAsync("vertexAttribPointer", index, size, type, normalized, stride, offset);

		/// <summary>Sets a float uniform.</summary>
		public Task UniformAsync(WebGLUniformLocation location, float x) => CallVoidAsync("uniform1f", location, x);

		/// <summary>Sets a vec2 uniform.</summary>
		public Task UniformAsync(WebGLUniformLocation location, float x, float y) => CallVoidAsync("uniform2f", location, x, y);

		/// <summary>Sets a vec3 uniform.</summary>
		public Task UniformAsync(WebGLUniformLocation location, float x, float y, float z) => CallVoidAsync("uniform3f", location, x, y, z);

		/// <summary>Sets a vec4 uniform.</summary>
		public Task UniformAsync(WebGLUniformLocation location, float x, float y, float z, float w) => CallVoidAsync("uniform4f", location, x, y, z, w);

		/// <summary>Sets an int (or sampler) uniform.</summary>
		public Task UniformAsync(WebGLUniformLocation location, int x) => CallVoidAsync("uniform1i", location, x);

		/// <summary>Sets a float array uniform (<c>float[]</c> in GLSL).</summary>
		public Task Uniform1fvAsync(WebGLUniformLocation location, float[] values)
			=> CallVoidAsync("uniform1fv", location, new Float32ArrayArg { Data = values });

		/// <summary>Sets a vec2 array uniform (2 floats per element).</summary>
		public Task Uniform2fvAsync(WebGLUniformLocation location, float[] values)
			=> CallVoidAsync("uniform2fv", location, new Float32ArrayArg { Data = values });

		/// <summary>Sets a vec3 array uniform (3 floats per element).</summary>
		public Task Uniform3fvAsync(WebGLUniformLocation location, float[] values)
			=> CallVoidAsync("uniform3fv", location, new Float32ArrayArg { Data = values });

		/// <summary>Sets a vec4 array uniform (4 floats per element).</summary>
		public Task Uniform4fvAsync(WebGLUniformLocation location, float[] values)
			=> CallVoidAsync("uniform4fv", location, new Float32ArrayArg { Data = values });

		/// <summary>Sets an int (or sampler) array uniform.</summary>
		public Task Uniform1ivAsync(WebGLUniformLocation location, int[] values)
			=> CallVoidAsync("uniform1iv", location, new Int32ArrayArg { Data = values });

		/// <summary>Sets a mat2 uniform from 4 floats in column-major order.</summary>
		public Task UniformMatrix2fvAsync(WebGLUniformLocation location, bool transpose, float[] matrix)
			=> CallVoidAsync("uniformMatrix2fv", location, transpose, new Float32ArrayArg { Data = matrix });

		/// <summary>Sets a mat3 uniform from 9 floats in column-major order.</summary>
		public Task UniformMatrix3fvAsync(WebGLUniformLocation location, bool transpose, float[] matrix)
			=> CallVoidAsync("uniformMatrix3fv", location, transpose, new Float32ArrayArg { Data = matrix });

		/// <summary>Sets a mat4 uniform from 16 floats in column-major order.</summary>
		public Task UniformMatrix4fvAsync(WebGLUniformLocation location, bool transpose, float[] matrix)
			=> CallVoidAsync("uniformMatrix4fv", location, transpose, new Float32ArrayArg { Data = matrix });

		#endregion

		#region Textures

		/// <summary>Creates a texture object.</summary>
		public async Task<WebGLTexture> CreateTextureAsync()
			=> new WebGLTexture((await CallHandleResultAsync("createTexture"))!.Value);

		/// <summary>Binds a texture to a target, e.g. <c>GL.TEXTURE_2D</c>. Null unbinds.</summary>
		public Task BindTextureAsync(uint target, WebGLTexture? texture) => CallVoidAsync("bindTexture", target, texture);

		/// <summary>Selects the active texture unit, e.g. <c>GL.TEXTURE0</c>.</summary>
		public Task ActiveTextureAsync(uint textureUnit) => CallVoidAsync("activeTexture", textureUnit);

		/// <summary>Sets an int texture parameter, e.g. min/mag filter or wrap mode.</summary>
		public Task TexParameteriAsync(uint target, uint parameter, uint value) => CallVoidAsync("texParameteri", target, parameter, value);

		/// <summary>Uploads an image loaded from URL to the texture bound to <c>GL.TEXTURE_2D</c> (RGBA).</summary>
		public Task TexImage2DAsync(string imageUrl, bool flipY = true) => TexImage2DAsync(GL.TEXTURE_2D, imageUrl, flipY);

		/// <summary>
		/// Uploads an image loaded from URL to the texture bound at the given target:
		/// <c>GL.TEXTURE_2D</c> or a cube map face (<c>GL.TEXTURE_CUBE_MAP_POSITIVE_X</c>, ...).
		/// </summary>
		public Task TexImage2DAsync(uint target, string imageUrl, bool flipY = true)
			=> InvokeHelperVoidAsync("texImage2DUrl", target, imageUrl, flipY);

		/// <summary>Uploads raw RGBA pixel bytes (4 bytes per pixel) to the texture bound to <c>GL.TEXTURE_2D</c>.</summary>
		public Task TexImage2DAsync(byte[] rgbaPixels, int width, int height)
			=> TexImage2DAsync(GL.TEXTURE_2D, 0, GL.RGBA, width, height, GL.RGBA, GL.UNSIGNED_BYTE, rgbaPixels);

		/// <summary>
		/// Full <c>texImage2D</c> overload. Pass null pixels to allocate an uninitialized texture,
		/// e.g. as a framebuffer color attachment for render to texture.
		/// </summary>
		public Task TexImage2DAsync(uint target, int level, uint internalFormat, int width, int height, uint format, uint type, byte[]? pixels)
			=> CallVoidAsync("texImage2D", target, level, internalFormat, width, height, 0, format, type,
				pixels is null ? null : new Uint8ArrayArg { Data = pixels.Select(x => (int)x).ToArray() });

		/// <summary>Updates a region of the texture bound at the given target with raw RGBA pixel bytes.</summary>
		public Task TexSubImage2DAsync(uint target, int level, int xOffset, int yOffset, int width, int height, byte[] rgbaPixels)
			=> CallVoidAsync("texSubImage2D", target, level, xOffset, yOffset, width, height, GL.RGBA, GL.UNSIGNED_BYTE,
				new Uint8ArrayArg { Data = rgbaPixels.Select(x => (int)x).ToArray() });

		/// <summary>Updates a region of the texture bound at the given target from an image loaded from URL.</summary>
		public Task TexSubImage2DAsync(uint target, int xOffset, int yOffset, string imageUrl, bool flipY = true)
			=> InvokeHelperVoidAsync("texSubImage2DUrl", target, xOffset, yOffset, imageUrl, flipY);

		/// <summary>Copies pixels from the current framebuffer into the texture bound at the given target.</summary>
		public Task CopyTexImage2DAsync(uint target, int level, uint internalFormat, int x, int y, int width, int height)
			=> CallVoidAsync("copyTexImage2D", target, level, internalFormat, x, y, width, height, 0);

		/// <summary>Generates the mipmap chain of the texture bound to the given target.</summary>
		public Task GenerateMipmapAsync(uint target) => CallVoidAsync("generateMipmap", target);

		/// <summary>
		/// Reads raw RGBA pixel bytes (4 bytes per pixel, bottom-up rows) from the current framebuffer
		/// (the canvas or a bound <see cref="WebGLFramebuffer"/>).
		/// </summary>
		public Task<byte[]> ReadPixelsAsync(int x, int y, int width, int height)
			=> InvokeHelperAsync<byte[]>("readPixels", x, y, width, height);

		/// <summary>Deletes a texture object and releases its .NET handle.</summary>
		public async Task DeleteTextureAsync(WebGLTexture texture)
		{
			await CallVoidAsync("deleteTexture", texture);
			await ReleaseHandleAsync(texture);
		}

		#endregion

		#region Framebuffers and renderbuffers

		/// <summary>Creates a framebuffer object for offscreen (render to texture) rendering.</summary>
		public async Task<WebGLFramebuffer> CreateFramebufferAsync()
			=> new WebGLFramebuffer((await CallHandleResultAsync("createFramebuffer"))!.Value);

		/// <summary>Binds a framebuffer as render target. Null binds the canvas (default framebuffer) again.</summary>
		public Task BindFramebufferAsync(uint target, WebGLFramebuffer? framebuffer) => CallVoidAsync("bindFramebuffer", target, framebuffer);

		/// <summary>Attaches a texture as color/depth/stencil attachment of the bound framebuffer.</summary>
		public Task FramebufferTexture2DAsync(uint target, uint attachment, uint textureTarget, WebGLTexture texture, int level = 0)
			=> CallVoidAsync("framebufferTexture2D", target, attachment, textureTarget, texture, level);

		/// <summary>Attaches a renderbuffer as attachment of the bound framebuffer.</summary>
		public Task FramebufferRenderbufferAsync(uint target, uint attachment, WebGLRenderbuffer renderbuffer)
			=> CallVoidAsync("framebufferRenderbuffer", target, attachment, GL.RENDERBUFFER, renderbuffer);

		/// <summary>Returns the completeness status of the bound framebuffer, <c>GL.FRAMEBUFFER_COMPLETE</c> when usable.</summary>
		public Task<uint> CheckFramebufferStatusAsync(uint target) => CallAsync<uint>("checkFramebufferStatus", target);

		/// <summary>Deletes a framebuffer object and releases its .NET handle.</summary>
		public async Task DeleteFramebufferAsync(WebGLFramebuffer framebuffer)
		{
			await CallVoidAsync("deleteFramebuffer", framebuffer);
			await ReleaseHandleAsync(framebuffer);
		}

		/// <summary>Creates a renderbuffer object (offscreen depth/stencil/color storage).</summary>
		public async Task<WebGLRenderbuffer> CreateRenderbufferAsync()
			=> new WebGLRenderbuffer((await CallHandleResultAsync("createRenderbuffer"))!.Value);

		/// <summary>Binds a renderbuffer to <c>GL.RENDERBUFFER</c>. Null unbinds.</summary>
		public Task BindRenderbufferAsync(WebGLRenderbuffer? renderbuffer) => CallVoidAsync("bindRenderbuffer", GL.RENDERBUFFER, renderbuffer);

		/// <summary>Allocates storage of the bound renderbuffer, e.g. <c>GL.DEPTH_COMPONENT16</c>.</summary>
		public Task RenderbufferStorageAsync(uint internalFormat, int width, int height)
			=> CallVoidAsync("renderbufferStorage", GL.RENDERBUFFER, internalFormat, width, height);

		/// <summary>Deletes a renderbuffer object and releases its .NET handle.</summary>
		public async Task DeleteRenderbufferAsync(WebGLRenderbuffer renderbuffer)
		{
			await CallVoidAsync("deleteRenderbuffer", renderbuffer);
			await ReleaseHandleAsync(renderbuffer);
		}

		#endregion

		#region Vertex array objects

		/// <summary>
		/// Creates a vertex array object which records attribute setup.
		/// WebGL2 native; on WebGL 1.0 automatically backed by the OES_vertex_array_object extension when available.
		/// </summary>
		public async Task<WebGLVertexArrayObject> CreateVertexArrayAsync()
			=> new WebGLVertexArrayObject((await CallHandleResultAsync("createVertexArray"))!.Value);

		/// <summary>Binds a vertex array object. Null unbinds.</summary>
		public Task BindVertexArrayAsync(WebGLVertexArrayObject? vertexArray) => CallVoidAsync("bindVertexArray", vertexArray);

		/// <summary>Deletes a vertex array object and releases its .NET handle.</summary>
		public async Task DeleteVertexArrayAsync(WebGLVertexArrayObject vertexArray)
		{
			await CallVoidAsync("deleteVertexArray", vertexArray);
			await ReleaseHandleAsync(vertexArray);
		}

		#endregion

		#region Draw

		/// <summary>Draws primitives from the bound array buffer, e.g. <c>DrawArraysAsync(GL.TRIANGLES, 0, 3)</c>.</summary>
		public Task DrawArraysAsync(uint mode, int first, int count) => CallVoidAsync("drawArrays", mode, first, count);

		/// <summary>Draws indexed primitives from the bound element array buffer.</summary>
		public Task DrawElementsAsync(uint mode, int count, uint type, int offset) => CallVoidAsync("drawElements", mode, count, type, offset);

		/// <summary>
		/// Sets how often a vertex attribute advances per instance: 0 per vertex (default), 1 per instance.
		/// WebGL2 native; on WebGL 1.0 automatically backed by the ANGLE_instanced_arrays extension when available.
		/// </summary>
		public Task VertexAttribDivisorAsync(int index, int divisor) => CallVoidAsync("vertexAttribDivisor", index, divisor);

		/// <summary>Draws many instances of the same geometry in one call (see <see cref="VertexAttribDivisorAsync"/>).</summary>
		public Task DrawArraysInstancedAsync(uint mode, int first, int count, int instanceCount)
			=> CallVoidAsync("drawArraysInstanced", mode, first, count, instanceCount);

		/// <summary>Draws many instances of the same indexed geometry in one call.</summary>
		public Task DrawElementsInstancedAsync(uint mode, int count, uint type, int offset, int instanceCount)
			=> CallVoidAsync("drawElementsInstanced", mode, count, type, offset, instanceCount);

		#endregion
	}
}
