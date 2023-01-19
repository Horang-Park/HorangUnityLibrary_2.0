using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Unity.VisualScripting;

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
	
	/// <summary>
	/// Log utility class.
	/// </summary>
	public static class Log
	{
		private static readonly string[] LoggerPriorityColorPrefix =
		{
			"<color=#2d75eb>",
			"<color=#ebebeb>",
			"<color=#fc960f>",
			"<color=#fc460f>",
			"<color=#f725a3>"
		};
		private static readonly char[] PathSeparator = { '\\', '/' };
		private static readonly List<string> LogHistory = new();

		private const string LoggerPriorityColorPostfix = "</color>";
		private const string FontSizePrefix = "<size=13>";
		private const string FontSizePostfix = "</size>";
		private const string Separator = " ▶ ";
		private const char LineNumberSeparator = ':';
		private const char OpenBracket = '[';
		private const char CloseBracket = ']';

		private const string LogFileNamePrefix = "+++ LOG +++ ";
		private const string LogFileExtension = ".log";

		/// <summary>
		/// Show log in Unity console window.
		/// </summary>
		/// <param name="message">To show log</param>
		/// <param name="logPriority">Log's priority</param>
		public static void Print(string message, LogPriority logPriority = LogPriority.Debug)
		{
			var builtLog = LogBuilder(message, logPriority, 2);
			
			LogHistory.Add(builtLog);

#if SHOW_LOG
			// ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
			switch (logPriority)
			{
				case LogPriority.Debug:
				case LogPriority.Verbose:
					UnityEngine.Debug.Log(builtLog);
					break;
				case LogPriority.Warning:
					UnityEngine.Debug.LogWarning(builtLog);
					break; 
				case LogPriority.Error:
				case LogPriority.Exception:
					UnityEngine.Debug.LogError(builtLog);
					break;
			}
#endif
		}

		/// <summary>
		/// Show log in Unity console window by current string.
		/// </summary>
		/// <param name="message">To this string make log and show</param>
		/// <param name="logPriority">Log's priority</param>
		/// <returns>Original string value</returns>
		public static string ToLog(this string message, LogPriority logPriority = LogPriority.Debug)
		{
			var builtLog = LogBuilder(message, logPriority, 2);
			
			LogHistory.Add(builtLog);

#if SHOW_LOG
			// ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
			switch (logPriority)
			{
				case LogPriority.Debug:
				case LogPriority.Verbose:
					UnityEngine.Debug.Log(builtLog);
					break;
				case LogPriority.Warning:
					UnityEngine.Debug.LogWarning(builtLog);
					break; 
				case LogPriority.Error:
				case LogPriority.Exception:
					UnityEngine.Debug.LogError(builtLog);
					break;
			}
#endif

			return message;
		}

		/// <summary>
		/// Clear all recorded log(s)
		/// </summary>
		public static void ClearLogHistory()
		{
			LogHistory.Clear();
		}

		/// <summary>
		/// Export log to provided file path. (Log history will be clear its operation done.)
		/// </summary>
		/// <param name="directoryPath">To export directory (If can't find directory, it will generate directory automatically)</param>
		/// <returns>If operation is not complete will return false, success is true</returns>
		public static async UniTask<bool> ExportLogHistory(string directoryPath)
		{
			if (LogHistory.Count < 1)
			{
				Print("Nothing to write log. Log history is clean.", LogPriority.Warning);
				
				return false;
			}
			
			var logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff");
			var fileTotalName = new StringBuilder(LogFileNamePrefix).Append(OpenBracket).Append(logDate).Append(CloseBracket).Append(LogFileExtension).ToString();
			var savePath = Path.Combine(directoryPath, fileTotalName);

			try
			{
				if (Directory.Exists(directoryPath) is false)
				{
					Directory.CreateDirectory(directoryPath);
				}
			
				await LogWrite(savePath, logDate);
			}
			catch (Exception e)
			{
				Print($"Can't generate log file in \"{directoryPath}\" as file name \"{fileTotalName}\". / Exception message: \"{e.Message}\"", LogPriority.Exception);
				
				ClearLogHistory();
				
				return false;
			}
			
			Print($"Successfully generate log in \"{savePath}\".");
			
			ClearLogHistory();
			
			return true;
		}

		private static string LogBuilder(string m, LogPriority p, int n)
		{
			var st = new StackTrace(true);
			var sf = GetStackFrame(st, n);
			var fn = Path.GetFileNameWithoutExtension(sf.GetFileName()?.Split(PathSeparator, StringSplitOptions.RemoveEmptyEntries)[^1]);
			var sb = new StringBuilder(FontSizePrefix);
			
			sb.Append(LoggerPriorityColorPrefix[(int)p]);
			sb.Append(OpenBracket);
			sb.Append(fn);
			sb.Append(LineNumberSeparator);
			sb.Append(sf.GetFileLineNumber());
			sb.Append(CloseBracket).Append(' ');
			sb.Append(sf.GetMethod().Name);
			sb.Append(Separator);
			sb.Append(m);
			sb.Append(LoggerPriorityColorPostfix);
			sb.Append(FontSizePostfix);

			return sb.ToString();
		}
		
		private static StackFrame GetStackFrame(StackTrace st, int n)
		{
			return st.GetFrame(n);
		}

		private static async UniTask LogWrite(string p, string d)
		{
			var fs = new FileStream(p, FileMode.Create);
			var sw = new StreamWriter(fs);

			await sw.WriteAsync($"Log file exported at {d}\n\n");

			await foreach (var log in LogHistory.ToUniTaskAsyncEnumerable())
			{
				await sw.WriteLineAsync(log);
			}

			await sw.DisposeAsync();
			await fs.DisposeAsync();
		}
	}
}