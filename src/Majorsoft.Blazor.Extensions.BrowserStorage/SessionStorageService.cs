
using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Extensions.BrowserStorage
{
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