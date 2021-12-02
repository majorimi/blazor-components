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
using System.Text.Json;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;

namespace Majorsoft.Blazor.Components.Typeahead.Tests
{
	[TestClass]
	public class TypeaheadInputTest : ComponentsTestBase<TypeaheadInput<string>>
	{
		private Mock<IClickBoundariesHandler> _clickBoundariesMock;
		private BunitJSModuleInterop _jsInteropModul;

		private class StatesWithFlags
		{
			public string Name { get; set; }
			public string Flag { get; set; }
		}
		private static StatesWithFlags[] _testData = null;
		private const string JsonData = "[{\"Name\":\"Alabama\",\"Flag\":\"5/5c/Flag_of_Alabama.svg/45px-Flag_of_Alabama.svg.png\"},{\"Name\":\"Alaska\",\"Flag\":\"e/e6/Flag_of_Alaska.svg/43px-Flag_of_Alaska.svg.png\"},{\"Name\":\"Arizona\",\"Flag\":\"9/9d/Flag_of_Arizona.svg/45px-Flag_of_Arizona.svg.png\"},{\"Name\":\"Arkansas\",\"Flag\":\"9/9d/Flag_of_Arkansas.svg/45px-Flag_of_Arkansas.svg.png\"},{\"Name\":\"California\",\"Flag\":\"0/01/Flag_of_California.svg/45px-Flag_of_California.svg.png\"},{\"Name\":\"Colorado\",\"Flag\":\"4/46/Flag_of_Colorado.svg/45px-Flag_of_Colorado.svg.png\"},{\"Name\":\"Connecticut\",\"Flag\":\"9/96/Flag_of_Connecticut.svg/39px-Flag_of_Connecticut.svg.png\"},{\"Name\":\"Delaware\",\"Flag\":\"c/c6/Flag_of_Delaware.svg/45px-Flag_of_Delaware.svg.png\"},{\"Name\":\"Florida\",\"Flag\":\"f/f7/Flag_of_Florida.svg/45px-Flag_of_Florida.svg.png\"},{\"Name\":\"Georgia\",\"Flag\":\"5/54/Flag_of_Georgia_%28U.S._state%29.svg/46px-Flag_of_Georgia_%28U.S._state%29.svg.png\"},{\"Name\":\"Hawaii\",\"Flag\":\"e/ef/Flag_of_Hawaii.svg/46px-Flag_of_Hawaii.svg.png\"},{\"Name\":\"Idaho\",\"Flag\":\"a/a4/Flag_of_Idaho.svg/38px-Flag_of_Idaho.svg.png\"},{\"Name\":\"Illinois\",\"Flag\":\"0/01/Flag_of_Illinois.svg/46px-Flag_of_Illinois.svg.png\"},{\"Name\":\"Indiana\",\"Flag\":\"a/ac/Flag_of_Indiana.svg/45px-Flag_of_Indiana.svg.png\"},{\"Name\":\"Iowa\",\"Flag\":\"a/aa/Flag_of_Iowa.svg/44px-Flag_of_Iowa.svg.png\"},{\"Name\":\"Kansas\",\"Flag\":\"d/da/Flag_of_Kansas.svg/46px-Flag_of_Kansas.svg.png\"},{\"Name\":\"Kentucky\",\"Flag\":\"8/8d/Flag_of_Kentucky.svg/46px-Flag_of_Kentucky.svg.png\"},{\"Name\":\"Louisiana\",\"Flag\":\"e/e0/Flag_of_Louisiana.svg/46px-Flag_of_Louisiana.svg.png\"},{\"Name\":\"Maine\",\"Flag\":\"3/35/Flag_of_Maine.svg/45px-Flag_of_Maine.svg.png\"},{\"Name\":\"Maryland\",\"Flag\":\"a/a0/Flag_of_Maryland.svg/45px-Flag_of_Maryland.svg.png\"},{\"Name\":\"Massachusetts\",\"Flag\":\"f/f2/Flag_of_Massachusetts.svg/46px-Flag_of_Massachusetts.svg.png\"},{\"Name\":\"Michigan\",\"Flag\":\"b/b5/Flag_of_Michigan.svg/45px-Flag_of_Michigan.svg.png\"},{\"Name\":\"Minnesota\",\"Flag\":\"b/b9/Flag_of_Minnesota.svg/46px-Flag_of_Minnesota.svg.png\"},{\"Name\":\"Mississippi\",\"Flag\":\"4/42/Flag_of_Mississippi.svg/45px-Flag_of_Mississippi.svg.png\"},{\"Name\":\"Missouri\",\"Flag\":\"5/5a/Flag_of_Missouri.svg/46px-Flag_of_Missouri.svg.png\"},{\"Name\":\"Montana\",\"Flag\":\"c/cb/Flag_of_Montana.svg/45px-Flag_of_Montana.svg.png\"},{\"Name\":\"Nebraska\",\"Flag\":\"4/4d/Flag_of_Nebraska.svg/46px-Flag_of_Nebraska.svg.png\"},{\"Name\":\"Nevada\",\"Flag\":\"f/f1/Flag_of_Nevada.svg/45px-Flag_of_Nevada.svg.png\"},{\"Name\":\"New Hampshire\",\"Flag\":\"2/28/Flag_of_New_Hampshire.svg/45px-Flag_of_New_Hampshire.svg.png\"},{\"Name\":\"New Jersey\",\"Flag\":\"9/92/Flag_of_New_Jersey.svg/45px-Flag_of_New_Jersey.svg.png\"},{\"Name\":\"New Mexico\",\"Flag\":\"c/c3/Flag_of_New_Mexico.svg/45px-Flag_of_New_Mexico.svg.png\"},{\"Name\":\"New York\",\"Flag\":\"1/1a/Flag_of_New_York.svg/46px-Flag_of_New_York.svg.png\"},{\"Name\":\"North Carolina\",\"Flag\":\"b/bb/Flag_of_North_Carolina.svg/45px-Flag_of_North_Carolina.svg.png\"},{\"Name\":\"North Dakota\",\"Flag\":\"e/ee/Flag_of_North_Dakota.svg/38px-Flag_of_North_Dakota.svg.png\"},{\"Name\":\"Ohio\",\"Flag\":\"4/4c/Flag_of_Ohio.svg/46px-Flag_of_Ohio.svg.png\"},{\"Name\":\"Oklahoma\",\"Flag\":\"6/6e/Flag_of_Oklahoma.svg/45px-Flag_of_Oklahoma.svg.png\"},{\"Name\":\"Oregon\",\"Flag\":\"b/b9/Flag_of_Oregon.svg/46px-Flag_of_Oregon.svg.png\"},{\"Name\":\"Pennsylvania\",\"Flag\":\"f/f7/Flag_of_Pennsylvania.svg/45px-Flag_of_Pennsylvania.svg.png\"},{\"Name\":\"Rhode Island\",\"Flag\":\"f/f3/Flag_of_Rhode_Island.svg/32px-Flag_of_Rhode_Island.svg.png\"},{\"Name\":\"South Carolina\",\"Flag\":\"6/69/Flag_of_South_Carolina.svg/45px-Flag_of_South_Carolina.svg.png\"},{\"Name\":\"South Dakota\",\"Flag\":\"1/1a/Flag_of_South_Dakota.svg/46px-Flag_of_South_Dakota.svg.png\"},{\"Name\":\"Tennessee\",\"Flag\":\"9/9e/Flag_of_Tennessee.svg/46px-Flag_of_Tennessee.svg.png\"},{\"Name\":\"Texas\",\"Flag\":\"f/f7/Flag_of_Texas.svg/45px-Flag_of_Texas.svg.png\"},{\"Name\":\"Utah\",\"Flag\":\"f/f6/Flag_of_Utah.svg/45px-Flag_of_Utah.svg.png\"},{\"Name\":\"Vermont\",\"Flag\":\"4/49/Flag_of_Vermont.svg/46px-Flag_of_Vermont.svg.png\"},{\"Name\":\"Virginia\",\"Flag\":\"4/47/Flag_of_Virginia.svg/44px-Flag_of_Virginia.svg.png\"},{\"Name\":\"Washington\",\"Flag\":\"5/54/Flag_of_Washington.svg/46px-Flag_of_Washington.svg.png\"},{\"Name\":\"West Virginia\",\"Flag\":\"2/22/Flag_of_West_Virginia.svg/46px-Flag_of_West_Virginia.svg.png\"},{\"Name\":\"Wisconsin\",\"Flag\":\"2/22/Flag_of_Wisconsin.svg/45px-Flag_of_Wisconsin.svg.png\"},{\"Name\":\"Wyoming\",\"Flag\":\"b/bc/Flag_of_Wyoming.svg/43px-Flag_of_Wyoming.svg.png\"}]";

