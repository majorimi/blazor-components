using System.Threading.Tasks;

using Majorsoft.Blazor.Components.Common.JsInterop.Focus;
using Majorsoft.Blazor.Components.CssEvents.Transition;

using Bunit;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Majorsoft.Blazor.Components.Modal.Tests
{
	[TestClass]
	public class ModalDialogTest
	{
		private Bunit.TestContext _testContext;
		private Mock<ITransitionEventsService> _transitionMock;
		private Mock<IFocusHandler> _focusHandlerMock;
		
		[TestInitialize]
		public void Init()
		{
			_testContext = new Bunit.TestContext();

			var mock = new Mock<ILogger<ModalDialog>>();
			_transitionMock = new Mock<ITransitionEventsService>();
			_focusHandlerMock = new Mock<IFocusHandler>();

			_testContext.Services.Add(new ServiceDescriptor(typeof(ILogger<ModalDialog>), mock.Object));
			_testContext.Services.Add(new ServiceDescriptor(typeof(ITransitionEventsService), _transitionMock.Object));
			_testContext.Services.Add(new ServiceDescriptor(typeof(IFocusHandler), _focusHandlerMock.Object));
		}

		[TestCleanup]
		public void Cleanup()
		{
			_testContext?.Dispose();
		}

		[TestMethod]
		public void ModalDialog_should_not_rendered_anything_until_opened()
		{
			var rendered = _testContext.RenderComponent<ModalDialog>(
				("id", "id1"), //HTML attributes
				("title", "text"), //HTML attributes
				(nameof(ModalDialog.OverlayOpacity), 0.5)
				);

			Assert.AreEqual(false, rendered.Instance.IsOpen);
			rendered.MarkupMatches("");
		}

		[TestMethod]
		public async Task ModalDialog_should_rendered_correctly_html_attributes_when_opened()
		{
			var rendered = _testContext.RenderComponent<ModalDialog>(
				("id", "id1"), //HTML attributes
				("title", "text") //HTML attributes
				);

			//Open
			await rendered.InvokeAsync(async () => { await rendered.Instance.Open(); });
			rendered.Render();

			var div = rendered.Find("div");
			Assert.IsNotNull(div);
			Assert.AreEqual(true, rendered.Instance.IsOpen);

			rendered.WaitForAssertion(() => rendered.MarkupMatches(@"<div class=""bmodal fade"" style=""opacity: 1; background-color: rgba(128, 128, 128, 0.90)"" id=""id1"" title=""text"">
		  <div class=""bmodal-content dynamicStyle"" tabindex=""100"">
			<div class=""bmodal-header"">
				  <button type = ""button""  class=""close"">
					<span aria-hidden=""true"">&times;</span>
					<span class=""sr-only"">Close</span>
				  </button>
				</div>
				<div class=""bmodal-body""></div>
			  </div>
			</div>
			<style>
				.fade {
					transition: opacity 0.25s linear;
				}
				.dynamicStyle {
					top: calc(15% &#x2B; 0px);
					left: 50%;
					min-width:200px;
					min-height:200px;
					width:auto;
					height:auto;
					transition: top 0.25s ease-in-out;
				}
			</style>"));
		}

		[TestMethod]
		public async Task ModalDialog_should_remove_dialog_from_DOM_when_closed()
		{
			var rendered = _testContext.RenderComponent<ModalDialog>(parameters => parameters
				.Add(p => p.Animate, true));

			Assert.AreEqual(false, rendered.Instance.IsOpen);
			rendered.MarkupMatches("");

			//Open
			await rendered.InvokeAsync(async () => { await rendered.Instance.Open(); });
			rendered.Render();
			Assert.AreEqual(true, rendered.Instance.IsOpen);

			rendered.WaitForAssertion(() => rendered.MarkupMatches(@"<div class=""bmodal fade"" style=""opacity: 1; background-color: rgba(128, 128, 128, 0.90)"">
		  <div class=""bmodal-content dynamicStyle"" tabindex=""100"">
			<div class=""bmodal-header"">
				  <button type = ""button""  class=""close"">
					<span aria-hidden=""true"">&times;</span>
					<span class=""sr-only"">Close</span>
				  </button>
				</div>
				<div class=""bmodal-body""></div>
			  </div>
			</div>
			<style>
				.fade {
					transition: opacity 0.25s linear;
				}
				.dynamicStyle {
					top: calc(15% &#x2B; 0px);
					left: 50%;
					min-width:200px;
					min-height:200px;
					width:auto;
					height:auto;
					transition: top 0.25s ease-in-out;
				}
			</style>"));


			//Close
			rendered.SetParametersAndRender(parameters => parameters
				.Add(p => p.Animate, false));

			await rendered.InvokeAsync(async () => await rendered.Instance.Close());
			rendered.Render();

			Assert.AreEqual(false, rendered.Instance.IsOpen);
			rendered.MarkupMatches("");
		}

		[TestMethod]
		public async Task ModalDialog_should_rendered_background_correctly()
		{
			var rendered = _testContext.RenderComponent<ModalDialog>(parameters => parameters
				.Add(p => p.OverlayBackgroundColor, "red")
				.Add(p => p.OverlayOpacity, 0.25));

			//Open
			await rendered.InvokeAsync(async () => { await rendered.Instance.Open(); });
			rendered.Render();

			var div = rendered.Find("div");
			Assert.IsNotNull(div);
			Assert.AreEqual(true, rendered.Instance.IsOpen);

			rendered.WaitForAssertion(() => rendered.MarkupMatches(@"<div class=""bmodal fade"" style=""opacity: 1; background-color: rgba(255, 0, 0, 0.25)"">
		  <div class=""bmodal-content dynamicStyle"" tabindex=""100"">
			<div class=""bmodal-header"">
				  <button type=""button"" class=""close"">
					<span aria-hidden=""true"">&times;</span>
					<span class=""sr-only"">Close</span>
				  </button>
				</div>
				<div class=""bmodal-body""></div>
			  </div>
			</div>
			<style>
				.fade {
					transition: opacity 0.25s linear;
				}
				.dynamicStyle {
					top: calc(15% &#x2B; 0px);
					left: 50%;
					min-width:200px;
					min-height:200px;
					width:auto;
					height:auto;
					transition: top 0.25s ease-in-out;
				}
			</style>"));
		}

		[TestMethod]
		public async Task ModalDialog_should_rendered_dimensions_correctly()
		{
			var rendered = _testContext.RenderComponent<ModalDialog>(parameters => parameters
				.Add(p => p.Height, 155)
				.Add(p => p.Width, 188)
				.Add(p => p.MinHeight, 999)
				.Add(p => p.MinWidth, 555));

			//Open
			await rendered.InvokeAsync(async () => { await rendered.Instance.Open(); });
			rendered.Render();

			var div = rendered.Find("div");
			Assert.IsNotNull(div);
			Assert.AreEqual(true, rendered.Instance.IsOpen);

			rendered.WaitForAssertion(() => rendered.MarkupMatches(@"<div class=""bmodal fade"" style=""opacity: 1; background-color: rgba(128, 128, 128, 0.90)"">
		  <div class=""bmodal-content dynamicStyle"" tabindex=""100"">
			<div class=""bmodal-header"">
				  <button type=""button"" class=""close"">
					<span aria-hidden=""true"">&times;</span>
					<span class=""sr-only"">Close</span>
				  </button>
				</div>
				<div class=""bmodal-body""></div>
			  </div>
			</div>
			<style>
				.fade {
					transition: opacity 0.25s linear;
				}
				.dynamicStyle {
					top: calc(15% &#x2B; 0px);
					left: 50%;
					min-width:555px;
					min-height:999px;
					width:188px;
					height:155px;
					transition: top 0.25s ease-in-out;
				}
			</style>"));
		}

		[TestMethod]
		public async Task ModalDialog_should_not_rendered_close_button_correctly()
		{
			var rendered = _testContext.RenderComponent<ModalDialog>(parameters => parameters
				.Add(p => p.ShowCloseButton, false));

			//Open
			await rendered.InvokeAsync(async () => { await rendered.Instance.Open(); });
			rendered.Render();

			var div = rendered.Find("div");
			Assert.IsNotNull(div);
			Assert.AreEqual(true, rendered.Instance.IsOpen);

			rendered.WaitForAssertion(() => rendered.MarkupMatches(@"<div class=""bmodal fade"" style=""opacity: 1; background-color: rgba(128, 128, 128, 0.90)"">
		  <div class=""bmodal-content dynamicStyle"" tabindex=""100"">
				<div class=""bmodal-body""></div>
			  </div>
			</div>
			<style>
				.fade {
					transition: opacity 0.25s linear;
				}
				.dynamicStyle {
					top: calc(15% &#x2B; 0px);
					left: 50%;
					min-width:200px;
					min-height:200px;
					width:auto;
					height:auto;
					transition: top 0.25s ease-in-out;
				}
			</style>"));
		}

		[TestMethod]
		public async Task ModalDialog_should_rendered_dialog_centered_correctly()
		{
			var rendered = _testContext.RenderComponent<ModalDialog>(parameters => parameters
				.Add(p => p.Centered, true));

			//Open
			await rendered.InvokeAsync(async () => { await rendered.Instance.Open(); });
			rendered.Render();

			var div = rendered.Find("div");
			Assert.IsNotNull(div);
			Assert.AreEqual(true, rendered.Instance.IsOpen);

			rendered.WaitForAssertion(() => rendered.MarkupMatches(@"<div class=""bmodal fade"" style=""opacity: 1; background-color: rgba(128, 128, 128, 0.90)"">
		  <div class=""bmodal-content dynamicStyle"" tabindex=""100"">
			<div class=""bmodal-header"">
				  <button type=""button"" class=""close"">
					<span aria-hidden=""true"">&times;</span>
					<span class=""sr-only"">Close</span>
				  </button>
				</div>
				<div class=""bmodal-body""></div>
			  </div>
			</div>
			<style>
				.fade {
					transition: opacity 0.25s linear;
				}
				.dynamicStyle {
					top: 50%;
					left: 50%;
					min-width:200px;
					min-height:200px;
					width:auto;
					height:auto;
					transition: top 0.25s ease-in-out;
				}
			</style>"));
		}

		[TestMethod]
		public async Task ModalDialog_should_not_rendered_close_button_but_render_haeder_correctly()
		{
			var rendered = _testContext.RenderComponent<ModalDialog>(parameters => parameters
				.Add(p => p.ShowCloseButton, false)
				.Add(p => p.Header, (RenderFragment)(builder =>
					{
						builder.AddMarkupContent(1, "Header...");
					}))
				);

			//Open
			await rendered.InvokeAsync(async () => { await rendered.Instance.Open(); });
			rendered.Render();

			var div = rendered.Find("div");
			Assert.IsNotNull(div);
			Assert.AreEqual(true, rendered.Instance.IsOpen);

			rendered.WaitForAssertion(() => rendered.MarkupMatches(@"<div class=""bmodal fade"" style=""opacity: 1; background-color: rgba(128, 128, 128, 0.90)"">
		  <div class=""bmodal-content dynamicStyle"" tabindex=""100"">
				<div class=""bmodal-header"">Header...</div>
				<div class=""bmodal-body""></div>
			  </div>
			</div>
			<style>
				.fade {
					transition: opacity 0.25s linear;
				}
				.dynamicStyle {
					top: calc(15% &#x2B; 0px);
					left: 50%;
					min-width:200px;
					min-height:200px;
					width:auto;
					height:auto;
					transition: top 0.25s ease-in-out;
				}
			</style>"));
		}

		[TestMethod]
		public async Task ModalDialog_should_rendered_close_button_with_haeder_correctly()
		{
			var rendered = _testContext.RenderComponent<ModalDialog>(parameters => parameters
				.Add(p => p.ShowCloseButton, true)
				.Add(p => p.Header, (RenderFragment)(builder =>
				{
					builder.AddMarkupContent(1, "Header...");
				}))
				);

			//Open
			await rendered.InvokeAsync(async () => { await rendered.Instance.Open(); });
			rendered.Render();

			var div = rendered.Find("div");
			Assert.IsNotNull(div);
			Assert.AreEqual(true, rendered.Instance.IsOpen);

			rendered.WaitForAssertion(() => rendered.MarkupMatches(@"<div class=""bmodal fade"" style=""opacity: 1; background-color: rgba(128, 128, 128, 0.90)"">
		  <div class=""bmodal-content dynamicStyle"" tabindex=""100"">
				<div class=""bmodal-header"">Header...<button type=""button"" class=""close"">
				<span aria-hidden=""true"">&times;</span>
				<span class=""sr-only"">Close</span>
			  </button></div>
				<div class=""bmodal-body""></div>
			  </div>
			</div>
			<style>
				.fade {
					transition: opacity 0.25s linear;
				}
				.dynamicStyle {
					top: calc(15% &#x2B; 0px);
					left: 50%;
					min-width:200px;
					min-height:200px;
					width:auto;
					height:auto;
					transition: top 0.25s ease-in-out;
				}
			</style>"));
		}

		[TestMethod]
		public async Task ModalDialog_should_rendered_content_correctly()
		{
			var rendered = _testContext.RenderComponent<ModalDialog>(parameters => parameters
				.Add(p => p.ShowCloseButton, false)
				.Add(p => p.Content, (RenderFragment)(builder =>
					{
						builder.AddMarkupContent(1, "Dialog content...");
					}))
				);

			//Open
			await rendered.InvokeAsync(async () => { await rendered.Instance.Open(); });
			rendered.Render();

			var div = rendered.Find("div");
			Assert.IsNotNull(div);
			Assert.AreEqual(true, rendered.Instance.IsOpen);

			rendered.WaitForAssertion(() => rendered.MarkupMatches(@"<div class=""bmodal fade"" style=""opacity: 1; background-color: rgba(128, 128, 128, 0.90)"">
		  <div class=""bmodal-content dynamicStyle"" tabindex=""100"">
				<div class=""bmodal-body"">Dialog content...</div>
			  </div>
			</div>
			<style>
				.fade {
					transition: opacity 0.25s linear;
				}
				.dynamicStyle {
					top: calc(15% &#x2B; 0px);
					left: 50%;
					min-width:200px;
					min-height:200px;
					width:auto;
					height:auto;
					transition: top 0.25s ease-in-out;
				}
			</style>"));
		}

		[TestMethod]
		public async Task ModalDialog_should_rendered_footer_correctly()
		{
			var rendered = _testContext.RenderComponent<ModalDialog>(parameters => parameters
				.Add(p => p.ShowCloseButton, false)
				.Add(p => p.Footer, (RenderFragment)(builder =>
				{
					builder.AddMarkupContent(1, "Dialog footer...");
				}))
				);

			//Open
			await rendered.InvokeAsync(async () => { await rendered.Instance.Open(); });
			rendered.Render();

			var div = rendered.Find("div");
			Assert.IsNotNull(div);
			Assert.AreEqual(true, rendered.Instance.IsOpen);

			rendered.WaitForAssertion(() => rendered.MarkupMatches(@"<div class=""bmodal fade"" style=""opacity: 1; background-color: rgba(128, 128, 128, 0.90)"">
		  <div class=""bmodal-content dynamicStyle"" tabindex=""100"">
				<div class=""bmodal-body""></div>
				<div class=""bmodal-footer"">Dialog footer...</div>
			  </div>
			</div>
			<style>
				.fade {
					transition: opacity 0.25s linear;
				}
				.dynamicStyle {
					top: calc(15% &#x2B; 0px);
					left: 50%;
					min-width:200px;
					min-height:200px;
					width:auto;
					height:auto;
					transition: top 0.25s ease-in-out;
				}
			</style>"));
		}

		[TestMethod]
		public async Task ModalDialog_should_close_on_overlay_click()
		{
			var closed = false;
			var rendered = _testContext.RenderComponent<ModalDialog>(parameters => parameters
			.Add(p => p.CloseOnOverlayClick, true)
			.Add(p => p.OnClose, args => { closed = true; }));

			//Open
			await rendered.InvokeAsync(async () => { await rendered.Instance.Open(); });
			rendered.Render();

			var div = rendered.Find("div");
			Assert.IsNotNull(div);
			Assert.AreEqual(true, rendered.Instance.IsOpen);

			//tr to close
			rendered.SetParametersAndRender(parameters => parameters
				.Add(p => p.Animate, false));

			try
			{
				div.Click();
			}
			catch //DEV click will remove elements from DOM and it causes exception!
			{ }
			rendered.Render();

			Assert.AreEqual(false, rendered.Instance.IsOpen);
			Assert.AreEqual(true, closed);
			rendered.WaitForAssertion(() => rendered.MarkupMatches(""));
		}

		[TestMethod]
		public async Task ModalDialog_should_not_close_on_overlay_click()
		{
			var closed = false;
			var rendered = _testContext.RenderComponent<ModalDialog>(parameters => parameters
			.Add(p => p.CloseOnOverlayClick, false)
			.Add(p => p.OnClose, args => { closed = true; }));

			//Open
			await rendered.InvokeAsync(async () => { await rendered.Instance.Open(); });
			rendered.Render();

			var div = rendered.Find("div");
			Assert.IsNotNull(div);
			Assert.AreEqual(true, rendered.Instance.IsOpen);

			//tr to close
			rendered.SetParametersAndRender(parameters => parameters
				.Add(p => p.Animate, false));

			div.Click();
			rendered.Render();

			rendered.WaitForAssertion(() => rendered.MarkupMatches(@"<div class=""bmodal"" style=""opacity: 1; background-color: rgba(128, 128, 128, 0.90)"">
		  <div class=""bmodal-content dynamicStyle"" tabindex=""100"">
			<div class=""bmodal-header"">
				  <button type = ""button""  class=""close"">
					<span aria-hidden=""true"">&times;</span>
					<span class=""sr-only"">Close</span>
				  </button>
				</div>
				<div class=""bmodal-body""></div>
			  </div>
			</div>
			<style>
				.fade {
					transition: opacity 0.25s linear;
				}
				.dynamicStyle {
					top: calc(15% &#x2B; 0px);
					left: 50%;
					min-width:200px;
					min-height:200px;
					width:auto;
					height:auto;
					transition: top 0.25s ease-in-out;
				}
			</style>"));
		}

		[TestMethod]
		public async Task ModalDialog_should_close_on_escape_key()
		{
			var closed = false;
			var rendered = _testContext.RenderComponent<ModalDialog>(parameters => parameters
			.Add(p => p.CloseOnEscapeKey, true)
			.Add(p => p.OnClose, args => { closed = true; }));

			//Open
			await rendered.InvokeAsync(async () => { await rendered.Instance.Open(); });
			rendered.Render();

			var div = rendered.Find("div");
			Assert.IsNotNull(div);
			Assert.AreEqual(true, rendered.Instance.IsOpen);

			//tr to close
			rendered.SetParametersAndRender(parameters => parameters
				.Add(p => p.Animate, false));

			try
			{
				div.KeyUp(Key.Escape);
			}
			catch //DEV click will remove elements from DOM and it causes exception!
			{ }
			rendered.Render();

			Assert.AreEqual(false, rendered.Instance.IsOpen);
			Assert.AreEqual(true, closed);
			rendered.WaitForAssertion(() => rendered.MarkupMatches(""));
		}

		[TestMethod]
		public async Task ModalDialog_should_not_close_on_escape_key()
		{
			var closed = false;
			var rendered = _testContext.RenderComponent<ModalDialog>(parameters => parameters
			.Add(p => p.CloseOnEscapeKey, false)
			.Add(p => p.OnClose, args => { closed = true; }));

			//Open
			await rendered.InvokeAsync(async () => { await rendered.Instance.Open(); });
			rendered.Render();

			var div = rendered.Find("div");
			Assert.IsNotNull(div);
			Assert.AreEqual(true, rendered.Instance.IsOpen);

			//tr to close
			rendered.SetParametersAndRender(parameters => parameters
				.Add(p => p.Animate, false));

			div.KeyUp(Key.Escape);
			rendered.Render();

			rendered.WaitForAssertion(() => rendered.MarkupMatches(@"<div class=""bmodal"" style=""opacity: 1; background-color: rgba(128, 128, 128, 0.90)"">
		  <div class=""bmodal-content dynamicStyle"" tabindex=""100"">
			<div class=""bmodal-header"">
				  <button type = ""button""  class=""close"">
					<span aria-hidden=""true"">&times;</span>
					<span class=""sr-only"">Close</span>
				  </button>
				</div>
				<div class=""bmodal-body""></div>
			  </div>
			</div>
			<style>
				.fade {
					transition: opacity 0.25s linear;
				}
				.dynamicStyle {
					top: calc(15% &#x2B; 0px);
					left: 50%;
					min-width:200px;
					min-height:200px;
					width:auto;
					height:auto;
					transition: top 0.25s ease-in-out;
				}
			</style>"));
		}
	}
}