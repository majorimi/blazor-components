using Blazor.Components.Common.JsInterop.Click;

using Microsoft.Extensions.Logging;

namespace Blazor.Components.Typeahead
{
	public sealed partial class TypeaheadInput<TItem> : TypeaheadBase<TItem>
	{
		protected override ILogger BaseLogger => _logger;

		protected override IClickBoundariesHandler ClickBoundariesHandler => _clickHandler;
	}
}