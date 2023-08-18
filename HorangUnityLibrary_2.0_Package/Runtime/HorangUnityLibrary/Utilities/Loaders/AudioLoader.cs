using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Horang.HorangUnityLibrary.Utilities.Loaders
{
	public struct AudioLoader
	{
		public enum SupportAudioType
		{
			Mpeg = 13,
			Ogg = 14,
			Wav = 20
		}
		
		private static readonly string[] supportAudioFileExtension = { @".mp3", @".ogg", @".wav" };

		private delegate UniTask<AudioClip> TaskDelegate(string path, CancellationToken cancellationToken);
		private static TaskDelegate loadManyDelegate;

		/// <summary>
		/// Load sprite from remote server or local path.
		/// </summary>
		/// <param name="uri">To load path from remote server</param>
		/// <param name="supportAudioType">To load audio type</param>
		/// <param name="cancellationToken">To cancellation</param>
		/// <returns>Async load sprite with UniTask</returns>
		public static async UniTask<AudioClip> LoadFromRemote(string uri, SupportAudioType supportAudioType, CancellationToken cancellationToken = default)
		{
			UnityWebRequest sizeRequester = null;
			var audioRequester = UnityWebRequestMultimedia.GetAudioClip(uri, (AudioType)(int)supportAudioType);

			try
			{
				sizeRequester = await UnityWebRequest.Head(uri).SendWebRequest().WithCancellation(cancellationToken);
			}
			catch (UnityWebRequestException e)
			{
				Log.Print($"Audio size get failed. Not allowed method. Response Code: {e.ResponseCode} / Error: {e.Error} / Message: {e.Message}", LogPriority.Warning);
			}

			if (sizeRequester is not null)
			{
				Log.Print($"Load start. URI: {audioRequester.uri.AbsoluteUri}, Size: {(float.Parse(sizeRequester.GetResponseHeader("Content-Length")) / 1024):0,0} KB", LogPriority.Verbose);
			
				sizeRequester.Dispose();
			}
			else
			{
				Log.Print($"Load start. URI: {audioRequester.uri.AbsoluteUri}", LogPriority.Verbose);
			}

			try
			{
				audioRequester = await audioRequester.SendWebRequest().ToUniTask(cancellationToken: cancellationToken).Timeout(TimeSpan.MaxValue);
			}
			catch (UnityWebRequestException e)
			{
				Log.Print($"Load failed. Response Code: {e.ResponseCode}, Message: {e.Message}, URI: {e.UnityWebRequest.uri}\nDownload handler error: {e.UnityWebRequest.downloadHandler.error}", LogPriority.Error);

				audioRequester.Dispose();

				throw;
			}
			catch (Exception e)
			{
				Log.Print($"Load failed. HR: {e.HResult}, Message: {e.Message}", LogPriority.Exception);
				
				audioRequester.Dispose();

				throw;
			}

			var handler = audioRequester.downloadHandler;
			var audioClip = ((DownloadHandlerAudioClip)handler).audioClip;
			
			Log.Print($"Load complete. URI: {audioRequester.uri.AbsoluteUri}, Response Code: {audioRequester.responseCode}", LogPriority.Verbose);
			
			audioRequester.Dispose();

			return audioClip;
		}

		/// <summary>
		/// Load many sprites from remote server or local path.
		/// </summary>
		/// <param name="uris">To load paths enumerable</param>
		/// <param name="supportAudioType">To load audio type</param>
		/// <param name="cancellationToken">To cancellation</param>
		/// <returns>Async load sprites array with UniTask</returns>
		public static async UniTask<AudioClip[]> LoadManyFromRemote(IEnumerable<string> uris, SupportAudioType supportAudioType, CancellationToken cancellationToken = default)
		{
			loadManyDelegate = (path, token) => LoadFromRemote(path, supportAudioType, token);

			try
			{
				return await UniTask.WhenAll(CreateAudioLoadTasks(uris, loadManyDelegate, cancellationToken)).AttachExternalCancellation(cancellationToken);
			}
			catch (Exception)
			{
				throw new OperationCanceledException();
			}
		}

		/// <summary>
		/// Load sprite from Unity internal resources folder.
		/// </summary>
		/// <param name="path">To load path from resources folder. beware not to add extension</param>
		/// <param name="cancellationToken">To cancellation</param>
		/// <returns>Async load sprite with UniTask</returns>
		public static async UniTask<AudioClip> LoadFromResourcesFolder(string path, CancellationToken cancellationToken = default)
		{
			if (await Resources.LoadAsync<AudioClip>(path).WithCancellation(cancellationToken) is AudioClip resourcesAudioClips)
			{
				Log.Print($"Load complete: {path}");
				
				return resourcesAudioClips;
			}
			
			Log.Print($"The audio in path({path}) is not exist.", LogPriority.Error);

			return null;
		}

		/// <summary>
		/// Load sprites from Unity internal resources folder.
		/// </summary>
		/// <param name="paths">To load paths enumerable</param>
		/// <param name="cancellationToken">To cancellation</param>
		/// <returns>Async load sprites array with UniTask</returns>
		public static async UniTask<AudioClip[]> LoadManyFromResourcesFolder(IEnumerable<string> paths, CancellationToken cancellationToken = default)
		{
			loadManyDelegate = LoadFromResourcesFolder;
			
			return await UniTask.WhenAll(CreateAudioLoadTasks(paths, loadManyDelegate, cancellationToken));
		}

		private static IEnumerable<UniTask<AudioClip>> CreateAudioLoadTasks(IEnumerable<string> p, TaskDelegate td, CancellationToken ct)
		{
			return Enumerable.Select(p, u => td.Invoke(u, ct));
		}
	}
}