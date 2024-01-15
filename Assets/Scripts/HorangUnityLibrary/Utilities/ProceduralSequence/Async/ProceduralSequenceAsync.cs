using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Horang.HorangUnityLibrary.Utilities.ProceduralSequence.Async
{
	public readonly struct ProceduralSequenceAsync
	{
		private readonly Queue<(Func<UniTask<bool>> onInitialize, Func<UniTask<bool>> onExecute, Func<UniTask> onSuccess, Func<UniTask> onFailure, string name)> sequenceSegmentQueue;
		private readonly string sequencerName;
		
		public ProceduralSequenceAsync(string name)
		{
			sequencerName = name;
			sequenceSegmentQueue = new Queue<(Func<UniTask<bool>> onInitialize, Func<UniTask<bool>> onExecute, Func<UniTask> onSuccess, Func<UniTask> onFailure, string name)>();
		}

		public void AddSequenceElement(SequenceSegmentAsync sequenceSegmentAsync)
		{
			sequenceSegmentQueue.Enqueue(UnpackSegment(sequenceSegmentAsync));
		}

		public async UniTask RunSequencer()
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

				if (await segment.onInitialize.Invoke() is false)
				{
					Log.Print($"The sequence segment named [{segment.name}] cannot initialized. Will run onFailure callback.", LogPriority.Error);
					
					await segment.onFailure.Invoke();

					break;
				}

				if (await segment.onExecute.Invoke() is false)
				{
					Log.Print($"An error occurred while running segment named [{segment.name}]. Will run onFailure callback.", LogPriority.Error);
					
					await segment.onFailure.Invoke();

					break;
				}
				
				Log.Print($"The segment named [{segment.name}] is run successfully.", LogPriority.Verbose);
				
				await segment.onSuccess.Invoke();
			}
			
			Log.Print($"[{sequencerName}] sequence end.", LogPriority.Verbose);
		}

		private static (Func<UniTask<bool>>, Func<UniTask<bool>>, Func<UniTask>, Func<UniTask>, string name) UnpackSegment(SequenceSegmentAsync ss)
		{
			Func<UniTask<bool>> onInitialize = ss.Initialize;
			Func<UniTask<bool>> onExecute = ss.Execute;
			Func<UniTask> onSuccess = ss.OnSuccess;
			Func<UniTask> onFailure = ss.OnFailure;

			return (onInitialize, onExecute, onSuccess, onFailure, ss.SegmentName);
		}
	}
}