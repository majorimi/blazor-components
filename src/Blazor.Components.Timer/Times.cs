namespace Blazor.Components.Timer
{
	/// <summary>
	/// Times to occur of <see cref="AdvancedTimer"/> ticks
	/// </summary>
	public record Times
	{
		/// <summary>
		/// Occurrence count
		/// </summary>
		public ulong Count { get; init; }

		private Times() { }

		/// <summary>
		/// Should occur only once
		/// </summary>
		/// <returns></returns>
		public static Times Once() => new Times { Count = 1 };

		/// <summary>
		/// Should occur until stopped
		/// </summary>
		/// <returns></returns>
		public static Times Infinite() => new Times { Count = ulong.MaxValue };

		/// <summary>
		/// Should occur exactly to the given number of times
		/// </summary>
		/// <param name="count">N occurrence</param>
		/// <returns></returns>
		public static Times Exactly(ulong count) => new Times { Count = count };
	}
}