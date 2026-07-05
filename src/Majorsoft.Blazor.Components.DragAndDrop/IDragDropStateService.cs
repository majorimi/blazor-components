using System;

namespace Majorsoft.Blazor.Components.DragAndDrop
{
	/// <summary>
	/// Scoped service that carries the strongly-typed payload of the current drag operation between
	/// <c>Draggable</c> and <c>DropZone</c> components.
	/// <para>
	/// The native HTML <c>DataTransfer</c> object can only carry strings and cannot be written from managed Blazor code,
	/// so this service is used to transfer arbitrary .NET objects within the same Blazor application.
	/// </para>
	/// </summary>
	public interface IDragDropStateService
	{
		/// <summary>
		/// The object currently being dragged, or <c>null</c> when no drag operation is in progress.
		/// </summary>
		object? ActiveItem { get; }

		/// <summary>
		/// Returns true when a drag operation is in progress (an active item is set).
		/// </summary>
		bool HasActiveItem { get; }

		/// <summary>
		/// Raised whenever the active item changes (set or cleared).
		/// </summary>
		event Action? ActiveItemChanged;

		/// <summary>
		/// Sets the object being dragged. Called by a <c>Draggable</c> on drag start.
		/// </summary>
		/// <param name="item">The dragged payload.</param>
		void SetActiveItem(object? item);

		/// <summary>
		/// Returns the active item cast to <typeparamref name="TItem"/>, or <c>default</c> when there is no
		/// active item or it is not assignable to <typeparamref name="TItem"/>.
		/// </summary>
		/// <typeparam name="TItem">Expected payload type.</typeparam>
		TItem? GetActiveItem<TItem>();

		/// <summary>
		/// Clears the active item. Called when a drag operation ends or an item is dropped.
		/// </summary>
		void Clear();
	}

	/// <inheritdoc cref="IDragDropStateService"/>
	internal sealed class DragDropStateService : IDragDropStateService
	{
		public object? ActiveItem { get; private set; }

		public bool HasActiveItem => ActiveItem is not null;

		public event Action? ActiveItemChanged;

		public void SetActiveItem(object? item)
		{
			ActiveItem = item;
			ActiveItemChanged?.Invoke();
		}

		public TItem? GetActiveItem<TItem>()
		{
			return ActiveItem is TItem typed ? typed : default;
		}

		public void Clear()
		{
			if (ActiveItem is null)
			{
				return;
			}

			ActiveItem = null;
			ActiveItemChanged?.Invoke();
		}
	}
}
