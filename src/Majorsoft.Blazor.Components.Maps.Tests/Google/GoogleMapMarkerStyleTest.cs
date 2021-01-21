using Majorsoft.Blazor.Components.Maps.Google;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Majorsoft.Blazor.Components.Maps.Tests.Google
{
	[TestClass]
	public class GoogleMapMarkerStyleTest
	{
		[TestMethod]
		public void GoogleMapMarkerStyle_should_handle_null()
		{
			var style = new GoogleMapMarkerStyle();

			Assert.IsNotNull(style.ToString());
			Assert.AreEqual("", style.ToString());
		}

		[TestMethod]
		public void GoogleMapMarkerStyle_should_generate_only_size()
		{
			var style = new GoogleMapMarkerStyle()
			{
				Size = GoogleMapMarkerSizes.Mid
			};

			Assert.AreEqual("size:mid", style.ToString());
		}

		[TestMethod]
		public void GoogleMapMarkerStyle_should_generate_only_color()
		{
			var style = new GoogleMapMarkerStyle()
			{
				Color = "white"
			};

			Assert.AreEqual("color:white", style.ToString());
		}

		[TestMethod]
		public void GoogleMapMarkerStyle_should_generate_only_label()
		{
			var style = new GoogleMapMarkerStyle()
			{
				Label = 'a',
			};

			Assert.AreEqual("label:A", style.ToString());
		}

		[TestMethod]
		public void GoogleMapMarkerStyle_should_generate_only_scale()
		{
			var style = new GoogleMapMarkerStyle()
			{
				Scale = 4
			};

			Assert.AreEqual("scale:4", style.ToString());
		}

		[TestMethod]
		public void GoogleMapMarkerStyle_should_generate_only_non_default()
		{
			var style = new GoogleMapMarkerStyle()
			{
				Label = '9',
				Color = "12345",
				Scale = 1,
				Size = GoogleMapMarkerSizes.Normal,
			};

			Assert.AreEqual("color:12345|label:9", style.ToString());
		}

		[TestMethod]
		public void GoogleMapMarkerStyle_should_generate_only_all()
		{
			var style = new GoogleMapMarkerStyle()
			{
				Color = "red",
				Size = GoogleMapMarkerSizes.Tiny,
				Label = 'X',
				Scale = 4
			};

			Assert.AreEqual("scale:4|size:tiny|color:red|label:X", style.ToString());
		}
	}
}