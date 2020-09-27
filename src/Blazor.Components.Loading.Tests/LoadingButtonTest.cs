using Blazor.Components.Loading;

using Bunit;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Blazor.Components.Loading.Tests
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
		public void LoadingButton_should_rendered_correctly_default_state()
		{
			var rendered = _testContext.RenderComponent<LoadingButton>(
				(nameof(LoadingButton.Type), ButtonTypes.Submit),
				("id", "id1"), //HTML attributes
				(nameof(LoadingButton.Content), (RenderFragment)(builder =>
					{
						builder.AddMarkupContent(1, "hello...");
					})),
				(nameof(LoadingButton.LoadingContent), (RenderFragment)(builder =>
					{
						builder.AddMarkupContent(1, "loading...");
					}))
				);
			
			var button = rendered.Find("button");

			Assert.IsNotNull(button);
			button.MarkupMatches("<button blazor:onclick=\"1\" type=\"submit\" id=\"id1\">hello...</button>");
		}

		[TestMethod]
		public void LoadingButton_should_rendered_correctly_loading_state()
		{
			var rendered = _testContext.RenderComponent<LoadingButton>(
				(nameof(LoadingButton.Type), ButtonTypes.Submit),
				("id", "id1"), //HTML attributes
				(nameof(LoadingButton.Content), (RenderFragment)(builder =>
				{
					builder.AddMarkupContent(1, "hello...");
				})),
				(nameof(LoadingButton.LoadingContent), (RenderFragment)(builder =>
				{
					builder.AddMarkupContent(1, "loading...");
				}))
				//EventCallback(nameof(LoadingButton.OnClicked), (MouseEventArgs args) => { /* handle callback */ })
				);

			var button = rendered.Find("button");
			button.Click();

			//TODO: finish it when events are working..
			Assert.IsNotNull(button);
			button.MarkupMatches("<button blazor:onclick=\"1\" type=\"submit\" id=\"id1\">hello...</button>");
		}
	}
}