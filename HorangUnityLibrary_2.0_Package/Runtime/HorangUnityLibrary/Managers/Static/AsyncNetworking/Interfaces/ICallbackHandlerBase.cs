using System.Net;

namespace Horang.HorangUnityLibrary.Managers.Static.AsyncNetworking.Interfaces
{
	public interface ICallbackHandlerBase
	{
		public void OnFailure(HttpStatusCode httpStatusCode, string message);
		public void OnDelay();
	}
	
}