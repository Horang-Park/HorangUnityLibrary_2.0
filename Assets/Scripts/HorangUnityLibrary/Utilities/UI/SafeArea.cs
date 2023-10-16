using System;
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
        
        private RectTransform targetRectTransform;
        private Rect lastSafeArea;
        private Vector2Int lastScreenSize;
        private ScreenOrientation lastScreenOrientation;
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
            cornerMarkTexture.SetPixel(0,0, Color.yellow);
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
            lastSafeArea = r;

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

            aMin.x /= Screen.width;
            aMin.y /= Screen.height;
            aMax.x /= Screen.width;
            aMax.y /= Screen.height;

            if (aMin is not { x: >= 0, y: >= 0 } || aMax is not { x: >= 0, y: >= 0 })
            {
                return;
            }
            
            targetRectTransform.anchorMin = aMin;
            targetRectTransform.anchorMax = aMax;
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
            var ratio = (float)Screen.width / Screen.height;
            var r = new Rect(Screen.safeArea.x, Screen.safeArea.y, Screen.safeArea.width, Screen.safeArea.height);
            var s = new GUIStyle
            {
                fontSize = (int)(30 * ratio),
                normal =
                {
                    textColor = ColorExtension.Rgba256ToColor(new ColorFormat256 {a = 255, r = 255, g = 0, b = 0})
                },
                alignment = TextAnchor.UpperCenter
            };

            GUI.Label(r, $"[Safe Area] Width: {lastSafeArea.width} / Height: {lastSafeArea.height}", s);
        }

        private void ShowCornerMarkers()
        {
            var x = lastSafeArea.x;
            var y = lastSafeArea.y;
            var w = lastSafeArea.width;
            var h = lastSafeArea.height;
            
            var lt = new Rect(x, y, 100, 100);
            var rt = new Rect(x + w - 100, y, 100, 100);
            var lb = new Rect(x, y + h - 100, 100, 100);
            var rb = new Rect(x + w - 100, y + h - 100, 100, 100);
            
            GUI.skin.box.normal.background = cornerMarkTexture;
            
            GUI.Box(lt, GUIContent.none);
            GUI.Box(rt, GUIContent.none);
            GUI.Box(lb, GUIContent.none);
            GUI.Box(rb, GUIContent.none);
        }
#endif
    }
}
