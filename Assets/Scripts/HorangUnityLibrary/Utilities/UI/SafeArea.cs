using Horang.HorangUnityLibrary.Utilities.CustomAttribute;
using Horang.HorangUnityLibrary.Utilities.UnityExtensions;
using UnityEngine;

namespace Horang.HorangUnityLibrary.Utilities.UI
{
	[RequireComponent(typeof(RectTransform))]
	[InspectorHideScriptField]
	public class SafeArea : MonoBehaviour
	{
		[Header("Settings")]
		public bool conformLeft = true;
		public bool conformRight = true;
		public bool conformTop = true;
		public bool conformBottom = true;
		[Header("Editor debug options")]
		public bool showSafeAreaSize = true;
		public bool showCornerMarkers = true;

		private Rect lastSafeArea;
		private Vector2 lastScreenSize;
		private ScreenOrientation lastScreenOrientation;
		private RectTransform targetRectTransform;
		private Texture2D cornerMarkTexture;

		private void Awake()
		{
			targetRectTransform = GetComponent<RectTransform>();

			Refresh();
		}

		private void Start()
		{
#if UNITY_EDITOR
			cornerMarkTexture = new Texture2D(1, 1);
			cornerMarkTexture.SetPixel(0, 0, ColorExtension.Rgba256ToColor(new ColorFormat256 { a = 190, r = 255, g = 255, b = 0 }));
			cornerMarkTexture.Apply();
#endif
		}

		private void Update()
		{
			Refresh();
		}

		private void Refresh()
		{
			var safeArea = Screen.safeArea;

			if (safeArea.Equals(lastSafeArea) 
			    && Screen.width.Equals(lastScreenSize.x) && Screen.height.Equals(lastScreenSize.y)
			    && Screen.orientation.Equals(lastScreenOrientation))
			{
			    return;
			}

			lastScreenSize.x = Screen.width;
			lastScreenSize.y = Screen.height;
			lastScreenOrientation = Screen.orientation;

			ApplySafeArea(safeArea);
		}

		private void ApplySafeArea(Rect r)
		{
			if (Screen.width <= 0 || Screen.height <= 0)
			{
			    Log.Print("Cannot apply safe area cause screen size is 0.", LogPriority.Error);
			    
			    return;
			}

			var aMin = r.position;
			var aMax = aMin + r.size;

			if (conformRight is false)
			{
				aMax.x = Screen.width;
			}

			if (conformLeft is false)
			{
				aMin.x = 0;
			}

			if (conformTop is false)
			{
				aMax.y = Screen.height;
			}

			if (conformBottom is false)
			{
				aMin.y = 0;
			}

			if (aMin is not { x: >= 0, y: >= 0 } || aMax is not { x: >= 0, y: >= 0 })
			{
			    return;
			}
			
			aMin.x /= Screen.width;
			aMin.y /= Screen.height;
			aMax.x /= Screen.width;
			aMax.y /= Screen.height;

			targetRectTransform.anchorMin = aMin;
			targetRectTransform.anchorMax = aMax;

			r.min = aMin;
			r.max = aMax;

			lastSafeArea = r;
		}

#if UNITY_EDITOR
		private void OnGUI()
		{
			if (showSafeAreaSize)
			{
				ShowSafeAreaSize();
			}

			if (showCornerMarkers)
			{
				ShowCornerMarkers();
			}
		}

		private void ShowSafeAreaSize()
		{
			const int size = 30;
			
			var r = new Rect(
				lastSafeArea.xMin * Screen.width, 
				lastSafeArea.yMin * Screen.height,
				Screen.safeArea.width,
				size);
			var s = new GUIStyle
			{
				fontSize = size,
				normal =
				{
					textColor = ColorExtension.Rgba256ToColor(new ColorFormat256 { a = 190, r = 255, g = 0, b = 0 })
				},
				alignment = TextAnchor.UpperCenter
			};

			GUI.Label(r, $"[Safe Area] Width: {lastSafeArea.max.x * Screen.width} / Height: {lastSafeArea.max.y * Screen.height}", s);
		}

		private void ShowCornerMarkers()
		{
			var l = lastSafeArea.xMin * Screen.width;
			var r = lastSafeArea.xMax * Screen.width;
			var t = lastSafeArea.yMin * Screen.height;
			var b = lastSafeArea.yMax * Screen.height;
			
			const int size = 50;

			var lt = new Rect(l, t, size, size);
			var rt = new Rect(r - size, t, size, size);
			var lb = new Rect(l, b - size, size, size);
			var rb = new Rect(r - size, b - size, size, size);

			GUI.skin.box.normal.background = cornerMarkTexture;

			GUI.Box(lt, GUIContent.none);
			GUI.Box(rt, GUIContent.none);
			GUI.Box(lb, GUIContent.none);
			GUI.Box(rb, GUIContent.none);
		}
#endif
	}
}