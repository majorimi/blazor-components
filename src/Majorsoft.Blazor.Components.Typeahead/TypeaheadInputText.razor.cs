using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Majorsoft.Blazor.Components.Common.JsInterop.ElementInfo;
using Majorsoft.Blazor.Components.Common.JsInterop.Scroll;
using Majorsoft.Blazor.Components.Core.HtmlColors;
using Majorsoft.Blazor.Components.Debounce;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;

namespace Majorsoft.Blazor.Components.Typeahead
{
	public partial class TypeaheadInputText<TItem> : ComponentBase, IAsyncDisposable
	{
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			_rect = await _typeahead.InnerElementReference.GetClientRectAsync();

			if (firstRender)
			{
				CheckRequiredPropertySelector();
			}
		}
		private string _componentId = Guid.NewGuid().ToString("n");
		private DomRect _rect;

		private bool _isOpen = false;
		private bool IsOpen
		{
			get => _isOpen;
			set
			{
				if (_isOpen == value)
					return;

				_isOpen = value;
				if (_isOpen)
				{
					if (OnDropdownOpen.HasDelegate)
					{
						InvokeAsync(async () =>
						{
							await OnDropdownOpen.InvokeAsync();
						});
					}
				}
				else
				{
					if (OnDropdownClose.HasDelegate)
					{
						InvokeAsync(async () =>
						{
							await OnDropdownClose.InvokeAsync();
						});
					}
				}
			}
		}

		private ElementReference _dropDown;
		private bool _hasNoResult = false;
		private bool _isSearching = false;
		private DebounceInputText _typeahead;
		private IEnumerable<TItem> _data;

		private TItem _activeItem = default(TItem);
		private TItem ActiveItem
		{
			get => _activeItem;
			set
			{
				_activeItem = value;
			}
		}

		public ElementReference InnerElementReference => _typeahead.InnerElementReference;

		//Values
		private string _value;
		[Parameter]
		public string Value
		{
			get => _value;
			set
			{
				if (value == _value)
				{
					return;
				}

				_value = value;
				if (OnInput.HasDelegate) //Immediately notify listeners of text change e.g. @bind
				{
					InvokeAsync(async () =>
					{
						await OnInput.InvokeAsync(_value);
					});
				}
			}
		}
		private TItem _selectedItem;
		[Parameter]
		public TItem SelectedItem
		{
			get => _selectedItem;
			set
			{
				if (_selectedItem?.Equals(value) ?? false)
				{
					return;
				}

				_selectedItem = value;
				Value = GetItemText(_selectedItem);
				if (OnSelectedItemChanged.HasDelegate) //Immediately notify listeners for model selected
				{
					InvokeAsync(async () =>
					{
						await OnSelectedItemChanged.InvokeAsync(_selectedItem);
					});
				}
			}
		}
		//[Parameter] public IEnumerable<TItem> SelectedItems {get; set; }

		//Data
		[Parameter] public IEnumerable<TItem> Data { get; set; } //Data for static values
		[Parameter] public Func<string, Task<IEnumerable<TItem>>> DataSource { get; set; } //Async Func to provide data
		[Parameter] public Func<TItem, string> LabelPropertySelector { get; set; } //When TItem is not string

		//Behaviors
		[Parameter] public bool SelectOnBlur { get; set; } = true;
		[Parameter] public bool ShowAllOnEmptyInput { get; set; } = true;
		[Parameter] public bool ChangeActiveItemOnHover { get; set; } = true;
		//[Parameter] public bool MultiSelect { get; set; } = false;

		private string _accentColor = "230, 230, 230";//gray

		[Parameter]
		public string AccentColor
		{
			get => _accentColor;
			set => _accentColor = new HtmlColor(value)?.RgbColor.ToRgbString();
		}

		//Size
		[Parameter] public double DropdownHeight { get; set; } = 150;
		[Parameter] public double DropdownWidth { get; set; } = 0; //auto
		[Parameter] public bool FitDropdownWidth { get; set; } = false;

		private string GetDropdownWidth()
		{
			if (FitDropdownWidth)
			{
				return _rect is null
					? "100%"
					: $"{_rect?.Width.ToString("0.00", CultureInfo.InvariantCulture)}px";
			}

			return DropdownWidth == 0 ? "auto" : $"{DropdownWidth}px";
		}

		//Templates
		[Parameter] public RenderFragment<TItem> ItemTemplate { get; set; }
		[Parameter] public RenderFragment NoResultContent { get; set; }
		[Parameter] public RenderFragment InProgressContent { get; set; }

