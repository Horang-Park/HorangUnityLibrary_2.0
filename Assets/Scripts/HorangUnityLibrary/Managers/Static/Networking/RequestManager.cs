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
		
		/// <summary>
		/// Send web request to remote.
		/// </summary>
		/// <param name="unityWebRequest">To send web request</param>
		/// <param name="onSuccess">Calling when request is successfully worked and callback json data</param>
		/// <param name="onDelay">Calling when request is going delay over delayTimeout parameter</param>
		/// <param name="onFailure">Calling when occurred error on requesting</param>
		/// <param name="onProgress">To get percentage of web request's progress (0~1)</param>
		/// <param name="timeout">To setting request's timeout</param>
		/// <param name="delayTimeout">To setting request's delay timeout</param>
		public static async UniTaskVoid Send(
			UnityWebRequest unityWebRequest,
			Action<string> onSuccess,
			Action onDelay = null,
			Action<long, string> onFailure = null,
			Action<float> onProgress = null,
			double timeout = 30000D,
			double delayTimeout = 3000D)
		{
			Log.Print($"URI: {unityWebRequest.uri.AbsoluteUri}, API method: {unityWebRequest.method}", LogPriority.Verbose);
			
			Delay(onDelay, delayTimeout).Forget();

			try
			{
				unityWebRequest = await unityWebRequest.SendWebRequest()
					.ToUniTask(Progress.CreateOnlyValueChanged(onProgress))
					.Timeout(TimeSpan.FromMilliseconds(timeout));
			}
			catch (UnityWebRequestException e)
			{
				CancelDelayTask();
				onFailure?.Invoke(e.UnityWebRequest.responseCode, e.UnityWebRequest.error);
				unityWebRequest.Dispose();

				return;
			}
			catch (Exception e)
			{
				CancelDelayTask();
				onFailure?.Invoke(e.HResult, e.Message);
				unityWebRequest.Dispose();

				return;
			}

			CancelDelayTask();
			Log.Print($"URI: {unityWebRequest.uri.AbsoluteUri}, Response Code: {unityWebRequest.responseCode}", LogPriority.Verbose);
			onSuccess?.Invoke(unityWebRequest.downloadHandler.text);
			
			unityWebRequest.Dispose();
		}
		
		/// <summary>
		/// Send web request to remote.
		/// </summary>
		/// <param name="unityWebRequest">To send web request</param>
		/// <param name="onSuccess">Calling when request is successfully worked and callback byte array data</param>
		/// <param name="onDelay">Calling when request is going delay over delayTimeout parameter</param>
		/// <param name="onFailure">Calling when occurred error on requesting</param>
		/// <param name="onProgress">To get percentage of web request's progress (0~1)</param>
		/// <param name="timeout">To setting request's timeout</param>
		/// <param name="delayTimeout">To setting request's delay timeout</param>
		public static async UniTaskVoid Send(
			UnityWebRequest unityWebRequest,
			Action<byte[]> onSuccess,
			Action onDelay = null,
			Action<long, string> onFailure = null,
			Action<float> onProgress = null,
			double timeout = 30000D,
			double delayTimeout = 3000D)
		{
			Log.Print($"URI: {unityWebRequest.uri.AbsoluteUri}, API method: {unityWebRequest.method}", LogPriority.Verbose);
			
			Delay(onDelay, delayTimeout).Forget();

			try
			{
				unityWebRequest = await unityWebRequest.SendWebRequest()
					.ToUniTask(Progress.CreateOnlyValueChanged(onProgress))
					.Timeout(TimeSpan.FromMilliseconds(timeout));
			}
			catch (UnityWebRequestException e)
			{
				CancelDelayTask();
				onFailure?.Invoke(e.UnityWebRequest.responseCode, e.UnityWebRequest.error);
				unityWebRequest.Dispose();

				return;
			}
			catch (Exception e)
			{
				CancelDelayTask();
				onFailure?.Invoke(e.HResult, e.Message);
				unityWebRequest.Dispose();

				return;
			}

			CancelDelayTask();
			Log.Print($"URI: {unityWebRequest.uri.AbsoluteUri}, Response Code: {unityWebRequest.responseCode}", LogPriority.Verbose);
			onSuccess?.Invoke(unityWebRequest.downloadHandler.data);
			
			unityWebRequest.Dispose();
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