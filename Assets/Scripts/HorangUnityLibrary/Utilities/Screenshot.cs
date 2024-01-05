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
		public static async UniTask<Texture2D> ShotSpecificUIAreaAsync(RectTransform targetRectTransform, string textureName = "Screenshot", bool useTransparency = false)
		{
			var topCanvas = targetRectTransform.GetComponentInParent<Canvas>();
			var beforeCanvasRenderMode = RenderMode.ScreenSpaceOverlay;
			var mainCamera = Camera.main;
			var corners = new Vector3[4];
			var targetRect = targetRectTransform.rect;

			if (mainCamera is null || !mainCamera)
			{
				Log.Print("Cannot find main camera.", LogPriority.Error);

				return null;
			}

			if (topCanvas is null || !topCanvas)
			{
				Log.Print("Cannot find parent canvas.", LogPriority.Error);

				return null;
			}

			await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);

			if (topCanvas.renderMode is RenderMode.ScreenSpaceOverlay)
			{
				beforeCanvasRenderMode = topCanvas.renderMode;
				
				topCanvas.renderMode = RenderMode.ScreenSpaceCamera;
				topCanvas.worldCamera = mainCamera;
			}

			
			targetRectTransform.GetWorldCorners(corners);

			corners[0] = mainCamera.WorldToScreenPoint(corners[0]);

			var resWidth = Screen.width;
			var resHeight = Screen.height;
			
			var cWidth = (int)targetRect.width;
			var cHeight = (int)targetRect.height;
			var cx = (int)corners[0].x;
			var cy = (int)corners[0].y;
			
			var rt = new RenderTexture(resWidth, resHeight, 24);

			mainCamera.targetTexture = rt;
			mainCamera.Render();

			RenderTexture.active = rt;

			var t = new Texture2D(cWidth, cHeight, useTransparency ? TextureFormat.RGBA32 : TextureFormat.RGB24, false)
			{
				name = textureName
			};
			
			t.ReadPixels(new Rect(cx, cy, cWidth, cHeight), 0, 0);
			t.Apply();

			topCanvas.renderMode = beforeCanvasRenderMode;
			mainCamera.targetTexture = null;
			RenderTexture.active = null;

			return t;
		}
	}
}