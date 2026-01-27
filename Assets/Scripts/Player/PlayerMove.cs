using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
	[Header("Physics")]
	[Range(3, 12)]
	[SerializeField] private float _moveSpeed = 5f;
	[SerializeField] private Rigidbody2D _rigidbody;
	private Vector2 _moveInput;

	public Rigidbody2D Body => _rigidbody;
	public Grid WorldGrid { get; set; }
	public Vector3Int CellPos { get; private set; }

	public void Stop()
	{
		_moveInput = Vector2.zero;
		Body.linearVelocity = Vector2.zero;
	}
	
	private void FixedUpdate()
	{
		if(_moveInput.magnitude > 0f)
			Body.linearVelocity = _moveInput * _moveSpeed;
		
		if(WorldGrid != null)
			CellPos = WorldGrid.WorldToCell(Body.position);
	}

	private void GetMoveInput(InputAction.CallbackContext cb)
	{
		if(cb.canceled) Stop();
		else if(Player.Instance.InputHandled)
		{
			_moveInput = cb.ReadValue<Vector2>();
		}
	}
	private void OnEnable()
	{
		var act = InputSystem.actions.FindAction("Move");
		if(act is not null)
		{
			act.performed += GetMoveInput;
			act.canceled += GetMoveInput;
		}
	}
	private void OnDisable()
	{
		var act = InputSystem.actions.FindAction("Move");
		if(act is not null)
		{
			act.performed -= GetMoveInput;
			act.canceled -= GetMoveInput;
		}
	}
}