using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Android;

namespace Horang.HorangUnityLibrary.Utilities
{
	public struct Permissions
	{
		/// <summary>
		/// Check this application has camera permission already.
		/// </summary>
		/// <returns>If it's application already have camera permission, will return true. otherwise false</returns>
		public static bool HasCameraPermission()
		{
#if UNITY_ANDROID
			return Permission.HasUserAuthorizedPermission(Permission.Camera);
#elif UNITY_IOS
			return Application.HasUserAuthorization(UserAuthorization.WebCam);
#else
			Log.Print($"Unsupported platform. [{Application.platform}) Always return true.", LogPriority.Warning);
			return true;
#endif
		}

		/// <summary>
		/// Check this application has microphone permission already.
		/// </summary>
		/// <returns>If it's application already have microphone permission, will return true. otherwise false</returns>
		public static bool HasMicrophonePermission()
		{
#if UNITY_ANDROID
			return Permission.HasUserAuthorizedPermission(Permission.Microphone);
#elif UNITY_IOS
			return Application.HasUserAuthorization(UserAuthorization.Microphone);
#else
			Log.Print($"Unsupported platform. [{Application.platform}) Always return true.", LogPriority.Warning);
			return true;
#endif
		}

		/// <summary>
		/// Request camera permission.
		/// </summary>
		/// <param name="onDenied">If user selected denied callback (Only use in Android)</param>
		/// <param name="onDeniedAndDoNotAskAgain">If user selected denied and don't ask again callback (Only use in Android)</param>
		/// <param name="onGranted">If use selected granted (Only use in Android)</param>
		/// <returns>Frame ready</returns>
		public static IEnumerator RequestCameraPermission(Action<string> onDenied, Action<string> onDeniedAndDoNotAskAgain, Action<string> onGranted = null)
		{
#if UNITY_ANDROID
			var callbacks = new PermissionCallbacks();
			callbacks.PermissionDenied += onDenied;
			callbacks.PermissionDeniedAndDontAskAgain += onDeniedAndDoNotAskAgain;
			callbacks.PermissionGranted += onGranted;

			Permission.RequestUserPermission(Permission.Camera, callbacks);
			yield return null;
#elif UNITY_IOS
			yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
#else
			Log.Print($"Unsupported platform. [{Application.platform}) Always bypass.", LogPriority.Warning);
			yield return null;
#endif
		}

		/// <summary>
		/// Request microphone permission.
		/// </summary>
		/// <param name="onDenied">If user selected denied callback (Only use in Android)</param>
		/// <param name="onDeniedAndDoNotAskAgain">If user selected denied and don't ask again callback (Only use in Android)</param>
		/// <param name="onGranted">If use selected granted (Only use in Android)</param>
		/// <returns>Frame ready</returns>
		public static IEnumerator RequestMicrophonePermission(Action<string> onDenied, Action<string> onDeniedAndDoNotAskAgain, Action<string> onGranted = null)
		{
#if UNITY_ANDROID
			var callbacks = new PermissionCallbacks();
			callbacks.PermissionDenied += onDenied;
			callbacks.PermissionDeniedAndDontAskAgain += onDeniedAndDoNotAskAgain;
			callbacks.PermissionGranted += onGranted;
			
			Permission.RequestUserPermission(Permission.Microphone, callbacks);
			yield return null;
#elif UNITY_IOS
			yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
#else
			Log.Print($"Unsupported platform. [{Application.platform}) Always bypass.", LogPriority.Warning);
			yield return null;
#endif
		}
	}
}