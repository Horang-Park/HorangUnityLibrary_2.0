using System;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Horang.HorangUnityLibrary.Utilities
{
	public static class Screenshot
	{
		/// <summary>
		/// Shot target camera view.
		/// Beware set target camera name before calling this method.
		/// </summary>
		/// <param name="captureCameraName">For camera name to take screenshot</param>
		/// <param name="textureName">To save texture name. it will add date, time and extension automatically</param>
		/// <param name="useTransparency">Set background is transparency</param>
		/// <returns>Texture2D</returns>
		/// <exception cref="InvalidOperationException">If cannot find target camera</exception>
		public static Texture2D ShotFromTargetCamera(string captureCameraName, string textureName = "Screenshot", bool useTransparency = false)
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
			var sb = new StringBuilder(textureName).Append($"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}").Append(".png");

			if (tC is null || !tC)
			{
				Log.Print("Invalid camera or cannot find target camera.", LogPriority.Exception);

				throw new InvalidOperationException();
			}
			
			textureName = sb.ToString();
			tC.targetTexture = rT;

			var sT = new Texture2D(w, h, useTransparency ? TextureFormat.RGBA32 : TextureFormat.RGB24, false);

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
			var sb = new StringBuilder(textureName).Append($"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}").Append(".png");
			
			await UniTask.DelayFrame(1);
			
			var sT = ScreenCapture.CaptureScreenshotAsTexture();
			sT.name = sb.ToString();

			return sT;
		}

		/// <summary>
		/// Screenshotting with UI RectTransform to shot specific UI area.
		/// </summary>
		/// <param name="targetRectTransform">To capture UI RectTransform</param>
		/// <param name="textureName">Capture texture name</param>
		/// <param name="useTransparency">Set background is transparency</param>
		/// <returns>Texture2D</returns>
		public static async UniTask<Texture2D> ShotSpecificUIArea(RectTransform targetRectTransform, string textureName = "Screenshot", bool useTransparency = false)
		{
			var sb = new StringBuilder(textureName).Append($"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}").Append(".png");

			await UniTask.DelayFrame(1);
			
			var corners = new Vector3[4];
			targetRectTransform.GetWorldCorners(corners);
			
			var bl = RectTransformUtility.WorldToScreenPoint(null, corners[0]);
			var tl = RectTransformUtility.WorldToScreenPoint(null, corners[1]);
			var tr = RectTransformUtility.WorldToScreenPoint(null, corners[2]);
 
			var h = tl.y - bl.y;
			var w = tr.x - bl.x;
 
			var t = new Texture2D((int)w, (int)h, useTransparency ? TextureFormat.RGBA32 : TextureFormat.RGB24, false);
			t.ReadPixels(new Rect(bl.x, bl.y, w, h), 0, 0);
			t.Apply();
			
			t.name = sb.ToString();

			return t;
		}
	}
}