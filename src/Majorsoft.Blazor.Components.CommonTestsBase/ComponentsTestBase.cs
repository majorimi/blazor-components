using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Majorsoft.Blazor.Components.CommonTestsBase
{
	[TestClass]
	public abstract class ComponentsTestBase
	{
		protected Bunit.TestContext _testContext;

		[TestInitialize]
		public void InitBase()
		{
			_testContext = new Bunit.TestContext();
		}

		[TestCleanup]
		public void CleanupBase()
		{
			_testContext?.Dispose();
		}
	}

	public abstract class ComponentsTestBase<T> : ComponentsTestBase
		where T : class
	{
		[TestInitialize]
		public void InitGenericBase()
		{
			var logger = new Mock<ILogger<T>>();
			_testContext.Services.Add(new ServiceDescriptor(typeof(ILogger<T>), logger.Object));
		}
	}
}