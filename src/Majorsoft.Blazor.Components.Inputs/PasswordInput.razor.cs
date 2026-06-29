using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.Extensions.Logging;

namespace Majorsoft.Blazor.Components.Inputs
{
	/// <summary>
	/// Password/secret text input with a customizable mask character, a customizable layout and a
	/// reveal (eye) button that shows or hides the entered value.
	/// <para>
	/// The real (typed) value is kept in C# at all times; the rendered <c>&lt;input&gt;</c> shows either
	/// the masked representation (a run of <see cref="MaskCharacter"/>) or the plain value, depending on
	/// <see cref="ShowPassword"/>. Editing in the middle of the value, selections, paste and delete are
	/// all supported by reconstructing the real value from the caret position after each input.
	/// </para>
	/// </summary>
	public partial class PasswordInput : ComponentBase, IAsyncDisposable
	{
		[Inject] private IJSRuntime JsRuntime { get; set; } = default!;
		[Inject] private ILogger<PasswordInput> Logger { get; set; } = default!;

		private ElementReference _inputRef;
		private IJSObjectReference? _jsModule;
		//Caret position to restore after Blazor re-renders the masked value (which would otherwise jump to the end).
		private int? _pendingCaret;

		/// <summary>
		/// Exposes the wrapped <see cref="ElementReference"/> of the inner &lt;input&gt;. Useful for JS interop, focus, etc.
		/// </summary>
		public ElementReference InnerElementReference => _inputRef;

		#region Value

		/// <summary>
		/// The secret value. Supports two-way binding via <c>@bind-Value</c>. Reading it always returns the plain text.
		/// </summary>
		[Parameter] public string? Value { get; set; }

		/// <summary>
		/// Callback invoked whenever <see cref="Value"/> changes. Enables <c>@bind-Value</c>.
		/// </summary>
		[Parameter] public EventCallback<string?> ValueChanged { get; set; }

		/// <summary>
		/// Callback invoked on every input/change with the new plain value.
		/// </summary>
		[Parameter] public EventCallback<string?> OnInput { get; set; }

		#endregion

		#region Masking / reveal

		/// <summary>
		/// Character used to mask the value when hidden, e.g. <c>'●'</c> (default), <c>'*'</c> or <c>'•'</c>.
		/// Set to <c>'\0'</c> to disable masking (always show plain text).
		/// </summary>
		[Parameter] public char MaskCharacter { get; set; } = '●';

		/// <summary>
		/// Whether the value is currently revealed (plain text) or masked. Supports two-way binding via <c>@bind-ShowPassword</c>.
		/// Default is false (masked).
		/// </summary>
		[Parameter] public bool ShowPassword { get; set; }

		/// <summary>
		/// Callback invoked when <see cref="ShowPassword"/> changes (e.g. via the reveal button). Enables <c>@bind-ShowPassword</c>.
		/// </summary>
		[Parameter] public EventCallback<bool> ShowPasswordChanged { get; set; }

		/// <summary>
		/// Whether the reveal (eye) button is rendered. Default is true.
		/// </summary>
		[Parameter] public bool ShowRevealButton { get; set; } = true;

		/// <summary>Tooltip/aria-label shown on the reveal button while the value is masked. Default is "Show password".</summary>
		[Parameter] public string ShowTitle { get; set; } = "Show password";

		/// <summary>Tooltip/aria-label shown on the reveal button while the value is revealed. Default is "Hide password".</summary>
		[Parameter] public string HideTitle { get; set; } = "Hide password";

		#endregion

		#region Input options

		/// <summary>Placeholder text shown when the input is empty.</summary>
		[Parameter] public string? Placeholder { get; set; }

		/// <summary>Maximum allowed characters. Default is 0 (unlimited).</summary>
		[Parameter] public int MaxLength { get; set; } = 0;

		/// <summary>When true the input is disabled. Default is false.</summary>
		[Parameter] public bool Disabled { get; set; }

		/// <summary>When true the value is read only. Default is false.</summary>
		[Parameter] public bool ReadOnly { get; set; }

		/// <summary>Value of the rendered <c>autocomplete</c> attribute. Default is "new-password" to discourage autofill.</summary>
		[Parameter] public string AutoComplete { get; set; } = "new-password";

		#endregion

		#region Styling / layout

		/// <summary>Custom CSS class(es) applied to the root wrapper element.</summary>
		[Parameter] public string? Class { get; set; }

		/// <summary>Custom inline style applied to the root wrapper element.</summary>
		[Parameter] public string? Style { get; set; }

		/// <summary>Custom CSS class(es) applied to the inner &lt;input&gt; element.</summary>
		[Parameter] public string? InputClass { get; set; }

		/// <summary>Custom CSS class(es) applied to the reveal (eye) button.</summary>
		[Parameter] public string? RevealButtonClass { get; set; }

		/// <summary>Optional custom content for the reveal button while the value is masked (replaces the default "eye" icon).</summary>
		[Parameter] public RenderFragment? ShowIconContent { get; set; }

		/// <summary>Optional custom content for the reveal button while the value is revealed (replaces the default "eye off" icon).</summary>
		[Parameter] public RenderFragment? HideIconContent { get; set; }

		/// <summary>Arbitrary HTML attributes applied to the inner &lt;input&gt; element.</summary>
		[Parameter(CaptureUnmatchedValues = true)]
		public Dictionary<string, object>? AdditionalAttributes { get; set; }

		#endregion