		//Debounce
		[Parameter] public double DebounceTime { get; set; } = 0;
		[Parameter] public int MinLength { get; set; } = 0;

		//Events
		[Parameter] public EventCallback<string> OnInput { get; set; }
		[Parameter] public EventCallback<TItem> OnSelectedItemChanged { get; set; }
		[Parameter] public EventCallback OnDropdownOpen { get; set; }
		[Parameter] public EventCallback OnDropdownClose { get; set; }
		[Parameter] public EventCallback<FocusEventArgs> OnFocus { get; set; }

		[Parameter(CaptureUnmatchedValues = true)]
		public Dictionary<string, object> AdditionalAttributes { get; set; }

		private async Task OnValueChanged(string value)
		{
			WriteDiag($"{nameof(OnValueChanged)} event new Value: '{value}', ShowAllOnEmptyInput: '{ShowAllOnEmptyInput}', MinLength: '{MinLength}'.");

			_hasNoResult = false;
			IsOpen = false;

			if (string.IsNullOrEmpty(value) && DataSource is null && ShowAllOnEmptyInput && (Data?.Any() ?? false)) //ShowAllOnEmptyInput
			{
				if (MinLength == 0)
				{
					WriteDiag($"{nameof(OnValueChanged)} event opening dropdown DataSource is NULL and ShowAllOnEmptyInput: '{ShowAllOnEmptyInput}', MinLength: '{MinLength}'.");
					_data = Data;
					IsOpen = true;
					ActiveItem = _data.First();
				}
				return;
			}

			try
			{
				_isSearching = true;

				if (DataSource is not null) //Priority is async search
				{
					WriteDiag($"{nameof(OnValueChanged)} DataSource is defined, async search Func will be called search Value: '{value}'.");
					IsOpen = false;
					_data = await DataSource(value);
				}
				else if ((Data?.Any() ?? false) && !string.IsNullOrEmpty(value)) //Search in data with given query value
				{
					WriteDiag($"{nameof(OnValueChanged)} DataSource NOT is defined, filtering static data Type: '{typeof(TItem)}', LabelPropertySelector: '{LabelPropertySelector}'  search Value: '{value}'.");
					if (typeof(TItem) == typeof(string)) //Data is string
						_data = Data.Cast<string>().Where(x => x?.ToLower().Contains(value.ToLower()) ?? false).Cast<TItem>();
					else
						_data = Data.Where(x => LabelPropertySelector(x)?.ToLower().Contains(value.ToLower()) ?? false);
				}
				else
				{
					WriteDiag($"{nameof(OnValueChanged)} no search term met data set to NULL search Value: '{value}'.");
					_data = null;
				}

				WriteDiag($"{nameof(OnValueChanged)} event data filtered result Count: '{_data?.Count()}' handling dropdown.");
				if (_data?.Count() > 0)
				{
					IsOpen = true;
					_hasNoResult = false;
					ActiveItem = _data.First();
				}
				else
				{
					IsOpen = false;
					_hasNoResult = true;
				}
			}
			finally
			{
				_isSearching = false;
			}
		}

