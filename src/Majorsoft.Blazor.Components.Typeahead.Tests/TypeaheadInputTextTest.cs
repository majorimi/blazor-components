using Bunit;
using Majorsoft.Blazor.Components.Common.JsInterop.Click;
using Majorsoft.Blazor.Components.Common.JsInterop.ElementInfo;
using Majorsoft.Blazor.Components.CommonTestsBase;
using Majorsoft.Blazor.Components.Debounce;
using Majorsoft.Blazor.Components.Timer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Majorsoft.Blazor.Components.Typeahead.Tests;

[TestClass]
public class TypeaheadInputTextTest : ComponentsTestBase<TypeaheadInput<string>>
{
	private IClickBoundariesHandler _clickBoundariesMock;
	private BunitJSModuleInterop _jsInteropModul;

	[TestInitialize]
	public void Init()
	{
		var logger = Substitute.For<ILogger<DebounceInputText>>();
		var logger2 = Substitute.For<ILogger<TypeaheadInputText<string>>>();
		_clickBoundariesMock = Substitute.For<IClickBoundariesHandler>();

		_testContext.Services.Add(new ServiceDescriptor(typeof(ILogger<DebounceInputText>), logger));
		_testContext.Services.Add(new ServiceDescriptor(typeof(ILogger<TypeaheadInputText<string>>), logger2));
		_testContext.Services.Add(new ServiceDescriptor(typeof(IClickBoundariesHandler), _clickBoundariesMock));

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
