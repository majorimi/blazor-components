using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Majorsoft.Blazor.Components.Maps.Tests
{
	[TestClass]
	public class GeolocationDataTest
	{
		[TestMethod]
		public void GeolocationData_should_handle_nulls()
		{
			var geo = new GeolocationData();

			Assert.AreEqual(false, geo.HasCoordinates);
			Assert.IsNull(geo.Address);
			Assert.IsNull(geo.ToString());
		}

		[TestMethod]
		public void GeolocationData_should_handle_location()
		{
			var geo = new GeolocationData()
			{
				Latitude = 1.0123456789,
				Longitude = 2.0123456789
			};

			Assert.AreEqual(true, geo.HasCoordinates);
			Assert.IsNull(geo.Address);
			Assert.AreEqual("1.0123456789,2.0123456789", geo.ToString());
		}

		[TestMethod]
		public void GeolocationData_should_handle_address()
		{
			var geo = new GeolocationData()
			{
				Address = "Test address"
			};

			Assert.AreEqual(false, geo.HasCoordinates);
			Assert.AreEqual("Test address", geo.Address);
			Assert.AreEqual("Test address", geo.ToString());
		}

		[TestMethod]
		public void GeolocationData_should_coordinate_override_address()
		{
			var geo = new GeolocationData()
			{
				Latitude = 1.0123456789,
				Longitude = 2.0123456789,
				Address = "Test address"
			};

			Assert.AreEqual(true, geo.HasCoordinates);
			Assert.AreEqual("Test address", geo.Address);
			Assert.AreEqual("1.0123456789,2.0123456789", geo.ToString());
		}
	}
}