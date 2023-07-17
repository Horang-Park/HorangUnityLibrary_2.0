using System;
using System.Collections.Generic;
using System.IO;
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
			Wav = 20,
		}
		
		private static readonly string[] supportImageFileExtension = { @".mp3", @".ogg", @".wav" };

		private delegate UniTask<AudioClip> TaskDelegate(string path, CancellationToken cancellationToken);
		private static TaskDelegate loadManyDelegate;

		/// <summary>
		/// Load sprite from local storage.
		/// </summary>
		/// <param name="path">To load path</param>
		/// <param name="cancellationToken">To cancellation</param>
		/// <returns>Async load sprite with UniTask</returns>
		public static async UniTask<AudioClip> LoadFromLocal(string path, CancellationToken cancellationToken = default)
		{
			var extension = Path.GetExtension(path);

			if (ValidationAudioFileExtension(extension) is false)
			{
				Log.Print($"To load audio path({path}) is missing file extension or not image file extension.\nSupport file extension is [mp3, ogg, wav].", LogPriority.Error);

				return null;
			}

			var fileInfo = ValidationFileExist(path);

			if (fileInfo is null)
			{
				return null;
			}

			var audioFileStream = new FileStream(path, FileMode.Open);
			var audioByteBuffer = new byte[audioFileStream.Length];
			var loadBytes = await audioFileStream.ReadAsync(audioByteBuffer, 0, (int)audioFileStream.Length, cancellationToken);
			
			Log.Print($"Load complete: {path}, Load bytes: {loadBytes}", LogPriority.Verbose);
			
			audioFileStream.Close();
			await audioFileStream.DisposeAsync();

			return ConvertByteAudioToAudioClip(audioByteBuffer);
		}

		/// <summary>
		/// Load many sprites from local storage.
		/// </summary>
		/// <param name="paths">To load paths enumerable</param>
		/// <param name="cancellationToken">To cancellation</param>
		/// <returns>Async load sprites array with UniTask</returns>
		public static async UniTask<AudioClip[]> LoadManyFromLocal(IEnumerable<string> paths, CancellationToken cancellationToken = default)
		{
			loadManyDelegate = LoadFromLocal;

			return await UniTask.WhenAll(CreateAudioLoadTasks(paths, loadManyDelegate, cancellationToken)).AttachExternalCancellation(cancellationToken);
		}

		/// <summary>
		/// Load sprite from remote server.
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
				Log.Print($"Image size get failed. Not allowed method. Response Code: {e.ResponseCode} / Error: {e.Error} / Message: {e.Message}", LogPriority.Warning);
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
		/// Load many sprites from remote server.
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
			
			Log.Print($"The image in path({path}) is not exist.", LogPriority.Error);

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

		private static bool ValidationAudioFileExtension(string e)
		{
			if (string.IsNullOrEmpty(e) || string.IsNullOrWhiteSpace(e))
			{
				return false;
			}

			return supportImageFileExtension.Contains(e);
		}

		private static FileInfo ValidationFileExist(string p)
		{
			var fileInfo = new FileInfo(p);

			if (fileInfo.Exists)
			{
				return fileInfo;
			}
			
			Log.Print($"To load file does not exist in path({p}).", LogPriority.Error);

			return null;
		}

		private static IEnumerable<UniTask<AudioClip>> CreateAudioLoadTasks(IEnumerable<string> p, TaskDelegate td, CancellationToken ct)
		{
			return Enumerable.Select(p, u => td.Invoke(u, ct));
		}
		
		private static AudioClip ConvertByteAudioToAudioClip(byte[] array) 
		{
			var floatArr = new float[array.Length / 4];
			
			for (var i = 0; i < floatArr.Length; i++) 
			{
				if (BitConverter.IsLittleEndian)
				{
					Array.Reverse(array, i * 4, 4);
				}
				
				floatArr[i] = BitConverter.ToSingle(array, i * 4) / 0x80000000;
			}


			var audioClip = AudioClip.Create(string.Empty, floatArr.Length, 1, 44100, false);
			audioClip.SetData(floatArr, 0);

			return audioClip;
		} 
		
	}
}