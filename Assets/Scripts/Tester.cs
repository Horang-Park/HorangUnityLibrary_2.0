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
	public SpriteRenderer sp;
	
	private StopwatchModule stopwatchModule;

	private void Start()
	{
		ModuleManager.Instance.RegisterModule(new StopwatchModule(ModuleManager.Instance));
		stopwatchModule = ModuleManager.Instance.GetModule<StopwatchModule>(typeof(StopwatchModule));
		stopwatchModule.ActiveModule();

		var r = UnityWebRequestFactory.Get("https://webb.nasa.gov/content/webbLaunch/assets/images/images2023/jan-31-23-potm2301a-4k.jpg", ("authorization", "token"), ("x-device-id", "device"));

		UniTask.Void(() =>
			RequestManager.Send(r,
				onSuccess: ByteToTexture,
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

	private void ByteToTexture(byte[] image)
	{
		var tex = new Texture2D(3840, 2160, TextureFormat.RGBA32, false);
		tex.LoadRawTextureData(image);
		tex.Apply();

		var s = Sprite.Create(tex, new Rect(0.0f, 0.0f, 3840, 2160), Vector2.one * 0.5f);

		sp.sprite = s;
	}
}