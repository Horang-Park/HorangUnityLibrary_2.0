using Cysharp.Threading.Tasks;
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

		public async UniTaskVoid Hide(int delayMilliseconds)
		{
			if (gameObject.activeSelf is false)
			{
				return;
			}
			
			OnHideInitialize();

			await UniTask.Delay(delayMilliseconds);
			
			gameObject.SetActive(false);
		}

		protected abstract void OnShowInitialize();
		protected abstract void OnHideInitialize();
		protected abstract void OnInitializeOnce();

		internal void Initialize()
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