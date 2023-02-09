using UnityEngine;

namespace Horang.HorangUnityLibrary.Utilities
{
#pragma warning disable CS0162
	public struct GetIntentExtraData
	{
		private const string ExtraDataValidationKey = "hasExtra";
		private const string ExtraDataGetKey = "getStringExtra";
		
		public static bool ExtraDataExistValidation(string key)
		{
#if !UNITY_ANDROID || UNITY_EDITOR
			Log.Print("This utility working only built Android application.", LogPriority.Error);

			return true;
#endif
			
			var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
			var intent = currentActivity.Call<AndroidJavaObject>("getIntent");

			return intent.Call<bool>(ExtraDataValidationKey, key);
		}
		
		public static string GetStringData(string key)
		{
#if !UNITY_ANDROID || UNITY_EDITOR
			Log.Print("This utility working only built Android application.", LogPriority.Error);

			return string.Empty;
#endif
			var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
			var intent = currentActivity.Call<AndroidJavaObject>("getIntent");

			return intent.Call<string>(ExtraDataGetKey, key);
		}
	}
#pragma warning restore CS0162
}