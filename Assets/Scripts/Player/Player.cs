using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
	[SerializeField] private PlayerData _data;
	static public PlayerData Data => Instance._data;

	[Header("Physics")]
	[Range(3, 12)]
	[SerializeField] private float _moveSpeed = 5f;
	[SerializeField] private Rigidbody2D _rigidbody;

	private bool _inputHandled;
	public bool InputHandled
	{
		get => _inputHandled;
		set
		{
			_inputHandled = value;
			
			_rigidbody.linearVelocity = Vector2.zero;
		}
	}
	public Detectable NowDetected { get; private set; }

	public void Detect(Detectable d)
	{
		if(NowDetected) NowDetected.OnFocusOut.Invoke();
		NowDetected = d;
		if(NowDetected) NowDetected.OnDetect.Invoke();
	}

	private void Move(InputAction.CallbackContext cb)
	{
		if(InputHandled)
			_rigidbody.linearVelocity = cb.ReadValue<Vector2>() * _moveSpeed;
	}
	private void Interact(InputAction.CallbackContext cb)
	{
		if(InputHandled && NowDetected != null)
			NowDetected.OnInteract.Invoke(true);
	}

	private void OnEnable()
	{
		InputAction act;
		act = InputSystem.actions.FindAction("Interact");
		if(act is not null)
		{
			act.started += Interact;
		}

		act = InputSystem.actions.FindAction("Move");
		if(act is not null)
		{
			act.performed += Move;
			act.canceled += Move;
		}
		InputHandled = true;
	}
	private void OnDisable()
	{
		InputAction act;
		act = InputSystem.actions.FindAction("Interact");
		if(act is not null)
		{
			act.started -= Interact;
		}

		act = InputSystem.actions.FindAction("Move");
		if(act is not null)
		{
			act.performed -= Move;
			act.canceled -= Move;
		}
		InputHandled = false;
	}

	#region Singleton
	private static Player _instance = null;
	public static Player Instance
	{
		get
		{
			if(!_instance) _instance = FindAnyObjectByType<Player>();
			return _instance;
		}
	}
	#endregion
}