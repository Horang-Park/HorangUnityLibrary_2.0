using System.IO;

namespace Horang.HorangUnityLibrary.Managers.Static.AsyncNetworking.Interfaces
{
	public interface ICallbackHandlerStream : ICallbackHandlerBase
	{
		public void OnSuccess(Stream response);
	}
}