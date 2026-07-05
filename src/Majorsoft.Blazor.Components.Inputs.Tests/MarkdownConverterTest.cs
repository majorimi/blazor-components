using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Majorsoft.Blazor.Components.Inputs.Tests
{
	[TestClass]
	public class MarkdownConverterTest
	{
		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		public void ToHtml_should_return_empty_for_null_or_empty(string input)
		{
			Assert.AreEqual(string.Empty, MarkdownConverter.ToHtml(input));
		}

		[TestMethod]
		[DataRow("# Title", "<h1>Title</h1>")]
		[DataRow("## Title", "<h2>Title</h2>")]
		[DataRow("###### Title", "<h6>Title</h6>")]
		public void ToHtml_should_render_headings(string input, string expected)
		{
			StringAssert.Contains(MarkdownConverter.ToHtml(input), expected);
		}

		[TestMethod]
		public void ToHtml_should_render_bold_italic_and_strikethrough()
		{
			var html = MarkdownConverter.ToHtml("**bold** *italic* ~~gone~~");

			StringAssert.Contains(html, "<strong>bold</strong>");
			StringAssert.Contains(html, "<em>italic</em>");
			StringAssert.Contains(html, "<del>gone</del>");
		}

		[TestMethod]
		public void ToHtml_should_render_inline_code_without_inner_formatting()
		{
			var html = MarkdownConverter.ToHtml("Use `**not bold**` here");

			StringAssert.Contains(html, "<code>**not bold**</code>");
			Assert.IsFalse(html.Contains("<strong>"), "Inline code content must not be transformed.");
		}

		[TestMethod]
		public void ToHtml_should_render_fenced_code_block_and_escape_content()
		{
			var html = MarkdownConverter.ToHtml("```\n<script>x</script>\n```");

			StringAssert.Contains(html, "<pre><code>");
			StringAssert.Contains(html, "&lt;script&gt;x&lt;/script&gt;");
		}

		[TestMethod]
		public void ToHtml_should_render_blockquote()
		{
			var html = MarkdownConverter.ToHtml("> quoted line");

			StringAssert.Contains(html, "<blockquote>");
			StringAssert.Contains(html, "quoted line");
		}

		[TestMethod]
		public void ToHtml_should_render_unordered_and_ordered_lists()
		{
			var ul = MarkdownConverter.ToHtml("- one\n- two");
			var ol = MarkdownConverter.ToHtml("1. one\n2. two");

			StringAssert.Contains(ul, "<ul>");
			StringAssert.Contains(ul, "<li>one</li>");
			StringAssert.Contains(ol, "<ol>");
			StringAssert.Contains(ol, "<li>two</li>");
		}

		[TestMethod]
		public void ToHtml_should_render_nested_lists()
		{
			var html = MarkdownConverter.ToHtml("- Parent\n  - Child1\n  - Child2");

			//The child list must be nested inside the parent <li>, not flattened to one level.
			StringAssert.Contains(html, "<li>Parent<ul>");
			StringAssert.Contains(html, "<li>Child1</li>");
			Assert.AreEqual(2, System.Text.RegularExpressions.Regex.Matches(html, "<ul>").Count);
		}

		[TestMethod]
		public void ToHtml_should_render_task_list_nested_under_bullet()
		{
			var html = MarkdownConverter.ToHtml("- Tasks:\n  - [x] done\n  - [ ] todo");

			StringAssert.Contains(html, "<li>Tasks:<ul class=\"bmde-tasklist\">");
			StringAssert.Contains(html, "disabled checked");
		}

		[TestMethod]
		public void ToHtml_should_render_task_list_with_checkboxes()
		{
			var html = MarkdownConverter.ToHtml("- [x] done\n- [ ] todo");

			StringAssert.Contains(html, "bmde-tasklist");
			StringAssert.Contains(html, "type=\"checkbox\" disabled checked");
			StringAssert.Contains(html, "done");
			StringAssert.Contains(html, "todo");
		}

		[TestMethod]
		public void ToHtml_should_render_links_and_images()
		{
			var link = MarkdownConverter.ToHtml("[text](https://a.b)");
			var image = MarkdownConverter.ToHtml("![alt](https://a.b/i.png)");

			StringAssert.Contains(link, "<a href=\"https://a.b\">text</a>");
			StringAssert.Contains(image, "<img src=\"https://a.b/i.png\" alt=\"alt\" />");
		}

		[TestMethod]
		public void ToHtml_should_render_pipe_table_with_alignment()
		{
			var html = MarkdownConverter.ToHtml("| A | B |\n| :- | -: |\n| 1 | 2 |");

			StringAssert.Contains(html, "<table>");
			StringAssert.Contains(html, "<th style=\"text-align:left\">A</th>");
			StringAssert.Contains(html, "<th style=\"text-align:right\">B</th>");
			StringAssert.Contains(html, "<td style=\"text-align:right\">2</td>");
		}

		[TestMethod]
		public void ToHtml_should_render_horizontal_rule()
		{
			StringAssert.Contains(MarkdownConverter.ToHtml("---"), "<hr />");
		}

		[TestMethod]
		public void ToHtml_should_escape_raw_html_to_prevent_injection()
		{
			var html = MarkdownConverter.ToHtml("<script>alert(1)</script>");

			Assert.IsFalse(html.Contains("<script>"), "Raw HTML must be escaped.");
			StringAssert.Contains(html, "&lt;script&gt;");
		}

		[TestMethod]
		public void ToHtml_should_keep_allowed_inline_tags_from_toolbar()
		{
			var html = MarkdownConverter.ToHtml("text <u>underline</u> end");

			StringAssert.Contains(html, "<u>underline</u>");
		}
	}
}
