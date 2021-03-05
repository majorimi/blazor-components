using System.Collections.Generic;
using System.Threading.Tasks;

namespace Majorsoft.Blazor.Extensions.BrowserStorage
{
	/// <summary>
	/// The Storage interface of the Web Storage API provides access to a particular domain's session or local storage. 
	/// It allows, for example, the addition, modification, or deletion of stored data items.
	/// https://developer.mozilla.org/en-US/docs/Web/API/Storage
	/// </summary>
	public interface IStorageService
	{
		/// <summary>
		/// Returns an integer representing the number of data items stored in the Storage object.
		/// </summary>
		/// <returns>Number of objects</returns>
		Task<int> GetLengthAsync();

		/// <summary>
		/// When invoked, will empty all keys out of the storage.
		/// </summary>
		/// <returns>Async Task</returns>
		Task ClearAsync();

		/// <summary>
		/// When passed a key name, will return that key's value.
		/// </summary>
		/// <typeparam name="T">Type param</typeparam>
		/// <param name="key">Storage key</param>
		/// <returns>Stored T instance object by key</returns>
		Task<T> GetItemAsync<T>(string key);

		/// <summary>
		/// When passed a key name, will return that key's value.
		/// </summary>
		/// <param name="key">Storage key</param>
		/// <returns>Sored object as String</returns>
		Task<string?> GetItemAsStringAsync(string key);

		/// <summary>
		/// Returns Storage key name by the given index.
		/// </summary>
		/// <param name="index">Index of the key</param>
		/// <returns>Storage key name by index</returns>
		Task<string?> GetKeyByIndexAsync(int index);

		/// <summary>
		/// Returns all keys in Storage.
		/// </summary>
		/// <returns>List of keys</returns>
		Task<IEnumerable<string>> GetAllKeysAsync();

		/// <summary>
		/// Checks if given key exists as Storage key.
		/// </summary>
		/// <param name="key">Storage key</param>
		/// <returns>True if key exists, otherwise False</returns>
		Task<bool> ContainKeyAsync(string key);

		/// <summary>
		/// When passed a key name, will remove that key from the storage.
		/// </summary>
		/// <param name="key">Storage key</param>
		/// <returns>Async Task</returns>
		Task RemoveItemAsync(string key);

		/// <summary>
		/// When passed a key name and value, will add that key to the storage, or update that key's value if it already exists.
		/// </summary>
		/// <typeparam name="T">Type param</typeparam>
		/// <param name="key">Storage key</param>
		/// <param name="data">Object to store</param>
		/// <returns>Async Task</returns>
		Task SetItemAsync<T>(string key, T data);
	}
}