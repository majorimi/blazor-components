using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Blazor.Components.CssEvents.Transition
{
	/// <summary>
	/// Collection of <see cref="TransitionEventInfo"/> to aggregate Transition events
	/// </summary>
	internal sealed class TransitionCollectionInfo : Collection<TransitionEventInfo>
	{
		private readonly Func<TransitionEventArgs[], Task> _transitionEndedCallback;

		public TransitionCollectionInfo(Func<TransitionEventArgs[], Task> transitionEndedCallback)
		{
			_transitionEndedCallback = transitionEndedCallback;
		}

		private List<TransitionEventArgs> _finishedTransitions = new List<TransitionEventArgs>();
		public async Task WhenAllFinished(TransitionEventArgs args)
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