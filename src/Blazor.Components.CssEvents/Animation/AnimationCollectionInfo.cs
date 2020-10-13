using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Blazor.Components.CssEvents.Animation
{
	/// <summary>
	/// Collection of <see cref="AnimationEventInfo"/> to aggregate Animation events
	/// </summary>
	internal sealed class AnimationCollectionInfo : Collection<AnimationEventInfo>
	{
		private readonly Func<AnimationEventArgs[], Task> _transitionEndedCallback;

		public AnimationCollectionInfo(Func<AnimationEventArgs[], Task> transitionEndedCallback)
		{
			_transitionEndedCallback = transitionEndedCallback;
		}

		private List<AnimationEventArgs> _finishedTransitions = new List<AnimationEventArgs>();
		public async Task WhenAllFinished(AnimationEventArgs args)
		{
			_finishedTransitions.Add(args);

			if (Count == _finishedTransitions.Count
				&& !_finishedTransitions.Select(s => s.AnimationName).Except(this.Select(s => s.AnimationName)).Any()
				&& !_finishedTransitions.Select(s => s.Element).Except(this.Select(s => s.Element)).Any())
			{
				await _transitionEndedCallback(_finishedTransitions.ToArray());
				_finishedTransitions.Clear();
			}
		}
	}
}