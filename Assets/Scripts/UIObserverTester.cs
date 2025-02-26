using Horang.HorangUnityLibrary.ComponentValueProviders.UI;
using Horang.HorangUnityLibrary.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class UIObserverTester : MonoBehaviour
{
	public Slider s;
	public Image fi;
	public ScrollRect scrollRect;

	private void Start()
	{
		s.Subscribe(SliderValueLog);
		// fi.Subscribe(v => Log.Print($"image fill amount: {v}"));
		// scrollRect.NormalizePositionSubscribe(v => Log.Print($"{v}"));
	}

	protected void SliderValueLog(float v)
	{
		Log.Print($"slider value: {v}");
	}
}