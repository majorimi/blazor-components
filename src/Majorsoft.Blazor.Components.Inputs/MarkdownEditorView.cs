namespace Majorsoft.Blazor.Components.Inputs
{
	/// <summary>
	/// Determines which pane(s) the <see cref="MarkdownEditor"/> shows.
	/// </summary>
	public enum MarkdownEditorView
	{
		/// <summary>
		/// Only the Markdown source editor (textarea) is visible.
		/// </summary>
		Edit,

		/// <summary>
		/// Only the rendered HTML preview is visible.
		/// </summary>
		Preview,

		/// <summary>
		/// The source editor and the rendered preview are shown side by side.
		/// </summary>
		Split,
	}
}
