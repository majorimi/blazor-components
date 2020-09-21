using System;
using System.Collections.Generic;
using System.Timers;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;

namespace Blazor.Components.Debounce.Input
{
	public abstract class DebounceTimerBase : ComponentBase
	{
		protected string InternalValue;

		[Parameter] public string CurrentValue { get; set; }
		[Parameter] public int MinLength { get; set; } = 0;
		[Parameter] public int Delay { get; set; } = 200;
		[Parameter] public bool ForceNotifyByEnter { get; set; } = true;
		[Parameter] public bool ForceNotifyOnBlur { get; set; } = true;

		[Parameter(CaptureUnmatchedValues = true)]
		public Dictionary<string, object> AdditionalAttributes { get; set; }

		[Parameter] public EventCallback<string> OnValueChanged { get; set; }

		private Timer _timre;
		protected abstract ILogger BaseLogger { get; }

		protected override void OnInitialized()
		{
			InternalValue = CurrentValue;

			_timre = new Timer(Delay);
			_timre.Elapsed += OnElapsed;
			_timre.AutoReset = false;

			WriteDiag($"Initialized with Value: '{InternalValue}', Timer interval: '{Delay}' ms, Min sting Length: '{MinLength}'.");
		}

		protected void OnTextChange(ChangeEventArgs e)
		{
			WriteDiag($"OnTextChange event: '{e.Value}'");

			_timre.Stop(); //Stop previous timer
			_timre.Start(); //Re-start timer

			InternalValue = e.Value?.ToString();
		}
		protected void OnBlur(FocusEventArgs e)
		{
			WriteDiag($"OnBlur event: '{e.Type}'");

			if (ForceNotifyOnBlur)
			{
				_timre.Stop(); //Stop timer
				OnElapsed(null, null);
			}
		}
		protected void OnKeyPress(KeyboardEventArgs e)
		{
			WriteDiag($"OnKeyPress event: '{e.Key}'");

			if (ForceNotifyByEnter && (e.Key?.Equals("Enter", StringComparison.OrdinalIgnoreCase) ?? false))
			{
				_timre.Stop(); //Stop timer
				OnElapsed(null, null);
			}
		}

		protected void OnElapsed(object source, ElapsedEventArgs e)
		{
			WriteDiag($"Timer triggered after: '{Delay}' ms delay, Value: '{InternalValue}'");

			var invokeValue = InternalValue?.Length >= MinLength
				? InternalValue
				: string.Empty;

			InvokeAsync(async () =>
			{
				WriteDiag($"Invoke ValueChanged event with: '{invokeValue}'");

				CurrentValue = InternalValue;
				await OnValueChanged.InvokeAsync(invokeValue);
				StateHasChanged();
			});
		}

		private void WriteDiag(string message)
		{
			BaseLogger.LogDebug($"Component {this.GetType()}: {message}");
		}
	}
}