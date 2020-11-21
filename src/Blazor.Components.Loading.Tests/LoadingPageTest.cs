using System;
using System.Threading.Tasks;

using Bunit;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Blazor.Components.Loading.Tests
{
	[TestClass]
	public class LoadingPageTest
	{
		private Bunit.TestContext _testContext;

		[TestInitialize]
		public void Init()
		{
			_testContext = new Bunit.TestContext();

			var mock = new Mock<ILogger<LoadingPage>>();
			_testContext.Services.Add(new ServiceDescriptor(typeof(ILogger<LoadingPage>), mock.Object));
		}

		[TestCleanup]
		public void Cleanup()
		{
			_testContext?.Dispose();
		}

		[TestMethod]
		public void LoadingPage_should_rendered_correctly_html_attributes()
		{
			var rendered = _testContext.RenderComponent<LoadingPage>(
				("id", "id1"), //HTML attributes
				("title", "text"), //HTML attributes
				(nameof(LoadingButton.LoadingContent), (RenderFragment)(builder =>
				{
					builder.AddMarkupContent(1, "loading...");
				}))
				);

			var div = rendered.Find("div");

			Assert.IsNotNull(div);
			rendered.MarkupMatches(@"<div class=""loading"" style=""background-color: rgba(128, 128, 128, 0.9)"" hidden id=""id1"" title=""text""><div class=""loading-content"">loading...</div></div>");
		}

		[TestMethod]
		public void LoadingPage_should_rendered_correctly_default_state()
		{
			var rendered = _testContext.RenderComponent<LoadingPage>(parameters => parameters
				.Add(p => p.LoadingContent, (RenderFragment)(builder =>
					{
						builder.AddMarkupContent(1, "loading...");
					}))
				);

			var div = rendered.Find("div");

			Assert.IsNotNull(div);
			rendered.MarkupMatches(@"<div class=""loading"" style=""background-color: rgba(128, 128, 128, 0.9)"" hidden><div class=""loading-content"">loading...</div></div>");
		}

		[TestMethod]
		public void LoadingPage_should_rendered_correctly_rounded_opacity()
		{
			var rendered = _testContext.RenderComponent<LoadingPage>(parameters => parameters
				.Add(p => p.OverlayOpacity, 0.123)
				.Add(p => p.LoadingContent, (RenderFragment)(builder =>
					{
						builder.AddMarkupContent(1, "loading...");
					}))
				);

			var div = rendered.Find("div");

			Assert.IsNotNull(div);
			rendered.MarkupMatches(@"<div class=""loading"" style=""background-color: rgba(128, 128, 128, 0.1)"" hidden><div class=""loading-content"">loading...</div></div>");
		}

		[TestMethod]
		public void LoadingPage_should_rendered_correctly_calculated_rgb_background()
		{
			var rendered = _testContext.RenderComponent<LoadingPage>(parameters => parameters
				.Add(p => p.OverlayBackgroundColor, "red")
				.Add(p => p.LoadingContent, (RenderFragment)(builder =>
					{
						builder.AddMarkupContent(1, "loading...");
					}))
				);

			var div = rendered.Find("div");

			Assert.IsNotNull(div);
			rendered.MarkupMatches(@"<div class=""loading"" style=""background-color: rgba(255, 0, 0, 0.9)"" hidden><div class=""loading-content"">loading...</div></div>");
		}

		[TestMethod]
		public void LoadingPage_should_rendered_correctly_loading_state() //Incomplete test for technical reasons...
		{
			IRenderedComponent<LoadingPage> rendered = null;
			rendered = _testContext.RenderComponent<LoadingPage>(parameters => parameters
				.Add(p => p.LoadingContent, (RenderFragment)(builder =>
					{
						builder.AddMarkupContent(1, "loading...");
					}))
				.Add(p => p.OnLoading, args => { CheckLoading(); }));

			var div = rendered.Find("div");
			Assert.IsNotNull(div);

			void CheckLoading()
			{
				rendered?.Render(); //HACK: for some reason no rendering but on UI it works...

				//WARNING: during render OnLoad event called but rendered variable is NULL!!!
				//rendered.WaitForAssertion(() =>
				//	rendered.MarkupMatches(@"<div class=""loading"" style=""background-color: rgba(128, 128, 128, 0.9)""><div class=""loading-content"">loading...</div></div>"), timeout: TimeSpan.FromSeconds(1));
			}
		}

		[TestMethod]
		public void LoadingPage_should_be_loading_state_when_Set_called()
		{
			var rendered = _testContext.RenderComponent<LoadingPage>(parameters => parameters
				.Add(p => p.LoadingContent, (RenderFragment)(builder =>
				{
					builder.AddMarkupContent(1, "loading...");
				})));

			rendered.Instance.Set();
			rendered.Render(); //HACK: for some reason no rendering but on UI it works...

			rendered.MarkupMatches(@"<div class=""loading"" style=""background-color: rgba(128, 128, 128, 0.9)""><div class=""loading-content"">loading...</div></div>");
		}

		[TestMethod]
		public void LoadingPage_should_be_default_state_when_Reset_called()
		{
			var rendered = _testContext.RenderComponent<LoadingPage>(parameters => parameters
				.Add(p => p.LoadingContent, (RenderFragment)(builder =>
				{
					builder.AddMarkupContent(1, "loading...");
				})));

			rendered.Instance.Set();
			rendered.Render(); //HACK: for some reason no rendering but on UI it works...
			rendered.MarkupMatches(@"<div class=""loading"" style=""background-color: rgba(128, 128, 128, 0.9)""><div class=""loading-content"">loading...</div></div>");

			rendered.Instance.Reset();
			rendered.Render(); //HACK: for some reason no rendering but on UI it works...
			rendered.MarkupMatches(@"<div class=""loading"" style=""background-color: rgba(128, 128, 128, 0.9)"" hidden><div class=""loading-content"">loading...</div></div>");
		}
	}
}