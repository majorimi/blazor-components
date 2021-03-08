using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Extensions.BrowserStorage
{
	/// <summary>
	/// Storage base class which implements all Local and Session storage features just need to be parametrized.
	/// </summary>
	public abstract class StorageServiceBase : IStorageService
	{
		private readonly IJSRuntime _jSRuntime;
		private readonly StorageTypes _storageType;
		private readonly string _storageName;

		public StorageServiceBase(IJSRuntime jSRuntime, StorageTypes storageType)
		{
			_jSRuntime = jSRuntime;
			_storageType = storageType;

			var name = _storageType.ToString();
			_storageName = name.Substring(0,1).ToLower() + name.Substring(1);
		}

		public async Task ClearAsync() => await _jSRuntime.InvokeVoidAsync($"{_storageName}.clear");

		public async Task<bool> ContainKeyAsync(string key) => await _jSRuntime.InvokeAsync<bool>($"{_storageName}.hasOwnProperty", key);

		public async Task<string?> GetItemAsStringAsync(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentException($"'{nameof(key)}' cannot be null or empty.", nameof(key));
			}

			return await _jSRuntime.InvokeAsync<string>($"{_storageName}.getItem", key);
		}

		public async Task<T> GetItemAsync<T>(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentException($"'{nameof(key)}' cannot be null or empty.", nameof(key));
			}

			if (typeof(T) == typeof(string) || typeof(T) == typeof(ValueType))
			{
				return await _jSRuntime.InvokeAsync<T>($"{_storageName}.getItem", key);
			}
			else
			{
				var jsonData = await _jSRuntime.InvokeAsync<string>($"{_storageName}.getItem", key);
				if (!string.IsNullOrWhiteSpace(jsonData))
				{
					var data = JsonSerializer.Deserialize<T>(jsonData);
					return data;
				}

				return default(T);
			}
		}
		
		public async Task<string?> GetKeyByIndexAsync(int index) => await _jSRuntime.InvokeAsync<string>($"{_storageName}.key", index);

		public async IAsyncEnumerable<string> GetAllKeysAsync()
		{
			var length = await CountAsync();
			for (int i = 0; i < length; i++)
			{
				yield return await GetKeyByIndexAsync(i);
			}
		}

		public async Task<int> CountAsync() => await _jSRuntime.InvokeAsync<int>("eval", $"{_storageName}.length");

		public async Task RemoveItemAsync(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentException($"Argument {nameof(key)} cannot by NULL or Empty.");
			}

			await _jSRuntime.InvokeVoidAsync($"{_storageName}.removeItem", key);
		}

		public async Task SetItemAsync<T>(string key, T data)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentException($"'{nameof(key)}' cannot be null or empty.", nameof(key));
			}

			if (data is string || data is ValueType)
			{
				await _jSRuntime.InvokeVoidAsync($"{_storageName}.setItem", key, data);
			}
			else
			{
				var jsonData = JsonSerializer.Serialize(data);
				await _jSRuntime.InvokeVoidAsync($"{_storageName}.setItem", key, jsonData);
			}
		}
	}
}