using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Horang.HorangUnityLibrary.Utilities
{
	public struct Compression
	{
		/// <summary>
		/// Compression the source path file.
		/// </summary>
		/// <param name="sourcePath">To compress file path</param>
		/// <param name="destinationPath">To export file path</param>
		/// <returns>If compression worked successfully, will return true. otherwise false</returns>
		public static bool Zip(string sourcePath, string destinationPath)
		{
			if (File.Exists(destinationPath))
			{
				File.Delete(destinationPath);
			}

			try
			{
				ZipFile.CreateFromDirectory(sourcePath, destinationPath);
			
				Log.Print($"The source in [{sourcePath}] is zipped in [{destinationPath}] successfully.", LogPriority.Verbose);

				return true;
			}
			catch (Exception e)
			{
				Log.Print($"The source({sourcePath}) cannot be compressed in [{destinationPath}] / Message: {e.Message}", LogPriority.Exception);

				return false;
			}
		}

		/// <summary>
		/// Decompression the source path file.
		/// </summary>
		/// <param name="sourcePath">To decompress file path</param>
		/// <param name="destinationPath">To export file path</param>
		/// <returns>If decompression worked successfully, will return true. otherwise false</returns>
		public static bool Unzip(string sourcePath, string destinationPath)
		{
			if (File.Exists(destinationPath))
			{
				File.Delete(destinationPath);
			}

			try
			{
				ZipFile.ExtractToDirectory(sourcePath, destinationPath);
			
				Log.Print($"The source in [{sourcePath}] is unzipped in [{destinationPath}] successfully.", LogPriority.Verbose);

				return true;
			}
			catch (Exception e)
			{
				Log.Print($"The source({sourcePath}) cannot be compressed in [{destinationPath}] / Message: {e.Message}", LogPriority.Exception);

				return false;
			}
		}

		/// <summary>
		/// To get file list from source path zip file.
		/// </summary>
		/// <param name="sourcePath">zip file path</param>
		/// <returns>File names in source path zip file</returns>
		public static IEnumerable<string> GetFileList(string sourcePath)
		{
			if (File.Exists(sourcePath) is false)
			{
				Log.Print($"The zip file in [{sourcePath}] is not exist.", LogPriority.Error);

				return null;
			}

			var archive = ZipFile.Open(sourcePath, ZipArchiveMode.Read);

			return archive.Entries.Select(entry => entry.Name);
		}
	}
}