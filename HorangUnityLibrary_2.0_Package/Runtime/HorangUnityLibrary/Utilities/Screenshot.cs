using System;
using System.Collections;
using System.Text;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Horang.HorangUnityLibrary.Utilities
{
	public static class Screenshot
	{
		private static string captureCameraName;
		public static string CaptureCameraName
		{
			set => captureCameraName = value;
		}

		/// <summary>
		/// Shot target camera view.
		/// Beware set target camera name before calling this method.
		/// </summary>
		/// <param name="textureName">To save texture name. it will add date, time and extension automatically</param>
		/// <returns>Texture2D</returns>
		/// <exception cref="InvalidOperationException">If cannot find target camera</exception>
		public static Texture2D ShotFromTargetCamera(string textureName = "Screenshot")
		{
			if (string.IsNullOrEmpty(captureCameraName))
			{
				Log.Print("Set capture target camera name before shot.", LogPriority.Error);

				return null;
			}

			var h = Screen.height;
			var w = Screen.width;
			var tC = GameObject.Find(captureCameraName).GetComponent(typeof(Camera)) as Camera;
			var rT = new RenderTexture(w, h, 24);
			var sb = new StringBuilder(textureName).Append($"{DateTime.Now.ToString(" yyyy-MM-dd_HH-mm-ss")}").Append(".png");

			if (tC is null || !tC)
			{
				Log.Print("Invalid camera or cannot find target camera.", LogPriority.Exception);

				throw new InvalidOperationException();
			}
			
			textureName = sb.ToString();
			tC.targetTexture = rT;

			var sT = new Texture2D(w, h, TextureFormat.RGBA32, false);

			tC.Render();

			RenderTexture.active = rT;
			
			sT.ReadPixels(new Rect(0.0f, 0.0f, w, h), 0, 0);
			sT.Apply(false);
			sT.name = textureName;

			return sT;
		}

		/// <summary>
		/// Screenshotting whole presenting screen.
		/// It's delaying 1 frame.
		/// </summary>
		/// <param name="textureName">To save texture name. it will add date, time and extension automatically</param>
		/// <returns>UniTask's texture 2d</returns>
		public static async UniTask<Texture2D> ShotWholeScreenAsync(string textureName = "Screenshot")
		{
			var sb = new StringBuilder(textureName).Append($"{DateTime.Now.ToString(" yyyy-MM-dd_HH-mm-ss")}").Append(".png");
			
			await UniTask.DelayFrame(1);
			
			var sT = ScreenCapture.CaptureScreenshotAsTexture();
			sT.name = sb.ToString();

			return sT;
		}
	}
}