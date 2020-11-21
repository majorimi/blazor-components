using System;
using System.Threading.Tasks;

using Bunit;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Blazor.Components.Timer.Tests
{
	[TestClass]
	public class AdvancedTimerTest
	{
		private Bunit.TestContext _testContext;

		[TestInitialize]
		public void Init()
		{
			_testContext = new Bunit.TestContext();

			var mock = new Mock<ILogger<AdvancedTimer>>();
			_testContext.Services.Add(new ServiceDescriptor(typeof(ILogger<AdvancedTimer>), mock.Object));
		}

		[TestCleanup]
		public void Cleanup()
		{
			_testContext?.Dispose();
		}

		[TestMethod]
		public void AdvancedTimer_should_rendered_nothing()
		{
			var rendered = _testContext.RenderComponent<AdvancedTimer>();

			rendered.MarkupMatches("");
		}

		[TestMethod]
		public async Task AdvancedTimer_should_execute_once()
		{
			var debounceTime = 30;
			var count = 0;

			var rendered = _testContext.RenderComponent<AdvancedTimer>(parameters => parameters
				.Add(p => p.DelayInMilisec, debounceTime)
				.Add(p => p.OnIntervalElapsed, c => { count = (int)c; }));
			
			await Task.Delay(debounceTime * 10); //wait for debounce

			rendered.MarkupMatches("");
			Assert.AreEqual(1, count);
		}

		[TestMethod]
		public async Task AdvancedTimer_should_execute_n_times()
		{
			var debounceTime = 30;
			var required = 3;
			var count = 0;

			var rendered = _testContext.RenderComponent<AdvancedTimer>(parameters => parameters
				.Add(p => p.DelayInMilisec, debounceTime)
				.Add(p => p.Occurring, Times.Exactly((ulong)required))
				.Add(p => p.OnIntervalElapsed, c => { count = (int)c; }));

			await Task.Delay(debounceTime * (10 * required)); //wait for debounce

			rendered.MarkupMatches("");
			Assert.AreEqual(required, count);
		}

		[TestMethod]
		public async Task AdvancedTimer_should_not_execute_when_autostart_false()
		{
			var debounceTime = 30;
			var count = 0;

			var rendered = _testContext.RenderComponent<AdvancedTimer>(parameters => parameters
				.Add(p => p.DelayInMilisec, debounceTime)
				.Add(p => p.AutoStart, false)
				.Add(p => p.OnIntervalElapsed, c => { count = (int)c; }));

			await Task.Delay(debounceTime * 10); //wait for debounce

			rendered.MarkupMatches("");
			Assert.AreEqual(0, count);
		}
	}
}