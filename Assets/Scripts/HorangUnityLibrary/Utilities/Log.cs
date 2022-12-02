using UnityEngine;

namespace HorangUnityLibrary.Utilities
{
	public enum LogPriority
	{
		Debug,
		Verbose,
		Warning,
		Error,
		Exception,
	}

	public static class Logging
	{
		public static string ToLog(this string message, LogPriority logPriority)
		{
			return message;
		}

		public static void Log(string message, LogPriority logPriority)
		{
			
		}
	}
}
