using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Bunit;
using Moq;
using Majorsoft.Blazor.Components.CommonTestsBase;
using Majorsoft.Blazor.Components.Common.JsInterop.Click;

namespace Majorsoft.Blazor.Components.Inputs.Tests
{
	[TestClass]
	public class RichTextEditorTest : ComponentsTestBase<RichTextEditor>
	{
		[TestInitialize]
		public void Init()
		{
			//The editor lazily imports its JS module and calls interop (setHtml/getMarkdown/command) on render.
			_testContext.JSInterop.Mode = JSRuntimeMode.Loose;

			//The toolbar's insert buttons host a Tooltips Popover, which needs a click-boundaries handler
			//and its own logger.
			var clickHandlerMock = new Mock<IClickBoundariesHandler>();
			clickHandlerMock
				.Setup(x => x.RegisterClickBoundariesAsync(It.IsAny<ElementReference>(), It.IsAny<Func<MouseEventArgs, Task>>(), It.IsAny<Func<MouseEventArgs, Task>>()))
				.Returns(Task.CompletedTask);
			clickHandlerMock
				.Setup(x => x.RemoveClickBoundariesAsync(It.IsAny<ElementReference>()))
				.Returns(Task.CompletedTask);

			_testContext.Services.Add(new ServiceDescriptor(typeof(IClickBoundariesHandler), clickHandlerMock.Object));
			_testContext.Services.Add(new ServiceDescriptor(typeof(ILogger<Tooltips.Popover>), new Mock<ILogger<Tooltips.Popover>>().Object));
		}

		[TestMethod]
		public void RichTextEditor_should_render_toolbar_and_contenteditable_surface()
		{
			var rendered = _testContext.Render<RichTextEditor>(p => p
				.Add(c => c.Value, ""));

			Assert.IsNotNull(rendered.Find(".bmde-toolbar"));
			var area = rendered.Find(".bmde-richarea");
			Assert.AreEqual("true", area.GetAttribute("contenteditable"));
		}

		[TestMethod]
		public void RichTextEditor_should_render_six_formatting_sections_no_view_switch()
		{
			var rendered = _testContext.Render<RichTextEditor>(p => p
				.Add(c => c.Value, ""));

			//Heading, Text style, List, Indent, Block, Insert; no Edit/Split/Preview switch like Markdown.
			Assert.AreEqual(6, rendered.FindAll(".bmde-section").Count);
		}

		[TestMethod]
		public void RichTextEditor_should_hide_sections_when_toggled_off()
		{
			var rendered = _testContext.Render<RichTextEditor>(p => p
				.Add(c => c.ShowHeadingSection, false)
				.Add(c => c.ShowTextStyleSection, false)
				.Add(c => c.ShowListSection, false)
				.Add(c => c.ShowIndentSection, false)
				.Add(c => c.ShowBlockSection, false)
				.Add(c => c.ShowInsertSection, false));

			Assert.AreEqual(0, rendered.FindAll(".bmde-section").Count);
		}

		[TestMethod]
		public void RichTextEditor_should_not_render_toolbar_when_disabled()
		{
			var rendered = _testContext.Render<RichTextEditor>(p => p
				.Add(c => c.ShowToolbar, false));

			Assert.AreEqual(0, rendered.FindAll(".bmde-toolbar").Count);
		}

		[TestMethod]
		public void RichTextEditor_should_mark_surface_non_editable_when_disabled()
		{
			var rendered = _testContext.Render<RichTextEditor>(p => p
				.Add(c => c.Disabled, true));

			var area = rendered.Find(".bmde-richarea");
			Assert.AreEqual("false", area.GetAttribute("contenteditable"));
		}

		[TestMethod]
		public void RichTextEditor_should_apply_placeholder_attribute()
		{
			var rendered = _testContext.Render<RichTextEditor>(p => p
				.Add(c => c.Placeholder, "Type here"));

			var area = rendered.Find(".bmde-richarea");
			Assert.AreEqual("Type here", area.GetAttribute("data-placeholder"));
		}

