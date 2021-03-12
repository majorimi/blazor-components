
using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Extensions.BrowserStorage
{
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
}