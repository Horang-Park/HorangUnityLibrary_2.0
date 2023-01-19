using System;
using HorangUnityLibrary;
using HorangUnityLibrary.Modules;
using HorangUnityLibrary.Utilities;
using UnityEngine;

public class Tester : MonoBehaviour
{
	[SerializeField] private ModuleManager moduleManager;

	private Requester req;

	private void Awake()
	{
		Log.Print("로그 유틸리티 테스트");
		var url = @"https://www.naver.com".ToLog();
	}

	private void Start()
	{
		moduleManager.AddModule(new Requester(moduleManager));

		req = moduleManager.GetModule<Requester>(typeof(Requester));
	}

	private async void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			var success = await Log.ExportLogHistory(Application.persistentDataPath + @"/Logs");
		}

		if (Input.GetKeyDown(KeyCode.F1))
		{
			req.ActiveModule();
			req.DoSomething();
		}
		
		if (Input.GetKeyDown(KeyCode.F2))
		{
			req.InactiveModule();
			req.DoSomething();
		}
	}
}
