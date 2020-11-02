using Blazor.Components.Core.HtmlColors;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Blazor.Components.Core.Tests.HtmlColors
{
	[TestClass]
	public class HtmlColorHelperTest
	{
		[TestMethod]
		public void HtmlColorHelper_should_have_the_correct_number_of_data()
		{
			Assert.AreEqual(148, HtmlColorHelper.NamedHtmlColors.Count);
		}
	}
}