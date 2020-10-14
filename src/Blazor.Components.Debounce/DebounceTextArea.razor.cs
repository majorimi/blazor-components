using Microsoft.Extensions.Logging;

namespace Blazor.Components.Debounce
{
	public sealed partial class DebounceTextArea : DebounceTimerBase
	{
		protected override ILogger BaseLogger
		{
			get { return _logger; }
		}
	}
}