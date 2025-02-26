using Horang.HorangUnityLibrary.ComponentValueProviders.UI;
using UnityEngine;

namespace DefaultNamespace
{
    public class UIObserverTesterChildOne : UIObserverTester
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                s.Unsubscribe(SliderValueLog);
            }
        }
    }
}