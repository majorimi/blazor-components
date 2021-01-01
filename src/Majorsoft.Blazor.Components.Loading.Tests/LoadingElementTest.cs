using Bunit;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using System;
using System.Threading.Tasks;

namespace Majorsoft.Blazor.Components.Loading.Tests
{
	[TestClass]
	public class LoadingElementTest
	{
		private Bunit.TestContext _testContext;

		[TestInitialize]
		public void Init()
		{
			_testContext = new Bunit.TestContext();

			var mock = new Mock<ILogger<LoadingElement>>();
			_testContext.Services.Add(new ServiceDescriptor(typeof(ILogger<LoadingElement>), mock.Object));
		}

		[TestCleanup]
		public void Cleanup()
		{
			_testContext?.Dispose();
		}

		[TestMethod]
		public void LoadingElement_should_not_render_by_default()
		{
			var rendered = _testContext.RenderComponent<LoadingElement>(parameters => parameters
				.Add(p => p.IsLoading, false)
				.Add(p => p.Content, (RenderFragment)(builder =>
					{
						builder.AddMarkupContent(1, "<div>Content</div>");
					}))
				.Add(p => p.LoadingContent, (RenderFragment)(builder =>
					{
						builder.AddMarkupContent(1, "loading...");
					}))
				);

			rendered.MarkupMatches(@"<div>Content</div>");
		}

		[TestMethod]
		public void LoadingElement_should_rendered_correctly_html_attributes()
		{
			var rendered = _testContext.RenderComponent<LoadingElement>(
				("IsLoading", true),
				("id", "id1"), //HTML attributes
				("title", "text"), //HTML attributes
					(nameof(LoadingButton.LoadingContent), (RenderFragment)(builder =>
					{
						builder.AddMarkupContent(1, "loading...");
					}))
				);

			var div = rendered.Find("div");

			Assert.IsNotNull(div);
			rendered.MarkupMatches(@"<div class=""loading"" style=""background-color: rgba(128, 128, 128, 0.9)"" id=""id1"" title=""text""><div class=""loading-content"">loading...</div></div>");

		}

		[TestMethod]
		public void LoadingElement_should_rendered_correctly_rounded_opacity()
		{
			var rendered = _testContext.RenderComponent<LoadingElement>(parameters => parameters
				.Add(p => p.IsLoading, true)
				.Add(p => p.OverlayOpacity, 0.123)
				.Add(p => p.Content, (RenderFragment)(builder =>
					{
						builder.AddMarkupContent(1, "<div>Content</div>");
					}))
				.Add(p => p.LoadingContent, (RenderFragment)(builder =>
					{
						builder.AddMarkupContent(1, "loading...");
					}))
				);

			var div = rendered.Find("div");

			Assert.IsNotNull(div);
			rendered.MarkupMatches(@"<div class=""loading"" style=""background-color: rgba(128, 128, 128, 0.1)""><div class=""loading-content"">loading...</div><div>Content</div></div>");
		}

		[TestMethod]
		public void LoadingElement_should_rendered_correctly_calculated_rgb_background()
		{
			var rendered = _testContext.RenderComponent<LoadingElement>(parameters => parameters
				.Add(p => p.IsLoading, true)
				.Add(p => p.OverlayBackgroundColor, "red")
				.Add(p => p.Content, (RenderFragment)(builder =>
					{
						builder.AddMarkupContent(1, "<div>Content</div>");
					}))
				.Add(p => p.LoadingContent, (RenderFragment)(builder =>
					{
						builder.AddMarkupContent(1, "loading...");
					}))
				);

			var div = rendered.Find("div");

			Assert.IsNotNull(div);
			rendered.MarkupMatches(@"<div class=""loading"" style=""background-color: rgba(255, 0, 0, 0.9)""><div class=""loading-content"">loading...</div><div>Content</div></div>");
		}

		[TestMethod]
		public void LoadingElement_should_rendered_correctly_loading_state()
		{
			IRenderedComponent<LoadingElement> rendered = null;
			rendered = _testContext.RenderComponent<LoadingElement>(parameters => parameters
				.Add(p => p.IsLoading, false)
				.Add(p => p.Content, (RenderFragment)(builder =>
				{
					builder.AddMarkupContent(1, "<div>Content</div>");
				}))
				.Add(p => p.LoadingContent, (RenderFragment)(builder =>
				{
					builder.AddMarkupContent(1, "loading...");
				}))
				.Add(p => p.OnLoading, async args => { await CheckLoading(); }));

			rendered.SetParametersAndRender(parameters => parameters
				.Add(p => p.IsLoading, true));

			var div = rendered.Find("div");
			Assert.IsNotNull(div);

			async Task CheckLoading()
			{
				await Task.Delay(200);

				rendered.WaitForAssertion(() =>
					rendered.MarkupMatches(@"<div class=""loading"" style=""background-color: rgba(128, 128, 128, 0.9)""><div class=""loading-content"">loading...</div><div>Content</div></div>"),
						timeout: TimeSpan.FromSeconds(1));
			}
		}

		[TestMethod]
		public void LoadingElement_should_be_loading_state_when_IsLoading_true()
		{
			var rendered = _testContext.RenderComponent<LoadingElement>(parameters => parameters
				.Add(p => p.IsLoading, false)
				.Add(p => p.Content, (RenderFragment)(builder =>
				{
					builder.AddMarkupContent(1, "<div>Content</div>");
				}))
				.Add(p => p.LoadingContent, (RenderFragment)(builder =>
				{
					builder.AddMarkupContent(1, "loading...");
				})));

			rendered.SetParametersAndRender(parameters => parameters
				.Add(p => p.IsLoading, true));

			rendered.MarkupMatches(@"<div class=""loading"" style=""background-color: rgba(128, 128, 128, 0.9)""><div class=""loading-content"">loading...</div><div>Content</div></div>");
		}

		[TestMethod]
		public void LoadingElement_should_be_default_state_when_IsLoading_false()
		{
			var rendered = _testContext.RenderComponent<LoadingElement>(parameters => parameters
				.Add(p => p.IsLoading, false)
				.Add(p => p.Content, (RenderFragment)(builder =>
				{
					builder.AddMarkupContent(1, "<div>Content</div>");
				}))
				.Add(p => p.LoadingContent, (RenderFragment)(builder =>
				{
					builder.AddMarkupContent(1, "loading...");
				})));

			rendered.SetParametersAndRender(parameters => parameters
				.Add(p => p.IsLoading, true));
			rendered.MarkupMatches(@"<div class=""loading"" style=""background-color: rgba(128, 128, 128, 0.9)""><div class=""loading-content"">loading...</div><div>Content</div></div>");

			rendered.SetParametersAndRender(parameters => parameters
				.Add(p => p.IsLoading, false));
			rendered.MarkupMatches(@"<div>Content</div>");
		}
	}
}