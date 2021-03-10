using Bunit;

using Majorsoft.Blazor.Components.Common.JsInterop.Geo;
using Majorsoft.Blazor.Components.Maps.Google;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majorsoft.Blazor.Components.Maps.Tests.Google
{
	[TestClass]
	public class GoogleStaticMapTest
	{
		private Bunit.TestContext _testContext;
		private Mock<IGeolocationService> _geoLocationMock;

		[TestInitialize]
		public void Init()
		{
			_testContext = new Bunit.TestContext();

			var logger = new Mock<ILogger<GoogleStaticMap>>();
			_geoLocationMock = new Mock<IGeolocationService>();

			_testContext.Services.Add(new ServiceDescriptor(typeof(ILogger<GoogleStaticMap>), logger.Object));
			_testContext.Services.Add(new ServiceDescriptor(typeof(IGeolocationService), _geoLocationMock.Object));
		}

		[TestCleanup]
		public void Cleanup()
		{
			_testContext?.Dispose();
		}

		[TestMethod]
		public void GoogleStaticMap_should_rendered_correctly_html_attributes()
		{
			var rendered = _testContext.RenderComponent<GoogleStaticMap>(
				("id", "id1"), //HTML attributes
				("class", "form-control w-100") //HTML attributes
				);

			var map = rendered.Find("img");

			Assert.IsNotNull(map);
			map.MarkupMatches(@"<img id=""id1"" class=""form-control w-100"" src=""https://maps.googleapis.com/maps/api/staticmap?center=&amp;zoom=12&amp;size=400x300&amp;scale=1&amp;maptype=roadmap&amp;format=gif&amp;key="" />");
		}

		[TestMethod]
		public void GoogleStaticMap_should_call_GetCurrentPosition_on_render()
		{
			_geoLocationMock.Setup(s => s.GetCurrentPositionAsync(It.IsAny<Func<GeolocationResult, Task>>(),
				It.IsAny<bool>(), It.IsAny<TimeSpan?>(), It.IsAny<TimeSpan?>()));

			var rendered = _testContext.RenderComponent<GoogleStaticMap>(parameters => parameters
				.Add(p => p.CenterCurrentLocationOnLoad, true));

			var map = rendered.Find("img");

			Assert.IsNotNull(map);
			map.MarkupMatches(@"<img src=""https://maps.googleapis.com/maps/api/staticmap?center=&amp;zoom=12&amp;size=400x300&amp;scale=1&amp;maptype=roadmap&amp;format=gif&amp;key="" />");

			_geoLocationMock.Verify(v => v.GetCurrentPositionAsync(It.IsAny<Func<GeolocationResult, Task>>(),
				It.IsAny<bool>(), It.IsAny<TimeSpan?>(), It.IsAny<TimeSpan?>()), Times.Once);
		}

		[TestMethod]
		public void GoogleStaticMap_should_not_call_GetCurrentPosition_on_render()
		{
			_geoLocationMock.Setup(s => s.GetCurrentPositionAsync(It.IsAny<Func<GeolocationResult, Task>>(),
				It.IsAny<bool>(), It.IsAny<TimeSpan?>(), It.IsAny<TimeSpan?>()));

			var rendered = _testContext.RenderComponent<GoogleStaticMap>(parameters => parameters
				.Add(p => p.CenterCurrentLocationOnLoad, false));

			var map = rendered.Find("img");

			Assert.IsNotNull(map);
			map.MarkupMatches(@"<img src=""https://maps.googleapis.com/maps/api/staticmap?center=&amp;zoom=12&amp;size=400x300&amp;scale=1&amp;maptype=roadmap&amp;format=gif&amp;key="" />");

			_geoLocationMock.Verify(v => v.GetCurrentPositionAsync(It.IsAny<Func<GeolocationResult, Task>>(),
				It.IsAny<bool>(), It.IsAny<TimeSpan?>(), It.IsAny<TimeSpan?>()), Times.Never);
		}

		[TestMethod]
		public void GoogleStaticMap_should_render_apiKey()
		{
			var rendered = _testContext.RenderComponent<GoogleStaticMap>(parameters => parameters
				.Add(p => p.ApiKey, "myApikey_here"));

			var map = rendered.Find("img");

			Assert.IsNotNull(map);
			map.MarkupMatches(@"<img src=""https://maps.googleapis.com/maps/api/staticmap?center=&amp;zoom=12&amp;size=400x300&amp;scale=1&amp;maptype=roadmap&amp;format=gif&amp;key=myApikey_here"" />");
		}

		[TestMethod]
		public void GoogleStaticMap_should_render_signature()
		{
			var rendered = _testContext.RenderComponent<GoogleStaticMap>(parameters => parameters
				.Add(p => p.ApiKey, "myApikey_here")
				.Add(p => p.Signature, "mySignature_here"));

			var map = rendered.Find("img");

			Assert.IsNotNull(map);
			map.MarkupMatches(@"<img src=""https://maps.googleapis.com/maps/api/staticmap?center=&amp;zoom=12&amp;size=400x300&amp;scale=1&amp;maptype=roadmap&amp;format=gif&amp;key=myApikey_here&amp;signature=mySignature_here"" />");
		}

		[TestMethod]
		public void GoogleStaticMap_should_render_center_location()
		{
			var rendered = _testContext.RenderComponent<GoogleStaticMap>(parameters => parameters
				.Add(p => p.Center, new GeolocationData(1.1, 2.2 )));

			var map = rendered.Find("img");

			Assert.IsNotNull(map);
			map.MarkupMatches(@"<img src=""https://maps.googleapis.com/maps/api/staticmap?center=1.1,2.2&amp;zoom=12&amp;size=400x300&amp;scale=1&amp;maptype=roadmap&amp;format=gif&amp;key="" />");
		}

		[TestMethod]
		public void GoogleStaticMap_should_render_zoom()
		{
			var rendered = _testContext.RenderComponent<GoogleStaticMap>(parameters => parameters
				.Add(p => p.Center, new GeolocationData(1.1, 2.2))
				.Add(p => p.ZoomLevel, (byte)0));

			var map = rendered.Find("img");

			Assert.IsNotNull(map);
			map.MarkupMatches(@"<img src=""https://maps.googleapis.com/maps/api/staticmap?center=1.1,2.2&amp;zoom=0&amp;size=400x300&amp;scale=1&amp;maptype=roadmap&amp;format=gif&amp;key="" />");
		}

		[TestMethod]
		public void GoogleStaticMap_should_render_size()
		{
			var rendered = _testContext.RenderComponent<GoogleStaticMap>(parameters => parameters
				.Add(p => p.Center, new GeolocationData(1.1, 2.2))
				.Add(p => p.Height, 1111)
				.Add(p => p.Width, 2222));

			var map = rendered.Find("img");

			Assert.IsNotNull(map);
			map.MarkupMatches(@"<img src=""https://maps.googleapis.com/maps/api/staticmap?center=1.1,2.2&amp;zoom=12&amp;size=2222x1111&amp;scale=1&amp;maptype=roadmap&amp;format=gif&amp;key="" />");
		}

		[TestMethod]
		public void GoogleStaticMap_should_render_scale()
		{
			var rendered = _testContext.RenderComponent<GoogleStaticMap>(parameters => parameters
				.Add(p => p.Center, new GeolocationData(1.1, 2.2))
				.Add(p => p.HighResolution, true));

			var map = rendered.Find("img");

			Assert.IsNotNull(map);
			map.MarkupMatches(@"<img src=""https://maps.googleapis.com/maps/api/staticmap?center=1.1,2.2&amp;zoom=12&amp;size=400x300&amp;scale=2&amp;maptype=roadmap&amp;format=gif&amp;key="" />");
		}

		[TestMethod]
		public void GoogleStaticMap_should_render_maptype()
		{
			var rendered = _testContext.RenderComponent<GoogleStaticMap>(parameters => parameters
				.Add(p => p.Center, new GeolocationData(1.1, 2.2)));

			foreach (var item in Enum.GetValues(typeof(GoogleMapTypes)))
			{
				rendered.SetParametersAndRender(parameters => parameters
					.Add(p => p.MapType, (GoogleMapTypes)item));

				var map = rendered.Find("img");

				Assert.IsNotNull(map);
				map.MarkupMatches(
					@$"<img src=""https://maps.googleapis.com/maps/api/staticmap?center=1.1,2.2&amp;zoom=12&amp;size=400x300&amp;scale=1&amp;maptype={item.ToString().ToLower()}&amp;format=gif&amp;key="" />");
			}
		}

		[TestMethod]
		public void GoogleStaticMap_should_render_format()
		{
			var rendered = _testContext.RenderComponent<GoogleStaticMap>(parameters => parameters
				.Add(p => p.Center, new GeolocationData(1.1, 2.2)));

			foreach (var item in Enum.GetValues(typeof(GoogleStaticMapImageFormats)))
			{
				rendered.SetParametersAndRender(parameters => parameters
					.Add(p => p.ImageFormat, (GoogleStaticMapImageFormats)item));

				var map = rendered.Find("img");

				Assert.IsNotNull(map);
				map.MarkupMatches(
					@$"<img src=""https://maps.googleapis.com/maps/api/staticmap?center=1.1,2.2&amp;zoom=12&amp;size=400x300&amp;scale=1&amp;maptype=roadmap&amp;format={item.ToString().ToLower()}&amp;key="" />");
			}
		}

		[TestMethod]
		public void GoogleStaticMap_should_render_language()
		{
			var rendered = _testContext.RenderComponent<GoogleStaticMap>(parameters => parameters
				.Add(p => p.Center, new GeolocationData(1.1, 2.2))
				.Add(p => p.Language, "en"));

			var map = rendered.Find("img");

			Assert.IsNotNull(map);
			map.MarkupMatches(@"<img src=""https://maps.googleapis.com/maps/api/staticmap?center=1.1,2.2&amp;zoom=12&amp;size=400x300&amp;scale=1&amp;maptype=roadmap&amp;format=gif&amp;language=en&amp;key="" />");
		}

		[TestMethod]
		public void GoogleStaticMap_should_render_region()
		{
			var rendered = _testContext.RenderComponent<GoogleStaticMap>(parameters => parameters
				.Add(p => p.Center, new GeolocationData(1.1, 2.2))
				.Add(p => p.Region, "en"));

			var map = rendered.Find("img");

			Assert.IsNotNull(map);
			map.MarkupMatches(@"<img src=""https://maps.googleapis.com/maps/api/staticmap?center=1.1,2.2&amp;zoom=12&amp;size=400x300&amp;scale=1&amp;maptype=roadmap&amp;format=gif&amp;region=en&amp;key="" />");
		}

		[TestMethod]
		public void GoogleStaticMap_should_render_style()
		{
			var rendered = _testContext.RenderComponent<GoogleStaticMap>(parameters => parameters
				.Add(p => p.Center, new GeolocationData(1.1, 2.2))
				.Add(p => p.Style, "customStyle"));

			var map = rendered.Find("img");

			Assert.IsNotNull(map);
			map.MarkupMatches(@"<img src=""https://maps.googleapis.com/maps/api/staticmap?center=1.1,2.2&amp;zoom=12&amp;size=400x300&amp;scale=1&amp;maptype=roadmap&amp;format=gif&amp;style=customStyle&amp;key="" />");
		}

		[TestMethod]
		public void GoogleStaticMap_should_handle_empty_path()
		{
			var rendered = _testContext.RenderComponent<GoogleStaticMap>(parameters => parameters
				.Add(p => p.Center, new GeolocationData(1.1, 2.2))
				.Add(p => p.Path, new List<GeolocationData>()));

			var map = rendered.Find("img");

			Assert.IsNotNull(map);
			map.MarkupMatches(@"<img src=""https://maps.googleapis.com/maps/api/staticmap?center=1.1,2.2&amp;zoom=12&amp;size=400x300&amp;scale=1&amp;maptype=roadmap&amp;format=gif&amp;key="" />");
		}

		[TestMethod]
		public void GoogleStaticMap_should_render_paths()
		{
			var rendered = _testContext.RenderComponent<GoogleStaticMap>(parameters => parameters
				.Add(p => p.Center, new GeolocationData(1.1, 2.2))
				.Add(p => p.Path, new List<GeolocationData>()
				{
					{ new GeolocationData(1.1, 2.2) },
					{ new GeolocationData("London") }
				}));

			var map = rendered.Find("img");

			Assert.IsNotNull(map);
			map.MarkupMatches(@"<img src=""https://maps.googleapis.com/maps/api/staticmap?size=400x300&amp;scale=1&amp;maptype=roadmap&amp;format=gif&amp;path=1.1,2.2|London&amp;key="" />");
		}

		[TestMethod]
		public void GoogleStaticMap_should_handle_empty_visible()
		{
			var rendered = _testContext.RenderComponent<GoogleStaticMap>(parameters => parameters
				.Add(p => p.Center, new GeolocationData(1.1, 2.2))
				.Add(p => p.VisibleLocations, new List<GeolocationData>()));

			var map = rendered.Find("img");

			Assert.IsNotNull(map);
			map.MarkupMatches(@"<img src=""https://maps.googleapis.com/maps/api/staticmap?center=1.1,2.2&amp;zoom=12&amp;size=400x300&amp;scale=1&amp;maptype=roadmap&amp;format=gif&amp;key="" />");
		}

		[TestMethod]
		public void GoogleStaticMap_should_render_visibles()
		{
			var rendered = _testContext.RenderComponent<GoogleStaticMap>(parameters => parameters
				.Add(p => p.Center, new GeolocationData(1.1, 2.2))
				.Add(p => p.VisibleLocations, new List<GeolocationData>()
				{
					{ new GeolocationData(1.1, 2.2) },
					{ new GeolocationData("London") }
				}));

			var map = rendered.Find("img");

			Assert.IsNotNull(map);
			map.MarkupMatches(@"<img src=""https://maps.googleapis.com/maps/api/staticmap?center=1.1,2.2&amp;size=400x300&amp;scale=1&amp;maptype=roadmap&amp;format=gif&amp;visible=1.1,2.2|London&amp;key="" />");
		}

		[TestMethod]
		public void GoogleStaticMap_should_handle_empty_markers()
		{
			var rendered = _testContext.RenderComponent<GoogleStaticMap>(parameters => parameters
				.Add(p => p.Center, new GeolocationData(1.1, 2.2))
				.Add(p => p.Markers, new List<GoogleStaticMapMarker>()));

			var map = rendered.Find("img");

			Assert.IsNotNull(map);
			map.MarkupMatches(@"<img src=""https://maps.googleapis.com/maps/api/staticmap?center=1.1,2.2&amp;zoom=12&amp;size=400x300&amp;scale=1&amp;maptype=roadmap&amp;format=gif&amp;key="" />");
		}

		[TestMethod]
		public void GoogleStaticMap_should_render_markers()
		{
			var markers = new List<GoogleStaticMapMarker>()
			{
				{ new GoogleStaticMapMarker() },
				{ new GoogleStaticMapMarker()
					{
						CustomIcon = new GoogleMapMarkerCustomIcon()
						{ Anchor = GoogleMapMarkerCustomIconAnchors.Left, IconUrl = "http://test.com" }
					}
				},
				{ new GoogleStaticMapMarker()
					{
						Style = new GoogleMapMarkerStyle()
						{ Color = "red", Label = 'A' }
					}
				},
				{ new GoogleStaticMapMarker()
					{
						Style = new GoogleMapMarkerStyle()
						{ Color = "0xFFAABB", Label = '2', Size = GoogleMapMarkerSizes.Mid }
					}
				},
			};

			markers.ElementAt(0).Locations.Add(new GeolocationData(1.111, 2.222));
			markers.ElementAt(1).Locations.Add(new GeolocationData(17.111, 33.222));
			markers.ElementAt(1).Locations.Add(new GeolocationData("London"));
			markers.ElementAt(2).Locations.Add(new GeolocationData("New York"));
			markers.ElementAt(3).Locations.Add(new GeolocationData("Budapest"));
			markers.ElementAt(3).Locations.Add(new GeolocationData(5.123, 8.99));

			var rendered = _testContext.RenderComponent<GoogleStaticMap>(parameters => parameters
				.Add(p => p.Center, new GeolocationData(1.1, 2.2))
				.Add(p => p.Markers, markers));

			var map = rendered.Find("img");

			Assert.IsNotNull(map);
			map.MarkupMatches(@"<img src=""https://maps.googleapis.com/maps/api/staticmap?size=400x300&amp;scale=1&amp;maptype=roadmap&amp;format=gif&amp;markers=1.111,2.222&amp;markers=anchor:left|icon:http://test.com|17.111,33.222|London&amp;markers=color:red|label:A|New York&amp;markers=size:mid|color:0xffaabb|label:2|Budapest|5.123,8.99&amp;key="" />");
		}
	}
}