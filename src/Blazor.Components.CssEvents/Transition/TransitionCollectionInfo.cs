using System;
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

			if (this.Count == _finishedTransitions.Count)
			{
				await _transitionEndedCallback(_finishedTransitions.ToArray());
				_finishedTransitions.Clear();
			}
		}
	}
}