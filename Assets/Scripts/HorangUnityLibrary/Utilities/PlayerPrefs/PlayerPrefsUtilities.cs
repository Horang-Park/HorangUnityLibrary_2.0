using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Horang.HorangUnityLibrary.Utilities.PlayerPrefs
{
	public struct PlayerPrefsUtilities
	{
		private const char ArrayPadding = '\t';

		internal static bool KeyValidation(string eK)
		{
			return UnityEngine.PlayerPrefs.HasKey(eK);
		}

		internal static IEnumerable<T> StringToArrayConverter<T>(string eK)
		{
			var encryptedValue = UnityEngine.PlayerPrefs.GetString(eK);
			var decrypt = Utilities.Encryption.Decrypt(encryptedValue).Split(ArrayPadding);
			var converter = TypeDescriptor.GetConverter(typeof(T));

			return from item
					in decrypt
				where !string.IsNullOrEmpty(item)
				select (T)converter.ConvertFrom(item);
		}

		internal static string ArrayToStringConverter<T>(IEnumerable<T> oV)
		{
			var sb = new StringBuilder();
			
			foreach (var item in oV)
			{
				sb.Append(item);
				sb.Append(ArrayPadding);
			}
			
			return Encryption.Encrypt(sb.ToString());
		}
	}
}