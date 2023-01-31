using System;
using System.Linq;
using HorangUnityLibrary.Utilities;

namespace HorangUnityLibrary.Managers.NumberImageFont
{
	public class NumberImageFontManager : MonoBaseManager
	{
		private Digit[] digits;

		public void SetValue(int value)
		{
			var digitIndex = 0;

			while (value > 0)
			{
				if (digitIndex >= digits.Length)
				{
					break;
				}
				
				digits[digitIndex++].Value = value % 10;
				value /= 10;
			}
		}

		protected override void Awake()
		{
			base.Awake();

			digits = GetComponentsInChildren<Digit>();

			if (digits.Length >= 1)
			{
				return;
			}
			
			Log.Print("Cannot find Digit components in children. This component will be destroy.", LogPriority.Error);
				
			Destroy(this);
		}

		private void Start()
		{
			for (var index = 1; index < digits.Length; index++)
			{
				var position = digits[index].transform.position;
				position.x += digits[index - 1].Bound.x * index;
				digits[index].transform.position = position;
			}

			var sortList = digits.ToList();
			sortList.Sort((digitLhs, digitRhs) => string.Compare(digitRhs.name, digitLhs.name, StringComparison.CurrentCultureIgnoreCase));
			digits = sortList.ToArray();
		}
	}
}