namespace Horang.HorangUnityLibrary.Managers.Static.Networking
{
	public interface IRequestCallbackBase
	{
		public void OnFailure(long code, string message);
		public void OnDelay();
	}

	public interface IRequestCallbackString : IRequestCallbackBase
	{
		public void OnSuccess(string response);
	}

	public interface IRequestCallbackByteArray : IRequestCallbackBase
	{
		public void OnSuccess(byte[] response);
	}
}