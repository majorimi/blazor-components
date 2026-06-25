using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Bunit;
using Majorsoft.Blazor.Components.CommonTestsBase;

namespace Majorsoft.Blazor.Components.Inputs.Tests
{
	[TestClass]
	public class MarkdownEditorTest : ComponentsTestBase<MarkdownEditor>
	{
		[TestInitialize]
		public void Init()
		{
			//The editor lazily imports its JS module and calls a few interop methods on render.
			_testContext.JSInterop.Mode = JSRuntimeMode.Loose;
		}

		[TestMethod]
		public void MarkdownEditor_should_render_toolbar_and_editor_by_default()
		{
			var rendered = _testContext.Render<MarkdownEditor>(p => p
				.Add(c => c.Value, ""));

			Assert.IsNotNull(rendered.Find(".bmde-toolbar"));
			Assert.IsNotNull(rendered.Find("textarea.bmde-textarea"));
			//All seven default sections (6 button sections + view) are present.
			Assert.AreEqual(7, rendered.FindAll(".bmde-section").Count);
		}

		[TestMethod]
		public void MarkdownEditor_should_render_initial_value_in_textarea()
		{
			var rendered = _testContext.Render<MarkdownEditor>(p => p
				.Add(c => c.Value, "# Hello"));

			var textarea = rendered.Find("textarea.bmde-textarea");
			StringAssert.Contains(textarea.GetAttribute("value") ?? textarea.TextContent, "# Hello");
		}

		[TestMethod]
		public void MarkdownEditor_should_hide_sections_when_toggled_off()
		{
			var rendered = _testContext.Render<MarkdownEditor>(p => p
				.Add(c => c.ShowHeadingSection, false)
				.Add(c => c.ShowTextStyleSection, false)
				.Add(c => c.ShowListSection, false)
				.Add(c => c.ShowIndentSection, false)
				.Add(c => c.ShowBlockSection, false)
				.Add(c => c.ShowInsertSection, false)
				.Add(c => c.ShowViewSection, false));

			Assert.AreEqual(0, rendered.FindAll(".bmde-section").Count);
		}

		[TestMethod]
		public void MarkdownEditor_should_not_render_toolbar_when_disabled()
		{
			var rendered = _testContext.Render<MarkdownEditor>(p => p
				.Add(c => c.ShowToolbar, false));

			Assert.AreEqual(0, rendered.FindAll(".bmde-toolbar").Count);
		}

		[TestMethod]
		public void MarkdownEditor_should_render_preview_only_in_preview_view()
		{
			var rendered = _testContext.Render<MarkdownEditor>(p => p
				.Add(c => c.Value, "# Title")
				.Add(c => c.View, MarkdownEditorView.Preview));

			Assert.AreEqual(0, rendered.FindAll("textarea.bmde-textarea").Count);
			var preview = rendered.Find(".bmde-preview");
			StringAssert.Contains(preview.InnerHtml, "<h1>Title</h1>");
		}

		[TestMethod]
		public void MarkdownEditor_should_keep_view_switch_in_preview_so_user_can_return()
		{
			var rendered = _testContext.Render<MarkdownEditor>(p => p
				.Add(c => c.Value, "# Title")
				.Add(c => c.View, MarkdownEditorView.Preview));

			//Toolbar stays (formatting hidden) so the Edit/Split/Preview switch is reachable from Preview.
			Assert.IsNotNull(rendered.Find(".bmde-toolbar"));
			Assert.IsTrue(rendered.FindAll("button").Any(b => b.TextContent.Trim() == "Edit"),
				"The Edit button must remain available in Preview mode.");
		}

		[TestMethod]
		public void MarkdownEditor_should_render_both_panes_in_split_view()
		{
			var rendered = _testContext.Render<MarkdownEditor>(p => p
				.Add(c => c.Value, "**bold**")
				.Add(c => c.View, MarkdownEditorView.Split));

			Assert.IsNotNull(rendered.Find("textarea.bmde-textarea"));
			var preview = rendered.Find(".bmde-preview");
			StringAssert.Contains(preview.InnerHtml, "<strong>bold</strong>");
		}

		[TestMethod]
		public void MarkdownEditor_should_show_remaining_counter_when_max_chars_set()
		{
			var rendered = _testContext.Render<MarkdownEditor>(p => p
				.Add(c => c.Value, "abc")
				.Add(c => c.MaxAllowedChars, 10));

			var counter = rendered.Find(".bmde-counter");
			StringAssert.Contains(counter.TextContent, "7 / 10");
		}

		[TestMethod]
		public void MarkdownEditor_increase_indent_should_prefix_line_with_spaces()
		{
			string? bound = "hello";
			var rendered = _testContext.Render<MarkdownEditor>(p => p
				.Add(c => c.Value, bound)
				.Add(c => c.ShowButtonTooltips, false) //plain buttons expose a title to find by
				.Add(c => c.ValueChanged, v => bound = v));

			rendered.FindAll("button.bmde-btn")
				.First(b => b.GetAttribute("title") == "Increase indent")
				.Click();

			Assert.AreEqual("  hello", bound);
		}

		[TestMethod]
		public void MarkdownEditor_decrease_indent_should_remove_leading_spaces()
		{
			string? bound = "    hello";
			var rendered = _testContext.Render<MarkdownEditor>(p => p
				.Add(c => c.Value, bound)
				.Add(c => c.ShowButtonTooltips, false)
				.Add(c => c.ValueChanged, v => bound = v));

			rendered.FindAll("button.bmde-btn")
				.First(b => b.GetAttribute("title") == "Decrease indent")
				.Click();

			Assert.AreEqual("  hello", bound);
		}
	}
}
