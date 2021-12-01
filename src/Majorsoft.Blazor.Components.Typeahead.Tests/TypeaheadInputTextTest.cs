using Majorsoft.Blazor.Components.Common.JsInterop.Click;
using Majorsoft.Blazor.Components.Debounce;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using Majorsoft.Blazor.Components.CommonTestsBase;
using Bunit;
using Majorsoft.Blazor.Components.Timer;
using Majorsoft.Blazor.Components.Common.JsInterop.ElementInfo;

namespace Majorsoft.Blazor.Components.Typeahead.Tests
{
	[TestClass]
	public class TypeaheadInputTextTest : ComponentsTestBase<TypeaheadInput<string>>
	{
		private Mock<IClickBoundariesHandler> _clickBoundariesMock;
		private BunitJSModuleInterop _jsInteropModul;

		[TestInitialize]
		public void Init()
		{
			var logger = new Mock<ILogger<DebounceInputText>>();
			var logger2 = new Mock<ILogger<TypeaheadInputText<string>>>();
			_clickBoundariesMock = new Mock<IClickBoundariesHandler>();

			_testContext.Services.Add(new ServiceDescriptor(typeof(ILogger<DebounceInputText>), logger.Object));
			_testContext.Services.Add(new ServiceDescriptor(typeof(ILogger<TypeaheadInputText<string>>), logger2.Object));
			_testContext.Services.Add(new ServiceDescriptor(typeof(IClickBoundariesHandler), _clickBoundariesMock.Object));

			_testContext.JSInterop.Mode = JSRuntimeMode.Strict;
#if DEBUG
			_jsInteropModul = _testContext.JSInterop.SetupModule("./_content/Majorsoft.Blazor.Components.Common.JsInterop/elementInfo.js");
#else
			_jsInteropModul = _testContext.JSInterop.SetupModule("./_content/Majorsoft.Blazor.Components.Common.JsInterop/elementInfo.min.js");
#endif
			_jsInteropModul.Setup<DomRect>("getBoundingClientRect", _ => true).SetResult(new DomRect());
		}

		[TestMethod]
		public void TypeaheadInputText_should_rendered_correctly_html_attributes()
		{
			////TODO: needs EditContext
			///
			//var rendered = _testContext.RenderComponent<TypeaheadInputText<string>>(
			//	("id", "id1"), //HTML attributes
			//	("class", "form-control w-100") //HTML attributes
			//	);

			//var input = rendered.Find("input");

			//Assert.IsNotNull(input);
			//input.MarkupMatches(@"<input autocomplete=""off"" id=""id1"" class=""form-control w-100"" />");
		}
	}
}