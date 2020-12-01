using Blazor.Components.Common.JsInterop.Click;
using Blazor.Components.Debounce;

using Bunit;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Blazor.Components.Typeahead.Tests
{
	[TestClass]
	public class TypeaheadInputText
	{
		private Bunit.TestContext _testContext;
		private Mock<IClickBoundariesHandler> _clickBoundariesMock;

		[TestInitialize]
		public void Init()
		{
			_testContext = new Bunit.TestContext();

			var mock = new Mock<ILogger<TypeaheadInput<string>>>();
			var mock2 = new Mock<ILogger<DebounceInput>>();
			_clickBoundariesMock = new Mock<IClickBoundariesHandler>();

			_testContext.Services.Add(new ServiceDescriptor(typeof(ILogger<TypeaheadInput<string>>), mock.Object));
			_testContext.Services.Add(new ServiceDescriptor(typeof(ILogger<DebounceInput>), mock2.Object));
			_testContext.Services.Add(new ServiceDescriptor(typeof(IClickBoundariesHandler), _clickBoundariesMock.Object));
		}

		[TestCleanup]
		public void Cleanup()
		{
			_testContext?.Dispose();
		}

		[TestMethod]
		public void TypeaheadInput_should_rendered_correctly_html_attributes()
		{
			//Test does not work unit new bUnit version
			//https://github.com/egil/bUnit/issues/231

			//var rendered = _testContext.RenderComponent<TypeaheadInput<string>>(
			//	("id", "id1"), //HTML attributes
			//	("class", "form-control w-100") //HTML attributes
			//	);

			//var input = rendered.Find("input");

			//Assert.IsNotNull(input);
			//input.MarkupMatches(@"<input id=""id1"" class=""form-control w-100"" />");
		}
	}
}