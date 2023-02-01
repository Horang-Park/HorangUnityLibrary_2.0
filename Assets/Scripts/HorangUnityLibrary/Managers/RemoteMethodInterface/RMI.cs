using System;

namespace HorangUnityLibrary.Managers.RemoteMethodInterface
{
	/// <summary>
	/// RMI attribute for use Remote Method Interface
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	// ReSharper disable once InconsistentNaming
	public sealed class RMI : Attribute
	{
		public RMI()
		{
		}
	}
}