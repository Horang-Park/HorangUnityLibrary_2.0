using System;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using HorangUnityLibrary.Foundation.Module;
using HorangUnityLibrary.Managers.Module;
using HorangUnityLibrary.Utilities;
using UnityEngine;
using UnityEngine.Networking;

namespace HorangUnityLibrary.Modules.AssetBundlePatchModule
{
	public class AssetBundlePatchModule : BaseModule
	{
		private CancellationTokenSource delayWaiterCancellationTokenSource = new();

		private const string VersionPlayerPrefsKey = "Version";

		public AssetBundlePatchModule(ModuleManager moduleManager) : base(moduleManager)
		{
		}

		public override bool ActiveModule()
		{
			if (base.ActiveModule() is false)
			{
				return false;
			}

			Log.Print("Module are activated", LogPriority.Verbose);

			return true;
		}

		public override bool InactiveModule()
		{
			if (base.InactiveModule() is false)
			{
				return false;
			}

			Log.Print("Module are inactivated", LogPriority.Verbose);

			return true;
		}

		/// <summary>
		/// Local asset bundle and remote asset bundle version check
		/// </summary>
		/// <param name="remoteVersion">To compare remote asset bundle version</param>
		/// <returns>If local version is difference with remote version, will return false. otherwise true</returns>
		public bool VersionCheck(string remoteVersion)
		{
			if (isThisModuleActivated is false)
			{
				return true;
			}

			if (PlayerPrefs.HasKey(VersionPlayerPrefsKey) is false)
			{
				return false;
			}

			var localVersion = PlayerPrefs.GetString(VersionPlayerPrefsKey);

			if (remoteVersion.Equals(localVersion))
			{
				return true;
			}

			PlayerPrefs.SetString(VersionPlayerPrefsKey, remoteVersion);

			return false;
		}

		/// <summary>
		/// Download latest asset bundle from remote server and save to local. (in persistentDataPath)
		/// </summary>
		/// <param name="remoteAssetBundleUri">To send web request</param>
		/// <param name="onSuccess">Calling when request is successfully worked, save asset to local. callback downloaded asset bundle</param>
		/// <param name="onDelay">Calling when request is going delay over delayTimeout parameter</param>
		/// <param name="onSizeCheck">Calling before download asset bundle in byte unit</param>
		/// <param name="onFailure">Calling when occurred error on requesting</param>
		/// <param name="onProgress">To get percentage of web request's progress (0~1)</param>
		/// <param name="timeout">To setting request's timeout</param>
		/// <param name="delayTimeout">To setting request's delay timeout</param>
		/// <param name="headerParameters">Header parameter</param>
		public async UniTask DownloadLatestVersionFromRemote(string remoteAssetBundleUri,
			Action<UnityEngine.AssetBundle> onSuccess,
			Action onDelay = null,
			Action<long> onSizeCheck = null,
			Action<long, string> onFailure = null,
			Action<float> onProgress = null,
			double timeout = 30000D,
			double delayTimeout = 3000D,
			params (string, string)[] headerParameters)
		{
			if (isThisModuleActivated is false)
			{
				return;
			}
			
			Delay(onDelay, delayTimeout).Forget();

			var assetBundleRequester = UnityWebRequestAssetBundle.GetAssetBundle(remoteAssetBundleUri);
			var sizeRequester = await UnityWebRequest.Head(remoteAssetBundleUri).SendWebRequest();
			
			foreach (var parameter in headerParameters)
			{
				assetBundleRequester.SetRequestHeader(parameter.Item1, parameter.Item2);
			}

			onSizeCheck?.Invoke(long.Parse(sizeRequester.GetResponseHeader("Content-Length")));
			
			Log.Print($"Start download asset bundle - URI: {remoteAssetBundleUri}, Size: {sizeRequester.GetResponseHeader("Content-Length")}", LogPriority.Verbose);
			
			sizeRequester.Dispose();

			try
			{
				assetBundleRequester = await assetBundleRequester.SendWebRequest()
					.ToUniTask(Progress.Create(onProgress))
					.Timeout(TimeSpan.FromMilliseconds(timeout));
			}
			catch (UnityWebRequestException e)
			{
				CancelDelayTask();
				onFailure?.Invoke(e.UnityWebRequest.responseCode, e.UnityWebRequest.error);
				assetBundleRequester.Dispose();

				return;
			}
			catch (Exception e)
			{
				CancelDelayTask();
				onFailure?.Invoke(e.HResult, e.Message);
				assetBundleRequester.Dispose();

				return;
			}

			CancelDelayTask();
			Log.Print($"URI: {assetBundleRequester.uri.AbsoluteUri}, Response Code: {assetBundleRequester.responseCode}", LogPriority.Verbose);

			var downloadedAssetBundle = await UnityEngine.AssetBundle.LoadFromMemoryAsync(assetBundleRequester.downloadHandler.data);
			
			await SaveAssetBundleToLocal(assetBundleRequester.downloadHandler.data, downloadedAssetBundle.name);
			
			onSuccess?.Invoke(downloadedAssetBundle);
			
			assetBundleRequester.Dispose();
		}

		private async UniTaskVoid Delay(Action oD, double tO)
		{
			await UniTask.Delay(
				TimeSpan.FromMilliseconds(tO),
				DelayType.DeltaTime,
				PlayerLoopTiming.Update,
				delayWaiterCancellationTokenSource.Token);

			oD?.Invoke();
		}

		private void CancelDelayTask()
		{
			delayWaiterCancellationTokenSource.Cancel();
			delayWaiterCancellationTokenSource.Dispose();
			delayWaiterCancellationTokenSource = new CancellationTokenSource();
		}

		private static async UniTask SaveAssetBundleToLocal(byte[] rawAssetBundleData, string fn)
		{
			var fullPath = Path.Combine(Application.persistentDataPath, fn);
			var fileSystem = File.Create(fullPath);
			
			await fileSystem.WriteAsync(rawAssetBundleData);
			
			fileSystem.Close();
			await fileSystem.DisposeAsync();
		}
	}
}