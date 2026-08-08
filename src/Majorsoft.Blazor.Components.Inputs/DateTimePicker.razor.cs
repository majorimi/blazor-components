using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

using Majorsoft.Blazor.Components.Tooltips;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;

namespace Majorsoft.Blazor.Components.Inputs
{
	/// <summary>
	/// Date or DateTime picker component: a text input with a calendar (and optional time) editor shown in a
	/// <see cref="Popover"/>. The Popover position, first day of week, date/time format string and culture are
	/// all configurable, and the component can work in Date only or DateTime mode (see <see cref="Mode"/>).
	/// <para>
	/// Formatting and parsing use <see cref="DateFormat"/> when set, otherwise the short date (and time) pattern
	/// of <see cref="Culture"/> (or the current culture). In DateTime mode a 12/24 hour clock and the visibility
	/// of the seconds editor are derived from the effective format string.
	/// </para>
	/// </summary>
	public partial class DateTimePicker : ComponentBase
	{
		[Inject] private ILogger<DateTimePicker> Logger { get; set; } = default!;

		private enum PickerView { Days, Months, Years }

		private ElementReference _inputRef;
		private bool _isOpen;
		//Bumped to re-create the <input> when typed text must be discarded: after an invalid or non-canonical
		//entry the rendered 'value' attribute may be unchanged, so Blazor would leave the stale text in the DOM.
		private int _inputKey;
		private PickerView _view = PickerView.Days;
		private DateTime _viewDate = DateTime.Today; //Month/year currently displayed in the calendar.

		/// <summary>
		/// Exposes the Blazor <see cref="ElementReference"/> of the inner &lt;input&gt;. Useful for JS interop, focus, etc.
		/// </summary>
		public ElementReference InnerElementReference => _inputRef;

		#region Value

		/// <summary>
		/// The picked date (and time). Supports two-way binding via <c>@bind-Value</c>.
		/// In <see cref="DateTimePickerMode.Date"/> mode the time part is always midnight.
		/// </summary>
		[Parameter] public DateTime? Value { get; set; }

		/// <summary>
		/// Callback invoked whenever <see cref="Value"/> changes. Enables <c>@bind-Value</c>.
		/// </summary>
		[Parameter] public EventCallback<DateTime?> ValueChanged { get; set; }

		#endregion

		#region Behavior

		/// <summary>
		/// Whether the component picks a Date only or a Date with Time. Default is <see cref="DateTimePickerMode.Date"/>.
		/// </summary>
		[Parameter] public DateTimePickerMode Mode { get; set; } = DateTimePickerMode.Date;

		/// <summary>
		/// Position of the picker Popover relative to the input. Default is <see cref="TooltipPositions.Bottom"/>.
		/// </summary>
		[Parameter] public TooltipPositions Position { get; set; } = TooltipPositions.Bottom;

		/// <summary>
		/// First day of week shown in the calendar. When null (default) the first day of week of the effective culture is used.
		/// </summary>
		[Parameter] public DayOfWeek? FirstDayOfWeek { get; set; }

		/// <summary>
		/// Custom .NET date/time format string used for formatting and parsing the input text, e.g. "yyyy-MM-dd" or "dd/MM/yyyy HH:mm".
		/// When not set the short date (and short time in DateTime mode) pattern of the effective culture is used.
		/// </summary>
		[Parameter] public string? DateFormat { get; set; }

		/// <summary>
		/// Culture used for formatting, parsing, day/month names and calendar rules.
		/// When null (default) <see cref="CultureInfo.CurrentCulture"/> is used.
		/// </summary>
		[Parameter] public CultureInfo? Culture { get; set; }

		/// <summary>
		/// Minimum selectable date (inclusive). Earlier days are disabled in the calendar and typed values are clamped.
		/// </summary>
		[Parameter] public DateTime? MinDate { get; set; }

		/// <summary>
		/// Maximum selectable date (inclusive). Later days are disabled in the calendar and typed values are clamped.
		/// </summary>
		[Parameter] public DateTime? MaxDate { get; set; }

		/// <summary>
		/// Step of the minutes dropdown in DateTime mode, e.g. 5, 15, 30. Default is 1 (every minute).
		/// The current value's minute is always offered even when it is off-step.
		/// </summary>
		[Parameter] public int MinuteStep { get; set; } = 1;

