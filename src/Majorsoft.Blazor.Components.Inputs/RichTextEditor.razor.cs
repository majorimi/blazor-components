using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Microsoft.Extensions.Logging;

namespace Majorsoft.Blazor.Components.Inputs
{
	/// <summary>
	/// WYSIWYG RichText editor component. Formatting (bold, italic, headings, lists, links, etc.) is applied
	/// inline on a live <c>contenteditable</c> surface &mdash; no markup is shown to the user &mdash; while the
	/// bound <see cref="Value"/> is kept as Markdown behind the scenes.
	/// <para>
	/// It shares the sectioned <see cref="EditorToolbar"/> and the <see cref="MarkdownConverter"/> with
	/// <see cref="MarkdownEditor"/>; each toolbar section can be turned on/off individually.
	/// </para>
	/// </summary>
	public partial class RichTextEditor : ComponentBase, IAsyncDisposable
	{
		[Inject] private IJSRuntime JsRuntime { get; set; } = default!;
		[Inject] private ILogger<RichTextEditor> Logger { get; set; } = default!;

		private ElementReference _editorRef;
		private IJSObjectReference? _jsModule;
		//The Markdown last synchronized with the DOM; used to tell user edits from external Value changes
		//so the contenteditable surface is only re-populated when the value really changed from outside.
		private string? _domMarkdown;
		private bool _needsHtmlSync;
		//Tracks the last Text style visibility pushed to JS so native shortcuts (Ctrl+B/I/U) are only
		//re-configured when the section is toggled, not on every render.
		private bool? _shortcutsEnabled;

		//Editable fields backing the Link / Image / Table insert popovers (hosted by the toolbar).
		private string _linkText = "";
		private string _linkUrl = "https://";
		private string _imageAlt = "";
		private string _imageUrl = "";
		private int _tableRows = 2;
		private int _tableColumns = 2;

		/// <summary>
		/// Exposes the wrapped <see cref="ElementReference"/> of the inner <c>contenteditable</c> element.
		/// </summary>
		public ElementReference InnerElementReference => _editorRef;

		#region Value

		/// <summary>
		/// The Markdown source text. Supports two-way binding via <c>@bind-Value</c>. The user edits rich
		/// content; this value is always serialized to/from Markdown.
		/// </summary>
		[Parameter] public string? Value { get; set; }

		/// <summary>Callback invoked whenever <see cref="Value"/> changes. Enables <c>@bind-Value</c>.</summary>
		[Parameter] public EventCallback<string?> ValueChanged { get; set; }

		/// <summary>Callback invoked on every edit with the new Markdown value.</summary>
		[Parameter] public EventCallback<string?> OnInput { get; set; }

		/// <summary>Callback invoked with the rendered HTML whenever the value changes.</summary>
		[Parameter] public EventCallback<string> OnHtmlChanged { get; set; }

		#endregion

		#region Editor options

		/// <summary>Placeholder text shown when the editor is empty. Default is a short hint.</summary>
		[Parameter] public string Placeholder { get; set; } = "Write something...";

		/// <summary>When true the editor is disabled (no input, no toolbar actions). Default is false.</summary>
		[Parameter] public bool Disabled { get; set; }

		/// <summary>When true the content is read only (toolbar actions are also disabled). Default is false.</summary>
		[Parameter] public bool ReadOnly { get; set; }

		/// <summary>Enables/disables the browser native spellchecker. Default is true.</summary>
		[Parameter] public bool SpellCheck { get; set; } = true;

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

		/// <summary>Shows or hides the footer (hint text and word counter). Default is true.</summary>
		[Parameter] public bool ShowFooter { get; set; } = true;

		/// <summary>When true each toolbar button shows a Tooltip; otherwise a native <c>title</c> is used. Default is true.</summary>
		[Parameter] public bool ShowButtonTooltips { get; set; } = true;

		/// <summary>Optional extra toolbar content rendered as an additional section (e.g. custom buttons).</summary>
		[Parameter] public RenderFragment? CustomToolbarContent { get; set; }

		/// <summary>Text shown on the left side of the footer.</summary>
		[Parameter] public string FooterText { get; set; } = "RichText editor";

		#endregion

		#region Styling

		/// <summary>Custom CSS class(es) applied to the root element.</summary>
		[Parameter] public string? Class { get; set; }

		/// <summary>Custom inline style applied to the root element.</summary>
		[Parameter] public string? Style { get; set; }

		/// <summary>Custom CSS class(es) applied to the toolbar element.</summary>
		[Parameter] public string? ToolbarClass { get; set; }

		/// <summary>Custom CSS class(es) applied to the editable content surface.</summary>
		[Parameter] public string? EditorClass { get; set; }

		/// <summary>Arbitrary HTML attributes applied to the rendered root element.</summary>
		[Parameter(CaptureUnmatchedValues = true)]
		public Dictionary<string, object>? AdditionalAttributes { get; set; }

		#endregion

		/// <summary>Returns the rendered HTML of the current Markdown <see cref="Value"/>.</summary>
		public string GetHtml() => MarkdownConverter.ToHtml(Value);

		private int WordCount => string.IsNullOrWhiteSpace(Value)
			? 0
			: Value.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;

		/// <summary>Sets focus to the editable content surface.</summary>
		public async Task FocusAsync()
		{
			var module = await GetModuleAsync();
			if (module is not null)
			{
				await module.InvokeVoidAsync("focusEditor", _editorRef);
			}
		}

		/// <inheritdoc />
		protected override void OnParametersSet()
		{
			//Value changed from outside (not by our own serialization): the DOM needs to be re-populated.
			if (Value != _domMarkdown)
			{
				_needsHtmlSync = true;
			}
		}

