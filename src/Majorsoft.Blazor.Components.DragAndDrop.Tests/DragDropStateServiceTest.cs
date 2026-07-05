using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Majorsoft.Blazor.Components.DragAndDrop.Tests
{
	[TestClass]
	public class DragDropStateServiceTest
	{
		private DragDropStateService _service = null!;

		[TestInitialize]
		public void Init() => _service = new DragDropStateService();

		[TestMethod]
		public void Service_should_start_empty()
		{
			Assert.IsFalse(_service.HasActiveItem);
			Assert.IsNull(_service.ActiveItem);
		}

		[TestMethod]
		public void SetActiveItem_should_store_item_and_raise_event()
		{
			var raised = 0;
			_service.ActiveItemChanged += () => raised++;

			_service.SetActiveItem("payload");

			Assert.IsTrue(_service.HasActiveItem);
			Assert.AreEqual("payload", _service.ActiveItem);
			Assert.AreEqual(1, raised);
		}

		[TestMethod]
		public void GetActiveItem_should_return_typed_item()
		{
			_service.SetActiveItem(42);

			Assert.AreEqual(42, _service.GetActiveItem<int>());
		}

		[TestMethod]
		public void GetActiveItem_should_return_default_for_mismatched_type()
		{
			_service.SetActiveItem("a string");

			Assert.AreEqual(0, _service.GetActiveItem<int>());
			Assert.IsNull(_service.GetActiveItem<int[]>());
		}

		[TestMethod]
		public void Clear_should_reset_state_and_raise_event_once()
		{
			_service.SetActiveItem("payload");

			var raised = 0;
			_service.ActiveItemChanged += () => raised++;

			_service.Clear();
			Assert.IsFalse(_service.HasActiveItem);
			Assert.IsNull(_service.ActiveItem);
			Assert.AreEqual(1, raised);

			//Clearing again when already empty does not raise.
			_service.Clear();
			Assert.AreEqual(1, raised);
		}
	}

	[TestClass]
	public class DragDropEffectsTest
	{
		[TestMethod]
		[DataRow(DragDropEffects.None, "none")]
		[DataRow(DragDropEffects.Copy, "copy")]
		[DataRow(DragDropEffects.CopyLink, "copyLink")]
		[DataRow(DragDropEffects.CopyMove, "copyMove")]
		[DataRow(DragDropEffects.Link, "link")]
		[DataRow(DragDropEffects.LinkMove, "linkMove")]
		[DataRow(DragDropEffects.Move, "move")]
		[DataRow(DragDropEffects.All, "all")]
		public void DragDropEffects_ToJsValue_should_map_to_html_value(DragDropEffects effect, string expected)
		{
			Assert.AreEqual(expected, effect.ToJsValue());
		}

		[TestMethod]
		[DataRow(DropEffect.None, "none")]
		[DataRow(DropEffect.Copy, "copy")]
		[DataRow(DropEffect.Move, "move")]
		[DataRow(DropEffect.Link, "link")]
		public void DropEffect_ToJsValue_should_map_to_html_value(DropEffect effect, string expected)
		{
			Assert.AreEqual(expected, effect.ToJsValue());
		}
	}
}
