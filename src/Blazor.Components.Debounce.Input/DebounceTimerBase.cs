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

		private bool _notifiedLastChange = false;
		private bool _disposedValue;
		private bool _debounceEnabled = true;

		private double _intervalInMilisec = 200;
		[Parameter]
		public double DebounceTime
		{
			get => _intervalInMilisec;
			set
			{
				if (value <= 0)
				{
					_debounceEnabled = false;
					_timer.Stop();
					return;
				}

				if (value == _intervalInMilisec)
				{
					return;
				}

				_intervalInMilisec = value;
				_debounceEnabled = true;

				if (_timer is not null)
				{
					_timer.Interval = _intervalInMilisec;
					_timer.AutoReset = false;
				}
			}
		}

		[Parameter] public string CurrentValue { get; set; }
		[Parameter] public int MinLength { get; set; } = 0;
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
			_timer = new Timer(DebounceTime);
			_timer.Elapsed += OnElapsed;
			_timer.AutoReset = false;

			WriteDiag($"Initialized with Value: '{InternalValue}', Timer interval: '{DebounceTime}' ms, MinLength: '{MinLength}', DebounceEnabled: {_debounceEnabled}.");
			base.OnInitialized();
		}

		protected void OnTextChange(ChangeEventArgs e)
		{
			WriteDiag($"OnTextChange event: '{e.Value}', DebounceEnabled: {_debounceEnabled}");

			InternalValue = e.Value?.ToString();

			if (!_debounceEnabled) //Do not notify
			{
				_timer.Stop();
				return;
			}

			_timer.Stop(); //Stop previous timer
			_timer.Start(); //Re-start timer
			_notifiedLastChange = false;
		}
		protected void OnBlur(FocusEventArgs e)
		{
			WriteDiag($"OnBlur event: '{e.Type}', ForceNotifyOnBlur: {ForceNotifyOnBlur}, DebounceEnabled: {_debounceEnabled}");

			if (ForceNotifyOnBlur)
			{
				_timer.Stop(); //Stop timer
				Notify();
			}
		}
		protected void OnKeyPress(KeyboardEventArgs e)
		{
			WriteDiag($"OnKeyPress event: '{e.Key}', ForceNotifyByEnter: {ForceNotifyByEnter}, DebounceEnabled: {_debounceEnabled}");

			if (ForceNotifyByEnter && (e.Key?.Equals("Enter", StringComparison.OrdinalIgnoreCase) ?? false))
			{
				_timer.Stop(); //Stop timer
				Notify();
			}
		}

		protected void OnElapsed(object source, ElapsedEventArgs e)
		{
			WriteDiag($"Timer triggered after: '{DebounceTime}' ms delay, DebounceEnabled: {_debounceEnabled}, Value: '{InternalValue}'");

			Notify();
		}

		private void Notify()
		{
			if(_notifiedLastChange)
			{
				WriteDiag($"Notify event was already sent NotifiedLastChange: {_notifiedLastChange}");
				return;
			}

			WriteDiag($"Start ValueChanged notification with length check. CurrentValue: '{InternalValue}' length is {InternalValue?.Length}, required MinLength: {MinLength} ");
			var invokeValue = InternalValue?.Length >= MinLength
				? InternalValue
				: string.Empty;

			InvokeAsync(async () =>
			{
				WriteDiag($"Invoke ValueChanged event with: '{invokeValue}'");

				_notifiedLastChange = true;
				CurrentValue = InternalValue;
				await OnValueChanged.InvokeAsync(invokeValue);

				StateHasChanged();
			});
		}

		private void WriteDiag(string message)
		{
			BaseLogger.LogDebug($"Component {this.GetType()}: {message}");
		}


		protected virtual void Dispose(bool disposing)
		{
			if (!_disposedValue)
			{
				if (disposing)
				{
					_timer.Elapsed -= OnElapsed;
					_timer.Dispose();
				}

				_disposedValue = true;
			}
		}

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
