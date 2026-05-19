using System;
using Bunit;
using Majorsoft.Blazor.Components.CommonTestsBase;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Majorsoft.Blazor.Components.Toggle.Tests
{
    [TestClass]
    public class ToggleSwitchStateChangeTest : ComponentsTestBase<ToggleSwitch>
    {
        [TestMethod]
        public void ToggleSwitch_should_render_initially_unchecked()
        {
            var rendered = _testContext.Render<ToggleSwitch>(parameters => parameters
                .Add(p => p.Checked, false));

            var input = rendered.Find("input");
            Assert.IsNotNull(input);
            Assert.AreEqual("0", input.GetAttribute("value"));
        }

        [TestMethod]
        public void ToggleSwitch_should_render_initially_checked()
        {
            var rendered = _testContext.Render<ToggleSwitch>(parameters => parameters
                .Add(p => p.Checked, true));

            var input = rendered.Find("input");
            Assert.IsNotNull(input);
            Assert.AreEqual("1", input.GetAttribute("value"));
        }

        [TestMethod]
        public void ToggleSwitch_should_update_when_parameters_change()
        {
            var rendered = _testContext.Render<ToggleSwitch>(parameters => parameters
                .Add(p => p.Checked, false));

            var input = rendered.Find("input");
            Assert.AreEqual("0", input.GetAttribute("value"));

            // Update parameters
            rendered.Render(p => p.Add(x => x.Checked, true));

            rendered.WaitForAssertion(() =>
            {
                Assert.AreEqual("1", input.GetAttribute("value"));
            });
        }

        [TestMethod]
        public void ToggleSwitch_should_support_color_customization()
        {
            var rendered = _testContext.Render<ToggleSwitch>(parameters => parameters
                .Add(p => p.Checked, true)
                .Add(p => p.OnColor, "green"));

            var input = rendered.Find("input");
            var style = input.GetAttribute("style");
            Assert.IsTrue(style.Contains("rgba(0, 128, 0"));
        }

        [TestMethod]
        public void ToggleSwitch_should_support_size_customization()
        {
            var rendered = _testContext.Render<ToggleSwitch>(parameters => parameters
                .Add(p => p.Width, 120)
                .Add(p => p.Height, 60));

            var input = rendered.Find("input");
            var style = input.GetAttribute("style");
            Assert.IsTrue(style.Contains("width:120px"));
            Assert.IsTrue(style.Contains("height:60px"));
        }
    }

    [TestClass]
    public class ToggleButtonStateChangeTest : ComponentsTestBase<ToggleButton>
    {
        [TestMethod]
        public void ToggleButton_should_be_unchecked_by_default()
        {
            var rendered = _testContext.Render<ToggleButton>(parameters => parameters
                .Add(p => p.Checked, false));

            var button = rendered.Find("button");
            Assert.IsNotNull(button);
            var style = button.GetAttribute("style");
            Assert.IsTrue(style.Contains("rgb(255, 255, 255)"));
        }

        [TestMethod]
        public void ToggleButton_should_be_checked_when_set()
        {
            var rendered = _testContext.Render<ToggleButton>(parameters => parameters
                .Add(p => p.Checked, true));

            var button = rendered.Find("button");
            var style = button.GetAttribute("style");
            Assert.IsTrue(style.Contains("rgb(211,211,211)"));
        }

        [TestMethod]
        public void ToggleButton_should_support_custom_content()
        {
            var rendered = _testContext.Render<ToggleButton>(parameters => parameters
                .Add(p => p.Content, "Click Me"));

            var button = rendered.Find("button");
            Assert.IsTrue(button.InnerHtml.Contains("Click Me"));
        }

        [TestMethod]
        public void ToggleButton_should_support_disabled_state()
        {
            var rendered = _testContext.Render<ToggleButton>(parameters => parameters
                .Add(p => p.Disabled, true));

            var button = rendered.Find("button");
            Assert.IsTrue(button.HasAttribute("disabled"));
        }

        [TestMethod]
        public void ToggleButton_should_update_when_checked_changes()
        {
            var rendered = _testContext.Render<ToggleButton>(parameters => parameters
                .Add(p => p.Checked, false));

            var button = rendered.Find("button");
            var style1 = button.GetAttribute("style");
            Assert.IsTrue(style1.Contains("rgb(255, 255, 255)"));

            rendered.Render(p => p.Add(x => x.Checked, true));

            rendered.WaitForAssertion(() =>
            {
                var style2 = button.GetAttribute("style");
                Assert.IsTrue(style2.Contains("rgb(211,211,211)"));
            });
        }

        [TestMethod]
        public void ToggleButton_should_support_size_customization()
        {
            var rendered = _testContext.Render<ToggleButton>(parameters => parameters
                .Add(p => p.Width, 80)
                .Add(p => p.Height, 80));

            var button = rendered.Find("button");
            var style = button.GetAttribute("style");
            Assert.IsTrue(style.Contains("width: 80px"));
            Assert.IsTrue(style.Contains("height: 80px"));
        }
    }
}
