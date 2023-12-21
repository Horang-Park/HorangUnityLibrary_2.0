using System.Collections.Generic;
using System.Linq;

namespace Horang.HorangUnityLibrary.Utilities.LanguageExtensions
{
	public static class GenericExtension
	{
		public static List<T> Shuffle<T>(this List<T> source)
		{
			var random = new System.Random();

			return source.OrderBy(_ => random.Next()).ToList();
		}

		public static bool TryAdd<T>(this List<T> source, T value)
		{
			if (source.Contains(value))
			{
				return false;
			}

			source.Add(value);

			return true;
		}

		public static bool TryPush<T>(this Stack<T> source, T value)
		{
			if (source.Contains(value))
			{
				return false;
			}
			
			source.Push(value);

			return true;
		}
	}
}