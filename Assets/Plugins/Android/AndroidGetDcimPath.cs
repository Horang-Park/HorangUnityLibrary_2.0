using UnityEngine;

namespace Plugins.Android
{
#pragma warning disable CS0162
	public static class AndroidGetDcimPath
	{
		public static string GetAndroidDcimPath()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			var javaClass = new AndroidJavaClass("android.os.Environment");
			var path = javaClass.CallStatic<AndroidJavaObject>("getExternalStoragePublicDirectory", 
					javaClass.GetStatic<string>("DIRECTORY_DCIM"))
				.Call<string>("getAbsolutePath");
			
			return path;
#else
			return Application.persistentDataPath;
#endif
		}
	}
#pragma warning restore CS0162
}