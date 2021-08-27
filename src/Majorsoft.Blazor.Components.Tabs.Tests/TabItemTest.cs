using System;

using Majorsoft.Blazor.Components.CommonTestsBase;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Majorsoft.Blazor.Components.Tabs.Tests
{
	[TestClass]
	public class TabItemTest : ComponentsTestBase<TabItem>
	{
		[ExpectedException(typeof(ArgumentNullException), "TabItem must exist within a TabsPanel (Parameter 'Parent')")]
		[TestMethod]
		public void TabItem_should_not_render_without_TabsPanel()
		{
			var rendered = _testContext.RenderComponent<TabItem>();
		}
	}
}