using System.Collections.Generic;

namespace Horang.HorangUnityLibrary.Utilities.PlayerPrefs
{
	public struct GetPlayerPrefs
	{
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
		
		public static long Long(string key)
		{
			var eK = Encryption.Encrypt(key);

			if (PlayerPrefsUtilities.KeyValidation(eK))
			{
				return long.Parse(Encryption.Decrypt(UnityEngine.PlayerPrefs.GetString(eK)));
			}
			
			Log.Print($"Cannot find the key [{key}] in local player preferences.", LogPriority.Error);

			return int.MaxValue;
		}

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
		
		public static float Float(string key)
		{
			var eK = Encryption.Encrypt(key);

			if (PlayerPrefsUtilities.KeyValidation(eK))
			{
				return float.Parse(Encryption.Decrypt(UnityEngine.PlayerPrefs.GetString(eK)));
			}
			
			Log.Print($"Cannot find the key [{key}] in local player preferences.", LogPriority.Error);

			return int.MaxValue;
		}
		
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
		
		public static double Double(string key)
		{
			var eK = Encryption.Encrypt(key);

			if (PlayerPrefsUtilities.KeyValidation(eK))
			{
				return double.Parse(Encryption.Decrypt(UnityEngine.PlayerPrefs.GetString(eK)));
			}
			
			Log.Print($"Cannot find the key [{key}] in local player preferences.", LogPriority.Error);

			return int.MaxValue;
		}
		
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