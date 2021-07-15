using Microsoft.Extensions.Logging;

namespace Majorsoft.Blazor.Components.Inputs
{
	public partial class MaxLengthInput : MaxLengthInputsBase
	{
		protected override ILogger BaseLogger
		{
			get { return _logger; }
		}
	}
}