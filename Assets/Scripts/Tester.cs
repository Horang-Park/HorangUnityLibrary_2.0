using HorangUnityLibrary.Utilities;
using UnityEngine;

public class Tester : MonoBehaviour
{
	private void Awake()
	{
		Log.Print("로그 유틸리티 테스트");
		var url = @"https://www.naver.com".ToLog();
	}

	private async void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			var success = await Log.ExportLogHistory(Application.persistentDataPath + @"/Logs");
		}
	}
}
