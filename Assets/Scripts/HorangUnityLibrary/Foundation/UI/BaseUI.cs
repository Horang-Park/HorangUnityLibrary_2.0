using UnityEngine;

namespace Horang.HorangUnityLibrary.Foundation.UI
{
	public abstract class BaseUI : MonoBehaviour
	{
		public bool IsVisible => gameObject.activeSelf;

		private bool isInitialized;

		public void Show()
		{
			if (gameObject.activeSelf)
			{
				return;
			}
			
			gameObject.SetActive(true);
			
			OnShowInitialize();
		}

		public void Hide()
		{
			if (gameObject.activeSelf is false)
			{
				return;
			}
			
			OnHideInitialize();
			
			gameObject.SetActive(false);
		}

		private void Awake()
		{
			Initialize();
		}

		protected abstract void OnShowInitialize();
		protected abstract void OnHideInitialize();
		protected abstract void OnInitializeOnce();

		private void Initialize()
		{
			if (isInitialized)
			{
				return;
			}
			
			OnInitializeOnce();

			isInitialized = true;
		}
	}
}