using Horang.HorangUnityLibrary.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tester : MonoBehaviour
{
	private PlayerInput playerInput;
	private InputAction keyboardActions;
	private InputAction mouseActions;

	private void Awake()
	{
		playerInput = GetComponent(typeof(PlayerInput)) as PlayerInput;
	}

	private void Start()
	{
		keyboardActions = playerInput.actions["Keyboard action list"];
		keyboardActions.started += KeyStarted;
		keyboardActions.performed += KeyPerformed;
		keyboardActions.canceled += KeyCanceled;

		mouseActions = playerInput.actions["Mouse action list"];
		mouseActions.performed += MousePerformed;
	}
	
	// Similar as key down
	private void KeyStarted(InputAction.CallbackContext callbackContext)
	{
		Log.Print($"key started: {callbackContext.control.name}");

		switch (callbackContext.control.name)
		{
			case "f1":
				mouseActions.Disable();
				break;
			case "f2":
				mouseActions.Enable();
				break;
		}
	}
	
	// When succeed did interaction
	private static void KeyPerformed(InputAction.CallbackContext callbackContext)
	{
		Log.Print($"key performed: {callbackContext.control.name} / duration: {callbackContext.duration}");
	}
	
	// When failed did interaction (like a time over)
	private static void KeyCanceled(InputAction.CallbackContext callbackContext)
	{
		Log.Print($"key canceled: {callbackContext.control.name} / duration: {callbackContext.duration}");
	}

	private static void MousePerformed(InputAction.CallbackContext callbackContext)
	{
		//Log.Print($"mouse performed: {callbackContext.action.ReadValue<Vector2>()}");
	}
}