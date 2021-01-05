using System;
using Microsoft.Extensions.DependencyInjection;

using Majorsoft.Blazor.Components.CssEvents.Transition;
using Majorsoft.Blazor.Components.CssEvents.Animation;

namespace Majorsoft.Blazor.Components.CssEvents
{
	/// <summary>
	/// Extension methods to register <see cref="ITransitionEventsService"/> and <see cref="IAnimationEventsService"/> into IServiceCollection
	/// </summary>
	public static class CssEventsExtension
	{
		/// <summary>
		/// Registers <see cref="ITransitionEventsService"/> and <see cref="IAnimationEventsService"/> as Transient
		/// to IServiceCollection in order to be able to inject CSS events...
		/// </summary>
		/// <param name="services">IServiceCollection instance</param>
		public static IServiceCollection AddCssEvents(this IServiceCollection services)
		{
			if (services == null)
			{
				throw new ArgumentNullException(nameof(services));
			}

			services.AddTransient<IAnimationEventsService, AnimationEventsService>();
			services.AddTransient<ITransitionEventsService, TransitionEventsService>();

			return services;
		}
	}
}