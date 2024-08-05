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
		
		public static async UniTask Get(string uri, HttpClient client, ICallbackHandlerText callback)
		{
			MainThreadDispatchLog($"HTTP requested by [GET] to \"{uri}\" destination.");
			
			Delay(client.Timeout.Milliseconds * 0.5, callback.OnDelay).Forget();
			
			using var message = await client.GetAsync(uri);

			if (message.IsSuccessStatusCode)
			{
				CancellationDelayTask();
				
				var result = await message.Content.ReadAsStringAsync();
				callback.OnSuccess(result);
				
				client.Dispose();

				return;
			}

			CancellationDelayTask();
			
			MainThreadDispatchLog($"HTTP request failed by [{(int)message.StatusCode}] code with \"{message.ReasonPhrase}\" reason.", LogPriority.Error);
			callback.OnFailure(message.StatusCode, message.ReasonPhrase);
			
			client.Dispose();
		}
		
		public static async UniTask Get(string uri, HttpClient client, ICallbackHandlerBytes callback)
		{
			MainThreadDispatchLog($"HTTP requested by [GET] to \"{uri}\" destination.");
			
			Delay(client.Timeout.Milliseconds * 0.5, callback.OnDelay).Forget();
			
			using var message = await client.GetAsync(uri);

			if (message.IsSuccessStatusCode)
			{
				CancellationDelayTask();
				
				var result = await message.Content.ReadAsByteArrayAsync();
				callback.OnSuccess(result);
				
				client.Dispose();

				return;
			}

			CancellationDelayTask();
			
			MainThreadDispatchLog($"HTTP request failed by [{(int)message.StatusCode}] code with \"{message.ReasonPhrase}\" reason.", LogPriority.Error);
			callback.OnFailure(message.StatusCode, message.ReasonPhrase);
			
			client.Dispose();
		}
		
		public static async UniTask Get(string uri, HttpClient client, ICallbackHandlerStream callback)
		{
			MainThreadDispatchLog($"HTTP requested by [GET] to \"{uri}\" destination.");
			
			Delay(client.Timeout.Milliseconds * 0.5, callback.OnDelay).Forget();
			
			using var message = await client.GetAsync(uri);

			if (message.IsSuccessStatusCode)
			{
				CancellationDelayTask();
				
				var result = await message.Content.ReadAsStreamAsync();
				callback.OnSuccess(result);
				
				client.Dispose();

				return;
			}

			CancellationDelayTask();
			
			MainThreadDispatchLog($"HTTP request failed by [{(int)message.StatusCode}] code with \"{message.ReasonPhrase}\" reason.", LogPriority.Error);
			callback.OnFailure(message.StatusCode, message.ReasonPhrase);
			
			client.Dispose();
		}

		public static async UniTask Post(string uri, HttpContent content, HttpClient client, ICallbackHandlerText callback)
		{
			MainThreadDispatchLog($"HTTP requested by [POST] to \"{uri}\" destination.");
			
			Delay(client.Timeout.Milliseconds * 0.5, callback.OnDelay).Forget();
			
			using var message = await client.PostAsync(uri, content);

			if (message.IsSuccessStatusCode)
			{
				CancellationDelayTask();
				
				var result = await message.Content.ReadAsStringAsync();
				callback.OnSuccess(result);
				
				content.Dispose();
				client.Dispose();

				return;
			}

			CancellationDelayTask();
			
			MainThreadDispatchLog($"HTTP request failed by [{(int)message.StatusCode}] code with \"{message.ReasonPhrase}\" reason.", LogPriority.Error);
			callback.OnFailure(message.StatusCode, message.ReasonPhrase);

			message.Dispose();
			content.Dispose();
			client.Dispose();
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