		/// <summary>
		/// When true (default) the value can also be typed into the input and is parsed with the effective
		/// format/culture on change. When false the input is read only and the value can only be picked.
		/// </summary>
		[Parameter] public bool AllowTextInput { get; set; } = true;

		/// <summary>
		/// When true (default) the picker Popover closes when the user clicks outside of it.
		/// </summary>
		[Parameter] public bool CloseOnOutsideClick { get; set; } = true;

		/// <summary>When true the whole component is disabled. Default is false.</summary>
		[Parameter] public bool Disabled { get; set; }

		/// <summary>When true the value cannot be changed (typing and picking are both blocked). Default is false.</summary>
		[Parameter] public bool ReadOnly { get; set; }

		/// <summary>Placeholder text shown when the input is empty.</summary>
		[Parameter] public string? Placeholder { get; set; }

		/// <summary>Whether the week number column is rendered in the calendar. Default is false.</summary>
		[Parameter] public bool ShowWeekNumbers { get; set; }

		/// <summary>Whether the Today (Date mode) / Now (DateTime mode) footer button is rendered. Default is true.</summary>
		[Parameter] public bool ShowTodayButton { get; set; } = true;

		/// <summary>Whether the Clear footer button is rendered. Default is true.</summary>
		[Parameter] public bool ShowClearButton { get; set; } = true;

		#endregion

		#region Texts (localization)

		/// <summary>Text of the Today footer button (Date mode). Default is "Today".</summary>
		[Parameter] public string TodayButtonText { get; set; } = "Today";

		/// <summary>Text of the Now footer button (DateTime mode). Default is "Now".</summary>
		[Parameter] public string NowButtonText { get; set; } = "Now";

		/// <summary>Text of the Clear footer button. Default is "Clear".</summary>
		[Parameter] public string ClearButtonText { get; set; } = "Clear";

		/// <summary>Text of the OK footer button which closes the picker (DateTime mode). Default is "OK".</summary>
		[Parameter] public string OkButtonText { get; set; } = "OK";

		#endregion

		#region Styling

		/// <summary>Custom CSS class(es) applied to the root (trigger container) element.</summary>
		[Parameter] public string? Class { get; set; }

		/// <summary>Custom inline style applied to the root (trigger container) element.</summary>
		[Parameter] public string? Style { get; set; }

		/// <summary>Custom CSS class(es) applied to the inner &lt;input&gt; element.</summary>
		[Parameter] public string? InputClass { get; set; }

		/// <summary>Custom CSS class(es) applied to the Popover panel element of the picker.</summary>
		[Parameter] public string? PickerClass { get; set; }

		/// <summary>Optional custom content for the calendar toggle button (replaces the default calendar icon).</summary>
		[Parameter] public RenderFragment? CalendarIconContent { get; set; }

		/// <summary>Arbitrary HTML attributes applied to the inner &lt;input&gt; element.</summary>
		[Parameter(CaptureUnmatchedValues = true)]
		public Dictionary<string, object>? AdditionalAttributes { get; set; }

		#endregion

		#region Events

		/// <summary>
		/// Gets or sets whether the picker Popover is open. Supports two-way binding with <c>@bind-IsOpen</c>.
		/// Set to true to open the picker programmatically, false to close it. Reading it returns the current open/closed state.
		/// </summary>
		[Parameter]
		public bool IsOpen
		{
			get => _isOpen;
			set => _isOpen = value;
		}

		/// <summary>
		/// Callback for two-way binding. Invoked with the new open/closed state whenever the picker opens or closes.
		/// </summary>
		[Parameter] public EventCallback<bool> IsOpenChanged { get; set; }

		/// <summary>Callback function called when the picker Popover is opening.</summary>
		[Parameter] public EventCallback OnOpen { get; set; }

		/// <summary>Callback function called when the picker Popover is closing.</summary>
		[Parameter] public EventCallback OnClose { get; set; }

		#endregion

		#region Computed properties

		private CultureInfo EffectiveCulture => Culture ?? CultureInfo.CurrentCulture;

		private string EffectiveFormat => !string.IsNullOrWhiteSpace(DateFormat)
			? DateFormat!
			: Mode == DateTimePickerMode.Date
				? EffectiveCulture.DateTimeFormat.ShortDatePattern
				: $"{EffectiveCulture.DateTimeFormat.ShortDatePattern} {EffectiveCulture.DateTimeFormat.ShortTimePattern}";

