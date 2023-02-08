using System.Collections.Generic;
using System.Globalization;

namespace Horang.HorangUnityLibrary.Utilities.PlayerPrefs
{
	public struct SetPlayerPrefs
	{
		public static void Int(string key, int value)
		{
			var eK = Encryption.Encrypt(key);
			var eV = Encryption.Encrypt(value.ToString());
			
			Save(eK, eV);
		}

		public static void IntArray(string key, IEnumerable<int> value)
		{
			var eK = Encryption.Encrypt(key);
			var eV = PlayerPrefsUtilities.ArrayToStringConverter(value);

			Save(eK, eV);
		}
		
		public static void Long(string key, long value)
		{
			var eK = Encryption.Encrypt(key);
			var eV = Encryption.Encrypt(value.ToString());
			
			Save(eK, eV);
		}

		public static void LongArray(string key, IEnumerable<long> value)
		{
			var eK = Encryption.Encrypt(key);
			var eV = PlayerPrefsUtilities.ArrayToStringConverter(value);

			Save(eK, eV);
		}
		
		public static void Float(string key, float value)
		{
			var eK = Encryption.Encrypt(key);
			var eV = Encryption.Encrypt(value.ToString(CultureInfo.InvariantCulture));
			
			Save(eK, eV);
		}

		public static void FloatArray(string key, IEnumerable<float> value)
		{
			var eK = Encryption.Encrypt(key);
			var eV = PlayerPrefsUtilities.ArrayToStringConverter(value);

			Save(eK, eV);
		}
		
		public static void Double(string key, double value)
		{
			var eK = Encryption.Encrypt(key);
			var eV = Encryption.Encrypt(value.ToString(CultureInfo.InvariantCulture));
			
			Save(eK, eV);
		}

		public static void DoubleArray(string key, IEnumerable<double> value)
		{
			var eK = Encryption.Encrypt(key);
			var eV = PlayerPrefsUtilities.ArrayToStringConverter(value);

			Save(eK, eV);
		}
		
		public static void String(string key, string value)
		{
			var eK = Encryption.Encrypt(key);
			var eV = Encryption.Encrypt(value);
			
			Save(eK, eV);
		}

		public static void StringArray(string key, IEnumerable<string> value)
		{
			var eK = Encryption.Encrypt(key);
			var eV = PlayerPrefsUtilities.ArrayToStringConverter(value);

			Save(eK, eV);
		}

		private static void Save(string eK, string eV)
		{
			UnityEngine.PlayerPrefs.SetString(eK, eV);
			UnityEngine.PlayerPrefs.Save();
		}
	}
}