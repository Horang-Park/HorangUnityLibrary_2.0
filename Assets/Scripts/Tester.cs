using System;
using HorangUnityLibrary;
using HorangUnityLibrary.Modules;
using HorangUnityLibrary.Modules.StopwatchModule;
using HorangUnityLibrary.Utilities;
using HorangUnityLibrary.Utilities.CustomAttribute;
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

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F1))
		{
			var d = RmiManager.Instance.Run(typeof(Tester), "Bim");
		}
	}

	[RMI]
	private string Bim()
	{
		Log.Print("apsodfjapojpfoj");

		return "asdfasdfasdf";
	}
}
