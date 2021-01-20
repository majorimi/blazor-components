using Majorsoft.Blazor.Components.Maps.Google;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Majorsoft.Blazor.Components.Maps.Tests.Google
{
	[TestClass]
	public class GoogleMapMarkerCustomIconTest
	{
		[TestMethod]
		public void GoogleMapMarkerCustomIcon_should_handle_null()
		{
			var icon = new GoogleMapMarkerCustomIcon();

			Assert.IsNotNull(icon.ToString());
			Assert.AreEqual("", icon.ToString());
		}

		[TestMethod]
		public void GoogleMapMarkerCustomIcon_should_generate_valid_string()
		{
			var icon = new GoogleMapMarkerCustomIcon()
			{
				IconUrl = "iconUrl"
			}; var icon2 = new GoogleMapMarkerCustomIcon()
			{
				Anchor = GoogleMapMarkerCustomIconAnchors.Center,
				IconUrl = "iconUrl"
			};

			Assert.AreEqual("anchor:top%7Cicon:iconUrl", icon.ToString());
			Assert.AreEqual("anchor:center%7Cicon:iconUrl", icon2.ToString());
		}
	}
}