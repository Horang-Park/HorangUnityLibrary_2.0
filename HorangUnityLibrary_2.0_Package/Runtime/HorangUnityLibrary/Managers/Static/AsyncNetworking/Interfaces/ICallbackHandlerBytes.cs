namespace Horang.HorangUnityLibrary.Managers.Static.AsyncNetworking.Interfaces
{
	public interface ICallbackHandlerBytes : ICallbackHandlerBase
	{
		public void OnSuccess(byte[] response);
	}
}