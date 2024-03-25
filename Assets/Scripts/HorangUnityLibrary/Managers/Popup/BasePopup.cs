using UnityEngine;

namespace Horang.HorangUnityLibrary.Managers.Popup
{
	public abstract class BasePopup : MonoBehaviour
	{
		public bool PopupActive
		{
			get => gameObject.activeSelf;
			set => gameObject.SetActive(value);
		}
		
		public void Show()
		{
			if (gameObject.activeSelf)
			{
				return;
			}
			
			SetPopupData();
			OnShowing();
		}

		public void Hide()
		{
			if (gameObject.activeSelf is false)
			{
				return;
			}
			
			OnHiding();
		}

		protected abstract void SetPopupData();
		protected abstract void OnShowing();
		protected abstract void OnHiding();
	}
}