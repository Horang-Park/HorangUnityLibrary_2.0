using UnityEngine;

namespace HorangUnityLibrary.Utilities.Foundation
{
	public class BaseManager<T> : MonoSingleton<T> where T : BaseManager<T>
	{
	}
}