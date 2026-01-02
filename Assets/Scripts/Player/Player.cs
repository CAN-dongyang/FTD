using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
	[SerializeField] private PlayerData _data;
	public PlayerData Data => _data;

	[Header("Physics")]
	[Range(3, 12)]
	[SerializeField] private float _moveSpeed = 5f;
	[SerializeField] private Rigidbody2D _rigidbody;

	[Header("Detective")]
	[SerializeField] private GameObject _detectedObject;
	public GameObject NowDetected => _detectedObject;
	public UnityEvent<GameObject> OnDetectEvent = new();

	public void Detect(GameObject obj)
	{
		OnDetectEvent.Invoke(_detectedObject = obj);
	}

	private void Move(InputAction.CallbackContext cb)
	{
		_rigidbody.linearVelocity = cb.ReadValue<Vector2>() * _moveSpeed;
	}
	private void Interact(InputAction.CallbackContext cb)
	{
		if(NowDetected is not null)
		{
			Debug.Log($"Interact to {NowDetected}");
			
			var schoolUI = FindAnyObjectByType<SchoolUIControl>();
			if(schoolUI) schoolUI.ToggleUI();
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

		OnDetectEvent.AddListener(obj =>
		{
			var schoolUI = FindAnyObjectByType<SchoolUIControl>();
			if(schoolUI) schoolUI.CloseUI();
		});
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

		OnDetectEvent.RemoveAllListeners();
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