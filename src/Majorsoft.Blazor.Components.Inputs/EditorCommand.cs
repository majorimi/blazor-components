namespace Majorsoft.Blazor.Components.Inputs
{
	/// <summary>
	/// Formatting commands raised by the shared <see cref="EditorToolbar"/> and interpreted by each editor
	/// (<see cref="MarkdownEditor"/> applies them to Markdown source, <see cref="RichTextEditor"/> applies
	/// them to the live <c>contenteditable</c> surface).
	/// </summary>
	public enum EditorCommand
	{
		/// <summary>Heading level 1.</summary>
		Heading1,
		/// <summary>Heading level 2.</summary>
		Heading2,
		/// <summary>Heading level 3.</summary>
		Heading3,
		/// <summary>Bold text.</summary>
		Bold,
		/// <summary>Italic text.</summary>
		Italic,
		/// <summary>Underlined text.</summary>
		Underline,
		/// <summary>Strikethrough text.</summary>
		Strikethrough,
		/// <summary>Unordered (bullet) list.</summary>
		BulletList,
		/// <summary>Ordered (numbered) list.</summary>
		NumberedList,
		/// <summary>Task (checkbox) list.</summary>
		TaskList,
		/// <summary>Decrease indentation (outdent) of the selected line(s).</summary>
		IndentDecrease,
		/// <summary>Increase indentation (indent) of the selected line(s).</summary>
		IndentIncrease,
		/// <summary>Block quote.</summary>
		Quote,
		/// <summary>Inline code span.</summary>
		InlineCode,
		/// <summary>Fenced code block.</summary>
		CodeBlock,
		/// <summary>Horizontal rule.</summary>
		HorizontalRule,
		/// <summary>Hyperlink.</summary>
		Link,
		/// <summary>Image.</summary>
		Image,
		/// <summary>Table.</summary>
		Table,
	}
}
