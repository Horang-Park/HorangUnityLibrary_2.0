using UnityEngine;

namespace Horang.HorangUnityLibrary.Utilities.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeArea : MonoBehaviour
    {
        public bool conformLeft = true;
        public bool conformRight = true;
        public bool conformTop = true;
        public bool conformBottom = true;
        
        private RectTransform panel;
        private Rect lastSafeArea;
        private Vector2Int lastScreenSize;
        private ScreenOrientation lastScreenOrientation;

        private void Awake()
        {
            panel = GetComponent<RectTransform>();
            
            Refresh();
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
            var aMax = r.position + r.size;
            
            if (!conformRight) aMax.x = Screen.width;
            if (!conformLeft) aMin.x = 0;
            if (!conformTop) aMax.y = Screen.height;
            if (!conformBottom) aMin.y = 0;

            aMin.x /= Screen.width;
            aMin.y /= Screen.height;
            aMax.x /= Screen.width;
            aMax.y /= Screen.height;

            if (aMin is not { x: >= 0, y: >= 0 } || aMax is not { x: >= 0, y: >= 0 })
            {
                return;
            }
            
            panel.anchorMin = aMin;
            panel.anchorMax = aMax;
        }
    }
}
