using Majorsoft.Blazor.Components.Maps.Google;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Majorsoft.Blazor.Components.Maps.Tests.Google
{
	[TestClass]
	public class GoogleMapMarkerTest
	{
		[TestMethod]
		public void GoogleMapMarker_should_handle_null()
		{
			var marker = new GoogleMapMarker();

			Assert.IsNotNull(marker.ToString());
			Assert.IsNotNull(marker.Locations);
			Assert.AreEqual("", marker.ToString());
		}

		[TestMethod]
		public void GoogleMapMarker_should_handle_empty_styles()
		{
			var marker = new GoogleMapMarker()
			{
				CustomIcon = new GoogleMapMarkerCustomIcon(),
				Style = new GoogleMapMarkerStyle()
			};

			Assert.IsNotNull(marker.ToString());
			Assert.AreEqual("", marker.ToString());
		}

		[TestMethod]
		public void GoogleMapMarker_should_handle_null_locations()
		{
			var marker = new GoogleMapMarker()
			{
				CustomIcon = new GoogleMapMarkerCustomIcon(),
				Style = new GoogleMapMarkerStyle()
			};
			marker.Locations.Add(null);
			marker.Locations.Add(null);

			Assert.IsNotNull(marker.ToString());
			Assert.AreEqual("", marker.ToString());
		}

		[TestMethod]
		public void GoogleMapMarker_should_handle_empty_locations()
		{
			var marker = new GoogleMapMarker()
			{
				CustomIcon = new GoogleMapMarkerCustomIcon(),
				Style = new GoogleMapMarkerStyle()
			};
			marker.Locations.Add(new GeolocationData());
			marker.Locations.Add(new GeolocationData());

			Assert.IsNotNull(marker.ToString());
			Assert.AreEqual("", marker.ToString());
		}

		[TestMethod]
		public void GoogleMapMarker_should_handle_location_with_no_styling()
		{
			var marker = new GoogleMapMarker()
			{
				CustomIcon = null,
				Style = null
			};

			marker.Locations.Add(new GeolocationData()
			{
				Latitude = 4.123,
				Longitude = 5.123
			});

			Assert.IsNotNull(marker.ToString());
			Assert.AreEqual("markers=4.123,5.123", marker.ToString());
		}

		[TestMethod]
		public void GoogleMapMarker_should_handle_locations_with_no_styling()
		{
			var marker = new GoogleMapMarker()
			{
				CustomIcon = new GoogleMapMarkerCustomIcon(),
				Style = new GoogleMapMarkerStyle()
			};

			marker.Locations.Add(new GeolocationData()
			{
				Latitude = 1.123,
				Longitude = 7.123
			});
			marker.Locations.Add(new GeolocationData()
			{
				Latitude = 4.123,
				Longitude = 5.123
			});

			Assert.IsNotNull(marker.ToString());
			Assert.AreEqual("markers=1.123,7.123|4.123,5.123", marker.ToString());
		}

		[TestMethod]
		public void GoogleMapMarker_should_handle_locations_with_style()
		{
			var marker = new GoogleMapMarker()
			{
				Style = new GoogleMapMarkerStyle()
				{
					Color = "red",
					Label = 'a',
					Size = GoogleMapMarkerSizes.Mid,
					Scale = 2
				}
			};

			marker.Locations.Add(new GeolocationData()
			{
				Latitude = 1.123,
				Longitude = 7.123
			});
			marker.Locations.Add(new GeolocationData()
			{
				Latitude = 4.123,
				Longitude = 5.123
			});

			Assert.IsNotNull(marker.ToString());
			Assert.AreEqual("markers=scale:2|size:mid|color:red|label:A|1.123,7.123|4.123,5.123", marker.ToString());
		}

		[TestMethod]
		public void GoogleMapMarker_should_handle_locations_with_customIcon()
		{
			var marker = new GoogleMapMarker()
			{
				CustomIcon = new GoogleMapMarkerCustomIcon()
				{
					Anchor = GoogleMapMarkerCustomIconAnchors.Center,
					IconUrl = "http://test.org"
				}
			};

			marker.Locations.Add(new GeolocationData()
			{
				Latitude = 1.123,
				Longitude = 7.123
			});
			marker.Locations.Add(new GeolocationData()
			{
				Latitude = 4.123,
				Longitude = 5.123
			});

			Assert.IsNotNull(marker.ToString());
			Assert.AreEqual("markers=anchor:center|icon:http://test.org|1.123,7.123|4.123,5.123", marker.ToString());
		}

		[TestMethod]
		public void GoogleMapMarker_should_handle_locations_with_style_overriden()
		{
			var marker = new GoogleMapMarker()
			{
				Style = new GoogleMapMarkerStyle()
				{
					Color = "red",
					Label = 'a',
					Size = GoogleMapMarkerSizes.Mid,
					Scale = 2
				},
				CustomIcon = new GoogleMapMarkerCustomIcon(),
			};

			marker.Locations.Add(new GeolocationData()
			{
				Latitude = 1.123,
				Longitude = 7.123
			});
			marker.Locations.Add(new GeolocationData()
			{
				Address = "London"
			});

			Assert.IsNotNull(marker.ToString());
			Assert.AreEqual("markers=1.123,7.123|London", marker.ToString());
		}
	}
}