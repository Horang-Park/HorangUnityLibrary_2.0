using System.Data;
using System.IO;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Horang.HorangUnityLibrary.Utilities.Parser
{
	public struct CsvParser
	{
		private const char SplitCharacter = ',';
		
		/// <summary>
		/// Load CSV file from local storage.
		/// </summary>
		/// <param name="filePath">To load file path</param>
		/// <returns>Async by UniTask and returning DataTable that made by CSV from file path</returns>
		public static async UniTask<DataTable> LoadFromLocal(string filePath)
		{
			if (Path.GetExtension(filePath).Equals(@".csv") is false)
			{
				Log.Print($"The file in path({filePath}) is not CSV file.", LogPriority.Error);

				await UniTask.CompletedTask;
			}
			
			var headerAddedFlag = false;
			var dT = new DataTable();
			var sr = new StreamReader(filePath, Encoding.UTF8);

			while (!sr.EndOfStream)
			{
				var line = await sr.ReadLineAsync();
				var data = line.Split(SplitCharacter);

				if (headerAddedFlag is false)
				{
					foreach (var item in data)
					{
						dT.Columns.Add(item);
					}
					
					headerAddedFlag = true;
				}

				dT.Rows.Add((object)data);
			}

			return dT;
		}
		
		/// <summary>
		/// Load CSV file from Unity resources folder.
		/// </summary>
		/// <param name="filePath">To load file path</param>
		/// <returns>Async by UniTask and returning DataTable that made by CSV from file path</returns>
		public static async UniTask<DataTable> LoadFromResources(string filePath)
		{
			var headerAddedFlag = false;
			var dT = new DataTable();
			var tA = await Resources.LoadAsync<TextAsset>(filePath) as TextAsset;
			var separatedLines = tA!.text.Split('\n');
			
			foreach (var line in separatedLines)
			{
				var data = line.Split(SplitCharacter);

				if (headerAddedFlag is false)
				{
					foreach (var item in data)
					{
						dT.Columns.Add(item);
					}

					headerAddedFlag = true;
				}

				dT.Rows.Add((object)data);
			}

			return dT;
		}
	}
}