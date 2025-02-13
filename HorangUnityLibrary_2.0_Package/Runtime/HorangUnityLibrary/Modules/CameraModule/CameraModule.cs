using System.Collections.Generic;
using Horang.HorangUnityLibrary.Utilities;
using UnityEngine;

namespace Horang.HorangUnityLibrary.Modules.CameraModule
{
	public static class CameraModule
	{
		private static readonly Dictionary<int, CameraData> Cameras = new();

		public static void OnInitialize()
		{
			FindCameras();
		}

		public static void Dispose()
		{
			Cameras.Clear();
		}

		/// <summary>
		/// Get camera which found.
		/// </summary>
		/// <param name="cameraName">To get camera game object name</param>
		/// <returns>If camera name is valid, return its camera. otherwise null</returns>
		public static Camera GetCamera(string cameraName)
		{
			var key = cameraName.GetHashCode();

			return CameraValidation(key, out var camera) ? camera.Camera : null;
		}

		private static void FindCameras()
		{
			var cameras = Object.FindObjectsByType<CameraData>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);

			if (cameras.Length < 1)
			{
				Log.Print("Cannot find camera data.", LogPriority.Error);

				return;
			}

			foreach (var camera in cameras)
			{
				var key = camera.gameObject.name.GetHashCode();

				Cameras.Add(key, camera);
			}
		}

		private static bool CameraValidation(int k, out CameraData cameraData)
		{
			if (Cameras.TryGetValue(k, out var camera))
			{
				cameraData = camera;

				return true;
			}

			cameraData = null;

			return false;
		}
	}
}