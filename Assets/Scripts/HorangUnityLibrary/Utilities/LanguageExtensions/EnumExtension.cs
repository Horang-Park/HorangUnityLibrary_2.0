using System;
using System.Linq;

namespace Horang.HorangUnityLibrary.Utilities.LanguageExtensions
{
	public static class EnumExtension
	{
		public static T[] GetAllEnumValueToArray<T>(this Enum _) where T : Enum
		{
			return Enum.GetValues(typeof(T))
				.Cast<T>()
				.ToArray();
		}
		
		public static T Last<T>(this Enum _) where T : Enum
		{
			return Enum
				.GetValues(typeof(T))
				.Cast<T>()
				.Last();
		}

		public static T First<T>(this Enum _) where T : Enum
		{
			return Enum
				.GetValues(typeof(T))
				.Cast<T>()
				.First();
		}

		public static int Count<T>(this Enum _) where T : Enum
		{
			return Enum.GetValues(typeof(T)).Length;
		}
	}
}