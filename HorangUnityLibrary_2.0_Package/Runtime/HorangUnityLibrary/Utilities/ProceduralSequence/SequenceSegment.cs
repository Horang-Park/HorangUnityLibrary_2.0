namespace Horang.HorangUnityLibrary.Utilities.ProceduralSequence
{
	public abstract class SequenceSegment
	{
		public string SegmentName { get; }

		protected SequenceSegment(string name)
		{
			SegmentName = name;
		}
		public abstract bool Initialize();
		public abstract bool Execute();
		public abstract void OnSuccess();
		public abstract void OnFailure();
	}
}