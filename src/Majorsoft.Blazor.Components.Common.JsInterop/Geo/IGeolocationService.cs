using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majorsoft.Blazor.Components.Common.JsInterop.Geo
{
	public interface IGeolocationService : IAsyncDisposable
	{
	}

	public sealed class GeolocationService : IGeolocationService
	{
		public ValueTask DisposeAsync() => throw new NotImplementedException();
	}
}
