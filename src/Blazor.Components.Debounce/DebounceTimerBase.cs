using System;
using System.Collections.Generic;
using System.Timers;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;

namespace Blazor.Components.Debounce
{
	public abstract class DebounceTimerBase : ComponentBase, IDisposable
	{
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
				if (value == _intervalInMilisec)
				{
					return;
				}

				if (_timer is not null)
				{
					_timer.Stop();
				}

				_intervalInMilisec = value;
				if (value <= -1)
				{
					_debounceEnabled = false;
					return;
				}
				if (value == 0) //Timer cannot handle 0
				{
					_debounceEnabled = true;
					return;
				}

				_debounceEnabled = true;
				if (_timer is not null)
				{
					_timer.Interval = _intervalInMilisec;
					_timer.AutoReset = false;
				}
			}
		}

		[Parameter] public string? Value { get; set; }
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
			//_timer = DebounceTime > 0 ? new Timer(DebounceTime) : new Timer();
			_timer = new Timer(DebounceTime);
			_timer.Elapsed += OnElapsed;
			_timer.AutoReset = false;

			WriteDiag($"Initialized with Value: '{Value}', Timer interval: '{DebounceTime}' ms, MinLength: '{MinLength}', DebounceEnabled: {_debounceEnabled}.");
			base.OnInitialized();
		}

		protected void OnTextChange(ChangeEventArgs e)
		{
			WriteDiag($"OnTextChange event: '{e.Value}', DebounceEnabled: {_debounceEnabled}, timer interval: {_intervalInMilisec}");
			_timer.Stop(); //Stop previous timer

			Value = e.Value?.ToString();
			_notifiedLastChange = false;

			if(_intervalInMilisec == 0) //Notify immediatelly
			{
				Notify();
			}

			if (!_debounceEnabled) //Do not notify
			{
				return;
			}

			_timer.Start(); //Re-start timer
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
			WriteDiag($"Timer triggered after: '{DebounceTime}' ms delay, DebounceEnabled: {_debounceEnabled}, Value: '{Value}'");

			Notify();
		}

		private void Notify()
		{
			if(_notifiedLastChange)
			{
				WriteDiag($"Notify event was already sent NotifiedLastChange: {_notifiedLastChange}");
				return;
			}

			WriteDiag($"Start ValueChanged notification with length check. Value: '{Value}' length is {Value?.Length}, required MinLength: {MinLength} ");
			var invokeValue = Value?.Length >= MinLength
				? Value
				: string.Empty;

			InvokeAsync(async () =>
			{
				WriteDiag($"Invoke ValueChanged event with: '{invokeValue}'");

				_notifiedLastChange = true;
				await OnValueChanged.InvokeAsync(invokeValue);
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