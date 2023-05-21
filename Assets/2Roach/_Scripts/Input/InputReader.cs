using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : ScriptableObject, GameInput.IGameplayActions
{
	
	// Assign delegate{} to events to initialise them with an empty delegate
	// so we can skip the null check when we use them

	// Gameplay
	public event UnityAction InteractStartEvent = delegate { };
	public event UnityAction<bool> InteractEvent = delegate { };
	public event UnityAction InteractCanceledEvent = delegate { };
	public event UnityAction<Vector2> MoveEvent = delegate { };
	public event UnityAction StartedRunning = delegate { };
	public event UnityAction StoppedRunning = delegate { };

	private GameInput _gameInput;

	private void OnEnable()
	{
		if (_gameInput == null)
		{
			_gameInput = new GameInput();
			_gameInput.Gameplay.SetCallbacks(this);
			EnableGameplayInput();
		}
	}

	private void OnDisable()
	{
		DisableAllInput();
	}

	public void OnInteract(InputAction.CallbackContext context)
	{
		InteractEvent.Invoke(context.performed);
		switch (context.phase)
		{
			case InputActionPhase.Started:
				InteractStartEvent.Invoke();
				break;
			case InputActionPhase.Canceled:
				InteractCanceledEvent.Invoke();
				break;
		}
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		MoveEvent.Invoke(context.ReadValue<Vector2>());
	}

	public void OnRun(InputAction.CallbackContext context)
	{
		switch (context.phase)
		{
			case InputActionPhase.Performed:
				StartedRunning.Invoke();
				break;
			case InputActionPhase.Canceled:
				StoppedRunning.Invoke();
				break;
		}
	}

	public void EnableGameplayInput()
	{
		_gameInput.Gameplay.Enable();
	}

    public void DisableAllInput()
	{
		_gameInput.Gameplay.Disable();
	}

	public void OnClick(InputAction.CallbackContext context)
	{

	}

	public void OnSubmit(InputAction.CallbackContext context)
	{

	}

	public void OnPoint(InputAction.CallbackContext context)
	{

	}
	
	public void OnRightClick(InputAction.CallbackContext context)
	{

	}

	public void OnNavigate(InputAction.CallbackContext context)
	{

	}

    public void OnCancel(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public void OnScrollWheel(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public void OnMiddleClick(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public void OnTrackedDevicePosition(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

}