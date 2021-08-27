using Bunit;

using Majorsoft.Blazor.Components.CommonTestsBase;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Majorsoft.Blazor.Components.Toggle.Tests
{
	[TestClass]
	public class ToggleSwitchTest : ComponentsTestBase<ToggleSwitch>
	{
		[TestMethod]
		public void ToggleSwitch_should_rendered_correctly_html_attributes()
		{
			var rendered = _testContext.RenderComponent<ToggleSwitch>(
				("title", "text") //HTML attributes
				);

			var input = rendered.Find("input");

			Assert.IsNotNull(input);
			Assert.IsNotNull(input.Id);
			Assert.IsTrue(input.HasAttribute("title"));

			var id = input.GetAttribute("id");
			input.MarkupMatches(@$"<input id=""{id}"" type=""range"" min=""0"" max=""1""  style=""width:80px; height:30px; cursor: pointer; border-radius: 15px; background-color: rgba(0, 0, 255, 0.50);"" title=""text"" value=""1"">");
		}

		[TestMethod]
		public void ToggleSwitch_should_rendered_correctly_value_true()
		{
			var rendered = _testContext.RenderComponent<ToggleSwitch>(parameters => parameters
					.Add(p => p.Checked, true));

			var input = rendered.Find("input");

			Assert.IsNotNull(input);
			var value = input.GetAttribute("value");
			Assert.AreEqual("1", value);
			Assert.IsFalse(input.HasAttribute("disabled"));

			var style = input.GetAttribute("style");
			Assert.AreEqual("width:80px; height:30px; cursor: pointer; border-radius: 15px; background-color: rgba(0, 0, 255, 0.50); ", style);
		}

		[TestMethod]
		public void ToggleSwitch_should_rendered_correctly_value_false()
		{
			var rendered = _testContext.RenderComponent<ToggleSwitch>(parameters => parameters
					.Add(p => p.Checked, false));

			var input = rendered.Find("input");

			Assert.IsNotNull(input);
			var value = input.GetAttribute("value");
			Assert.AreEqual("0", value);
			Assert.IsFalse(input.HasAttribute("disabled"));

			var id = input.GetAttribute("id");
			input.MarkupMatches(@$"<input id=""{id}"" type=""range"" min=""0"" max=""1""  style=""width:80px; height:30px; cursor: pointer; border-radius: 15px; background-color: rgba(169, 169, 169, 0.50);"" value=""0"">");
		}

		[TestMethod]
		public void ToggleSwitch_should_rendered_correctly_disabled()
		{
			var rendered = _testContext.RenderComponent<ToggleSwitch>(parameters => parameters
					.Add(p => p.Disabled, true)
					.Add(p => p.Checked, true));

			var input = rendered.Find("input");

			Assert.IsNotNull(input);
			Assert.IsTrue(input.HasAttribute("disabled"));

			var id = input.GetAttribute("id");
			input.MarkupMatches(@$"<input id=""{id}"" type=""range"" min=""0"" max=""1""  style=""width:80px; height:30px; cursor: pointer; border-radius: 15px; background-color: rgba(230, 230, 230, 0.50);"" value=""1"" disabled="""">");

			rendered.SetParametersAndRender(parameters => parameters.Add(p => p.Checked, false));
			rendered.WaitForAssertion(() =>
			{
				input.MarkupMatches(@$"<input id=""{id}"" type=""range"" min=""0"" max=""1""  style=""width:80px; height:30px; cursor: pointer; border-radius: 15px; background-color: rgba(230, 230, 230, 0.50);"" value=""0"" disabled="""">");
			});
		}

		[TestMethod]
		public void ToggleSwitch_should_rendered_correctly_width()
		{
			var rendered = _testContext.RenderComponent<ToggleSwitch>(parameters => parameters
					.Add(p => p.Width, 110));

			var input = rendered.Find("input");
			Assert.IsNotNull(input);

			var style = input.GetAttribute("style");
			Assert.AreEqual("width:110px; height:30px; cursor: pointer; border-radius: 15px; background-color: rgba(0, 0, 255, 0.50); ", style);
		}

		[TestMethod]
		public void ToggleSwitch_should_rendered_correctly_height()
		{
			var rendered = _testContext.RenderComponent<ToggleSwitch>(parameters => parameters
					.Add(p => p.Height, 110));

			var input = rendered.Find("input");
			Assert.IsNotNull(input);

			var style = input.GetAttribute("style");
			Assert.AreEqual("width:80px; height:110px; cursor: pointer; border-radius: 55px; background-color: rgba(0, 0, 255, 0.50); ", style);
		}

		[TestMethod]
		public void ToggleSwitch_should_rendered_correctly_onColor()
		{
			var rendered = _testContext.RenderComponent<ToggleSwitch>(parameters => parameters
					.Add(p => p.OnColor, "red"));

			var input = rendered.Find("input");
			Assert.IsNotNull(input);

			var style = input.GetAttribute("style");
			Assert.AreEqual("width:80px; height:30px; cursor: pointer; border-radius: 15px; background-color: rgba(255, 0, 0, 0.50); ", style);
		}

		[TestMethod]
		public void ToggleSwitch_should_rendered_correctly_offColor()
		{
			var rendered = _testContext.RenderComponent<ToggleSwitch>(parameters => parameters
					.Add(p => p.OffColor, "240,240,240")
					.Add(p => p.Checked, false));

			var input = rendered.Find("input");
			Assert.IsNotNull(input);

			var style = input.GetAttribute("style");
			Assert.AreEqual("width:80px; height:30px; cursor: pointer; border-radius: 15px; background-color: rgba(240, 240, 240, 0.50); ", style);
		}

		[TestMethod]
		public void ToggleSwitch_should_rendered_correctly_when_toggled()
		{
			var rendered = _testContext.RenderComponent<ToggleSwitch>(parameters => parameters
					.Add(p => p.Checked, false));

			var input = rendered.Find("input");

			Assert.IsNotNull(input);

			rendered.SetParametersAndRender(parameters => parameters.Add(p => p.Checked, true));
			rendered.WaitForAssertion(() =>
			{
				var value = input.GetAttribute("value");
				Assert.AreEqual("1", value);
			});

			Assert.IsFalse(input.HasAttribute("disabled"));
		}

		[TestMethod]
		public void ToggleSwitch_should_rendered_correctly_HandleStyle()
		{
			var rendered = _testContext.RenderComponent<ToggleSwitch>(parameters => parameters
					.Add(p => p.HandleStyle, ToggleSwitchStyle.Circle));

			var input = rendered.Find("input");
			var id = input.GetAttribute("id");

			input.MarkupMatches(@$"<input id=""{id}"" type=""range"" min=""0"" max=""1""  style=""width:80px; height:30px; cursor: pointer; border-radius: 15px; background-color: rgba(0, 0, 255, 0.50);"" value=""1"">");

			rendered.SetParametersAndRender(parameters => parameters.Add(p => p.HandleStyle, ToggleSwitchStyle.Ellipse));
			rendered.WaitForAssertion(() =>
			{
				input.MarkupMatches(@$"<input id=""{id}"" type=""range"" min=""0"" max=""1""  style=""width:80px; height:30px; cursor: pointer; border-radius: 15px; background-color: rgba(0, 0, 255, 0.50);"" value=""1"">");
			});


			rendered.SetParametersAndRender(parameters => parameters.Add(p => p.HandleStyle, ToggleSwitchStyle.Square));
			rendered.WaitForAssertion(() =>
			{
				input.MarkupMatches(@$"<input id=""{id}"" type=""range"" min=""0"" max=""1""  style=""width:80px; height:30px; cursor: pointer; border-radius: 0px; background-color: rgba(0, 0, 255, 0.50);"" value=""1"">");
			});
		}
	}
}