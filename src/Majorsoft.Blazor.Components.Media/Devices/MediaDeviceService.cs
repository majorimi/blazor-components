using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Components.Media
{
	/// <summary>
	/// Implementation of <see cref="IMediaDeviceService"/>
	/// </summary>
	public class MediaDeviceService : IMediaDeviceService
	{
		private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

		/// <summary>Default constructor.</summary>
		/// <param name="jsRuntime">IJSRuntime instance</param>
		public MediaDeviceService(IJSRuntime jsRuntime)
		{
			string js = "";
#if DEBUG
			js = "./_content/Majorsoft.Blazor.Components.Media/media.js";
#else
			js = "./_content/Majorsoft.Blazor.Components.Media/media.min.js";
#endif

			_moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>("import", js).AsTask());
		}

		/// <inheritdoc/>
		public async ValueTask<bool> IsCaptureSupportedAsync()
		{
			var module = await _moduleTask.Value;
			return await module.InvokeAsync<bool>("isCaptureSupported");
		}

		/// <inheritdoc/>
		public async ValueTask<bool> IsMediaRecorderSupportedAsync()
		{
			var module = await _moduleTask.Value;
			return await module.InvokeAsync<bool>("isMediaRecorderSupported");
		}

		/// <inheritdoc/>
		public async ValueTask<IEnumerable<MediaDeviceInfo>> GetMediaDevicesAsync()
		{
			var module = await _moduleTask.Value;
			return await module.InvokeAsync<MediaDeviceInfo[]>("getDevices");
		}

		/// <inheritdoc/>
		public async ValueTask<string?> RequestPermissionsAsync(bool audio = true, bool video = true)
		{
			var module = await _moduleTask.Value;
			return await module.InvokeAsync<string?>("requestPermissions", audio, video);
		}

		/// <inheritdoc/>
		public async ValueTask<IEnumerable<string>> GetSupportedMimeTypesAsync(bool video)
		{
			var module = await _moduleTask.Value;
			return await module.InvokeAsync<string[]>("getSupportedMimeTypes", video);
		}

		/// <inheritdoc/>
		public async ValueTask DisposeAsync()
		{
			if (_moduleTask.IsValueCreated)
			{
				try
				{
					var module = await _moduleTask.Value;
					await module.DisposeAsync();
				}
				catch (JSDisconnectedException)
				{
					//Circuit or page already gone, nothing to release.
				}
			}
		}
	}
}
