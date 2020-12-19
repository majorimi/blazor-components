using Microsoft.Extensions.Logging;

namespace Majorsoft.Blazor.Components.Debounce
{
	public sealed partial class DebounceInput : DebounceTimerBase
	{
		protected override ILogger BaseLogger
		{
			get { return _logger; }
		}
	}
}