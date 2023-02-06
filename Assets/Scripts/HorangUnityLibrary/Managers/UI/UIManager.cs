using System.Collections.Generic;
using Horang.HorangUnityLibrary.Foundation.Manager;
using Horang.HorangUnityLibrary.Foundation.UI;
using Horang.HorangUnityLibrary.Utilities;
using UnityEngine;

namespace Horang.HorangUnityLibrary.Managers.UI
{
	public class UIManager : MonoBaseManager
	{
		[System.Serializable]
		public struct BaseUIData
		{
			public string uiName;
			public bool isShowFirst;
			public BaseUI baseUI;
		}

		[SerializeField] private List<BaseUIData> baseUis;

		private readonly Dictionary<int, BaseUI> baseUiDictionary = new();
		private readonly Stack<BaseUI> baseUiUseHistory = new();

		private bool firstUiOnFlag;

		public T GetBaseUi<T>(string uiName) where T : BaseUI
		{
			var key = uiName.GetHashCode();

			if (UiNameValidation(key, out var result) is false)
			{
				Log.Print($"There is no UI named [{uiName}].", LogPriority.Error);
				
				return null;
			}

			if (result.IsVisible is false)
			{
				baseUiUseHistory.Push(result);
			}

			return result as T;
		}

		public void PutBaseUi()
		{
			if (baseUiUseHistory.Count < 1)
			{
				Log.Print("There is no turn off UIs", LogPriority.Error);
				
				return;
			}

			var result = baseUiUseHistory.Pop();
			
			result.Hide();
		}

		protected override void Awake()
		{
			base.Awake();
			
			DataProcessing();
		}

		private void DataProcessing()
		{
			foreach (var ui in baseUis)
			{
				var key = ui.uiName.GetHashCode();
				
				ui.baseUI.Hide();

				if (ui.isShowFirst && firstUiOnFlag is false)
				{
					ui.baseUI.Show();
					
					baseUiUseHistory.Push(ui.baseUI);

					firstUiOnFlag = true;
				}
				
				baseUiDictionary.Add(key, ui.baseUI);
			}
		}

		private bool UiNameValidation(int k, out BaseUI bU)
		{
			if (baseUiDictionary.ContainsKey(k))
			{
				bU = baseUiDictionary[k];

				return true;
			}
			
			bU = null;
			
			return false;
		}
	}
}