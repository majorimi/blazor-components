namespace Blazor.Components.Timer
{
	public record Times
	{
		public ulong Count { get; init; }

		private Times() { }

		public static Times Once() => new Times { Count = 1 };
		public static Times Infinite() => new Times { Count = ulong.MaxValue };
		public static Times Exactly(ulong count) => new Times { Count = count };
	}
}