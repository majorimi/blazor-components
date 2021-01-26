using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Majorsoft.Blazor.Components.Maps.Tests
{
	[TestClass]
	public class GeolocationCoordinateTest
	{
		[TestMethod]
		public void GeolocationCoordinate_should_handle_nulls()
		{
			var coord = new GeolocationCoordinate();
			var coord2 = new GeolocationCoordinate()
			{
				Latitude = 0.0
			};
			var coord3 = new GeolocationCoordinate()
			{
				Longitude = 0.0
			};

			Assert.AreEqual(false, coord.HasCoordinates);
			Assert.AreEqual(false, coord2.HasCoordinates);
			Assert.AreEqual(false, coord3.HasCoordinates);
		}

		[TestMethod]
		public void GeolocationCoordinate_should_return_correct_String_format()
		{
			var coord = new GeolocationCoordinate();
			var coord2 = new GeolocationCoordinate()
			{
				Latitude = 0.0
			};
			var coord3 = new GeolocationCoordinate()
			{
				Longitude = 0.0
			};
			var coord4 = new GeolocationCoordinate()
			{
				Latitude = 1.0123456789,
				Longitude = 2.0123456789
			};

			Assert.AreEqual("", coord.ToString());
			Assert.AreEqual("", coord2.ToString());
			Assert.AreEqual("", coord3.ToString());
			Assert.AreEqual("1.0123456789,2.0123456789", coord4.ToString());
		}
	}
}