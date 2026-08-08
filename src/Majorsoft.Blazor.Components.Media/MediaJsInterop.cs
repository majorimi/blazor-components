using System.IO;
using System.Threading.Tasks;

using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Components.Media
{
	/// <summary>
	/// Shared JS interop helpers of the Media components.
	/// </summary>
	internal static class MediaJsInterop
	{
		/// <summary>Downloads binary data kept in the JS module (recordings, photos) as a .NET stream.</summary>
		internal static async Task<Stream?> GetJsDataStreamAsync(IJSObjectReference module, string function, int id, long maxAllowedSize)
		{
			var dataRef = await module.InvokeAsync<IJSStreamReference?>(function, id);
			if (dataRef is null)
			{
				return null;
			}

			//The reference must not be disposed here: on Server hosting the returned stream pulls
			//the data lazily and disposing the reference would abort the transfer.
			return await dataRef.OpenReadStreamAsync(maxAllowedSize);
		}
	}
}
