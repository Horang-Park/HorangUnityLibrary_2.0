using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using Cysharp.Threading.Tasks;
using Horang.HorangUnityLibrary.Managers.Module;
using Horang.HorangUnityLibrary.Managers.Static.AsyncNetworking;
using Horang.HorangUnityLibrary.Managers.Static.AsyncNetworking.Interfaces;
using Horang.HorangUnityLibrary.Modules.AudioModule;
using Horang.HorangUnityLibrary.Utilities;
using Horang.HorangUnityLibrary.Utilities.FiniteStateMachine;
using Horang.HorangUnityLibrary.Utilities.PlayerPrefs;
using Horang.HorangUnityLibrary.Utilities.UnityExtensions;
using UniRx;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Tester : MonoBehaviour
{
	private PlayerInput playerInput;
	private InputAction keyboardActions;
	private InputAction mouseActions;
	private FsmRunner sampleFsMachine;

	public Image colorExpression;

	public float radius;
	public float angle;
	public int step;

	public Text pathText;

	private void Awake()
	{
		playerInput = GetComponent(typeof(PlayerInput)) as PlayerInput;
		
		Log.Print($"{GetPlayerPrefs.String("test")}");
		
		ModuleManager.Instance.RegisterModule(new AudioModule());
	}

	private async void Start()
	{
		SetPlayerPrefs.String("test", "HELLO");
		
		keyboardActions = playerInput.actions["Keyboard action list"];
		keyboardActions.performed += KeyPerformed;

		mouseActions = playerInput.actions["Mouse press action"];
		mouseActions.performed += MousePerformed;

		var s = new StateOne("StateOne");
		sampleFsMachine = new FsmRunner(s, "Sample Finite State Machine");
		
		// var htc = ColorExtension.HexToColor("FF0000");
		// colorExpression.color = htc;
		
		Log.Print($"{ColorExtension.ColorToHex(new Color(1.0f, 0.4f, 0.7f, 1.0f))}");
		
		// req permission
		if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead) is false ||
		    Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite) is false)
		{
			Permission.RequestUserPermission(Permission.ExternalStorageRead);
			Permission.RequestUserPermission(Permission.ExternalStorageWrite);
		}

		var ms = new MemoryStream();
		var req = HttpClientFactory.Create();
		var cal = new Handler();
		var d = new MultipartFormDataContent
		{
			{ new StreamContent(ms) }
		};

		await HttpRequestManager.Get("https://jsonplaceholder.typicode.com/tdodos/3", req, cal);
		await HttpRequestManager.Post("", d, req, cal);
		
		req.Dispose();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			// var r = Random.Range(0.0f, 1.0f);
			// var g = Random.Range(0.0f, 1.0f);
			// var b = Random.Range(0.0f, 1.0f);
			// colorExpression.color = new Color(r, g, b, 1.0f);

			colorExpression.color = colorExpression.color.SetR(1.0f);
			colorExpression.color = colorExpression.color.SetG(0.0f);
			colorExpression.color = colorExpression.color.SetB(0.0f);

			// var t = await Screenshot.ShotWholeScreenAsync();
			//
			// NativeGallery.SaveImageToGallery(t, "Screenshot", t.name, (success, path) =>
			// {	
			// 	Log.Print($"success?: {success} / path: {path}");
			// });
		}

		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			colorExpression.color = colorExpression.color.SetR(0.0f);
			colorExpression.color = colorExpression.color.SetG(1.0f);
			colorExpression.color = colorExpression.color.SetB(0.0f);
		}

		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			colorExpression.color = colorExpression.color.SetR(0.0f);
			colorExpression.color = colorExpression.color.SetG(0.0f);
			colorExpression.color = colorExpression.color.SetB(1.0f);
		}

		transform.Translate(Vector3.down * Time.deltaTime * 10.0f);
	}

	private void OnApplicationQuit()
	{
		UniTask.Void(SaveLogAsync);
	}

	private async UniTaskVoid SaveLogAsync()
	{
		await Log.ExportLogHistory(Application.persistentDataPath);
	}

	private void OnDrawGizmos()
	{
		GizmoExtension.DrawWireFanShape(Color.green, transform.position, transform.forward, radius, angle, step);
		GizmoExtension.DrawWireCircle(Color.blue, transform.position, radius * 1.2f);
	}

	// Similar as key down
	private void KeyPerformed(InputAction.CallbackContext callbackContext)
	{
		Log.Print($"key performed: {callbackContext.control.name}");

		switch (callbackContext.control.name)
		{
			case "f1":
				sampleFsMachine.ChangeState(new StateOne("StateOne"));
				
				//ModuleManager.Instance.GetModule<AudioModule>()!.Play("file_example_OOG_5MG");
				break;
			case "f2":
				sampleFsMachine.ChangeState(new StateTwo("StateTwo"));
				break;
			case "f3":
				sampleFsMachine.ChangeState(new StateThree("StateThree"));
				break;
		}
	}

	private void MousePerformed(InputAction.CallbackContext callbackContext)
	{
		Log.Print($"mouse performed: {callbackContext.control.name} / {callbackContext.phase}");
	}
}

// FSM Samples
public class StateOne : State
{
	public override void Enter()
	{
		Log.Print("StateOne state enter");
	
		UniTask.Void(() => SaveAndLoad.Save(Application.persistentDataPath + "/MY DATA.txt", "English", "This is test text.", WriteMode.New));
		UniTask.Void(() => SaveAndLoad.Save(Application.persistentDataPath + "/MY DATA.txt", "한국어", "이것은 테스트 텍스트입니다."));
		
		SetPlayerPrefs.Bool("TEST_TEST", true);
	}
	
	public override void Update()
	{
	}
	
	public override void Exit()
	{
		Log.Print("StateOne state exit");
	}

	public StateOne(string name) : base(name)
	{
	}
}

public class StateTwo : State
{
	private string data1;
	private string data2;
	
	public override void Enter()
	{
		Log.Print("StateTwo state enter");
		
		Log.Print($"TEST_TEST -> {GetPlayerPrefs.Bool("TEST_TEST")}");
		
		UniTask.Void(GetDatas);
	}

	private async UniTaskVoid GetDatas()
	{
		data1 = await SaveAndLoad.Load(Application.persistentDataPath + "/MY DATA.txt", "English");
		data2 = await SaveAndLoad.Load(Application.persistentDataPath + "/MY DATA.txt", "한국어");
	}

	public override void Update()
	{
	}

	public override void Exit()
	{
		data1.ToLog();
		data2.ToLog();
		
		Log.Print("StateTwo state exit");
	}

	public StateTwo(string name) : base(name)
	{
	}
}

public class StateThree : State
{
	public override void Enter()
	{
		Log.Print("StateThree state enter");
	}

	public override void Update()
	{
	}

	public override void Exit()
	{
		Log.Print("StateThree state exit");
	}

	public StateThree(string name) : base(name)
	{
	}
}

public class Handler : ICallbackHandlerText
{
	public void OnFailure(HttpStatusCode httpStatusCode, string message)
	{
	}

	public void OnDelay()
	{
	}

	public void OnSuccess(string response)
	{
		Log.Print(response);
	}
}