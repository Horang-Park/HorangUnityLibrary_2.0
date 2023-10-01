using System.Collections.Generic;
using Horang.HorangUnityLibrary.Foundation.Module;
using Horang.HorangUnityLibrary.Utilities;
using UnityEngine;

namespace Horang.HorangUnityLibrary.Modules.ExternalApplicationLaunchModule
{
#pragma warning disable CS0162
	public sealed class ExternalApplicationLaunchModule : BaseModule
	{
		private AndroidJavaObject unityActivity;
		private AndroidJavaObject androidPackageManager;

		private const string GetIntentMethodName = "getLaunchIntentForPackage";
		private const string AddExtraMethodName = "putExtra";

		internal override void OnInitialize()
		{
#if !UNITY_ANDROID || UNITY_EDITOR
			Log.Print("This module working only built Android application.", LogPriority.Error);

			return;
#endif
			var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
			androidPackageManager = unityActivity.Call<AndroidJavaObject>("getPackageManager");
		}

		internal override void Dispose()
		{
			unityActivity?.Dispose();
			androidPackageManager?.Dispose();
		}

		public void LaunchExternalApplication(string applicationPackageName)
		{
			var externalAppIntent = androidPackageManager.Call<AndroidJavaObject>(GetIntentMethodName, applicationPackageName);
			
			externalAppIntent.Call("startActivity", unityActivity);
			
			unityActivity.Call("finish");
		}

		public void LaunchExternalApplication(string applicationPackageName, Dictionary<string, string> extraDatas)
		{
			var externalAppIntent = androidPackageManager.Call<AndroidJavaObject>(GetIntentMethodName, applicationPackageName);

			foreach (var extraData in extraDatas)
			{
				externalAppIntent.Call<AndroidJavaObject>(AddExtraMethodName, extraData.Key, extraData.Value);
			}
		
			externalAppIntent.Call("startActivity", unityActivity);
			
			unityActivity.Call("finish");
		}
	}
#pragma warning restore CS0162
}