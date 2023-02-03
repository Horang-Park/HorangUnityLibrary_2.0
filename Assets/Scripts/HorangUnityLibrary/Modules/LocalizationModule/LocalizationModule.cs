using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using HorangUnityLibrary.Foundation.Module;
using HorangUnityLibrary.Managers.Module;
using HorangUnityLibrary.Utilities;
using UnityEngine;
using UnityEngine.Networking;

namespace HorangUnityLibrary.Modules.LocalizationModule
{
	public class LocalizationModule : BaseModule
	{
		private readonly Dictionary<SystemLanguage, Dictionary<int, string>> textTables = new();
		private CancellationTokenSource delayWaiterCancellationTokenSource = new();
		
		public LocalizationModule(ModuleManager moduleManager) : base(moduleManager)
		{
		}

		public override bool ActiveModule()
		{
			if (base.ActiveModule() is false)
			{
				return false;
			}
			
			Log.Print("Module are activated.", LogPriority.Verbose);

			return true;
		}

		public override bool InactiveModule()
		{
			if (base.InactiveModule() is false)
			{
				return false;
			}
			
			Log.Print("Module are inactivated.", LogPriority.Verbose);

			return true;
		}

		/// <summary>
		/// Load localization file from local storage.
		/// </summary>
		/// <param name="language">To load language</param>
		/// <param name="path">To load file path</param>
		/// <exception cref="FileNotFoundException">Throw if file is not exist</exception>
		public async UniTask LoadLocalizationFromLocal(SystemLanguage language, string path)
		{
			var fileInfo = new FileInfo(path);

			if (fileInfo.Exists is false)
			{
				Log.Print($"The file in path({path}) is not exist.", LogPriority.Exception);

				throw new FileNotFoundException();
			}
			
			var reader = new StreamReader(path);
			var value = await reader.ReadToEndAsync();
			
			reader.Close();
			reader.Dispose();
			
			ParsingDataAndSave(language, value);
		}

		/// <summary>
		/// Load localization file from Unity resources folder.
		/// </summary>
		/// <param name="language">To load language</param>
		/// <param name="path">To load file path</param>
		/// <exception cref="FileNotFoundException">Throw if file is not exist</exception>
		/// <exception cref="InvalidCastException">Throw if file cannot convert into TextAsset</exception>
		public async UniTask LoadLocalizationFromResources(SystemLanguage language, string path)
		{
			var textAssetObject = await Resources.LoadAsync<TextAsset>(path);

			switch (textAssetObject)
			{
				case TextAsset textAsset:
					ParsingDataAndSave(language, textAsset.text);
					break;
				case null:
					throw new FileNotFoundException();
				default:
					throw new InvalidCastException();
			}
		}

		/// <summary>
		/// Download localization file from remote and save automatically in local storage. (persistentDataPath)
		/// </summary>
		/// <param name="language">Localization file's language type</param>
		/// <param name="uri">Download path from remote</param>
		/// <param name="onSuccess">Calling when request is successfully worked, save asset to local. callback downloaded asset bundle</param>
		/// <param name="onDelay">Calling when request is going delay over delayTimeout parameter</param>
		/// <param name="onSizeCheck">Calling before download asset bundle in byte unit</param>
		/// <param name="onFailure">Calling when occurred error on requesting</param>
		/// <param name="onProgress">To get percentage of web request's progress (0~1)</param>
		/// <param name="timeout">To setting request's timeout</param>
		/// <param name="delayTimeout">To setting request's delay timeout</param>
		/// <param name="headerParameters">Header parameters</param>
		public async UniTask LoadLocalizationFromRemote(SystemLanguage language,
			string uri,
			Action onSuccess,
			Action onDelay = null,
			Action<long> onSizeCheck = null,
			Action<long, string> onFailure = null,
			Action<float> onProgress = null,
			double timeout = 30000D,
			double delayTimeout = 3000D,
			params (string, string)[] headerParameters)
		{
			Delay(onDelay, delayTimeout).Forget();
			
			var requester = UnityWebRequest.Get(uri);
			var headerRequester = await UnityWebRequest.Head(uri).SendWebRequest();
			
			foreach (var parameter in headerParameters)
			{
				requester.SetRequestHeader(parameter.Item1, parameter.Item2);
			}
			
			onSizeCheck?.Invoke(long.Parse(headerRequester.GetRequestHeader("Content-Length")));
			
			requester.downloadHandler = new DownloadHandlerFile(Application.persistentDataPath);
			
			Log.Print($"Load start. URI: {requester.uri}, Size: {headerRequester.GetRequestHeader("Content-Length")}");
			
			headerRequester.Dispose();
			
			try
			{
				requester = await requester.SendWebRequest()
					.ToUniTask(Progress.Create(onProgress))
					.Timeout(TimeSpan.FromMilliseconds(timeout));
			}
			catch (UnityWebRequestException e)
			{
				CancelDelayTask();
				onFailure?.Invoke(e.UnityWebRequest.responseCode, e.UnityWebRequest.error);
				requester.Dispose();

				return;
			}
			catch (Exception e)
			{
				CancelDelayTask();
				onFailure?.Invoke(e.HResult, e.Message);
				requester.Dispose();

				return;
			}

			CancelDelayTask();
			Log.Print($"Load complete. URI: {requester.uri.AbsoluteUri}, Response Code: {requester.responseCode}", LogPriority.Verbose);

			onSuccess?.Invoke();
			
			ParsingDataAndSave(language, requester.downloadHandler.text);
			
			requester.Dispose();
		}

