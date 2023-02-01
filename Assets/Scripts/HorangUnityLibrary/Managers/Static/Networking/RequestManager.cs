using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using HorangUnityLibrary.Utilities;
using UnityEngine.Networking;

namespace HorangUnityLibrary.Managers.Static.Networking
{
	public static class RequestManager
	{
		private static CancellationTokenSource delayWaiterCancellationTokenSource = new();
		
		public static async UniTaskVoid Send(
			UnityWebRequest www,
			Action<string> onSuccess,
			Action onDelay = null,
			Action<long, string> onFailure = null,
			Action<float> onProgress = null,
			double timeout = 5000D,
			double delayTimeout = 3000D)
		{
			Log.Print($"URI: {www.uri.AbsoluteUri}, API method: {www.method}", LogPriority.Verbose);
			
			Delay(onDelay, delayTimeout).Forget();

			try
			{
				www = await www.SendWebRequest().ToUniTask(Progress.Create(onProgress)).Timeout(TimeSpan.FromMilliseconds(timeout));
			}
			catch (UnityWebRequestException e)
			{
				CancelDelayTask();
				onFailure?.Invoke(e.UnityWebRequest.responseCode, e.UnityWebRequest.error);
				www.Dispose();

				return;
			}
			catch (Exception e)
			{
				CancelDelayTask();
				onFailure?.Invoke(e.HResult, e.Message);
				www.Dispose();

				return;
			}

			CancelDelayTask();
			Log.Print($"URI: {www.uri.AbsoluteUri}, Response Code: {www.responseCode}", LogPriority.Verbose);
			onSuccess?.Invoke(www.downloadHandler.text);
			
			www.Dispose();
		}

		private static async UniTaskVoid Delay(Action oD, double tO)
		{
			await UniTask.Delay(
				TimeSpan.FromMilliseconds(tO),
				DelayType.DeltaTime,
				PlayerLoopTiming.Update,
				delayWaiterCancellationTokenSource.Token);
			
			oD?.Invoke();
		}

		private static void CancelDelayTask()
		{
			delayWaiterCancellationTokenSource.Cancel();
			delayWaiterCancellationTokenSource.Dispose();
			delayWaiterCancellationTokenSource = new CancellationTokenSource();
		}
	}
}