using System.Text.Json.Serialization;

namespace Majorsoft.Blazor.Components.Canvas
{
	/// <summary>
	/// Base class of all .NET references to JS side objects (gradients, patterns, WebGL buffers, shaders, etc.).
	/// The object itself lives in a handle map inside the JS module; only the numeric handle travels through
	/// JS interop serialized as <c>{ "__h": n }</c>.
	/// </summary>
	public abstract class JsObjectRef
	{
		/// <summary>Numeric handle identifying the JS object inside the owning context.</summary>
		[JsonPropertyName("__h")]
		public int Handle { get; }

		internal JsObjectRef(int handle)
		{
			Handle = handle;
		}
	}

	/// <summary>Deserialization target for JS results that are object handles.</summary>
	internal sealed class JsHandleDto
	{
		[JsonPropertyName("__h")]
		public int Handle { get; set; }
	}

	/// <summary>Marker wrapper serialized as <c>{ "__f32": [...] }</c> and revived to a JS Float32Array.</summary>
	internal sealed class Float32ArrayArg
	{
		[JsonPropertyName("__f32")]
		public float[] Data { get; set; } = System.Array.Empty<float>();
	}

	/// <summary>Marker wrapper serialized as <c>{ "__i32": [...] }</c> and revived to a JS Int32Array.</summary>
	internal sealed class Int32ArrayArg
	{
		[JsonPropertyName("__i32")]
		public int[] Data { get; set; } = System.Array.Empty<int>();
	}

	/// <summary>Marker wrapper serialized as <c>{ "__u16": [...] }</c> and revived to a JS Uint16Array.</summary>
	internal sealed class Uint16ArrayArg
	{
		[JsonPropertyName("__u16")]
		public int[] Data { get; set; } = System.Array.Empty<int>();
	}

	/// <summary>Marker wrapper serialized as <c>{ "__u8": [...] }</c> and revived to a JS Uint8Array.</summary>
	internal sealed class Uint8ArrayArg
	{
		[JsonPropertyName("__u8")]
		public int[] Data { get; set; } = System.Array.Empty<int>();
	}
}
