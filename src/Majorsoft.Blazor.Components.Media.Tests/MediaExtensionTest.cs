using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Majorsoft.Blazor.Components.Media.Tests
{
	[TestClass]
	public class MediaExtensionTest
	{
		[TestMethod]
		public void AddMediaComponents_should_check_null_ServiceCollection()
		{
			Assert.ThrowsExactly<ArgumentNullException>(() => ((IServiceCollection)null!).AddMediaComponents());
		}

		[TestMethod]
		public async System.Threading.Tasks.Task AddMediaComponents_should_register_MediaDeviceService()
		{
			var services = new ServiceCollection();
			services.AddSingleton(new Mock<IJSRuntime>().Object);

			services.AddMediaComponents();

			//The service only implements IAsyncDisposable so the container must be disposed async.
			await using var provider = services.BuildServiceProvider();
			var service = provider.GetRequiredService<IMediaDeviceService>();

			Assert.IsNotNull(service);
			Assert.IsInstanceOfType(service, typeof(MediaDeviceService));
		}
	}
}
