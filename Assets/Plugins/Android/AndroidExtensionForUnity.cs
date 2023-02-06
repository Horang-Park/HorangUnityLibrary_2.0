using Horang.HorangUnityLibrary.Utilities;
using UnityEngine;

namespace Plugins.Android
{
	public static class AndroidExtensionForUnity
	{
		[System.Serializable]
		public record PackageVersion
		{
			public string versionCode;
			public string versionName;
		}
		
		public static void SendBroadcast(string broadcastNativeUrl, string targetPackageName)
		{
#if !UNITY_ANDROID || UNITY_EDITOR
			Log.Print("This function working only built Android application.", LogPriority.Error);

			return;
#endif
			
			AndroidJavaObject activityContext;
			AndroidJavaClass javaClass;
			AndroidJavaObject javaClassInstance = null;
			
			using (var activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
			}

			using (javaClass = new AndroidJavaClass("com.changsoopark.androidextensionforunity.NativeBroadcasting"))
			{
				if (javaClass is not null)
				{
					javaClassInstance = javaClass.CallStatic<AndroidJavaObject>("instance");

					javaClassInstance.Call("setContext", activityContext);
				}
			}
		
			javaClassInstance.Call("SendBroadcast", broadcastNativeUrl, targetPackageName);
		}

		public static PackageVersion GetPackageVersion(string targetPackageName)
		{
#if !UNITY_ANDROID || UNITY_EDITOR
			Log.Print("This function working only built Android application.", LogPriority.Error);

			return null;
#endif
			
			AndroidJavaObject activityContext;
			AndroidJavaClass javaClass;
			AndroidJavaObject javaClassInstance = null;
			
			using (var activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
			}

			using (javaClass = new AndroidJavaClass("com.changsoopark.androidextensionforunity.NativeGetApplicationVersion"))
			{
				if (javaClass is not null)
				{
					javaClassInstance = javaClass.CallStatic<AndroidJavaObject>("instance");

					javaClassInstance.Call("setContext", activityContext);
				}
			}
		
			var res = javaClassInstance.Call<string>("GetApplicationVersion", targetPackageName);

			return JsonParser.JsonParsing<PackageVersion>(res);
		}

		public static string GetHardwareId()
		{
#if !UNITY_ANDROID || UNITY_EDITOR
			Log.Print("This function working only built Android application.", LogPriority.Error);

			return string.Empty;
#endif
			AndroidJavaObject activityContext;
			AndroidJavaClass javaClass;
			AndroidJavaObject javaClassInstance = null;
			
			using (var activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
			}

			using (javaClass = new AndroidJavaClass("com.changsoopark.androidextensionforunity.NativeGetSSAID"))
			{
				if (javaClass is not null)
				{
					javaClassInstance = javaClass.CallStatic<AndroidJavaObject>("instance");

					javaClassInstance.Call("setContext", activityContext);
				}
			}

			return javaClassInstance.Call<string>("GetAndroidSSAID");
		}
	}
}