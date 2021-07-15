using Microsoft.Extensions.Logging;

namespace Majorsoft.Blazor.Components.Inputs
{
	public partial class MaxLengthTextarea : MaxLengthInputsBase
	{
		protected override ILogger BaseLogger
		{
			get { return _logger; }
		}
	}
}