		private DayOfWeek EffectiveFirstDayOfWeek => FirstDayOfWeek ?? EffectiveCulture.DateTimeFormat.FirstDayOfWeek;

		private bool Use12HourClock => HasFormatToken('h') || HasFormatToken('t');

		private bool ShowSeconds => HasFormatToken('s');

		private string? FormattedValue => Value?.ToString(EffectiveFormat, EffectiveCulture);

		//Date/time the time editors and day selection are based on when no value is picked yet.
		private DateTime BaselineValue => Value ?? DateTime.Today;

		private int DisplayHour => Use12HourClock
			? (BaselineValue.Hour % 12 == 0 ? 12 : BaselineValue.Hour % 12)
			: BaselineValue.Hour;

		private string AmPmValue => BaselineValue.Hour >= 12
			? EffectiveCulture.DateTimeFormat.PMDesignator
			: EffectiveCulture.DateTimeFormat.AMDesignator;

		private string HeaderLabel => _view switch
		{
			PickerView.Days => _viewDate.ToString(EffectiveCulture.DateTimeFormat.YearMonthPattern, EffectiveCulture),
			PickerView.Months => _viewDate.ToString("yyyy", EffectiveCulture),
			_ => $"{YearRangeStart}-{YearRangeStart + 11}",
		};

		//12 year blocks aligned to year 1: 1-12, 13-24, ... so navigation pages are stable.
		private int YearRangeStart => _viewDate.Year - ((_viewDate.Year - 1) % 12);

		private IEnumerable<string> WeekdayNames
		{
			get
			{
				var names = EffectiveCulture.DateTimeFormat.ShortestDayNames;
				for (var i = 0; i < 7; i++)
				{
					yield return names[((int)EffectiveFirstDayOfWeek + i) % 7];
				}
			}
		}

		//Always 6 weeks (42 cells) so the panel height does not jump between months.
		private IEnumerable<DateTime[]> CalendarWeeks
		{
			get
			{
				var firstOfMonth = new DateTime(_viewDate.Year, _viewDate.Month, 1);
				var offset = ((int)firstOfMonth.DayOfWeek - (int)EffectiveFirstDayOfWeek + 7) % 7;
				var start = SafeAddDays(firstOfMonth, -offset);

				for (var week = 0; week < 6; week++)
				{
					var days = new DateTime[7];
					for (var i = 0; i < 7; i++)
					{
						days[i] = SafeAddDays(start, (week * 7) + i);
					}
					yield return days;
				}
			}
		}

		private IEnumerable<int> HourOptions
		{
			get
			{
				if (Use12HourClock)
				{
					for (var hour = 1; hour <= 12; hour++) yield return hour;
				}
				else
				{
					for (var hour = 0; hour < 24; hour++) yield return hour;
				}
			}
		}

		private IEnumerable<int> MinuteOptions
		{
			get
			{
				var step = Math.Clamp(MinuteStep, 1, 60);
				var minutes = new SortedSet<int>();
				for (var minute = 0; minute < 60; minute += step)
				{
					minutes.Add(minute);
				}
				minutes.Add(BaselineValue.Minute); //Keep an off-step current value selectable.
				return minutes;
			}
		}

		#endregion

		#region Open/close

		//Central place that changes the open/closed state and notifies @bind-IsOpen listeners.
		private async Task SetOpenAsync(bool isOpen)
		{
			if (_isOpen == isOpen)
			{
				return;
			}

			_isOpen = isOpen;
			if (IsOpenChanged.HasDelegate)
			{
				await IsOpenChanged.InvokeAsync(isOpen);
			}
		}

		private async Task OnInputClickAsync()
		{
			if (!Disabled && !ReadOnly)
			{
				await SetOpenAsync(true);
			}
		}

		//The Popover reports its own state changes here too (e.g. outside click or Esc closed it).
		private async Task OnPopoverIsOpenChanged(bool isOpen) => await SetOpenAsync(isOpen);

		private async Task OnPopoverOpenedAsync()
		{
			//Start on the month of the current value (or today) in Days view every time the picker opens.
			_view = PickerView.Days;
			_viewDate = new DateTime(BaselineValue.Year, BaselineValue.Month, 1);

			if (OnOpen.HasDelegate)
			{
				await OnOpen.InvokeAsync();
			}
		}

