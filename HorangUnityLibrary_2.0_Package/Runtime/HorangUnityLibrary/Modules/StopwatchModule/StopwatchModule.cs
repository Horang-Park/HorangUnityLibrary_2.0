using System;
using System.Collections.Generic;
using Horang.HorangUnityLibrary.Utilities;

namespace Horang.HorangUnityLibrary.Modules.StopwatchModule
{
	public static class StopwatchModule
	{
		private static readonly Dictionary<int, StopwatchElement> Stopwatches = new();

		/// <summary>
		/// Start stopwatch. If parameter named stopwatch is not exist, it will make new one.
		/// </summary>
		/// <param name="name">Name of stopwatch</param>
		public static void Start(string name)
		{
			var key = name.GetHashCode();
			StopwatchElement stopwatchElement;

			if (ValidateStopwatch(key) is false)
			{
				Log.Print($"[{name}] stopwatch is not exist. generate one.", LogPriority.Verbose);

				stopwatchElement = StopwatchElement.Create(name);
				Stopwatches.Add(key, stopwatchElement);
			}
			else
			{
				stopwatchElement = Stopwatches[key];
			}

			if (stopwatchElement.IsRunning)
			{
				Log.Print($"[{stopwatchElement.StopwatchName}] stopwatch is already running.", LogPriority.Warning);

				return;
			}

			if (stopwatchElement.IsPaused)
			{
				Log.Print($"[{stopwatchElement.StopwatchName}] stopwatch is paused.", LogPriority.Verbose);

				return;
			}

			stopwatchElement.Start();
		}

		/// <summary>
		/// Stop stopwatch.
		/// </summary>
		/// <param name="name">Name of stopwatch</param>
		/// <returns>If stopwatch is invalid, it will return null. otherwise elapsed time into millisecond</returns>
		public static long? Stop(string name)
		{
			var key = name.GetHashCode();

			if (ValidateStopwatch(key) is false)
			{
				Log.Print($"Cannot find stopwatch that named [{name}]", LogPriority.Error);

				return null;
			}

			var stopwatchElement = Stopwatches[key];

			if (stopwatchElement.IsRunning is false)
			{
				Log.Print($"[{stopwatchElement.StopwatchName}] stopwatch is not running.", LogPriority.Warning);

				return null;
			}

			return stopwatchElement.Stop();
		}

		/// <summary>
		/// Pause stopwatch its status is running.
		/// </summary>
		/// <param name="name">Name of stopwatch</param>
		public static void Pause(string name)
		{
			var key = name.GetHashCode();

			if (ValidateStopwatch(key) is false)
			{
				Log.Print($"Cannot find stopwatch that named [{name}]", LogPriority.Error);

				return;
			}

			var stopwatchElement = Stopwatches[key];

			if (stopwatchElement.IsRunning is false)
			{
				Log.Print($"[{stopwatchElement.StopwatchName}] stopwatch is not running.", LogPriority.Warning);

				return;
			}
			
			if (stopwatchElement.IsPaused)
			{
				Log.Print($"[{stopwatchElement.StopwatchName}] stopwatch is already paused.", LogPriority.Warning);

				return;
			}

			stopwatchElement.Pause();
		}

		/// <summary>
		/// Resume stopwatch its status is paused.
		/// </summary>
		/// <param name="name">Name of stopwatch</param>
		public static void Resume(string name)
		{
			var key = name.GetHashCode();

			if (ValidateStopwatch(key) is false)
			{
				Log.Print($"Cannot find stopwatch that named [{name}]", LogPriority.Error);

				return;
			}

			var stopwatchElement = Stopwatches[key];
			
			if (stopwatchElement.IsPaused is false)
			{
				Log.Print($"[{stopwatchElement.StopwatchName}] stopwatch is not pause.", LogPriority.Warning);

				return;
			}

			stopwatchElement.Resume();
		}

		/// <summary>
		/// The first start date and time of parameter named stopwatch.
		/// </summary>
		/// <param name="name">Name of stopwatch</param>
		/// <returns>If stopwatch is invalid, it will return null. otherwise first start datetime</returns>
		public static DateTime? StopwatchStartDateTime(string name)
		{
			var key = name.GetHashCode();

			if (ValidateStopwatch(key) is false)
			{
				Log.Print($"Cannot find stopwatch that named [{name}]", LogPriority.Error);

				return DateTime.MinValue;
			}

			return Stopwatches[key].StopwatchStartDateTime;
		}
		
		/// <summary>
		/// The most last stopped date and time of parameter named stopwatch.
		/// </summary>
		/// <param name="name">Name of stopwatch</param>
		/// <returns>If stopwatch is invalid, it will return null. otherwise datetime</returns>
		public static DateTime? StopwatchLastStopDateTime(string name)
		{
			var key = name.GetHashCode();

			if (ValidateStopwatch(key) is false)
			{
				Log.Print($"Cannot find stopwatch that named [{name}]", LogPriority.Error);

				return DateTime.MinValue;
			}

			return Stopwatches[key].StopwatchLastStopDateTime;
		}
		
		/// <summary>
		/// The most last elapsed time when stopwatch is stopped.
		/// </summary>
		/// <param name="name">Name of stopwatch</param>
		/// <returns>If stopwatch is invalid, it will return null. otherwise last elapsed time into millisecond</returns>
		public static long? StopwatchLastElapsedTime(string name)
		{
			var key = name.GetHashCode();

			if (ValidateStopwatch(key) is false)
			{
				Log.Print($"Cannot find stopwatch that named [{name}]", LogPriority.Error);

				return null;
			}

			return Stopwatches[key].LastElapsedTime;
		}

		public static void AddTimeTriggerEvent(string name, long milliseconds, Action action)
		{
			var key = name.GetHashCode();
			StopwatchElement stopwatchElement;

			if (ValidateStopwatch(key) is false)
			{
				Log.Print($"[{name}] stopwatch is not exist. generate one.", LogPriority.Verbose);

				stopwatchElement = StopwatchElement.Create(name);
				Stopwatches.Add(key, stopwatchElement);
			}
			else
			{
				stopwatchElement = Stopwatches[key];
			}
			
			stopwatchElement.AddTimeTriggerEvent(milliseconds, action);
		}

		public static void Dispose()
		{
			foreach (var stopwatchElement in Stopwatches.Values)
			{
				stopwatchElement.Dispose();
			}
		}
		
		private static bool ValidateStopwatch(int k)
		{
			return Stopwatches.ContainsKey(k);
		}
	}
}