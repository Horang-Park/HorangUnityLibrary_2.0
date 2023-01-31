using System.Text;
using UnityEngine;

namespace HorangUnityLibrary.Foundation
{
	public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		[Header("Singleton Options")]
		public bool dontDestroyOnLoadObject;
		public HideFlags gameObjectHideFlags = HideFlags.NotEditable;
		
		private static T instance;
		// ReSharper disable once StaticMemberInGenericType
		private static readonly object Gate = new object();

		public static T Instance
		{
			get
			{
				lock (Gate)
				{
					if (instance is not null)
					{
						return instance;
					}
					
					instance = (T)FindObjectOfType(typeof(T));

					if (FindObjectsOfType(typeof(T)).Length > 1)
					{
						return instance;
					}

					if (instance != null)
					{
						return instance;
					}
					
					var singleton = new GameObject();
					instance = singleton.AddComponent<T>();
					singleton.name = new StringBuilder("[Singleton] ").Append(typeof(T)).ToString();
					
					return instance;
				}
			}
		}

		protected virtual void Awake()
		{
			gameObject.hideFlags = gameObjectHideFlags;
			
			if (dontDestroyOnLoadObject)
			{
				DontDestroyOnLoad(gameObject);
			}
		}
	}
}