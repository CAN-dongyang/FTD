using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
	public GameObject detectedObject;
	public UnityEvent<GameObject> OnDetectEvent = new();

	[Range(3, 12)]
	[SerializeField] private float _moveSpeed = 5f;
	[SerializeField] private Rigidbody2D _rigidbody;

	private void Move(InputAction.CallbackContext cb)
	{
		_rigidbody.linearVelocity = cb.ReadValue<Vector2>() * _moveSpeed;
	}
	private void Interact(InputAction.CallbackContext cb)
	{
		if(detectedObject is not null)
		{
			
		}
	}

	private void OnEnable()
	{
		InputSystem.actions.Enable();

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

		InputSystem.actions.Disable();
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