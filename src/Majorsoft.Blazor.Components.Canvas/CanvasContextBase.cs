using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Components.Canvas
{
	/// <summary>
	/// Base class of <see cref="Canvas2DContext"/> and WebGL contexts. Dispatches method calls and property
	/// writes to the JS side rendering context and supports batching: between <see cref="BeginBatch"/> and
	/// <see cref="EndBatchAsync"/> all void operations are buffered and sent in a single JS interop call.
	/// </summary>
	public abstract class CanvasContextBase : IAsyncDisposable
	{
		private readonly IJSObjectReference _module;
		private readonly int _id;
		private List<BatchOp>? _batch;
		private bool _disposed;

		internal IJSObjectReference Module => _module;
		internal int Id => _id;

		internal CanvasContextBase(IJSObjectReference module, int id)
		{
			_module = module;
			_id = id;
		}

		/// <summary>True when operations are currently buffered instead of executed immediately.</summary>
		public bool IsBatching => _batch is not null;

		/// <summary>
		/// Starts buffering void operations (draw calls, property writes). Call <see cref="EndBatchAsync"/>
		/// to execute all buffered operations in one JS interop round trip. Operations that return a value
		/// flush the pending batch first to keep ordering.
		/// </summary>
		public void BeginBatch() => _batch ??= new List<BatchOp>();

		/// <summary>Executes all buffered operations in one JS interop call and ends batching.</summary>
		public async Task EndBatchAsync()
		{
			var ops = _batch;
			_batch = null;

			if (ops is not null && ops.Count > 0)
			{
				await _module.InvokeVoidAsync("batch", _id, ops);
			}
		}

		private protected async Task CallVoidAsync(string method, params object?[] args)
		{
			if (_batch is not null)
			{
				_batch.Add(BatchOp.Call(method, args));
				return;
			}
			await _module.InvokeVoidAsync("call", _id, method, args);
		}

		private protected async Task<T> CallAsync<T>(string method, params object?[] args)
		{
			await FlushBatchAsync();
			return await _module.InvokeAsync<T>("call", _id, method, args);
		}

		private protected async Task<int?> CallHandleResultAsync(string method, params object?[] args)
		{
			await FlushBatchAsync();
			var dto = await _module.InvokeAsync<JsHandleDto?>("call", _id, method, args);
			return dto?.Handle;
		}

		private protected async Task SetAsync(string property, object? value)
		{
			if (_batch is not null)
			{
				_batch.Add(BatchOp.Set(property, value));
				return;
			}
			await _module.InvokeVoidAsync("setProperty", _id, property, value);
		}

		private protected async Task<T> GetAsync<T>(string property)
		{
			await FlushBatchAsync();
			return await _module.InvokeAsync<T>("getProperty", _id, property);
		}

		private protected async Task<T> InvokeHelperAsync<T>(string function, params object?[] args)
		{
			await FlushBatchAsync();
			return await InvokeArgsAsync<T>(function, args);
		}

		private protected async Task InvokeHelperVoidAsync(string function, params object?[] args)
		{
			await FlushBatchAsync();
			var callArgs = new object?[args.Length + 1];
			callArgs[0] = _id;
			Array.Copy(args, 0, callArgs, 1, args.Length);
			await _module.InvokeVoidAsync(function, callArgs);
		}

		private async Task<T> InvokeArgsAsync<T>(string function, object?[] args)
		{
			var callArgs = new object?[args.Length + 1];
			callArgs[0] = _id;
			Array.Copy(args, 0, callArgs, 1, args.Length);
			return await _module.InvokeAsync<T>(function, callArgs);
		}

		internal async Task<T> CallOnHandleAsync<T>(JsObjectRef target, string method, params object?[] args)
		{
			await FlushBatchAsync();
			return await _module.InvokeAsync<T>("callHandle", _id, target.Handle, method, args);
		}

		internal async Task CallOnHandleVoidAsync(JsObjectRef target, string method, params object?[] args)
		{
			await FlushBatchAsync();
			await _module.InvokeVoidAsync("callHandle", _id, target.Handle, method, args);
		}

		/// <summary>Releases a JS object handle (gradient, buffer, texture, etc.) from the JS side handle map.</summary>
		public async Task ReleaseHandleAsync(JsObjectRef objectRef)
			=> await _module.InvokeVoidAsync("releaseHandle", _id, objectRef.Handle);

		/// <summary>
		/// Returns the canvas content as a data URL, e.g. <c>data:image/png;base64,...</c>.
		/// </summary>
		/// <param name="type">Image MIME type: "image/png" (default), "image/jpeg" or "image/webp".</param>
		/// <param name="quality">JPEG/WebP quality between 0 and 1, null for browser default.</param>
		public async Task<string> ToDataURLAsync(string type = "image/png", double? quality = null)
			=> await InvokeHelperAsync<string>("toDataURL", type, quality);

		private Task FlushBatchAsync() => _batch is not null ? EndBatchAsync() : Task.CompletedTask;

		/// <summary>Releases the JS side context, handles and render loop.</summary>
		public virtual async ValueTask DisposeAsync()
		{
			if (_disposed)
			{
				return;
			}
			_disposed = true;

			try
			{
				await _module.InvokeVoidAsync("dispose", _id);
			}
			catch (JSDisconnectedException)
			{
				//Circuit or page already gone, nothing to release.
			}
		}

		internal sealed class BatchOp
		{
			[JsonPropertyName("t")]
			public string Type { get; set; } = "c";

			[JsonPropertyName("m")]
			public string? Method { get; set; }

			[JsonPropertyName("a")]
			public object?[]? Args { get; set; }

			[JsonPropertyName("p")]
			public string? Property { get; set; }

			[JsonPropertyName("v")]
			public object? Value { get; set; }

			public static BatchOp Call(string method, object?[] args)
				=> new BatchOp { Type = "c", Method = method, Args = args };

			public static BatchOp Set(string property, object? value)
				=> new BatchOp { Type = "s", Property = property, Value = value };
		}
	}
}
