using System;

namespace HorangUnityLibrary.Utilities.CustomAttribute
{
	[AttributeUsage(AttributeTargets.Method)]
	// ReSharper disable once InconsistentNaming
	public class RMI : Attribute
	{
		public RMI()
		{
		}
	}
}