		//Maps a shared toolbar command to the matching contenteditable formatting command (applied in JS).
		//Link, Image and Table are not raised here: they are toolbar Popover triggers handled below.
		private async Task HandleCommandAsync(EditorCommand command)
		{
			if (Disabled || ReadOnly)
			{
				return;
			}

			var module = await GetModuleAsync();
			if (module is null)
			{
				return;
			}

			var markdown = await module.InvokeAsync<string?>("command", _editorRef, command.ToString());
			await CommitAsync(markdown);
		}

		//Called when a toolbar Insert Popover opens: snapshot the current selection (so it survives the
		//focus moving into the popover form) and reset that form's fields.
		private async Task OnInsertPopoverOpenAsync(EditorCommand command)
		{
			if (Disabled || ReadOnly)
			{
				return;
			}

			var module = await GetModuleAsync();
			if (module is null)
			{
				return;
			}

			switch (command)
			{
				case EditorCommand.Link:
					_linkText = await module.InvokeAsync<string>("saveSelection", _editorRef);
					_linkUrl = "https://";
					break;
				case EditorCommand.Image:
					await module.InvokeVoidAsync("saveSelection", _editorRef);
					_imageAlt = "";
					_imageUrl = "";
					break;
				case EditorCommand.Table:
					await module.InvokeVoidAsync("saveSelection", _editorRef);
					_tableRows = 2;
					_tableColumns = 2;
					break;
			}
		}

		//Enter accepts the open insert form; Escape is handled by the Popover itself (CloseOnEscapeKey).
		private async Task OnPopoverFormKeyDownAsync(KeyboardEventArgs e, EditorToolbar.InsertPopoverContext context)
		{
			if (!string.Equals(e.Key, "Enter", StringComparison.OrdinalIgnoreCase))
			{
				return;
			}

			switch (context.Command)
			{
				case EditorCommand.Link: await ConfirmLinkAsync(); break;
				case EditorCommand.Image: await ConfirmImageAsync(); break;
				case EditorCommand.Table: await ConfirmTableAsync(); break;
			}

			await context.Close.InvokeAsync();
		}

		private async Task ConfirmLinkAsync()
		{
			var module = await GetModuleAsync();
			if (module is not null)
			{
				var markdown = await module.InvokeAsync<string?>("insertLink", _editorRef, _linkText, _linkUrl);
				await CommitAsync(markdown);
			}
		}

		private async Task ConfirmImageAsync()
		{
			if (string.IsNullOrEmpty(_imageUrl))
			{
				return;
			}

			var module = await GetModuleAsync();
			if (module is not null)
			{
				var markdown = await module.InvokeAsync<string?>("insertImage", _editorRef, _imageAlt, _imageUrl);
				await CommitAsync(markdown);
			}
		}

		private async Task ConfirmTableAsync()
		{
			var module = await GetModuleAsync();
			if (module is not null)
			{
				var markdown = await module.InvokeAsync<string?>("insertTable", _editorRef,
					Math.Clamp(_tableRows, 1, 50), Math.Clamp(_tableColumns, 1, 20));
				await CommitAsync(markdown);
			}
		}

		private async Task OnContentInputAsync()
		{
			var module = await GetModuleAsync();
			if (module is null)
			{
				return;
			}

			var markdown = await module.InvokeAsync<string?>("getMarkdown", _editorRef);
			await CommitAsync(markdown);
		}

		//Stores the Markdown read back from the DOM and notifies listeners, without forcing a DOM re-sync.
		private async Task CommitAsync(string? markdown)
		{
			markdown ??= "";
			_domMarkdown = markdown;
			Value = markdown;

			if (ValueChanged.HasDelegate)
			{
				await ValueChanged.InvokeAsync(markdown);
			}
			if (OnInput.HasDelegate)
			{
				await OnInput.InvokeAsync(markdown);
			}
			if (OnHtmlChanged.HasDelegate)
			{
				await OnHtmlChanged.InvokeAsync(MarkdownConverter.ToHtml(markdown));
			}

			Logger.LogDebug($"{nameof(RichTextEditor)}: value changed, length: '{markdown.Length}'.");
		}

		/// <inheritdoc />
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (!_needsHtmlSync && _shortcutsEnabled == ShowTextStyleSection)
			{
				return;
			}

			var module = await GetModuleAsync();
			if (module is null)
			{
				return;
			}

			//Gate the native Ctrl+B/I/U shortcuts on the Text style section so a hidden section also
			//disables its keyboard accelerators (the three share that section's visibility).
			if (_shortcutsEnabled != ShowTextStyleSection)
			{
				_shortcutsEnabled = ShowTextStyleSection;
				await module.InvokeVoidAsync("configure", _editorRef,
					ShowTextStyleSection, ShowTextStyleSection, ShowTextStyleSection);
			}

			if (_needsHtmlSync)
			{
				//Convert the Markdown value to HTML (shared converter) and load it into the contenteditable.
				var html = MarkdownConverter.ToHtml(Value);
				await module.InvokeVoidAsync("setHtml", _editorRef, html);
				_domMarkdown = Value;
				_needsHtmlSync = false;
			}
		}

		private async Task<IJSObjectReference?> GetModuleAsync()
		{
			try
			{
#if DEBUG
				_jsModule ??= await JsRuntime.InvokeAsync<IJSObjectReference>("import",
					"./_content/Majorsoft.Blazor.Components.Inputs/richTextEditor.js");
#else
				_jsModule ??= await JsRuntime.InvokeAsync<IJSObjectReference>("import",
					"./_content/Majorsoft.Blazor.Components.Inputs/richTextEditor.min.js");
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
	}
}