		[TestMethod]
		public void RichTextEditor_GetHtml_should_convert_value_to_html()
		{
			var rendered = _testContext.Render<RichTextEditor>(p => p
				.Add(c => c.Value, "# Title"));

			StringAssert.Contains(rendered.Instance.GetHtml(), "<h1>Title</h1>");
		}

		[TestMethod]
		public void RichTextEditor_should_show_word_count_in_footer()
		{
			var rendered = _testContext.Render<RichTextEditor>(p => p
				.Add(c => c.Value, "one two three"));

			StringAssert.Contains(rendered.Find(".bmde-counter").TextContent, "3 words");
		}

		[TestMethod]
		public void RichTextEditor_should_not_show_any_insert_popover_initially()
		{
			var rendered = _testContext.Render<RichTextEditor>(p => p
				.Add(c => c.Value, ""));

			Assert.AreEqual(0, rendered.FindAll(".bmde-insert-form").Count);
		}

		[TestMethod]
		public void RichTextEditor_Link_button_should_open_link_popover_with_text_and_url()
		{
			var rendered = _testContext.Render<RichTextEditor>(p => p
				.Add(c => c.Value, ""));

			ClickInsertButton(rendered, "Link");

			//The Tooltips Popover panel hosts the form, anchored below the icon.
			Assert.IsNotNull(rendered.Find(".bpopover .bmde-insert-form"));
			//Two inputs: link text and URL.
			Assert.AreEqual(2, rendered.FindAll(".bmde-insert-form input").Count);
		}

		[TestMethod]
		public void RichTextEditor_Image_button_should_open_url_only_popover_with_insert_disabled_until_url()
		{
			var rendered = _testContext.Render<RichTextEditor>(p => p
				.Add(c => c.Value, ""));

			ClickInsertButton(rendered, "Image");

			//No file browser (embedding would need external storage): a URL is required instead.
			Assert.AreEqual(0, rendered.FindAll(".bmde-insert-form input[type=file]").Count);
			Assert.IsNotNull(rendered.Find(".bmde-insert-form input[type=url]"));

			//Insert stays disabled until a URL is entered.
			var insert = rendered.FindAll(".bmde-insert-form .bmde-popover-actions button").First();
			Assert.IsTrue(insert.HasAttribute("disabled"));

			rendered.Find(".bmde-insert-form input[type=url]").Input("https://example.com/cat.png");
			Assert.IsFalse(rendered.FindAll(".bmde-insert-form .bmde-popover-actions button").First().HasAttribute("disabled"));
		}

		[TestMethod]
		public void RichTextEditor_Table_button_should_open_table_dimensions_popover()
		{
			var rendered = _testContext.Render<RichTextEditor>(p => p
				.Add(c => c.Value, ""));

			ClickInsertButton(rendered, "Table");

			//Rows and columns inputs to choose the dimensions.
			Assert.AreEqual(2, rendered.FindAll(".bmde-insert-form input[type=number]").Count);
		}

		[TestMethod]
		public void RichTextEditor_insert_popover_should_close_on_header_close_button()
		{
			var rendered = _testContext.Render<RichTextEditor>(p => p
				.Add(c => c.Value, ""));

			ClickInsertButton(rendered, "Link");
			Assert.AreEqual(1, rendered.FindAll(".bmde-insert-form").Count);

			//The Popover header close (x) button dismisses it (Esc/outside-click do the same at runtime).
			rendered.Find(".bpopover-close").Click();

			Assert.AreEqual(0, rendered.FindAll(".bmde-insert-form").Count);
		}

		//Clicks the insert toolbar button (a Popover trigger) identified by its title (Link / Image / Table).
		private static void ClickInsertButton(IRenderedComponent<RichTextEditor> rendered, string title)
		{
			rendered.FindAll("button.bmde-btn")
				.First(b => b.GetAttribute("title") == title)
				.Click();
		}
	}
}
