using System.Threading.Tasks;

using Majorsoft.Blazor.Components.CssEvents.Transition;

using Bunit;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using Majorsoft.Blazor.Components.Timer;
using System;
using Majorsoft.Blazor.Components.CommonTestsBase;

namespace Majorsoft.Blazor.Components.Notifications.Tests.Toasts
{
	[TestClass]
	public class ToastTest : ComponentsTestBase<Toast>
	{
		private Mock<ITransitionEventsService> _transitionMock;
		private Mock<IToastService> _toastServiceMock;

		[TestInitialize]
		public void Init()
		{
			var logger = new Mock<ILogger<AdvancedTimer>>();
			_transitionMock = new Mock<ITransitionEventsService>();
			_toastServiceMock = new Mock<IToastService>();

			_toastServiceMock.SetupGet(g => g.GlobalSettings).Returns(new ToastContainerGlobalSettings() { });

			_testContext.Services.Add(new ServiceDescriptor(typeof(ILogger<AdvancedTimer>), logger.Object));
			_testContext.Services.Add(new ServiceDescriptor(typeof(ITransitionEventsService), _transitionMock.Object));
			_testContext.Services.Add(new ServiceDescriptor(typeof(IToastService), _toastServiceMock.Object));
		}

		[TestMethod]
		public void Toast_should_not_rendered_html_when_not_IsVisible()
		{
			var rendered = _testContext.RenderComponent<Toast>(parameters => parameters
				.Add(p => p.Settings, new ToastSettings() { IsVisible = false }));

			Assert.AreEqual(false, rendered.Instance.Settings.IsVisible);

			rendered.WaitForAssertion(() => rendered.MarkupMatches(@""));
		}

