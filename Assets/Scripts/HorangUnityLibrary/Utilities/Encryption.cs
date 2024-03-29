using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine.Device;

namespace Horang.HorangUnityLibrary.Utilities
{
	public struct Encryption
	{
		private static readonly RijndaelManaged Rijndael = CreateRijndaelManaged();

		/// <summary>
		/// Encryption.
		/// </summary>
		/// <param name="toEncrypt">To encrypt data</param>
		/// <returns>Encrypted data</returns>
		public static string Encrypt(string toEncrypt)
		{
#if UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR
			var cTransform = Rijndael.CreateEncryptor();
			var toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);
			var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
			
			return Convert.ToBase64String(resultArray, 0, resultArray.Length);
#else
			Log.Print($"Unsupported platform. ({Application.platform}) Always return parameter value.", LogPriority.Warning);

			return toEncrypt;
#endif
		}

		/// <summary>
		/// Encryption byte array.
		/// </summary>
		/// <param name="toEncrypt">To encrypt data</param>
		/// <returns>Encrypted data</returns>
		public static byte[] Encrypt(byte[] toEncrypt)
		{
#if UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR
			var cTransform = Rijndael.CreateEncryptor();
			var resultArray = cTransform.TransformFinalBlock(toEncrypt, 0, toEncrypt.Length);

			return resultArray;
#else
			Log.Print($"Unsupported platform. ({Application.platform}) Always return parameter value.", LogPriority.Warning);

			return toEncrypt;
#endif
		}
		
		/// <summary>
		/// Decryption.
		/// </summary>
		/// <param name="toDecrypt">To decrypt data</param>
		/// <returns>Decrypted data</returns>
		public static string Decrypt(string toDecrypt)
		{
#if UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR
			var toDecryptArray = Convert.FromBase64String(toDecrypt);
			var cTransform = Rijndael.CreateDecryptor();
			var resultArray = cTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);
			
			return Encoding.UTF8.GetString(resultArray);
#else
			Log.Print($"Unsupported platform. ({Application.platform}) Always return parameter value.", LogPriority.Warning);

			return toDecrypt;
#endif
		}
		
		/// <summary>
		/// Decryption byte array.
		/// </summary>
		/// <param name="toDecrypt">To decrypt data</param>
		/// <returns>Decrypted data</returns>
		public static byte[] Decrypt(byte[] toDecrypt)
		{
#if UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR
			var cTransform = Rijndael.CreateDecryptor();
			var resultArray = cTransform.TransformFinalBlock(toDecrypt, 0, toDecrypt.Length);

			return resultArray;
#else
			Log.Print($"Unsupported platform. ({Application.platform}) Always return parameter value.", LogPriority.Warning);

			return toDecrypt;
#endif
		}
		
		private static RijndaelManaged CreateRijndaelManaged()
		{
			var result = new RijndaelManaged();
			result.Mode = CipherMode.CBC;
			result.Padding = PaddingMode.PKCS7;
			result.KeySize = 128;
			result.BlockSize = 128;
			
			var pwdBytes = Encoding.UTF8.GetBytes(SystemInfo.deviceUniqueIdentifier);
			var keyBytes = new byte[16];
			var len = pwdBytes.Length;
			
			if (len > keyBytes.Length)
			{
				len = keyBytes.Length;
			}
			
			Array.Copy(pwdBytes, keyBytes, len);
			result.Key = keyBytes;
			result.IV = keyBytes;

			return result;
		}
	}
}