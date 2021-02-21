using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Majorsoft.Blazor.Components.Maps.Tests
{
	[TestClass]
	public class GeolocationCoordinateTest
	{
		[TestMethod]
		public void GeolocationCoordinate_should_handle_nulls()
		{
			var coord = new GeolocationCoordinate(null, null);
			var coord2 = new GeolocationCoordinate(0.0, null);
			var coord3 = new GeolocationCoordinate(null, 0.0);

			Assert.AreEqual(false, coord.HasCoordinates);
			Assert.AreEqual(false, coord2.HasCoordinates);
			Assert.AreEqual(false, coord3.HasCoordinates);
		}

		[TestMethod]
		public void GeolocationCoordinate_should_return_correct_String_format()
		{
			var coord = new GeolocationCoordinate(null, null);
			var coord2 = new GeolocationCoordinate(0.0, null);
			var coord3 = new GeolocationCoordinate(null, 0.0);
			var coord4 = new GeolocationCoordinate(1.0123456789, 2.0123456789);

			Assert.AreEqual("", coord.ToString());
			Assert.AreEqual("", coord2.ToString());
			Assert.AreEqual("", coord3.ToString());
			Assert.AreEqual("1.0123456789,2.0123456789", coord4.ToString());
		}
	}
}