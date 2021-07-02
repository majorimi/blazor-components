using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace Majorsoft.Blazor.Components.Inputs
{
	/// <summary>
	/// Base class for MaxLength components.
	/// </summary>
	public abstract class MaxLengthInputsBase : ComponentBase
	{
		protected abstract ILogger BaseLogger { get; }

		protected override async Task OnParametersSetAsync()
		{
			await CalculateRemaining();
		}

		protected int _remainingChars;
		protected ElementReference _inputRef;
		/// <summary>
		/// Exposes a Blazor <see cref="ElementReference"/> of the wrapped around HTML element. It can be used e.g. for JS interop, etc.
		/// </summary>
		public ElementReference InnerElementReference => _inputRef;

		/// <summary>
		/// Value of the rendered HTML element. Initial field value can be set to given string or omitted (leave empty).
		/// Also control actual value can be read out (useful when MinLenght not reached).
		/// </summary>
		[Parameter] public string? Value { get; set; }

		/// <summary>
		/// Maximum allowed characters to type in.
		/// </summary>
		[Parameter] public int MaxAllowedChars { get; set; } = 50;

		/// <summary>
		/// Contdown label text to change or localize message.
		/// </summary>
		[Parameter] public string CountdownText { get; set; } = "Remaining characters: ";

		/// <summary>
		/// Contdown label and value CSS calss property to style message.
		/// </summary>
		[Parameter] public string CountdownTextClass { get; set; }

		//Events
		/// <summary>
		/// Callback function called when HTML control received keyboard inputs.
		/// </summary>
		[Parameter] public EventCallback<string> OnInput { get; set; }

		/// <summary>
		/// Callback function called when HTML control received keyboard inputs remaining allowed chars calculated and sent as even args.
		/// </summary>
		[Parameter] public EventCallback<int> OnRemainingCharsChanged { get; set; }


		/// <summary>
		/// Blazor capture for any unmatched HTML attributes.
		/// </summary>
		[Parameter(CaptureUnmatchedValues = true)]
		public Dictionary<string, object> AllOtherAttributes { get; set; }

		protected async Task OnTextChange(ChangeEventArgs e)
		{
			WriteDiag($"{nameof(OnTextChange)} event: '{e.Value}', MaxAllowedChars: '{MaxAllowedChars}'.");

			if (OnInput.HasDelegate) //Immediately notify listeners of text change e.g. @bind
			{
				await OnInput.InvokeAsync(e.Value?.ToString());
			}

			Value = e.Value?.ToString();
			await CalculateRemaining();
		}

		private async Task CalculateRemaining()
		{
			var tmp = _remainingChars;
			_remainingChars = MaxAllowedChars - (Value?.Length ?? 0);

			WriteDiag($"{nameof(CalculateRemaining)} event value Length: '{Value?.Length}', _remainingChars: '{_remainingChars}'.");

			if (OnRemainingCharsChanged.HasDelegate && tmp != _remainingChars)
			{
				await OnRemainingCharsChanged.InvokeAsync(_remainingChars);
			}
		}

		private void WriteDiag(string message)
		{
			BaseLogger.LogDebug($"Component {this.GetType()}: {message}");
		}
	}
}