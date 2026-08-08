using System;

namespace Majorsoft.Blazor.Components.Grid
{
	/// <summary>
	/// Sort directions of a <see cref="DataGridColumn{TItem}"/> in <see cref="DataGrid{TItem}"/> component.
	/// </summary>
	public enum SortDirections
	{
		/// <summary>
		/// Column is not sorted.
		/// </summary>
		None,
		/// <summary>
		/// Column is sorted ascending.
		/// </summary>
		Ascending,
		/// <summary>
		/// Column is sorted descending.
		/// </summary>
		Descending
	}

	/// <summary>
	/// Text alignments of a <see cref="DataGridColumn{TItem}"/> cell content.
	/// </summary>
	public enum TextAligns
	{
		/// <summary>
		/// Align cell content to left.
		/// </summary>
		Left,
		/// <summary>
		/// Align cell content to center.
		/// </summary>
		Center,
		/// <summary>
		/// Align cell content to right.
		/// </summary>
		Right
	}

	/// <summary>
	/// Event arguments for <see cref="DataGrid{TItem}"/> sorting changed event.
	/// </summary>
	public class DataGridSortEventArgs
	{
		/// <summary>
		/// Data Field name of the sorted column.
		/// </summary>
		public string? Field { get; }

		/// <summary>
		/// Applied sort direction.
		/// </summary>
		public SortDirections SortDirection { get; }

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="field">Data Field name of the sorted column</param>
		/// <param name="sortDirection">Applied sort direction</param>
		public DataGridSortEventArgs(string? field, SortDirections sortDirection)
		{
			Field = field;
			SortDirection = sortDirection;
		}
	}
}
