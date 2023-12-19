using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Horang.HorangUnityLibrary.Foundation.Manager;
using Horang.HorangUnityLibrary.Foundation.UI;
using Horang.HorangUnityLibrary.Utilities;
using UnityEngine;

namespace Horang.HorangUnityLibrary.Managers.UI
{
	public sealed class UIManager : MonoBaseManager
	{
		[System.Serializable]
		public struct BaseUIData
		{
			public string uiName;
			public bool isShowFirst;
			public BaseUI baseUI;
		}

		[SerializeField] private List<BaseUIData> baseUIs;

		private readonly Dictionary<int, BaseUI> baseUIDictionary = new();
		private readonly Stack<BaseUI> baseUIUseHistory = new();

		private bool firstUiOnFlag;

		public T GetBaseUI<T>(string uiName) where T : BaseUI
		{
			var key = uiName.GetHashCode();

			if (UINameValidation(key, out var result))
			{
				return result as T;
			}
			
			Log.Print($"There is no UI named [{uiName}].", LogPriority.Error);
				
			return null;
		}

		public void PushHistory(BaseUI target)
		{
			baseUIUseHistory.Push(target);
		}

		public void PopHistory()
		{
			if (baseUIUseHistory.Count < 1)
			{
				Log.Print("There is no turn off UIs", LogPriority.Error);
				
				return;
			}

			var result = baseUIUseHistory.Pop();
			
			result.Hide();
		}

		public void PopHistory(int delayMilliseconds)
		{
			if (baseUIUseHistory.Count < 1)
			{
				Log.Print("There is no turn off UIs", LogPriority.Error);
				
				return;
			}

			var result = baseUIUseHistory.Pop();
			
			UniTask.Void(() => result.Hide(delayMilliseconds));
		}

		protected override void Awake()
		{
			base.Awake();
			
			DataProcessing();
		}

		private void DataProcessing()
		{
			foreach (var ui in baseUIs)
			{
				var key = ui.uiName.GetHashCode();
				
				ui.baseUI.Hide();

				if (ui.isShowFirst && firstUiOnFlag is false)
				{
					ui.baseUI.Show();
					
					baseUIUseHistory.Push(ui.baseUI);

					firstUiOnFlag = true;
				}
				
				baseUIDictionary.Add(key, ui.baseUI);
			}
		}

		private bool UINameValidation(int k, out BaseUI bU)
		{
			if (baseUIDictionary.TryGetValue(k, out var value))
			{
				bU = value;

				return true;
			}
			
			bU = null;
			
			return false;
		}
	}
}