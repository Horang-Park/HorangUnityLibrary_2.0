using Horang.HorangUnityLibrary.ComponentValueProviders.UI;
using UnityEngine;

namespace DefaultNamespace
{
    public class UIObserverTesterChildTwo : UIObserverTester
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F2))
            {
                s.Unsubscribe(SliderValueLog);
            }
        }
    }
}