		[ClassInitialize]
		public static void TestInitForAll(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext context)
		{
			_testData = JsonSerializer.Deserialize<StatesWithFlags[]>(JsonData);
		}

		[TestInitialize]
		public void Init()
		{
			var logger = new Mock<ILogger<DebounceInput>>();
			var logger2 = new Mock<ILogger<AdvancedTimer>>();
			_clickBoundariesMock = new Mock<IClickBoundariesHandler>();

			_testContext.Services.Add(new ServiceDescriptor(typeof(ILogger<DebounceInput>), logger.Object));
			_testContext.Services.Add(new ServiceDescriptor(typeof(ILogger<AdvancedTimer>), logger2.Object));
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
		public void TypeaheadInput_should_rendered_correctly_html_attributes()
		{
			var rendered = _testContext.RenderComponent<TypeaheadInput<string>>(
				("id", "id1"), //HTML attributes
				("class", "form-control w-100") //HTML attributes
				);

			var input = rendered.Find("input");

			Assert.IsNotNull(input);
			input.MarkupMatches(@"<input autocomplete=""off"" id=""id1"" class=""form-control w-100"" />");
		}

		[TestMethod]
		public void TypeaheadInput_should_rendered_correctly()
		{
			var rendered = _testContext.RenderComponent<TypeaheadInput<string>>(parameters => parameters
					.Add(p => p.InProgressContent, "<strong>Searching...</strong>")
					.Add(p => p.Data, _testData.Select(x => x.Name))
					.Add(p => p.MinLength, 5));

			var input = rendered.Find("input");

			Assert.IsNotNull(input);
			input.MarkupMatches(@"<input autocomplete=""off"" class=""typeahead"" />");
		}

		[TestMethod]
		public async Task TypeaheadInput_should_rendered_correctly_DropdownHeight()
		{
			//TODO: it should open dropdown....
			var rendered = _testContext.RenderComponent<TypeaheadInput<string>>(parameters => parameters
					.Add(p => p.InProgressContent, "<strong>Searching...</strong>")
					.Add(p => p.NoResultContent, "<strong>Not found...</strong>")
					.Add(p => p.Data, _testData.Select(x => x.Name))
					.Add(p => p.ShowAllOnEmptyInput, true)
					.Add(p => p.FitDropdownWidth, false)
					.Add(p => p.DropdownHeight, 50)
					.Add(p => p.DropdownWidth, 500));

			var input = rendered.Find("input");
			
			//input.Focus();
			//await input.TriggerEventAsync("onmouseenter", new MouseEventArgs());
			input.Input("t");
			rendered.SetParametersAndRender();

			Assert.IsNotNull(input);
			rendered.WaitForAssertion(() => input.MarkupMatches(@"<input value=""t"" autocomplete=""off"" class=""typeahead"" />"));
		}

		[TestMethod]
		public void TypeaheadInput_should_set_SelectedItem_with_OnSelectedItemChanged_event_triggered()
		{
			string selectedItem = "";
			int eventCounter = 0;
			var rendered = _testContext.RenderComponent<TypeaheadInput<string>>(parameters => parameters
					.Add(p => p.InProgressContent, "<strong>Searching...</strong>")
					.Add(p => p.Data, _testData.Select(x => x.Name))
					.Add(p => p.OnSelectedItemChanged, x => { selectedItem = x; eventCounter++; })
					.Add(p => p.SelectedItem, "Texas"));

			var input = rendered.Find("input");

			Assert.IsNotNull(input);
			Assert.AreEqual(rendered.Instance.Value, "Texas");
			Assert.AreEqual(rendered.Instance.SelectedItem, selectedItem);
			Assert.AreEqual(1, eventCounter);

			input.MarkupMatches(@"<input value=""Texas"" autocomplete=""off"" class=""typeahead"" />");
		}
	}
}