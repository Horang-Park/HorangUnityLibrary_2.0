using System;
using System.Collections.Generic;
using Horang.HorangUnityLibrary.Utilities;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Horang.HorangUnityLibrary.ComponentValueProviders.UI
{
	public static class SliderProvider
	{
		private static readonly Dictionary<int, IDisposable> Subscribers = new();

		public static void Subscribe(this Slider slider, Action<float> target)
		{
			var key = GetKey(target);
			
			if (Subscribers.ContainsKey(key))
			{
				Log.Print($"Already subscribed method. [{target.Method.Name}]", LogPriority.Error);

				return;
			}
			
			Subscribers.Add(key, slider.OnValueChangedAsObservable()
				.DistinctUntilChanged()
				.Subscribe(target));
		}

		public static void Unsubscribe(this Slider _, Action<float> target)
		{
			var key = GetKey(target);
			
			if (Subscribers.TryGetValue(key, out var subscriber) is false)
			{
				Log.Print($"Not subscribed method. [{target.Method.Name}]", LogPriority.Error);

				return;
			}

			subscriber.Dispose();
			Subscribers.Remove(key);
		}

		private static int GetKey(Action<float> target)
		{
			var targetScript = (MonoBehaviour)target.Target;

			if (targetScript is not null)
			{
				return targetScript.gameObject.GetInstanceID();
			}

			Log.Print("Target script is not inheritance MonoBehaviour.", LogPriority.Warning);

			return int.MaxValue;
		}
	}
}