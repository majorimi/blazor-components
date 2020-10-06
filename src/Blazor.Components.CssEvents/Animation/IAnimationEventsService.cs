using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Components.CssEvents.Animation
{
	public interface IAnimationEventsService : IAsyncDisposable
	{
	}

	public class AnimationEventsService  : IAnimationEventsService
	{
		public async ValueTask DisposeAsync()
		{

		}
	}
}