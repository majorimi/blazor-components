using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.Extensions.Logging;

namespace Majorsoft.Blazor.Components.Inputs
{
	/// <summary>
	/// Markdown text editor component with a customizable, sectioned toolbar and a live HTML preview.
	/// <para>
	/// Each toolbar section (Headings, Text style, Lists, Blocks, Insert, View) can be turned on/off
	/// individually via the corresponding <c>Show...Section</c> parameter, and every button shows a
	/// Tooltip (from the Majorsoft Tooltips package) describing its action.
	/// </para>
	/// </summary>
	public partial class MarkdownEditor : ComponentBase, IAsyncDisposable
	{
		[Inject] private IJSRuntime JsRuntime { get; set; } = default!;
		[Inject] private ILogger<MarkdownEditor> Logger { get; set; } = default!;

		private ElementReference _editorRef;
		private IJSObjectReference? _jsModule;
		private (int Start, int End)? _pendingSelection;
		private bool _tabHandlerAttached;

		/// <summary>
		/// Exposes the wrapped <see cref="ElementReference"/> of the inner Markdown source &lt;textarea&gt;.
		/// Useful for JS interop, focus management, etc.
		/// </summary>
		public ElementReference InnerElementReference => _editorRef;

		#region Value

		/// <summary>
		/// The Markdown source text. Supports two-way binding via <c>@bind-Value</c>.
		/// </summary>
		[Parameter] public string? Value { get; set; }

		/// <summary>
		/// Callback invoked whenever <see cref="Value"/> changes (typing or toolbar edits). Enables <c>@bind-Value</c>.
		/// </summary>
		[Parameter] public EventCallback<string?> ValueChanged { get; set; }

		/// <summary>
		/// Callback invoked on every input/change of the Markdown source with the new value.
		/// </summary>
		[Parameter] public EventCallback<string?> OnInput { get; set; }

		/// <summary>
		/// Callback invoked with the rendered HTML whenever the Markdown source changes.
		/// </summary>
		[Parameter] public EventCallback<string> OnHtmlChanged { get; set; }

		#endregion

		#region Editor options

		/// <summary>
		/// Placeholder text shown when the editor is empty. Default is a short hint.
		/// </summary>
		[Parameter] public string Placeholder { get; set; } = "Write Markdown here...";

		/// <summary>
		/// Number of visible text rows of the source editor. Default is 8.
		/// </summary>
		[Parameter] public int Rows { get; set; } = 8;

		/// <summary>
		/// Maximum allowed characters. When greater than 0 a <c>maxlength</c> is applied and a remaining
		/// characters counter is shown in the footer. Default is 0 (unlimited).
		/// </summary>
		[Parameter] public int MaxAllowedChars { get; set; } = 0;

		/// <summary>
		/// When true the editor is disabled (no input, no toolbar actions). Default is false.
		/// </summary>
		[Parameter] public bool Disabled { get; set; }

		/// <summary>
		/// When true the source text is read only (toolbar actions are also disabled). Default is false.
		/// </summary>
		[Parameter] public bool ReadOnly { get; set; }

		/// <summary>
		/// Enables/disables the browser native spellchecker on the source editor. Default is true.
		/// </summary>
		[Parameter] public bool SpellCheck { get; set; } = true;

		/// <summary>
		/// When true pressing Tab inside the editor inserts <see cref="TabIndent"/> instead of moving focus. Default is true.
		/// </summary>
		[Parameter] public bool TabIndents { get; set; } = true;

		/// <summary>
		/// The string inserted when Tab is pressed and <see cref="TabIndents"/> is enabled. Default is two spaces.
		/// </summary>
		[Parameter] public string TabIndent { get; set; } = "  ";

		#endregion

		#region View

		/// <summary>
		/// Which pane(s) are visible: source <see cref="MarkdownEditorView.Edit"/>, rendered
		/// <see cref="MarkdownEditorView.Preview"/> or both <see cref="MarkdownEditorView.Split"/>.
		/// Default is <see cref="MarkdownEditorView.Edit"/>. Supports two-way binding via <c>@bind-View</c>.
		/// </summary>
		[Parameter] public MarkdownEditorView View { get; set; } = MarkdownEditorView.Edit;

		/// <summary>
		/// Callback invoked when <see cref="View"/> changes. Enables <c>@bind-View</c>.
		/// </summary>
		[Parameter] public EventCallback<MarkdownEditorView> ViewChanged { get; set; }

		#endregion

		#region Toolbar sections

		/// <summary>Shows or hides the whole toolbar. Default is true.</summary>
		[Parameter] public bool ShowToolbar { get; set; } = true;

		/// <summary>Shows or hides the Headings section (H1, H2, H3). Default is true.</summary>
		[Parameter] public bool ShowHeadingSection { get; set; } = true;

