using Microsoft.VisualStudio.TestTools.UnitTesting;

using Bunit;
using Majorsoft.Blazor.Components.CommonTestsBase;

namespace Majorsoft.Blazor.Components.Inputs.Tests
{
	[TestClass]
	public class MaxLengthInputTest : ComponentsTestBase<MaxLengthInput>
	{
		[TestMethod]
		public void MaxLengthInput_should_rendered_correctly_html_attributes()
		{
			var rendered = _testContext.RenderComponent<MaxLengthInput>(
				("id", "id1"), //HTML attributes
				("class", "form-control w-100") //HTML attributes
				);

			var input = rendered.Find("input");
			var label = rendered.Find("label");

			Assert.IsNotNull(input);
			Assert.IsNotNull(label);
			input.MarkupMatches(@"<input maxlength=""50""  id=""id1"" class=""form-control w-100"" >");
			label.MarkupMatches(@"<label class="""">Remaining characters: 50</label>");
		}

		[TestMethod]
		public void MaxLengthInput_should_rendered_initial_value()
		{
			var rendered = _testContext.RenderComponent<MaxLengthInput>(parameters => parameters
				.Add(p => p.Value, "test"));

			var input = rendered.Find("input");
			var label = rendered.Find("label");

			Assert.IsNotNull(input);
			Assert.IsNotNull(label);
			input.MarkupMatches(@"<input value=""test"" maxlength=""50""/>");
			label.MarkupMatches(@"<label class="""">Remaining characters: 46</label>");
		}

		[TestMethod]
		public void MaxLengthInput_should_rendered_initial_value_with_countdown_text()
		{
			var rendered = _testContext.RenderComponent<MaxLengthInput>(parameters => parameters
				.Add(p => p.Value, "test")
				.Add(p => p.CountdownText, "Remaining chars: "));

			var input = rendered.Find("input");
			var label = rendered.Find("label");

			Assert.IsNotNull(input);
			Assert.IsNotNull(label);
			input.MarkupMatches(@"<input value=""test"" maxlength=""50""/>");
			label.MarkupMatches(@"<label class="""">Remaining chars: 46</label>");
		}

		[TestMethod]
		public void MaxLengthInput_should_rendered_initial_MaxAllowedChars()
		{
			var rendered = _testContext.RenderComponent<MaxLengthInput>(parameters => parameters
				.Add(p => p.MaxAllowedChars, 11));

			var input = rendered.Find("input");
			var label = rendered.Find("label");

			Assert.IsNotNull(input);
			Assert.IsNotNull(label);
			input.MarkupMatches(@"<input maxlength=""11""/>");
			label.MarkupMatches(@"<label class="""">Remaining characters: 11</label>");
		}

		[TestMethod]
		public void MaxLengthInput_should_rendered_without_ShowRemainingChars()
		{
			var rendered = _testContext.RenderComponent<MaxLengthInput>(parameters => parameters
				.Add(p => p.ShowRemainingChars, false));

			var input = rendered.Find("input");
			var label = rendered.Find("label");

			Assert.IsNotNull(input);
			Assert.IsNotNull(label);
			input.MarkupMatches(@"<input maxlength=""50""/>");
			label.MarkupMatches(@"<label class="""">Remaining characters: </label>");
		}

		[TestMethod]
		public void MaxLengthInput_should_rendered_initial_CountdownTextClass()
		{
			var rendered = _testContext.RenderComponent<MaxLengthInput>(parameters => parameters
				.Add(p => p.CountdownTextClass, "css1 css2"));

			var input = rendered.Find("input");
			var label = rendered.Find("label");

			Assert.IsNotNull(input);
			Assert.IsNotNull(label);
			input.MarkupMatches(@"<input maxlength=""50""/>");
			label.MarkupMatches(@"<label class=""css1 css2"">Remaining characters: 50</label>");
		}

		[TestMethod]
		public void MaxLengthInput_should_rendered_and_event_triggered()
		{
			string text = string.Empty;
			int remaining = 0;

			var rendered = _testContext.RenderComponent<MaxLengthInput>(parameters => parameters
				.Add(p => p.OnInput, val => { text = val; })
				.Add(p => p.OnRemainingCharsChanged, val => { remaining = val; }));

			var input = rendered.Find("input");
			var label = rendered.Find("label");

			input.Input("t");

			Assert.IsNotNull(input);
			Assert.IsNotNull(label);
			Assert.AreEqual("t", text);
			Assert.AreEqual(49, remaining);

			input.MarkupMatches(@"<input value=""t"" maxlength=""50""/>");
			label.MarkupMatches(@"<label class="""">Remaining characters: 49</label>");
		}
	}
}