		/// <summary>
		/// Getting localization data which matching key
		/// </summary>
		/// <param name="key">To get localization target name</param>
		/// <param name="language">To get language</param>
		/// <returns>If can find key in localization table, It return its value. otherwise, string.Empty</returns>
		public string Get(string key, SystemLanguage language)
		{
			var hashKey = key.GetHashCode();

			if (textTables.ContainsKey(language))
			{
				var table = textTables[language];

				if (table.ContainsKey(hashKey))
				{
					return table[hashKey];
				}
				
				Log.Print($"There is no key [{key}] in [{language}] localization table.", LogPriority.Error);

				return string.Empty;
			}
			
			Log.Print($"There is no [{language}] language's localization table.", LogPriority.Error);

			return string.Empty;
		}

		/// <summary>
		/// Getting current loaded localization language
		/// </summary>
		/// <returns>Enumerable of loaded localization languages</returns>
		public IEnumerable<SystemLanguage> CurrentLoadedLanguages() => textTables.Select(item => item.Key);
		
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

		private void ParsingDataAndSave(SystemLanguage sL, string d)
		{
			var table = new Dictionary<int, string>();
			var separateLines = d.Split('\n');

			var debugIndex = 1;

			foreach (var line in separateLines)
			{
				if (line.Contains('#') || string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
				{
					debugIndex++;
					
					continue;
				}
				
				var processLine = line.Trim();
				var keyValue = processLine.Split("=");

				if (keyValue.Length != 2)
				{
					Log.Print($"Meet invalid line while reading. Edit the file and re-run application. | Line index: {debugIndex}", LogPriority.Error);

					return;
				}

				keyValue[0] = keyValue[0].Trim();
				keyValue[1] = keyValue[1].Trim();

				if (string.IsNullOrEmpty(keyValue[0]))
				{
					Log.Print($"Meet empty key while reading. Edit the file and re-run application. | Line index: {debugIndex}", LogPriority.Error);

					throw new FormatException();
				}
				
				if (string.IsNullOrEmpty(keyValue[1]))
				{
					Log.Print($"Meet empty value while reading. Edit the file and re-run application. | Line index: {debugIndex}", LogPriority.Error);
					
					throw new FormatException();
				}

				var key = keyValue[0].GetHashCode();
				var value = keyValue[1];

				if (table.ContainsKey(key))
				{
					Log.Print($"[{keyValue[0]}] key is already exist. / Line index: {debugIndex}", LogPriority.Error);
					
					debugIndex++;

					continue;
				}

				value = value.Replace("\"", string.Empty);
				
				table.Add(key, value);
				
				debugIndex++;
			}

			if (textTables.ContainsKey(sL))
			{
				foreach (var item in table.Where(item => !textTables[sL].ContainsKey(item.Key)))
				{
					textTables[sL].Add(item.Key, item.Value);
				}

				return;
			}
			
			textTables.Add(sL, table);
		}
	}
}