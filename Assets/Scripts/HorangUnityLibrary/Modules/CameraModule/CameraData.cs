using Horang.HorangUnityLibrary.Utilities.CustomAttribute;
using UnityEngine;

namespace Horang.HorangUnityLibrary.Modules.CameraModule
{
	[InspectorHideScriptField]
	public sealed class CameraData : MonoBehaviour
	{
		public Camera Camera { get; private set; }

		private void Awake()
		{
			Camera = GetComponent(typeof(Camera)) as Camera;
		}
	}
}