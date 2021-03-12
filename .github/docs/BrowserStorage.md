Blazor Browser Storage Extensions
============
[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-build-check)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=6)
[![Package Version](https://img.shields.io/nuget/v/Majorsoft.Blazor.Extensions.BrowserStorage?label=Latest%20Version)](https://www.nuget.org/packages/Majorsoft.Blazor.Extensions.BrowserStorage/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Majorsoft.Blazor.Extensions.BrowserStorage?label=Downloads)](https://www.nuget.org/packages/Majorsoft.Blazor.Extensions.BrowserStorage/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/majorimi/blazor-components/blob/master/LICENSE)

# About

Blazor extension which enables Browser Local and Session storages and Cookies store access for Blazor applications.
**All components work with WebAssembly and Server hosted models**. 
For code examples [see usage](https://github.com/majorimi/blazor-components/blob/master/src/Majorsoft.Blazor.Components.TestApps.Common/Components/BrowserStorage.razor).

You can try it out by using the [demo app](https://blazorextensions.z6.web.core.windows.net/browserstorage).

# Extension services

- **`ILocalStorageService`**: is an injectable service for accessing Browser's Local Storage API.
- **`ISessionStorageService`**: is an injectable service for accessing Browser's Session Storage API.
- **`ICookieStoreService`**: is an injectable service for accessing Browser's Cookie Store API.

## `ILocalStorageService` service (See [demo app](https://blazorextensions.z6.web.core.windows.net/browserstorage#local))
Browser Local Storage is an injectable ILocalStorageService service for accessing Browser's [Local Storage API](https://developer.mozilla.org/en-US/docs/Web/API/Window/localStorage).

### Functions
- **`CountAsync`: `Task<Cookie> GetAsync(string name)`** <br />
Returns an integer representing the number of data items stored in the Storage object.
- **`ClearAsync`: `Task ClearAsync()`** <br />
When invoked, will empty all keys out of the storage.
- **`GetItemAsync`: `Task<T> GetItemAsync<T>(string key)`** <br />
When passed a key name, will return that key's value.
- **`GetItemAsync`: `Task<T> GetItemAsync<T>(string key)`** <br />
When passed a key name, will return that key's value.
- **`GetItemAsStringAsync`: `Task<string?> GetItemAsStringAsync(string key)`** <br />
When passed a key name, will return that key's value.
- **`GetKeyByIndexAsync`: `Task<string?> GetKeyByIndexAsync(int index)`** <br />
Returns Storage key name by the given index.
- **`GetAllKeysAsync`: `IAsyncEnumerable<string> GetAllKeysAsync()`** <br />
Returns all keys in Storage.
- **`ContainKeyAsync`: `Task<bool> ContainKeyAsync(string key)`** <br />
Checks if given key exists as Storage key.
- **`RemoveItemAsync`: `Task RemoveItemAsync(string key)`** <br />
When passed a key name, will remove that key from the storage.
- **`SetItemAsync`: `Task SetItemAsync<T>(string key, T data)`** <br />
When passed a key name and value, will add that key to the storage, or update that key's value if it already exists.

## `ISessionStorageService` service (See [demo app](https://blazorextensions.z6.web.core.windows.net/browserstorage#session))
Browser Session Storage is an injectable ISessionStorageService service for accessing Browser's [Session Storage API](https://developer.mozilla.org/en-US/docs/Web/API/Window/sessionStorage).

### Functions
- **`CountAsync`: `Task<Cookie> GetAsync(string name)`** <br />
Returns an integer representing the number of data items stored in the Storage object.
- **`ClearAsync`: `Task ClearAsync()`** <br />
When invoked, will empty all keys out of the storage.
- **`GetItemAsync`: `Task<T> GetItemAsync<T>(string key)`** <br />
When passed a key name, will return that key's value.
- **`GetItemAsync`: `Task<T> GetItemAsync<T>(string key)`** <br />
When passed a key name, will return that key's value.
- **`GetItemAsStringAsync`: `Task<string?> GetItemAsStringAsync(string key)`** <br />
When passed a key name, will return that key's value.
- **`GetKeyByIndexAsync`: `Task<string?> GetKeyByIndexAsync(int index)`** <br />
Returns Storage key name by the given index.
- **`GetAllKeysAsync`: `IAsyncEnumerable<string> GetAllKeysAsync()`** <br />
Returns all keys in Storage.
- **`ContainKeyAsync`: `Task<bool> ContainKeyAsync(string key)`** <br />
Checks if given key exists as Storage key.
- **`RemoveItemAsync`: `Task RemoveItemAsync(string key)`** <br />
When passed a key name, will remove that key from the storage.
- **`SetItemAsync`: `Task SetItemAsync<T>(string key, T data)`** <br />
When passed a key name and value, will add that key to the storage, or update that key's value if it already exists.

## `ICookieStoreService` service (See [demo app](https://blazorextensions.z6.web.core.windows.net/browserstorage#cookie))
Browser Cookie Store is an **injectable `ICookieStoreService` service** for accessing Browser's [Cookie Store API](https://developer.mozilla.org/en-US/docs/Web/API/CookieStore).
**Note: This feature is available only in secure contexts (HTTPS), in some or all supporting browsers.**

### Functions
- **`GetAsync`: `Task<Cookie> GetAsync(string name)`** <br />
Gets a single cookie with the given name.
- **`GetAllAsync`: `Task<IEnumerable<Cookie>> GetAllAsync()`** <br />
Gets all matching cookies.
- **`SetAsync`: `Task SetAsync(Cookie cookie)`** <br />
Sets a cookie with the given name and value.
- **`DeleteAsync`: `Task DeleteAsync(string name)`** <br />
Deletes a cookie with the given name.

# Configuration

## Installation

**Majorsoft.Blazor.Extensions.BrowserStorage** is available on [NuGet](https://www.nuget.org/packages/Majorsoft.Blazor.Extensions.BrowserStorage/). 

```sh
dotnet add package Majorsoft.Blazor.Extensions.BrowserStorage
```
Use the `--version` option to specify a [preview version](https://www.nuget.org/packages/Majorsoft.Blazor.Extensions.BrowserStorage/absoluteLatest) to install.

## Usage

Add using statement to your Blazor `<component/page>.razor` file. Or globally reference it into `_Imports.razor` file.
```
@using Majorsoft.Blazor.Extensions.BrowserStorage
```

### `ILocalStorageService` usage
Following code example shows how to use **`ILocalStorageService`** service. 
Use Browser Developer tools (F12) to see results.

```
LocalStorage item Count: @_localStorageCount

@if (_localStorageCount > 0)
{
	<table class="table table-bordered table-striped table-hover">
		<thead>
			<tr>
				<th width="25%">Key</th>
				<th>Value</th>
			</tr>
		</thead>
		@foreach (var item in _localStorageItems)
		{
			<tr>

				<td>@item.Key</td>
				<td>@item.Value</td>
			</tr>
		}
	</table>
}

@inject ILocalStorageService _localStorageService

@code {
	private int _localStorageCount;
	private IList<KeyValuePair<string, string>> _localStorageItems;

	protected override async Task OnInitializedAsync()
	{
		await InsertLocalStorageItems();
		_localStorageCount = await _localStorageService.CountAsync();
		await RefreshLocalStorageItems();
	}

	private async Task InsertLocalStorageItems()
	{
		await _localStorageService.SetItemAsync("Local_Numeric_value", 2.5);
		await _localStorageService.SetItemAsync("Local_String_value", "hello");
		await _localStorageService.SetItemAsync("Local_Data time", DateTime.Now);
		await _localStorageService.SetItemAsync<UserInfo>("Local_Test_object", new UserInfo { Name = "Name", Age = 22 });
		await _localStorageService.SetItemAsync<string>("Local_Null_string", null);
		await _localStorageService.SetItemAsync<UserInfo>("Local_Null_object", null);
	}
	private async Task RefreshLocalStorageItems()
	{
		_localStorageItems = new List<KeyValuePair<string, string>>();
		await foreach (var key in _localStorageService.GetAllKeysAsync())
		{
			if (key is null)
				continue;

			_localStorageItems.Add(new KeyValuePair<string, string>(key, await _localStorageService.GetItemAsStringAsync(key)));
		}

		_localUserInfo = await _localStorageService.GetItemAsync<UserInfo>("Local_customData") ?? new UserInfo();
	}
}
```

### `ISessionStorageService` usage
Following code example shows how to use **`ISessionStorageService`** service. 
Use Browser Developer tools (F12) to see results.

```
SessionStorage item Count: @_sessionStorageCount

@if (_sessionStorageCount > 0)
{
	<table class="table table-bordered table-striped table-hover">
		<thead>
			<tr>
				<th width="25%">Key</th>
				<th>Value</th>
			</tr>
		</thead>
		@foreach (var item in _sessionStorageItems)
		{
			<tr>

				<td>@item.Key</td>
				<td>@item.Value</td>
			</tr>
		}
	</table>
}

@inject ISessionStorageService _sessionStorageService

@code {
	private int _sessionStorageCount;
	private IList<KeyValuePair<string, string>> _sessionStorageItems;

	protected override async Task OnInitializedAsync()
	{
		await InsertSessionStorageItems();
		_sessionStorageCount = await _sessionStorageService.CountAsync();
		await RefreshSessionStorageItems();
	}	

	private async Task InsertSessionStorageItems()
	{
		await _sessionStorageService.SetItemAsync("Session_Numeric_value", 2.5);
		await _sessionStorageService.SetItemAsync("Session_String_value", "hello");
		await _sessionStorageService.SetItemAsync("Session_Data time", DateTime.Now);
		await _sessionStorageService.SetItemAsync<UserInfo>("Session_Test_object", new UserInfo { Name = "Name", Age = 22 });
		await _sessionStorageService.SetItemAsync<string>("Session_Null_string", null);
		await _sessionStorageService.SetItemAsync<UserInfo>("Session_Null_object", null);
	}
	private async Task RefreshSessionStorageItems()
	{
		_sessionStorageItems = new List<KeyValuePair<string, string>>();
		await foreach (var key in _sessionStorageService.GetAllKeysAsync())
		{
			if (key is null)
				continue;

			_sessionStorageItems.Add(new KeyValuePair<string, string>(key, await _sessionStorageService.GetItemAsStringAsync(key)));
		}

		_sessionUserInfo = await _sessionStorageService.GetItemAsync<UserInfo>("Session_customData") ?? new UserInfo();
	}	
}
```

### `ICookieStoreService` usage
Following code example shows how to use **`ICookieStoreService`** service. 
Use Browser Developer tools (F12) to see results.

**Note: This feature is available only in secure contexts (HTTPS), in some or all supporting browsers.**

```
LocalStorage item Count: @_allCookies?.Count()

@if (_allCookies?.Count() > 0)
{
	<table class="table table-bordered table-striped table-hover">
		<thead>
			<tr>
				<th>Name</th>
				<th>Value</th>
				<th>Domain</th>
				<th>Port</th>
				<th>Path</th>
				<th>Expires</th>
				<th>HttpOnly</th>
				<th>Secure</th>
			</tr>
		</thead>
		@foreach (var item in _allCookies)
		{
			<tr>
				<td>@item.Name</td>
				<td>@item.Value</td>
				<td>@item.Domain</td>
				<td>@item.Port</td>
				<td>@item.Path</td>
				<td>@item.ExpiresAt (@item.Expires)</td>
				<td>@item.HttpOnly</td>
				<td>@item.Secure</td>
			</tr>
		}
	</table>

	<button class="btn btn-primary mt-2" @onclick="@(RemoveCustomCookieAndRefresh)">Remove custom Cookie from Store</button>
}

<label><strong>Add custom Cookie to Store</strong></label>
<br />
Value:
<input class="form-control" @bind-value="_cookie.Value" type="text" />

<button class="btn btn-primary mt-2" @onclick="@(async () => await SaveCustomCookieAndRefresh(_cookie != null ? _cookie.Value : ""))">Save custom Cookie to Store</button>


@inject ICookieStoreService _cookieStoreService

@code {
	protected override async Task OnInitializedAsync()
	{
		_cookie = (await _cookieStoreService.GetAsync("CustomCookie")) ?? new Cookie();
		_allCookies = await _cookieStoreService.GetAllAsync();
	}

	//CookieStore
	Cookie _cookie = new Cookie();
	IEnumerable<Cookie> _allCookies;

	private async Task SaveCustomCookieAndRefresh(string value)
	{
		await _cookieStoreService.SetAsync(new Cookie()
		{
			Name = "CustomCookie",
			Value = value,
			//Path = "/",
			//Domain = "localhost",
			//Secure = true,
			//Expires = Cookie.ConvertToUnixDate(DateTime.Now.AddDays(1))
		});

		_cookie = await _cookieStoreService.GetAsync("CustomCookie");
		_allCookies = await _cookieStoreService.GetAllAsync();
	}
	private async Task RemoveCustomCookieAndRefresh()
	{
		await _cookieStoreService.DeleteAsync("CustomCookie");
		_cookie = new Cookie();
		_allCookies = await _cookieStoreService.GetAllAsync();
	}
}
```