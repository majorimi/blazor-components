namespace Majorsoft.Blazor.Components.DragAndDrop
{
	/// <summary>
	/// Mirrors the HTML Drag and Drop API <c>DataTransfer.effectAllowed</c> values.
	/// Specifies the kind of operations that are allowed for a drag source.
	/// <para>
	/// These values are <b>advisory</b>: the browser uses them for cursor feedback and to negotiate/permit a drop, but it
	/// never performs the copy/move/link operation itself. The integrator must implement the actual data change in the
	/// drop handler. See <see cref="DropEffect"/> for the target-side counterpart.
	/// </para>
	/// </summary>
	public enum DragDropEffects
	{
		/// <summary>
		/// No operation is permitted (<c>none</c>).
		/// </summary>
		None,
		/// <summary>
		/// A copy of the source item is made (<c>copy</c>).
		/// </summary>
		Copy,
		/// <summary>
		/// A copy or link operation is permitted (<c>copyLink</c>).
		/// </summary>
		CopyLink,
		/// <summary>
		/// A copy or move operation is permitted (<c>copyMove</c>).
		/// </summary>
		CopyMove,
		/// <summary>
		/// A link to the source is established (<c>link</c>).
		/// </summary>
		Link,
		/// <summary>
		/// A link or move operation is permitted (<c>linkMove</c>).
		/// </summary>
		LinkMove,
		/// <summary>
		/// The source item is moved (<c>move</c>).
		/// </summary>
		Move,
		/// <summary>
		/// Every operation (copy, move and link) is permitted (<c>all</c>). This is the default.
		/// </summary>
		All
	}

	/// <summary>
	/// Mirrors the HTML Drag and Drop API <c>DataTransfer.dropEffect</c> values.
	/// Controls the feedback (cursor) shown by the browser over a drop target.
	/// <para>
	/// These values are <b>advisory</b>: they set the cursor and take part in drop negotiation with the source's
	/// <see cref="DragDropEffects"/>, but the browser does not actually copy, move or link anything. The integrator is
	/// responsible for performing the operation (e.g. relocating the item for <see cref="Move"/>, duplicating it for
	/// <see cref="Copy"/>, creating a reference for <see cref="Link"/>) inside the drop handler.
	/// </para>
	/// </summary>
	public enum DropEffect
	{
		/// <summary>
		/// The item may not be dropped here (<c>none</c>).
		/// </summary>
		None,
		/// <summary>
		/// A copy of the source item is made (<c>copy</c>).
		/// </summary>
		Copy,
		/// <summary>
		/// The source item is moved (<c>move</c>). This is the default.
		/// </summary>
		Move,
		/// <summary>
		/// A link to the source is established (<c>link</c>).
		/// </summary>
		Link
	}

	internal static class DragDropEffectsExtensions
	{
		/// <summary>
		/// Converts a <see cref="DragDropEffects"/> value to its HTML <c>effectAllowed</c> string representation.
		/// </summary>
		public static string ToJsValue(this DragDropEffects effect) => effect switch
		{
			DragDropEffects.None => "none",
			DragDropEffects.Copy => "copy",
			DragDropEffects.CopyLink => "copyLink",
			DragDropEffects.CopyMove => "copyMove",
			DragDropEffects.Link => "link",
			DragDropEffects.LinkMove => "linkMove",
			DragDropEffects.Move => "move",
			_ => "all",
		};

		/// <summary>
		/// Converts a <see cref="DropEffect"/> value to its HTML <c>dropEffect</c> string representation.
		/// </summary>
		public static string ToJsValue(this DropEffect effect) => effect switch
		{
			DropEffect.None => "none",
			DropEffect.Copy => "copy",
			DropEffect.Link => "link",
			_ => "move",
		};
	}
}
