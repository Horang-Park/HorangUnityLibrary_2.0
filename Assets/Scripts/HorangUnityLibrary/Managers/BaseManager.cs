using HorangUnityLibrary.Utilities.Foundation;

namespace HorangUnityLibrary.Managers
{
	public class BaseManager<T> : MonoSingleton<T> where T : BaseManager<T>
	{
	}
}