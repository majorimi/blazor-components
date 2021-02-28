using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Extensions.BrowserStorage
{
	public interface IStorageService
	{
		Task<int> GetLengthAsync();
	}

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

		public async Task<int> GetLengthAsync() => await _jSRuntime.InvokeAsync<int>("eval", $"{_storageName}.length");
	}

	public interface ILocalStorageService : IStorageService
	{ }

	public class LocalStorageService : StorageServiceBase, ILocalStorageService
	{
		public LocalStorageService(IJSRuntime jSRuntime) 
			: base(jSRuntime, StorageTypes.LocalStorage)
		{
		}
	}

	public interface ISessionStorageService : IStorageService
	{ }

	public class SessionStorageService : StorageServiceBase, ISessionStorageService
	{
		public SessionStorageService(IJSRuntime jSRuntime) 
			: base(jSRuntime, StorageTypes.SessionStorage)
		{
		}
	}
}