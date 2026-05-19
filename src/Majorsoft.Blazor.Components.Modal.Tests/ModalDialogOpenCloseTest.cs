using System;
using System.Threading.Tasks;
using Bunit;
using Majorsoft.Blazor.Components.Common.JsInterop.Focus;
using Majorsoft.Blazor.Components.CssEvents.Transition;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Majorsoft.Blazor.Components.CommonTestsBase;

namespace Majorsoft.Blazor.Components.Modal.Tests
{
    [TestClass]
    public class ModalDialogOpenCloseTest : ComponentsTestBase<ModalDialog>
    {
        private Mock<ITransitionEventsService> _transitionMock;
        private Mock<IFocusHandler> _focusHandlerMock;

        [TestInitialize]
        public void Init()
        {
            _transitionMock = new Mock<ITransitionEventsService>();
            _focusHandlerMock = new Mock<IFocusHandler>();

            _testContext.Services.Add(new ServiceDescriptor(typeof(ITransitionEventsService), _transitionMock.Object));
            _testContext.Services.Add(new ServiceDescriptor(typeof(IFocusHandler), _focusHandlerMock.Object));
        }

        [TestMethod]
        public void ModalDialog_should_start_closed()
        {
            var rendered = _testContext.Render<ModalDialog>();

            Assert.AreEqual(false, rendered.Instance.IsOpen);
            rendered.MarkupMatches("");
        }

        [TestMethod]
        public async Task ModalDialog_should_open_when_Open_called()
        {
            var rendered = _testContext.Render<ModalDialog>();

            await rendered.InvokeAsync(async () => { await rendered.Instance.Open(); });
            rendered.Render();

            Assert.AreEqual(true, rendered.Instance.IsOpen);
        }

        [TestMethod]
        public async Task ModalDialog_should_close_when_Close_called()
        {
            var rendered = _testContext.Render<ModalDialog>(parameters => parameters
                .Add(p => p.Animate, false));

            await rendered.InvokeAsync(async () => { await rendered.Instance.Open(); });
            rendered.Render();
            Assert.AreEqual(true, rendered.Instance.IsOpen);

            await rendered.InvokeAsync(async () => { await rendered.Instance.Close(); });
            rendered.Render();
            Assert.AreEqual(false, rendered.Instance.IsOpen);
        }

        [TestMethod]
        public async Task ModalDialog_should_handle_multiple_open_close_cycles()
        {
            var rendered = _testContext.Render<ModalDialog>(parameters => parameters
                .Add(p => p.Animate, false));

            for (int i = 0; i < 3; i++)
            {
                await rendered.InvokeAsync(async () => { await rendered.Instance.Open(); });
                rendered.Render();
                Assert.AreEqual(true, rendered.Instance.IsOpen);

                await rendered.InvokeAsync(async () => { await rendered.Instance.Close(); });
                rendered.Render();
                Assert.AreEqual(false, rendered.Instance.IsOpen);
            }
        }

        [TestMethod]
        public async Task ModalDialog_should_render_overlay_with_custom_color()
        {
            var rendered = _testContext.Render<ModalDialog>(parameters => parameters
                .Add(p => p.OverlayBackgroundColor, "red")
                .Add(p => p.OverlayOpacity, 0.5));

            await rendered.InvokeAsync(async () => { await rendered.Instance.Open(); });
            rendered.Render();

            rendered.WaitForAssertion(() =>
            {
                var modal = rendered.Find(".bmodal");
                Assert.IsNotNull(modal);
                var style = modal.GetAttribute("style");
                Assert.IsTrue(style.Contains("rgba(255, 0, 0, 0.50)"));
            });
        }

        [TestMethod]
        public async Task ModalDialog_should_support_custom_dimensions()
        {
            var rendered = _testContext.Render<ModalDialog>(parameters => parameters
                .Add(p => p.Width, 400)
                .Add(p => p.Height, 300)
                .Add(p => p.MinWidth, 400)
                .Add(p => p.MinHeight, 300));

            await rendered.InvokeAsync(async () => { await rendered.Instance.Open(); });
            rendered.Render();

            Assert.AreEqual(true, rendered.Instance.IsOpen);
        }

        [TestMethod]
        public async Task ModalDialog_should_render_header_and_close_button()
        {
            var rendered = _testContext.Render<ModalDialog>();

            await rendered.InvokeAsync(async () => { await rendered.Instance.Open(); });
            rendered.Render();

            rendered.WaitForAssertion(() =>
            {
                var header = rendered.Find(".bmodal-header");
                Assert.IsNotNull(header);

                var closeButton = rendered.Find("button.close");
                Assert.IsNotNull(closeButton);
            });
        }

        [TestMethod]
        public async Task ModalDialog_should_render_body()
        {
            var rendered = _testContext.Render<ModalDialog>();

            await rendered.InvokeAsync(async () => { await rendered.Instance.Open(); });
            rendered.Render();

            rendered.WaitForAssertion(() =>
            {
                var body = rendered.Find(".bmodal-body");
                Assert.IsNotNull(body);
            });
        }
    }

    [TestClass]
    public class ModalDialogOpacityTest : ComponentsTestBase<ModalDialog>
    {
        private Mock<ITransitionEventsService> _transitionMock;
        private Mock<IFocusHandler> _focusHandlerMock;

        [TestInitialize]
        public void Init()
        {
            _transitionMock = new Mock<ITransitionEventsService>();
            _focusHandlerMock = new Mock<IFocusHandler>();

            _testContext.Services.Add(new ServiceDescriptor(typeof(ITransitionEventsService), _transitionMock.Object));
            _testContext.Services.Add(new ServiceDescriptor(typeof(IFocusHandler), _focusHandlerMock.Object));
        }

        [TestMethod]
        public async Task ModalDialog_should_handle_zero_opacity()
        {
            var rendered = _testContext.Render<ModalDialog>(parameters => parameters
                .Add(p => p.OverlayOpacity, 0));

            await rendered.InvokeAsync(async () => { await rendered.Instance.Open(); });
            rendered.Render();

            rendered.WaitForAssertion(() =>
            {
                var modal = rendered.Find(".bmodal");
                Assert.IsNotNull(modal);
                var style = modal.GetAttribute("style");
                Assert.IsTrue(style.EndsWith(", 0.00)"));
            });
        }

        [TestMethod]
        public async Task ModalDialog_should_handle_full_opacity()
        {
            var rendered = _testContext.Render<ModalDialog>(parameters => parameters
                .Add(p => p.OverlayOpacity, 1));

            await rendered.InvokeAsync(async () => { await rendered.Instance.Open(); });
            rendered.Render();

            rendered.WaitForAssertion(() =>
            {
                var modal = rendered.Find(".bmodal");
                Assert.IsNotNull(modal);
                var style = modal.GetAttribute("style");
                Assert.IsTrue(style.EndsWith(", 1.00)"));
            });
        }

        [TestMethod]
        public async Task ModalDialog_should_handle_intermediate_opacity()
        {
            var rendered = _testContext.Render<ModalDialog>(parameters => parameters
                .Add(p => p.OverlayOpacity, 0.75));

            await rendered.InvokeAsync(async () => { await rendered.Instance.Open(); });
            rendered.Render();

            rendered.WaitForAssertion(() =>
            {
                var modal = rendered.Find(".bmodal");
                Assert.IsNotNull(modal);
                var style = modal.GetAttribute("style");
                Assert.IsTrue(style.Contains("0.75"));
            });
        }
    }
}
