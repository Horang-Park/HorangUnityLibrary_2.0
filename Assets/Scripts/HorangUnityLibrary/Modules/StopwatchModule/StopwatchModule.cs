using System;
using System.Collections.Generic;
using HorangUnityLibrary.Utilities;

namespace HorangUnityLibrary.Modules.StopwatchModule
{
	public class StopwatchModule : BaseModule
	{
		private readonly Dictionary<int, StopwatchElement> stopwatches = new();

		public StopwatchModule(ModuleManager moduleManager) : base(moduleManager)
		{
		}

		public override bool ActiveModule()
		{
			if (base.ActiveModule() is false)
			{
				return false;
			}
			
			Log.Print("Module are activated");

			return true;
		}

		public override bool InactiveModule()
		{
			if (base.InactiveModule() is false)
			{
				return false;
			}
			
			Log.Print("Module are inactivated");

			return true;
		}

		public void Start(string name)
		{
			if (isThisModuleActivated is false)
			{
				return;
			}
			
			var key = name.GetHashCode();
			StopwatchElement stopwatchElement;

			if (ValidateStopwatch(key) is false)
			{
				Log.Print($"[{name}] stopwatch is not exist. generate one.", LogPriority.Verbose);

				stopwatchElement = new StopwatchElement
				{
					StopwatchName = name
				};
				stopwatches.Add(key, stopwatchElement);
			}
			else
			{
				stopwatchElement = stopwatches[key];
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

		public long? Stop(string name)
		{
			if (isThisModuleActivated is false)
			{
				return null;
			}
			
			var key = name.GetHashCode();

			if (ValidateStopwatch(key) is false)
			{
				Log.Print($"Cannot find stopwatch that named [{name}]", LogPriority.Error);

				return null;
			}

			var stopwatchElement = stopwatches[key];

			if (stopwatchElement.IsRunning is false)
			{
				Log.Print($"[{stopwatchElement.StopwatchName}] stopwatch is not running.", LogPriority.Warning);

				return null;
			}

			return stopwatchElement.Stop();
		}

		public void Pause(string name)
		{
			if (isThisModuleActivated is false)
			{
				return;
			}
			
			var key = name.GetHashCode();

			if (ValidateStopwatch(key) is false)
			{
				Log.Print($"Cannot find stopwatch that named [{name}]", LogPriority.Error);

				return;
			}

			var stopwatchElement = stopwatches[key];

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

		public void Resume(string name)
		{
			if (isThisModuleActivated is false)
			{
				return;
			}
			
			var key = name.GetHashCode();

			if (ValidateStopwatch(key) is false)
			{
				Log.Print($"Cannot find stopwatch that named [{name}]", LogPriority.Error);

				return;
			}

			var stopwatchElement = stopwatches[key];
			
			if (stopwatchElement.IsPaused is false)
			{
				Log.Print($"[{stopwatchElement.StopwatchName}] stopwatch is not pause.", LogPriority.Warning);

				return;
			}

			stopwatchElement.Resume();
		}

		public DateTime? StopwatchStartDateTime(string name)
		{
			if (isThisModuleActivated is false)
			{
				return null;
			}
			
			var key = name.GetHashCode();

			if (ValidateStopwatch(key) is false)
			{
				Log.Print($"Cannot find stopwatch that named [{name}]", LogPriority.Error);

				return DateTime.MinValue;
			}

			return stopwatches[key].StopwatchStartDateTime;
		}
		
		public DateTime? StopwatchLastStopDateTime(string name)
		{
			if (isThisModuleActivated is false)
			{
				return null;
			}
			
			var key = name.GetHashCode();

			if (ValidateStopwatch(key) is false)
			{
				Log.Print($"Cannot find stopwatch that named [{name}]", LogPriority.Error);

				return DateTime.MinValue;
			}

			return stopwatches[key].StopwatchLastStopDateTime;
		}
		
		public long? StopwatchLastElapsedTime(string name)
		{
			if (isThisModuleActivated is false)
			{
				return null;
			}
			
			var key = name.GetHashCode();

			if (ValidateStopwatch(key) is false)
			{
				Log.Print($"Cannot find stopwatch that named [{name}]", LogPriority.Error);

				return null;
			}

			return stopwatches[key].LastElapsedTime;
		}
		
		private bool ValidateStopwatch(int k)
		{
			return stopwatches.ContainsKey(k);
		}
	}
}