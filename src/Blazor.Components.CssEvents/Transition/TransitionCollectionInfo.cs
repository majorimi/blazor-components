using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Blazor.Components.CssEvents.Transition
{
	public class TransitionCollectionInfo : Collection<TransitionInfo>
	{
		private readonly Func<TransitionEndEventArgs[], Task> _transitionEndedCallback;

		public TransitionCollectionInfo(Func<TransitionEndEventArgs[], Task> transitionEndedCallback)
		{
			_transitionEndedCallback = transitionEndedCallback;
		}

		private List<TransitionEndEventArgs> _finishedTransitions = new List<TransitionEndEventArgs>();
		public async Task WhenAllFinished(TransitionEndEventArgs args)
		{
			_finishedTransitions.Add(args);

			if (Count == _finishedTransitions.Count
				&& !_finishedTransitions.Select(s => s.PropertyName).Except(this.Select(s => s.TransitionPropertyName)).Any()
				&& !_finishedTransitions.Select(s => s.Element).Except(this.Select(s => s.Element)).Any())
			{
				await _transitionEndedCallback(_finishedTransitions.ToArray());
				_finishedTransitions.Clear();
			}
		}
	}
}