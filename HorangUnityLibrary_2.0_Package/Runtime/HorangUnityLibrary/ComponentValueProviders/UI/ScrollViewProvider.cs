using System;
using System.Collections.Generic;
using Horang.HorangUnityLibrary.Utilities;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Horang.HorangUnityLibrary.ComponentValueProviders.UI
{
	public static class ScrollViewProvider
	{
		public static void NormalizePositionSubscribe(this ScrollRect scrollRect, Action<Vector2> target)
		{
			var key = GetKey(target);
			
			if (Subscribers.ContainsKey(key))
			{
				Log.Print($"Already subscribed method. [{target.Method.Name}]", LogPriority.Error);

				return;
			}
			
			Subscribers.Add(key, Observable.EveryUpdate()
				.DistinctUntilChanged(l => NormalizeUpdateCheck(l, scrollRect.normalizedPosition))
				.Subscribe(_ => target?.Invoke(scrollRect.normalizedPosition)));
		}
		
		public static void Unsubscribe(this ScrollRect _, Action<Vector2> target)
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
		private static Vector2 _beforeNormalizePosition;

		private static bool NormalizeUpdateCheck(long _, Vector2 current)
		{
			if (_beforeNormalizePosition.Equals(current))
			{
				return false;
			}

			_beforeNormalizePosition = current;

			return true;
		}

		private static int GetKey(Action<Vector2> target)
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