		private async Task OnKeyDown(KeyboardEventArgs e)
		{
			WriteDiag($"OnKeyDown event: '{e.Key}'");

			if (e.Key?.Equals("Escape", StringComparison.OrdinalIgnoreCase) ?? false)
			{
				IsOpen = false;
			}
			else if (e.Key?.Equals("Enter", StringComparison.OrdinalIgnoreCase) ?? false)
			{
				if (IsOpen)
				{
					await SelectItem(ActiveItem);
				}
			}
			else if (e.Key?.Equals("ArrowDown", StringComparison.OrdinalIgnoreCase) ?? false)
			{
				if (_data is null || !IsOpen)
				{
					return;
				}

				var index = Array.IndexOf(_data.ToArray(), ActiveItem);
				index++;
				if (index >= _data.Count())
				{
					index = 0;
					await _dropDown.ScrollToTopAsync();
				}

				ActiveItem = _data.ElementAt(index);

				if (index != 0)
				{
					await _dropDown.ScrollInParentByClassAsync("list-item active");
				}
			}
			else if (e.Key?.Equals("ArrowUp", StringComparison.OrdinalIgnoreCase) ?? false)
			{
				if (_data is null || !IsOpen)
				{
					return;
				}

				var index = Array.IndexOf(_data.ToArray(), ActiveItem);
				index--;
				if (index < 0)
				{
					index = _data.Count() - 1;
					await _dropDown.ScrollToEndAsync();
				}

				ActiveItem = _data.ElementAt(index);

				if (index != _data.Count() - 1)
				{
					await _dropDown.ScrollInParentByClassAsync("list-item active");
				}
			}
		}
		private async Task OnFocused(FocusEventArgs e)
		{
			WriteDiag($"{nameof(OnFocused)} event: '{e.Type}'.");
			_hasNoResult = false;

			await _clickHandler.RegisterClickBoundariesAsync(_typeahead.InnerElementReference, OnOutsideClick, OnInsideClick);

			await Activate();

			if (OnFocus.HasDelegate)
			{
				WriteDiag($"{nameof(OnFocus)} delegate: '{e.Type}'.");
				await OnFocus.InvokeAsync(e);
			}
		}
		private async Task OnOutsideClick(MouseEventArgs e)
		{
			WriteDiag($"{nameof(OnOutsideClick)} event button: '{e.Button}'.");

			await _clickHandler.RemoveClickBoundariesAsync(_typeahead.InnerElementReference);
			if (SelectOnBlur && IsOpen)
			{
				await SelectItem(ActiveItem);
				return;
			}

			IsOpen = false;
			StateHasChanged(); //textbox re-rendered but dropdown not
		}
		private async Task OnInsideClick(MouseEventArgs e)
		{
			WriteDiag($"{nameof(OnInsideClick)} event button: '{e.Button}'.");

			await Activate();
			StateHasChanged(); //textbox re-rendered but dropdown not
		}

		private void ItemHover(int index, TItem item)
		{
			WriteDiag($"{nameof(ItemHover)} event index: '{index}', item: '{item}', ChangeActiveItemOnHover: {ChangeActiveItemOnHover}.");
			if (ChangeActiveItemOnHover)
			{
				ActiveItem = item;
			}
		}
		private async Task ItemClicked(int index, TItem item)
		{
			WriteDiag($"{nameof(ItemClicked)} event index: '{index}', item: '{item}'.");
			ActiveItem = item;
			await SelectItem(ActiveItem);
		}

		private async Task Activate()
		{
			if (!IsOpen && DataSource is null && ShowAllOnEmptyInput && MinLength == 0)
			{
				WriteDiag($"{nameof(Activate)} event opening dropdown DataSource is NULL and ShowAllOnEmptyInput: '{ShowAllOnEmptyInput}', MinLength: '{MinLength}', IsOpen: '{IsOpen}'.");
				if (string.IsNullOrWhiteSpace(Value))
				{
					_data = Data;

					if (!IsOpen)
					{
						IsOpen = true;
						ActiveItem = _data.First();
					}
				}
				else
				{
					await OnValueChanged(Value);
				}
			}
		}
		private async Task SelectItem(TItem item)
		{
			_selectedItem = item;
			Value = GetItemText(item);
			IsOpen = false;
			try
			{
				if (OnSelectedItemChanged.HasDelegate)
				{
					await OnSelectedItemChanged.InvokeAsync(item);
				}
			}
			finally { }
		}

		private bool IsLabelSelectorRequired() => typeof(TItem) != typeof(string);
		private bool IsDefaultSearchUsed() => Data is not null && DataSource is null;

		private void CheckRequiredPropertySelector()
		{
			if (IsLabelSelectorRequired() && LabelPropertySelector == null)
			{
				throw new Exception($"Data type: {typeof(TItem)} is not Sytem.String. You must provide a string property selector use '{LabelPropertySelector}'");
			}
		}
		private MarkupString HighlightItems(string text)
		{
			if (IsDefaultSearchUsed() && !string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(Value))
			{
				var pattern = $"({Value})";
				var regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
				var replacement = "<strong>$0</strong>";

				var result = regex.Replace(text, replacement);
				return ((MarkupString)result);
			}

			return (MarkupString)(text ?? string.Empty);
		}
		private string GetItemText(TItem item)
		{
			string text = IsLabelSelectorRequired() && LabelPropertySelector is not null
				? LabelPropertySelector(item)
				: item.ToString();

			return text;
		}

		private void WriteDiag(string message)
		{
			_logger.LogDebug($"Component {this.GetType()}: {message}");
		}

		public async ValueTask DisposeAsync()
		{
			if (_clickHandler is not null)
			{
				await _clickHandler.DisposeAsync();
			}
		}
	}
}