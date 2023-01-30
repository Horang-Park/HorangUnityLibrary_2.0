using System;
using System.Diagnostics;
using HorangUnityLibrary.Utilities;

namespace HorangUnityLibrary.Modules.StopwatchModule
{
	public class StopwatchElement : Stopwatch
	{
		public string StopwatchName { get; set; }
		public DateTime StopwatchStartDateTime { get; private set; }
		public DateTime StopwatchLastStopDateTime { get; private set; }
		public long LastElapsedTime { get; private set; }
		public new bool IsRunning { get; private set; }
		public bool IsPaused { get; private set; }

		public new void Start()
		{
			base.Start();

			Log.Print($"[{StopwatchName}] is start.");

			StopwatchStartDateTime = DateTime.Now;
			IsRunning = true;
		}

		public new long Stop()
		{
			base.Stop();
			
			Log.Print($"[{StopwatchName}] is stop. - [{ElapsedMilliseconds}]ms");

			var elapsedTime = ElapsedMilliseconds;
			LastElapsedTime = ElapsedMilliseconds;
			StopwatchLastStopDateTime = DateTime.Now;
			IsRunning = false;
			IsPaused = false;

			Reset();
			
			return elapsedTime;
		}

		public void Pause()
		{
			base.Stop();

			LastElapsedTime = ElapsedMilliseconds;
			StopwatchLastStopDateTime = DateTime.Now;
			IsRunning = false;
			IsPaused = true;
		}

		public void Resume()
		{
			base.Start();

			IsRunning = true;
			IsPaused = false;
		}
	}
}