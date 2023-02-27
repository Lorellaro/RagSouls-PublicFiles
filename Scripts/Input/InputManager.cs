using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

[DefaultExecutionOrder(-1)]
public class InputManager : Singleton<InputManager>
{

	public Vector2 movementInput;
	public Vector2 look;
	public float spectatorUpDown;
    public Vector2 spectatorMovementInput;
    private PlayerControls1 _playerControls;
    public bool jumpHeld = false;
	//public bool quitGame = false;

	public delegate void BaseAction();
	public event BaseAction OnStartJump;
	public event BaseAction OnPerformedJump;
	public event BaseAction OnEndJump;
	//public event BaseAction OnStartQuit;
	public event BaseAction OnStartInteract;
	public event BaseAction OnStartLockOn;
	public event BaseAction OnStartAttack;
	public event BaseAction OnStartStrongAttack;
	public event BaseAction OnStartDodge;
	public event BaseAction OnPerformedLook;
	public event BaseAction OnStartReturn;

	private string currentControlInput = "Temp";

	// private Gamepad _gamepad;
	// private Keyboard _keyboard;

	private void Awake()
	{
		_playerControls = new PlayerControls1();
	}

	private void OnEnable()
	{
		AllowInput(true);
		InputUser.onChange += onInputDeviceChange;
		ResetControllerRumble();
	}

	private void OnDisable()
	{
		ResetControllerRumble();
		InputUser.onChange -= onInputDeviceChange;
		AllowInput(false);
	}

	public void AllowInput(bool allowInput)
	{
		if (allowInput)
		{
			_playerControls.Enable();
			// Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		else
		{
			_playerControls.Disable();
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}

	void Start()
	{
		//Player
		_playerControls.PlayerControls.Jump.started += ctx => StartJumpPrimary();
		_playerControls.PlayerControls.Jump.performed += ctx => PerformedJumpPrimary();
		_playerControls.PlayerControls.Jump.canceled += ctx => EndJumpPrimary();
		//_playerControls.PlayerControls.Quit.started += ctx => StartQuitPrimary();
		_playerControls.PlayerControls.Interact.started += ctx => StartInteractPrimary();
		_playerControls.PlayerControls.LockOn.started += ctx => StartLockOnPrimary();
		_playerControls.PlayerControls.Attack.started += ctx => StartAttackPrimary();
		_playerControls.PlayerControls.StrongAttack.started += ctx => StartStrongAttackPrimary();
		_playerControls.PlayerControls.Dodge.started += ctx => StartDodgePrimary();
		_playerControls.PlayerControls.Look.performed += ctx => PerformLookPrimary();
		_playerControls.PlayerControls.Return.started += ctx => StartReturnPrimary();

		/*
		if (Application.platform == RuntimePlatform.WebGLPlayer)
		{
			_playerControls.PlayerControls.Look.ApplyBindingOverride(1, new InputBinding { overrideProcessors = "scaleVector2(x=50,y=50)" });
		}
		*/
	}

	private void onInputDeviceChange(InputUser user, InputUserChange change, InputDevice device)
	{
		if (change == InputUserChange.ControlSchemeChanged)
		{
			if (user.controlScheme != null)
				currentControlInput = user.controlScheme.Value.name;
		}
	}

    private void spectatorMovementPerformed(InputAction.CallbackContext context)
    {
        Vector2 vec = context.ReadValue<Vector2>();
        spectatorMovementInput = vec;
    }

    private void StartSpectatorUpDown()
    {
        spectatorUpDown = _playerControls.SpectatorMode.UpDown.ReadValue<float>();
    }

    private void StartJumpPrimary()
	{
		jumpHeld = true;
		OnStartJump?.Invoke();
	}

	private void PerformedJumpPrimary()
	{
		OnPerformedJump?.Invoke();
	}

	private void EndJumpPrimary()
	{
		jumpHeld = false;
		OnEndJump?.Invoke();
	}

	private void PerformLookPrimary()
	{
		OnPerformedLook?.Invoke();
	}

	/*
	private void StartQuitPrimary()
	{
		OnStartQuit?.Invoke();
	}
	*/

	public void SetControllerRumble(float rumbleStrength)
	{
		if (Gamepad.current != null)
		{
			Gamepad.current.SetMotorSpeeds(rumbleStrength, rumbleStrength);
		}
	}

	public void ResetControllerRumble()
	{
		if (Gamepad.current != null)
			Gamepad.current.ResetHaptics();
	}

	public void StartInteractPrimary()
	{
		//Question mark means check if null before invoke
		//     interactHeld = true;
		OnStartInteract?.Invoke();
		// }
		// public void StopInteractPrimary()
		// {
		//     //Question mark means check if null before invoke
		//     interactHeld = false;
		//     OnStopInteract?.Invoke();
	}

	public void StartLockOnPrimary()
	{
		OnStartLockOn?.Invoke();
	}

	public void StartAttackPrimary()
	{
		OnStartAttack?.Invoke();
	}

	public void StartStrongAttackPrimary()
	{
		OnStartStrongAttack?.Invoke();
	}

	public void StartDodgePrimary()
	{
		OnStartDodge?.Invoke();
	}

	public void StartReturnPrimary()
	{
		OnStartReturn?.Invoke();
	}

	private void Update()
	{
		Gamepad gamepad = Gamepad.current;
		Keyboard keyboard = Keyboard.current;

		if (_playerControls.PlayerControls.Move.activeControl != null)
		{
			currentControlInput = _playerControls.PlayerControls.Move.activeControl.layout;
		}
		if (keyboard != null && currentControlInput.Equals("Key"))
		{
			movementInput = _playerControls.PlayerControls.Move.ReadValue<Vector2>();

		}
		else if (gamepad != null && Application.platform == RuntimePlatform.WebGLPlayer && currentControlInput.Equals("Button"))
		{
			movementInput = _playerControls.PlayerControls.Move.ReadValue<Vector2>();
			movementInput.y *= -1;
		}
		else if (gamepad != null && Application.platform != RuntimePlatform.WebGLPlayer && currentControlInput.Equals("Button"))
		{
			movementInput = _playerControls.PlayerControls.Move.ReadValue<Vector2>();
		}
		else
		{
			movementInput = _playerControls.PlayerControls.Move.ReadValue<Vector2>();
		}

		look = _playerControls.PlayerControls.Look.ReadValue<Vector2>();

        //Spectator Movement
        spectatorMovementInput = _playerControls.PlayerControls.Move.ReadValue<Vector2>();
        spectatorUpDown = _playerControls.SpectatorMode.UpDown.ReadValue<float>();
    }

	private void OnDestroy()
	{
		ResetControllerRumble();
	}

	private void OnApplicationQuit()
	{
		ResetControllerRumble();
	}

	
}
