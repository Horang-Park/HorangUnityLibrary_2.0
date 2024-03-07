using System.Collections.Generic;

namespace Horang.HorangUnityLibrary.Utilities.PlayerPrefs
{
	public struct GetPlayerPrefs
	{
		/// <summary>
		/// Get saved bool value.
		/// </summary>
		/// <param name="key">original key</param>
		/// <returns>If found key, returning saved value. otherwise false</returns>
		public static bool Bool(string key)
		{
			var eK = Encryption.Encrypt(key);

			if (PlayerPrefsUtilities.KeyValidation(eK))
			{
				var eV = UnityEngine.PlayerPrefs.GetString(eK);
				var dV = Encryption.Decrypt(eV);
				
				return bool.Parse(dV);
			}
			
			Log.Print($"Cannot find the key [{key}] in local player preferences.", LogPriority.Error);

			return false;
		}

		/// <summary>
		/// Get saved bool array value.
		/// </summary>
		/// <param name="key">original key</param>
		/// <returns>If found key, returning saved value. otherwise null</returns>
		public static IEnumerable<bool> BoolArray(string key)
		{
			var eK = Encryption.Encrypt(key);

			if (PlayerPrefsUtilities.KeyValidation(eK))
			{
				var dV = PlayerPrefsUtilities.StringToArrayConverter<bool>(eK);

				return dV;
			}
			
			Log.Print($"Cannot find the key [{key}] in local player preferences.", LogPriority.Error);

			return null;
		}
		
		/// <summary>
		/// Get saved int value.
		/// </summary>
		/// <param name="key">original key</param>
		/// <returns>If found key, returning saved value. otherwise int.MaxValue</returns>
		public static int Int(string key)
		{
			var eK = Encryption.Encrypt(key);

			if (PlayerPrefsUtilities.KeyValidation(eK))
			{
				return int.Parse(Encryption.Decrypt(UnityEngine.PlayerPrefs.GetString(eK)));
			}
			
			Log.Print($"Cannot find the key [{key}] in local player preferences.", LogPriority.Error);

			return int.MaxValue;
		}

		/// <summary>
		/// Get saved int array value.
		/// </summary>
		/// <param name="key">original key</param>
		/// <returns>If found key, returning saved value. otherwise null</returns>
		public static IEnumerable<int> IntArray(string key)
		{
			var eK = Encryption.Encrypt(key);

			if (PlayerPrefsUtilities.KeyValidation(eK))
			{
				return PlayerPrefsUtilities.StringToArrayConverter<int>(eK);
			}
			
			Log.Print($"Cannot find the key [{key}] in local player preferences.", LogPriority.Error);

			return null;
		}
		
		/// <summary>
		/// Get saved long value.
		/// </summary>
		/// <param name="key">original key</param>
		/// <returns>If found key, returning saved value. otherwise long.MaxValue</returns>
		public static long Long(string key)
		{
			var eK = Encryption.Encrypt(key);

			if (PlayerPrefsUtilities.KeyValidation(eK))
			{
				return long.Parse(Encryption.Decrypt(UnityEngine.PlayerPrefs.GetString(eK)));
			}
			
			Log.Print($"Cannot find the key [{key}] in local player preferences.", LogPriority.Error);

			return long.MaxValue;
		}

		/// <summary>
		/// Get saved long array value.
		/// </summary>
		/// <param name="key">original key</param>
		/// <returns>If found key, returning saved value. otherwise null</returns>
		public static IEnumerable<long> LongArray(string key)
		{
			var eK = Encryption.Encrypt(key);

			if (PlayerPrefsUtilities.KeyValidation(eK))
			{
				return PlayerPrefsUtilities.StringToArrayConverter<long>(eK);
			}
			
			Log.Print($"Cannot find the key [{key}] in local player preferences.", LogPriority.Error);

			return null;
		}
		
		/// <summary>
		/// Get saved float value.
		/// </summary>
		/// <param name="key">original key</param>
		/// <returns>If found key, returning saved value. otherwise float.MaxValue</returns>
		public static float Float(string key)
		{
			var eK = Encryption.Encrypt(key);

			if (PlayerPrefsUtilities.KeyValidation(eK))
			{
				return float.Parse(Encryption.Decrypt(UnityEngine.PlayerPrefs.GetString(eK)));
			}
			
			Log.Print($"Cannot find the key [{key}] in local player preferences.", LogPriority.Error);

			return float.MaxValue;
		}
		
		/// <summary>
		/// Get saved float array value.
		/// </summary>
		/// <param name="key">original key</param>
		/// <returns>If found key, returning saved value. otherwise null</returns>
		public static IEnumerable<float> FloatArray(string key)
		{
			var eK = Encryption.Encrypt(key);

			if (PlayerPrefsUtilities.KeyValidation(eK))
			{
				return PlayerPrefsUtilities.StringToArrayConverter<float>(eK);
			}
			
			Log.Print($"Cannot find the key [{key}] in local player preferences.", LogPriority.Error);

			return null;
		}
		
		/// <summary>
		/// Get saved double value.
		/// </summary>
		/// <param name="key">original key</param>
		/// <returns>If found key, returning saved value. otherwise double.MaxValue</returns>
		public static double Double(string key)
		{
			var eK = Encryption.Encrypt(key);

			if (PlayerPrefsUtilities.KeyValidation(eK))
			{
				return double.Parse(Encryption.Decrypt(UnityEngine.PlayerPrefs.GetString(eK)));
			}
			
			Log.Print($"Cannot find the key [{key}] in local player preferences.", LogPriority.Error);

			return double.MaxValue;
		}
		
		/// <summary>
		/// Get saved double array value.
		/// </summary>
		/// <param name="key">original key</param>
		/// <returns>If found key, returning saved value. otherwise null</returns>
		public static IEnumerable<double> DoubleArray(string key)
		{
			var eK = Encryption.Encrypt(key);

			if (PlayerPrefsUtilities.KeyValidation(eK))
			{
				return PlayerPrefsUtilities.StringToArrayConverter<double>(eK);
			}
			
			Log.Print($"Cannot find the key [{key}] in local player preferences.", LogPriority.Error);

			return null;
		}
		
		/// <summary>
		/// Get saved string value.
		/// </summary>
		/// <param name="key">original key</param>
		/// <returns>If found key, returning saved value. otherwise string.Empty</returns>
		public static string String(string key)
		{
			var eK = Encryption.Encrypt(key);

			if (PlayerPrefsUtilities.KeyValidation(eK))
			{
				return Encryption.Decrypt(UnityEngine.PlayerPrefs.GetString(eK));
			}
			
			Log.Print($"Cannot find the key [{key}] in local player preferences.", LogPriority.Error);

			return string.Empty;
		}
		
		/// <summary>
		/// Get saved string array value.
		/// </summary>
		/// <param name="key">original key</param>
		/// <returns>If found key, returning saved value. otherwise null</returns>
		public static IEnumerable<string> StringArray(string key)
		{
			var eK = Encryption.Encrypt(key);

			if (PlayerPrefsUtilities.KeyValidation(eK))
			{
				return PlayerPrefsUtilities.StringToArrayConverter<string>(eK);
			}
			
			Log.Print($"Cannot find the key [{key}] in local player preferences.", LogPriority.Error);

			return null;
		}
	}
}