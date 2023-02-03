using HorangUnityLibrary.Managers.Module;
using HorangUnityLibrary.Modules.StopwatchModule;
using UnityEngine;

public class Tester : MonoBehaviour
{
	private StopwatchModule stopwatchModule;

	private void Start()
	{
		ModuleManager.Instance.RegisterModule(new StopwatchModule(ModuleManager.Instance));
		stopwatchModule = ModuleManager.Instance.GetModule<StopwatchModule>(typeof(StopwatchModule));
		stopwatchModule.ActiveModule();
	}
}