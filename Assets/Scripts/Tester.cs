using System;
using HorangUnityLibrary;
using HorangUnityLibrary.Modules;
using HorangUnityLibrary.Modules.StopwatchModule;
using HorangUnityLibrary.Utilities;
using HorangUnityLibrary.Utilities.CustomAttribute;
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

		stopwatchModule.ActiveModule();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F1))
		{
			var d = RMIManager.Instance.Run(typeof(Tester), "Bim");
		}
	}

	[RMI]
	private void Bim()
	{
		Log.Print("apsodfjapojpfoj");
	}
}