		private async Task OnPopoverClosedAsync()
		{
			if (OnClose.HasDelegate)
			{
				await OnClose.InvokeAsync();
			}
		}

		private async Task OnInputKeyDownAsync(KeyboardEventArgs args)
		{
			if (string.Equals(args.Key, "Escape", StringComparison.OrdinalIgnoreCase))
			{
				await SetOpenAsync(false);
			}
			else if (string.Equals(args.Key, "ArrowDown", StringComparison.OrdinalIgnoreCase))
			{
				await OnInputClickAsync();
			}
		}

		#endregion

		#region Calendar navigation

		private void NavigatePrevious()
		{
			_viewDate = _view switch
			{
				PickerView.Days => SafeAddMonths(_viewDate, -1),
				PickerView.Months => SafeAddYears(_viewDate, -1),
				_ => SafeAddYears(_viewDate, -12),
			};
		}

		private void NavigateNext()
		{
			_viewDate = _view switch
			{
				PickerView.Days => SafeAddMonths(_viewDate, 1),
				PickerView.Months => SafeAddYears(_viewDate, 1),
				_ => SafeAddYears(_viewDate, 12),
			};
		}

		//Header label click drills up: Days -> Months -> Years.
		private void SwitchViewUp()
		{
			_view = _view switch
			{
				PickerView.Days => PickerView.Months,
				_ => PickerView.Years,
			};
		}

		private void SelectMonth(int month)
		{
			_viewDate = new DateTime(_viewDate.Year, month, 1);
			_view = PickerView.Days;
		}

		private void SelectYear(int year)
		{
			_viewDate = new DateTime(year, _viewDate.Month, 1);
			_view = PickerView.Months;
		}

		#endregion

		#region Selection / value handling

		private async Task SelectDayAsync(DateTime day)
		{
			var newValue = Mode == DateTimePickerMode.Date
				? day.Date
				: day.Date + BaselineValue.TimeOfDay; //Keep the already picked time part.

			await SetValueAsync(newValue);
			_viewDate = new DateTime(day.Year, day.Month, 1);

			if (Mode == DateTimePickerMode.Date)
			{
				await SetOpenAsync(false);
			}
		}

		private async Task SelectTodayAsync()
		{
			if (Mode == DateTimePickerMode.Date)
			{
				await SelectDayAsync(DateTime.Today);
			}
			else
			{
				var now = DateTime.Now;
				await SetValueAsync(new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second));
				_viewDate = new DateTime(now.Year, now.Month, 1);
			}
		}

		private async Task ClearAsync() => await SetValueAsync(null);

		private async Task OnTextChangedAsync(ChangeEventArgs args)
		{
			var text = args.Value?.ToString();

			if (string.IsNullOrWhiteSpace(text))
			{
				await SetValueAsync(null);
			}
			else if (DateTime.TryParseExact(text, EffectiveFormat, EffectiveCulture, DateTimeStyles.AllowWhiteSpaces, out var parsed)
				|| DateTime.TryParse(text, EffectiveCulture, DateTimeStyles.AllowWhiteSpaces, out parsed))
			{
				await SetValueAsync(parsed);
				_viewDate = new DateTime(BaselineValue.Year, BaselineValue.Month, 1);
			}
			else
			{
				WriteDiag($"Could not parse input text: '{text}' with format: '{EffectiveFormat}', culture: '{EffectiveCulture.Name}'.");
			}

			//Discard typed text that does not match the canonical formatted value (invalid, clamped or
			//non-canonical input), otherwise it would stay visible since the rendered 'value' did not change.
			if (!string.Equals(text ?? "", FormattedValue ?? "", StringComparison.Ordinal))
			{
				_inputKey++;
			}
		}

		//Central place that normalizes (Date mode), clamps (Min/MaxDate) and pushes out a new value.
		private async Task SetValueAsync(DateTime? newValue)
		{
			if (newValue.HasValue)
			{
				if (Mode == DateTimePickerMode.Date)
				{
					newValue = newValue.Value.Date;
				}

				var min = Mode == DateTimePickerMode.Date ? MinDate?.Date : MinDate;
				var max = Mode == DateTimePickerMode.Date ? MaxDate?.Date : MaxDate;

				if (min.HasValue && newValue < min)
				{
					newValue = min;
				}
				if (max.HasValue && newValue > max)
				{
					newValue = max;
				}
			}

			if (Nullable.Equals(newValue, Value))
			{
				return;
			}

			Value = newValue;
			WriteDiag($"Value changed to: '{newValue?.ToString("O")}'.");

			if (ValueChanged.HasDelegate)
			{
				await ValueChanged.InvokeAsync(newValue);
			}
		}

