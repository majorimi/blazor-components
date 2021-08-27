using System.Threading.Tasks;

using Bunit;

using Majorsoft.Blazor.Components.CommonTestsBase;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Majorsoft.Blazor.Components.Toggle.Tests
{
	[TestClass]
	public class ToggleButtonTest : ComponentsTestBase<ToggleButton>
	{
		[TestMethod]
		public void ToggleButton_should_rendered_correctly_html_attributes()
		{
			var rendered = _testContext.RenderComponent<ToggleButton>(
				("title", "text") //HTML attributes
				);

			var input = rendered.Find("button");

			Assert.IsNotNull(input);
			Assert.IsNotNull(input.Id);
			Assert.IsTrue(input.HasAttribute("title"));
		}

		[TestMethod]
		public void ToggleButton_should_rendered_correctly_Checked_true()
		{
			var rendered = _testContext.RenderComponent<ToggleButton>(parameters => parameters
					.Add(p => p.Checked, true));

			var input = rendered.Find("button");

			Assert.IsNotNull(input);
			Assert.IsFalse(input.HasAttribute("disabled"));

			var id = input.GetAttribute("id");
			input.MarkupMatches(@$"<button id=""{id}"" class=""toggleButton"" style=""width: 30px; height: 30px; background-color: rgb(211,211,211);"" ></button>");
		}

		[TestMethod]
		public void ToggleButton_should_rendered_correctly_Content()
		{
			var rendered = _testContext.RenderComponent<ToggleButton>(parameters => parameters
					.Add(p => p.Content, "<strong>B</strong>"));

			var input = rendered.Find("button");

			Assert.IsNotNull(input);
			Assert.IsFalse(input.HasAttribute("disabled"));

			var id = input.GetAttribute("id");
			input.MarkupMatches(@$"<button id=""{id}"" class=""toggleButton"" style=""width: 30px; height: 30px; background-color: rgb(255, 255, 255);"" ><strong>B</strong></button>");
		}

		[TestMethod]
		public void ToggleButton_should_rendered_correctly_Checked_false()
		{
			var rendered = _testContext.RenderComponent<ToggleButton>(parameters => parameters
					.Add(p => p.Checked, false));

			var input = rendered.Find("button");

			Assert.IsNotNull(input);
			Assert.IsFalse(input.HasAttribute("disabled"));

			var id = input.GetAttribute("id");
			input.MarkupMatches(@$"<button id=""{id}"" class=""toggleButton"" style=""width: 30px; height: 30px; background-color: rgb(255, 255, 255);"" ></button>");
		}

		[TestMethod]
		public void ToggleButton_should_rendered_correctly_disabled()
		{
			var rendered = _testContext.RenderComponent<ToggleButton>(parameters => parameters
					.Add(p => p.Disabled, true));

			var input = rendered.Find("button");

			Assert.IsNotNull(input);
			Assert.IsTrue(input.HasAttribute("disabled"));

			var id = input.GetAttribute("id");
			input.MarkupMatches(@$"<button id=""{id}"" class=""toggleButton"" style=""width: 30px; height: 30px; background-color: rgb(255, 255, 255);"" disabled=""""></button>");
		}

		[TestMethod]
		public void ToggleButton_should_rendered_correctly_width()
		{
			var rendered = _testContext.RenderComponent<ToggleButton>(parameters => parameters
					.Add(p => p.Width, 110));

			var input = rendered.Find("button");
			Assert.IsNotNull(input);

			var id = input.GetAttribute("id");
			input.MarkupMatches(@$"<button id=""{id}"" class=""toggleButton"" style=""width: 110px; height: 30px; background-color: rgb(255, 255, 255);"" ></button>");
		}

		[TestMethod]
		public void ToggleButton_should_rendered_correctly_height()
		{
			var rendered = _testContext.RenderComponent<ToggleButton>(parameters => parameters
					.Add(p => p.Height, 110));

			var input = rendered.Find("button");
			Assert.IsNotNull(input);

			var id = input.GetAttribute("id");
			input.MarkupMatches(@$"<button id=""{id}"" class=""toggleButton"" style=""width: 30px; height: 110px; background-color: rgb(255, 255, 255);"" ></button>");
		}

		[TestMethod]
		public async Task ToggleButton_should_rendered_correctly_onHoverEvent()
		{
			var rendered = _testContext.RenderComponent<ToggleButton>(parameters => parameters
					.Add(p => p.Checked, false));

			var input = rendered.Find("button");
			Assert.IsNotNull(input);

			await input.TriggerEventAsync("onmouseenter", new MouseEventArgs());

			var id = input.GetAttribute("id");
			input.MarkupMatches(@$"<button id=""{id}"" class=""toggleButton"" style=""width: 30px; height: 30px; background-color: rgb(245, 245, 245);"" ></button>");

			rendered.SetParametersAndRender(parameters => parameters.Add(p => p.Checked, true));
			await input.TriggerEventAsync("onmouseenter", new MouseEventArgs()); //When checked no Hover color change

			rendered.WaitForAssertion(() =>
			{
				input.MarkupMatches(@$"<button id=""{id}"" class=""toggleButton"" style=""width: 30px; height: 30px; background-color: rgb(211, 211, 211);"" ></button>");
			});
		}

		[TestMethod]
		public void ToggleButton_should_rendered_correctly_onColor()
		{
			var rendered = _testContext.RenderComponent<ToggleButton>(parameters => parameters
				.Add(p => p.Checked, true)
				.Add(p => p.OnColor, "red"));

			var input = rendered.Find("button");
			Assert.IsNotNull(input);

			var id = input.GetAttribute("id");
			input.MarkupMatches(@$"<button id=""{id}"" class=""toggleButton"" style=""width: 30px; height: 30px; background-color: rgb(255, 0, 0);"" ></button>");
		}

		[TestMethod]
		public void ToggleButton_should_rendered_correctly_offColor()
		{
			var rendered = _testContext.RenderComponent<ToggleButton>(parameters => parameters
					.Add(p => p.OffColor, "240,240,240")
					.Add(p => p.Checked, false));

			var input = rendered.Find("button");
			Assert.IsNotNull(input);

			var id = input.GetAttribute("id");
			input.MarkupMatches(@$"<button id=""{id}"" class=""toggleButton"" style=""width: 30px; height: 30px; background-color: rgb(240, 240, 240);"" ></button>");
		}

		[TestMethod]
		public void ToggleButton_should_rendered_correctly_when_toggled()
		{
			var rendered = _testContext.RenderComponent<ToggleButton>(parameters => parameters
					.Add(p => p.Checked, false));

			var input = rendered.Find("button");

			Assert.IsNotNull(input);

			var id = input.GetAttribute("id");
			input.MarkupMatches(@$"<button id=""{id}"" class=""toggleButton"" style=""width: 30px; height: 30px; background-color: rgb(255, 255, 255);"" ></button>");

			rendered.SetParametersAndRender(parameters => parameters.Add(p => p.Checked, true));
			rendered.WaitForAssertion(() =>
			{
				input.MarkupMatches(@$"<button id=""{id}"" class=""toggleButton"" style=""width: 30px; height: 30px; background-color: rgb(211, 211, 211);"" ></button>");
			});

			Assert.IsFalse(input.HasAttribute("disabled"));
		}
	}
}