using System.Collections.Generic;
using Horang.HorangUnityLibrary.Foundation.Module;
using Horang.HorangUnityLibrary.Managers.Module;
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
		
		public ExternalApplicationLaunchModule(ModuleManager moduleManager) : base(moduleManager)
		{
		}

		public override bool ActiveModule()
		{
#if !UNITY_ANDROID || UNITY_EDITOR
			Log.Print("This module working only built Android application.", LogPriority.Error);

			return false;
#endif
			
			if (base.ActiveModule() is false)
			{
				return false;
			}
			
			Log.Print("Module are activated.", LogPriority.Verbose);

			return true;
		}

		public override bool InactiveModule()
		{
#if !UNITY_ANDROID || UNITY_EDITOR
			Log.Print("This module working only built Android application.", LogPriority.Error);

			return false;
#endif
			if (base.InactiveModule() is false)
			{
				return false;
			}
			
			Log.Print("Module are inactivated.", LogPriority.Verbose);

			return true;
		}

		public override void InitializeOnce()
		{
#if !UNITY_ANDROID || UNITY_EDITOR
			Log.Print("This module working only built Android application.", LogPriority.Error);

			return;
#endif
			
			base.InitializeOnce();
			
			var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
			androidPackageManager = unityActivity.Call<AndroidJavaObject>("getPackageManager");
		}

		public void LaunchExternalApplication(string applicationPackageName)
		{
			if (isThisModuleActivated is false)
			{
				return;
			}
			
			var externalAppIntent = androidPackageManager.Call<AndroidJavaObject>(GetIntentMethodName, applicationPackageName);
			
			externalAppIntent.Call("startActivity", unityActivity);
			
			unityActivity.Call("finish");
		}

		public void LaunchExternalApplication(string applicationPackageName, Dictionary<string, string> extraDatas)
		{
			if (isThisModuleActivated is false)
			{
				return;
			}

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