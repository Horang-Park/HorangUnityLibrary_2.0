using System;
using System.Collections;
using System.IO;
using Cysharp.Threading.Tasks;
using Horang.HorangUnityLibrary.Managers.RemoteMethodInterface;
using Horang.HorangUnityLibrary.Utilities;
using Horang.HorangUnityLibrary.Utilities.FiniteStateMachine;
using Horang.HorangUnityLibrary.Utilities.UnityExtensions;
using Plugins.Android;
using UniRx;
using UnityEditor;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Tester : MonoBehaviour
{
	private PlayerInput playerInput;
	private InputAction keyboardActions;
	private FsmRunner sampleFsMachine;

	public Image colorExpression;

	public float radius;
	public float angle;
	public int step;

	public Text pathText;

	private void Awake()
	{
		playerInput = GetComponent(typeof(PlayerInput)) as PlayerInput;
	}

	private void Start()
	{
		keyboardActions = playerInput.actions["Keyboard action list"];
		keyboardActions.performed += KeyPerformed;

		var s = new StateOne("StateOne");
		sampleFsMachine = new FsmRunner(s, "Sample Finite State Machine");

		var htc = ColorExtension.HexToColor("FF0000");
		colorExpression.color = htc;
		
		// req permission
		if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead) is false ||
		    Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite) is false)
		{
			Permission.RequestUserPermission(Permission.ExternalStorageRead);
			Permission.RequestUserPermission(Permission.ExternalStorageWrite);
		}
	}

	private async void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			var r = Random.Range(0.0f, 1.0f);
			var g = Random.Range(0.0f, 1.0f);
			var b = Random.Range(0.0f, 1.0f);
			colorExpression.color = new Color(r, g, b, 1.0f);

			var t = await Screenshot.ShotWholeScreenAsync();
			
			NativeGallery.SaveImageToGallery(t, "Screenshot", t.name, (success, path) =>
			{	
				Log.Print($"success?: {success} / path: {path}");
			});
		}
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
				break;
			case "f2":
				sampleFsMachine.ChangeState(new StateTwo("StateTwo"));
				break;
			case "f3":
				sampleFsMachine.ChangeState(new StateThree("StateThree"));
				break;
		}
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