using System.Collections.Generic;

namespace Majorsoft.Blazor.Components.DragAndDrop
{
	/// <summary>
	/// Options passed to the <c>registerDraggable</c> JS interop function. Serialized to a plain JS object.
	/// </summary>
	internal sealed class DraggableJsOptions
	{
		public string EffectAllowed { get; set; } = "all";
		public IReadOnlyDictionary<string, string>? Data { get; set; }
		public string? DragImageElementId { get; set; }
		public int DragImageOffsetX { get; set; }
		public int DragImageOffsetY { get; set; }
	}

	/// <summary>
	/// Options passed to the <c>registerDropZone</c> JS interop function. Serialized to a plain JS object.
	/// </summary>
	internal sealed class DropZoneJsOptions
	{
		public string DropEffect { get; set; } = "move";
	}
}
