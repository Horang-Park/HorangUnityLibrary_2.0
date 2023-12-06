using System.Collections.Generic;
using System.Linq;

namespace Horang.HorangUnityLibrary.Utilities.LanguageExtensions
{
	public static class GenericExtension
	{
		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source) where T : List<T>
		{
			var random = new System.Random();
		
			return source.OrderBy(_ => random.Next());
		}

		public static bool TryAdd<T>(this List<T> source, T value) where T : List<T>
		{
			if (source.Contains(value))
			{
				return false;
			}

			source.Add(value);

			return true;
		}

		public static bool TryPush<T>(this Stack<T> source, T value) where T : Stack<T>
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