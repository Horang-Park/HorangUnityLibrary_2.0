using System;
using System.Collections.Generic;

namespace Horang.HorangUnityLibrary.Utilities.ProceduralSequence
{
	public readonly struct ProceduralSequence
	{
		private readonly Queue<(Func<bool> onInitialize, Func<bool> onExecute, Action onSuccess, Action onFailure, string name)> sequenceSegmentQueue;
		private readonly string sequencerName;
		
		public ProceduralSequence(string name)
		{
			sequencerName = name;
			sequenceSegmentQueue = new Queue<(Func<bool> onInitialize, Func<bool> onExecute, Action onSuccess, Action onFailure, string name)>();
		}

		public void AddSequenceElement(SequenceSegment sequenceSegment)
		{
			sequenceSegment.Initialize();
			
			sequenceSegmentQueue.Enqueue(UnpackSegment(sequenceSegment));
		}

		public void RunSequencer()
		{
			if (sequenceSegmentQueue.Count < 1)
			{
				Log.Print("This sequencer is not have any segments.", LogPriority.Error);

				return;
			}
			
			Log.Print($"[{sequencerName}] sequence start.", LogPriority.Verbose);

			while (sequenceSegmentQueue.Count > 0)
			{
				var segment = sequenceSegmentQueue.Dequeue();
				
				Log.Print($"The segment named [{segment.name}] is run start.", LogPriority.Verbose);

				if (segment.onInitialize.Invoke() is false)
				{
					Log.Print($"The sequence segment named [{segment.name}] cannot initialized. Will run onFailure callback.", LogPriority.Error);
					
					segment.onFailure.Invoke();

					break;
				}

				if (segment.onExecute.Invoke() is false)
				{
					Log.Print($"An error occurred while running segment named [{segment.name}]. Will run onFailure callback.", LogPriority.Error);
					
					segment.onFailure.Invoke();

					break;
				}
				
				Log.Print($"The segment named [{segment.name}] is run successfully.", LogPriority.Verbose);
				
				segment.onSuccess.Invoke();
			}
			
			Log.Print($"[{sequencerName}] sequence end.", LogPriority.Verbose);
		}

		private static (Func<bool>, Func<bool>, Action, Action, string name) UnpackSegment(SequenceSegment ss)
		{
			Func<bool> onInitialize = ss.Initialize;
			Func<bool> onExecute = ss.Execute;
			Action onSuccess = ss.OnSuccess;
			Action onFailure = ss.OnFailure;

			return (onInitialize, onExecute, onSuccess, onFailure, ss.SegmentName);
		}
	}
}