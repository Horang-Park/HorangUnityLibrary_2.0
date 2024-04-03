using System;
using System.Net.Http;
using System.Threading;
using Cysharp.Threading.Tasks;
using Horang.HorangUnityLibrary.Managers.Static.AsyncNetworking.Interfaces;
using Horang.HorangUnityLibrary.Utilities;
using UniRx;

namespace Horang.HorangUnityLibrary.Managers.Static.AsyncNetworking
{
	public static class HttpRequestManager
	{
		private static CancellationTokenSource delayWaiterCancellationTokenSource = new();
		
		public static async UniTask Get(string uri, HttpClient httpClient, ICallbackHandlerText callbackHandler)
		{
			MainThreadDispatchLog($"HTTP requested by GET to URI: {uri} destination.");
			
			Delay(httpClient.Timeout.Milliseconds * 0.5, callbackHandler.OnDelay).Forget();
			
			using var message = await httpClient.GetAsync(uri);

			if (message.IsSuccessStatusCode)
			{
				CancellationDelayTask();
				
				var result = await message.Content.ReadAsStringAsync();
				callbackHandler.OnSuccess(result);
				
				httpClient.Dispose();

				return;
			}

			CancellationDelayTask();
			
			MainThreadDispatchLog($"HTTP request failed by [{message.StatusCode}] code with \"{message.ReasonPhrase}\" reason.");
			callbackHandler.OnFailure(message.StatusCode, message.ReasonPhrase);
			
			httpClient.Dispose();
		}
		
		public static async UniTask Get(string uri, HttpClient httpClient, ICallbackHandlerBytes callbackHandler)
		{
			MainThreadDispatchLog($"HTTP requested by GET to URI: {uri} destination.");
			
			Delay(httpClient.Timeout.Milliseconds * 0.5, callbackHandler.OnDelay).Forget();
			
			using var message = await httpClient.GetAsync(uri);

			if (message.IsSuccessStatusCode)
			{
				CancellationDelayTask();
				
				var result = await message.Content.ReadAsByteArrayAsync();
				callbackHandler.OnSuccess(result);
				
				httpClient.Dispose();

				return;
			}

			CancellationDelayTask();
			
			MainThreadDispatchLog($"HTTP request failed by [{message.StatusCode}] code with \"{message.ReasonPhrase}\" reason.");
			callbackHandler.OnFailure(message.StatusCode, message.ReasonPhrase);
			
			httpClient.Dispose();
		}
		
		public static async UniTask Get(string uri, HttpClient httpClient, ICallbackHandlerStream callbackHandler)
		{
			MainThreadDispatchLog($"HTTP requested by GET to URI: {uri} destination.");
			
			Delay(httpClient.Timeout.Milliseconds * 0.5, callbackHandler.OnDelay).Forget();
			
			using var message = await httpClient.GetAsync(uri);

			if (message.IsSuccessStatusCode)
			{
				CancellationDelayTask();
				
				var result = await message.Content.ReadAsStreamAsync();
				callbackHandler.OnSuccess(result);
				
				httpClient.Dispose();

				return;
			}

			CancellationDelayTask();
			
			MainThreadDispatchLog($"HTTP request failed by [{message.StatusCode}] code with \"{message.ReasonPhrase}\" reason.");
			callbackHandler.OnFailure(message.StatusCode, message.ReasonPhrase);
			
			httpClient.Dispose();
		}

		private static async UniTaskVoid Delay(double delayTime, Action onDelayAction)
		{
			await UniTask.Delay(
				TimeSpan.FromMilliseconds(delayTime),
				DelayType.Realtime,
				PlayerLoopTiming.FixedUpdate,
				delayWaiterCancellationTokenSource.Token);
			
			onDelayAction?.Invoke();
		}
		
		private static void CancellationDelayTask()
		{
			delayWaiterCancellationTokenSource.Cancel();
			delayWaiterCancellationTokenSource.Dispose();
			delayWaiterCancellationTokenSource = new CancellationTokenSource();
		}

		private static void MainThreadDispatchLog(string message, LogPriority priority = LogPriority.Verbose)
		{
			MainThreadDispatcher.Send(_ =>
			{
				Log.Print(message, priority);
			}, null);
		}
	}
}