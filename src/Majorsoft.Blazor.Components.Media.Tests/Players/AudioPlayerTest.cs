using System.Threading.Tasks;

using Bunit;

using Majorsoft.Blazor.Components.CommonTestsBase;
using Majorsoft.Blazor.Components.Media.Players;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Majorsoft.Blazor.Components.Media.Tests.Players
{
	[TestClass]
	public class AudioPlayerTest : ComponentsTestBase<MediaPlayerBase>
	{
		[TestInitialize]
		public void Init()
		{
			_testContext.JSInterop.Mode = JSRuntimeMode.Loose;
		}

		[TestMethod]
		public void AudioPlayer_should_render_audio_element_with_attributes()
		{
			var rendered = _testContext.Render<AudioPlayer>(parameters => parameters
				.Add(p => p.Source, "https://test/audio.mp3")
				.Add(p => p.Loop, true)
				.Add(p => p.Class, "my-class")
				.Add(p => p.Style, "width: 300px;")
				.AddUnmatched("title", "text"));

			var audio = rendered.Find("audio");

			Assert.AreEqual("https://test/audio.mp3", audio.GetAttribute("src"));
			Assert.IsTrue(audio.HasAttribute("loop"));
			Assert.IsTrue(audio.HasAttribute("controls")); //default true
			Assert.IsFalse(audio.HasAttribute("autoplay"));
			Assert.AreEqual("my-class", audio.GetAttribute("class"));
			Assert.AreEqual("width: 300px;", audio.GetAttribute("style"));
			Assert.AreEqual("text", audio.GetAttribute("title"));
		}

		[TestMethod]
		public async Task AudioPlayer_should_dispatch_player_events()
		{
			MediaPlayerState? pauseState = null;

			var rendered = _testContext.Render<AudioPlayer>(parameters => parameters
				.Add(p => p.OnPause, state => { pauseState = state; }));

			await rendered.Instance.PlayerEventAsync("pause", new MediaPlayerState { Paused = true, CurrentTime = 3 });

			Assert.IsNotNull(pauseState);
			Assert.IsTrue(pauseState.Paused);
			Assert.AreEqual(3, pauseState.CurrentTime);
		}
	}
}
