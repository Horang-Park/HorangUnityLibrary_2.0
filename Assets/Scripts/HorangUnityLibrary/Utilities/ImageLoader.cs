using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace HorangUnityLibrary.Utilities
{
	public static class ImageLoader
	{
		private static readonly string[] SupportImageFileExtension = { @".png", @".jpg", @".jpeg", @".tga" };

		private delegate UniTask<Sprite> TaskDelegate(string path);
		private static TaskDelegate loadManyDelegate;
		private static readonly CancellationTokenSource Cts = new();

		/// <summary>
		/// Load sprite from local storage.
		/// </summary>
		/// <param name="path">To load path</param>
		/// <returns>Async load sprite with UniTask</returns>
		public static async UniTask<Sprite> LoadFromLocal(string path)
		{
			var extension = Path.GetExtension(path);

			if (ValidationImageFileExtension(extension) is false)
			{
				Log.Print($"To load image path({path}) is missing file extension or not image file extension.\nSupport file extension is [png, jpg, jpeg, tga].", LogPriority.Error);

				return null;
			}

			var fileInfo = ValidationFileExist(path);

			if (fileInfo is null)
			{
				return null;
			}

			var textureFileStream = new FileStream(path, FileMode.Open);
			var textureByteBuffer = new byte[textureFileStream.Length];
			var loadBytes = await textureFileStream.ReadAsync(textureByteBuffer, 0, (int)textureFileStream.Length);
			
			Log.Print($"Load complete: {path}, Load bytes: {loadBytes}", LogPriority.Verbose);
			
			textureFileStream.Close();
			await textureFileStream.DisposeAsync();

			return ConvertByteTextureToSprite(textureByteBuffer);
		}

		/// <summary>
		/// Load many sprites from local storage.
		/// </summary>
		/// <param name="paths">To load paths enumerable</param>
		/// <returns>Async load sprites array with UniTask</returns>
		public static async UniTask<Sprite[]> LoadManyFromLocal(IEnumerable<string> paths)
		{
			loadManyDelegate = LoadFromLocal;

			return await UniTask.WhenAll(CreateImageLoadTasks(paths, loadManyDelegate));
		}

		/// <summary>
		/// Load sprite from remote server.
		/// </summary>
		/// <param name="uri">To load path from remote server</param>
		/// <returns>Async load sprite with UniTask</returns>
		public static async UniTask<Sprite> LoadFromRemote(string uri)
		{
			var imageRequester = UnityWebRequestTexture.GetTexture(uri);
			var sizeRequester = await UnityWebRequest.Head(uri).SendWebRequest();

			Log.Print($"Load start. URI: {imageRequester.uri.AbsoluteUri}, Size: {(float.Parse(sizeRequester.GetResponseHeader("Content-Length")) / 1024):0,0} KB", LogPriority.Verbose);
			
			sizeRequester.Dispose();
			
			try
			{
				imageRequester = await imageRequester.SendWebRequest().ToUniTask(cancellationToken: Cts.Token).Timeout(TimeSpan.MaxValue);
			}
			catch (UnityWebRequestException e)
			{
				Log.Print($"Load failed. Response Code: {e.ResponseCode}, Message: {e.Message}, URI: {e.UnityWebRequest.uri}\nDownload handler error: {e.UnityWebRequest.downloadHandler.error}", LogPriority.Error);

				imageRequester.Dispose();
				Cts.Cancel(true);

				throw;
			}
			catch (Exception e)
			{
				Log.Print($"Load failed. HR: {e.HResult}, Message: {e.Message}", LogPriority.Exception);
				
				imageRequester.Dispose();
				Cts.Cancel(true);

				throw;
			}

			var loadedSprite = ConvertByteTextureToSprite(imageRequester.downloadHandler.data);
			
			Log.Print($"Load complete. URI: {imageRequester.uri.AbsoluteUri}, Response Code: {imageRequester.responseCode}", LogPriority.Verbose);
			
			imageRequester.Dispose();

			return loadedSprite;
		}

		/// <summary>
		/// Load many sprites from remote server.
		/// </summary>
		/// <param name="uris">To load paths enumerable</param>
		/// <returns>Async load sprites array with UniTask</returns>
		public static async UniTask<Sprite[]> LoadManyFromRemote(IEnumerable<string> uris)
		{
			loadManyDelegate = LoadFromRemote;

			try
			{
				return await UniTask.WhenAll(CreateImageLoadTasks(uris, loadManyDelegate));
			}
			catch (Exception)
			{
				Cts.Cancel(true);
				
				throw new OperationCanceledException();
			}
		}

		/// <summary>
		/// Load sprite from Unity internal resources folder.
		/// </summary>
		/// <param name="path">To load path from resources folder. beware not to add extension</param>
		/// <returns>Async laod sprite with UniTask</returns>
		public static async UniTask<Sprite> LoadFromResourcesFolder(string path)
		{
			if (await Resources.LoadAsync<Sprite>(path) is Sprite resourcesSprite)
			{
				Log.Print($"Load complete: {path}");
				
				return resourcesSprite;
			}
			
			Log.Print($"The image in path({path}) is not exist.", LogPriority.Error);

			return null;
		}

		/// <summary>
		/// Load sprites from Unity internal resources folder.
		/// </summary>
		/// <param name="paths">To load paths enumerable</param>
		/// <returns>Async load sprites array with UniTask</returns>
		public static async UniTask<Sprite[]> LoadManyFromResourcesFolder(IEnumerable<string> paths)
		{
			loadManyDelegate = LoadFromResourcesFolder;
			
			return await UniTask.WhenAll(CreateImageLoadTasks(paths, loadManyDelegate));
		}

		private static bool ValidationImageFileExtension(string e)
		{
			if (string.IsNullOrEmpty(e) || string.IsNullOrWhiteSpace(e))
			{
				return false;
			}

			return SupportImageFileExtension.Contains(e);
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

		private static Sprite ConvertByteTextureToSprite(byte[] btb)
		{
			var texture = new Texture2D(2, 2);
			texture.LoadImage(btb);

			var rect = new Rect(0, 0, texture.width, texture.height);
			
			return Sprite.Create(texture, rect, Vector2.one * 0.5f);
		}

		private static IEnumerable<UniTask<Sprite>> CreateImageLoadTasks(IEnumerable<string> p, TaskDelegate td)
		{
			return Enumerable.Select(p, td.Invoke);
		}
	}
}