		[TestMethod]
		public void Toast_should_rendered_correctly_html_when_IsVisible()
		{
			var rendered = _testContext.RenderComponent<Toast>(parameters => parameters
				.Add(p => p.Settings, new ToastSettings() { IsVisible = true }));

			var div = rendered.Find("div");
			Assert.IsNotNull(div);
			Assert.AreEqual(true, rendered.Instance.Settings.IsVisible);

			rendered.WaitForAssertion(() => rendered.MarkupMatches(@"<div class=""btoast-main bnotify-normal-primary"" style=""opacity: 1;
					margin-bottom: 17px;
					box-shadow: 1px 5px 20px 0px #c7c7c7;"" tabindex=""750""  >
			  <div class=""btoast-body"" >
				<div >
				  <svg class=""btoast-img"" focusable=""false"" viewBox=""0 0 24 24"" aria-hidden=""true"" >
					<path fill-rule=""evenodd"" clip-rule=""evenodd"" d=""m18.598 2.865c0-0.552-0.447-1-1-1s-1 0.448-1 1v2c0 0.552 0.447 1 1 1s1-0.448 1-1v-2zm-12 2c0 0.552-0.447 1-1 1s-1-0.448-1-1v-2c0-0.552 0.447-1 1-1s1 0.448 1 1v2zm13 5v10h-16v-10h16zm2-6h-2v1c0 1.103-0.897 2-2 2s-2-0.897-2-2v-1h-8v1c0 1.103-0.897 2-2 2s-2-0.897-2-2v-1h-2v18h20v-18zm-13 10h-2v-2h2v2zm4 0h-2v-2h2v2zm4 0h-2v-2h2v2zm-8 4h-2v-2h2v2zm4 0h-2v-2h2v2zm4 0h-2v-2h2v2z"" ></path>
				  </svg>
				</div>
				<div class=""btoast-text"" ></div>
				<button type=""button""  class=""close normal"" >
				  <span aria-hidden=""true"" >×</span>
				  <span class=""sr-only"" >Close</span>
				</button>
			  </div>
			  <div class=""btoast-progress primary start"" style=""transition: width 10s linear;"" ></div>
			</div>"));
		}

		[TestMethod]
		public void Toast_should_not_render_ShowIcon_false()
		{
			var rendered = _testContext.RenderComponent<Toast>(parameters => parameters
				.Add(p => p.Settings, new ToastSettings() 
				{ 
					IsVisible = true,
					ShowIcon = false
				}));

			var div = rendered.Find("div");
			Assert.IsNotNull(div);
			Assert.AreEqual(true, rendered.Instance.Settings.IsVisible);

			rendered.WaitForAssertion(() => rendered.MarkupMatches(@"<div class=""btoast-main bnotify-normal-primary"" style=""opacity: 1;
					margin-bottom: 17px;
					box-shadow: 1px 5px 20px 0px #c7c7c7;"" tabindex=""750""  >
			  <div class=""btoast-body"" >
				<div class=""btoast-text"" ></div>
				<button type=""button""  class=""close normal"" >
				  <span aria-hidden=""true"" >×</span>
				  <span class=""sr-only"" >Close</span>
				</button>
			  </div>
			  <div class=""btoast-progress primary start"" style=""transition: width 10s linear;"" ></div>
			</div>"));
		}

		[TestMethod]
		public void Toast_should_not_render_ShowCloseButton_false()
		{
			var rendered = _testContext.RenderComponent<Toast>(parameters => parameters
				.Add(p => p.Settings, new ToastSettings() 
				{ 
					IsVisible = true,
					ShowIcon = false,
					ShowCloseButton = false
				}));

			var div = rendered.Find("div");
			Assert.IsNotNull(div);
			Assert.AreEqual(true, rendered.Instance.Settings.IsVisible);

			rendered.WaitForAssertion(() => rendered.MarkupMatches(@"<div class=""btoast-main bnotify-normal-primary"" style=""opacity: 1;
					margin-bottom: 17px;
					box-shadow: 1px 5px 20px 0px #c7c7c7;"" tabindex=""750""  >
			  <div class=""btoast-body"" >
				<div class=""btoast-text"" ></div>
			  </div>
			  <div class=""btoast-progress primary start"" style=""transition: width 10s linear;"" ></div>
			</div>"));
		}

		[TestMethod]
		public void Toast_should_render_Content()
		{
			var rendered = _testContext.RenderComponent<Toast>(parameters => parameters
				.Add(p => p.Settings, new ToastSettings()
				{
					IsVisible = true,
					ShowIcon = false,
					ShowCloseButton = false,
					Content = b => b.AddMarkupContent(0, @"<strong>Hi..</strong>")
				}));

			var div = rendered.Find("div");
			Assert.IsNotNull(div);

			rendered.WaitForAssertion(() => rendered.MarkupMatches(@"<div class=""btoast-main bnotify-normal-primary"" style=""opacity: 1;
					margin-bottom: 17px;
					box-shadow: 1px 5px 20px 0px #c7c7c7;"" tabindex=""750""  >
			  <div class=""btoast-body"" >
				<div class=""btoast-text"" >
					<strong>Hi..</strong>
				</div>
			  </div>
			  <div class=""btoast-progress primary start"" style=""transition: width 10s linear;"" ></div>
			</div>"));
		}

		[TestMethod]
		public void Toast_should_render_CustomIconSvgPath()
		{
			var rendered = _testContext.RenderComponent<Toast>(parameters => parameters
				.Add(p => p.Settings, new ToastSettings()
				{
					IsVisible = true,
					ShowIcon = true,
					ShowCloseButton = false,
					Content = b => b.AddMarkupContent(0, @"<strong>Hi..</strong>"),
					CustomIconSvgPath = "svg path value"
				}));

			var div = rendered.Find("div");
			Assert.IsNotNull(div);

			rendered.WaitForAssertion(() => rendered.MarkupMatches(@"<div class=""btoast-main bnotify-normal-primary"" style=""opacity: 1;
					margin-bottom: 17px;
					box-shadow: 1px 5px 20px 0px #c7c7c7;"" tabindex=""750""  >
			  <div class=""btoast-body"" >
				<div >
				  <svg class=""btoast-img"" focusable=""false"" viewBox=""0 0 24 24"" aria-hidden=""true"" >
					<path fill-rule=""evenodd"" clip-rule=""evenodd"" d=""svg path value"" ></path>
				  </svg>
				</div>
				<div class=""btoast-text"" >
					<strong>Hi..</strong>
				</div>
			  </div>
			  <div class=""btoast-progress primary start"" style=""transition: width 10s linear;"" ></div>
			</div>"));
		}

		[TestMethod]
		public void Toast_should_render_Types()
		{
			var rendered = _testContext.RenderComponent<Toast>(parameters => parameters
				.Add(p => p.Settings, new ToastSettings()
				{
					IsVisible = true,
					ShowIcon = false,
					ShowCloseButton = false
				}));

			var div = rendered.Find("div");
			Assert.IsNotNull(div);

			foreach (var types in Enum.GetValues<NotificationTypes>())
			{
				rendered.SetParametersAndRender(parameters => parameters.Add(p => p.Settings, new ToastSettings()
				{
					IsVisible = true,
					ShowIcon = false,
					ShowCloseButton = false,
					Type = types
				}));

				rendered.WaitForAssertion(() => rendered.MarkupMatches($@"<div class=""btoast-main bnotify-normal-{types.ToString().ToLower()}"" style=""opacity: 1;
					margin-bottom: 17px;
					box-shadow: 1px 5px 20px 0px #c7c7c7;"" tabindex=""750""  >
			  <div class=""btoast-body"" >
				<div class=""btoast-text"" ></div>
			  </div>
			  <div class=""btoast-progress {types.ToString().ToLower()} start"" style=""transition: width 10s linear;"" ></div>
			</div>"));
			}
		}

		[TestMethod]
		public void Toast_should_render_NotificationStyles()
		{
			var rendered = _testContext.RenderComponent<Toast>(parameters => parameters
				.Add(p => p.Settings, new ToastSettings()
				{
					IsVisible = true,
					ShowIcon = false,
					ShowCloseButton = false
				}));

			var div = rendered.Find("div");
			Assert.IsNotNull(div);

			foreach (var types in Enum.GetValues<NotificationTypes>())
			{
				rendered.SetParametersAndRender(parameters => parameters.Add(p => p.Settings, new ToastSettings()
				{
					IsVisible = true,
					ShowIcon = false,
					ShowCloseButton = false,
					Type = types
				}));

				foreach (var style in Enum.GetValues<NotificationStyles>())
				{
					rendered.SetParametersAndRender(parameters => parameters.Add(p => p.Settings, new ToastSettings()
					{
						IsVisible = true,
						ShowIcon = false,
						ShowCloseButton = false,
						Type = types,
						NotificationStyle = style,
					}));

					var progress = style != NotificationStyles.Strong ? $" { types.ToString().ToLower()}" : " strong";

					rendered.WaitForAssertion(() => rendered.MarkupMatches($@"<div class=""btoast-main bnotify-{style.ToString().ToLower()}-{types.ToString().ToLower()}"" style=""opacity: 1;
					margin-bottom: 17px;
					box-shadow: 1px 5px 20px 0px #c7c7c7;"" tabindex=""750""  >
				  <div class=""btoast-body"" >
					<div class=""btoast-text"" ></div>
				  </div>
				  <div class=""btoast-progress {progress} start"" style=""transition: width 10s linear;"" ></div>
				</div>"));
				}
			}
		}

		[TestMethod]
		public void Toast_should_not_render_ShowCloseCountdownProgress_false()
		{
			var rendered = _testContext.RenderComponent<Toast>(parameters => parameters
				.Add(p => p.Settings, new ToastSettings()
				{
					IsVisible = true,
					ShowIcon = false,
					ShowCloseCountdownProgress = false
				}));

			var div = rendered.Find("div");
			Assert.IsNotNull(div);
			Assert.AreEqual(true, rendered.Instance.Settings.IsVisible);

			rendered.WaitForAssertion(() => rendered.MarkupMatches(@"<div class=""btoast-main bnotify-normal-primary"" style=""opacity: 1;
					margin-bottom: 17px;
					box-shadow: 1px 5px 20px 0px #c7c7c7;"" tabindex=""750""  >
			  <div class=""btoast-body"" >
				<div class=""btoast-text"" ></div>
				<button type=""button""  class=""close normal"" >
				  <span aria-hidden=""true"" >×</span>
				  <span class=""sr-only"" >Close</span>
				</button>
			  </div>
			</div>"));
		}

		[TestMethod]
		public void Toast_should_render_ShadowEffect()
		{
			var rendered = _testContext.RenderComponent<Toast>(parameters => parameters
				.Add(p => p.Settings, new ToastSettings()
				{
					IsVisible = true,
					ShowIcon = false,
					ShowCloseCountdownProgress = false,
					ShadowEffect = 15,
				}));

			var div = rendered.Find("div");
			Assert.IsNotNull(div);
			Assert.AreEqual(true, rendered.Instance.Settings.IsVisible);

			rendered.WaitForAssertion(() => rendered.MarkupMatches(@"<div class=""btoast-main bnotify-normal-primary"" style=""opacity: 1;
					margin-bottom: 27px;
					box-shadow: 3px 15px 20px 1px #c7c7c7;"" tabindex=""750""  >
			  <div class=""btoast-body"" >
				<div class=""btoast-text"" ></div>
				<button type=""button""  class=""close normal"" >
				  <span aria-hidden=""true"" >×</span>
				  <span class=""sr-only"" >Close</span>
				</button>
			  </div>
			</div>"));
		}

		[TestMethod]
		public void Toast_should_AutoClose()
		{
			_transitionMock.Setup(s => s.RegisterTransitionEndedAsync(It.IsAny<ElementReference>(), It.IsAny<Func<TransitionEventArgs, Task>>(), It.IsAny<string>()))
				.Callback<ElementReference, Func<TransitionEventArgs, Task>, string>((element, func, name) => 
				{
					func.Invoke(new TransitionEventArgs());
				})
				.Returns(Task.CompletedTask);

			var rendered = _testContext.RenderComponent<Toast>(parameters => parameters
				.Add(p => p.Settings, new ToastSettings()
				{
					IsVisible = true,
					ShowIcon = false,
					ShowCloseButton = false,
					Content = b => b.AddMarkupContent(0, @"<strong>Hi..</strong>"),
					AutoCloseInSec = 1
				}));

			var div = rendered.Find("div");
			Assert.IsNotNull(div);

			rendered.WaitForAssertion(() => rendered.MarkupMatches(@"<div class=""btoast-main bnotify-normal-primary"" style=""opacity: 1;
					margin-bottom: 17px;
					box-shadow: 1px 5px 20px 0px #c7c7c7;"" tabindex=""750""  >
			  <div class=""btoast-body"" >
				<div class=""btoast-text"" >
					<strong>Hi..</strong>
				</div>
			  </div>
			  <div class=""btoast-progress primary start"" style=""transition: width 1s linear;"" ></div>
			</div>"), TimeSpan.FromSeconds(1));

			rendered.Render();

			rendered.WaitForAssertion(() =>
			{
				rendered.MarkupMatches("");
			}, TimeSpan.FromSeconds(2));
		}
	}
}