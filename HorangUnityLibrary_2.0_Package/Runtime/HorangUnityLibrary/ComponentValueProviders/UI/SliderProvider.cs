using System;
using System.Collections.Generic;
using Horang.HorangUnityLibrary.Utilities;
using UniRx;
using UnityEngine.UI;

namespace Horang.HorangUnityLibrary.ComponentValueProviders.UI
{
	public static class SliderProvider
	{
		public static void Subscribe(this Slider slider, Action<float> target)
		{
			var key = target.Method.MetadataToken;
			
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
			var key = target.Method.MetadataToken;
			
			if (Subscribers.ContainsKey(key) is false)
			{
				Log.Print($"Not subscribed method. [{target.Method.Name}]", LogPriority.Error);

				return;
			}

			var subscriber = Subscribers[target.Method.MetadataToken];
			subscriber.Dispose();
			Subscribers.Remove(target.Method.MetadataToken);
		}
		
		private static readonly Dictionary<int, IDisposable> Subscribers = new();
	}
}