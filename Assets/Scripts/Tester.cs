using System;
using Horang.HorangUnityLibrary.Managers.RemoteMethodInterface;
using Horang.HorangUnityLibrary.Utilities;
using Horang.HorangUnityLibrary.Utilities.FiniteStateMachine;
using Horang.HorangUnityLibrary.Utilities.ProceduralSequence.Async;
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

		var s = new StateOne("StateOne");
		sampleFsMachine = new FiniteStateMachine(s, "Sample Finite State Machine");
	}

	// Similar as key down
	private void KeyPerformed(InputAction.CallbackContext callbackContext)
	{
		Log.Print($"key performed: {callbackContext.control.name}");

		State s;

		switch (callbackContext.control.name)
		{
			case "f1":
				s = new StateOne("StateOne");
				sampleFsMachine.ChangeState(s);
				break;
			case "f2":
				s = new StateTwo("StateTwo");
				sampleFsMachine.ChangeState(s);
				break;
			case "f3":
				s = new StateThree("StateThree");
				sampleFsMachine.ChangeState(s);
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
	public override void Enter()
	{
		Log.Print("StateTwo state enter");
	}

	public override void Update()
	{
	}

	public override void Exit()
	{
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