using System;
using System.Collections.Generic;
using System.Reflection;
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
			var key = target.Method;
			
			if (subscribers.ContainsKey(key))
			{
				Log.Print($"Already subscribed method. [{target.Method.Name}]", LogPriority.Error);

				return;
			}
			
			subscribers.Add(key, Observable.EveryUpdate()
				.DistinctUntilChanged(l => NormalizeUpdateCheck(l, scrollRect.normalizedPosition))
				.Subscribe(_ => target?.Invoke(scrollRect.normalizedPosition)));
		}
		
		public static void Unsubscribe(this ScrollRect _, Action<Vector2> target)
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

		private static Vector2 beforeNormalizePosition;

		private static bool NormalizeUpdateCheck(long _, Vector2 current)
		{
			if (beforeNormalizePosition.Equals(current))
			{
				return false;
			}

			beforeNormalizePosition = current;

			return true;
		}
	}
}