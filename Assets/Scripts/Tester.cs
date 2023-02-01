using Cysharp.Threading.Tasks;
using HorangUnityLibrary.Foundation.Module;
using HorangUnityLibrary.Managers.Module;
using HorangUnityLibrary.Managers.RemoteMethodInterface;
using HorangUnityLibrary.Managers.Static.Networking;
using HorangUnityLibrary.Modules.StopwatchModule;
using HorangUnityLibrary.Utilities;
using UnityEngine;

public class Tester : MonoBehaviour
{
	private StopwatchModule stopwatchModule;

	private void Start()
	{
		ModuleManager.Instance.RegisterModule(new StopwatchModule(ModuleManager.Instance));
		stopwatchModule = ModuleManager.Instance.GetModule<StopwatchModule>(typeof(StopwatchModule));
		stopwatchModule.ActiveModule();

		var r = UnityWebRequestFactory.Get("https://www.naver.com/", ("authorization", "token"), ("x-device-id", "device"));

		UniTask.Void(() =>
			RequestManager.Send(r, data => $"data => {data}".ToLog(),
				onFailure: (code, msg) => $"code: {code}, msg: {msg}".ToLog(),
				onProgress: percent => $"{r.uri.AbsoluteUri} - {percent * 100.0f}%".ToLog()));
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