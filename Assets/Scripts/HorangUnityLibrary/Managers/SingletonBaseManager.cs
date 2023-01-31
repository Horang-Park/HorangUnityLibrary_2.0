using System;
using HorangUnityLibrary.Utilities.Foundation;
using UnityEngine;

namespace HorangUnityLibrary.Managers
{
	public class SingletonBaseManager<T> : MonoSingleton<T> where T : SingletonBaseManager<T>
	{}

	public class MonoBaseManager : MonoBehaviour
	{
		[Header("Manager Options")]
		public bool dontDestroyOnLoadObject;
		public HideFlags gameObjectHideFlags = HideFlags.NotEditable;

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