using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Android;
using Application = UnityEngine.Device.Application;

namespace Horang.HorangUnityLibrary.Utilities
{
	public struct Permissions
	{
		/// <summary>
		/// Check this application has camera permission already.
		/// This function works only Android, iOS.
		/// Other platform will return true always.
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
		/// This function works only Android, iOS.
		/// Other platform will return true always.
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
		/// Check this application has external storage read permission already.
		/// This function works only built Android application.
		/// Other platform will return true always.
		/// also you must add read permission code in your AndroidManifest.xml file.
		/// </summary>
		/// <returns>If it's application already have external storage read permission, will return true. otherwise false</returns>
		public static bool HasExternalStorageReadPermission()
		{
#if UNITY_ANDROID
			return Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead);
#else
			Log.Print($"Unsupported platform. [{Application.platform}) Always return true.", LogPriority.Warning);
			return true;
#endif
		}
		
		/// <summary>
		/// Check this application has external storage write permission already.
		/// This function works only built Android application.
		/// Other platform will return true always.
		/// also you must add write permission code in your AndroidManifest.xml file.
		/// </summary>
		/// <returns>If it's application already have external storage read permission, will return true. otherwise false</returns>
		public static bool HasExternalStorageWritePermission()
		{
#if UNITY_ANDROID
			return Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite);
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
		
		/// <summary>
		/// Request external storage read permission.
		/// This function work only built Android application.
		/// </summary>
		/// <param name="onDenied">If user selected denied callback (Only use in Android)</param>
		/// <param name="onDeniedAndDoNotAskAgain">If user selected denied and don't ask again callback (Only use in Android)</param>
		/// <param name="onGranted">If use selected granted (Only use in Android)</param>
		/// <returns>Frame ready</returns>
		public static IEnumerator RequestExternalStorageReadPermission(Action<string> onDenied, Action<string> onDeniedAndDoNotAskAgain, Action<string> onGranted = null)
		{
#if UNITY_ANDROID
			var callbacks = new PermissionCallbacks();
			callbacks.PermissionDenied += onDenied;
			callbacks.PermissionDeniedAndDontAskAgain += onDeniedAndDoNotAskAgain;
			callbacks.PermissionGranted += onGranted;

			Permission.RequestUserPermission(Permission.ExternalStorageRead, callbacks);
			yield return null;
#else
			Log.Print($"Unsupported platform. [{Application.platform}) Always bypass.", LogPriority.Warning);
			yield return null;
#endif
		}
		
		/// <summary>
		/// Request external storage write permission.
		/// This function work only built Android application.
		/// </summary>
		/// <param name="onDenied">If user selected denied callback (Only use in Android)</param>
		/// <param name="onDeniedAndDoNotAskAgain">If user selected denied and don't ask again callback (Only use in Android)</param>
		/// <param name="onGranted">If use selected granted (Only use in Android)</param>
		/// <returns>Frame ready</returns>
		public static IEnumerator RequestExternalStorageWritePermission(Action<string> onDenied, Action<string> onDeniedAndDoNotAskAgain, Action<string> onGranted = null)
		{
#if UNITY_ANDROID
			var callbacks = new PermissionCallbacks();
			callbacks.PermissionDenied += onDenied;
			callbacks.PermissionDeniedAndDontAskAgain += onDeniedAndDoNotAskAgain;
			callbacks.PermissionGranted += onGranted;

			Permission.RequestUserPermission(Permission.ExternalStorageWrite, callbacks);
			yield return null;
#else
			Log.Print($"Unsupported platform. [{Application.platform}) Always bypass.", LogPriority.Warning);
			yield return null;
#endif
		}
	}
}