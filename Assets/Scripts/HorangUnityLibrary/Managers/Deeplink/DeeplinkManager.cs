using System.Collections.Generic;
using HorangUnityLibrary.Foundation.Manager;
using HorangUnityLibrary.Utilities;
using UnityEngine;

namespace HorangUnityLibrary.Managers.Deeplink
{
	public abstract class DeeplinkManager : MonoBaseManager
	{
		public List<string> deeplinkParameters = new();
		public char deeplinkSeparator;

		protected override void Awake()
		{
			base.Awake();

			if (DeeplinkManagerValidation() is false)
			{
				Log.Print("Deeplink manager validation is failed.", LogPriority.Error);

				return;
			}

			Application.deepLinkActivated += OnDeeplinkActivated;

			if (string.IsNullOrEmpty(Application.absoluteURL))
			{
				Log.Print("Deeplink url is empty or null.", LogPriority.Error);

				return;
			}
			
			OnDeeplinkActivated(Application.absoluteURL);
		}

		protected abstract void OnDeeplinkActivated(string deeplinkUrl);

		private bool DeeplinkManagerValidation()
		{
			if (deeplinkParameters.Count < 1)
			{
				Log.Print("There is no deeplink parameter(s).", LogPriority.Error);

				return false;
			}

			// ReSharper disable once InvertIf
			if (deeplinkSeparator.Equals(null) || char.IsWhiteSpace(deeplinkSeparator))
			{
				Log.Print("Invalid deeplink separator.", LogPriority.Error);

				return false;
			}

			return true;
		}
	}
}