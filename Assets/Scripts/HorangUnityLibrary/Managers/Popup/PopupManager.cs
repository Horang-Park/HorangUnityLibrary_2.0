using System.Collections.Generic;
using System.Linq;
using Horang.HorangUnityLibrary.Foundation.Manager;
using Horang.HorangUnityLibrary.Utilities;
using UnityEngine;

namespace Horang.HorangUnityLibrary.Managers.Popup
{
	public sealed class PopupManager : MonoBaseManager
	{
		[System.Serializable]
		public struct BasePopupData
		{
			public string popupName;
			public BasePopup basePopup;
		}
		
		[System.Serializable]
		internal struct Popup
		{
			public BasePopup BasePopup { get; init; }
			public GameObject MainGameObject { get; init; }
			public GameObject DimGameObject { get; init; }
			public int DefaultSortOrder { get; init; }
			public Canvas MainGameObjectCanvas { get; init; }
			public int CurrentSortOrder { get; set; }
		}

		[SerializeField] private int popupSortOrderOffset = 900;
		[SerializeField] private List<BasePopupData> basePopups;
		[SerializeField] private GameObject dimPrefab;
		
		private readonly Dictionary<int, Popup> basePopupDictionary = new();
		private readonly Stack<Popup> basePopupUseHistory = new();
		
		/// <summary>
		/// Call when want to show popup
		/// </summary>
		/// <param name="popupName">To show popup name</param>
		/// <typeparam name="T">The popup that inheritance BasePopup class type</typeparam>
		/// <returns>T</returns>
		public T GetBasePopup<T>(string popupName) where T : BasePopup
		{
			var key = popupName.GetHashCode();

			if (PopupNameValidation(key, out var result) is false)
			{
				Log.Print($"There is no popup named [{popupName}].", LogPriority.Error);
				
				return null;
			}

			if (basePopupUseHistory.Count < 1)
			{
				result.DimGameObject.SetActive(true);
				result.MainGameObjectCanvas.sortingOrder = basePopupUseHistory.Peek().CurrentSortOrder;
				basePopupUseHistory.Push(result);

				return result.BasePopup as T;
			}
			
			result.DimGameObject.SetActive(true);
			result.MainGameObjectCanvas.sortingOrder = basePopupUseHistory.Peek().CurrentSortOrder + 1;
			basePopupUseHistory.Push(result);
				
			return result.BasePopup as T;
		}

		/// <summary>
		/// Call when popup is hide
		/// </summary>
		public void PopHistory()
		{
			if (basePopupUseHistory.Count < 1)
			{
				Log.Print("There is no turn off popups", LogPriority.Error);
				
				return;
			}

			var result = basePopupUseHistory.Pop();
			
			result.DimGameObject.SetActive(false);
			result.MainGameObjectCanvas.sortingOrder = result.DefaultSortOrder;
			result.CurrentSortOrder = result.DefaultSortOrder;
			
			result.BasePopup.Hide();
		}
		
		protected override void Awake()
		{
			base.Awake();
			
			DataProcessing();
		}

		private void DataProcessing()
		{
			foreach (var data in basePopups.Select((basePopup, index) => (basePopup, index)))
			{
				var k = data.basePopup.popupName.GetHashCode();
				
				var mainGameObject = new GameObject(data.basePopup.popupName);
				mainGameObject.transform.parent = transform;
				
				var dimGameObject = Instantiate(dimPrefab, mainGameObject.transform);
				
				var mainGameObjectCanvas = mainGameObject.AddComponent<Canvas>();
				mainGameObjectCanvas.overrideSorting = true;
				mainGameObjectCanvas.sortingOrder = popupSortOrderOffset + data.index;
				
				var newPopup = new Popup
				{
					DefaultSortOrder = popupSortOrderOffset + data.index, BasePopup = data.basePopup.basePopup,
					MainGameObject = mainGameObject,
					DimGameObject = dimGameObject,
					CurrentSortOrder = popupSortOrderOffset + data.index,
					MainGameObjectCanvas = mainGameObjectCanvas
				};

				basePopupDictionary.Add(k, newPopup);
			}
		}

		private bool PopupNameValidation(int k, out Popup bP)
		{
			if (basePopupDictionary.TryGetValue(k, out var value))
			{
				bP = value;

				return true;
			}
			
			bP = default;
			
			return false;
		}
	}
}