		/// <summary>Shows or hides the Text style section (Bold, Italic, Underline, Strikethrough). Default is true.</summary>
		[Parameter] public bool ShowTextStyleSection { get; set; } = true;

		/// <summary>Shows or hides the Lists section (Bullet, Numbered, Task). Default is true.</summary>
		[Parameter] public bool ShowListSection { get; set; } = true;

		/// <summary>Shows or hides the Indentation section (Decrease / Increase indent). Default is true.</summary>
		[Parameter] public bool ShowIndentSection { get; set; } = true;

		/// <summary>Shows or hides the Blocks section (Quote, Inline code, Code block, Horizontal rule). Default is true.</summary>
		[Parameter] public bool ShowBlockSection { get; set; } = true;

		/// <summary>Shows or hides the Insert section (Link, Image, Table). Default is true.</summary>
		[Parameter] public bool ShowInsertSection { get; set; } = true;

		/// <summary>Shows or hides the View section (Edit / Split / Preview toggle). Default is true.</summary>
		[Parameter] public bool ShowViewSection { get; set; } = true;

		/// <summary>Shows or hides the footer (hint text and remaining character counter). Default is true.</summary>
		[Parameter] public bool ShowFooter { get; set; } = true;

		/// <summary>When true each toolbar button shows a Tooltip; otherwise a native <c>title</c> is used. Default is true.</summary>
		[Parameter] public bool ShowButtonTooltips { get; set; } = true;

		/// <summary>Optional extra toolbar content rendered as an additional section (e.g. custom buttons).</summary>
		[Parameter] public RenderFragment? CustomToolbarContent { get; set; }

		/// <summary>Text shown on the left side of the footer. Default is a short Markdown hint.</summary>
		[Parameter] public string FooterText { get; set; } = "Markdown supported";

		#endregion

		#region Styling

		/// <summary>Custom CSS class(es) applied to the root element.</summary>
		[Parameter] public string? Class { get; set; }

		/// <summary>Custom inline style applied to the root element.</summary>
		[Parameter] public string? Style { get; set; }

		/// <summary>Custom CSS class(es) applied to the toolbar element.</summary>
		[Parameter] public string? ToolbarClass { get; set; }

		/// <summary>Custom CSS class(es) applied to the source &lt;textarea&gt;.</summary>
		[Parameter] public string? EditorClass { get; set; }

		/// <summary>Custom CSS class(es) applied to the rendered preview pane.</summary>
		[Parameter] public string? PreviewClass { get; set; }

		/// <summary>Arbitrary HTML attributes applied to the rendered root element.</summary>
		[Parameter(CaptureUnmatchedValues = true)]
		public Dictionary<string, object>? AdditionalAttributes { get; set; }

		#endregion

		/// <summary>The rendered HTML of the current <see cref="Value"/>.</summary>
		private string Html => MarkdownConverter.ToHtml(Value);

		private bool IsPreview => View == MarkdownEditorView.Preview;

		private int RemainingChars => Math.Max(0, MaxAllowedChars - (Value?.Length ?? 0));

		/// <summary>Returns the rendered HTML of the current Markdown <see cref="Value"/>.</summary>
		public string GetHtml() => Html;

		/// <summary>Sets focus to the inner Markdown source editor.</summary>
		public async Task FocusAsync()
		{
			var module = await GetModuleAsync();
			if (module is not null)
			{
				await module.InvokeVoidAsync("focusEditor", _editorRef);
			}
		}

		#region Editing operations

		//Maps a shared toolbar command to the corresponding Markdown source manipulation.
		private Task HandleCommandAsync(EditorCommand command) => command switch
		{
			EditorCommand.Heading1 => ToggleLinePrefixAsync("# "),
			EditorCommand.Heading2 => ToggleLinePrefixAsync("## "),
			EditorCommand.Heading3 => ToggleLinePrefixAsync("### "),
			EditorCommand.Bold => WrapSelectionAsync("**", "**", "bold text"),
			EditorCommand.Italic => WrapSelectionAsync("*", "*", "italic text"),
			EditorCommand.Underline => WrapSelectionAsync("<u>", "</u>", "underlined text"),
			EditorCommand.Strikethrough => WrapSelectionAsync("~~", "~~", "strikethrough"),
			EditorCommand.BulletList => ToggleLinePrefixAsync("- "),
			EditorCommand.NumberedList => OrderedListAsync(),
			EditorCommand.TaskList => ToggleLinePrefixAsync("- [ ] "),
			EditorCommand.IndentIncrease => IndentLinesAsync(true),
			EditorCommand.IndentDecrease => IndentLinesAsync(false),
			EditorCommand.Quote => ToggleLinePrefixAsync("> "),
			EditorCommand.InlineCode => WrapSelectionAsync("`", "`", "code"),
			EditorCommand.CodeBlock => InsertBlockAsync("```\n", "\n```", "code"),
			EditorCommand.HorizontalRule => InsertBlockAsync("\n---\n", "", ""),
			EditorCommand.Link => WrapSelectionAsync("[", "](https://)", "link text"),
			EditorCommand.Image => WrapSelectionAsync("![", "](https://)", "alt text"),
			EditorCommand.Table => InsertBlockAsync(
				"\n| Column 1 | Column 2 |\n| -------- | -------- |\n| Cell 1   | Cell 2   |\n", "", ""),
			_ => Task.CompletedTask,
		};

