using System;
using System.Collections.Generic;
using System.Reflection;
using Horang.HorangUnityLibrary.Utilities;
using UniRx;
using UnityEngine.UI;

namespace Horang.HorangUnityLibrary.ComponentValueProviders.UI
{
	public static class ImageProvider
	{
		public static void Subscribe(this Image image, Action<float> target)
		{
			var key = target.Method;
			
			if (subscribers.ContainsKey(key))
			{
				Log.Print($"Already subscribed method. [{target.Method.Name}]", LogPriority.Error);

				return;
			}
			
			subscribers.Add(key, Observable.EveryUpdate()
				.DistinctUntilChanged(l => UpdateCheck(l, image))
				.Subscribe(_ => target.Invoke(image.fillAmount)));
		}

		public static void Unsubscribe(this Image _, Action<float> target)
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

		private static float beforeFillAmount;

		private static bool UpdateCheck(long _, Image target)
		{
			if (beforeFillAmount.Equals(target.fillAmount))
			{
				return false;
			}

			beforeFillAmount = target.fillAmount;

			return true;
		}
	}
}