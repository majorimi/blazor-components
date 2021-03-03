using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

		public async Task<string> GetItemAsStringAsync(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentException($"Argument {nameof(key)} cannot by NULL or Empty.");
			}

			return await _jSRuntime.InvokeAsync<string>($"{_storageName}.getItem", key);
		}

		public Task<T> GetItemAsync<T>(string key) => throw new NotImplementedException();
		
		public async Task<string> GetKeyAsync(int index) => await _jSRuntime.InvokeAsync<string>($"{_storageName}.key", index);

		public async Task<IEnumerable<string>> GetKeysAsync()
		{
			var length = await GetLengthAsync();

			var tasks = new List<Task<string>>();
			for (int i = 0; i < length; i++)
			{
				tasks.Add(GetKeyAsync(i));
			}

			var keys = await Task.WhenAll(tasks);
			return keys;
		}

		public async Task<int> GetLengthAsync() => await _jSRuntime.InvokeAsync<int>("eval", $"{_storageName}.length");

		public async Task RemoveItemAsync(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentException($"Argument {nameof(key)} cannot by NULL or Empty.");
			}

			await _jSRuntime.InvokeVoidAsync($"{_storageName}.removeItem", key);
		}

		public Task SetItemAsync<T>(string key, T data) => throw new NotImplementedException();
	}

	/// <summary>
	/// Implementation of <see cref="ILocalStorageService"/>
	/// </summary>
	public class LocalStorageService : StorageServiceBase, ILocalStorageService
	{
		public LocalStorageService(IJSRuntime jSRuntime) 
			: base(jSRuntime, StorageTypes.LocalStorage)
		{
		}
	}

	/// <summary>
	/// Implementation of <see cref="ISessionStorageService"/>
	/// </summary>
	public class SessionStorageService : StorageServiceBase, ISessionStorageService
	{
		public SessionStorageService(IJSRuntime jSRuntime) 
			: base(jSRuntime, StorageTypes.SessionStorage)
		{
		}
	}
}