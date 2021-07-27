using Majorsoft.Blazor.Components.Common.JsInterop.Click;
using Majorsoft.Blazor.Components.Debounce;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using Majorsoft.Blazor.Components.CommonTestsBase;

namespace Majorsoft.Blazor.Components.Typeahead.Tests
{
	[TestClass]
	public class TypeaheadInputText : ComponentsTestBase<TypeaheadInput<string>>
	{
		private Mock<IClickBoundariesHandler> _clickBoundariesMock;

		[TestInitialize]
		public void Init()
		{
			var logger = new Mock<ILogger<DebounceInput>>();
			_clickBoundariesMock = new Mock<IClickBoundariesHandler>();

			_testContext.Services.Add(new ServiceDescriptor(typeof(ILogger<DebounceInput>), logger.Object));
			_testContext.Services.Add(new ServiceDescriptor(typeof(IClickBoundariesHandler), _clickBoundariesMock.Object));
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