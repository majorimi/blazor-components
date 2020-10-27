using System;

using Blazor.Components.Common.JsInterop.Click;
using Blazor.Components.Common.JsInterop.Focus;
using Blazor.Components.Common.JsInterop.Scroll;

using Microsoft.Extensions.DependencyInjection;

namespace Blazor.Components.Common.JsInterop
{
	/// <summary>
	/// Extension methods to register required JS Interop services into IServiceCollection
	/// </summary>
	public static class JsInteropExtension
	{
		/// <summary>
		/// Registers required JS Interop services into IServiceCollection
		/// </summary>
		/// <param name="services">IServiceCollection instance</param>
		public static IServiceCollection AddJsInteropExtensions(this IServiceCollection services)
		{
			if (services == null)
			{
				throw new ArgumentNullException(nameof(services));
			}

			services.AddTransient<IClickBoundariesHandler, ClickBoundariesHandler>();
			services.AddTransient<IFocusHandler, FocusHandler>();
			services.AddTransient<IScrollHandler, ScrollHandler>();
			
			return services;
		}
	}
}