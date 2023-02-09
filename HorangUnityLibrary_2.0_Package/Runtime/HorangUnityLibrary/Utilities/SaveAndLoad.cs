using System.IO;
using System.Text;
using Cysharp.Threading.Tasks;

namespace Horang.HorangUnityLibrary.Utilities
{
	public enum WriteMode
	{
		Append = 6,
		New = 2,
	}
	
	public struct SaveAndLoad
	{
		public static async UniTaskVoid Save(string path, string key, string value, WriteMode writeMode = WriteMode.Append)
		{
			var fs = File.Exists(path) is false ? new FileStream(path, FileMode.Create) : new FileStream(path, (FileMode)(int)writeMode);
			var sw = new StreamWriter(fs);
			var sb = new StringBuilder(key).Append(" = ").Append(value).Append('\n').ToString();

			await sw.WriteAsync(sb);

			sw.Close();
			fs.Close();
			
			await sw.DisposeAsync();
			await fs.DisposeAsync();
		}

		public static async UniTask<string> Load(string path, string key)
		{
			var fs = File.Exists(path) is false ? null : new FileStream(path, FileMode.Open);

			if (fs is null)
			{
				Log.Print($"Cannot find file in path({path}).", LogPriority.Error);

				return string.Empty;
			}
			
			var sr = new StreamReader(fs, Encoding.UTF8);

			while (!sr.EndOfStream)
			{
				var line = await sr.ReadLineAsync();
				var data = line.Split(" = ");

				if (string.IsNullOrEmpty(data[0]) || string.IsNullOrWhiteSpace(data[0])
				    && string.IsNullOrEmpty(data[1]) || string.IsNullOrWhiteSpace(data[1])
				    && data.Length < 2)
				{
					Log.Print("Data invalid.", LogPriority.Error);
					
					sr.Close();
					fs.Close();
			
					sr.Dispose();
					await fs.DisposeAsync();
					
					return string.Empty;
				}

				if (data[0].Equals(key) is false)
				{
					continue;
				}
				
				sr.Close();
				fs.Close();
			
				sr.Dispose();
				await fs.DisposeAsync();
					
				return data[1];
			}
			
			Log.Print($"Cannot find value with key({key})", LogPriority.Error);
			
			sr.Close();
			fs.Close();
			
			sr.Dispose();
			await fs.DisposeAsync();

			return string.Empty;
		}
	}
}