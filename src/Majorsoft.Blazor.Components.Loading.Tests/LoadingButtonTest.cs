using System;
using System.Threading.Tasks;

using Bunit;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Majorsoft.Blazor.Components.Loading.Tests
{
	[TestClass]
	public class LoadingButtonTest
	{
		private Bunit.TestContext _testContext;

		[TestInitialize]
		public void Init()
		{
			_testContext = new Bunit.TestContext();

			var mock = new Mock<ILogger<LoadingButton>>();
			_testContext.Services.Add(new ServiceDescriptor(typeof(ILogger<LoadingButton>), mock.Object));
		}

		[TestCleanup]
		public void Cleanup()
		{
			_testContext?.Dispose();
		}

		[TestMethod]
		public void LoadingButton_should_rendered_correctly_html_attributes()
		{
			var rendered = _testContext.RenderComponent<LoadingButton>(
				("id", "id1"), //HTML attributes
				("class", "btn"), //HTML attributes
				(nameof(LoadingButton.Content), (RenderFragment)(builder =>
				{
					builder.AddMarkupContent(1, "hello...");
				}))
				);

			var button = rendered.Find("button");

			Assert.IsNotNull(button);
			button.MarkupMatches("<button blazor:onclick=\"1\" type=\"button\" id=\"id1\" class=\"btn\">hello...</button>");
		}

		[TestMethod]
		public void LoadingButton_should_rendered_correctly_default_state()
		{
			var rendered = _testContext.RenderComponent<LoadingButton>(parameters => parameters
				.Add(p => p.Type, ButtonTypes.Submit)
				.Add(p => p.Content, (RenderFragment)(builder =>
				{
					builder.AddMarkupContent(1, "hello...");
				}))
				.Add(p => p.LoadingContent, (RenderFragment)(builder =>
				{
					builder.AddMarkupContent(1, "loading...");
				}))
				);
			
			var button = rendered.Find("button");

			Assert.IsNotNull(button);
			button.MarkupMatches("<button blazor:onclick=\"1\" type=\"submit\">hello...</button>");
		}

		[TestMethod]
		public void LoadingButton_should_rendered_correctly_loading_state() //Incomplete test for technical reasons...
		{
			IRenderedComponent<LoadingButton> rendered = null;

			rendered = _testContext.RenderComponent<LoadingButton>(parameters => parameters
				.Add(p => p.Type, ButtonTypes.Submit)
				.Add(p => p.Content, (RenderFragment)(builder =>
				{
					builder.AddMarkupContent(1, "hello...");
				}))
				.Add(p => p.LoadingContent, (RenderFragment)(builder =>
				{
					builder.AddMarkupContent(1, "loading...");
				}))
				.Add(p => p.OnClicked, async args => { await CheckClick(); }));

			var button = rendered.Find("button");
			button.Click();

			Assert.IsNotNull(button);

			async Task CheckClick()
			{
				await Task.Delay(200);

				rendered.WaitForAssertion(() =>
					rendered.MarkupMatches("<button blazor:onclick=\"1\" type=\"submit\" disabled=\"\">loading...</button>"), 
					timeout: TimeSpan.FromSeconds(1));
			}
		}

		[TestMethod]
		public void LoadingButton_should_rendered_correctly_loading_state_enabled()
		{
			var rendered = _testContext.RenderComponent<LoadingButton>(parameters => parameters
				.Add(p => p.Type, ButtonTypes.Submit)
				.Add(p => p.DisabledWhenLoading, false)
				.Add(p => p.Content, (RenderFragment)(builder =>
				{
					builder.AddMarkupContent(1, "hello...");
				}))
				.Add(p => p.LoadingContent, (RenderFragment)(builder =>
				{
					builder.AddMarkupContent(1, "loading...");
				})));

			rendered.SetParametersAndRender(parameters => parameters
				.Add(p => p.IsLoading, true));

			rendered.WaitForAssertion(() => rendered.MarkupMatches("<button blazor:onclick=\"1\" type=\"submit\">loading...</button>"), timeout: TimeSpan.FromSeconds(1));
		}

		[TestMethod]
		public void LoadingButton_should_be_loading_state_when_IsLoading_true()
		{
			var rendered = _testContext.RenderComponent<LoadingButton>(parameters => parameters
				.Add(p => p.Type, ButtonTypes.Submit)
				.Add(p => p.Content, (RenderFragment)(builder =>
				{
					builder.AddMarkupContent(1, "hello...");
				}))
				.Add(p => p.LoadingContent, (RenderFragment)(builder =>
				{
					builder.AddMarkupContent(1, "loading...");
				})));

			rendered.SetParametersAndRender(parameters => parameters
				.Add(p => p.IsLoading, true));

			rendered.WaitForAssertion(() => rendered.MarkupMatches("<button blazor:onclick=\"1\" type=\"submit\" disabled=\"\">loading...</button>"), timeout: TimeSpan.FromSeconds(1));
		}

		[TestMethod]
		public void LoadingButton_should_be_default_state_when_IsLoading_false()
		{
			var rendered = _testContext.RenderComponent<LoadingButton>(parameters => parameters
				.Add(p => p.Type, ButtonTypes.Submit)
				.Add(p => p.Content, (RenderFragment)(builder =>
				{
					builder.AddMarkupContent(1, "hello...");
				}))
				.Add(p => p.LoadingContent, (RenderFragment)(builder =>
				{
					builder.AddMarkupContent(1, "loading...");
				})));

			rendered.SetParametersAndRender(parameters => parameters
				.Add(p => p.IsLoading, true));
			rendered.WaitForAssertion(() => rendered.MarkupMatches("<button blazor:onclick=\"1\" type=\"submit\" disabled=\"\">loading...</button>"), timeout: TimeSpan.FromSeconds(2));

			rendered.SetParametersAndRender(parameters => parameters
				.Add(p => p.IsLoading, false));
			rendered.WaitForAssertion(() => rendered.MarkupMatches("<button blazor:onclick=\"1\" type=\"submit\">hello...</button>"), timeout: TimeSpan.FromSeconds(2));
		}
	}
}