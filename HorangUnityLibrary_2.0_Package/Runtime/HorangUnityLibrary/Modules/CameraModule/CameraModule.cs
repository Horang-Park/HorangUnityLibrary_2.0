using System.Collections.Generic;
using Horang.HorangUnityLibrary.Foundation.Module;
using Horang.HorangUnityLibrary.Utilities;
using UnityEngine;

namespace Horang.HorangUnityLibrary.Modules.CameraModule
{
	public sealed class CameraModule : BaseModule
	{
		private readonly Dictionary<int, CameraData> cameras = new();

		internal override void OnInitialize()
		{
			FindCameras();
		}

		internal override void Dispose()
		{
			cameras.Clear();
		}

		/// <summary>
		/// Get camera which found.
		/// </summary>
		/// <param name="cameraName">To get camera game object name</param>
		/// <returns>If camera name is valid, return its camera. otherwise null</returns>
		public Camera GetCamera(string cameraName)
		{
			var key = cameraName.GetHashCode();

			return CameraValidation(key, out var camera) ? camera.Camera : null;
		}

		private void FindCameras()
		{
			var cameraDatas = Object.FindObjectsByType<CameraData>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);

			if (cameraDatas.Length < 1)
			{
				Log.Print("Cannot find camera data.", LogPriority.Error);

				return;
			}

			foreach (var camera in cameraDatas)
			{
				var key = camera.gameObject.name.GetHashCode();

				cameras.Add(key, camera);
			}
		}

		private bool CameraValidation(int k, out CameraData cameraData)
		{
			if (cameras.TryGetValue(k, out var camera))
			{
				cameraData = camera;

				return true;
			}

			cameraData = null;

			return false;
		}
	}
}