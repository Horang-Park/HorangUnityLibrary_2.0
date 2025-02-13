using System.Collections.Generic;
using System.Globalization;

namespace Horang.HorangUnityLibrary.Utilities.PlayerPrefs
{
	public struct SetPlayerPrefs
	{
		/// <summary>
		/// Save int value.
		/// </summary>
		/// <param name="key">To save key</param>
		/// <param name="value">To save value</param>
		public static void Int(string key, int value)
		{
			var eK = Encryption.Encrypt(key);
			var eV = Encryption.Encrypt(value.ToString());
			
			Save(eK, eV);
		}

		/// <summary>
		/// Save int array value.
		/// </summary>
		/// <param name="key">To save key</param>
		/// <param name="value">To save value</param>
		public static void IntArray(string key, IEnumerable<int> value)
		{
			var eK = Encryption.Encrypt(key);
			var eV = PlayerPrefsUtilities.ArrayToStringConverter(value);

			Save(eK, eV);
		}
		
		/// <summary>
		/// Save long value.
		/// </summary>
		/// <param name="key">To save key</param>
		/// <param name="value">To save value</param>
		public static void Long(string key, long value)
		{
			var eK = Encryption.Encrypt(key);
			var eV = Encryption.Encrypt(value.ToString());
			
			Save(eK, eV);
		}

		/// <summary>
		/// Save long array value.
		/// </summary>
		/// <param name="key">To save key</param>
		/// <param name="value">To save value</param>
		public static void LongArray(string key, IEnumerable<long> value)
		{
			var eK = Encryption.Encrypt(key);
			var eV = PlayerPrefsUtilities.ArrayToStringConverter(value);

			Save(eK, eV);
		}
		
		/// <summary>
		/// Save float value.
		/// </summary>
		/// <param name="key">To save key</param>
		/// <param name="value">To save value</param>
		public static void Float(string key, float value)
		{
			var eK = Encryption.Encrypt(key);
			var eV = Encryption.Encrypt(value.ToString(CultureInfo.InvariantCulture));
			
			Save(eK, eV);
		}

		/// <summary>
		/// Save float array value.
		/// </summary>
		/// <param name="key">To save key</param>
		/// <param name="value">To save value</param>
		public static void FloatArray(string key, IEnumerable<float> value)
		{
			var eK = Encryption.Encrypt(key);
			var eV = PlayerPrefsUtilities.ArrayToStringConverter(value);

			Save(eK, eV);
		}
		
		/// <summary>
		/// Save double value.
		/// </summary>
		/// <param name="key">To save key</param>
		/// <param name="value">To save value</param>
		public static void Double(string key, double value)
		{
			var eK = Encryption.Encrypt(key);
			var eV = Encryption.Encrypt(value.ToString(CultureInfo.InvariantCulture));
			
			Save(eK, eV);
		}

		/// <summary>
		/// Save double array value.
		/// </summary>
		/// <param name="key">To save key</param>
		/// <param name="value">To save value</param>
		public static void DoubleArray(string key, IEnumerable<double> value)
		{
			var eK = Encryption.Encrypt(key);
			var eV = PlayerPrefsUtilities.ArrayToStringConverter(value);

			Save(eK, eV);
		}
		
		/// <summary>
		/// Save string value.
		/// </summary>
		/// <param name="key">To save key</param>
		/// <param name="value">To save value</param>
		public static void String(string key, string value)
		{
			var eK = Encryption.Encrypt(key);
			var eV = Encryption.Encrypt(value);
			
			Save(eK, eV);
		}

		/// <summary>
		/// Save string array value.
		/// </summary>
		/// <param name="key">To save key</param>
		/// <param name="value">To save value</param>
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