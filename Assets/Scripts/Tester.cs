using System;
using HorangUnityLibrary;
using HorangUnityLibrary.Modules;
using HorangUnityLibrary.Utilities;
using UnityEngine;

public class Tester : MonoBehaviour
{
	private ModuleExample tes;

	private void Awake()
	{
		Log.Print("로그 유틸리티 테스트");
		var url = @"https://www.naver.com".ToLog();
	}

	private void Start()
	{
		ModuleManager.Instance.RegisterModule(new ModuleExample(ModuleManager.Instance));

		tes = ModuleManager.Instance.GetModule<ModuleExample>(typeof(ModuleExample));
	}

	private async void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			var success = await Log.ExportLogHistory(Application.persistentDataPath + @"/Logs");
		}

		if (Input.GetKeyDown(KeyCode.F1))
		{
			tes.ActiveModule();
			tes.DoSomething();
		}
		
		if (Input.GetKeyDown(KeyCode.F2))
		{
			tes.InactiveModule();
			tes.DoSomething();
		}

		if (Input.GetKeyDown(KeyCode.F3))
		{
			ModuleManager.Instance.UnregisterModule(typeof(ModuleExample));
		}
	}
}
