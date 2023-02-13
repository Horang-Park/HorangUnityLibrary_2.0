using Cysharp.Threading.Tasks;
using Horang.HorangUnityLibrary.Utilities;
using Horang.HorangUnityLibrary.Utilities.FiniteStateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tester : MonoBehaviour
{
	private PlayerInput playerInput;
	private InputAction keyboardActions;
	private FiniteStateMachine sampleFsMachine;

	private void Awake()
	{
		playerInput = GetComponent(typeof(PlayerInput)) as PlayerInput;
	}

	private void Start()
	{
		keyboardActions = playerInput.actions["Keyboard action list"];
		keyboardActions.performed += KeyPerformed;

		var s = new StateOne().Create();
		sampleFsMachine = new FiniteStateMachine(s, "Sample Finite State Machine");
	}

	// Similar as key down
	private void KeyPerformed(InputAction.CallbackContext callbackContext)
	{
		Log.Print($"key performed: {callbackContext.control.name}");

		IState s;

		switch (callbackContext.control.name)
		{
			case "f1":
				s = new StateOne().Create();
				sampleFsMachine.ChangeState(s);
				break;
			case "f2":
				s = new StateTwo().Create();
				sampleFsMachine.ChangeState(s);
				break;
			case "f3":
				s = new StateThree().Create();
				sampleFsMachine.ChangeState(s);
				break;
		}
	}
}

// FSM Samples
public struct StateOne : IState
{
	public string Name { get; private set; }
	
	public IState Create()
	{
		Name = nameof(StateOne);
	
		return this;
	}
	
	public void Enter()
	{
		Log.Print("StateOne state enter");
	
		UniTask.Void(() => SaveAndLoad.Save(Application.persistentDataPath + "/MY DATA.txt", "English", "This is test text.", WriteMode.New));
		UniTask.Void(() => SaveAndLoad.Save(Application.persistentDataPath + "/MY DATA.txt", "한국어", "이것은 테스트 텍스트입니다."));
	}
	
	public void Update()
	{
	}
	
	public void Exit()
	{
		Log.Print("StateOne state exit");
	}
}

public struct StateTwo : IState
{
	private string data1;
	private string data2;

	public string Name { get; private set; }

	public IState Create()
	{
		Name = nameof(StateTwo);

		return this;
	}

	public void Enter()
	{
		Log.Print("StateTwo state enter");
		
		UniTask.Void(GetDatas);
	}

	private async UniTaskVoid GetDatas()
	{
		data1 = await SaveAndLoad.Load(Application.persistentDataPath + "/MY DATA.txt", "English");
		data2 = await SaveAndLoad.Load(Application.persistentDataPath + "/MY DATA.txt", "한국어");
	}

	public void Update()
	{
	}

	public void Exit()
	{
		data1.ToLog();
		data2.ToLog();
		
		Log.Print("StateTwo state exit");
	}
}

public struct StateThree : IState
{
	public string Name { get; private set; }

	public IState Create()
	{
		Name = nameof(StateThree);

		return this;
	}

	public void Enter()
	{
		Log.Print("StateThree state enter");
	}

	public void Update()
	{
	}

	public void Exit()
	{
		Log.Print("StateThree state exit");
	}
}