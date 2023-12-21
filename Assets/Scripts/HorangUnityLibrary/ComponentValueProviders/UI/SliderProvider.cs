using System;
using System.Collections.Generic;
using System.Reflection;
using Horang.HorangUnityLibrary.Utilities;
using UniRx;
using UnityEngine.UI;

namespace Horang.HorangUnityLibrary.ComponentValueProviders.UI
{
	public static class SliderProvider
	{
		public static void Subscribe(this Slider slider, Action<float> target)
		{
			var key = target.Method;
			
			if (subscribers.ContainsKey(key))
			{
				Log.Print($"Already subscribed method. [{target.Method.Name}]", LogPriority.Error);

				return;
			}
			
			subscribers.Add(key, slider.OnValueChangedAsObservable()
				.DistinctUntilChanged()
				.Subscribe(target));
		}

		public static void Unsubscribe(this Slider _, Action<float> target)
		{
			var key = target.Method;
			
			if (subscribers.ContainsKey(key) is false)
			{
				Log.Print($"Not subscribed method. [{target.Method.Name}]", LogPriority.Error);

				return;
			}

			var subscriber = subscribers[target.Method];
			subscriber.Dispose();
			subscribers.Remove(target.Method);
		}
		
		private static readonly Dictionary<MethodInfo, IDisposable> subscribers = new();
	}
}