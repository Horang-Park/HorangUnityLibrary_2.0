using Cysharp.Threading.Tasks;
using Horang.HorangUnityLibrary.Utilities;
using Horang.HorangUnityLibrary.Utilities.FiniteStateMachine;
using Horang.HorangUnityLibrary.Utilities.UnityExtensions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Tester : MonoBehaviour
{
	private PlayerInput playerInput;
	private InputAction keyboardActions;
	private FsmRunner sampleFsMachine;

	public Image colorExpression;

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

		var htc = ColorExtension.HexToColor("8153F5");
		colorExpression.color = htc;
	}

	// Similar as key down
	private void KeyPerformed(InputAction.CallbackContext callbackContext)
	{
		Log.Print($"key performed: {callbackContext.control.name}");

		switch (callbackContext.control.name)
		{
			case "f1":
				FsmRunner.ChangeState(new StateOne("StateOne"));
				break;
			case "f2":
				FsmRunner.ChangeState(new StateTwo("StateTwo"));
				break;
			case "f3":
				FsmRunner.ChangeState(new StateThree("StateThree"));
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