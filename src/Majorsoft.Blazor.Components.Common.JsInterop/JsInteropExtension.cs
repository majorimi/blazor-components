using System;

using Majorsoft.Blazor.Components.Common.JsInterop.BrowserColorTheme;
using Majorsoft.Blazor.Components.Common.JsInterop.BrowserDate;
using Majorsoft.Blazor.Components.Common.JsInterop.Click;
using Majorsoft.Blazor.Components.Common.JsInterop.Clipboard;
using Majorsoft.Blazor.Components.Common.JsInterop.Focus;
using Majorsoft.Blazor.Components.Common.JsInterop.Geo;
using Majorsoft.Blazor.Components.Common.JsInterop.GlobalMouseEvents;
using Majorsoft.Blazor.Components.Common.JsInterop.Head;
using Majorsoft.Blazor.Components.Common.JsInterop.Language;
using Majorsoft.Blazor.Components.Common.JsInterop.Navigation;
using Majorsoft.Blazor.Components.Common.JsInterop.Resize;
using Majorsoft.Blazor.Components.Common.JsInterop.Scroll;

using Microsoft.Extensions.DependencyInjection;

namespace Majorsoft.Blazor.Components.Common.JsInterop
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
		/// <returns>IServiceCollection</returns>
		public static IServiceCollection AddJsInteropExtensions(this IServiceCollection services)
		{
			if (services == null)
			{
				throw new ArgumentNullException(nameof(services));
			}

			services.AddTransient<IClickBoundariesHandler, ClickBoundariesHandler>();
			services.AddTransient<IFocusHandler, FocusHandler>();
			services.AddTransient<IScrollHandler, ScrollHandler>();
			services.AddTransient<IClipboardHandler, ClipboardHandler>();
			services.AddTransient<IGlobalMouseEventHandler, GlobalMouseEventHandler>();
			services.AddTransient<IResizeHandler, ResizeHandler>();
			services.AddTransient<ILanguageService, LanguageService>();
			services.AddTransient<IGeolocationService, GeolocationService>();
			services.AddTransient<IHtmlHeadService, HtmlHeadService>();
			services.AddTransient<IBrowserDateService, BrowserDateService>();
			services.AddTransient<IBrowserThemeService, BrowserThemeService>();
			services.AddTransient<INavigationHistoryService, NavigationHistoryService>();

			return services;
		}
	}
}