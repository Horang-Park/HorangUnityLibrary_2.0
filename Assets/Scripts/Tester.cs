using HorangUnityLibrary;
using HorangUnityLibrary.Modules;
using HorangUnityLibrary.Modules.StopwatchModule;
using UnityEngine;

public class Tester : MonoBehaviour
{
	private ModuleExample moduleExample;
	private StopwatchModule stopwatchModule;

	private void Start()
	{
		ModuleManager.Instance.RegisterModule(new ModuleExample(ModuleManager.Instance));
		ModuleManager.Instance.RegisterModule(new StopwatchModule(ModuleManager.Instance));

		moduleExample = ModuleManager.Instance.GetModule<ModuleExample>(typeof(ModuleExample));
		stopwatchModule = ModuleManager.Instance.GetModule<StopwatchModule>(typeof(StopwatchModule));
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F1))
		{
			ModuleManager.Instance.RegisterModule(new ModuleExample(ModuleManager.Instance));
		}
		
		if (Input.GetKeyDown(KeyCode.F2))
		{
			ModuleManager.Instance.UnregisterModule(typeof(ModuleExample));
		}
		
		if (Input.GetKeyDown(KeyCode.F3))
		{
			ModuleManager.Instance.RegisterModule(new StopwatchModule(ModuleManager.Instance));
		}
		
		if (Input.GetKeyDown(KeyCode.F4))
		{
			ModuleManager.Instance.UnregisterModule(typeof(StopwatchModule));
		}
	}
}
