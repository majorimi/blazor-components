namespace Majorsoft.Blazor.Components.Inputs
{
	/// <summary>
	/// Supported edit modes of the <see cref="DateTimePicker"/> component.
	/// </summary>
	public enum DateTimePickerMode
	{
		/// <summary>
		/// Only a date can be picked, the time part of the value is always midnight (00:00:00).
		/// </summary>
		Date,

		/// <summary>
		/// Date and time can be picked, the calendar popup also renders time (hour/minute and optionally second) editors.
		/// </summary>
		DateTime
	}
}