		//The string actually shown in the <input>: plain value when revealed/unmasked, otherwise a run of MaskCharacter.
		private string DisplayValue
		{
			get
			{
				var value = Value ?? "";
				if (ShowPassword || MaskCharacter == '\0')
				{
					return value;
				}

				return new string(MaskCharacter, value.Length);
			}
		}

		/// <summary>Sets focus to the inner &lt;input&gt;.</summary>
		public async Task FocusAsync()
		{
			var module = await GetModuleAsync();
			if (module is not null)
			{
				await module.InvokeVoidAsync("focusInput", _inputRef);
			}
		}

		/// <summary>Toggles between the masked and the revealed (plain text) state.</summary>
		public Task TogglePasswordVisibilityAsync() => ToggleVisibilityAsync();

		private async Task OnInputAsync(ChangeEventArgs e)
		{
			var newDisplay = e.Value?.ToString() ?? "";
			string newValue;

			if (ShowPassword || MaskCharacter == '\0')
			{
				//Plain text: what is shown is the value. Blazor leaves the DOM value untouched (it matches), so no caret restore needed.
				newValue = newDisplay;
			}
			else
			{
				//Masked: only the freshly typed/pasted chars are plain text in newDisplay; the rest are mask chars.
				//Reconstruct the real value around the edit using the caret position.
				var caret = await GetCaretAsync() ?? newDisplay.Length;
				newValue = ReconstructValue(Value ?? "", newDisplay, caret);

				//After re-render the masked value differs from what the browser holds, so the caret jumps to the end: restore it.
				_pendingCaret = Math.Min(caret, MaxLength > 0 ? Math.Min(newValue.Length, MaxLength) : newValue.Length);
			}

			if (MaxLength > 0 && newValue.Length > MaxLength)
			{
				newValue = newValue.Substring(0, MaxLength);
			}

			await UpdateValueAsync(newValue);
		}

		//Rebuilds the real value from the previous real value, the input's new (partially masked) text and the caret.
		//A single contiguous edit replaces old[s..e) with the freshly entered text new[s..caret); the suffix after the
		//caret is unchanged, which pins down 'e' from the lengths, and the unchanged prefix pins down 's'.
		private string ReconstructValue(string oldReal, string newDisplay, int caret)
		{
			int oldLen = oldReal.Length; //masked display length == real length
			int newLen = newDisplay.Length;

			caret = Math.Clamp(caret, 0, newLen);

			//End of the replaced region in the old value (suffix after caret stayed the same).
			int end = Math.Clamp(oldLen - (newLen - caret), 0, oldLen);

			//Start of the changed region: leading run that still looks masked (and never past the caret or 'end').
			int prefix = 0;
			while (prefix < oldLen && prefix < newLen && newDisplay[prefix] == MaskCharacter)
			{
				prefix++;
			}
			int start = Math.Max(0, Math.Min(Math.Min(prefix, caret), end));

			var inserted = newDisplay.Substring(start, caret - start);
			return oldReal.Substring(0, start) + inserted + oldReal.Substring(end);
		}

		private async Task ToggleVisibilityAsync()
		{
			if (Disabled)
			{
				return;
			}

			ShowPassword = !ShowPassword;
			if (ShowPasswordChanged.HasDelegate)
			{
				await ShowPasswordChanged.InvokeAsync(ShowPassword);
			}

			//Keep the caret at the end and focus on the field so the user can keep typing after toggling.
			_pendingCaret = Value?.Length ?? 0;
		}

		//Central place that pushes a new value out through @bind-Value and the OnInput callback.
		private async Task UpdateValueAsync(string? newValue)
		{
			Value = newValue;

			if (ValueChanged.HasDelegate)
			{
				await ValueChanged.InvokeAsync(newValue);
			}
			if (OnInput.HasDelegate)
			{
				await OnInput.InvokeAsync(newValue);
			}

			Logger.LogDebug($"{nameof(PasswordInput)}: value changed, length: '{newValue?.Length ?? 0}'.");
		}

		#region Lifecycle / JS interop

		/// <inheritdoc />
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (_pendingCaret is not { } caret)
			{
				return;
			}

			_pendingCaret = null;
			var module = await GetModuleAsync();
			if (module is not null)
			{
				await module.InvokeVoidAsync("setCaret", _inputRef, caret);
			}
		}

		private async Task<int?> GetCaretAsync()
		{
			var module = await GetModuleAsync();
			if (module is null)
			{
				return null;
			}

			return await module.InvokeAsync<int?>("getCaret", _inputRef);
		}

		private async Task<IJSObjectReference?> GetModuleAsync()
		{
			try
			{
#if DEBUG
				_jsModule ??= await JsRuntime.InvokeAsync<IJSObjectReference>("import",
					"./_content/Majorsoft.Blazor.Components.Inputs/passwordInput.js");
#else
				_jsModule ??= await JsRuntime.InvokeAsync<IJSObjectReference>("import",
					"./_content/Majorsoft.Blazor.Components.Inputs/passwordInput.min.js");
#endif
				return _jsModule;
			}
			catch (JSDisconnectedException) { return null; }
			catch (ObjectDisposedException) { return null; }
		}

		/// <inheritdoc />
		public async ValueTask DisposeAsync()
		{
			if (_jsModule is not null)
			{
				try
				{
					await _jsModule.DisposeAsync();
				}
				catch (JSDisconnectedException) { }
				catch (ObjectDisposedException) { }
			}
		}

		#endregion
	}
}
