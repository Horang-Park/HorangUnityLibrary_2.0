using System.Collections.Generic;
using Horang.HorangUnityLibrary.Utilities;
using UnityEngine;

namespace Horang.HorangUnityLibrary.Modules.ExternalApplicationLaunchModule
{
#pragma warning disable CS0162
	public static class ExternalApplicationLaunchModule
	{
		private static AndroidJavaObject _unityActivity;
		private static AndroidJavaObject _androidPackageManager;

		private const string GetIntentMethodName = "getLaunchIntentForPackage";
		private const string AddExtraMethodName = "putExtra";

		public static void OnInitialize()
		{
#if !UNITY_ANDROID || UNITY_EDITOR
			Log.Print("Working only built Android application.", LogPriority.Error);

			return;
#endif
			var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			_unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
			_androidPackageManager = _unityActivity.Call<AndroidJavaObject>("getPackageManager");
		}
		
		public static void Dispose()
		{
			_unityActivity?.Dispose();
			_androidPackageManager?.Dispose();
		}

		public static void LaunchExternalApplication(string applicationPackageName)
		{
			var externalAppIntent = _androidPackageManager.Call<AndroidJavaObject>(GetIntentMethodName, applicationPackageName);
			
			externalAppIntent.Call("startActivity", _unityActivity);
			
			_unityActivity.Call("finish");
		}

		public static void LaunchExternalApplication(string applicationPackageName, Dictionary<string, string> extraData)
		{
			var externalAppIntent = _androidPackageManager.Call<AndroidJavaObject>(GetIntentMethodName, applicationPackageName);

			foreach (var ed in extraData)
			{
				externalAppIntent.Call<AndroidJavaObject>(AddExtraMethodName, ed.Key, ed.Value);
			}
		
			externalAppIntent.Call("startActivity", _unityActivity);
			
			_unityActivity.Call("finish");
		}
	}
#pragma warning restore CS0162
}