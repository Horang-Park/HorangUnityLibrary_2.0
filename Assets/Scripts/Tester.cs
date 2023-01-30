using HorangUnityLibrary;
using HorangUnityLibrary.Modules;
using HorangUnityLibrary.Modules.StopwatchModule;
using UnityEngine;

public class Tester : MonoBehaviour
{
	private ModuleExample moduleExample;
	private StopwatchModule mainTestModule;

	private void Start()
	{
		ModuleManager.Instance.RegisterModule(new ModuleExample(ModuleManager.Instance));
		ModuleManager.Instance.RegisterModule(new StopwatchModule(ModuleManager.Instance));

		moduleExample = ModuleManager.Instance.GetModule<ModuleExample>(typeof(ModuleExample));
		mainTestModule = ModuleManager.Instance.GetModule<StopwatchModule>(typeof(StopwatchModule));
	}

	private async void Update()
	{
		if (Input.GetKeyDown(KeyCode.F1))
		{
			mainTestModule.ActiveModule();
		}
		
		if (Input.GetKeyDown(KeyCode.F2))
		{
			mainTestModule.InactiveModule();
		}
		
		if (Input.GetKeyDown(KeyCode.F3))
		{
			mainTestModule.Start("TEST");
		}
		
		if (Input.GetKeyDown(KeyCode.F4))
		{
			var t = mainTestModule.Stop("TEST");
		}
		
		if (Input.GetKeyDown(KeyCode.F5))
		{
			mainTestModule.Pause("TEST");
		}
		
		if (Input.GetKeyDown(KeyCode.F6))
		{
			mainTestModule.Resume("TEST");
		}

		if (Input.GetKeyDown(KeyCode.F7))
		{
			moduleExample.ActiveModule();
		}
		
		if (Input.GetKeyDown(KeyCode.F8))
		{
			moduleExample.InactiveModule();
		}
		
		if (Input.GetKeyDown(KeyCode.F9))
		{
			moduleExample.DoSomething();
		}

		if (Input.GetKeyDown(KeyCode.F11))
		{
			ModuleManager.Instance.RegisterModule(new StopwatchModule(ModuleManager.Instance));

			mainTestModule = ModuleManager.Instance.GetModule<StopwatchModule>(typeof(StopwatchModule));
		}

		if (Input.GetKeyDown(KeyCode.F12))
		{
			ModuleManager.Instance.UnregisterModule(typeof(StopwatchModule));
		}
	}
}
