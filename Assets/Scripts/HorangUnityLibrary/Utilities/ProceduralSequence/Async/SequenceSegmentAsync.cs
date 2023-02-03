using Cysharp.Threading.Tasks;

namespace HorangUnityLibrary.Utilities.ProceduralSequence.Async
{
	public abstract class SequenceSegmentAsync
	{
		public string SegmentName { get; }

		protected SequenceSegmentAsync(string name)
		{
			SegmentName = name;
		}
		public abstract UniTask<bool> Initialize();
		public abstract UniTask<bool> Execute();
		public abstract UniTask OnSuccess();
		public abstract UniTask OnFailure();
	}
}