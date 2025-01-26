using System.Text;
using UnityEngine;

namespace Horang.HorangUnityLibrary.Foundation
{
	public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		[Header("Singleton Options")]
		public bool dontDestroyOnLoadObject;
		public HideFlags gameObjectHideFlags = HideFlags.NotEditable;
		
		private static T _instance;

		public static T Instance
		{
			get
			{
				if (_instance != null || _instance)
				{
					return _instance;
				}
				
				_instance = (T)FindFirstObjectByType(typeof(T));
				
				if (_instance != null || _instance)
				{
					return _instance;
				}

				if (FindObjectsByType(typeof(T), FindObjectsSortMode.None).Length > 1 || _instance)
				{
					return _instance;
				}
				
				var singleton = new GameObject();
				_instance = singleton.AddComponent<T>();
				singleton.name = new StringBuilder("[Singleton] ").Append(typeof(T)).ToString();
				
				return _instance;
			}
		}

		protected virtual void Awake()
		{
			_instance = this as T;
			
			gameObject.hideFlags = gameObjectHideFlags;
			
			if (dontDestroyOnLoadObject)
			{
				DontDestroyOnLoad(gameObject);
			}
		}
	}
}