using Bunit;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Blazor.Components.Toggle.Tests
{
	[TestClass]
	public class ToggleSwitchTest
	{
		private Bunit.TestContext _testContext;

		[TestInitialize]
		public void Init()
		{
			_testContext = new Bunit.TestContext();

			var mock = new Mock<ILogger<ToggleSwitch>>();
			_testContext.Services.Add(new ServiceDescriptor(typeof(ILogger<ToggleSwitch>), mock.Object));
		}

		[TestCleanup]
		public void Cleanup()
		{
			_testContext?.Dispose();
		}

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
		}

		[TestMethod]
		public void ToggleSwitch_should_rendered_correctly_value_true()
		{
			var rendered = _testContext.RenderComponent<ToggleSwitch>(parameters => parameters
					.Add(p => p.Value, true));

			var input = rendered.Find("input");

			Assert.IsNotNull(input);
			var value = input.GetAttribute("value");
			Assert.AreEqual("1", value);
			Assert.IsFalse(input.HasAttribute("disabled"));

			var style = input.GetAttribute("style");
			Assert.AreEqual("width:80px; height:30px; cursor: pointer; border-radius: 15px; background-color: rgba(0, 0, 255, 0.5); ", style);
		}

		[TestMethod]
		public void ToggleSwitch_should_rendered_correctly_value_false()
		{
			var rendered = _testContext.RenderComponent<ToggleSwitch>(parameters => parameters
					.Add(p => p.Value, false));

			var input = rendered.Find("input");

			Assert.IsNotNull(input);
			var value = input.GetAttribute("value");
			Assert.AreEqual("0", value);
			Assert.IsFalse(input.HasAttribute("disabled"));
		}

		[TestMethod]
		public void ToggleSwitch_should_rendered_correctly_disabled()
		{
			var rendered = _testContext.RenderComponent<ToggleSwitch>(parameters => parameters
					.Add(p => p.Disabled, true));

			var input = rendered.Find("input");

			Assert.IsNotNull(input);
			Assert.IsTrue(input.HasAttribute("disabled"));
		}

		[TestMethod]
		public void ToggleSwitch_should_rendered_correctly_width()
		{
			var rendered = _testContext.RenderComponent<ToggleSwitch>(parameters => parameters
					.Add(p => p.Width, 110));

			var input = rendered.Find("input");
			Assert.IsNotNull(input);

			var style = input.GetAttribute("style");
			Assert.AreEqual("width:110px; height:30px; cursor: pointer; border-radius: 15px; background-color: rgba(0, 0, 255, 0.5); ", style);
		}

		[TestMethod]
		public void ToggleSwitch_should_rendered_correctly_height()
		{
			var rendered = _testContext.RenderComponent<ToggleSwitch>(parameters => parameters
					.Add(p => p.Height, 110));

			var input = rendered.Find("input");
			Assert.IsNotNull(input);

			var style = input.GetAttribute("style");
			Assert.AreEqual("width:80px; height:110px; cursor: pointer; border-radius: 55px; background-color: rgba(0, 0, 255, 0.5); ", style);
		}

		[TestMethod]
		public void ToggleSwitch_should_rendered_correctly_onColor()
		{
			var rendered = _testContext.RenderComponent<ToggleSwitch>(parameters => parameters
					.Add(p => p.OnColor, "red"));

			var input = rendered.Find("input");
			Assert.IsNotNull(input);

			var style = input.GetAttribute("style");
			Assert.AreEqual("width:80px; height:30px; cursor: pointer; border-radius: 15px; background-color: rgba(255, 0, 0, 0.5); ", style);
		}

		[TestMethod]
		public void ToggleSwitch_should_rendered_correctly_offColor()
		{
			var rendered = _testContext.RenderComponent<ToggleSwitch>(parameters => parameters
					.Add(p => p.OffColor, "240,240,240")
					.Add(p => p.Value, false));

			var input = rendered.Find("input");
			Assert.IsNotNull(input);

			var style = input.GetAttribute("style");
			Assert.AreEqual("width:80px; height:30px; cursor: pointer; border-radius: 15px; background-color: rgba(240, 240, 240, 0.5); ", style);
		}

		[TestMethod]
		public void ToggleSwitch_should_rendered_correctly_when_toggled()
		{
			var rendered = _testContext.RenderComponent<ToggleSwitch>(parameters => parameters
					.Add(p => p.Value, false));

			var input = rendered.Find("input");

			Assert.IsNotNull(input);

			rendered.SetParametersAndRender(parameters => parameters.Add(p => p.Value, true));
			rendered.WaitForAssertion(() =>
			{
				var value = input.GetAttribute("value");
				Assert.AreEqual("1", value);
			});

			Assert.IsFalse(input.HasAttribute("disabled"));
		}
	}
}