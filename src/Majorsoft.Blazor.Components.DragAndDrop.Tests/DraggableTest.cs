using System.Threading.Tasks;

using Bunit;

using Majorsoft.Blazor.Components.CommonTestsBase;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Majorsoft.Blazor.Components.DragAndDrop.Tests
{
	[TestClass]
	public class DraggableTest : ComponentsTestBase
	{
		private DragDropStateService _state = null!;

		[TestInitialize]
		public void Init()
		{
			_state = new DragDropStateService();
			_testContext.JSInterop.Mode = JSRuntimeMode.Loose;
			_testContext.Services.AddSingleton<IDragDropStateService>(_state);
		}

		[TestMethod]
		public void Draggable_should_render_draggable_element_with_child_content()
		{
			var rendered = _testContext.Render<Draggable<string>>(parameters => parameters
				.Add(p => p.Item, "payload")
				.AddChildContent("<span>card</span>"));

			var div = rendered.Find(".bdraggable");
			Assert.AreEqual("true", div.GetAttribute("draggable"));
			Assert.IsNotNull(div.QuerySelector("span"));
			Assert.AreEqual("card", div.QuerySelector("span")!.TextContent);
		}

		[TestMethod]
		public void Draggable_should_render_not_draggable_when_Disabled()
		{
			var rendered = _testContext.Render<Draggable<string>>(parameters => parameters
				.Add(p => p.Disabled, true)
				.AddChildContent("<span>card</span>"));

			Assert.AreEqual("false", rendered.Find(".bdraggable").GetAttribute("draggable"));
			Assert.IsTrue(rendered.Find(".bdraggable").ClassList.Contains("bdraggable-disabled"));
		}

		[TestMethod]
		public async Task Draggable_should_set_active_item_and_fire_OnDragStart()
		{
			var started = false;

			var rendered = _testContext.Render<Draggable<string>>(parameters => parameters
				.Add(p => p.Item, "payload")
				.Add(p => p.OnDragStart, (DragEventArgs e) => started = true)
				.AddChildContent("<span>card</span>"));

			await rendered.Find(".bdraggable").TriggerEventAsync("ondragstart", new DragEventArgs());

			Assert.IsTrue(started);
			Assert.AreEqual("payload", _state.ActiveItem);
			Assert.IsTrue(rendered.Find(".bdraggable").ClassList.Contains("bdraggable-dragging"));
		}

		[TestMethod]
		public async Task Draggable_should_clear_active_item_and_fire_OnDragEnd()
		{
			var ended = false;

			var rendered = _testContext.Render<Draggable<string>>(parameters => parameters
				.Add(p => p.Item, "payload")
				.Add(p => p.OnDragEnd, (DragEventArgs e) => ended = true)
				.AddChildContent("<span>card</span>"));

			await rendered.Find(".bdraggable").TriggerEventAsync("ondragstart", new DragEventArgs());
			Assert.IsTrue(_state.HasActiveItem);

			await rendered.Find(".bdraggable").TriggerEventAsync("ondragend", new DragEventArgs());

			Assert.IsTrue(ended);
			Assert.IsFalse(_state.HasActiveItem);
			Assert.IsFalse(rendered.Find(".bdraggable").ClassList.Contains("bdraggable-dragging"));
		}

		[TestMethod]
		public async Task Draggable_should_not_start_drag_when_Disabled()
		{
			var rendered = _testContext.Render<Draggable<string>>(parameters => parameters
				.Add(p => p.Item, "payload")
				.Add(p => p.Disabled, true)
				.AddChildContent("<span>card</span>"));

			await rendered.Find(".bdraggable").TriggerEventAsync("ondragstart", new DragEventArgs());

			Assert.IsFalse(_state.HasActiveItem);
		}

		[TestMethod]
		public async Task Draggable_should_fire_OnDrag()
		{
			var dragged = false;

			var rendered = _testContext.Render<Draggable<string>>(parameters => parameters
				.Add(p => p.OnDrag, (DragEventArgs e) => dragged = true)
				.AddChildContent("<span>card</span>"));

			await rendered.Find(".bdraggable").TriggerEventAsync("ondrag", new DragEventArgs());

			Assert.IsTrue(dragged);
		}

		[TestMethod]
		public void Draggable_should_apply_custom_class_style_and_unmatched_attributes()
		{
			var rendered = _testContext.Render<Draggable<string>>(parameters => parameters
				.Add(p => p.Class, "my-card")
				.Add(p => p.Style, "color: red;")
				.AddUnmatched("data-test", "value")
				.AddChildContent("<span>card</span>"));

			var div = rendered.Find(".bdraggable");
			Assert.IsTrue(div.ClassList.Contains("my-card"));
			Assert.AreEqual("color: red;", div.GetAttribute("style"));
			Assert.AreEqual("value", div.GetAttribute("data-test"));
		}
	}
}