		#endregion

		#region Time editing

		private async Task OnHourChangedAsync(ChangeEventArgs args)
		{
			if (!int.TryParse(args.Value?.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var hour))
			{
				return;
			}

			if (Use12HourClock)
			{
				hour = (hour % 12) + (BaselineValue.Hour >= 12 ? 12 : 0);
			}

			await SetTimeAsync(hour, BaselineValue.Minute, BaselineValue.Second);
		}

		private async Task OnMinuteChangedAsync(ChangeEventArgs args)
		{
			if (int.TryParse(args.Value?.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var minute))
			{
				await SetTimeAsync(BaselineValue.Hour, minute, BaselineValue.Second);
			}
		}

		private async Task OnSecondChangedAsync(ChangeEventArgs args)
		{
			if (int.TryParse(args.Value?.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var second))
			{
				await SetTimeAsync(BaselineValue.Hour, BaselineValue.Minute, second);
			}
		}

		private async Task OnAmPmChangedAsync(ChangeEventArgs args)
		{
			var isPm = string.Equals(args.Value?.ToString(), EffectiveCulture.DateTimeFormat.PMDesignator, StringComparison.OrdinalIgnoreCase);
			var hour = (BaselineValue.Hour % 12) + (isPm ? 12 : 0);

			await SetTimeAsync(hour, BaselineValue.Minute, BaselineValue.Second);
		}

		private Task SetTimeAsync(int hour, int minute, int second)
			=> SetValueAsync(BaselineValue.Date + new TimeSpan(hour, minute, second));

		#endregion

		#region Helpers

		private bool IsDayDisabled(DateTime day)
			=> (MinDate.HasValue && day.Date < MinDate.Value.Date)
			|| (MaxDate.HasValue && day.Date > MaxDate.Value.Date);

		private bool IsMonthDisabled(int month)
		{
			var monthStart = new DateTime(_viewDate.Year, month, 1);
			var monthEnd = new DateTime(_viewDate.Year, month, DateTime.DaysInMonth(_viewDate.Year, month));

			return (MinDate.HasValue && monthEnd < MinDate.Value.Date)
				|| (MaxDate.HasValue && monthStart > MaxDate.Value.Date);
		}

		private bool IsYearDisabled(int year)
			=> (MinDate.HasValue && year < MinDate.Value.Year)
			|| (MaxDate.HasValue && year > MaxDate.Value.Year);

		private int GetWeekNumber(DateTime day)
			=> EffectiveCulture.Calendar.GetWeekOfYear(day, EffectiveCulture.DateTimeFormat.CalendarWeekRule, EffectiveFirstDayOfWeek);

		//Returns true when the effective format contains the given custom format specifier outside of quoted literals.
		private bool HasFormatToken(char token)
		{
			char? quote = null;
			foreach (var c in EffectiveFormat)
			{
				if (quote.HasValue)
				{
					if (c == quote.Value)
					{
						quote = null;
					}
					continue;
				}
				if (c == '\'' || c == '"')
				{
					quote = c;
					continue;
				}
				if (c == token)
				{
					return true;
				}
			}
			return false;
		}

		private static DateTime SafeAddDays(DateTime date, int days)
		{
			var ticks = date.Ticks + (days * TimeSpan.TicksPerDay);
			if (ticks < DateTime.MinValue.Ticks)
			{
				return DateTime.MinValue;
			}
			if (ticks > DateTime.MaxValue.Ticks)
			{
				return DateTime.MaxValue.Date;
			}
			return new DateTime(ticks);
		}

		private static DateTime SafeAddMonths(DateTime date, int months)
		{
			try { return date.AddMonths(months); }
			catch (ArgumentOutOfRangeException) { return date; }
		}

		private static DateTime SafeAddYears(DateTime date, int years)
		{
			try { return date.AddYears(years); }
			catch (ArgumentOutOfRangeException) { return date; }
		}

		private void WriteDiag(string message) => Logger.LogDebug($"Component {GetType()}: {message}");

		#endregion
	}
}
