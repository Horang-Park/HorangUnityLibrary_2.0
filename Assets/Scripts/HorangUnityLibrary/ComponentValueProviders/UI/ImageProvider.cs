using System;
using System.Collections.Generic;
using System.Reflection;
using Horang.HorangUnityLibrary.Utilities;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Horang.HorangUnityLibrary.ComponentValueProviders.UI
{
	public static class ImageProvider
	{
		public static void Subscribe(this Image image, Action<float> target)
		{
			var key = GetKey(target);
			
			if (Subscribers.ContainsKey(key))
			{
				Log.Print($"Already subscribed method. [{target.Method.Name}]", LogPriority.Error);

				return;
			}
			
			Subscribers.Add(key, Observable.EveryUpdate()
				.DistinctUntilChanged(l => UpdateCheck(l, image))
				.Subscribe(_ => target.Invoke(image.fillAmount)));
		}

		public static void Unsubscribe(this Image _, Action<float> target)
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
		
		private static readonly Dictionary<int, IDisposable> Subscribers = new();
		private static float _beforeFillAmount;

		private static bool UpdateCheck(long _, Image target)
		{
			if (_beforeFillAmount.Equals(target.fillAmount))
			{
				return false;
			}

			_beforeFillAmount = target.fillAmount;

			return true;
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