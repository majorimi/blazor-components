using System;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Majorsoft.Blazor.Components.DragAndDrop.Tests
{
	[TestClass]
	public class DragAndDropExtensionsTest
	{
		[TestMethod]
		public void AddDragAndDrop_should_register_state_service_as_scoped()
		{
			var services = new ServiceCollection();

			services.AddDragAndDrop();

			var descriptor = services.Single(s => s.ServiceType == typeof(IDragDropStateService));
			Assert.AreEqual(ServiceLifetime.Scoped, descriptor.Lifetime);
			Assert.AreEqual(typeof(DragDropStateService), descriptor.ImplementationType);

			using var provider = services.BuildServiceProvider();
			using var scope = provider.CreateScope();
			Assert.IsNotNull(scope.ServiceProvider.GetService<IDragDropStateService>());
		}

		[TestMethod]
		public void AddDragAndDrop_should_throw_on_null_services()
		{
			Assert.ThrowsExactly<ArgumentNullException>(() => ((IServiceCollection)null!).AddDragAndDrop());
		}
	}
}
