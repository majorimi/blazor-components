Blazor Components Inputs controls
============
[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-build-check)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=6)
[![Package Version](https://img.shields.io/nuget/v/Majorsoft.Blazor.Components.Inputs?label=Latest%20Version)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Inputs/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Majorsoft.Blazor.Components.Inputs?label=Downloads)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Inputs/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/majorimi/blazor-components/blob/master/LICENSE)

# About

Blazor components that renders an HTML `<input>`, `<textarea>` elements also extending `InputText` and `InputTextArea` Blazor provided components with `maxlength` set and counter to show remaining characters.
The package also ships a **`MarkdownEditor`** and a Word-like **`RichTextEditor`** that share a customizable sectioned toolbar and a Markdown engine.
**All components work with WebAssembly and Server hosted models**. 
For code examples [see usage](https://github.com/majorimi/blazor-components/blob/master/src/Majorsoft.Blazor.Components.TestApps.Common/Components/MaxLengthInputPage.razor).

You can try it out by using the [demo app](https://blazorextensions.z6.web.core.windows.net/maxLengthInput).

![Inputs demo](https://raw.githubusercontent.com/majorimi/blazor-components-docs/main/github/docs/gifs/maxLengthInput.gif)

# Components

- **`MaxLengthInput`**: wraps and renders HTML `<input>` field and sets `maxlength` property with notification onChange.
- **`MaxLengthInputText`**: extends `InputText` Blazor provided component (it supports form validation and `@bind-Value=`) and sets `maxlength` property with notification onChange.
- **`MaxLengthTextarea`**: wraps and renders HTML `<textarea>` field and sets `maxlength` property with notification onChange.
- **`MaxLengthInputTextArea`**: extends `InputTextArea` Blazor provided component (it supports form validation and `@bind-Value=`) and sets `maxlength` property with notification onChange.
- **`MarkdownEditor`**: Markdown source editor with a customizable sectioned toolbar and a live HTML preview (Edit / Split / Preview views).
- **`RichTextEditor`**: Word-like WYSIWYG editor that applies formatting inline on a `contenteditable` surface while keeping the bound value as Markdown, so it round-trips cleanly with `MarkdownEditor`.
- **`PasswordInput`**: secret text input with a customizable mask character (dot, star, bullet, etc.), a customizable layout and a reveal (eye) button that shows or hides the entered value.

## `MaxLengthInput` and `MaxLengthInputText` components

Blazor components that are wrapping around standard  HTML `<input>`, `<textarea>` elements and sets `maxlength` property with notification onChange.
Can fit for any Blazor App e.g. when DB field has limited length and on UI same limited chars should be set. And on each chars entered the remaining allowed chars counter will be shown.

## `MaxLengthTextarea` and `MaxLengthInputTextArea` components

Blazor components that are wrapping around standard exiting Blazor components. Rendered HTML result will be standard `<input>`, `<textarea>` as well.
Can fit for any Blazor App but use this only when you need Blazor provided FROM validation as well.

### Properties

All components have exactly the same features only rendering different HTML elements, hence 
they have the same **properties**, **events** and **functions** as well.

- **`Value`: `string? { get; set; }`** <br />
  Value of the rendered HTML element. Initial field value can be set to given string or omitted (leave empty). 
  Also control actual value can be read out.
  <br />**\* Note**: in case of `DebounceInputText` and `DebounceInputTextArea` this property is inherited from Blazor
  component and requires slightly different code with `@bind`, see usage example differences.
- **`MaxAllowedChars`: `int { get; set; }` (default: 50)** <br />
  Maximum allowed characters to type in.
- **`CountdownText`: `string { get; set; }` (default: "Remaining characters: ")** <br />
 Countdown label text to change or localize message.
- **`ShowRemainingChars`: `bool { get; set; }` (default: true)** <br />
Should show remaining character values at the end of the `CountdownText` or not.
**Note**: RemainingCharacters value can be acquired from `OnRemainingCharsChanged` event parameter.
- **`CountdownTextClass`: `string { get; set; }` (default: "")** <br />
  Countdown label and value CSS class property to style message.
- **`InnerElementReference`: `ElementReference { get; }`** <br />
  Exposes a Blazor `ElementReference` of the wrapped around HTML element. It can be used e.g. for JS interop, etc.

**Arbitrary HTML attributes e.g.: `id="input1" class="form-control w-25"` will be passed to the corresponding rendered HTML element `<input>` or `<textarea>`**.

```
<MaxLengthInput 
	id="in1"
	class="form-control w-25" 
	placeholder="@($"You can type maximum: {_maxLengthInputTextAreaAllowed} char(s)")"
	... />
```

**Will be rendered to:**
```
<input id="in1" class="form-control w-25" placeholder="You can type maximum: 20 char(s)" ... />
```

### Events
- **`OnValueChanged`: `EventCallback<string>` delegate** <br />
  Callback function called when value was changed.
- **`OnRemainingCharsChanged`: `EventCallback<int>` delegate** <br />
  Callback function called when HTML control received keyboard inputs remaining allowed chars calculated and sent as even args.
  Can be used to style or change the Countdown message.

## `MarkdownEditor` and `RichTextEditor` components

![MarkdownEditor demo](https://raw.githubusercontent.com/majorimi/blazor-components-docs/main/github/docs/gifs/markdownEditor.gif)

![RichTextEditor demo](https://raw.githubusercontent.com/majorimi/blazor-components-docs/main/github/docs/gifs/richTextEditor.gif)

Two editors that share one sectioned, Tooltip-powered toolbar and a dependency-free Markdown engine, so
content round-trips cleanly between them. The **`MarkdownEditor`** edits Markdown *source* with a live
preview; the **`RichTextEditor`** is a Word-like *WYSIWYG* surface whose bound `Value` is always Markdown.

The toolbar is split into independently toggleable sections: **Headings**, **Text style** (bold, italic,
underline, strikethrough), **Lists** (bullet, numbered, task), **Indentation** (decrease / increase),
**Blocks** (quote, inline code, code block, horizontal rule) and **Insert** (link, image, table). In the
`RichTextEditor` the Insert buttons open a small `Popover` (anchored under the icon) to enter the link
text/URL, image URL or table dimensions; pressing **Enter** accepts and **Esc** closes it.

> **\* Note**: the editors render the `Popover` from the Tooltips package, which relies on the JS interop
> services. Register them once in your host app(s): `services.AddJsInteropExtensions();`

### Styles

The editor CSS ships in the package; link it once (e.g. in `index.html` / `App.razor` / `_Host.cshtml`):

```
<link rel="stylesheet" href="_content/Majorsoft.Blazor.Components.Inputs/editor.css" />
```

### Shared properties

Both editors expose the same set of common properties:

- **`Value`: `string? { get; set; }`** — the Markdown text. Supports two-way binding via `@bind-Value`.
- **`Placeholder`: `string { get; set; }`** — hint shown when the editor is empty.
- **`Disabled`: `bool { get; set; }` (default: false)** — disables input and all toolbar actions.
- **`ReadOnly`: `bool { get; set; }` (default: false)** — content is read only (toolbar actions disabled too).
- **`SpellCheck`: `bool { get; set; }` (default: true)** — toggles the browser native spellchecker.
- **`ShowToolbar`: `bool { get; set; }` (default: true)** — shows/hides the whole toolbar.
- **`ShowHeadingSection` / `ShowTextStyleSection` / `ShowListSection` / `ShowIndentSection` / `ShowBlockSection` / `ShowInsertSection`: `bool { get; set; }` (default: true)** — show/hide individual toolbar sections.
- **`ShowFooter`: `bool { get; set; }` (default: true)** — shows/hides the footer (hint + counter).
- **`ShowButtonTooltips`: `bool { get; set; }` (default: true)** — each button shows a Tooltip; otherwise a native `title`.
- **`CustomToolbarContent`: `RenderFragment? { get; set; }`** — extra toolbar content rendered as an additional section.
- **`FooterText`: `string { get; set; }`** — text shown on the left side of the footer.
- **`Class` / `Style` / `ToolbarClass` / `EditorClass`: `string? { get; set; }`** — custom CSS class(es) / inline style for the root, toolbar and editing surface.
- **`InnerElementReference`: `ElementReference { get; }`** — the wrapped editing element (the `<textarea>` for Markdown, the `contenteditable` for RichText).

**Shared events and functions:**

- **`ValueChanged`: `EventCallback<string?>`** — enables `@bind-Value`.
- **`OnInput`: `EventCallback<string?>`** — invoked on every edit with the new Markdown value.
- **`OnHtmlChanged`: `EventCallback<string>`** — invoked with the rendered HTML whenever the value changes.
- **`GetHtml()`: `string`** — returns the rendered HTML of the current value.
- **`FocusAsync()`: `Task`** — sets focus to the editing surface.

### `MarkdownEditor` specific properties

- **`View`: `MarkdownEditorView { get; set; }` (default: `Edit`)** — which pane(s) are visible: `Edit`, `Split` or `Preview`. Supports two-way binding via `@bind-View` (`ViewChanged`).
- **`ShowViewSection`: `bool { get; set; }` (default: true)** — shows/hides the Edit / Split / Preview switch.
- **`VisibleRows`: `int { get; set; }` (default: 8)** — number of visible text rows of the source `<textarea>` (its `rows` attribute).
- **`MaxAllowedChars`: `int { get; set; }` (default: 0)** — when greater than 0 a `maxlength` is applied and a remaining-characters counter is shown.
- **`TabIndents`: `bool { get; set; }` (default: true)** and **`TabIndent`: `string { get; set; }` (default: two spaces)** — pressing Tab inserts `TabIndent` instead of moving focus.
- **`PreviewClass`: `string? { get; set; }`** — custom CSS class(es) for the rendered preview pane.

### `RichTextEditor` specific properties

- **`Height`: `int { get; set; }` (default: 0)** — fixed editor height in pixels (toolbar + content + footer). When greater than 0 the content area scrolls once it overflows; when **0** the height is **auto** and grows with the content.
- **`Width`: `int { get; set; }` (default: 0)** — fixed editor width in pixels. When **0** the width is **auto** (it fills its container).

### Usage

```
@using Majorsoft.Blazor.Components.Inputs

@* Link the editor stylesheet once in your app, e.g. in index.html / App.razor. *@
<link rel="stylesheet" href="_content/Majorsoft.Blazor.Components.Inputs/editor.css" />

<MarkdownEditor @bind-Value="_markdown"
                View="MarkdownEditorView.Split"
                VisibleRows="14"
                MaxAllowedChars="2000"
                Placeholder="Start typing Markdown..." />

<RichTextEditor @bind-Value="_markdown"
                Height="300"
                Placeholder="Start writing..." />

@code {
    private string _markdown = "# Hello\n\nThis is **shared** Markdown.";
}
```

Both editors are bound to the same `_markdown` value above, demonstrating that the WYSIWYG and Markdown
representations share one format.

## `PasswordInput` component

![PasswordInput demo](https://raw.githubusercontent.com/majorimi/blazor-components-docs/main/github/docs/gifs/passwordInput.gif)

Blazor component that renders a secret text `<input>` with a **customizable mask character**, a **customizable layout**
and a reveal (**eye**) button to show or hide the entered value. The real value is always kept in C# and exposed via
`@bind-Value` (reading it always returns the plain text); while masked the input shows a run of the chosen mask
character. Editing in the middle of the value, text selection, paste and delete are all supported because the real
value is reconstructed from the caret position after each input.

### Styles

`PasswordInput` ships a stylesheet in the package. Link it **once** in your app (e.g. `index.html` / `App.razor`):

```
<link rel="stylesheet" href="_content/Majorsoft.Blazor.Components.Inputs/passwordInput.css" />
```

All class names are prefixed with `bpwd` and every part is overridable via the `Class`, `InputClass` and
`RevealButtonClass` parameters.

### Properties

- **`Value`: `string? { get; set; }`** <br />
  The secret value. Supports two-way binding via `@bind-Value`; reading it always returns the plain text.
- **`MaskCharacter`: `char { get; set; }` (default: `'●'`)** <br />
  Character used to mask the value while hidden, e.g. `'●'`, `'*'` or `'•'`. Set to `'\0'` to disable masking.
- **`ShowPassword`: `bool { get; set; }` (default: false)** <br />
  Whether the value is currently revealed (plain text) or masked. Supports two-way binding via `@bind-ShowPassword`.
- **`ShowRevealButton`: `bool { get; set; }` (default: true)** <br />
  Whether the reveal (eye) button is rendered.
- **`ShowTitle` / `HideTitle`: `string { get; set; }`** <br />
  Tooltip/aria-label shown on the reveal button while masked / revealed.
- **`Placeholder`: `string? { get; set; }`** <br />
  Placeholder text shown when the input is empty.
- **`MaxLength`: `int { get; set; }` (default: 0)** <br />
  Maximum allowed characters. `0` means unlimited.
- **`Disabled` / `ReadOnly`: `bool { get; set; }`** <br />
  Standard disabled / read only states.
- **`AutoComplete`: `string { get; set; }` (default: "new-password")** <br />
  Value of the rendered `autocomplete` attribute.
- **`Class` / `Style` / `InputClass` / `RevealButtonClass`: `string? { get; set; }`** <br />
  CSS hooks to customize the wrapper, the `<input>` and the reveal button.
- **`ShowIconContent` / `HideIconContent`: `RenderFragment?`** <br />
  Optional custom content for the reveal button replacing the default eye icons (masked / revealed state).
- **`AdditionalAttributes`: `Dictionary<string, object>?`** <br />
  Arbitrary HTML attributes splatted onto the inner `<input>`.

### Events

- **`ValueChanged`: `EventCallback<string?>`** — enables `@bind-Value`.
- **`OnInput`: `EventCallback<string?>`** — invoked on every input with the new plain value.
- **`ShowPasswordChanged`: `EventCallback<bool>`** — enables `@bind-ShowPassword`.

### Functions

- **`FocusAsync()`** — sets focus to the inner `<input>`.
- **`TogglePasswordVisibilityAsync()`** — toggles between the masked and revealed states.

### Usage

```
@using Majorsoft.Blazor.Components.Inputs

@* Link the stylesheet once in your app, e.g. in index.html / App.razor. *@
<link rel="stylesheet" href="_content/Majorsoft.Blazor.Components.Inputs/passwordInput.css" />

<PasswordInput @bind-Value="_password"
               MaskCharacter="*"
               MaxLength="20"
               Placeholder="Enter your password" />

@code {
    private string? _password;
}
```

# Configuration

## Installation

**Majorsoft.Blazor.Components.Inputs** is available on [NuGet](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Inputs/). 

```sh
dotnet add package Majorsoft.Blazor.Components.Inputs
```
Use the `--version` option to specify a [preview version](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Inputs/absoluteLatest) to install.

## Usage

Add using statement to your Blazor <component/page>.razor file. Or globally reference it into `_Imports.razor` file.
```
@using Majorsoft.Blazor.Components.Inputs
```

### `MaxLengthInput` and `MaxLengthTextarea` usage

Following code example shows how to use **`MaxLengthInput`** component in your Blazor App with model binding
on specific `OnInput` event. So it will enable two way binding between components. You can omit it and use `Value` directly for one way binding.

**Note**: using **`MaxLengthTextarea`** component basically the same but it will render HTML `<textarea>`.

```
<style>
	.countDownText {
		font-size: small;
		color: gray;
	}
	.countDownText.red {
		color: red;
	}
	.countDownText.right {
		width: 100%;
		text-align: right;
	}
</style>

<div class="row pb-2">
	<div class="col-12 col-lg-8 col-xl-5">
		<MaxLengthInput id="maxInput1" class="form-control w-100" placeholder="@($"You can type maximum: {_maxLengthInputAllowed} char(s)")"
						@bind-Value="@_maxLengthInputValue" @bind-Value:event="OnInput"
						MaxAllowedChars="@_maxLengthInputAllowed"
						CountdownTextClass="@_maxLengthInputTextClass"
						OnRemainingCharsChanged="MaxLengthInputRemainingCharsChanged" />
	</div>
</div>

<div>Actual value: @_maxLengthInputValue</div>

@code {
	//MaxLengthInput
	private int _maxLengthInputAllowed = 10;
	private string _maxLengthInputValue = "";
	private string _maxLengthInputTextClass = "countDownText";

	private void MaxLengthInputRemainingCharsChanged(int remainingChars)
	{
		_maxLengthInputTextClass = "countDownText";

		if (_maxLengthInputAllowed * 0.2 >= remainingChars)
		{
			_maxLengthInputTextClass = "countDownText red";
		}
	}
}	
```

### `MaxLengthInputText` and `MaxLengthInputTextArea` usage

Following code example shows how to use **`DebounceInputText`** component with model binding and form validation in your Blazor App.

**Note**: using **`MaxLengthInputTextArea`** component basically the same but it will render HTML `<textarea>`.

```
<style>
	.countDownText {
		font-size: small;
		color: gray;
	}
	.countDownText.red {
		color: red;
	}
	.countDownText.right {
		width: 100%;
		text-align: right;
	}
</style>

<EditForm Model="@exampleModel">
	<DataAnnotationsValidator />
	<ValidationSummary />
	<div class="row pb-2">
		<div class="col-12 col-lg-8 col-xl-5">
			<MaxLengthInputText id="maxInput2" class="form-control w-100" placeholder="@($"You can type maximum: {_maxLengthInputTextAllowed} char(s)")"
								@bind-Value="exampleModel.Name"
								MaxAllowedChars="@_maxLengthInputTextAllowed"
								CountdownTextClass="@_maxLengthInputTextTextClass"
								OnRemainingCharsChanged="MaxLengthInputTextRemainingCharsChanged" />
		</div>
	</div>
	<div class="pb-2">
		<button class="btn btn-primary" type="submit">Submit</button>
	</div>
</EditForm>
		
<div>Actual value: @(exampleModel.Name)</div>

@code {
	//MaxLengthInputText
	private int _maxLengthInputTextAllowed = 10;
	private string _maxLengthInputTextTextClass = "countDownText";

	private void MaxLengthInputTextRemainingCharsChanged(int remainingChars)
	{
		_maxLengthInputTextTextClass = "countDownText";

		if (_maxLengthInputTextAllowed * 0.2 >= remainingChars)
		{
			_maxLengthInputTextTextClass = "countDownText red";
		}
	}

	//Form model
	private ExampleModel exampleModel = new ExampleModel() { Name = "" };
	public class ExampleModel
	{
		[Required]
		[StringLength(10, ErrorMessage = "Name is too long.")]
		public string Name { get; set; }
	}
}
```