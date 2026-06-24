using System;
using System.Runtime.CompilerServices;

using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("Majorsoft.Blazor.Components.DragAndDrop.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Majorsoft.Blazor.Components.DragAndDrop
{
	/// <summary>
	/// Extension methods to register required Drag and Drop services into <see cref="IServiceCollection"/>.
	/// </summary>
	public static class DragAndDropExtensions
	{
		/// <summary>
		/// Registers the required Drag and Drop services into the <see cref="IServiceCollection"/>.
		/// Call this once during application startup to enable strongly-typed payload transfer between
		/// <c>Draggable</c> and <c>DropZone</c> components.
		/// </summary>
		/// <param name="services"><see cref="IServiceCollection"/> instance.</param>
		/// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
		public static IServiceCollection AddDragAndDrop(this IServiceCollection services)
		{
			if (services is null)
			{
				throw new ArgumentNullException(nameof(services));
			}

			services.AddScoped<IDragDropStateService, DragDropStateService>();

			return services;
		}
	}
}
