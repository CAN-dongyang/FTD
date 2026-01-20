using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
	[Header("Physics")]
	[Range(3, 12)]
	[SerializeField] private float _moveSpeed = 5f;
	[SerializeField] private Rigidbody2D _rigidbody;

	[Header("Properties")]
	[SerializeField] private PlayerData _data;
	static public PlayerData Data => Instance._data;

	[SerializeField] private PlayerUIControl _playerUI;
	public static PlayerUIControl UI => Instance._playerUI;

	private bool _inputHandled;
	public bool InputHandled
	{
		get => _inputHandled;
		set
		{
			_inputHandled = value;
			if(!_inputHandled)
			{
				_moveInput = _rigidbody.linearVelocity = Vector2.zero;
				UI.SelectMenu(PlayerUIControl.MENU.LOCK);
				GameData.Time.Pause();
			}
			///////////////////////////////////////////
			else if(UI.NowMenu == PlayerUIControl.MENU.LOCK)
			{
				UI.SelectMenu(PlayerUIControl.MENU.DEFAULT);
				GameData.Time.Resume();
			}
		}
	}

	public void Detect(Detectable d)
	{
		if(NowDetected) NowDetected.OnFocusOut.Invoke();
		NowDetected = d;
		if(NowDetected) NowDetected.OnDetect.Invoke();
	}
	public Detectable NowDetected { get; private set; }

	public Grid WorldGrid { get; set; }
	public Vector3Int CellPos { get; private set; }

	public void Spawn(Grid grid, Vector3Int cell)
	{
		Spawn(grid.CellToWorld(cell));
	}
	public void Spawn(Vector3 pos)
	{
		_rigidbody.MovePosition(pos);
	}

	private void FixedUpdate()
	{
		if(_moveInput.magnitude > 0f)
			_rigidbody.linearVelocity = _moveInput * _moveSpeed;
		
		if(WorldGrid != null)
			CellPos = WorldGrid.WorldToCell(_rigidbody.position);
	}
	[SerializeField] private Vector2 _moveInput;
	private void Move(InputAction.CallbackContext cb)
	{
		if(InputHandled)
		{
			_moveInput = cb.ReadValue<Vector2>();
			if(cb.canceled) _rigidbody.linearVelocity = Vector2.zero;
		}
	}
	private void Interact(InputAction.CallbackContext cb)
	{
		if(InputHandled && NowDetected != null)
			NowDetected.OnInteract.Invoke();
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