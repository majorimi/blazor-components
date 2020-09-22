using System;
using System.Collections.Generic;
using System.Timers;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;

namespace Blazor.Components.Debounce.Input
{
	public abstract class DebounceTimerBase : ComponentBase, IDisposable
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

		private Timer _timer;
		protected abstract ILogger BaseLogger { get; }

		protected override void OnInitialized()
		{
			InternalValue = CurrentValue;

			_timer = new Timer(Delay);
			_timer.Elapsed += OnElapsed;
			_timer.AutoReset = false;

			WriteDiag($"Initialized with Value: '{InternalValue}', Timer interval: '{Delay}' ms, Min sting Length: '{MinLength}'.");
		}

		protected void OnTextChange(ChangeEventArgs e)
		{
			WriteDiag($"OnTextChange event: '{e.Value}'");

			_timer.Stop(); //Stop previous timer
			_timer.Start(); //Re-start timer

			InternalValue = e.Value?.ToString();
		}
		protected void OnBlur(FocusEventArgs e)
		{
			WriteDiag($"OnBlur event: '{e.Type}'");

			if (ForceNotifyOnBlur)
			{
				_timer.Stop(); //Stop timer
				OnElapsed(null, null);
			}
		}
		protected void OnKeyPress(KeyboardEventArgs e)
		{
			WriteDiag($"OnKeyPress event: '{e.Key}'");

			if (ForceNotifyByEnter && (e.Key?.Equals("Enter", StringComparison.OrdinalIgnoreCase) ?? false))
			{
				_timer.Stop(); //Stop timer
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

		public void Dispose()
		{
			if (_timer is not null)
			{
				_timer.Elapsed -= OnElapsed;
				_timer.Dispose();
			}
		}

		private void WriteDiag(string message)
		{
			BaseLogger.LogDebug($"Component {this.GetType()}: {message}");
		}
	}
}
