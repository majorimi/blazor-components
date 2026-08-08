using System.Threading.Tasks;

using Bunit;

using Majorsoft.Blazor.Components.CommonTestsBase;
using Majorsoft.Blazor.Components.Media.Players;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Majorsoft.Blazor.Components.Media.Tests.Players
{
	[TestClass]
	public class VideoPlayerTest : ComponentsTestBase<MediaPlayerBase>
	{
		[TestInitialize]
		public void Init()
		{
			_testContext.JSInterop.Mode = JSRuntimeMode.Loose;
		}

		[TestMethod]
		public void VideoPlayer_should_render_pixel_dimensions_as_html_attributes()
		{
			var rendered = _testContext.Render<VideoPlayer>(parameters => parameters
				.Add(p => p.Width, 800)
				.Add(p => p.Height, 600));

			var video = rendered.Find("video");

			Assert.AreEqual("800", video.GetAttribute("width"));
			Assert.AreEqual("600", video.GetAttribute("height"));
			Assert.IsFalse((video.GetAttribute("style") ?? "").Contains("%"));
		}

		[TestMethod]
		public void VideoPlayer_should_render_percent_dimensions_as_css_style()
		{
			var rendered = _testContext.Render<VideoPlayer>(parameters => parameters
				.Add(p => p.Width, 50)
				.Add(p => p.Height, 100)
				.Add(p => p.IsPlayerDimensionInPixels, false)
				.Add(p => p.Style, "border: 1px solid red;"));

			var video = rendered.Find("video");

			Assert.IsFalse(video.HasAttribute("width"));
			Assert.IsFalse(video.HasAttribute("height"));

			var style = video.GetAttribute("style");
			StringAssert.Contains(style, "width: 50%");
			StringAssert.Contains(style, "height: 100%");
			StringAssert.Contains(style, "border: 1px solid red;"); //user style is kept after the dimensions
		}

		[TestMethod]
		public void VideoPlayer_should_render_media_attributes()
		{
			var rendered = _testContext.Render<VideoPlayer>(parameters => parameters
				.Add(p => p.Source, "https://test/video.mp4")
				.Add(p => p.Poster, "https://test/poster.png")
				.Add(p => p.Autoplay, true)
				.Add(p => p.Loop, true)
				.Add(p => p.Muted, true)
				.Add(p => p.Preload, "metadata")
				.Add(p => p.Class, "my-class")
				.AddUnmatched("title", "text"));

			var video = rendered.Find("video");

			Assert.AreEqual("https://test/video.mp4", video.GetAttribute("src"));
			Assert.AreEqual("https://test/poster.png", video.GetAttribute("poster"));
			Assert.IsTrue(video.HasAttribute("autoplay"));
			Assert.IsTrue(video.HasAttribute("loop"));
			Assert.IsTrue(video.HasAttribute("muted"));
			Assert.IsTrue(video.HasAttribute("controls")); //default true
			Assert.AreEqual("metadata", video.GetAttribute("preload"));
			Assert.AreEqual("my-class", video.GetAttribute("class"));
			Assert.AreEqual("text", video.GetAttribute("title"));
		}

		[TestMethod]
		public void VideoPlayer_should_not_render_controls_when_disabled()
		{
			var rendered = _testContext.Render<VideoPlayer>(parameters => parameters
				.Add(p => p.ShowControls, false));

			var video = rendered.Find("video");

			Assert.IsFalse(video.HasAttribute("controls"));
		}

		[TestMethod]
		public async Task VideoPlayer_should_call_js_player_functions()
		{
			var module = _testContext.JSInterop.SetupModule("./_content/Majorsoft.Blazor.Components.Media/media.js");
			module.Mode = JSRuntimeMode.Loose;
			module.Setup<int>("initPlayer", _ => true).SetResult(1);

			var rendered = _testContext.Render<VideoPlayer>();
			var player = rendered.Instance;

			Assert.IsTrue(await player.PlayAsync()); //Loose mode returns null error = success
			await player.PauseAsync();
			await player.SeekAsync(12.5);
			await player.SetVolumeAsync(0.5);
			await player.SetMutedAsync(true);
			await player.SetPlaybackRateAsync(2);

			module.VerifyInvoke("playerPlay");
			module.VerifyInvoke("playerPause");
			module.VerifyInvoke("playerSeek");
			module.VerifyInvoke("playerSetVolume");
			module.VerifyInvoke("playerSetMuted");
			module.VerifyInvoke("playerSetRate");
		}

		[TestMethod]
		public async Task VideoPlayer_should_dispatch_player_events()
		{
			MediaPlayerState? playState = null;
			MediaPlayerState? endedState = null;
			var errorMessage = "";

			var rendered = _testContext.Render<VideoPlayer>(parameters => parameters
				.Add(p => p.OnPlay, state => { playState = state; })
				.Add(p => p.OnEnded, state => { endedState = state; })
				.Add(p => p.OnError, error => { errorMessage = error; }));

			var player = rendered.Instance;

			await player.PlayerEventAsync("play", new MediaPlayerState { CurrentTime = 1.5, Paused = false });
			await player.PlayerEventAsync("ended", new MediaPlayerState { Ended = true });
			await player.PlayerErrorAsync("Media error code 4");

			Assert.IsNotNull(playState);
			Assert.AreEqual(1.5, playState.CurrentTime);
			Assert.IsNotNull(endedState);
			Assert.IsTrue(endedState.Ended);
			Assert.AreEqual("Media error code 4", errorMessage);
		}

		[TestMethod]
		public async Task VideoPlayer_should_ignore_unknown_and_unsubscribed_events()
		{
			var rendered = _testContext.Render<VideoPlayer>();

			//No callbacks are set, must not throw.
			await rendered.Instance.PlayerEventAsync("play", new MediaPlayerState());
			await rendered.Instance.PlayerEventAsync("unknown", new MediaPlayerState());
		}
	}
}
