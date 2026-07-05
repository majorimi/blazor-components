using System.Threading.Tasks;

using Bunit;

using Majorsoft.Blazor.Components.CommonTestsBase;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Majorsoft.Blazor.Components.DragAndDrop.Tests
{
	[TestClass]
	public class DropZoneTest : ComponentsTestBase
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
		public void DropZone_should_render_with_child_content_and_not_active()
		{
			var rendered = _testContext.Render<DropZone<string>>(parameters => parameters
				.AddChildContent("<span>zone</span>"));

			var div = rendered.Find(".bdropzone");
			Assert.IsNotNull(div.QuerySelector("span"));
			Assert.IsFalse(div.ClassList.Contains("bdropzone-active"));
			Assert.IsFalse(rendered.Instance.IsDragOver);
		}

		[TestMethod]
		public async Task DropZone_should_become_active_on_dragenter_and_inactive_on_dragleave()
		{
			var rendered = _testContext.Render<DropZone<string>>(parameters => parameters
				.AddChildContent("<span>zone</span>"));

			await rendered.Find(".bdropzone").TriggerEventAsync("ondragenter", new DragEventArgs());
			Assert.IsTrue(rendered.Instance.IsDragOver);
			Assert.IsTrue(rendered.Find(".bdropzone").ClassList.Contains("bdropzone-active"));

			await rendered.Find(".bdropzone").TriggerEventAsync("ondragleave", new DragEventArgs());
			Assert.IsFalse(rendered.Instance.IsDragOver);
		}

		[TestMethod]
		public async Task DropZone_should_stay_active_until_nested_enters_are_balanced()
		{
			var rendered = _testContext.Render<DropZone<string>>(parameters => parameters
				.AddChildContent("<span>zone</span>"));

			var zone = rendered.Find(".bdropzone");
			//enter zone, then enter a child element (two enters)
			await zone.TriggerEventAsync("ondragenter", new DragEventArgs());
			await zone.TriggerEventAsync("ondragenter", new DragEventArgs());
			//leaving the child only (one leave) should keep it active
			await zone.TriggerEventAsync("ondragleave", new DragEventArgs());
			Assert.IsTrue(rendered.Instance.IsDragOver);

			await zone.TriggerEventAsync("ondragleave", new DragEventArgs());
			Assert.IsFalse(rendered.Instance.IsDragOver);
		}

		[TestMethod]
		public async Task DropZone_should_fire_OnDrop_and_OnItemDrop_with_active_item()
		{
			string? droppedItem = null;
			var rawDropFired = false;
			_state.SetActiveItem("payload");

			var rendered = _testContext.Render<DropZone<string>>(parameters => parameters
				.Add(p => p.OnDrop, (DragEventArgs e) => rawDropFired = true)
				.Add(p => p.OnItemDrop, item => droppedItem = item)
				.AddChildContent("<span>zone</span>"));

			await rendered.Find(".bdropzone").TriggerEventAsync("ondrop", new DragEventArgs());

			Assert.IsTrue(rawDropFired);
			Assert.AreEqual("payload", droppedItem);
			//state is cleared after drop
			Assert.IsFalse(_state.HasActiveItem);
		}

		[TestMethod]
		public async Task DropZone_should_not_fire_OnItemDrop_when_AcceptCondition_rejects()
		{
			var itemDropFired = false;
			_state.SetActiveItem("payload");

			var rendered = _testContext.Render<DropZone<string>>(parameters => parameters
				.Add(p => p.AcceptCondition, (string? item) => false)
				.Add(p => p.OnItemDrop, item => itemDropFired = true)
				.AddChildContent("<span>zone</span>"));

			await rendered.Find(".bdropzone").TriggerEventAsync("ondrop", new DragEventArgs());

			Assert.IsFalse(itemDropFired);
		}

		[TestMethod]
		public async Task DropZone_should_show_reject_style_when_AcceptCondition_rejects_on_enter()
		{
			_state.SetActiveItem("payload");

			var rendered = _testContext.Render<DropZone<string>>(parameters => parameters
				.Add(p => p.AcceptCondition, (string? item) => false)
				.AddChildContent("<span>zone</span>"));

			await rendered.Find(".bdropzone").TriggerEventAsync("ondragenter", new DragEventArgs());

			Assert.IsTrue(rendered.Find(".bdropzone").ClassList.Contains("bdropzone-reject"));
		}

		[TestMethod]
		public async Task DropZone_should_ignore_drop_when_Disabled()
		{
			var itemDropFired = false;
			_state.SetActiveItem("payload");

			var rendered = _testContext.Render<DropZone<string>>(parameters => parameters
				.Add(p => p.Disabled, true)
				.Add(p => p.OnItemDrop, item => itemDropFired = true)
				.AddChildContent("<span>zone</span>"));

			await rendered.Find(".bdropzone").TriggerEventAsync("ondrop", new DragEventArgs());

			Assert.IsFalse(itemDropFired);
			Assert.IsTrue(rendered.Find(".bdropzone").ClassList.Contains("bdropzone-disabled"));
		}

		[TestMethod]
		public async Task DropZone_should_expose_dropped_files_via_DragEventArgs()
		{
			string[]? files = null;

			var rendered = _testContext.Render<DropZone<string>>(parameters => parameters
				.Add(p => p.OnDrop, (DragEventArgs e) => files = e.DataTransfer?.Files)
				.AddChildContent("<span>zone</span>"));

			var args = new DragEventArgs { DataTransfer = new DataTransfer { Files = new[] { "a.txt", "b.png" } } };
			await rendered.Find(".bdropzone").TriggerEventAsync("ondrop", args);

			Assert.IsNotNull(files);
			CollectionAssert.AreEqual(new[] { "a.txt", "b.png" }, files);
		}

		[TestMethod]
		public async Task DragAndDrop_should_transfer_typed_item_end_to_end()
		{
			var draggable = _testContext.Render<Draggable<int>>(parameters => parameters
				.Add(p => p.Item, 99)
				.AddChildContent("<span>card</span>"));

			int? dropped = null;
			var zone = _testContext.Render<DropZone<int>>(parameters => parameters
				.Add(p => p.OnItemDrop, item => dropped = item)
				.AddChildContent("<span>zone</span>"));

			await draggable.Find(".bdraggable").TriggerEventAsync("ondragstart", new DragEventArgs());
			await zone.Find(".bdropzone").TriggerEventAsync("ondrop", new DragEventArgs());

			Assert.AreEqual(99, dropped);
		}

		[TestMethod]
		public async Task DropZone_should_clear_active_highlight_when_drag_ends_globally()
		{
			_state.SetActiveItem("payload");

			var rendered = _testContext.Render<DropZone<string>>(parameters => parameters
				.AddChildContent("<span>zone</span>"));

			//hover the zone, then the drag is cancelled/dropped elsewhere (no dragleave fires here)
			await rendered.Find(".bdropzone").TriggerEventAsync("ondragenter", new DragEventArgs());
			Assert.IsTrue(rendered.Instance.IsDragOver);

			//global drag end clears the shared state and must reset this zone's highlight
			_state.Clear();

			Assert.IsFalse(rendered.Instance.IsDragOver);
			Assert.IsFalse(rendered.Find(".bdropzone").ClassList.Contains("bdropzone-active"));
		}

		[TestMethod]
		public void DropZone_should_pass_unmatched_attributes_to_root()
		{
			var rendered = _testContext.Render<DropZone<string>>(parameters => parameters
				.AddUnmatched("data-test", "value")
				.AddChildContent("<span>zone</span>"));

			Assert.AreEqual("value", rendered.Find(".bdropzone").GetAttribute("data-test"));
		}
	}
}