		private async Task OnEditorInputAsync(ChangeEventArgs e)
		{
			await UpdateValueAsync(e.Value?.ToString());
		}

		//Wraps the current selection (or inserts the placeholder when nothing is selected) with before/after.
		private async Task WrapSelectionAsync(string before, string after, string placeholder)
		{
			if (!await CanEdit())
			{
				return;
			}

			var info = await GetSelectionAsync();
			var value = info.Value ?? "";
			var selected = value.Substring(info.Start, info.End - info.Start);
			var content = selected.Length > 0 ? selected : placeholder;

			var newValue = value.Substring(0, info.Start) + before + content + after + value.Substring(info.End);
			var selStart = info.Start + before.Length;
			_pendingSelection = (selStart, selStart + content.Length);

			await UpdateValueAsync(newValue);
		}

		//Inserts a block of text at the caret, replacing any selection. Caret is placed over the placeholder.
		private async Task InsertBlockAsync(string before, string after, string placeholder)
		{
			if (!await CanEdit())
			{
				return;
			}

			var info = await GetSelectionAsync();
			var value = info.Value ?? "";
			var inserted = before + placeholder + after;

			var newValue = value.Substring(0, info.Start) + inserted + value.Substring(info.End);
			var selStart = info.Start + before.Length;
			_pendingSelection = (selStart, selStart + placeholder.Length);

			await UpdateValueAsync(newValue);
		}

		//Adds (or removes when all lines already have it) the given prefix on every line of the selection.
		private async Task ToggleLinePrefixAsync(string prefix)
		{
			if (!await CanEdit())
			{
				return;
			}

			var info = await GetSelectionAsync();
			var value = info.Value ?? "";

			var lineStart = info.Start == 0 ? 0 : value.LastIndexOf('\n', info.Start - 1) + 1;
			var lineEnd = value.IndexOf('\n', info.End);
			if (lineEnd == -1)
			{
				lineEnd = value.Length;
			}

			var block = value.Substring(lineStart, lineEnd - lineStart);
			var lines = block.Split('\n');

			var allPrefixed = Array.TrueForAll(lines, l => l.StartsWith(prefix));
			for (var i = 0; i < lines.Length; i++)
			{
				lines[i] = allPrefixed ? lines[i].Substring(prefix.Length) : prefix + lines[i];
			}

			var newBlock = string.Join("\n", lines);
			var newValue = value.Substring(0, lineStart) + newBlock + value.Substring(lineEnd);
			_pendingSelection = (lineStart, lineStart + newBlock.Length);

			await UpdateValueAsync(newValue);
		}

		//Numbers every selected line as an ordered list (or removes the numbering when already numbered).
		private async Task OrderedListAsync()
		{
			if (!await CanEdit())
			{
				return;
			}

			var info = await GetSelectionAsync();
			var value = info.Value ?? "";

			var lineStart = info.Start == 0 ? 0 : value.LastIndexOf('\n', info.Start - 1) + 1;
			var lineEnd = value.IndexOf('\n', info.End);
			if (lineEnd == -1)
			{
				lineEnd = value.Length;
			}

			var block = value.Substring(lineStart, lineEnd - lineStart);
			var lines = block.Split('\n');

			var numbered = new System.Text.RegularExpressions.Regex(@"^\d+\.\s");
			var allNumbered = Array.TrueForAll(lines, l => numbered.IsMatch(l));
			var counter = 1;
			for (var i = 0; i < lines.Length; i++)
			{
				lines[i] = allNumbered ? numbered.Replace(lines[i], "") : $"{counter++}. {lines[i]}";
			}

			var newBlock = string.Join("\n", lines);
			var newValue = value.Substring(0, lineStart) + newBlock + value.Substring(lineEnd);
			_pendingSelection = (lineStart, lineStart + newBlock.Length);

			await UpdateValueAsync(newValue);
		}

