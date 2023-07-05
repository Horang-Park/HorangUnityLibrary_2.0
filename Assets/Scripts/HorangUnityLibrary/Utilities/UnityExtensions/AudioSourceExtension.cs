using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Horang.HorangUnityLibrary.Utilities.UnityExtensions
{
	public static class AudioSourceExtension
	{
		public static void OnPlayEndEventTrigger(this AudioSource audioSource, Action onPlayEnd, CancellationToken cancellationToken)
		{
			UniTask.Void(() => OnPlayEnd(audioSource.clip.length, onPlayEnd, cancellationToken));
		}

		private static async UniTaskVoid OnPlayEnd(float clipLength, Action action, CancellationToken cancellationToken)
		{
			await UniTask.Delay(TimeSpan.FromSeconds(clipLength), cancellationToken: cancellationToken);
			
			action?.Invoke();
		}
	}
}