using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Horang.HorangUnityLibrary.Utilities;
using UnityEngine.Networking;

namespace Horang.HorangUnityLibrary.Managers.Static.Networking
{
	public static class RequestManager
	{
		private static CancellationTokenSource delayWaiterCancellationTokenSource = new();
		private static CancellationTokenSource webRequestCancellationTokenSource = new();

		/// <summary>
		/// Send web request to remote.
		/// </summary>
		/// <param name="unityWebRequest">To send web request</param>
		/// <param name="callback">callbacks</param>
		/// <param name="onProgress">To get percentage of web request's progress (0~1)</param>
		/// <param name="timeout">To setting request's timeout</param>
		/// <param name="delayTimeout">To setting request's delay timeout</param>
		public static async UniTaskVoid Send(
			UnityWebRequest unityWebRequest,
			IRequestCallbackString callback,
			Action<float> onProgress = null,
			double timeout = 30000D,
			double delayTimeout = 3000D)
		{
			Log.Print($"URI: {unityWebRequest.uri.AbsoluteUri}, API method: {unityWebRequest.method}", LogPriority.Verbose);
			
			Delay(callback.OnDelay, delayTimeout).Forget();

			try
			{
				unityWebRequest = await unityWebRequest.SendWebRequest()
					.ToUniTask(progress: Progress.CreateOnlyValueChanged(onProgress), cancellationToken: webRequestCancellationTokenSource.Token)
					.Timeout(TimeSpan.FromMilliseconds(timeout));
			}
			catch (UnityWebRequestException e)
			{
				CancelDelayTask();
				callback.OnFailure(e.UnityWebRequest.responseCode, e.UnityWebRequest.error);
				unityWebRequest.Dispose();

				return;
			}
			catch (Exception e)
			{
				CancelDelayTask();
				callback.OnFailure(e.HResult, e.Message);
				unityWebRequest.Dispose();

				return;
			}

			CancelDelayTask();
			Log.Print($"URI: {unityWebRequest.uri.AbsoluteUri}, Response Code: {unityWebRequest.responseCode}", LogPriority.Verbose);
			callback.OnSuccess(unityWebRequest.downloadHandler.text);
			
			unityWebRequest.Dispose();
		}

		/// <summary>
		/// Send web request to remote.
		/// </summary>
		/// <param name="unityWebRequest">To send web request</param>
		/// <param name="callback">callbacks</param>
		/// <param name="onProgress">To get percentage of web request's progress (0~1)</param>
		/// <param name="timeout">To setting request's timeout</param>
		/// <param name="delayTimeout">To setting request's delay timeout</param>
		public static async UniTaskVoid Send(
			UnityWebRequest unityWebRequest,
			IRequestCallbackByteArray callback,
			Action<float> onProgress = null,
			double timeout = 30000D,
			double delayTimeout = 3000D)
		{
			Log.Print($"URI: {unityWebRequest.uri.AbsoluteUri}, API method: {unityWebRequest.method}", LogPriority.Verbose);
			
			Delay(callback.OnDelay, delayTimeout).Forget();

			try
			{
				unityWebRequest = await unityWebRequest.SendWebRequest()
					.ToUniTask(progress: Progress.CreateOnlyValueChanged(onProgress), cancellationToken: webRequestCancellationTokenSource.Token)
					.Timeout(TimeSpan.FromMilliseconds(timeout));
			}
			catch (UnityWebRequestException e)
			{
				CancelDelayTask();
				callback.OnFailure(e.UnityWebRequest.responseCode, e.UnityWebRequest.error);
				unityWebRequest.Dispose();

				return;
			}
			catch (Exception e)
			{
				CancelDelayTask();
				callback.OnFailure(e.HResult, e.Message);
				unityWebRequest.Dispose();

				return;
			}

			CancelDelayTask();
			Log.Print($"URI: {unityWebRequest.uri.AbsoluteUri}, Response Code: {unityWebRequest.responseCode}", LogPriority.Verbose);
			callback.OnSuccess(unityWebRequest.downloadHandler.data);
			
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

			CancelWebRequest();
		}

		private static void CancelDelayTask()
		{
			delayWaiterCancellationTokenSource.Cancel();
			delayWaiterCancellationTokenSource.Dispose();
			delayWaiterCancellationTokenSource = new CancellationTokenSource();
		}

		private static void CancelWebRequest()
		{
			webRequestCancellationTokenSource.Cancel();
			webRequestCancellationTokenSource.Dispose();
			webRequestCancellationTokenSource = new CancellationTokenSource();
		}
	}
}