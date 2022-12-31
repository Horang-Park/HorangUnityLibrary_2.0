using System;
using System.Diagnostics;
using System.IO;
using System.Text;
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
		private static readonly string[] LoggerPriorityColorPrefix =
		{
			"<color=#2d75eb>",
			"<color=#ebebeb>",
			"<color=#fc960f>",
			"<color=#fc460f>",
			"<color=#f725a3>",
			"<color=#a6a6a6>",
		};
		private const string LoggerPriorityColorPostfix = "</color>";

		private const string FontSizePrefix = "<size=13>";
		private const string FontSizePostfix = "</size>";
		private const string FontBoldPrefix = "<b>";
		private const string FontBoldPostfix = "</b>";

		private static readonly char[] PathSeparator = { '\\', '/' };
		
		private const string Separator = " ▶ ";
		private const char LineNumberSeparator = ':';

		private const char OpenBracket = '[';
		private const string CloseBracket = "] ";

		public static string Log(LogPriority priority, string message)
		{
#if SHOW_LOG
			var st = new StackTrace(true);
			var sf = st.GetFrame(1);
			var fn = Path.GetFileNameWithoutExtension(sf.GetFileName()?.Split(PathSeparator, StringSplitOptions.RemoveEmptyEntries)[^1]);
			var sb = new StringBuilder(FontBoldPrefix);
			
			sb.Append(FontSizePrefix);
			sb.Append(LoggerPriorityColorPrefix[(int)priority]);
			sb.Append(OpenBracket);
			sb.Append(fn);
			sb.Append(LineNumberSeparator);
			sb.Append(sf.GetFileLineNumber());
			sb.Append(CloseBracket);
			sb.Append(sf.GetMethod().Name);
			sb.Append(Separator);
			sb.Append(message);
			sb.Append(LoggerPriorityColorPostfix);
			sb.Append(FontSizePostfix);
			sb.Append(FontBoldPostfix);

			switch (priority)
			{
				case LogPriority.Debug:
				case LogPriority.Verbose:
					UnityEngine.Debug.Log(sb.ToString());
					break;
				case LogPriority.Warning:
					UnityEngine.Debug.LogWarning(sb.ToString());
					break;
				case LogPriority.Error:
				case LogPriority.Exception:
					UnityEngine.Debug.LogError(sb.ToString());
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(priority), priority, null);
			}
			
			sb.Clear();
#endif

			return message;
		}
	}
}