		//Indents (or outdents) every selected line by two spaces, e.g. to nest list items. The caret/selection
		//is shifted to follow the text (not expanded over the whole block) so the inserted spaces aren't shown
		//as a selection, matching the RichText editor's native behavior.
		private async Task IndentLinesAsync(bool increase)
		{
			if (!await CanEdit())
			{
				return;
			}

			const string unit = "  ";
			var info = await GetSelectionAsync();
			var value = info.Value ?? "";

			var lineStart = info.Start == 0 ? 0 : value.LastIndexOf('\n', info.Start - 1) + 1;
			var lineEnd = value.IndexOf('\n', info.End);
			if (lineEnd == -1)
			{
				lineEnd = value.Length;
			}

			var lines = value.Substring(lineStart, lineEnd - lineStart).Split('\n');

			var mappedStart = info.Start;
			var mappedEnd = info.End;
			var origCursor = lineStart;
			var newCursor = lineStart;

			for (var i = 0; i < lines.Length; i++)
			{
				var original = lines[i];
				int removed; //leading chars removed; negative means indentation was added.
				if (increase)
				{
					lines[i] = unit + original;
					removed = -unit.Length;
				}
				else
				{
					var r = 0;
					while (r < unit.Length && r < original.Length && (original[r] == ' ' || original[r] == '\t'))
					{
						r++;
					}
					lines[i] = original.Substring(r);
					removed = r;
				}

				var origLineEnd = origCursor + original.Length;
				var newLineLen = lines[i].Length;

				//Map each boundary that lies on this line, keeping it within the line's new bounds.
				if (info.Start >= origCursor && info.Start <= origLineEnd)
				{
					mappedStart = newCursor + Math.Clamp(info.Start - origCursor - removed, 0, newLineLen);
				}
				if (info.End >= origCursor && info.End <= origLineEnd)
				{
					mappedEnd = newCursor + Math.Clamp(info.End - origCursor - removed, 0, newLineLen);
				}

				origCursor = origLineEnd + 1;
				newCursor += newLineLen + 1;
			}

			var newBlock = string.Join("\n", lines);
			var newValue = value.Substring(0, lineStart) + newBlock + value.Substring(lineEnd);
			_pendingSelection = (mappedStart, mappedEnd);

			await UpdateValueAsync(newValue);
		}

		private async Task SetViewAsync(MarkdownEditorView view)
		{
			if (View == view)
			{
				return;
			}

			View = view;
			if (ViewChanged.HasDelegate)
			{
				await ViewChanged.InvokeAsync(view);
			}
		}

		//Central place that pushes a new value out through @bind-Value and the change/HTML callbacks.
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
			if (OnHtmlChanged.HasDelegate)
			{
				await OnHtmlChanged.InvokeAsync(Html);
			}

			Logger.LogDebug($"{nameof(MarkdownEditor)}: value changed, length: '{newValue?.Length ?? 0}'.");
		}

		private async Task<bool> CanEdit()
		{
			if (Disabled || ReadOnly)
			{
				return false;
			}

			return await GetModuleAsync() is not null;
		}

		private async Task<SelectionInfo> GetSelectionAsync()
		{
			var module = await GetModuleAsync();
			if (module is null)
			{
				return new SelectionInfo { Value = Value ?? "" };
			}

			var info = await module.InvokeAsync<SelectionInfo?>("getSelectionInfo", _editorRef);
			return info ?? new SelectionInfo { Value = Value ?? "" };
		}

		//Selection info returned from JS. Defaults to the end of the current value when unavailable.
		private sealed class SelectionInfo
		{
			public int Start { get; set; }
			public int End { get; set; }
			public string? Value { get; set; }
		}

		#endregion

		#region Lifecycle / JS interop

		/// <inheritdoc />
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			var module = await GetModuleAsync();
			if (module is null)
			{
				return;
			}

			if (TabIndents && !_tabHandlerAttached && View != MarkdownEditorView.Preview)
			{
				_tabHandlerAttached = await module.InvokeAsync<bool>("enableTabIndent", _editorRef, TabIndent);
			}

			//Restore the caret/selection after a toolbar edit, once Blazor has written the new value.
			if (_pendingSelection is { } selection)
			{
				_pendingSelection = null;
				await module.InvokeVoidAsync("setSelection", _editorRef, selection.Start, selection.End);
			}
		}

		private async Task<IJSObjectReference?> GetModuleAsync()
		{
			try
			{
#if DEBUG
				_jsModule ??= await JsRuntime.InvokeAsync<IJSObjectReference>("import",
					"./_content/Majorsoft.Blazor.Components.Inputs/markdownEditor.js");
#else
				_jsModule ??= await JsRuntime.InvokeAsync<IJSObjectReference>("import",
					"./_content/Majorsoft.Blazor.Components.Inputs/markdownEditor.min.js");
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
